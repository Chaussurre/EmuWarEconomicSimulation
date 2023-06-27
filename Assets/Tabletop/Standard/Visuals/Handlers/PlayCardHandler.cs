using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop.Standard
{
    public class PlayCardHandler : MonoBehaviour
    {
        [SerializeField] float PlayHeight;

        public void OnCardInteract(CardStackVisualHandler<StandardCardData>.CardInteractionData data)
        {
            if (data.MousePosition.y < PlayHeight || data.LeftClickStatus != CardStackVisualHandler<StandardCardData>.ClickStatus.Drop)
                return;

            data.CardManager.ActionsManager.AddAction(new ActionPlayWatcher.PlayData()
            {
                CardID = data.Target.CardID
            });
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