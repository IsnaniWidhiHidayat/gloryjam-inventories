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
    public class ItemSpawnerComponent : ItemComponent<ItemSpawnerComponent,ItemSpawnerHandler>
    {
        #region static
        private static ItemSpawnerEvent Event = new ItemSpawnerEvent();
        #endregion

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

            //Trigger event
            Event.stack = stack;
            ItemSpawnerEvent.Trigger(Event);

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
            ItemSpawnerEvent.Trigger(Event);

            return null;
        }
        #endregion
    }
}
