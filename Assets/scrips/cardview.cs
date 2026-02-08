using UnityEngine;
using UnityEngine.UI;
using Sliklak;

public class CardView : MonoBehaviour
{
    public Image artwork;
    public Card cardData;

    public void Setup(Card card)
    {
        cardData = card;
        artwork.sprite = card.cardArtwork;
    }
}
