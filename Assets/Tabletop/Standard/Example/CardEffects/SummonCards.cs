using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop.Standard
{
    public class SummonCards : MonoBehaviour
    {
        [Serializable]
        public struct SummonData
        {
            public Card CardToSummon;
            public bool UseDefaultData;
            public CardData data;
        }

        [SerializeField] private List<SummonData> CardsToSummon = new();
        [SerializeField] private bool Immediate;

        public void OnEffectTrigger(CardManager<CardData> CardManager, Card.CardInstance card)
        {
            foreach (var summonData in CardsToSummon)
            {
                var actionData = ActionSummonWatcher.Summon(summonData.CardToSummon, summonData.UseDefaultData ? null : summonData.data);
                if (Immediate)
                    CardManager.ActionsManager.AddActionImmediate(actionData);
                else
                    CardManager.ActionsManager.AddAction(actionData);
            }

        }
    }
}
