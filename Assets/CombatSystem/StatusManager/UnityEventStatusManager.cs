using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class UnityEventStatusManager : StatusManager<UnityEventDataWatcher<StatusStackChange>>
    {
        protected override UnityEventDataWatcher<StatusStackChange> InitDataWatcher() => new();
    }
}
