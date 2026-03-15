using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyController : MonoBehaviour
{
    public Button easyButton;
    public Button normalButton;
    public Button hardButton;
    public Button nightmareButton;

    public Image easyImage;
    public Image normalImage;
    public Image hardImage;
    public Image nightmareImage;

    public TMP_Text descriptionText;

    public Button startingBonusButton;

    public GameObject nightmareLockIcon;

    public Color selectedColor = new Color(1f, 0.9f, 0.4f);
    public Color normalColor = Color.white;

    public Difficulty currentDifficulty = Difficulty.Normal;

    bool nightmareUnlocked;

    void Start()
    {
        nightmareUnlocked = PlayerPrefs.GetInt("NightmareUnlocked", 0) == 1;

        nightmareButton.interactable = nightmareUnlocked;

        if (nightmareLockIcon != null)
            nightmareLockIcon.SetActive(!nightmareUnlocked);

        SelectDifficulty(currentDifficulty);
    }

    public void SelectDifficulty(Difficulty difficulty)
    {
        if (difficulty == Difficulty.Nightmare && !nightmareUnlocked)
            return;

        currentDifficulty = difficulty;

        ResetButtonColors();

        switch (difficulty)
        {
            case Difficulty.Easy:
                easyImage.color = selectedColor;
                descriptionText.text = "Enemies are weaker and rewards are higher.";
                startingBonusButton.interactable = true;
                break;

            case Difficulty.Normal:
                normalImage.color = selectedColor;
                descriptionText.text = "Balanced challenge recommended for new players.";
                startingBonusButton.interactable = true;
                break;

            case Difficulty.Hard:
                hardImage.color = selectedColor;
                descriptionText.text = "Enemies are stronger and rewards are reduced.";
                startingBonusButton.interactable = true;
                break;

            case Difficulty.Nightmare:
                nightmareImage.color = selectedColor;
                descriptionText.text = "Brutal mode. Starting bonuses are disabled.";
                startingBonusButton.interactable = false;
                break;
        }
    }

    void ResetButtonColors()
    {
        easyImage.color = normalColor;
        normalImage.color = normalColor;
        hardImage.color = normalColor;
        nightmareImage.color = normalColor;
    }
}