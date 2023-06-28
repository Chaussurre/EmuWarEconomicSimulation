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
            var createCardData = ActionCreateCardWatcher.CreateCard(cardToCreate, new() { stack = this }, CardManager);
            CardManager.ActionsManager.AddAction(createCardData);
        }
    }
}
