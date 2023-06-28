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
            public StandardCardData data;
        }

        [SerializeField] private List<SummonData> CardsToSummon = new();
        [SerializeField] private bool Immediate;

        public void OnEffectTrigger(CardManager<StandardCardData> CardManager, Card.CardInstance card)
        {
            foreach (var summonData in CardsToSummon)
            {
                var actionData = new ActionSummonWatcher.SummonData()
                {
                    CardID = null,
                    CardModel = summonData.CardToSummon,
                    CardData = summonData.UseDefaultData ? null : summonData.data,
                };
                if (Immediate)
                    CardManager.ActionsManager.AddActionImmediate(actionData);
                else
                    CardManager.ActionsManager.AddAction(actionData);
            }

        }
    }
}
