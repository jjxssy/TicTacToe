using UnityEngine;


[CreateAssetMenu(menuName = "Card Effects/Silence")]
public class Silence : CardEffect
{
    public override void Execute(EffectContext context)
    {
        GameManager.Instance.StartTargeting(target => 
        {
            if (!target.isPlayerCard) 
            {
                target.ApplySilence();
            }
        });
    }
}