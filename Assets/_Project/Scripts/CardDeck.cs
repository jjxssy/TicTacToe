using UnityEngine;
using UnityEngine.EventSystems; // Required for clicking
using System; // Required for Action
using System.Collections.Generic;
using TMPro;

public class Deck : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
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
            OnCardDrawn?.Invoke(drawnData); 
            Debug.Log($"Deck: Broadcasted draw for {drawnData.name}");
        }
    }

    public TextMeshProUGUI deckCountText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        deckCountText.text = $"cards {cards.Count}";
        deckCountText.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        deckCountText.gameObject.SetActive(false);
    }
}