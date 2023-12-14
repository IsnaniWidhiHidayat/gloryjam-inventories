using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using UnityEngine;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemUsageHandler : ItemComponentHandler
    {
        #region fields
        [BoxGroup(grpConfig)]
        [Tooltip("check this if you want item not unuse when disposed")]
        public bool persistent;
        #endregion

        #region property 
        #if ODIN_INSPECTOR
        [ShowInInspector,ShowIf(nameof(InspectorShowRuntime)),BoxGroup(grpRuntime)]
        #endif
        public abstract bool inUse {get;}
        #endregion

        #region methods  
        public abstract bool Use();
        public abstract bool Unuse();
        #endregion
    }
}
