using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinator
{
    public class CardStackVisual<TCardData> : MonoBehaviour, IEnumerable<CardVisual<TCardData>> where TCardData : struct
    {
        public enum DisplayStyle
        {
            FixedSpacingFromLeft,
            FixedSpacingFromRight,
            FixedSpacingCentered,
            ExpandSpacing
        }

        [SerializeField] private CardManager<TCardData> CardManager;
        public CardStack<TCardData> CardStack;
        [SerializeField] protected DisplayStyle Style;
        [SerializeField] protected float FixedSpacing;
        [SerializeField] protected int margins;
        [SerializeField] protected bool CardOnTopIsShownFirst;
        [SerializeField] protected bool OnlyShowTop;

        [SerializeField] protected int DeltaSortingOrdering;

        private List<CardVisual<TCardData>> CardVisuals = new();
        private HashSet<CardVisual<TCardData>> UncontroledCards = new();

        [SerializeField] private Vector3 from;
        [SerializeField] private Vector3 to;

        private void Awake()
        {
            CardManager.VisualManager.RegisterStack(this);
        }

        private void Update()
        {
            for (int i = 0; i < CardVisuals.Count; i++)
                if (!UncontroledCards.Contains(CardVisuals[i]))
                    CardVisuals[i]?.MoveTo(GetPosition(i, CardVisuals.Count));
        }

        public void InsertCard(Card<TCardData>.CardInstance card, int index, PlayerMask? Visibility = null)
        {
            var position = GetPosition(index, CardVisuals.Count + 1);
            CardVisuals.Insert(index, CardManager.VisualManager.GetVisual(card, Visibility, position));
            CardVisuals[index].UpdateData(card.data);

            for (int i = 0; i < CardVisuals.Count; i++)
                SetOrdering(i);
        }

        public void SetCard(Card<TCardData>.CardInstance card, int index, PlayerMask? Visibility = null)
        {
            var position = GetPosition(index, CardVisuals.Count);
            CardVisuals[index] = CardManager.VisualManager.GetVisual(card, Visibility, position);
            CardVisuals[index].UpdateData(card.data);

            SetOrdering(index);
        }

        public void UpdateCard(TCardData data, int index)
        {
            CardVisuals[index].UpdateData(data);
        }

        public void RemoveCard(int index)
        {
            CardVisuals.RemoveAt(index);
        }
        public void RemoveCard(CardVisual<TCardData> cardVisual)
        {
            CardVisuals.Remove(cardVisual);
        }

        public void Clear()
        {
            CardVisuals.Clear();
        }

        public void SetControl(CardVisual<TCardData> cardVisual, bool value)
        {
            if (value)
                UncontroledCards.Remove(cardVisual);
            else
                UncontroledCards.Add(cardVisual);
        }

        internal void DumpVisuals(HashSet<int> DumpBuffer, PlayerMask PointOfView)
        {
            if (OnlyShowTop)
            {
                while (CardVisuals.Count > 0)
                    CardVisuals.RemoveAt(0);

                var card = CardStack.GetCard(CardStack.Size - 1);
                if (card.HasValue)
                {
                    if (CardVisuals.Count > 0)
                        SetCard(card.Value, 0, PointOfView);
                    else
                        InsertCard(card.Value, 0, PointOfView);
                    DumpBuffer.Remove(card.Value.CardID);
                }

                return;
            }

            for (int i = 0; i < CardStack.Size; i++)
            {
                var card = CardStack.GetCard(i);

                if (i >= CardVisuals.Count)
                    InsertCard(card.Value, i, PointOfView);
                else
                    SetCard(card.Value, i, PointOfView);
                DumpBuffer.Remove(card.Value.CardID);
            }

            for (int i = CardVisuals.Count - 1; i >= CardStack.Size; i--)
                CardVisuals.RemoveAt(i);
        }

        private Vector3 GetPosition(int CardIndex, int max)
        {
            return Vector3.Lerp(from, to, GetPosFloat(CardIndex, max)) + transform.position;
        }

        private float GetPosFloat(int index, int max)
        {
            switch (Style)
            {
                case DisplayStyle.FixedSpacingFromLeft:
                    return GetFixedPos(index, max, 0);
                case DisplayStyle.FixedSpacingFromRight:
                    return GetFixedPos(index, max, 1);
                case DisplayStyle.FixedSpacingCentered:
                    return GetFixedPos(index, max, 0.5f);

                case DisplayStyle.ExpandSpacing:
                    return GetExpandPos(index, max);
            }
            return 0;
        }

        private float GetFixedPos(int index, int max, float originLerp)
        {
            var dist = Vector3.Distance(from, to);
            if (dist <= 0.001f)
                return 0.5f;

            //the size of the space taken by all cards
            var size = (FixedSpacing * max) / dist;
            if (size > 1)
                return GetExpandPos(index, max);

            //the size of possible positions for the center of the list of cards
            var margins = size / 2;

            var centerPos = Mathf.Lerp(margins, 1 - margins, originLerp);
            var left = centerPos - size / 2;
            var right = centerPos + size / 2;

            return Mathf.Lerp(left, right, (float)index / max) + FixedSpacing / 2 / dist;
        }

        private float GetExpandPos(int index, int max)
        {
            if (max == 1)
                return 0;

            return (float)(index + margins) / (max + 2 * margins - 1);
        }

        private void SetOrdering(int index)
        {
            var card = CardVisuals[index];

            if (OnlyShowTop)
                card.SetRenderingOrder(DeltaSortingOrdering);
            else if (CardOnTopIsShownFirst)
                card.SetRenderingOrder(index + DeltaSortingOrdering);
            else
                card.SetRenderingOrder(CardVisuals.Count - index - 1 + DeltaSortingOrdering);
        }
            

        public IEnumerator<CardVisual<TCardData>> GetEnumerator()
        {
            return CardVisuals.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return CardVisuals.GetEnumerator();
        }

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(from + transform.position, 0.1f) ;
            Gizmos.DrawLine(from + transform.position, to + transform.position);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(to + transform.position, 0.1f);
        }
#endif
    }
}