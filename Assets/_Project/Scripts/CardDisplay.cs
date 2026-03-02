using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using System;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class CardDisplay : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [Header("Card Data")]
    public CardData data;

    public SpriteRenderer sr;
    private Transform originalParent;
    
    private Vector3 positionBeforeHover; 
    private Vector3 originalScale;
    private Color originalColor;
    private int originalSortingOrder;

    private bool isDragging = false;
    private bool isPlacedOnMap = false; // האם הקלף כבר הונח?

    [Header("Hover Settings")]
    public float glowPower = 1.5f;
    public float hoverYOffset = 0.5f;
    public float hoverScale = 1.1f;
    public Tilemap targetTilemap; // וודא שזה משויך ב-Inspector
    public bool isPlayerCard = true; 

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        originalSortingOrder = sr.sortingOrder;
        originalScale = transform.localScale;
    }

    void Start()
    {
        if (data != null) LoadCard(data);
        if (targetTilemap == null)
        {
            targetTilemap = UnityEngine.Object.FindFirstObjectByType<Tilemap>();
        }
    }
    public static event Action<CardData, Vector2Int> OnCardPlacedOnBoard; 

    public void LoadCard(CardData newData)
    {
        data = newData;
        if (data != null && data.artwork != null) sr.sprite = data.artwork;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDragging) return;
        if (isPlacedOnMap)
        {
            // הגדלה קטנה לקלפים על הלוח
            transform.localScale = new Vector3(0.65f, 0.65f, 1f);
            sr.sortingOrder = 100;
            return;
        }
    
        // התנהגות רגילה ביד
        positionBeforeHover = transform.localPosition;
        sr.color = new Color(glowPower, glowPower, glowPower, 1f);
        sr.sortingOrder = 100;
        transform.localScale = originalScale * hoverScale;
        transform.localPosition = positionBeforeHover + new Vector3(0, hoverYOffset, 0);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragging) return;
    
        if (isPlacedOnMap)
        {
            // חזרה לגודל המונח
            transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            sr.sortingOrder = originalSortingOrder;
            return;

        }
        ResetVisual();
    }
    private void ResetVisual()
    {
        sr.color = originalColor;
        sr.sortingOrder = originalSortingOrder;
        transform.localScale = originalScale;
        transform.localPosition = positionBeforeHover; 
    }

    public void OnBeginDrag(PointerEventData eventData)
{
    if (isPlacedOnMap || GameManager.Instance.currentState != GameState.PlayerTurn)
    {
        isDragging = false; // וודא שנשאר false
        return; 
    }
        
    isDragging = true;
    originalParent = transform.parent;
    positionBeforeHover = transform.localPosition;
    transform.SetParent(null);
    transform.rotation = Quaternion.identity;
    sr.sortingOrder = 200;
}

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || isPlacedOnMap) return; // בודק isDragging במקום state
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        mouseWorldPos.z = 0f; 
        transform.position = mouseWorldPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging || isPlacedOnMap) return; // בודק isDragging במקום state
        isDragging = false;

        Vector3 checkPos = transform.position;
        checkPos.z = 0;
        Vector3Int cellPos = targetTilemap.WorldToCell(checkPos);

        if (targetTilemap.HasTile(cellPos) && !GameManager.Instance.IsCellOccupied(new Vector2Int(cellPos.x, cellPos.y)))
        {
            OnCardPlacedOnBoard?.Invoke(data, new Vector2Int(cellPos.x, cellPos.y));
            Destroy(gameObject);
        }
        else
        {
            ReturnToHand();
        }
    }



 

    
    private void ReturnToHand()
    {
        transform.SetParent(originalParent);
        ResetVisual();

        if (originalParent != null)
        {
            Hand hand = originalParent.GetComponent<Hand>();
            if (hand != null) hand.UpdateLayout();
        }
    }
    public void SetAsPlaced()
    {
        isPlacedOnMap = true;
        transform.localScale = new Vector3(0.5f, 0.5f, 1f); 
        if (targetTilemap != null)
        {
            transform.SetParent(targetTilemap.transform);
        }
    }
}