using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{   
    #if ODIN_INSPECTOR
    [Serializable,HideReferenceObjectPicker,HideDuplicateReferenceBox]
    [Toggle("Enabled")]
    #endif
    public class ItemUseableComponent : ItemComponent<ItemUseableComponent,ItemUsageHandler,ItemUseableState>
    {
        #region inner class
        [Serializable]
        public enum TriggerType{
            Manual,
            Instant,
        }
        #endregion

        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        #endif
        public TriggerType trigger;
        #endregion

        #region property
        #if ODIN_INSPECTOR
        [ShowInInspector,ShowIf(nameof(InspectorShowRuntime)),BoxGroup(grpRuntime)]
        #endif
        public bool inUse{
            get{
                var result = false;

                for (int i = 0; i < handlers.Count; i++)
                {
                    if(handlers[i] == null)
                        continue;

                    result |= handlers[i].inUse;
                    if(result) break;
                }

                return result;
            }
        }

        public override string ComponentName => "Usage";
        public override int ComponentPropertyOrder => 2;
        #endregion

        #region methods
        public override void OnInit()
        {
            base.OnInit();

            if(trigger == TriggerType.Instant){
                Use();
            }
        }
        public override void OnDispose()
        {
            base.OnDispose();

            Unuse();
        }

        public virtual bool Use(){
            var prevInUse = inUse;
            var result = false;

            for (int i = 0; i < handlers.Count; i++)
            {
                if(handlers[i] == null)
                    continue;

                result |= handlers[i].Use();
            }

            if(!prevInUse && inUse) {
                inventory?.InvokeOnItemUse(_stack);
            }

            inventory?.SaveState();

            return result;
        }
        public virtual bool Unuse(){
            var prevInUse = inUse;
            var result = false;

            for (int i = 0; i < handlers.Count; i++)
            {
                if(handlers[i] == null)
                    continue;

                result |= handlers[i].Unuse();
            }
            
            if(prevInUse && !inUse){
                inventory?.InvokeOnItemUnuse(_stack);
            }

            inventory?.SaveState();

            return result;
        }

        public override void SaveState()
        {
            base.SaveState();

            state.inUse = inUse;
        }
        public override void LoadState()
        {
            base.LoadState();

            if(!inUse && state.inUse){
                Use();
            }else if(inUse && !state.inUse){
                Unuse();
            }
        }

        public override ItemComponent CreateInstance()
        {
            var clone = base.CreateInstance() as ItemUseableComponent;
                clone.trigger = trigger;
            return clone;
        }
        #endregion
    }
}