using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    #if ODIN_INSPECTOR
    [Serializable,HideReferenceObjectPicker]
    [Toggle("Enabled")]
    #endif
    public class ItemSpawnerComponent : ItemComponent<ItemSpawnerHandler,ItemSpawnerComponent>
    {
        #region property
        public override string ComponentName => "Spawner";
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
            result = stack?.GetComponent<ItemSpawnerComponent>();
            return result != null;
        }
        public static bool TryGetComponentSpawner(this Item item,out ItemSpawnerComponent result){
            result = item?.GetComponent<ItemSpawnerComponent>();
            return result != null;
        }

        public static bool ContainComponentSpawner(this ItemStack stack){
            return stack.ContainComponent<ItemSpawnerComponent>();
        }
        public static bool ContainComponentSpawner(this Item item){
            return item.ContainComponent<ItemSpawnerComponent>();
        }
    
    }
}
