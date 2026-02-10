using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [Header("Setup")]
    public GameObject cardPrefab;    // ה-Prefab של הקלף הריק (עם תמונה וטקסט)
    public Transform handPos;       // האובייקט handpos מה-Hierarchy
    
    [Header("Deck Data")]
    public List<Sprite> allCardImages; // רשימת תמונות של כל הקלפים בחפיסה
    private List<Sprite> deck;         // החפיסה הפעילה ממנה נמשוך

    void Start()
    {
        // מעתיקים את רשימת הקלפים לחפיסה הפעילה בתחילת המשחק
        deck = new List<Sprite>(allCardImages);
    }

    public void DrawCard()
    {
        // 1. בדיקה אם נשארו קלפים בחפיסה
        if (deck.Count == 0)
        {
            Debug.Log("החפיסה ריקה!");
            return;
        }
        Debug.Log("DrawCard clicked");

        // 2. הגרלת מספר רנדומלי לפי כמות הקלפים שנשארו
        int randomIndex = Random.Range(0, deck.Count);
        Sprite selectedCard = deck[randomIndex];

        // 3. יצירת הקלף בתוך ה-handpos
        GameObject newCard = Instantiate(cardPrefab, handPos);

        // 4. עדכון התצוגה של הקלף (בהנחה שיש לקלף רכיב Image)
        // הערה: וודא שב-Prefab של הקלף יש רכיב Image
        UnityEngine.UI.Image cardImage = newCard.GetComponentInChildren<UnityEngine.UI.Image>();
        if (cardImage != null)
        {
            cardImage.sprite = selectedCard;
        }

        // 5. הסרת הקלף מהחפיסה כדי שלא יצא שוב
        deck.RemoveAt(randomIndex);
    }
}