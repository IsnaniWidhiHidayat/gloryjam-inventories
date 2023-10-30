using System;
using Sirenix.OdinInspector;

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemUsageHandler : ItemUsageHandler<ItemUsageState>
    {   

    }

    [Serializable]
    public abstract class ItemUsageHandler<T> : ItemComponentHandler<ItemUsageHandler,T>
    where T : ItemUsageState,new()
    {
        #region property
        [ShowInInspector]
        #if ODIN_INSPECTOR
        [HideInEditorMode]
        #endif
        public abstract bool inUse {get;}
        #endregion

        #region methods
        public virtual bool InternalUse(){
            var r = Use();
            SaveState();
            return r;
        }
        public virtual bool InternalUnuse(){
            var r = Unuse();
            SaveState();
            return r;
        }
        
        public abstract bool Use();
        public abstract bool Unuse();
        
        public override void SaveState()
        {
            state.inUse = inUse;
        }
        public override void LoadState()
        {
            if(state == null) return;
            if(!inUse && state.inUse){
                Use();
            }else if(inUse && !state.inUse){
                Unuse();
            }
        }
        #endregion
    }
}
