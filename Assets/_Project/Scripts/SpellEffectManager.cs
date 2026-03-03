using UnityEngine;
using UnityEngine.EventSystems;

public class Efact : MonoBehaviour , IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Card dropped on effect area!");
        CardDisplay card = eventData.pointerDrag.GetComponent<CardDisplay>();
        if (card != null)
        {
            return;
        }
        Destroy(card.gameObject);
    }

}
