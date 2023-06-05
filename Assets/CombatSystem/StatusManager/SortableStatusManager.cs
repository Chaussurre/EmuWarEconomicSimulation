using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem
{
    public class SortableStatusManager : StatusManager<SortableDataWatcher<StatusStackChange>>
    {
        protected override SortableDataWatcher<StatusStackChange> InitDataWatcher() => new();
    }
}
