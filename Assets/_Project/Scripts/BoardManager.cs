using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance; 
    public Tilemap tilemap;

    // המילון כעת שומר את ה-ScriptableObject עצמו כערך
    private Dictionary<Vector2Int, (CardData card, bool isPlayer)> board = 
    new Dictionary<Vector2Int, (CardData card, bool isPlayer)>();

    void Awake() { Instance = this; }

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
        new Vector2Int(0, 1),   // למעלה
        new Vector2Int(1, 1),   // אלכסון ימין-למעלה
        new Vector2Int(1, -1),  // אלכסון ימין-למטה
        new Vector2Int(-1, 1),  // אלכסון שמאל-למעלה
        new Vector2Int(-1, -1)  // אלכסון שמאל-למטה
    };

    public void RegisterCard(Vector2Int cell, CardData cardData, bool isPlayer)
    {
        if (board.ContainsKey(cell)) return;
        board[cell] = (cardData, isPlayer);
        Debug.Log($"נרשם קלף בתא {cell}, שחקן: {isPlayer}, סה\"כ קלפים: {board.Count}");
        CheckWholeBoard();
        
    }

    public void CheckWholeBoard()
    {
        foreach (var cell in board.Keys)
        {
            foreach (Vector2Int dir in directions)
            {
                if (CheckLine(cell, dir))
                {
                    bool isPlayer = board[cell].isPlayer;
                    Debug.Log(isPlayer ? "Player wins!" : "Computer wins!");
                }
            }
        }

    }
        bool CheckLine(Vector2Int start, Vector2Int dir)
    {
        if (!board.ContainsKey(start)) return false;

        bool owner = board[start].isPlayer;

        if (GetOwnerAt(start + dir) == owner && GetOwnerAt(start + dir * 2) == owner)
            return true;

        if (GetOwnerAt(start - dir) == owner && GetOwnerAt(start - dir * 2) == owner)
            return true;

        if (GetOwnerAt(start - dir) == owner && GetOwnerAt(start + dir) == owner)
            return true;

        return false;
    }

    public bool? GetOwnerAt(Vector2Int cell)
    {
        if (board.ContainsKey(cell)) return board[cell].isPlayer;
        return null;
    }
    public CardData GetCardAt(Vector2Int cell)
    {
        if (board.ContainsKey(cell)) return board[cell].card;
        return null;
    }
}