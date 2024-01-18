using GloryJam.Event;
using UnityEngine;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [AddComponentMenu("Glory Jam/Inventory/Event/Item Useable Event Listener")]
    public class ItemUseableEventListener : MonoBehaviour, EventListener<ItemUseableEvent>
    {
        #region const
        const string grpConfig = "Config";
        const string grpReuired = "Required";
        const string grpEvent = "Event";
        const string grpFilter = "Filter";
        #endregion

        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpFilter)]
        [HideLabel]
        #endif
        public ItemEventFilter filter;
        #endregion

        #region event
        #if ODIN_INSPECTOR
        [BoxGroup(grpEvent)]
        #endif
        public UnityEvent<ItemStack> onUse,onUnuse;
        #endregion

        #region methods
        private void OnEnable() {
            this.RegisterEvent();
        }
        private void OnDisable() {
            this.UnregisterEvent();
        }
        private void InvokeOnUse(ItemStack stack){
            onUse?.Invoke(stack);
        }
        private void InvokeOnUnuse(ItemStack stack){
            onUnuse?.Invoke(stack);
        }
        #endregion

        #region callback
        public void OnEvent(object sender,ItemUseableEvent e){
            var pass = filter.IsPass(sender as Inventory,e.stack);
            if(!pass) return;

            switch(e.type){
                case ItemUseableEvent.Type.Use:{
                    InvokeOnUse(e.stack);
                    break;
                }

                case ItemUseableEvent.Type.Unuse:{
                    InvokeOnUnuse(e.stack);
                    break;
                }
            }
        }
        #endregion
    }
}