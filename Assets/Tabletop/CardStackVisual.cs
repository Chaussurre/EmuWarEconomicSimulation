using System.Collections.Generic;
using UnityEngine;

namespace Tabletop
{
    public class CardStackVisual<TCardData> : MonoBehaviour where TCardData : struct
    {
        public enum DisplayStyle
        {
            FixedSpacingFromLeft,
            FixedSpacingFromRight,
            FixedSpacingCentered,
            ExpandSpacing
        }

        [SerializeField] private CardVisualManager<TCardData> VisualManager;
        [SerializeField] private CardStack<TCardData> CardStack;
        [SerializeField] protected DisplayStyle Style;
        [SerializeField] protected float FixedSpacing;
        [SerializeField] protected int margins;

        private List<CardVisual<TCardData>> CardVisuals = new();

        public Vector2 Size;

        private void Update()
        {
            for (int i = 0; i < CardVisuals.Count; i++)
                CardVisuals[i]?.MoveTo(GetPosition(i));
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
    }
}