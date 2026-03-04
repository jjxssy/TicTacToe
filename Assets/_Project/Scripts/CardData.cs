using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public int damage;
    public int money;
    public CardType type;

    public UseType useType;

    

    public Color glowColor = Color.white;


    public List<EffectType> powerUps;
    public List<EffectType> powerDowns;
    public List<OnEnter> onEnter;

    public Sprite artwork;

    public enum CardType
    {
        Swords, Cups, Wands, Pentacles, MajorArcana, Basic
    }

    public enum UseType
    {
        spell ,unit,Trap
    }

    public enum EffectType
    {
        Confusion, DoubleDamage, UpMore, DownMore, Heal, Silence , kill, DrawCard , Freeze ,
         AttackConfusion , MoveUnit , stealth , RemoveBuffs , RemoveDebuffs , Revel , shield , Debuff,
         CounterAttack
    }
    public enum OnEnter
    {
        None, DamageAllEnemies, HealAllAllies, BuffNextUnit, DebuffNextEnemy
    }
}
