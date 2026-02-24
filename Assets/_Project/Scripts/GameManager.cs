
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
        newCard.GetComponent<CardDisplay>().LoadCard(data);
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
        SetState(GameState.PlayerTurn);
        
    }

    public void SpawnCardOnBoard(CardData data, Vector2Int cell)
    {
        Vector3 spawnPos = BoardManager.Instance.tilemap.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));
        spawnPos.z = -0.1f;

        GameObject newCard = Instantiate(cardPrefab, spawnPos, Quaternion.identity);
        CardDisplay display = newCard.GetComponent<CardDisplay>();
        
        display.LoadCard(data);
        display.SetAsPlaced(); 
    }
    



    
}
