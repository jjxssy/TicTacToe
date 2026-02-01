import math
import pygame
from tictactoe2 import Button, run_tictactoe



def main():
    pygame.init()
    pygame.mixer.init()
    screen = pygame.display.set_mode((0, 0), pygame.FULLSCREEN)
    pygame.display.set_caption("Menu")
    clock = pygame.time.Clock()



    font = pygame.font.Font(
        r"C:\Users\nisay butbul\Desktop\משחק\assets\Jersey10-Regular.ttf", 100
    )
    font1 = pygame.font.Font(
        r"C:\Users\nisay butbul\Desktop\משחק\assets\Jersey10-Regular.ttf", 50
    )

    # -------- TITLE --------
    text = "Tic Tac Toe"
    chars = [font.render(c, True, (255, 255, 255)) for c in text]

    letters = []
    x = 175
    for i, c in enumerate(chars):
        letters.append({"surf": c, "x": x, "phase": i * 0.5})
        x += c.get_width() + 4

    # -------- PATHS --------
    BOARD = r"assets/board.png"
    XIMG = r"C:assets/x.png"
    OIMG = r"assets/circle.jpg"
    BTN = r"assets/button.jpg"
    BTN_PLAY = r"assets/play_new.png"
    click = pygame.mixer.Sound(r"202314__7778__click-1.mp3")
    hover_sound = pygame.mixer.Sound(r"405159__rayolf__btn_hover_2.wav")

    bg_paths = [
        r"assets/backgrounds/back1.jpg",
        r"assets/backgrounds/back2.png",
        r"assets/backgrounds/back3.png",
        r"assets/backgrounds/back4.png"
    ]

    backgrounds = []
    for path in bg_paths:
        img = pygame.image.load(path).convert_alpha()
        img = pygame.transform.scale(img, (800, 600))
        img.set_alpha(0)
        backgrounds.append(img)

    bg_surface = pygame.Surface((800, 600)).convert_alpha()

    current_bg = 0
    fade_speed = 4

    # -------- WAVE --------
    base_y = 150
    amplitude = 10
    wave_speed = 0.05
    t = 0

    # -------- BUTTONS --------
    play_img = pygame.transform.scale(
        pygame.image.load(BTN_PLAY), (100, 100)
    )
    exit_img = pygame.image.load(BTN)

    play_btn = Button(400, 300, play_img, "", font, 100, 100, click_sound= click ,hover_sound=hover_sound)
    exit_btn = Button(400, 420, exit_img, "Exit", font1, 100, 100 ,click_sound=click ,hover_sound= hover_sound)

    running = True
    while running:
        clock.tick(60)
        mouse = pygame.mouse.get_pos()
        t += wave_speed

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

                if exit_btn.click(mouse):
                    running = False

        # -------- FADE LOGIC --------
        if current_bg < len(backgrounds):
            alpha = backgrounds[current_bg].get_alpha()
            if alpha < 255:
                backgrounds[current_bg].set_alpha(alpha + fade_speed)
            else:
                current_bg += 1

        bg_surface.fill((0, 0, 0))

        for i in range(current_bg):
            bg_surface.blit(backgrounds[i], (0, 0))

        if current_bg < len(backgrounds):
            bg_surface.blit(backgrounds[current_bg], (0, 0))

        screen.blit(bg_surface, (0, 0))

        # -------- TITLE --------
        for l in letters:
            y = base_y + math.sin(t + l["phase"]) * amplitude
            screen.blit(l["surf"], (l["x"], y))

        # -------- BUTTONS --------
        play_btn.hover(mouse)
        play_btn.update(screen)
        exit_btn.hover(mouse)
        exit_btn.update(screen)
        pygame.display.flip()

    pygame.quit()


if __name__ == "__main__":
    main()
