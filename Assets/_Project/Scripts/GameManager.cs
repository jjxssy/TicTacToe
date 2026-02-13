using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Setup")]
    public GameObject cardPrefab;
    public Transform handTransform;

    private void OnEnable()
    {
        // Start listening to the Deck's broadcast
        Deck.OnCardDrawn += HandleCardDrawn;
    }

    private void OnDisable()
    {
        // Stop listening (Always clean up your events!)
        Deck.OnCardDrawn -= HandleCardDrawn;
    }

    // This method runs automatically whenever the Deck invokes OnCardDrawn
    private void HandleCardDrawn(CardData data)
    {
        Debug.Log("GameManager: Received draw event. Spawning card...");

        GameObject newCard = Instantiate(cardPrefab, handTransform);
        newCard.GetComponent<CardDisplay>().LoadCard(data);
        
        // The Hand layout script will handle the positioning automatically
    }
}