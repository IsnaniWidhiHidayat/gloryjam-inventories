using System;
using Sirenix.OdinInspector;

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemUsageHandler : ItemComponentHandler<ItemUsageHandler>
    {
        #region property
        [ShowInInspector]
        #if ODIN_INSPECTOR
        [HideInEditorMode]
        #endif
        public abstract bool inUse {get;}
        #endregion

        #region methods
        public abstract bool Use();
        public abstract bool Unuse();
        #endregion
    }
}
