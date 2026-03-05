using UnityEngine;

// השורה הזו מאפשרת לך ליצור את האפקט כקובץ בתיקיית ה-Assets
[CreateAssetMenu(fileName = "NewDoubleDamage", menuName = "Cards/Effects/Combat/Double Damage")]
public class DoubleDamageEffect : CardEffect
{
    public float multiplier = 2f;

    public override void Execute(EffectContext context)
    {
        if (!context.IsReversed)
            context.Game.damageMultiplier = multiplier;
        else
            context.Game.damageMultiplier = 1f / multiplier;
    }
}