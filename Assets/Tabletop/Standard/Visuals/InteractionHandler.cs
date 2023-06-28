using System.Collections;
using UnityEngine;

namespace Tabletop.Standard
{
    public abstract class InteractionHandler : MonoBehaviour
    {
        public abstract void OnCardInteract(CardStackVisualHandler<CardData>.CardInteractionData data);
    }
}