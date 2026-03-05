using System;
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


 
    public List<CardEffect> powerUps = new List<CardEffect>();
    public List<CardEffect> powerDowns = new List<CardEffect>();
  
    

    public Sprite artwork;

    public enum CardType
    {
        Swords, Cups, Wands, Pentacles, MajorArcana, Basic
    }

    public enum UseType
    {
        Spell, Unit, Trap
    }
    public string GetTooltipText()
    {
        string text = "<b>Power Ups:</b>\n";
        foreach(var e in powerUps) text += "- " + e.name + "\n";
        
        if(powerDowns.Count > 0)
        {
            text += "\n<b>Power Downs (Reversed):</b>\n";
            foreach(var e in powerDowns) text += "- " + e.name + "\n";
        }
        return text;
    }
/*
    public enum EffectType
    {
        Confusion, DoubleDamage, UpMore, DownMore, Heal, Silence, Kill, DrawCard, Freeze,
        AttackConfusion, MoveUnit, Stealth, RemoveBuffs, RemoveDebuffs, Revel, Shield, Debuff,
        CounterAttack
    }
*/
}
