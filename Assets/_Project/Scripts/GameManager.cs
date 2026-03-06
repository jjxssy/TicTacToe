
using System.Collections; // חשוב בשביל IEnumerator
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum GameState { PlayerTurn, CPUTurn, Busy, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Deck playerDeck;

    public TurnTimer turnTimer;
    public bool isItPlayerTurn;

    [HideInInspector] public float damageMultiplier = 1f;

    [Header("Setup")]
    public GameObject cardPrefab;
    public Transform handTransform;

    [Header("Turn Management")]
    public GameState currentState;

    [Header("UI References")]
    public TextMeshProUGUI turnCountText;    
    public TextMeshProUGUI turnIndicatorText;

    [Header("CPU Settings")]
    public List<CardData> cpuDeck; 

    [Header("Mechanics")]
    public float reverseChanceBonus = 0f; 

    [Header("Mana Settings")]
    public int currentMana = 3;
    public const int MAX_MANA = 3;
    [SerializeField] private TextMeshProUGUI manaText;

    private int TurnCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetState(GameState.PlayerTurn);
        for (int i = 0; i < 5; i++)
        {
            if (playerDeck != null && playerDeck.cards.Count > 0)
            {
                int randomIndex = Random.Range(0, playerDeck.cards.Count);
                CardData drawnCard = playerDeck.cards[randomIndex];
                playerDeck.cards.RemoveAt(randomIndex);
                HandleCardDrawn(drawnCard);
            }
        }
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
        //Debug.Log("Current State: " + currentState);
        isItPlayerTurn = (currentState == GameState.PlayerTurn);

        if (currentState == GameState.PlayerTurn)
        {
            turnTimer?.StartTimer();
            Debug.Log("Player's turn started. Timer started.");
            TurnCount++;
            Debug.Log("Turn Count: " + TurnCount);
            currentMana = MAX_MANA;
            UpdateVisualUI();
           
        }
        else
        {
            turnTimer?.StopTimer(); // עוצר בתור CPU
        }
        UpdateVisualUI();

        if (currentState == GameState.CPUTurn)
        {
            StartCoroutine(CPUTurnRoutine());
            Debug.Log("CPU's turn started. Executing CPU actions...");
        }

    }
    public void EndTurn()
    {
        if (currentState != GameState.PlayerTurn) return; 
        if (playerDeck != null && playerDeck.cards.Count > 0)
        {
            int randomIndex = Random.Range(0, playerDeck.cards.Count);
            CardData drawnCard = playerDeck.cards[randomIndex];
            playerDeck.cards.RemoveAt(randomIndex);
            HandleCardDrawn(drawnCard);
        }
        SetState(GameState.CPUTurn);
    }
    private void HandleCardDrawn(CardData data)
    {

        int cardsInHand = handTransform.childCount;

        if (cardsInHand >= 8)
        {
            Debug.Log("היד מלאה! הקלף נהרס.");
            return; 
        }
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

    public void ApplyCardEffects(CardData data, GameObject cardObject)
    {
    EffectContext context = new EffectContext();
    context.Card = data;
    bool canBeReversed = data.powerDowns != null && data.powerDowns.Count > 0;
    bool isReversed = false;

    if (canBeReversed)
    {
        isReversed = UnityEngine.Random.Range(0f, 1f) <= 0.5f;
        // הסיכוי הבסיסי הוא 0.5 (50%), ונוסיף לו את הבונוס מהבלבול
        float finalReverseThreshold = 0.5f + reverseChanceBonus;
        finalReverseThreshold = Mathf.Clamp(finalReverseThreshold, 0f, 1f);

        isReversed = UnityEngine.Random.Range(0f, 1f) <= finalReverseThreshold;

        Debug.Log($"Card: {data.cardName} | Can be Reversed: {canBeReversed} | Reverse Chance: {finalReverseThreshold * 100}% | Is Reversed: {isReversed}");
    }

    context.IsReversed = isReversed;

    if (isReversed)
    {
        cardObject.transform.rotation = Quaternion.Euler(0, 0, 180);
        Debug.Log("reversed card! Applying powerDowns.");
        
    }
        // דוגמה ללוגיקה בתוך ApplyCardEffects
    if (!isReversed)
    {
        foreach (var effect in data.powerUps)
        {
            if (effect != null) 
            {
                effect.Execute(context);
                Debug.Log("Executing PowerUp: " + effect.name); // בדיקה ב-Console
            }
        }
    }
    else
    {
        foreach (var effect in data.powerDowns)
        {
            if (effect != null) effect.Execute(context);
            Debug.Log("applied powerDown effect.");
        }
    }
    }

    public bool CanAffordCard(int cost)
    {
        return currentMana >= cost;
    }

    public void SpendMana(int amount)
    {
        currentMana -= amount;
        UpdateVisualUI();
    }
    private void UpdateVisualUI()
    {
        if (manaText != null) manaText.text = "Mana " + currentMana + "/3";

        if (turnCountText != null)
        {
            turnCountText.text = $"Turn : {TurnCount}";
        }
        if (turnIndicatorText != null)
        {
            if (isItPlayerTurn)
            {
                turnIndicatorText.text = "Your Turn";
                turnIndicatorText.color = Color.green;
            }
            else
            {
                turnIndicatorText.text = "Opponent's Turn";
                turnIndicatorText.color = Color.red;
            }
        }
    }
}

