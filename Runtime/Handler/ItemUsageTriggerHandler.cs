using System;
using System.Collections.Generic;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemUsageTriggerHandler : ItemUsageHandler
    {
        #region field
        #if ODIN_INSPECTOR
        [ListDrawerSettings(Expanded = true,DraggableItems = false,ListElementLabelName = "title")]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        #endif
        public List<ItemTriggerHandler> triggers = new List<ItemTriggerHandler>();

        #if ODIN_INSPECTOR
        [ListDrawerSettings(Expanded = true,DraggableItems = false,ListElementLabelName = "title")]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        #endif
        public List<ItemUsageHandler> handlers = new List<ItemUsageHandler>();
        #endregion

        #region property
        public override string name => "Usage Trigger";

        #if ODIN_INSPECTOR
        [ShowInInspector,ShowIf(nameof(InspectorShowRuntime)),BoxGroup(grpRuntime)]
        #endif
        public override bool inUse => _inUse;
        #endregion

        #region private
        private bool _inUse;
        #endregion

        #region methods
        public override void SetComponent(ItemComponent component)
        {
            base.SetComponent(component);
            for (int i = 0; i < triggers.Count; i++)
            {
                triggers[i]?.SetComponent(component);
            }

            for (int i = 0; i < handlers.Count; i++)
            {
                handlers[i]?.SetComponent(component);
            }
        }
        public override bool Use(){ 
            //register trigger
            for (int i = 0; i < triggers.Count; i++)
            {
                if(triggers[i] == null) continue;
                triggers[i].OnInit();
                triggers[i].onTrigger -= OnTrigger;
                triggers[i].onTrigger += OnTrigger;
            }

            _inUse = true;

            return true;
        }
        public override bool Unuse(){
            //unregister trigger
            for (int i = 0; i < triggers.Count; i++)
            {
                if(triggers[i] == null) continue;
                triggers[i].onTrigger -= OnTrigger;
                triggers[i].OnDispose();
            }

            //unuse item
            for (int i = 0; i < handlers.Count; i++)
            {
                handlers[i]?.Unuse();
            }

            _inUse = false;

            return true;
        }
        public override ItemComponentHandler CreateInstance()
        {
            var clone = new ItemUsageTriggerHandler(){
                id = id
            };

            //clone triggers
            if(triggers?.Count > 0){
                if(clone.triggers == null) clone.triggers = new List<ItemTriggerHandler>();

                for (int i = 0; i < triggers.Count; i++){
                    if(triggers[i] == null) continue;
                    var trigger = triggers[i].CreateInstance() as ItemTriggerHandler;
                    if(trigger == null) continue;
                    clone.triggers.Add(trigger);
                }
            }

            //clone handlers
            if(handlers?.Count > 0){
                if(clone.handlers == null) clone.handlers = new List<ItemUsageHandler>();

                for (int i = 0; i < handlers.Count; i++)
                {
                    if(handlers[i] == null) continue;
                    var handler = handlers[i].CreateInstance() as ItemUsageHandler;
                    if(handler == null) continue;
                    clone.handlers.Add(handler);
                }
            }

            return clone;
        }
        #endregion

        #region callback
        public override void OnInit()
        {
            for (int i = 0; i < handlers.Count; i++)
            {
                handlers[i]?.OnInit();
            }

            if(inUse) Unuse();
        }
        public override void OnPostInit(){
            for (int i = 0; i < handlers.Count; i++)
            {
                handlers[i]?.OnPostInit();
            }
        }
        public override void OnDispose(){
            for (int i = 0; i < handlers.Count; i++)
            {
                handlers[i]?.OnDispose();
            }
        }
        private void OnTrigger()
        {
            for (int i = 0; i < handlers.Count; i++)
            {
                handlers[i]?.Use();
            }
        }
        #endregion
    }
}
