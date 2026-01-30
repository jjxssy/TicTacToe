import pygame
import random

# -------- Positions --------
POSITIONS = {
    7: (100, 50), 8: (325, 50), 9: (550, 50),
    4: (100, 225), 5: (325, 225), 6: (550, 225),
    1: (100, 400), 2: (325, 400), 3: (550, 400)
}

KEY_MAP = {
    pygame.K_KP1: 1, pygame.K_KP2: 2, pygame.K_KP3: 3,
    pygame.K_KP4: 4, pygame.K_KP5: 5, pygame.K_KP6: 6,
    pygame.K_KP7: 7, pygame.K_KP8: 8, pygame.K_KP9: 9
}

# -------- Background paths --------
bg_paths = [
    r"C:\Users\nisay butbul\Desktop\משחק\back1.jpg",
    r"C:\Users\nisay butbul\Desktop\משחק\back21.png",
    r"C:\Users\nisay butbul\Desktop\משחק\back22.png",
    r"C:\Users\nisay butbul\Desktop\משחק\back23.png"
]


# -------- Win check --------
def check_win(moves):
    wins = [
        (1, 2, 3), (4, 5, 6), (7, 8, 9),
        (1, 4, 7), (2, 5, 8), (3, 6, 9),
        (1, 5, 9), (3, 5, 7)
    ]
    return any(all(x in moves for x in combo) for combo in wins)


# -------- AI move --------
def bestMove(available, my_moves, opponent_moves):
    wins = [
        (1, 2, 3), (4, 5, 6), (7, 8, 9),
        (1, 4, 7), (2, 5, 8), (3, 6, 9),
        (1, 5, 9), (3, 5, 7)
    ]
    # Try to win
    for combo in wins:
        needed = [x for x in combo if x not in my_moves]
        if len(needed) == 1 and needed[0] in available:
            return needed[0]
    # Block opponent
    for combo in wins:
        needed = [x for x in combo if x not in opponent_moves]
        if len(needed) == 1 and needed[0] in available:
            return needed[0]
    # Center
    if 5 in available:
        return 5
    # Corners
    corners = [1, 3, 7, 9]
    free = [c for c in corners if c in available]
    if free:
        return random.choice(free)
    # Anything else
    return random.choice(available)


# -------- Sprite manager --------
class SpriteManager:
    def __init__(self, image):
        self.image = image
        self.moves = []
        self.positions = []

    def add_move(self, num):
        self.moves.append(num)
        self.positions.append(POSITIONS[num])

    def draw(self, screen):
        for pos in self.positions:
            screen.blit(self.image, pos)


# -------- Button --------
class Button:
    def __init__(self, x, y, image, text, font, scale_x, scale_y):
        self.original_image = pygame.transform.scale(image, (scale_x, scale_y))
        self.image = self.original_image
        self.font = font
        self.text_str = text

        self.rect = self.image.get_rect(center=(x, y))

        self.text = self.font.render(self.text_str, True, "white")
        self.text_rect = self.text.get_rect(center=self.rect.center)

        self.base_size = (scale_x, scale_y)
        self.hover_size = (scale_x + 10, scale_y + 10)
        self.is_hovered = False

    def update(self, screen):
        screen.blit(self.image, self.rect)
        screen.blit(self.text, self.text_rect)

    def click(self, pos):
        return self.rect.collidepoint(pos)

    def hover(self, pos):
        if self.rect.collidepoint(pos):
            if not self.is_hovered:
                self.image = pygame.transform.scale(
                    self.original_image, self.hover_size
                )
                self.rect = self.image.get_rect(center=self.rect.center)
                self.text_rect = self.text.get_rect(center=self.rect.center)
                self.is_hovered = True
        else:
            if self.is_hovered:
                self.image = pygame.transform.scale(
                    self.original_image, self.base_size
                )
                self.rect = self.image.get_rect(center=self.rect.center)
                self.text_rect = self.text.get_rect(center=self.rect.center)
                self.is_hovered = False







# -------- Run TicTacToe --------
def run_tictactoe(screen, clock, board_path, x_path, o_path, button_path):
    font = pygame.font.Font(r"C:\Users\nisay butbul\Desktop\משחק\assets\Jersey10-Regular.ttf", 40)

    # Load backgrounds
    backgrounds = []
    for path in bg_paths:
        img = pygame.image.load(path).convert_alpha()
        img = pygame.transform.scale(img, (800, 600))
        img.set_alpha(0)
        backgrounds.append(img)

    fade_speed = 2
    current_bg = 0

    # Load board and sprites
    board = pygame.transform.scale(pygame.image.load(board_path), (200, 200))
    x_img = pygame.transform.scale(pygame.image.load(x_path), (150, 150))
    o_img = pygame.transform.scale(pygame.image.load(o_path), (150, 150))
    btn_img = pygame.transform.scale(pygame.image.load(button_path), (150, 150))

    restart_btn = Button(400, 300, btn_img, "Restart", font,150,150)

    available = list(range(1, 10))
    player = SpriteManager(x_img)
    robot = SpriteManager(o_img)
    turn = "PLAYER"
    show_btn = False
    screen.fill(pygame.Color("black"))

    # game loop
    while True:
        clock.tick(60)
        mouse = pygame.mouse.get_pos()

        # -------- Fade backgrounds --------
        if current_bg < len(backgrounds):
            alpha = backgrounds[current_bg].get_alpha()
            if alpha < 255:
                backgrounds[current_bg].set_alpha(alpha + fade_speed)
            else:
                current_bg += 1

        for bg in backgrounds[:current_bg + 1]:
            screen.blit(bg, (0, 0))
        screen.blit(board, (0, 0))

        # -------- Events --------
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                return "EXIT"
            if event.type == pygame.KEYDOWN and event.key == pygame.K_ESCAPE:
                return "MENU"

            if event.type == pygame.MOUSEBUTTONDOWN and show_btn:
                if restart_btn.click(mouse):
                    available = list(range(1, 10))
                    player.moves.clear()
                    player.positions.clear()
                    robot.moves.clear()
                    robot.positions.clear()
                    turn = "PLAYER"
                    show_btn = False

            if event.type == pygame.KEYDOWN and turn == "PLAYER":
                if event.key in KEY_MAP:
                    move = KEY_MAP[event.key]
                    if move in available:
                        player.add_move(move)
                        available.remove(move)

                        if check_win(player.moves) or not available:
                            turn = "END"
                            show_btn = True
                        else:
                            turn = "ROBOT"

        # -------- Robot turn --------
        if turn == "ROBOT" and available:
            pygame.time.delay(300)
            move = bestMove(available, robot.moves, player.moves)
            robot.add_move(move)
            available.remove(move)

            if check_win(robot.moves) or not available:
                turn = "END"
                show_btn = True
            else:
                turn = "PLAYER"

        # -------- Draw --------
        player.draw(screen)
        robot.draw(screen)

        if show_btn:
            restart_btn.hover(mouse)
            restart_btn.update(screen)

        pygame.display.flip()
