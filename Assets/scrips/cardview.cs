using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sliklak;

public class CardView : MonoBehaviour
{
    [Header("UI")]
    public Image cardImage;
    public TMP_Text cardNameText;

    private Card cardData;

    public void Setup(Card card)
    {
        cardData = card;

        cardImage.sprite = card.cardArtwork;
        cardNameText.text = card.cardName;
    }

    public Card GetCardData()
    {
        return cardData;
    }
}
