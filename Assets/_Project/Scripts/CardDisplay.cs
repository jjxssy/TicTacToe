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

    private SpriteRenderer sr;
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
            targetTilemap = GameObject.FindObjectOfType<Tilemap>();
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
        if (isDragging || isPlacedOnMap) return;
        positionBeforeHover = transform.localPosition;
        sr.color = new Color(glowPower, glowPower, glowPower, 1f);
        sr.sortingOrder = 100;
        transform.localScale = originalScale * hoverScale;
        transform.localPosition = positionBeforeHover + new Vector3(0, hoverYOffset, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragging || isPlacedOnMap) return;
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
        if (isPlacedOnMap) return; // לא גוררים קלף שכבר הונח (אלא אם תרצה אחרת)
        if (isPlacedOnMap || GameManager.Instance.currentState != GameState.PlayerTurn)
    {
        return; 
    }
        
        isDragging = true;
        originalParent = transform.parent;
        
        // שמירת מיקום חזרה ליד
        positionBeforeHover = transform.localPosition;

        
        transform.SetParent(null); // ניתוק מהיד
        transform.rotation = Quaternion.identity;
        sr.sortingOrder = 200;
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(eventData.position);
        mouseWorldPos.z = 0f; 
        transform.position = mouseWorldPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        // בדיקת מיקום ב-Tilemap
        Vector3 checkPos = transform.position;
        checkPos.z = 0; // חשוב לאיפוס עומק
        Vector3Int cellPos = targetTilemap.WorldToCell(checkPos);
        

        if (targetTilemap.HasTile(cellPos))
        {
            OnCardPlacedOnBoard?.Invoke(data, new Vector2Int(cellPos.x, cellPos.y));
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
        if (targetTilemap != null) transform.SetParent(targetTilemap.transform);
        //GetComponent<SpriteRenderer>().color = new Color(1f, 0.8f, 0.8f); 
    }
}