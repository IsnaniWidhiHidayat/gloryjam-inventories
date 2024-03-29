using GloryJam.Event;
using UnityEngine;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [AddComponentMenu("Glory Jam/Inventory/Inventory Event Listener")]
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
        private string _inventoryID;
        #endregion

        #region events
        #if ODIN_INSPECTOR
        [BoxGroup(grpEvent)]
        #endif
        public UnityEvent<ItemStack> onStackInit,onStackDispose;
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
        private void InvokeStackDispose(ItemStack stack){
            onStackDispose?.Invoke(stack);
        }
        #endregion

        #region EventListener
        public void OnEvent(object sender, InventoryEvent e)
        {
            //check inventory
            var inventory = sender as Inventory;
            if(inventory == null) return;

            //Check id
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
        #endregion
    }
}