using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemUsageHandler : ItemComponentHandler
    {
        #region fields
        [Tooltip("check this if you want item not unuse when disposed")]
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        #endif
        public bool persistent;
        #endregion

        #region property 
        #if ODIN_INSPECTOR
        [ShowInInspector]
        [ShowIf(nameof(InspectorShowRuntime))]
        [BoxGroup(grpRuntime)]
        #endif
        public abstract bool inUse {get;}
        #endregion

        #region methods  
        public abstract bool Use();
        public abstract bool Unuse();
        #endregion
    }
}
