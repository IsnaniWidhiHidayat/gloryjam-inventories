using System;
using UnityEngine;
using GloryJam.DataAsset;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{   
    [Serializable]
    public class ItemUseableComponent : ItemComponent<ItemUseableComponent,ItemUsageHandler,ItemUseableState>
    {
        #region static
        private static ItemUseableEvent Event = new ItemUseableEvent();
        #endregion

        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        #endif
        public ItemUseableTrigger.Type trigger;

        #if ODIN_INSPECTOR
        [ShowIf(nameof(trigger),ItemUseableTrigger.Type.Custom)]
        [ValidateInput(nameof(InspectorValidateTriggers),"Please remove empty trigger")]
        [ListDrawerSettings(Expanded = true,DraggableItems = false,ListElementLabelName = "name")]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        [Space(1)]
        #endif
        public ItemUseableTrigger[] triggers = new ItemUseableTrigger[0];
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

        public override string name => "Usage";
        public override int propertyOrder => 2;
        #endregion

        #region inspector
        #if ODIN_INSPECTOR
        [Button("Use"),PropertyOrder(100),BoxGroup(grpDebug),ButtonGroup(grpDebug + "/Buttons"),ShowIf(nameof(InspectorShowRuntime))]
        private void InspectorUse(){
            Use();
        }

        [Button("Unuse"),ButtonGroup(grpDebug + "/Buttons"),ShowIf(nameof(InspectorShowRuntime))]
        private void InspectorUnUse(){
            Unuse();
        }
        private static bool InspectorValidateTriggers(ItemUseableTrigger[] triggers)
        {
            return triggers == null ? true : !Array.Exists(triggers, x => x == null);
        }
        #endif
        #endregion

        #region methods
        public override void SetStack(ItemStack stack)
        {
            base.SetStack(stack);

            //set trigger
            if(trigger == ItemUseableTrigger.Type.Custom && triggers?.Length > 0){
                for (int i = 0; i < triggers.Length; i++)
                {
                    if(triggers[i] == null) continue;
                    triggers[i].SetComponent(this);
                }
            }
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

            //Trigger use
            if(!prevInUse && inUse) {
                Event.type  = ItemUseableEvent.Type.Use;
                Event.stack = stack;
                ItemUseableEvent.Trigger(inventory,Event);
            }

            var useableComponents = stack.GetComponents<ItemUseableComponent>();
            var isRearmost = Array.IndexOf(useableComponents,this) == useableComponents.Length - 1;

            if(isRearmost) {
                //Get IItem Useable and invoke Use
                if(result && stack.TryGetComponents<IItemUseable>(out var useables)){
                    for (int i = 0; i < useables.Length; i++)
                    {
                        if(useables[i] == null) continue;
                        useables[i].OnUse();
                    }
                }
    
                //consume item
                if(stack.TryGetComponentConsume(out var consume)){
                    consume.Consume();
                }
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
            
            //Trigger unuse
            if(prevInUse && !inUse){
                Event.type  = ItemUseableEvent.Type.Unuse;
                Event.stack = stack;
                ItemUseableEvent.Trigger(inventory,Event);
            }

            var useableComponents = stack.GetComponents<ItemUseableComponent>();
            var isRearmost = Array.IndexOf(useableComponents,this) == useableComponents.Length - 1;

            if(isRearmost) {
                //Get IItem Useable and invoke Unuse
                if(result && stack.TryGetComponents<IItemUseable>(out var useables)){
                    for (int i = 0; i < useables.Length; i++)
                    {
                        if(useables[i] == null) continue;
                        useables[i].OnUnuse();
                    }
                }
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

            //create triggers instance
            if(trigger == ItemUseableTrigger.Type.Custom){
                clone.triggers = triggers.CreateInstance();
            }

            return clone;
        }
        #endregion

        #region callback
        public override void OnInit()
        {
            base.OnInit();

            //invoke triggers init
            if(trigger == ItemUseableTrigger.Type.Custom && triggers?.Length > 0){
                for (int i = 0; i < triggers.Length; i++)
                {
                    if(triggers[i] == null) continue;
                    triggers[i].OnInit();
                    triggers[i].onTrigger -= OnTrigger;
                    triggers[i].onTrigger += OnTrigger;
                }
            }
        }
        public override void OnPostInit(){
            if(trigger == ItemUseableTrigger.Type.Instant && inventory != null && Application.isPlaying){
                Use();
            }
        }
        public override void OnDispose()
        {
            if(inUse) Unuse();

            base.OnDispose();

            //invoke triggers dispose
            if(trigger == ItemUseableTrigger.Type.Custom && triggers?.Length > 0){
                for (int i = 0; i < triggers.Length; i++)
                {
                    if(triggers[i] == null) continue;
                    triggers[i].onTrigger -= OnTrigger;
                    triggers[i].OnDispose();
                }
            }
        }
        private void OnTrigger()
        {
            Use();
        }
        #endregion
    }
}