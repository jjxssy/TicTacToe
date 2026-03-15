using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance;

    public GameObject tooltipPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;

    private void Awake() => Instance = this;

    private void Start()
    {
        
        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(false);
        }
    }

    public void ShowTooltip(CardData data)
    {
        tooltipPanel.SetActive(true);
        nameText.text = data.cardName;
        
        // כאן אתה יכול להחליט מה להציג - למשל את האפקטים
        descriptionText.text = data.GetTooltipText();
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }

    private void Update()
    {
        // שהחלונית תעקוב אחרי העכבר
        if (tooltipPanel.activeSelf)
        {
            
            Vector2 mousePosition = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
        }
    }
}