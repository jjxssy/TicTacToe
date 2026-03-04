using UnityEngine;
using UnityEngine.EventSystems;

public class Efact : MonoBehaviour 
{

    private float damageMultiplier = 1f;
    private float confusionChance = 0f;

    public static Efact Instance;

    private void Awake()
    {
        Instance = this;
    }


    public void TriggerSpellCard(CardData data, bool reversed)
    {
        if (!reversed)
        {
            foreach (var effect in data.powerUps)
                ApplyEffect(effect);
        }
        else
        {
            foreach (var effect in data.powerDowns)
                ApplyEffect(effect);
        }
    }

    private void ApplyEffect(CardData.EffectType effect)
    {
        switch (effect)
        {
            case CardData.EffectType.DoubleDamage: DoubleDamageEffect(); break;
            case CardData.EffectType.Confusion: ConfusionEffect(); break;
           // case CardData.EffectType.UpMore: UpMore(); break;
            //case CardData.EffectType.DownMore: DownMore(); break;
        }
    }



    public void DoubleDamageEffect()
    {
        
        Debug.Log($"Double damage effect triggered for card");
    }

    public void ConfusionEffect()
    {
        
        Debug.Log($"Confusion effect triggered for card");
    }

    public static bool RollSpellDirection()
    {
        bool reversed = Random.value < 0.5f;
        Debug.Log(reversed ? "Down" : "UP");
        return reversed;
    }

}
