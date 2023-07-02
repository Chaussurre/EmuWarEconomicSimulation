using System.Collections;
using UnityEngine;
using dataBuffer = CombatSystem.DataWatcher<Cardinator.Standard.ActionCreateCardWatcher.CreateCardData>.DataWatcherBuffer;

namespace Cardinator.Standard
{
    public class HideCardsInDecks : MonoBehaviour
    {
        public PlayField PlayField;
        public CardManager CardManager;

        public void OnCardCreate(dataBuffer createCardData)
        {
            foreach(var field in PlayField.Fields)
            {
                if (field.Deck == createCardData.DataBuffer.Position.stack)
                {
                    var data = createCardData.DataBuffer;
                    data.VisibleMask = PlayerMask.Empty;
                    createCardData.DataBuffer = data;

                    return;
                }
            }
        }
    }
}