using System;
using Sirenix.OdinInspector;

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemUsageHandler : ItemComponentHandler
    {
        #region property 
        #if ODIN_INSPECTOR
        [ShowInInspector,HideInEditorMode,BoxGroup(grpRuntime)]
        #endif
        public abstract bool inUse {get;}
        #endregion

        #region methods  
        public abstract bool Use();
        public abstract bool Unuse();
        #endregion
    }

    [Serializable]
    public abstract class ItemUsageHandler<T> : ItemUsageHandler
    where T : ItemComponentHandlerState, new()
    {
        #region fields
        [ShowIf(nameof(state)),BoxGroup("State"),HideLabel,PropertyOrder(-1)]
        public T state;
        #endregion

        #region methods
        public override ItemComponentHandler CreateInstance(){
            var r = base.CreateInstance() as ItemUsageHandler<T>;
            r.state = new T();
            return r; 
        }
        #endregion
    }
}
