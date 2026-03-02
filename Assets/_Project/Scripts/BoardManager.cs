using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance; 
    public Tilemap tilemap;

    [Header("Health")]
    public Health playerHealth;
    public Health enemyHealth;
    

    // המילון כעת שומר את ה-ScriptableObject עצמו כערך
    private Dictionary<Vector2Int, (CardData card, bool isPlayer)> board = 
    new Dictionary<Vector2Int, (CardData card, bool isPlayer)>();
    void Awake() 
    { 
        Instance = this; 
        Debug.Log($"BoardManager Awake - enemyHealth: {enemyHealth}, playerHealth: {playerHealth}");
    }

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
    List<Vector2Int> GetHexNeighbors(Vector2Int cell)
{
    int x = cell.x;
    int y = cell.y;
    
    if (y % 2 == 0)
    {
        return new List<Vector2Int> {
            new Vector2Int(x+1, y),
            new Vector2Int(x-1, y),
            new Vector2Int(x, y+1),
            new Vector2Int(x, y-1),
            new Vector2Int(x-1, y+1),
            new Vector2Int(x-1, y-1)
        };
    }
    else
    {
        return new List<Vector2Int> {
            new Vector2Int(x+1, y),
            new Vector2Int(x-1, y),
            new Vector2Int(x, y+1),
            new Vector2Int(x, y-1),
            new Vector2Int(x+1, y+1),
            new Vector2Int(x+1, y-1)
        };
    }
}

    public void RegisterCard(Vector2Int cell, CardData cardData, bool isPlayer)
    {
        if (board.ContainsKey(cell)) return;
        board[cell] = (cardData, isPlayer);
        Debug.Log($"נרשם קלף בתא {cell}, שחקן: {isPlayer}, סה\"כ קלפים: {board.Count}");
        CheckWholeBoard();
        
    }
    public void CheckWholeBoard()
    {
        List<Vector2Int> cellsToRemove = new List<Vector2Int>();

        foreach (var cell in new List<Vector2Int>(board.Keys))
        {
            if (!board.ContainsKey(cell)) continue;
            bool owner = board[cell].isPlayer;

            List<Vector2Int> neighbors = GetHexNeighbors(cell);
            foreach (var neighbor in neighbors)
            {
                if (GetOwnerAt(neighbor) != owner) continue;

                List<Vector2Int> neighborNeighbors = GetHexNeighbors(neighbor);
                foreach (var third in neighborNeighbors)
                {
                    if (third == cell) continue;
                    if (GetOwnerAt(third) != owner) continue;

                    // מיין את השלושה כדי למנוע כפילויות
                    List<Vector2Int> line = new List<Vector2Int> { cell, neighbor, third };
                    line.Sort((a, b) => a.x != b.x ? a.x.CompareTo(b.x) : a.y.CompareTo(b.y));

                    // בדוק אם כבר מטפלים בקו הזה
                    if (line[0] == line[1] || line[1] == line[2]) continue;
                    if (cellsToRemove.Contains(line[0]) && 
                        cellsToRemove.Contains(line[1]) && 
                        cellsToRemove.Contains(line[2])) continue;

                    Debug.Log(owner ? "Player wins!" : "Computer wins!");
                    
                    int totalDamage = 0;
                    foreach (var c in line)
                        if (board.ContainsKey(c))
                            totalDamage += board[c].card.damage;

                    if (owner)
                    {
                        if (enemyHealth != null) 
                            enemyHealth.TakeDamage(totalDamage);
                        else 
                            Debug.LogError("enemyHealth is null!");
                    }
                    else
                    {
                        if (playerHealth != null) 
                            playerHealth.TakeDamage(totalDamage);
                        else 
                            Debug.LogError("playerHealth is null!");
                    }

                    foreach (var c in line)
                        if (!cellsToRemove.Contains(c))
                            cellsToRemove.Add(c);
                }
            }
        }

        foreach (var cell in cellsToRemove)
        {
            board.Remove(cell);
            RemoveCardVisual(cell);
        }
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

    void RemoveCardVisual(Vector2Int cell)
    {
        Vector3 worldPos = tilemap.GetCellCenterWorld(new Vector3Int(cell.x, cell.y, 0));
        CardDisplay[] allCards = FindObjectsByType<CardDisplay>(FindObjectsSortMode.None);
        foreach (var card in allCards)
        {
            Vector2 cardPos = new Vector2(card.transform.position.x, card.transform.position.y);
            Vector2 cellPos = new Vector2(worldPos.x, worldPos.y);
            if (Vector2.Distance(cardPos, cellPos) < 0.5f)
            {
                Destroy(card.gameObject);
                return;
            }
        }
    }


}