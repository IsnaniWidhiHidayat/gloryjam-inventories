using UnityEngine;
using System;
using System.Collections.Generic;
using GloryJam.DataAsset;
using Codice.CM.WorkspaceServer.Lock;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories{

    [RequireComponent(typeof(Inventory))]
    public class InventoryItemSet : MonoBehaviour {

        #region const
        const string grpConfig = "Config";
        const string grpRequired = "Required";
        const string grpRuntime = "Runtime";
        const string grpEvent = "Events";
        const string grpDebug = "Debug";
        #endregion

        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpRequired),SerializeField]
        #endif
        private Inventory _inventory;

        #if ODIN_INSPECTOR
        [BoxGroup(grpRequired),SerializeField]
        [ListDrawerSettings(DraggableItems = false,Expanded = true)]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        [InlineEditor]
        #endif
        private ItemSetDataAsset[] _itemSet = new ItemSetDataAsset[0];
        #endregion

        #region private
        private Dictionary<Item,ItemSetProgress>[] _itemSetMatch;
        #endregion

        #region methods
        private void OnEnable() {
            _inventory.onItemInit.AddListener(OnItemInit);
            _inventory.onItemDispose.AddListener(OnItemDispose);
        }
        private void OnDisable() {
            _inventory.onItemInit.RemoveListener(OnItemInit);
            _inventory.onItemDispose.RemoveListener(OnItemDispose);
        }
        private void Reset() {
            if(_inventory == null) _inventory = GetComponent<Inventory>();
        }
        #endregion

        #region callback
        private void OnItemInit(ItemStack stack)
        {
            //check stack
            if(stack == null || stack.item == null) return;

            //init item match
            if(_itemSetMatch == null){
                _itemSetMatch = new Dictionary<Item, ItemSetProgress>[_itemSet.Length];
                for (int i = 0; i < _itemSetMatch.Length; i++)
                {
                    var itemSet = _itemSet[i];
                    var itemSetMatch = _itemSetMatch[i] = new Dictionary<Item, ItemSetProgress>();
                    
                    for (int j = 0; j < itemSet.value.items.Length; j++)
                    {
                        var item  = itemSet.value.items[j].item.value;
                        var count = itemSet.value.items[j].count;

                        itemSetMatch[item] = new ItemSetProgress(){
                            current = 0,
                            max = count
                        };
                    }
                }
            }

            for (int i = 0; i < _itemSetMatch.Length; i++)
            {
                var itemSet = _itemSet[i];
                var itemSetMatch = _itemSetMatch[i];

                //check empty
                if(itemSetMatch == null) continue;

                //check empty key
                if(!itemSetMatch.ContainsKey(stack.item)) continue;

                //check max count
                if(itemSetMatch[stack.item].current >= itemSetMatch[stack.item].max) continue;

                //increase amount
                itemSetMatch[stack.item].current++;
                
                //invoke handler
                if(itemSet.value.handler?.Length > 0) {
                    for (int j = 0; j < itemSet.value.handler?.Length; j++)
                    {
                        var handler = itemSet.value.handler[j];
                        if(handler == null) continue;
                        handler.OnItemMatch(itemSetMatch,_inventory);
                    }
                }
            }
        }
        private void OnItemDispose(ItemStack stack)
        {
            for (int i = 0; i < _itemSetMatch.Length; i++)
            {
                var itemSet = _itemSet[i];
                var itemSetMatch = _itemSetMatch[i];

                //check empty
                if(itemSetMatch == null) continue;

                //check empty key
                if(!itemSetMatch.ContainsKey(stack.item)) continue;

                //check current count
                if(itemSetMatch[stack.item].current <= 0) continue;

                //decrease amount
                itemSetMatch[stack.item].current--;

                //invoke handler
                if(itemSet.value.handler?.Length > 0) {
                    for (int j = 0; j < itemSet.value.handler?.Length; j++)
                    {
                        var handler = itemSet.value.handler[j];
                        if(handler == null) continue;
                        handler.OnItemMatch(itemSetMatch,_inventory);
                    }
                }
            }
        }
        #endregion
    }
}