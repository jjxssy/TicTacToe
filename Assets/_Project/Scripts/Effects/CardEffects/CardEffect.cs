using System;
using UnityEngine;

[Serializable]
public abstract class CardEffect : ScriptableObject 
{   
    public abstract void Execute(EffectContext context);
}

public class EffectContext
{
    public CardData Card;
    public bool IsReversed;
    public BoardManager Board => BoardManager.Instance;
    public GameManager Game => GameManager.Instance;
}