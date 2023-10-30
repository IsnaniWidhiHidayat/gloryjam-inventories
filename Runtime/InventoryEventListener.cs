using System;
using GloryJam.Event;
using UnityEngine;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [AddComponentMenu("Glory Jam/Inventory/" + nameof(InventoryEventListener))]
    public class InventoryEventListener : MonoBehaviour, EventListener<InventoryEvent>
    {
        #region const
        const string grpEvent = "Events";
        const string grpConfig = "Config";
        #endregion

        #region fields
        [SerializeField]
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        #endif
        private string _id;
        #endregion

        #region events
        #if ODIN_INSPECTOR
        [BoxGroup(grpEvent)]
        #endif
        public UnityEvent<ItemStack> onStackInit,onStackUse,onStackUnuse,onStackDispose;
        #endregion

        #region methods
        private void OnEnable() {
            this.RegisterEvent();
        }
        private void OnDisable() {
            this.UnregisterEvent();
        }
        
        private void InvokeStackInit(ItemStack stack){
            onStackInit?.Invoke(stack);
        }
        private void InvokeStackUse(ItemStack stack){
            onStackUse?.Invoke(stack);
        }
        private void InvokeStackUnuse(ItemStack stack){
            onStackUnuse?.Invoke(stack);
        }
        private void InvokeStackDispose(ItemStack stack){
            onStackDispose?.Invoke(stack);
        }
        #endregion

        #region EventListener
        public void OnEvent(InventoryEvent e){
            //Check id
            if(!string.IsNullOrEmpty(_id) && _id != e.id) return;
            
            switch(e.type){
                case InventoryEvent.Type.Init:{
                    InvokeStackInit(e.stack);
                    break;
                }

                case InventoryEvent.Type.Use:{
                    InvokeStackUse(e.stack);
                    break;
                }

                case InventoryEvent.Type.Unuse:{
                    InvokeStackUnuse(e.stack);
                    break;
                }

                case InventoryEvent.Type.Dispose:{
                    InvokeStackDispose(e.stack);
                    break;
                }
            }
        }
        #endregion
    }
}