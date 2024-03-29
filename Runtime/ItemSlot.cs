using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemSlot : IDisposable
    {
        #region fields
        #if ODIN_INSPECTOR
        [ShowInInspector,HideLabel,HideDuplicateReferenceBox,HideReferenceObjectPicker]
        #endif
        protected Item _item;
        #endregion

        #region property
        public Item item => _item;
        public Inventory inventory => _inventory;
        public int index => inventory != null ? inventory.GetIndexOfSlot(this) : -1;
        public int count => _stack != null? _stack.Count : 0;
        public List<ItemStack> stack => _stack;
        public bool inUse => this.InUse();
        public string name {
            get{
                return _item != null ? _item.name : string.Empty;
            }
        }
        public virtual string title{
            get{
                var inUse  = this.inUse;
                var result = $"{name} : {count}";

                if(inUse){
                    result += " (In Use)";
                }

                return result;
            }
        }
        #endregion

        #region private
        protected Inventory _inventory;

        #if ODIN_INSPECTOR
        [ShowInInspector,ListDrawerSettings(IsReadOnly = true,Expanded = true,ListElementLabelName = "title")]
        [HideDuplicateReferenceBox,HideReferenceObjectPicker]
        #endif
        protected List<ItemStack> _stack;
        #endregion

        #region constructor
        public ItemSlot(Item item,Inventory inventory){
            _stack = new List<ItemStack>();
            _item = item;
            SetInventory(inventory);
        }
        #endregion

        #region methods
        // public void Init(){
        //     if(_stack?.Count > 0) 
        //     {
        //         for (int i = 0; i < _stack.Count; i++)
        //         {
        //             _stack[i]?.Init();
        //         }
        //     }
        // }
        public void Add(int count = 1){
            count = Mathf.Clamp(count,0,_item.maxStack);
            if(count <= 0) return;
            
            for (int i = 0; i < count; i++){
                Add(_item.CreateInstance());
            }
        }
        public void Add(params ItemStack[] itemStacks){
            if(itemStacks?.Length > 0) {
                for (int i = 0; i < itemStacks.Length; i++)
                {
                    _stack.Add(itemStacks[i]);
                    itemStacks[i].SetSlot(this);
                    //itemStacks[i].Init();
                }
            }
        }
        
        public void SetInventory(Inventory inventory){
            _inventory = inventory;

            if(_stack?.Count > 0) 
            {
                for (int i = 0; i < _stack.Count; i++)
                {
                    _stack[i].SetSlot(this);
                }
            }
        }  
        
        public ItemStack[] Peek(){
            return Peek(count);
        }
        public ItemStack[] Peek(int count){
            var result = default(ItemStack[]);
                count  = Mathf.Min(count, _stack.Count);

            if(count > 0){
                result = new ItemStack[count];

                for (int i = 0; i < count; i++)
                {
                    var stack = _stack[_stack.Count - 1 - i];
                    result[result.Length - 1 - i] = stack;
                }
            }

            return result;
        }      
        
        public bool SplitSlot(int splitSize){
            if(splitSize <= 0)
                return default;

            if(_inventory == null){
                Debug.LogError("Slot is not part of inventory");
                return default;
            }
            
            if(count <= splitSize){
                Debug.LogError("Split failed, item count is less than desire");
                return default;
            }
            
            if(index < 0){
                Debug.LogError("Split failed, item is not part of inventory");
                return default;
            }
            
            var splitQuantities = new List<int>();
            var quantity = count;
            
            while (quantity >= splitSize){
                splitQuantities.Add(splitSize);
                quantity -= splitSize;
            }
            
            if (quantity > 0){
                splitQuantities.Add(quantity);
            }

            var reqSlot = splitQuantities.Count - 1;

            if(reqSlot <= _inventory.GetEmptySlot(out var emptySlotIndex)){
                for (int i = 1; i < splitQuantities.Count; i++)
                {
                    var stacks = Peek(splitQuantities[i]);
                    
                    //remove from old slot
                    for (int j = 0; j < stacks.Length; j++)
                    {
                        if(stacks[j] == null) continue;
                        _stack.Remove(stacks[j]);
                    }

                    //create new slot fill with existing stack
                    var newSlot = new ItemSlot(_item,_inventory);
                        newSlot.Add(stacks);

                    _inventory.slots[emptySlotIndex[i-1]] = newSlot;
                }
                return true;
            }else{
                Debug.LogError($"Required {reqSlot} slot to split item");
                return false;
            }
            
        }
        public bool SplitSlotBy(ItemSlot slot,int count){
            if(count <= 0)
                return false;
            
            if(_inventory == null){
                Debug.LogError("Slot is not part of inventory");
                return default;
            }

            if(slot.count <= count){
                Debug.LogError("Split failed, item count is less than desire");
                return false;
            }
            
            if(index < 0){
                Debug.LogError("Split failed, item is not part of inventory");
                return false;
            }

            if(_inventory.GetEmptySlot(out var emptySlotIndex) > 0){
                var stacks = slot.Peek(count);
                //remove from old slot
                for (int j = 0; j < stacks.Length; j++)
                {
                    if(stacks[j] == null) continue;
                    _stack.Remove(stacks[j]);
                }

                //create new slot fill with existing stack
                var newSlot = new ItemSlot(_item,_inventory);
                    newSlot.Add(stacks);

                _inventory.slots[emptySlotIndex[0]] = newSlot;
                return true;
            }else{
                Debug.LogError($"Required 1 slot to split item");
                return false;
            }
        }
        
        public ItemStack GetFirst(){
            return _stack[0];
        }
        public ItemStack GetLast(){
            return _stack[_stack.Count - 1];
        }     
        
        public int GetStackIndex(ItemStack stack){
            return _stack.IndexOf(stack);
        }
        
        public void Dispose()
        {
            Dispose(count);
        }
        public void Dispose(int count){
            count = Mathf.Min(count, this.count);
            for (int i = 0; i < count; i++)
            {
                var idx = this.count - 1;
                _stack[idx]?.Dispose();
            }
        }
        public void Dispose(params ItemStack[] stack){
            if(stack?.Length > 0){
                for (int i = 0; i < stack.Length; i++)
                {
                    if(stack[i] == null) continue;
                    _stack.Remove(stack[i]);
                    stack[i].SetSlot(null);
                }
            }

            //remove slot from invenory if empty
            if(count <= 0){
                if(index >= 0){
                    _inventory.slots[index] = null;
                }

                _item = item;
                SetInventory(null);
            }
        }

        public bool Available(int count){
            
            if(count <= 0){
                return false;
            }
            
            if(this.count <= 0){
                return false;
            }

            if(count > this.count){
                return false;
            }

            return true;
        }
        #endregion
    }
}