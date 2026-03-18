using UnityEngine;
using TMPro;

public class StartingBonusController : MonoBehaviour
{
    public TMP_Text bonusText;
    public GameObject popup;

    void Start()
    {
        UpdateText();
    }

    public void OpenPopup()
    {
        popup.SetActive(true);
    }

    public void SelectBonus(StartingBonus bonus)
    {
        GameplaySettingsManager.Instance.SetStartingBonus(bonus);

        UpdateText();

        popup.SetActive(false);
    }

    void UpdateText()
    {
        switch (GameplaySettingsManager.Instance.startingBonus)
        {
            case StartingBonus.Gold:
                bonusText.text = "+50 Gold";
                break;

            case StartingBonus.RandomRing:
                bonusText.text = "Random Ring";
                break;

            default:
                bonusText.text = "None";
                break;
        }
    }
}