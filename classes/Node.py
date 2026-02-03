from dataclasses import dataclass

@dataclass
class Node:
    floor: int
    col: int
    x: int
    y: int
    kind: str  # "fight", "elite", "shop", "heal", "boss"
    active: bool = False   # clickable now
    visited: bool = False  # already chosen
