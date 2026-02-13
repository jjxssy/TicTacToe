using UnityEngine;
using UnityEngine.EventSystems; // Required for clicking
using System; // Required for Action
using System.Collections.Generic;

public class Deck : MonoBehaviour, IPointerClickHandler
{
    public List<CardData> cards = new List<CardData>();

    // The Event: "Hey everyone, a card was drawn, and here is its data!"
    public static event Action<CardData> OnCardDrawn;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (cards.Count > 0)
        {
            CardData drawnData = cards[0];
            cards.RemoveAt(0);

            // Trigger the event (The ?. ensures it doesn't crash if nothing is listening)
            OnCardDrawn?.Invoke(drawnData);
            
            Debug.Log($"Deck: Broadcasted draw for {drawnData.name}");
        }
    }
}