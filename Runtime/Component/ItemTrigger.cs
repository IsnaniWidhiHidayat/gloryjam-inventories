using System;
using GloryJam.DataAsset;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemTrigger : IInstance<ItemTrigger>
    {
        #region innerclass
        [Serializable]
        public enum Type{
            Manual,
            Instant,
            Custom,
        }
        #endregion

        #region field
        public Type type;

        #if ODIN_INSPECTOR
        [ShowIf(nameof(type),Type.Custom)]
        [ValidateInput(nameof(InspectorValidateTriggers),"Please remove empty trigger")]
        [ListDrawerSettings(Expanded = true,DraggableItems = false,ListElementLabelName = "name")]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        [Space(1)]
        #endif
        public ItemTriggerHandler[] triggers = new ItemTriggerHandler[0];
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
            if(type == Type.Custom && triggers?.Length > 0){
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
            if(type == Type.Custom && triggers?.Length > 0){
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
        public ItemTrigger CreateInstance(){
            var clone = new ItemTrigger();
                clone.type = type;
                
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