using UnityEngine;
using GloryJam.SaveLoad;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using GloryJam.DataAsset;

namespace GloryJam.Inventories
{
    [AddComponentMenu("Glory Jam/Inventory/Inventory Save Handler")]
    public class InventorySaveHandler : SaveHandler<InventorySaveData>
    {
        #region fields
        [BoxGroup(grpSaveData),SerializeField]
        private DataReference<InventorySaveData> _saveData;

        [BoxGroup(grpRequired),SerializeField,Required,InlineEditor]
        private InventoryDataAsset _inventoryData;
        #endregion

        #region methods
        public override InventorySaveData Save()
        {
            var inventoryData = _inventoryData;

            _saveData.Reset();
            _saveData.value.maxSlot = inventoryData.value.slots.Length;

            if(inventoryData.value.slots?.Length > 0){
                for (int i = 0; i < inventoryData.value.slots.Length; i++)
                {
                    var slot = inventoryData.value.slots[i];

                    //check empty slot
                    if(slot == null) continue;

                    //check empty stack
                    if(slot.stack == null || slot.stack.Count <= 0) continue;

                    var stackSaveData = default(List<ItemStackSaveData>);

                    for (int j = 0; j < slot.stack.Count; j++)
                    {
                        var stack = slot.stack[j];

                        //check empty stack
                        if(stack == null) continue;

                        //check contain stateComponent
                        if(!stack.TryGetComponentState(out var stateComponent)) continue;

                        //check empty handlers
                        if(stateComponent.handlers == null || stateComponent.handlers.Count <= 0) continue;

                        var stateSaveData = default(List<ItemStateSaveData>);

                        for (int k = 0; k < stateComponent.handlers.Count; k++)
                        {
                            //check empty handler
                            var handler = stateComponent.handlers[k];
                            if(handler == null) continue;

                            //check empty save data
                            var saveData = handler.saveData;
                            if(saveData == null) continue;

                            //initial saveData
                            if(stateSaveData == null) stateSaveData = new List<ItemStateSaveData>();

                            //add save data to list
                            saveData.id = handler.id;
                            stateSaveData.Add(saveData);
                        }

                        //check contain state save data
                        if(stateSaveData?.Count > 0)
                        {
                            //initial stack save data
                            if(stackSaveData == null) stackSaveData = new List<ItemStackSaveData>();

                            //add stack save data
                            stackSaveData.Add(new ItemStackSaveData(){
                                state = stateSaveData
                            });
                        }
                    }

                    //add to save data
                    _saveData.value.items.Add(new ItemSaveData(){
                        id = slot.item.id,
                        index = slot.index,
                        stack = stackSaveData
                    });
                }
            }

            return _saveData.value;
        }

        // private void Reset() {
        //     if(_inventory == null) _inventory.GetComponent<Inventory>();
        // }
        public override string ToString()
        {
            return name;
        }
        #endregion
    }
}

