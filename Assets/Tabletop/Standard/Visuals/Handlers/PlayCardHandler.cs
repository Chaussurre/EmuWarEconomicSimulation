using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop.Standard
{
    public class PlayCardHandler : InteractionHandler
    {
        [SerializeField] float PlayHeight;

        public override void OnCardInteract(CardStackVisualHandler<CardData>.CardInteractionData data)
        {
            if (data.MousePosition.y < PlayHeight || data.LeftClickStatus != CardStackVisualHandler<CardData>.ClickStatus.Drop)
                return;

            var playData = ActionPlayWatcher.Play(data.Target.CardID);
            data.CardManager.ActionsManager.AddAction(playData);
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