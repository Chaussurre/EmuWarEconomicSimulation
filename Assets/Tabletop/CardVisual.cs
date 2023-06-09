using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop
{
    public abstract class CardVisual<TCardData> : MonoBehaviour where TCardData : struct
    {
        private int OriginalCardID;
        private CardPool<TCardData> CardPool;

        private Vector3 localTarget;
        [SerializeField] private float speedLerp;

        public void InitID(int ID, CardPool<TCardData> cardPool)
        {
            OriginalCardID = ID;
            CardPool = cardPool;
        }

        public CardVisual<TCardData> UpdateData(Card<TCardData>.CardInstance Card)
        {
            if (Card.CardID != OriginalCardID)
            {
                Destroy(gameObject);

                return CardPool.GetCard(Card)
                    .CreateVisual(Card, CardPool);
            }
         
            
            UpdateInternData(Card.data);
            return this;
        }

        abstract protected void UpdateInternData(TCardData cardData);

        abstract public void SetRenderingOrder(int order);

        public void MoveTo(Vector3 localTarget)
        {
            this.localTarget = localTarget;
        }

        private void Update()
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, localTarget, speedLerp * Time.deltaTime);
        }
    }
}
