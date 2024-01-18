using GloryJam.Event;
using UnityEngine;
using UnityEngine.Events;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [AddComponentMenu("Glory Jam/Inventory/Event/Item Spawner Event Listener")]
    public class ItemSpawnerEventListener : MonoBehaviour, EventListener<ItemSpawnerEvent>
    {
        #region const
        protected const string grpConfig = "Config";
        protected const string grpReuired = "Required";
        protected const string grpEvent = "Event";
        protected const string grpFilter = "Filter";
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
        public UnityEvent<ItemStack> onSpawn;
        #endregion

        #region methods
        private void OnEnable() {
            this.RegisterEvent();
        }
        private void OnDisable() {
            this.UnregisterEvent();
        }
        private void InvokeOnSpawn(ItemStack stack){
            onSpawn?.Invoke(stack);
        }
        #endregion

        #region callback
        public void OnEvent(object sender,ItemSpawnerEvent e){
            var pass = filter.IsPass(sender as Inventory,e.stack);
            if(!pass) return;

            InvokeOnSpawn(e.stack);
        }
        #endregion
    }
}