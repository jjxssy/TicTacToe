using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class Hand : MonoBehaviour
{

    [Header("Layout Settings")]
    [Range(1f, 10f)] public float maxWidth = 5f;
    [Range(0f, 45f)] public float maxRotation = 15f;
    [Range(0f, 2f)] public float arcHeight = 0.5f; // Makes it look more like a "hand"
    
    public void AddCard(GameObject cardPrefab)
    {
        GameObject newCard = Instantiate(cardPrefab, transform);
        UpdateLayout();
    }

    private void Update()
    {
        // For testing/placeholder purposes, we update every frame in editor
        UpdateLayout();
    }

    public void UpdateLayout()
    {
        int childCount = transform.childCount;
        if (childCount == 0) return;

        // Calculate spacing
        float spacing = maxWidth / (childCount > 1 ? childCount - 1 : 1);
        if (childCount == 1) spacing = 0;

        float startX = -((childCount - 1) * spacing) / 2f;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            
            // 1. Calculate X Position
            float xPos = startX + (i * spacing);

            // 2. Calculate Rotation and Arc (The "Fan" effect)
            // Normalized value: -1 for leftmost, 0 for middle, 1 for rightmost
            float normalizedIndex = (childCount > 1) ? (2f * i / (childCount - 1) - 1f) : 0f;

            float rotationZ = -normalizedIndex * maxRotation;
            float yOffset = -Mathf.Abs(normalizedIndex) * arcHeight;

            // Apply values
            child.localPosition = new Vector3(xPos, yOffset, 0f);
            child.localRotation = Quaternion.Euler(0, 0, rotationZ);
            
            // Optional: Order in Layer
            // Ensure cards on the right are drawn on top of cards on the left
            var sr = child.GetComponent<SpriteRenderer>();
            if (sr != null) sr.sortingOrder = i;
        }
    }
}
