import math
import pygame
from tictactoe2 import Button, run_tictactoe


def main():
    pygame.init()
    screen = pygame.display.set_mode((800, 600))
    pygame.display.set_caption("Menu")
    clock = pygame.time.Clock()
    font = pygame.font.Font(r"C:\Users\nisay butbul\Desktop\משחק\assets\Jersey10-Regular.ttf", 100)

    # -------- Animated title --------
    text = "Tic Tac Toe"
    chars = [font.render(c, True, (255, 255, 255)) for c in text]

    letters = []
    x3 = 175
    for i, c in enumerate(chars):
        letters.append({
            "s": c,
            "x": x3,
            "phase": i * 0.5
        })
        x3 += c.get_width() + 4

    # -------- Paths --------
    BOARD = r"C:\Users\nisay butbul\Desktop\משחק\board.png"
    XIMG = r"C:\Users\nisay butbul\Desktop\משחק\x.png"
    OIMG = r"C:\Users\nisay butbul\Desktop\משחק\circle.jpg"
    BTN = r"C:\Users\nisay butbul\Desktop\משחק\button.jpg"
    BTN_play = r"C:\Users\nisay butbul\Desktop\משחק\play_new.png"

    # -------- Backgrounds --------
    bg_paths = [
        r"C:\Users\nisay butbul\Desktop\משחק\back1.jpg",
        r"C:\Users\nisay butbul\Desktop\משחק\back2.png",
        r"C:\Users\nisay butbul\Desktop\משחק\back3.png",
        r"C:\Users\nisay butbul\Desktop\משחק\back4.png"
    ]

    backgrounds = []
    for path in bg_paths:
        img = pygame.image.load(path).convert_alpha()
        img = pygame.transform.scale(img, (800, 600))
        img.set_alpha(0)
        backgrounds.append(img)

    current_bg = 0
    fade_speed = 4

    # -------- Smooth wave settings --------
    base_y = 150
    amplitude = 10  # כמה האות זזה למעלה/למטה
    wave_speed = 0.03  # קטן = איטי וחלק
    time = 0

    # -------- Button --------
    btn_img = pygame.image.load(BTN_play)
    btn_img = pygame.transform.scale(btn_img, (100, 100))
    play_btn = Button(400, 300, btn_img, "", font,100,100)

    running = True
    while running:
        clock.tick(60)
        mouse = pygame.mouse.get_pos()
        time += wave_speed

        # -------- Fade backgrounds --------
        if current_bg < len(backgrounds):
            alpha = backgrounds[current_bg].get_alpha()
            if alpha < 255:
                backgrounds[current_bg].set_alpha(alpha + fade_speed)
            else:
                current_bg += 1

            for bg in backgrounds[:current_bg + 1]:
                screen.blit(bg, (0, 0))

        # -------- Events --------
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                running = False

            if event.type == pygame.MOUSEBUTTONDOWN:
                if play_btn.click(mouse):
                    result = run_tictactoe(
                        screen, clock,
                        BOARD, XIMG, OIMG, BTN
                    )
                    if result == "EXIT":
                        running = False

        # -------- Smooth animated letters --------
        for l in letters:
            y = base_y + math.sin(time + l["phase"]) * amplitude
            screen.blit(l["s"], (l["x"], y))

        # -------- Button --------
        play_btn.hover(mouse)
        play_btn.update(screen)

        pygame.display.flip()

    pygame.quit()


if __name__ == "__main__":
    main()
