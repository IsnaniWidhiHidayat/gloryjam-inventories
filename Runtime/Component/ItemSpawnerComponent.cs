using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    #if ODIN_INSPECTOR
    [Serializable,HideReferenceObjectPicker,HideDuplicateReferenceBox]
    [Toggle("Enabled")]
    #endif
    [DisallowMultipleItemComponent]
    public class ItemSpawnerComponent : ItemComponent<ItemSpawnerHandler,ItemSpawnerComponent>
    {
        #region property
        public override string ComponentName => "Spawner";

        public override int ComponentPropertyOrder => 99;
        #endregion

        #region methods
        public GameObject Spawn()
        {
            for (int i = 0; i < handlers.Count; i++)
            {
                if(handlers[i] == null) continue;
                return handlers[i].Spawn();
            }

            return null;
        }
        public GameObject Spawn<T1>() where T1 : ItemSpawnerHandler
        {
            for (int i = 0; i < handlers.Count; i++)
            {
                if(handlers[i] == null) continue;
                if(handlers[i] is T1) return handlers[i].Spawn();
            }

            return null;
        }
        #endregion
    }

    public static class ItemSpawnerComponentExtend
    {
        public static bool TryGetComponentSpawner(this ItemStack stack,out ItemSpawnerComponent result){
            result = default;
            return stack != null ? stack.TryGetComponent(out result) : default;
        }
        public static bool TryGetComponentSpawner(this Item item,out ItemSpawnerComponent result){
            result = default;
            return item != null ? item.TryGetComponent(out result) : default;
        }

        public static bool ContainComponentSpawner(this ItemStack stack){
            return stack.ContainComponent<ItemSpawnerComponent>();
        }
        public static bool ContainComponentSpawner(this Item item){
            return item.ContainComponent<ItemSpawnerComponent>();
        }
    
    }
}
