using UnityEngine;
using Sliklak;

public class GameManager : MonoBehaviour
{
    public Deck deck;

    public void Draw()
    {
        Card card = deck.DrawCard();

        if (card != null)
        {
            Debug.Log("Drew card: " + card.cardName);
        }
    }
}

