using System;
using GloryJam.Event;
using UnityEngine;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [AddComponentMenu("Glory Jam/Inventory/Item Event Listener")]
    public class ItemEventListener : MonoBehaviour, 
    EventListener<InventoryEvent>,
    EventListener<ItemUseableEvent>
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
        private string _inventoryID;

        [SerializeField]
        #if ODIN_INSPECTOR
        [BoxGroup(grpRequired),Required]
        #endif
        private ItemAsset _item;
        #endregion

        #region events
        #if ODIN_INSPECTOR
        [BoxGroup(grpEvent)]
        #endif
        public UnityEvent<ItemStack> onStackInit,onStackUse,onStackUnuse,onStackDispose;
        #endregion

        #region methods
        private void OnEnable() {
            this.RegisterEvent<InventoryEvent>();
            this.RegisterEvent<ItemUseableEvent>();
        }
        private void OnDisable() {
            this.UnregisterEvent<InventoryEvent>();
            this.UnregisterEvent<ItemUseableEvent>();
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
        public void OnEvent(object sender, InventoryEvent e)
        {
            var inventory = sender as Inventory;
            if(inventory == null) return;
            if(!string.IsNullOrEmpty(_inventoryID) && _inventoryID != inventory.id) return;
            
            switch(e.type){
                case InventoryEvent.Type.Init:{
                    InvokeStackInit(e.stack);
                    break;
                }

                case InventoryEvent.Type.Dispose:{
                    InvokeStackDispose(e.stack);
                    break;
                }
            }
        }

        public void OnEvent(object sender, ItemUseableEvent e)
        {
            //check item
            if(_item.value != e.stack.item) return;

            //Check inventory id
            if(!string.IsNullOrEmpty(_inventoryID) && e.stack.inventory.id != _inventoryID) return;

            switch(e.type){
                case ItemUseableEvent.Type.Use:{
                    InvokeStackUse(e.stack);
                    break;
                }

                case ItemUseableEvent.Type.Unuse:{
                    InvokeStackUnuse(e.stack);
                    break;
                }
            }
        }
        #endregion
    }
}