using UnityEngine;

[CreateAssetMenu(fileName = "NewConfusionEffect", menuName = "Cards/Effects/Confusion")]
public class ConfusionEffect : CardEffect
{
    public float bonusChance = 0.02f; // ה-2% שביקשת

    public override void Execute(EffectContext context)
    {
        Debug.Log("<color=magenta>Confusion!</color> סיכוי ההיפוך עלה ב-2%");
        
        // גישה ל-GameManager דרך הקונטקסט והוספת הסיכוי
        context.Game.reverseChanceBonus += bonusChance;
    }
}