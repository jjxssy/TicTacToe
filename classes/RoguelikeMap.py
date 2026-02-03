import pygame
import random
from Node import Node
from constants import *


class RoguelikeMap:
    def __init__(self, screen_rect: pygame.Rect, seed=None):
        self.rect = screen_rect
        self.rng = random.Random(seed)

        self.nodes_by_floor: list[list[Node]] = []
        self.edges: list[tuple[Node, Node]] = []  # (from, to)

        self.current_floor = 0
        self.selected_node = None

        self._generate_nodes()
        self._generate_edges()
        self._activate_start()

    # ----------- START -----------
    def _activate_start(self):
        # only floor 0 nodes are clickable at the start
        for n in self.nodes_by_floor[0]:
            n.active = True

    # ----------- GENERATION -----------
    def _node_kind_for_floor(self, floor: int) -> str:
        if floor == FLOORS - 1:
            return "boss"
        roll = self.rng.random()
        if roll < 0.60: return "fight"
        if roll < 0.75: return "heal"
        if roll < 0.90: return "shop"
        return "elite"

    # ----------- INPUT -----------
    def click(self, mx: int, my: int):
        """
        Return the chosen node if clicked (only from current floor).
        Otherwise return None.
        """
        # Only allow clicking nodes on CURRENT floor (prevents going back)
        for n in self.nodes_by_floor[self.current_floor]:
            if not n.active or n.visited:
                continue

            # circle hit detection
            if (mx - n.x) ** 2 + (my - n.y) ** 2 <= NODE_RADIUS ** 2:
                self._select_node(n)
                return n

        return None

#    ----------- PROGRESSION -----------
    def _select_node(self, node: Node):
        """
        Mark node as visited and unlock next floor connected nodes.
        """
        node.visited = True
        node.active = False
        self.selected_node = node

        # HARD LOCK: never allow older floors to be active again
        for f in range(node.floor + 1):
            for old in self.nodes_by_floor[f]:
                old.active = False

        # If this was the last floor (boss), stop progression
        if node.floor >= FLOORS - 1:
            return

        # Move to next floor
        self.current_floor = node.floor + 1

        # Lock everything on next floor first
        for n in self.nodes_by_floor[self.current_floor]:
            n.active = False

        # Activate only nodes connected from the selected node
        for a, b in self.edges:
            if a is node:
                b.active = True

    def _generate_nodes(self):
        """
        Create nodes for each floor and store them in self.nodes_by_floor.
        Uses constants:
          FLOORS, NODES_PER_FLOOR, NODE_GAP_X, NODE_GAP_Y, MAP_CENTER_X, MAP_MARGIN_TOP
        """
        self.nodes_by_floor = []

        for f in range(FLOORS):
            # how many nodes in this floor
            n = self.rng.randint(NODES_PER_FLOOR[0], NODES_PER_FLOOR[1])

            # center the row horizontally
            total_width = (n - 1) * NODE_GAP_X
            start_x = MAP_CENTER_X - total_width // 2

            floor_nodes: list[Node] = []
            for c in range(n):
                # small random offsets so it doesn't look like a perfect grid
                jitter_x = self.rng.randint(-18, 18)
                jitter_y = self.rng.randint(-10, 10)

                x = start_x + c * NODE_GAP_X + jitter_x

                # Put floor 0 at the bottom, last floor near the top
                y = MAP_MARGIN_TOP + (FLOORS - 1 - f) * NODE_GAP_Y + jitter_y

                kind = self._node_kind_for_floor(f)

                floor_nodes.append(Node(floor=f, col=c, x=x, y=y, kind=kind))

            self.nodes_by_floor.append(floor_nodes)

    def _generate_edges(self):
        """
        Create forward-only connections between floor f and floor f+1.
        Stores edges as (from_node, to_node) in self.edges.

        Uses constants:
          FLOORS, MAX_EDGES_PER_NODE
        """
        self.edges = []

        if FLOORS <= 1:
            return

        for f in range(FLOORS - 1):
            curr = self.nodes_by_floor[f]
            nxt = self.nodes_by_floor[f + 1]

            # 1) Create edges from each node in curr to 1..MAX_EDGES_PER_NODE nodes in nxt
            for node in curr:
                # Prefer connections to nearby x positions to reduce line chaos
                targets = sorted(nxt, key=lambda t: abs(t.x - node.x))

                # choose among the closest few
                pick_pool = targets[:min(3, len(targets))]

                # how many outgoing edges from this node
                num_edges = self.rng.randint(1, min(MAX_EDGES_PER_NODE, len(pick_pool)))

                chosen = self.rng.sample(pick_pool, k=num_edges)
                for t in chosen:
                    self.edges.append((node, t))

            # 2) Ensure every node in nxt has at least one incoming edge
            for t in nxt:
                has_incoming = any(b is t for (a, b) in self.edges)
                if not has_incoming:
                    closest = min(curr, key=lambda n: abs(n.x - t.x))
                    self.edges.append((closest, t))

    def _kind_color(self, kind: str):
        if kind == "fight": return (90, 160, 255)
        if kind == "elite": return (255, 90, 90)
        if kind == "shop": return (255, 210, 90)
        if kind == "heal": return (120, 255, 170)
        if kind == "boss": return (190, 90, 255)
        return (220, 220, 220)

    def draw(self, screen: pygame.Surface, font: pygame.font.Font):
        # background
        screen.fill((16, 14, 24))

        # title
        title = font.render("Choose your path", True, (240, 240, 240))
        screen.blit(title, (self.rect.centerx - title.get_width() // 2, 24))

        # edges (lines)
        for a, b in self.edges:
            color = (80, 80, 110)
            if self.selected_node is not None and a is self.selected_node:
                color = (200, 200, 255)
            pygame.draw.line(screen, color, (a.x, a.y), (b.x, b.y), 3)

        # nodes (circles)
        for floor_nodes in self.nodes_by_floor:
            for n in floor_nodes:
                base = self._kind_color(n.kind)

                if n.visited:
                    outline = (190, 190, 190)
                    fill = (60, 60, 70)
                elif n.active:
                    outline = (255, 255, 255)
                    fill = base
                else:
                    outline = (100, 100, 120)
                    fill = (40, 40, 50)

                pygame.draw.circle(screen, fill, (n.x, n.y), NODE_RADIUS)
                pygame.draw.circle(screen, outline, (n.x, n.y), NODE_RADIUS, 3)

        # hint
        hint = font.render("Click an available node to continue", True, (190, 190, 210))
        screen.blit(hint, (self.rect.centerx - hint.get_width() // 2, self.rect.bottom - 40))
