using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemUsageHandler : ItemComponentHandler
    {
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

    [Serializable]
    public abstract class ItemUsageHandler<T> : ItemUsageHandler
    where T : ItemComponentHandlerState, new()
    {
        #region fields
        #if ODIN_INSPECTOR
        [ShowIf(nameof(state)),BoxGroup("State"),HideLabel,PropertyOrder(-1)]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        #endif
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
