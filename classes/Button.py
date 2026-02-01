import pygame

class Button:
    def __init__(self, x, y, image, text, font, scale_x, scale_y,click_sound, hover_sound):
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