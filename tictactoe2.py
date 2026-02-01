import sys
import pygame
import random

from classes.Button import Button

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
TARGET_RECT = pygame.Rect(550, 50, 150, 150)

# -------- Background paths --------
bg_paths = [
    r"assets\backgrounds\back1.jpg",
    r"assets\backgrounds\back21.png",
    r"assets\backgrounds\back22.png",
    r"assets\backgrounds\back23.png"
]

# -------- sound --------
pygame.mixer.init()
click_sound = pygame.mixer.Sound(r"202314__7778__click-1.mp3")
hover_sound = pygame.mixer.Sound(r"405159__rayolf__btn_hover_2.wav")

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
    def __init__(self, x, y, image, text, font, scale_x, scale_y, click_sound, hover_sound):
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
        self.click_sound = click_sound
        self.hover_sound = hover_sound

    def update(self, screen):
        screen.blit(self.image, self.rect)
        screen.blit(self.text, self.text_rect)

    def click(self, pos):
        if self.rect.collidepoint(pos):
            if self.click_sound:
                self.click_sound.play()
            return True
        return False

    def hover(self, pos):
        if self.rect.collidepoint(pos):
            if not self.is_hovered:
                self.image = pygame.transform.scale(self.original_image, self.hover_size)
                self.rect = self.image.get_rect(center=self.rect.center)
                self.text_rect = self.text.get_rect(center=self.rect.center)
                self.is_hovered = True
                if self.hover_sound:
                    self.hover_sound.play()

        else:
            if self.is_hovered:
                self.image = pygame.transform.scale(self.original_image, self.base_size)
                self.rect = self.image.get_rect(center=self.rect.center)
                self.text_rect = self.text.get_rect(center=self.rect.center)
                self.is_hovered = False


class PlayingCard:
    def __init__(self, x, y, image):
        self.image = image
        self.start_pos = (x, y)
        self.rect = self.image.get_rect(center=(x, y))
        self.dragging = False
        self.offset = (0, 0)

    def update(self, screen):
        if self.dragging:
            mx, my = pygame.mouse.get_pos()
            self.rect.center = (mx - self.offset[0], my - self.offset[1])
        screen.blit(self.image, self.rect)

    def hover(self, pos):
        if self.rect.collidepoint(pos):
            if not self.is_hovered:
                self.image = pygame.transform.scale(self.original_image, self.hover_size)
                self.rect = self.image.get_rect(center=self.rect.center)
                self.text_rect = self.text.get_rect(center=self.rect.center)
                self.is_hovered = True
                if self.hover_sound:
                    self.hover_sound.play()

        else:
            if self.is_hovered:
                self.image = pygame.transform.scale(self.original_image, self.base_size)
                self.rect = self.image.get_rect(center=self.rect.center)
                self.text_rect = self.text.get_rect(center=self.rect.center)
                self.is_hovered = False

    def reset_position(self):
        self.dragging = False
        self.rect.center = self.start_pos


# def pauseMenu():

#     if
#     elif
#     elif
#     elif
#     else

def save(self, x, y, image):
    self.image = image
    self.x = x
    self.y = y


# def option():


def main_menu():
    return "MENU"


def exit():
    pygame.quit()
    sys.exit()


# פונקציית עזר לטשטוש
def get_blurred_screenshot(screen, amount=4):
    screenshot = screen.copy()
    size = screenshot.get_size()
    # הקטנה והגדלה ליצירת אפקט טשטוש (Blur)
    small = pygame.transform.smoothscale(screenshot, (size[0] // amount, size[1] // amount))
    return pygame.transform.smoothscale(small, size)


point1 = 10


# -------- Run TicTacToe --------
def run_tictactoe(screen, clock, board_path, x_path, o_path, button_path):
    font = pygame.font.Font(r"assets/Jersey10-Regular.ttf", 40)

    # Load backgrounds
    backgrounds = []
    for path in bg_paths:
        img = pygame.image.load(path).convert_alpha()
        img = pygame.transform.scale(img, (800, 600))
        img.set_alpha(0)
        backgrounds.append(img)

    fade_speed = 5
    current_bg = 0

    # Load board and sprites
    board = pygame.transform.scale(pygame.image.load(board_path), (200, 200))
    x_img = pygame.transform.scale(pygame.image.load(x_path), (150, 150))
    o_img = pygame.transform.scale(pygame.image.load(o_path), (150, 150))
    btn_img = pygame.transform.scale(pygame.image.load(button_path), (150, 150))

    restart_btn = Button(400, 300, btn_img, "Restart", font, 150, 150, click_sound=click_sound, hover_sound=hover_sound)
    # card1 = PlayingCard(100, 100, x_img, 100, 50)
    exitButton = Button(400, 300, btn_img, "exit", font, 150, 150, click_sound=click_sound, hover_sound=hover_sound)

    available = list(range(1, 10))
    player = SpriteManager(x_img)
    robot = SpriteManager(o_img)
    turn = "PLAYER"
    show_btn = False
    screen.fill(pygame.Color("black"))
    dragged_card = None
    cards = []
    cards.append(PlayingCard(500, 500, x_img))
    show9 = False
    exit_button = False
    pause_overlay = None  # משתנה לשמירת תמונת הטשטוש

    # game loop
    while True:
        clock.tick(60)
        mouse = pygame.mouse.get_pos()

        # אם אנחנו לא בפאוז, נעדכן את הרקע והמשחק
        if not exit_button:
            # -------- Fade backgrounds --------
            if current_bg < len(backgrounds):
                alpha = backgrounds[current_bg].get_alpha()
                if alpha < 255:
                    backgrounds[current_bg].set_alpha(alpha + fade_speed)
                else:
                    current_bg += 1

            for bg in backgrounds[:current_bg + 1]:
                screen.blit(bg, (0, 0))
            screen.blit(board, (200, 200))

            # כאן יבוא הציור של השחקנים והקלפים (player.draw, וכו')
            player.draw(screen)
            robot.draw(screen)
            for card in cards:
                card.update(screen)

        # -------- Events --------
        for event in pygame.event.get():

            if event.type == pygame.QUIT:
                return "EXIT"

            if event.type == pygame.KEYDOWN and event.key == pygame.K_ESCAPE:
                if not exit_button:
                    # on click make screenshot and blur it
                    pause_overlay = get_blurred_screenshot(screen)
                    exit_button = True
                else:
                    exit_button = False

            # ----- MOUSE (cards) -----
            if not exit_button:
                # לוגיקת עכבר רגילה של המשחק כאן
                pass
            else:
                if event.type == pygame.MOUSEBUTTONDOWN:
                    if exitButton.click(event.pos):
                        return "MENU"

        # -------- Drawing Pause Menu --------
        if exit_button:
            if pause_overlay:
                screen.blit(pause_overlay, (0, 0))

            # הוספת שכבת שקיפות כהה מעל הטשטוש
            darken = pygame.Surface((800, 600), pygame.SRCALPHA)
            darken.fill((0, 0, 0, 100))
            screen.blit(darken, (0, 0))

            exitButton.hover(mouse)
            exitButton.update(screen)

        pygame.display.flip()
