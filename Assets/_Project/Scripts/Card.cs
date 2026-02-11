using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class Card : ScriptableObject
{
    [SerializeField] private string cardName;
    public string CardName => cardName;
    [SerializeField] private int damage;
    [SerializeField] private int money;
    [SerializeField] private CardType type;


    [SerializeField] private List<EffectType> cardPowerUps;
    [SerializeField] private List<EffectType> cardPowerDowns;

    [SerializeField] private Sprite cardArtwork;
    public Sprite CardArtwork => cardArtwork;

    public enum CardType
    {
        Swords, Cups, Wands, Pentacles, MajorArcana, Basic
    }

    public enum EffectType
    {
        Confusion, DoubleDamage, UpMore, DownMore 
    }
}
