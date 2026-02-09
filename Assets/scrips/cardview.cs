using UnityEngine;
using UnityEngine.UI;
using Sliklak;

public class CardView : MonoBehaviour
{
    public Image artwork;
    public Text cardNameText;

    private Card cardData;

    public void Setup(Card card)
    {
        cardData = card;
        artwork.sprite = card.cardArtwork;
        cardNameText.text = card.cardName;
    }

    public void PlayCard()
    {
        Debug.Log("Played card: " + cardData.cardName);
        Destroy(gameObject); // מסיר מהיד ויזואלית
    }
}
