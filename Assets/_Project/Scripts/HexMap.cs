using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.InputSystem;

public class HexClickDetector : MonoBehaviour
{
    public Tilemap tilemap;
    public Camera mainCamera;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // 1. חישוב מיקום הלחיצה בעולם
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = -mainCamera.transform.position.z; 
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);

            // 2. המרה למיקום תא ברשת
            Vector3Int cellPos = tilemap.WorldToCell(worldPos);

            // 3. שליפת ה-Asset (ה-Tile) מהמיקום הזה
            TileBase clickedTile = tilemap.GetTile(cellPos);

            // 4. הבדיקה החשובה: האם יש שם אסט?
            if (clickedTile != null)
            {
                // כאן הקוד ירוץ רק אם לחצת על משושה קיים
                Debug.Log($"לחצת על משושה! שם האסט: {clickedTile.name} במיקום: {cellPos}");
                
                // כאן תוכל להוסיף לוגיקה משלך, למשל:
                // DoSomethingWithHex(cellPos);
            }
            else
            {
                // כאן הקוד ירוץ אם לחצת על שטח ריק בתוך ה-Grid
                Debug.Log("לחצת על שטח ריק (אין כאן אסט)");
            }
        }
    }
}