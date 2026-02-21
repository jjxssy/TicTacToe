using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance; 
    public Tilemap tilemap;

    // המילון כעת שומר את ה-ScriptableObject עצמו כערך
    private Dictionary<Vector2Int, CardData> board = new Dictionary<Vector2Int, CardData>();

    void Awake() { Instance = this; }

    // פונקציה למציאת משבצות פנויות
    public List<Vector2Int> GetEmptyCells()
    {
        List<Vector2Int> emptyCells = new List<Vector2Int>();

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            Vector2Int pos2d = new Vector2Int(pos.x, pos.y);

            if (tilemap.HasTile(pos) && !board.ContainsKey(pos2d))
            {
                emptyCells.Add(pos2d);
            }
        }
        return emptyCells;
    }

    private Vector2Int[] directions = new Vector2Int[]
    {
        new Vector2Int(1, 0),   // ימין
        new Vector2Int(0, 1),   // למעלה-ימין
        new Vector2Int(-1, 1)   // למעלה-שמאל
    };

    public void PlaceCard(Vector2 worldPos, CardData cardData)
    {
        Vector3Int cell3d = tilemap.WorldToCell(worldPos);
        Vector2Int cell = new Vector2Int(cell3d.x, cell3d.y);

        if (board.ContainsKey(cell))
        {
            Debug.Log("התא כבר תפוס!");
            return;
        }

        board[cell] = cardData; 
        Debug.Log($"הונח קלף {cardData.cardName} בתא {cell}");

        CheckWholeBoard();
    }

    void CheckWholeBoard()
    {
        foreach (var cell in board.Keys)
        {
            foreach (Vector2Int dir in directions)
            {
                if (CheckLine(cell, dir))
                {
                    // עכשיו אפשר לגשת לכל נתון בתוך ה-Class
                    Debug.Log("🔥 נמצא 3 בשורה מסוג: " + board[cell].cardName);
                }
            }
        }
    }

    bool CheckLine(Vector2Int start, Vector2Int dir)
    {
        if (!board.ContainsKey(start)) return false;

        CardData type = board[start];
        
        // השוואה בין ה-ScriptableObjects עצמם
        return GetCardAt(start + dir) == type && 
               GetCardAt(start + dir * 2) == type;
    }

    public CardData GetCardAt(Vector2Int cell)
    {
        if (board.ContainsKey(cell)) return board[cell];
        return null;
    }
}