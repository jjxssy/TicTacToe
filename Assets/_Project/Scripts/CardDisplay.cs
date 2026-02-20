using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

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
        if (targetTilemap == null) Debug.LogError("נא לגרור Tilemap ל-Inspector!");
        // אם השדה ריק (מה שקורה בקלון), נחפש את ה-Tilemap בסצנה
        if (targetTilemap == null)
        {
            targetTilemap = GameObject.FindObjectOfType<Tilemap>();
        }
    }

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
        return; // השחקן לא יכול לגרור
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
            // הצלחה! הקלף נדבק למשושה
            SnapToGrid(cellPos);
        }
        else
        {
            // כישלון - חזרה ליד
            ReturnToHand();
        }
    }

    private void SnapToGrid(Vector3Int cellPos)
    {
        isPlacedOnMap = true;
        
        Vector3 tileWorldPos = targetTilemap.GetCellCenterWorld(cellPos);
        tileWorldPos.z = -0.01f; 
        transform.position = tileWorldPos;

        transform.SetParent(targetTilemap.transform);

        transform.localScale = new Vector3(0.15f, 0.15f, 1f); 
        sr.sortingOrder = originalSortingOrder; 

        // === השורה החדשה שמחברת ל-BoardManager ===
        // אנחנו שולחים את המיקום ואת שם הקלף (מה-CardData)
        if (BoardManager.Instance != null)
        {
            BoardManager.Instance.PlaceCard(transform.position, data.cardName);
        }
        // ==========================================
        GameManager.Instance.SetState(GameState.CPUTurn);
        
        Debug.Log("נצמד למשושה: " + cellPos);
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
        transform.localScale = new Vector3(0.15f, 0.15f, 1f); // הגודל הקטן על הלוח
        if (targetTilemap != null) transform.SetParent(targetTilemap.transform);
        
        // אפשר גם לשנות לו צבע טיפה כדי שנבין שזה של המחשב
        GetComponent<SpriteRenderer>().color = new Color(1f, 0.8f, 0.8f); 
    }
}