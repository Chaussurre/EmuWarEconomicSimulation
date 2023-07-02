using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Tabletop
{
    public class CardStackVisualHandler<TCardData> : MonoBehaviour where TCardData : struct
    {
        public enum ClickStatus
        {
            Nothing, Grab, Hold, Drop
        }

        public enum MouseStatus
        {
            Nothing, Enter, Hover, Exit
        }

        [Serializable]
        public struct CardInteractionData
        {
            public CardVisual<TCardData> Target;
            public Card<TCardData>.CardInstance TargetInstance;

            public ClickStatus LeftClickStatus;
            public ClickStatus RightClickStatus;

            public bool LeftClick;
            public bool RightClick;

            public MouseStatus MouseStatus;
            public Vector3 MousePosition;

            public CardStackVisualHandler<TCardData> Handler;
            public CardVisualManager<TCardData> VisualManager;

            public bool isHovered()
            {
                return MouseStatus == MouseStatus.Enter || MouseStatus == MouseStatus.Hover;
            }

            public bool isClicking(int click)
            {
                var clickValue = click == 0 ? LeftClickStatus : RightClickStatus;
                return clickValue == ClickStatus.Grab || clickValue == ClickStatus.Hold;
            }

            public bool isAnythingHappening()
            {
                return MouseStatus != MouseStatus.Nothing || LeftClickStatus != ClickStatus.Nothing || RightClickStatus != ClickStatus.Nothing;
            }
        }

        public CardStackVisual<TCardData> StackVisual;
        public CardManager<TCardData> CardManager;

        public UnityEvent<CardInteractionData> OnCardInteract = new();
        private CardInteractionData interactionData;

        private void Awake()
        {
            CardManager.ActionsManager.OnResolved.AddListener(resetInteractionData);
        }

        private void OnDestroy()
        {
            CardManager.ActionsManager.OnResolved.RemoveListener(resetInteractionData);
        }


        private void Update()
        {
            interactionData.Handler = this;
            interactionData.VisualManager = CardManager.VisualManager;

            interactionData.LeftClick = Input.GetMouseButton(0);
            interactionData.RightClick = Input.GetMouseButton(1);

            interactionData.MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            interactionData.MousePosition -= Vector3.forward * interactionData.MousePosition.z;

            var HoverTarget = FindTarget(interactionData.MousePosition);
            UpdateHover(HoverTarget);
            UpdateClick(0);
            UpdateClick(1);
            UpdateTarget(HoverTarget);
            if (interactionData.Target != null && CardManager.VisualManager.LockMouseHandler(this))
                OnCardInteract?.Invoke(interactionData);
            else
                CardManager.VisualManager.UnlockMouseHandler(this);

            foreach (var cardVisual in StackVisual)
                if (cardVisual != interactionData.Target)
                    StackVisual.SetControl(cardVisual, true);
        }

        private void UpdateTarget(CardVisual<TCardData> hoverTarget)
        {
            if (!interactionData.isAnythingHappening())
            {
                if (hoverTarget)
                {
                    var card = CardManager.GetCardInstance(hoverTarget.CardID);
                    if (!card.HasValue)
                        return;
                    interactionData.TargetInstance = card.Value;
                }

                interactionData.Target = hoverTarget;
            }
        }

        private void UpdateHover(CardVisual<TCardData> HoverTarget)
        {

            if (interactionData.isHovered())
            {
                interactionData.MouseStatus = MouseStatus.Hover;

                if (interactionData.Target != HoverTarget)
                   interactionData.MouseStatus = MouseStatus.Exit;
            }
            else
            {
                interactionData.MouseStatus = MouseStatus.Nothing;

                if (HoverTarget && interactionData.Target == HoverTarget)
                    interactionData.MouseStatus = MouseStatus.Enter;
            }
        }

        private void UpdateClick(int click)
        {
            var clickValue = click == 0 ? interactionData.LeftClickStatus : interactionData.RightClickStatus;

            if (clickValue == ClickStatus.Grab)
                clickValue = ClickStatus.Hold;
            else if (clickValue == ClickStatus.Hold && !Input.GetMouseButton(click))
                clickValue = ClickStatus.Drop;
            else if (clickValue == ClickStatus.Drop)
                clickValue = ClickStatus.Nothing;
            else if (clickValue == ClickStatus.Nothing && interactionData.isHovered() && Input.GetMouseButtonDown(click))
                clickValue = ClickStatus.Grab;

            if (click == 0)
                interactionData.LeftClickStatus = clickValue;
            else
                interactionData.RightClickStatus = clickValue;
        }


        private CardVisual<TCardData> FindTarget(Vector3 point)
        {
            foreach (var cardVisual in StackVisual)
                if (cardVisual.IsPointOnCard(point))
                    return cardVisual;

            if (interactionData.Target?.IsPointOnCard(point) ?? false)
                return interactionData.Target;

            return null;
        }

        private void resetInteractionData()
        {
            interactionData = new();
        }
    }
}