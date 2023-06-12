using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tabletop.Standard
{
    public struct StrikeData
    {
        public int CardID1;
        public int CardID2;
        public bool strikeBack;
    }

    public class ActionStrikeWatcher : ActionWatcher<StandardCardData, StrikeData>
    {
        protected override void Apply(StrikeData actionData)
        {
            //todo
        }
    }
}
