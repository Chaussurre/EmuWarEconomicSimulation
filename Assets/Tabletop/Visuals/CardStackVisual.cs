using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop
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

        private List<CardVisual<TCardData>> CardVisuals = new();
        private HashSet<CardVisual<TCardData>> UncontroledCards = new();

        [SerializeField] private Vector2 Size;

        private void Start()
        {
            CardManager.VisualManager.RegisterStack(CardStack, this);
        }

        private void Update()
        {
            for (int i = 0; i < CardVisuals.Count; i++)
                if (!UncontroledCards.Contains(CardVisuals[i]))
                    CardVisuals[i]?.MoveTo(GetPosition(i));
        }

        public int Count => CardVisuals.Count;

        public void InsertCard(Card<TCardData>.CardInstance card, int index)
        {
            CardVisuals.Insert(index, CardManager.VisualManager.GetVisual(card));
            CardVisuals[index].UpdateData(card.data);
        }

        public void SetCard(Card<TCardData>.CardInstance card, int index)
        {
            CardVisuals[index] = CardManager.VisualManager.GetVisual(card);
            CardVisuals[index].UpdateData(card.data);
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

        private Vector3 GetPosition(int CardIndex)
        {
            return GetPosFloat(CardIndex) * Vector3.right + transform.position;
        }

        private float GetPosFloat(int index)
        {
            switch (Style)
            {
                case DisplayStyle.FixedSpacingFromLeft:
                    return GetFixedPos(index, 0);
                case DisplayStyle.FixedSpacingFromRight:
                    return GetFixedPos(index, 1);
                case DisplayStyle.FixedSpacingCentered:
                    return GetFixedPos(index, 0.5f);

                case DisplayStyle.ExpandSpacing:
                    return GetExpandPos(index);
            }
            return 0;
        }

        private float GetFixedPos(int index, float originLerp)
        {
            //the size of the space taken by all cards
            var size = FixedSpacing * CardVisuals.Count;
            if (size > Size.x)
                return GetExpandPos(index);

            //the size of possible positions for the center of the list of cards
            var centerSize = Size.x - size;

            var centerPos = Mathf.Lerp(-centerSize / 2, centerSize / 2, originLerp);
            var left = centerPos - size / 2;
            var right = centerPos + size / 2;

            return Mathf.Lerp(left, right, (float)index / CardVisuals.Count);
        }

        private float GetExpandPos(int index)
        {
            if (CardVisuals.Count == 1)
                return 0;

            return Mathf.Lerp(-Size.x / 2, Size.x / 2, (float)(index + margins) / (CardVisuals.Count + 2 * margins - 1));
        }

        public IEnumerator<CardVisual<TCardData>> GetEnumerator()
        {
            return CardVisuals.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return CardVisuals.GetEnumerator();
        }
    }
}