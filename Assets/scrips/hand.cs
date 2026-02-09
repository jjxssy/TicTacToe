using System.Collections.Generic;
using UnityEngine;
using Sliklak;

public class Hand : MonoBehaviour
{
    public Deck deck;
    public GameObject cardPrefab;
    public Transform handArea;

    public List<Card> cardsInHand = new();

    public void DrawCard()
    {
        Card card = deck.DrawCard();
        if (card == null) return;

        cardsInHand.Add(card);

        GameObject cardGO = Instantiate(cardPrefab, handArea);
        cardGO.GetComponent<CardView>().Setup(card);
    }

    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            DrawCard();
        }
    }

    //public void RemoveCard(Card card)
    //{
     //   cardsInHand.Remove(card);
   // }
}
