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
        #region static
        public static ItemSlot[] Create(Item item,int count){
            var result = default(ItemSlot[]);
        
            if(count > 0) {
                var reqSlot = count / item.maxStack;
                var remain  = count % item.maxStack;
                    reqSlot = remain > 0 ? reqSlot + 1 : reqSlot;
    
                if(reqSlot > 0){
                    result = new ItemSlot[reqSlot];

                    for (int i = 0; i < result.Length; i++)
                    {
                        var amount = item.maxStack;

                        if(remain > 0 && i == result.Length - 1){
                            amount = remain;
                        }

                        result[i] = new ItemSlot(item,amount,null);
                    }
                }
            }

            return result;
        }
        #endregion

        #region fields
        [SerializeField]
        #if ODIN_INSPECTOR
        [ShowInInspector,FoldoutGroup("Item"),HideLabel]
        #endif
        protected Item _item;
        #endregion

        #region property
        public Item item => _item;
        public Inventory inventory => _inventory;
        public int count => _stack != null? _stack.Count : 0;
        
        public ItemStack this[int index]
        {
            get { return _stack[index]; }
        }
        #endregion

        #region private
        protected Inventory _inventory;

        #if ODIN_INSPECTOR
        [ShowInInspector,ListDrawerSettings(IsReadOnly = true,Expanded = true),FoldoutGroup("$StackGrpName")]
        #endif
        protected List<ItemStack> _stack;
        #endregion

        #region inspector
        private string StackGrpName{
            get{
                return $"Stack : {count}";
            }
        }
        #endregion

        #region constructor
        public ItemSlot(Item item,int count,Inventory inventory){
            _stack = new List<ItemStack>();
            SetInventory(inventory);
            SetItem(item);
            Add(count);
        }
        // public ItemSlot(Item item,int count,Inventory inventory) : this(item,count)
        // {
        //     SetInventory(inventory);
        // }
        public ItemSlot(Item item,ItemStack[] values){
            _stack = new List<ItemStack>();
            SetItem(item);
            Add(values);
        }
        #endregion

        #region methods
        public void Add(int count = 1){
            for (int i = 0; i < count; i++){
                var newStack = _item.CreateInstance();
                _stack.Add(newStack);
                newStack.Init(this);  
            }
        }
        public void Add(params ItemStack[] items){
            if(items?.Length > 0) {
                for (int i = 0; i < items.Length; i++)
                {
                    _stack.Add(items[i]);
                    items[i].Init(this);
                }
            }
        }
        public void Remove(int count){
            count   = Mathf.Min(count, _stack.Count);

            for (int i = 0; i < count; i++)
            {
                var idx = _stack.Count - 1;
                _stack.RemoveAt(idx);
                _stack[idx].Init(null);
            }
        }
        public void RemoveAll(){
            Remove(count);
        }
        public void RemoveStack(ItemStack stack){
            _stack.Remove(stack);
        }
        public void SetItem(Item item){
            _item = item;
        }
        public void SetInventory(Inventory inventory){
            if(_inventory == inventory) return;

            _inventory = inventory;
            for (int i = 0; i < _stack.Count; i++)
            {
                _stack[i].Init(this);
            }
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
        public ItemStack[] PeekAll(){
            return Peek(count);
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
            _item = null;
            _inventory = null;
            _stack?.Clear();
            _stack = null;
        }
        public void Dispose(int count){
            count = Mathf.Min(count, _stack.Count);
            for (int i = 0; i < count; i++)
            {
                var idx     = _stack.Count - 1;
                var removed = _stack[idx];
                removed.Dispose();
            }
        }    
        public bool ItemAvailable(int count){
            
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