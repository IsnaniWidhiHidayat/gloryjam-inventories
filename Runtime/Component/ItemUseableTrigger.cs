using System;
using GloryJam.DataAsset;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace GloryJam.Inventories
{
    [Serializable]
    #if ODIN_INSPECTOR
    [Toggle("Enabled",CollapseOthersOnExpand = false)]
    #endif
    public struct ItemUseableTrigger : IInstance<ItemUseableTrigger>
    {
        #region field
        public bool Enabled;

        #if ODIN_INSPECTOR
        [ValidateInput(nameof(InspectorValidateTriggers),"Please remove empty trigger")]
        [ListDrawerSettings(Expanded = true,DraggableItems = false,ListElementLabelName = "title")]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        [Space(1)]
        #endif
        public ItemTriggerHandler[] triggers;
        #endregion

        #region property
        public string name => "Trigger";

        public string title {
            get{
                if(!Enabled) return name;

                return $"{name} ({(triggers != null ? triggers.Length : 0)})";
            }
        }
        #endregion

        #region events
        public event Action onTrigger;
        #endregion

        #region inspector
        #if ODIN_INSPECTOR
        private bool InspectorValidateTriggers(ItemTriggerHandler[] triggers)
        {
            return triggers == null ? true : !Array.Exists(triggers, x => x == null);
        }
        #endif
        #endregion

        #region methods
        public void SetComponent(ItemComponent component){
            if(triggers?.Length > 0){
                for (int i = 0; i < triggers.Length; i++)
                {
                    if(triggers[i] == null) continue;
                    triggers[i].SetComponent(component);
                }
            }
        }
        public void StartListenTrigger(){
            if(triggers?.Length > 0){
                for (int i = 0; i < triggers.Length; i++)
                {
                    if(triggers[i] == null) continue;
                    triggers[i].OnInit();
                    triggers[i].onTrigger -= OnTrigger;
                    triggers[i].onTrigger += OnTrigger;
                }
            }
        }
        public void StopListenTrigger(){
            if(triggers?.Length > 0){
                for (int i = 0; i < triggers.Length; i++)
                {
                    if(triggers[i] == null) continue;
                    triggers[i].onTrigger -= OnTrigger;
                    triggers[i].OnDispose();
                }
            }
        }
        public void InvokeOnTrigger(){
            onTrigger?.Invoke();
        }
        public ItemUseableTrigger CreateInstance(){
            var clone = new ItemUseableTrigger();
                
            if(triggers?.Length > 0){
                if(clone.triggers == null) clone.triggers = new ItemTriggerHandler[triggers.Length];

                for (int i = 0; i < triggers.Length; i++){
                    if(triggers[i] == null) continue;
                    clone.triggers[i] = triggers[i].CreateInstance() as ItemTriggerHandler;
                }
            }

            return clone;
        }
        #endregion

        #region callback
        private void OnTrigger()
        {
            InvokeOnTrigger();
        }
        #endregion
    }
}