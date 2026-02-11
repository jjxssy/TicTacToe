using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform handPos;
    
    [Header("Deck Data")]
    [SerializeField] private List<Card> deck;

    void Start()
    {
        
    }

    public void DrawCard()
    {
        if (deck.Count == 0)
        {
            Debug.Log("Empty Deck!");
            return;
        }
        Debug.Log("DrawCard clicked");

        int randomIndex = Random.Range(0, deck.Count);
        Card selectedCard = deck[randomIndex];

        GameObject newCard = Instantiate(cardPrefab, handPos);

        UnityEngine.UI.Image cardImage = newCard.GetComponentInChildren<UnityEngine.UI.Image>();
        if (cardImage != null)
        {
            cardImage.sprite = selectedCard.CardArtwork;
        }

        TextMeshProUGUI cardText = newCard.GetComponentInChildren<TextMeshProUGUI>();

        if (cardText != null)
        {
            cardText.text = selectedCard.CardName;
        }
        

        deck.RemoveAt(randomIndex);
    }
}