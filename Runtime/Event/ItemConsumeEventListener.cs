using GloryJam.Event;
using UnityEngine;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [AddComponentMenu("Glory Jam/Inventory/Event/Item Consume Event Listener")]
    public class ItemConsumeEventListener : MonoBehaviour, EventListener<ItemConsumeEvent>
    {
        #region const
        protected const string grpConfig = "Config";
        protected const string grpReuired = "Required";
        protected const string grpEvent = "Event";
        protected const string grpFilter = "Filter";
        #endregion

        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpFilter),HideLabel]
        #endif
        public ItemEventFilter filter;
        #endregion

        #region event
        #if ODIN_INSPECTOR
        [BoxGroup(grpEvent)]
        #endif
        public UnityEvent<ItemStack> onConsume;
        #endregion

        #region methods
        private void OnEnable() {
            this.RegisterEvent();
        }
        private void OnDisable() {
            this.UnregisterEvent();
        }
        private void InvokeOnConsume(ItemStack stack){
            onConsume?.Invoke(stack);
        }
        #endregion

        #region callback
        public void OnEvent(object sender,ItemConsumeEvent e){
            var pass = filter.IsPass(sender as Inventory,e.stack);
            if(!pass) return;

            InvokeOnConsume(e.stack);
        }
        #endregion
    }
}