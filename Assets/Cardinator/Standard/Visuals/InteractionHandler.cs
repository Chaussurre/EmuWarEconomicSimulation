using System.Collections;
using UnityEngine;

namespace Cardinator.Standard
{
    public abstract class InteractionHandler : MonoBehaviour
    {
        public abstract void OnCardInteract(CardStackVisualHandler<CardData>.CardInteractionData data);
    }
}