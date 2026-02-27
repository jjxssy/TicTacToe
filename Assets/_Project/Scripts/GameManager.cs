
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
    public List<CardData> cpuDeck; 

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
        CardDisplay.OnCardPlacedOnBoard += SpawnCardOnBoard;
    }

    private void OnDisable()
    {
        Deck.OnCardDrawn -= HandleCardDrawn;
        CardDisplay.OnCardPlacedOnBoard -= SpawnCardOnBoard;
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
        CardDisplay display = newCard.GetComponent<CardDisplay>();
        display.LoadCard(data);
        display.isPlayerCard = true;
        


    }
    IEnumerator CPUTurnRoutine()
    {
        
        yield return new WaitForSeconds(1.5f); 

        List<Vector2Int> possibleMoves = BoardManager.Instance.GetEmptyCells();

        if (possibleMoves.Count > 0 && cpuDeck.Count > 0)
        {
            
            Vector2Int targetCell = possibleMoves[Random.Range(0, possibleMoves.Count)];
            CardData cpuCard = cpuDeck[Random.Range(0, cpuDeck.Count)];
            SpawnCardOnBoard(cpuCard, targetCell);
            
        }
        yield return new WaitForSeconds(0.5f);
    }
    public void SpawnCardOnBoard(CardData data, Vector2Int cell)
    {
        Vector3 spawnPos = BoardManager.Instance.tilemap.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));
        spawnPos.z = -0.1f;

        GameObject newCard = Instantiate(cardPrefab, spawnPos, Quaternion.identity);
        CardDisplay display = newCard.GetComponent<CardDisplay>();
        bool isPlayer = (currentState == GameState.PlayerTurn);
        
        display.LoadCard(data);
        display.SetAsPlaced(); 
        if (currentState == GameState.PlayerTurn)
        {
            SetState(GameState.CPUTurn);
            display.isPlayerCard = true;
            BoardManager.Instance.RegisterCard(cell, data, isPlayer);
        }
        else if (currentState == GameState.CPUTurn)
        {
            SetState(GameState.PlayerTurn);
            display.sr.color = new Color(1f, 0.6f, 0.6f, 1f);
            display.isPlayerCard = false;
            BoardManager.Instance.RegisterCard(cell, data, isPlayer);
        }

    }

    internal bool IsCellOccupied(Vector2Int vector2Int)
    {
        if (BoardManager.Instance == null || BoardManager.Instance.tilemap == null)
            return false;

        Vector3 cellCenter = BoardManager.Instance.tilemap.GetCellCenterWorld(new Vector3Int(vector2Int.x, vector2Int.y, 0));
        const float tolerance = 0.1f;
        CardDisplay[] allCards = FindObjectsByType<CardDisplay>(FindObjectsSortMode.None);
        foreach (var card in allCards)
        {
            Vector2 cardPos2D = new Vector2(card.transform.position.x, card.transform.position.y);
            Vector2 cellPos2D = new Vector2(cellCenter.x, cellCenter.y);
            if (Vector2.Distance(cardPos2D, cellPos2D) <= tolerance)
                return true;
        }
        return false;
    }
}
