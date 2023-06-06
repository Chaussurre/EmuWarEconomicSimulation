using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop
{
    public abstract class CardVisual<TCardData> : MonoBehaviour where TCardData : struct
    {
        private int OriginalCardID;
        private CardManager<TCardData> Manager;

        public void InitID(int ID, CardManager<TCardData> manager)
        {
            OriginalCardID = ID;
            Manager = manager;
        }

        public CardVisual<TCardData> UpdateData(Card<TCardData>.CardInstance Card)
        {
            if (Card.ID != OriginalCardID)
            {
                Destroy(gameObject);

                return Manager.GetCard(Card)
                    .CreateVisual(Card, Manager);
            }
         
            
            UpdateInternData(Card.data);
            return this;
        }

        abstract protected void UpdateInternData(TCardData cardData);
    }
}
