using UnityEngine;

public class Silence : CardEffect
{
    public override void Execute(EffectContext context)
    {
        // כאן נכנסת הלוגיקה של השיתוק
       // GameManager.Instance.isSelectingTarget = true;
        GameManager.Instance.SetState(GameState.Busy);
        
        Debug.Log("Silence effect started - Please select a target.");
    }

}
