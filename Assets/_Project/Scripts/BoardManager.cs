using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance; // סינגלטון כדי שיהיה קל לגשת מכל מקום
    public Tilemap tilemap;

    // עדיף לשמור ID או שם של קלף כדי שהבדיקה תהיה לפי "סוג" ולא לפי אובייקט
    private Dictionary<Vector3Int, string> board = new Dictionary<Vector3Int, string>();
    public List<Vector3Int> GetEmptyCells()
    {
        List<Vector3Int> emptyCells = new List<Vector3Int>();

        // cellBounds.allPositionsWithin עובר על כל הריבועים בטווח של ה-Tilemap
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            // אנחנו בודקים: 
            // 1. האם יש משושה מצויר במיקום הזה?
            // 2. האם המילון (board) לא מכיל כבר קלף במיקום הזה?
            if (tilemap.HasTile(pos) && !board.ContainsKey(pos))
            {
                emptyCells.Add(pos);
            }
        }
        return emptyCells;
    }

    // כיוונים למשושים (Pointy Top)
    private Vector3Int[] directions = new Vector3Int[]
    {
        new Vector3Int(1, 0, 0),  // ימין
        new Vector3Int(0, 1, 0),  // למעלה-ימין (במערכת של Unity Hex)
        new Vector3Int(-1, 1, 0)  // למעלה-שמאל
    };

    void Awake() { Instance = this; }

    public void PlaceCard(Vector3 worldPos, string cardName)
    {
        Vector3Int cell = tilemap.WorldToCell(worldPos);

        if (board.ContainsKey(cell))
        {
            Debug.Log("התא כבר תפוס!");
            return;
        }

        board[cell] = cardName; // שומרים את שם הקלף במיקום הזה
        Debug.Log($"הונח קלף {cardName} בתא {cell}");

        CheckWholeBoard();
    }

    void CheckWholeBoard()
    {
        foreach (var cell in board.Keys)
        {
            foreach (Vector3Int dir in directions)
            {
                if (CheckLine(cell, dir))
                {
                    Debug.Log("🔥 נמצא 3 בשורה מסוג: " + board[cell]);
                }
            }
        }
    }

    bool CheckLine(Vector3Int start, Vector3Int dir)
    {
        string type = board[start];
        // בודקים אם שני התאים הבאים ברצף מכילים את אותו סוג קלף
        return GetCardTypeAt(start + dir) == type && 
               GetCardTypeAt(start + dir * 2) == type;
    }

    string GetCardTypeAt(Vector3Int cell)
    {
        if (board.ContainsKey(cell)) return board[cell];
        return null;
    }

}