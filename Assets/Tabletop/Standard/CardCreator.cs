using UnityEngine;

namespace Tabletop.Standard
{
    public class CardCreator : StandardCardStack
    {
        public StandardCardManager CardManager;
        public StandardCard cardToCreate;
        public int cardToPlay;

        [ContextMenu("Create Card")]
        public void Create()
        {
            var cardModelID = CardManager.CardPool.GetCardIndex(cardToCreate);
            CardManager.ActionsManager.AddAction(new ActionCreateCardWatcher.CreateCardData()
            {
                CardModelID = cardModelID,
                Position = new() { stack = this }
            });
        }

        [ContextMenu("Play Card")]
        public void Play()
        {
            var instance = GetCard(cardToPlay);

            if (!instance.HasValue)
                return;

            ActionPlayWatcher.PlayData action = new()
            {
                CardID = instance.Value.CardID,
            };
            CardManager.ActionsManager.AddAction(action);
        }
    }
}
