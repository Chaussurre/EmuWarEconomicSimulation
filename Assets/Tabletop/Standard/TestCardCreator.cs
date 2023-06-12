using UnityEngine;

namespace Tabletop.Standard
{
    public class TestCardCreator : MonoBehaviour
    {
        public StandardCardPool CardPool;
        public StandardCardStack Stack;
        public Card<StandardCardData> CardToCreate;
        public StandardCardData CardData;
        public int Index;

        [ContextMenu("Create Card")]
        public void Create()
        {
            Stack.InsertCard(CardPool.CreateInstance(CardToCreate.name, CardData ), Index);
        }

        [ContextMenu("Update Card")]
        public void UpdateCard()
        {
            Stack.UpdateCard(CardPool.CreateInstance(CardToCreate, CardData), Index);
        }

        [ContextMenu("Remove Card")]
        public void Remove()
        {
            Stack.RemoveCard(Index);
        }
    }
}
