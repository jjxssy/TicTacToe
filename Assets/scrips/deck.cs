using System.Collections.Generic;
using UnityEngine;
using Sliklak;

public class Deck : MonoBehaviour
{
    public List<Card> cards = new List<Card>();

    public Card DrawCard()
    {
        if (cards.Count == 0)
        {
            Debug.LogWarning("Deck is empty!");
            return null;
        }

        Card drawnCard = cards[0];   
        cards.RemoveAt(0);          
        return drawnCard;
    }
}
