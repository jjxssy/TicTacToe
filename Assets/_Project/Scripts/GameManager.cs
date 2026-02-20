using System.Collections; // חשוב בשביל IEnumerator
using System.Collections.Generic;
using UnityEngine;

public enum GameState { PlayerTurn, CPUTurn, Busy, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Setup")]
    public GameObject cardPrefab;
    public Transform handTransform;

    [Header("Turn Management")]
    public GameState currentState;

    [Header("CPU Settings")]
    public List<CardData> cpuDeck; // רשימת קלפים שהמחשב יכול להשתמש בהם

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetState(GameState.PlayerTurn);
    }

    private void OnEnable()
    {
        Deck.OnCardDrawn += HandleCardDrawn;
    }

    private void OnDisable()
    {
        Deck.OnCardDrawn -= HandleCardDrawn;
    }

    public void SetState(GameState newState)
    {
        currentState = newState;
        Debug.Log("Current State: " + currentState);

        if (currentState == GameState.CPUTurn)
        {
            StartCoroutine(CPUTurnRoutine());
        }
    }

    private void HandleCardDrawn(CardData data)
    {
        Debug.Log("GameManager: Received draw event. Spawning card...");
        GameObject newCard = Instantiate(cardPrefab, handTransform);
        newCard.GetComponent<CardDisplay>().LoadCard(data);
    }

    // גרסה אחת ומאוחדת של הלוגיקה
    IEnumerator CPUTurnRoutine()
    {
        yield return new WaitForSeconds(1.5f); // "זמן חשיבה"

        // 1. מציאת משבצות פנויות דרך ה-BoardManager
        List<Vector3Int> possibleMoves = BoardManager.Instance.GetEmptyCells();

        if (possibleMoves.Count > 0 && cpuDeck.Count > 0)
        {
            // 2. בחירת מהלך אקראי
            Vector3Int targetCell = possibleMoves[Random.Range(0, possibleMoves.Count)];
            
            // 3. בחירת קלף אקראי מהדק של המחשב
            CardData cpuCard = cpuDeck[Random.Range(0, cpuDeck.Count)];

            // 4. יצירת הקלף ויזואלית על הלוח
            SpawnCPUCard(targetCell, cpuCard);
            
            // 5. רישום ב-BoardManager לבדיקת 3 בשורה
            BoardManager.Instance.PlaceCard(BoardManager.Instance.tilemap.GetCellCenterWorld(targetCell), cpuCard.cardName);
        }

        yield return new WaitForSeconds(0.5f);
        SetState(GameState.PlayerTurn);
    }

    void SpawnCPUCard(Vector3Int cell, CardData data)
    {
        Vector3 spawnPos = BoardManager.Instance.tilemap.GetCellCenterWorld(cell);
        spawnPos.z = -0.01f;

        GameObject newCard = Instantiate(cardPrefab, spawnPos, Quaternion.identity);
        CardDisplay display = newCard.GetComponent<CardDisplay>();
        
        display.LoadCard(data);
        
        // הגדרה שהקלף הונח (כדי שהשחקן לא יזיז אותו)
        display.SetAsPlaced(); 
    }
}