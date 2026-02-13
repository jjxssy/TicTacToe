using UnityEngine;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CardData data; // Drag an SO here for testing, or push it via script
    private SpriteRenderer sr;

    private Color originalColor;
    private int originalSortingOrder;

    [Header("Glow Intensity")]
    public float glowPower = 0.5f; // Values > 1 trigger the Bloom

    [Header("Hover Settings")]
    public float hoverYOffset = 0.5f;
    private Vector3 originalPos;

    void Start() {
        if (data != null) LoadCard(data);   
    }

    public void LoadCard(CardData newData) {
        data = newData;
        sr.sprite = data.artwork;
        // Update text meshes, etc.
    }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
        originalSortingOrder = sr.sortingOrder;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 1. Overdrive the color values to trigger Bloom
        // This makes the sprite 'Super White'
        sr.color = new Color(glowPower, glowPower, glowPower, 1f);

        // 2. Bring card to front so it doesn't glow 'behind' other cards
        sr.sortingOrder = 100; 
        
        // 3. Optional: Subtle scale up
        transform.localScale = Vector3.one * 1.05f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset everything
        sr.color = originalColor;
        sr.sortingOrder = originalSortingOrder;
        transform.localScale = Vector3.one;
    }
}
