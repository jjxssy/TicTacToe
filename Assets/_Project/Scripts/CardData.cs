using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public int damage;
    public int money;
    public CardType type;

    public Color glowColor = Color.white;


    public List<EffectType> powerUps;
    public List<EffectType> powerDowns;

    public Sprite artwork;

    public enum CardType
    {
        Swords, Cups, Wands, Pentacles, MajorArcana, Basic
    }

    public enum EffectType
    {
        Confusion, DoubleDamage, UpMore, DownMore 
    }
}
