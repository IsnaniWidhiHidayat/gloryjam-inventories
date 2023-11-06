using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{   
    #if ODIN_INSPECTOR
    [Serializable,HideReferenceObjectPicker,HideDuplicateReferenceBox]
    [Toggle("Enabled",CollapseOthersOnExpand = false)]
    #endif
    public class ItemUseableComponent : ItemComponent<ItemUseableComponent,ItemUsageHandler,ItemUseableState>
    {
        #region static
        private static ItemUseableEvent Event = new ItemUseableEvent();
        #endregion

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

        #region inspector
        #if ODIN_INSPECTOR
        [Button("Use"),BoxGroup(grpDebug),ButtonGroup(grpDebug + "/Buttons"),ShowIf(nameof(InspectorShowRuntime))]
        private void InspectorUse(){
            Use();
        }

        [Button("Unuse"),ButtonGroup(grpDebug + "/Buttons"),ShowIf(nameof(InspectorShowRuntime))]
        private void InspectorUnUse(){
            Unuse();
        }
        #endif
        #endregion

        #region methods
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
                //Trigger event
                Event.type  = ItemUseableEvent.Type.Use;
                Event.stack = stack;
                ItemUseableEvent.Trigger(Event);
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
                //Trigger event
                Event.type  = ItemUseableEvent.Type.Unuse;
                Event.stack = stack;
                ItemUseableEvent.Trigger(Event);
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

        #region callback
        public override void OnPostInit(){
            if(trigger == TriggerType.Instant && Application.isPlaying){
                Use();
            }
        }
        public override void OnDispose()
        {
            if(inUse) Unuse();

            base.OnDispose();
        }
        #endregion
    }
}