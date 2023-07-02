using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cardinator.Standard
{
    public class PlayCardHandler : InteractionHandler
    {
        [SerializeField] float PlayHeight;
        public PlayerManager PlayerManager;

        public override void OnCardInteract(CardStackVisualHandler<CardData>.CardInteractionData data)
        {
            if (data.MousePosition.y < PlayHeight || data.LeftClickStatus != CardStackVisualHandler<CardData>.ClickStatus.Drop)
                return;

            var player = PlayerManager.ActivePlayer;
            player.PlayCard(data.Target.CardID);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new(-10, PlayHeight), new(10, PlayHeight));
        }
#endif
    }
}