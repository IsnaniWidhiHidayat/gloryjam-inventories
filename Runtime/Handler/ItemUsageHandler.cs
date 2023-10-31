using System;
using Sirenix.OdinInspector;

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemUsageHandler : ItemComponentHandler
    {
        #region const
        protected const string grpConfig = "Config";
        protected const string grpRuntime = "Runtime";
        protected const string grpRequired = "Required";
        #endregion

        #region property 
        #if ODIN_INSPECTOR
        [ShowInInspector,HideInEditorMode,BoxGroup(grpRuntime)]
        #endif
        public abstract bool inUse {get;}
        #endregion

        #region methods  
        public abstract bool Use();
        public abstract bool Unuse();
        
        public override void SaveState()
        {
            var state = this.state as ItemUsageState;
            if(state == null) return;

            state.inUse = inUse;
        }
        public override void LoadState()
        {
            var state = this.state as ItemUsageState;
            if(state == null) return;
            
            if(!inUse && state.inUse){
                Use();
            }else if(inUse && !state.inUse){
                Unuse();
            }
        }
        
        protected override ItemComponentHandlerState CreateState()
        {
            return new ItemUsageState();
        }
        #endregion
    }
}
