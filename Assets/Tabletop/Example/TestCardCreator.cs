using UnityEngine;

namespace Tabletop
{
    public class TestCardCreator : MonoBehaviour
    {
        public TestCardPool CardPool;
        public TestCardStack Stack;
        public Card<int> CardToCreate;
        public int CardData;
        public int Index;

        [ContextMenu("Create Card")]
        public void Create()
        {
            Stack.InsertCard(CardPool.CreateInstance(CardToCreate.name, CardData), Index);
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
