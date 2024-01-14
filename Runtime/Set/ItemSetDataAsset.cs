using System;
using System.Collections.Generic;
using GloryJam.DataAsset;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemSetData
    {
        #region fields
        #if ODIN_INSPECTOR
        [TableList(AlwaysExpanded = true)]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        #endif
        public ItemReferenceCount [] items = new ItemReferenceCount [0];

        #if ODIN_INSPECTOR
        [ListDrawerSettings(DraggableItems = false,Expanded = true,ListElementLabelName = "title")]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        #endif
        public ItemSetHandler[] handlers = new ItemSetHandler[0];
        #endregion

        #region property
        protected Dictionary<Item,ItemStack[]> _itemTracker;
        #endregion

        #region methods
        private void TryInitialize(){
            if(_itemTracker == null){
                _itemTracker = new Dictionary<Item, ItemStack[]>();
                
                for (int i = 0; i < items.Length; i++)
                {
                    if(items[i] == null) continue;
                    if(items[i].item == null) continue;
                    if(items[i].count <= 0) continue;

                    _itemTracker[items[i].item.value] = new ItemStack[items[i].count];
                }
            }
        }
        #endregion

        #region callback
        public void OnItemStackInit(ItemStack stack){
            Debug.Log($"OnItemStackInit : {stack.item.id}");

            TryInitialize();

            if(_itemTracker == null) return;
            if(!_itemTracker.ContainsKey(stack.item)) return;

            //get empty index from tracker
            var emptyIndex = Array.FindIndex(_itemTracker[stack.item], (x)=> x == null);

            //no index found
            if(emptyIndex < 0) return;

            //set stack to empty index
            _itemTracker[stack.item][emptyIndex] = stack;

            //invoke handler
            for (int i = 0; i < handlers.Length; i++)
            {
                handlers[i]?.OnItemMatch(_itemTracker);
            }
        }
        public void OnItemStackDispose(ItemStack stack){
            Debug.Log($"OnItemStackDispose : {stack.item.id}");

            if(_itemTracker == null) return;
            if(!_itemTracker.ContainsKey(stack.item)) return;

            //get index from tracker
            var stackIndex = Array.FindIndex(_itemTracker[stack.item], (x)=> x == stack);
            
            //no index found
            if(stackIndex < 0) return;

            //set stack to null
            _itemTracker[stack.item][stackIndex] = null;

            //invoke handler
            for (int i = 0; i < handlers.Length; i++)
            {
                handlers[i]?.OnItemMatch(_itemTracker);
            }
        }
        #endregion
    }

    [CreateAssetMenu(menuName = "Glory Jam/Inventory/Item Set Data")]
    public class ItemSetDataAsset : DataAsset<ItemSetData>{}
}
