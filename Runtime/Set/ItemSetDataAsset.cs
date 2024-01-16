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
        #region const
        protected const string grpConfig = "Config";
        protected const string grpRuntime = "Runtime";
        protected const string grpHandler = "Handler";
        #endregion

        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        [TableList(AlwaysExpanded = true)]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        #endif
        public ItemReferenceCount [] items = new ItemReferenceCount [0];

        #if ODIN_INSPECTOR
        [BoxGroup(grpHandler)]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        #endif
        public ItemSetHandler handler;
        #endregion

        #region private
        #if ODIN_INSPECTOR
        [ShowIf(nameof(InspectorShowRuntime))]
        [BoxGroup(grpRuntime),ShowInInspector,ReadOnly]
        [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout,KeyLabel = "Item ID",ValueLabel = "Item Stacks")]
        #endif
        [NonSerialized]
        public Dictionary<string,ItemStack[]> itemTracker;
        #endregion

        #region inspector
        #if ODIN_INSPECTOR
        public bool InspectorShowRuntime(){
            return Application.isPlaying;
        }
        #endif
        #endregion

        #region methods
        private void TryInitialize(){
            if(itemTracker == null){
                itemTracker = new Dictionary<string, ItemStack[]>();
                
                for (int i = 0; i < items.Length; i++)
                {
                    if(items[i] == null) continue;
                    if(items[i].item == null) continue;
                    if(items[i].count <= 0) continue;

                    itemTracker[items[i].item.value.id] = new ItemStack[items[i].count];
                }
            }
        }
        #endregion

        #region callback
        public void OnItemStackInit(ItemStack stack){
            TryInitialize();

            if(itemTracker == null) return;
            if(!itemTracker.ContainsKey(stack.item.id)) return;

            //get empty index from tracker
            var emptyIndex = Array.FindIndex(itemTracker[stack.item.id], (x)=> x == null);

            //no index found
            if(emptyIndex < 0) return;

            //set stack to empty index
            itemTracker[stack.item.id][emptyIndex] = stack;

            //invoke handler
            handler?.OnItemMatch(itemTracker);
        }
        public void OnItemStackDispose(ItemStack stack){
            Debug.Log($"OnItemStackDispose : {stack.item.id}");

            if(itemTracker == null) return;
            if(!itemTracker.ContainsKey(stack.item.id)) return;

            //get index from tracker
            var stackIndex = Array.FindIndex(itemTracker[stack.item.id], (x)=> x == stack);
            
            //no index found
            if(stackIndex < 0) return;

            //set stack to null
            itemTracker[stack.item.id][stackIndex] = null;

            //invoke handler
            handler?.OnItemMatch(itemTracker);
        }
        #endregion
    }

    [CreateAssetMenu(menuName = "Glory Jam/Inventory/Item Set Data")]
    public class ItemSetDataAsset : DataAsset<ItemSetData>
    {
        
    }
}
