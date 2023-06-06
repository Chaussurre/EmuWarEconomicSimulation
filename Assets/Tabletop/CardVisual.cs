using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop
{
    public abstract class CardVisual<TCardData> : MonoBehaviour where TCardData : struct
    {
        abstract public void UpdateData(TCardData cardData);
    }
}
