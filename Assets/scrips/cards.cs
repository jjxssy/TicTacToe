using System.Collections.Generic;
using UnityEngine;

namespace Sliklak
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Card")]
    public class Card : ScriptableObject
    {
        public string cardName;
        public int cardDamage;
        public int cardMoney;
        public CardType cardType;


        public List<CardPowerUp> cardPowerUps;
        public List<CardPowerDown> cardPowerDowns;

        public enum CardType
        {
            Swords,
            Cups,
            Wands,
            Pentacles,
            MajorArcana,
            Basic
        }

        public enum CardPowerUp
        {
            Confusion,
            DoubleDamage,
            UpMore,
            DownMore
        }

        public enum CardPowerDown
        {
            Confusion,
            DoubleDamage,
            UpMore,
            DownMore
        }

        public Sprite cardArtwork;
    }
}
