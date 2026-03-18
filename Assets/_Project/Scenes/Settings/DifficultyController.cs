using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyController : MonoBehaviour
{
    public Image easyImage;
    public Image normalImage;
    public Image hardImage;
    public Image nightmareImage;

    public TMP_Text descriptionText;

    public Button startingBonusButton;
    public Toggle permadeathToggle;

    public Color selectedColor = new Color(1f, 0.9f, 0.4f);
    public Color normalColor = Color.white;

    void Start()
    {
        SelectDifficulty(GameplaySettingsManager.Instance.difficulty);
    }

public void SelectEasy()
{
    SelectDifficulty(Difficulty.Easy);
}

public void SelectNormal()
{
    SelectDifficulty(Difficulty.Normal);
}

public void SelectHard()
{
    SelectDifficulty(Difficulty.Hard);
}

public void SelectNightmare()
{
    SelectDifficulty(Difficulty.Nightmare);
}

    public void SelectDifficulty(Difficulty difficulty)
    {
        GameplaySettingsManager.Instance.SetDifficulty(difficulty);

        ResetButtonColors();

        switch (difficulty)
        {
            case Difficulty.Easy:

                easyImage.color = selectedColor;
                descriptionText.text = "Enemies are weaker and rewards are higher.";

                startingBonusButton.interactable = true;
                permadeathToggle.interactable = true;

                break;

            case Difficulty.Normal:

                normalImage.color = selectedColor;
                descriptionText.text = "Balanced challenge recommended for new players.";

                startingBonusButton.interactable = true;
                permadeathToggle.interactable = true;

                break;

            case Difficulty.Hard:

                hardImage.color = selectedColor;
                descriptionText.text = "Enemies are stronger and rewards are reduced.";

                startingBonusButton.interactable = true;
                permadeathToggle.interactable = true;

                break;

            case Difficulty.Nightmare:

                nightmareImage.color = selectedColor;
                descriptionText.text = "Brutal mode. Permadeath forced. Starting bonuses disabled.";

                permadeathToggle.isOn = true;

                startingBonusButton.interactable = false;
                permadeathToggle.interactable = false;

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