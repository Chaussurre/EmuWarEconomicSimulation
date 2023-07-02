using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop
{
    [Serializable]
    public class CardVisualManager<TCardData> where TCardData : struct
    {
        [Tooltip("If this is on, when all actions and their visuals are done, this will remake all visuals at once." +
            " Useful if not all actions have visuals")]
        public bool DumpVisualsOnActionsEnd;
        public int PlayerPointOfView;
        public Vector3 CreationPosition;

        private CardManager<TCardData> CardManager;

        private Dictionary<int, CardVisual<TCardData>> VisualTracker = new();
        private HashSet<CardStackVisual<TCardData>> StackVisuals = new();

        private HashSet<int> DumpBuffer = new();

        private CardStackVisualHandler<TCardData> HandlerMouseLock; //only one handler can use the mouse

        private void OnResolve()
        {
            if (DumpVisualsOnActionsEnd)
                DumpVisuals();
        }

        public void DumpVisuals()
        {
            foreach(var cardID in VisualTracker.Keys)
                DumpBuffer.Add(cardID);

            foreach (var stackVis in StackVisuals)
                stackVis.DumpVisuals(DumpBuffer, new(PlayerPointOfView));

            foreach (var cardID in DumpBuffer)
                DeleteVisual(cardID);

            DumpBuffer.Clear();
        }

        internal void Init(CardManager<TCardData> cardManager)
        {
            CardManager = cardManager;
            cardManager.ActionsManager.OnResolved.AddListener(OnResolve);
        }

        public CardVisual<TCardData> GetVisual(Card<TCardData>.CardInstance card,
                                               PlayerMask? Visibility = null,
                                               Vector3? DefaultPosition = null)
        {
            var position = DefaultPosition ?? CreationPosition;

            if (VisualTracker.TryGetValue(card.CardID, out var visual))
            {
                position = visual.transform.position;
                if (CheckVisibility(visual, card, Visibility))
                    return visual;
                else
                    DeleteVisual(card.CardID);
            }

            var createdVisual = CreateUntrackedVisual(card, position, Visibility);
            VisualTracker.Add(card.CardID, createdVisual);
            return createdVisual;
        }

        private bool CheckVisibility(CardVisual<TCardData> visual, Card<TCardData>.CardInstance card, PlayerMask? visibility = null)
        {
            var pointOfView = visibility ?? PlayerMask.All;

            return visual.isHidden != (card.VisibleMask * pointOfView);
        }

        public CardVisual<TCardData> CreateUntrackedVisual(Card<TCardData>.CardInstance card, 
            Vector3? DefaultPosition = null,
            PlayerMask? Visibility = null)
        {
            if (!card.VisibleMask * (Visibility ?? PlayerMask.All))
            {
                var hiddenModel = CardManager.CardPool.GetHiddenCardTemplate();
                var hiddenVisual = hiddenModel.CreateVisual(card);
                hiddenVisual.transform.position = DefaultPosition ?? CreationPosition;
                hiddenVisual.isHidden = true;

                return hiddenVisual;
            }

            var model = CardManager.CardPool.GetCard(card);
            var createdVisual = model.CreateVisual(card);
            createdVisual.transform.position = DefaultPosition ?? CreationPosition;

            return createdVisual;
        }

        public void DeleteVisual(int CardID)
        {
            if (VisualTracker.TryGetValue(CardID, out var value))
                GameObject.Destroy(value.gameObject);

            VisualTracker.Remove(CardID);
        }

        public void RegisterStack(CardStackVisual<TCardData> stackVisual)
        {
            StackVisuals.Add(stackVisual);
        }

        public bool LockMouseHandler(CardStackVisualHandler<TCardData> Handler)
        {
            if (!HandlerMouseLock)
                HandlerMouseLock = Handler;
            return HandlerMouseLock == Handler;
        }

        public void UnlockMouseHandler(CardStackVisualHandler<TCardData> Handler)
        {
            if (HandlerMouseLock == Handler)
                HandlerMouseLock = null;
        }
    }
}