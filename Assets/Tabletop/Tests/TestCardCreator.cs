using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop
{
    public class TestCardCreator : MonoBehaviour
    {
        public CardManager<int> Manager;
        public CardStack<int> Emplacement;
        public Card<int>.CardInstance CardToCreate;

        [ContextMenu("Create Card")]
        public void Create()
        {
            Emplacement.AddCard(CardToCreate, Manager);
        }

        [ContextMenu("Remove Card")]
        public void Remove()
        {
            Emplacement.RemoveCard(Manager);
        }
    }
}
