using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    [DisallowMultipleItemComponent]
    public class ItemSpawnerComponent : ItemComponent<ItemSpawnerComponent,ItemSpawnerHandler>
    {
        #region static
        private static ItemSpawnerEvent Event = new ItemSpawnerEvent();
        #endregion

        #region property
        public override string name => "Spawner";
        public override int propertyOrder => 99;
        #endregion

        #region methods
        public GameObject Spawn()
        {
            for (int i = 0; i < handlers.Count; i++)
            {
                if(handlers[i] == null) continue;
                return handlers[i].Spawn();
            }

            //Trigger event
            Event.stack = stack;
            ItemSpawnerEvent.Trigger(inventory,Event);

            return null;
        }
        public GameObject Spawn<T1>() where T1 : ItemSpawnerHandler
        {
            for (int i = 0; i < handlers.Count; i++)
            {
                if(handlers[i] == null) continue;
                if(handlers[i] is T1) return handlers[i].Spawn();
            }

            //Trigger event
            Event.stack = stack;
            ItemSpawnerEvent.Trigger(inventory,Event);

            return null;
        }
        #endregion
    }
}
