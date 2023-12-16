using System;
using System.Collections.Generic;
using GloryJam.Extend;



#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
using UnityEngine;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemUsageTriggerHandler : ItemUsageHandler, IHandlers<ItemUsageHandler>
    {
        #region field
        #if ODIN_INSPECTOR
        [Space(1)]
        [ListDrawerSettings(Expanded = true,DraggableItems = false,ListElementLabelName = "title")]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        #endif
        public List<ItemTriggerHandler> triggers = new List<ItemTriggerHandler>();

        #if ODIN_INSPECTOR
        [Space(1)]
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
            StartListening();

            return true;
        }
        public override bool Unuse(){
            StopListening();

            //unuse item
            for (int i = 0; i < handlers.Count; i++)
            {
                handlers[i]?.Unuse();
            }

            return true;
        }
        private void StartListening(){
            _inUse = true;

            //register trigger
            for (int i = 0; i < triggers.Count; i++)
            {
                if(triggers[i] == null) continue;
                triggers[i].OnInit();
                triggers[i].onTrigger -= OnTrigger;
                triggers[i].onTrigger += OnTrigger;
            }
        }
        private void StopListening(){
            _inUse = false;

            //unregister trigger
            for (int i = 0; i < triggers.Count; i++)
            {
                if(triggers[i] == null) continue;
                triggers[i].onTrigger -= OnTrigger;
                triggers[i].OnDispose();
            }

        }
        public List<ItemUsageHandler> GetHandlers()
        {
            return handlers;
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
            if(inventory == null) StopListening();

            for (int i = 0; i < handlers.Count; i++)
            {
                handlers[i]?.OnInit();
            }
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
            var markLog = 0;
            for (int i = 0; i < handlers.Count; i++)
            {
                if(handlers[i] == null) continue;
                if(markLog == 0){
                    $"Usage Trigger Handler {stack}".Log(DebugFilter.Item);
                    markLog++;
                }
                handlers[i]?.Use();
            }
        }
        #endregion
    }
}
