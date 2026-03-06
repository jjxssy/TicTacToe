using UnityEngine;

[CreateAssetMenu(fileName = "NewDestroyEffect", menuName = "Card Effects/Destroy")]
public class DestroyEffect : CardEffect
{
    public override void Execute(EffectContext context)
    {
        GameManager.Instance.StartTargeting(target => 
        {
            if (!target.isPlayerCard)
            {
                Debug.Log("Destroying: " + target.gameObject.name);
                Destroy(target.gameObject);
            }
        });
    }
}