using UnityEngine;

namespace Tabletop.Standard
{
    public class CardCreator : StandardCardStack
    {
        public StandardActionManager ActionManager;
        public StandardCard cardToCreate;
        public int cardToPlay;

        [ContextMenu("Create Card")]
        public void Create()
        {
            var instance = CardManager.CreateInstance(cardToCreate);
            AddCard(instance);
        }

        [ContextMenu("Play Card")]
        public void Play()
        {
            var instance = GetCard(cardToPlay);

            ActionPlayWatcher.PlayData action = new()
            {
                CardID = instance.CardID,
            };
            ActionManager.AddAction(action);
        }
    }
}
