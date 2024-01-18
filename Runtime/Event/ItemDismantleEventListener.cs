using GloryJam.Event;
using UnityEngine;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [AddComponentMenu("Glory Jam/Inventory/Event/Item Dismantle Event Listener")]
    public class ItemDismantleEventListener : MonoBehaviour, EventListener<ItemDismantleEvent>
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
        public UnityEvent<ItemStack> onDismantle;
        #endregion

        #region methods
        private void OnEnable() {
            this.RegisterEvent();
        }
        private void OnDisable() {
            this.UnregisterEvent();
        }
        private void InvokeOnDismantle(ItemStack stack){
            onDismantle?.Invoke(stack);
        }
        #endregion

        #region callback
        public void OnEvent(object sender,ItemDismantleEvent e){
            var pass = filter.IsPass(sender as Inventory,e.stack);
            if(!pass) return;

            InvokeOnDismantle(e.stack);
        }
        #endregion
    }
}