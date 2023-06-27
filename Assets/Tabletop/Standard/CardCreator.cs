using UnityEngine;

namespace Tabletop.Standard
{
    public class CardCreator : CardStack
    {
        public CardManager CardManager;
        public Card cardToCreate;

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
    }
}
