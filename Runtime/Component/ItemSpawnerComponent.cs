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
        public override int order => 99;
        public override bool showID => false;
        public override bool requiredId => false;
        #endregion

        #region methods
        public GameObject Spawn()
        {
            var stack = this.stack;

            //create instance of stack
            if(stack == null && item != null){
                stack = item.CreateInstance();
            }

            Debug.Log($"[Inventory]{inventory?.name} Spawn {stack?.item?.id}, stack:{stack}");

            //spawn object
            var clone = default(GameObject);
            for (int i = 0; i < handlers.Count; i++)
            {
                if(handlers[i] == null) continue;
                clone = handlers[i].Spawn(stack);
                break;
            }

            //set stack
            var objStack = clone?.GetComponent<IItemObjectStack>();
            if(objStack == null){
                objStack = clone?.AddComponent<ItemObjectStack>();
            }

            if(objStack != null) objStack.stack = stack;

            //Trigger event
            Event.stack  = stack;
            Event.Object = clone;
            ItemSpawnerEvent.Trigger(inventory,Event);

            return clone;
        }
        public GameObject Spawn<T1>() where T1 : ItemSpawnerHandler
        {

            var stack = this.stack;

            //create instance of stack
            if(stack == null && item != null){
                stack = item.CreateInstance();
            }

            Debug.Log($"[Inventory]{inventory?.name} Spawn {stack?.item?.id}, stack:{stack}");

            //spawn object
            var clone = default(GameObject);
            for (int i = 0; i < handlers.Count; i++)
            {
                if(handlers[i] == null) continue;
                if(handlers[i] is T1){
                    clone =  handlers[i].Spawn(stack);
                    break;
                }
            }

            //set stack
            var objStack = clone?.GetComponent<IItemObjectStack>();
            if(objStack == null){
                objStack = clone?.AddComponent<ItemObjectStack>();
            }

            if(objStack != null) objStack.stack = stack;


            //Trigger event
            Event.stack = stack;
            Event.Object = clone;
            ItemSpawnerEvent.Trigger(inventory,Event);

            return clone;
        }
        #endregion
    }
}
