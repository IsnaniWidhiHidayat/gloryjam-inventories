using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GloryJam.DataAsset;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [AddComponentMenu("Glory Jam/Inventory/" + nameof(Inventory))]
    public class Inventory : 
#if ODIN_INSPECTOR
    SerializedMonoBehaviour
#else
    MonoBehaviour
#endif
    
    {
        #region const
        const string grpConfig = "Config";
        const string grpRequired = "Required";
        const string grpRuntime = "Runtime";
        const string grpEvent = "Events";
        #endregion

        #region static
        private static List<Inventory> _inventorys = new List<Inventory>();
        public static Inventory GetInventoryById(string id){
            return _inventorys.Find(x => x.id == id);
        }
        public static void AddInventory(Inventory inventory){
            _inventorys.Add(inventory);
        }
        public static void RemoveInventory(Inventory inventory){
             _inventorys.Remove(inventory);
        }
        #endregion

        #region inner class
        [Serializable]
        public class ItemPair{
            public DataReference<Item> item;
            public int count;
        }
        #endregion

        #region fields
        [SerializeField]
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        #endif
        protected string _id = "Main";
        
        [SerializeField]
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        #endif
        protected int _maxSlot = 10; 

        #if ODIN_INSPECTOR
        [TableList,BoxGroup(grpConfig)]
        #endif
        public ItemPair[] initialItems;

        #if ODIN_INSPECTOR
        [TableList,BoxGroup(grpConfig)]
        #endif
        public ItemPair[] initialUseItems;

        #if ODIN_INSPECTOR
        [BoxGroup(grpRuntime),SerializeField]
        #endif
        protected DataReference<InventoryData> data;
        #endregion

        #region property
        public string id => _id;
        public int maxSlot => _maxSlot;
        public int length => items != null? items.Length : 0;
        public bool inited => _inited;
        protected ItemSlot[] items {
            get{
                if(data != null){
                    return data.value.items;
                }

                return default;
            }
            set{
                data.value.items = value;
            }
        }
        
        public ItemSlot this[int index]
        {
            get { return  items[index]; }
            private set{
                items[index] = value;
                value?.SetInventory(this);
            }
        }
        #endregion 

        #region events
        #if ODIN_INSPECTOR
        [BoxGroup(grpEvent)]
        #endif
        public UnityEvent<ItemStack> onItemInit,onItemUse,onItemUnuse,onItemDispose;
        #endregion

        #region private
        private bool _inited;
        #endregion

        #region methods
        private void OnEnable() {
            AddInventory(this);
        }
        private void OnDisable() {
            RemoveInventory(this);
        }
        private void Start() {
            Initialize();
        }
        private void Initialize()
        {
            //add default items
            items = new ItemSlot[maxSlot];

            foreach (var item in initialItems)
            {
                if(item == null)
                    continue;

                AddItem(item.item.value,item.count,true);
            }

            //use default item
            foreach (var item in initialUseItems)
            {
                if(item == null)
                    continue;

                var itemSlots = this.GetSlots(item.item.value);
                var useRemain = item.count;
               
                if (itemSlots?.Length > 0 && useRemain > 0)
                {
                    for (int i = 0; i < itemSlots.Length; i++)
                    {
                        if(itemSlots[i].count >= useRemain ){
                            itemSlots[i].Use(useRemain);
                            useRemain = 0;
                        }else{
                            useRemain -= itemSlots[i].count;
                            itemSlots[i].Use(itemSlots[i].count);
                        }
                    }
                }
            }

            _inited = true;
        }
        public bool AddSlot(ItemSlot slot,bool combine){
            if(slot.inventory == this || GetIndexOfSlot(slot) >= 0 ){
                Debug.LogError("Can't add itemslot to this inventory cause owned by same inventory");
                return false;
            }
        
            var r = true;
            var reqSlot = GetRequiredSlot(slot.item,slot.count,combine,out int lst);
            
            if(reqSlot <= GetEmptySlot(out int[] emptySlotIndex)){
                var slots = GetSlots((x) => x != null && x.item == slot.item && (x.count <= slot.item.maxStack));
                if(slots?.Length > 0){
                    for (int i = 0; i < slots.Length ; i++)
                    {
                        if(slot.count <= 0)
                            continue;
    
                        var avail = slots[i].item.maxStack - slots[i].count;
                        if(avail <= 0)
                            continue;

                        if(avail >= slot.count){
                            var stacks = slot.PeekAll();
                            slot.RemoveAll();
                            slot.Dispose();
                            slots[i].Add(stacks);
                        }else if(avail < slot.count){
                            var stacks = slot.Peek(avail);
                            slot.Remove(avail);
                            slots[i].Add(stacks);
                        }
    
                        if(slot.count <= 0)
                            break;
                    }
                }

                //check remain left
                if(slot.count > 0){
                    slot.inventory?.RemoveSlot(slot);
                    this[emptySlotIndex[0]] = slot;
                }
            }else{
                r &= false;
                Debug.LogError($"Required {reqSlot} slot to add item");
            }
            return r;
        }
        public bool AddSlot(ItemSlot[] slots,bool combine){
            if(slots?.Length <= 0)
                return false;

            var result = true;
            if(combine){
                var itemToAdd = default(Dictionary<Item,int>);
                
                //Create Dic Item with count
                for (int i = 0; i < slots.Length ; i++){
                    if(slots[i].inventory == this || GetIndexOfSlot(slots[i]) >= 0 ){
                        Debug.LogError("Can't add itemslot to this inventory cause owned by same inventory");
                        result &= false;
                    }

                    if(!result)
                        break;

                    var key     = slots[i].item;
                    var value   = slots[i].count;
                    
                    if(itemToAdd == null){
                        itemToAdd = new Dictionary<Item, int>();
                    }

                    if(!itemToAdd.ContainsKey(key)){
                        itemToAdd[key] = 0;
                    }
                    
                    itemToAdd[key] += value;
                }
                
                //Check if can add
                if(result){
                    result &= CanAddItem(itemToAdd,combine);
                }

                //Add & combine to exist item
                if(result){
                    for (int i = 0; i < slots.Length ; i++){
                        result &= AddSlot(slots[i],combine);
                    }
                }

                itemToAdd?.Clear();
                itemToAdd = null;
            }else{
                var emptySlotIndex  = default(int[]);
                var emptySlot       = GetEmptySlot(out emptySlotIndex);
                if(emptySlot >= slots.Length) {
                    for (int i = 0; i < slots.Length; i++)
                    {
                        slots[i].inventory?.DisposeSlot(slots[i]);
                        this[emptySlotIndex[i]] = slots[i];
                    }
                    result = true;
                }else{
                    result = false;
                    Debug.LogError($"Required {emptySlot} slot to add items");
                }
            }

            return result;
        }
        public bool AddItem(Item item,int count,bool combine)
        {
            if(count <= 0) return false;
            if(!CanAddItem(item,count,combine)) return false;

            //Check existing item
            var remain  = count;
            if(combine) {
                var items = Array.FindAll(this.items,x => x != null && x.item == item && x.count < x.item.maxStack);
                if(items?.Length > 0){
                    for (var i = 0; i < items.Length ;i++)
                    {
                        if(remain <= 0) continue;
                        var avail = items[i].item.maxStack - items[i].count;
                        var add = 0;
                        
                        if(avail >= remain){
                            add = remain;
                            remain = 0;
                        }else if(avail < remain){
                            remain -= avail;
                            add = avail;
                        }

                        for (int j = 0; j < add; j++) items[i].Add();
                        if(remain <= 0) break;
                    }
                }
            }

            if(remain > 0){
                for (int i = 0; i < this.items.Length; i++)
                {
                    if(this.items[i] != null) continue;

                    var addCount = remain - item.maxStack > 0 ? item.maxStack : remain;
                    this.items[i] = new ItemSlot(item,addCount,this);
                    remain -= addCount;

                    if(remain <= 0) break;
                }
            }

            return remain == 0;
        }
        public bool AddItem(Dictionary<Item,int> items,bool combine){
            var reqSlot = 0;
            var remain  = 0;
           
            foreach (var v in items){
                reqSlot += GetRequiredSlot(v.Key,v.Value,combine,out remain);
            }

            if(reqSlot <=  GetEmptySlot()){
                var result = true;
                foreach (var v in items){
                    result &= AddItem(v.Key,v.Value,combine);
                }
                return result;
            }else{
                Debug.LogError($"Required {reqSlot} slot to add item");

                return false;
            }
        }
        public bool DisposeItem(params Item[] items){
            if(items?.Length > 0){
                for (int i = 0; i < items.Length; i++)
                {
                    var slots = GetSlots(items[i]);
                    if(slots?.Length > 0) {
                        for (int j = 0; j < slots.Length; j++)
                        {
                            slots[j]?.Dispose();
                        }
                    }
                }

                return true;
            }

            return false;
        }       
        public bool DisposeItem(Item item,int count)
        {
            if(count <= 0)
                return false;

            //Check existing item
            var slots  = GetSlots(item);
            
            //Check existing item
            if(slots?.Length> 0){
                var remain  = count;
                for (var i = 0; i < slots.Length; i++)
                {
                    if(remain <= 0)
                        continue;

                    if(remain >= slots[i].count){
                        DisposeSlot(slots[i]);
                    }else if(remain < slots[i].count){
                        slots[i].Dispose(remain);
                        remain = 0;
                    }
                }

                return remain <= 0;
            }

            return false;
        }     
        public bool DisposeSlot(params ItemSlot[] slots){
            if(slots?.Length > 0) {
                for (int i = 0; i < slots.Length; i++)
                {
                    var index = GetIndexOfSlot(slots[i]);
                    if(index >= 0){
                        items[index] = null;
                        slots[i].Dispose();
                        slots[i].SetInventory(null);
                    }
                }

                return true;
            }

            return false;
        }
        public bool DisposeSlot(ItemSlot slot,int count){
            count = Mathf.Clamp(count,0,slot.item.maxStack);
            if(count <= 0)
                return false;

            if(count >= slot.count){
                return DisposeSlot(slot);
            }else{
                slot.Dispose(count);
                return true;
            }
        }     
        public bool RemoveSlot(params ItemSlot[] slots){
            if(slots?.Length > 0) {
                for (int i = 0; i < slots.Length; i++)
                {
                    var index = GetIndexOfSlot(slots[i]);
                    if(index >= 0){
                        items[index] = null;
                    }
                }

                return true;
            }

            return false;
        }
        public int GetEmptySlot(out int[] index){
            index = default(int[]);
            
            var tmp = default(List<int>);
            for (int i = 0; i < items.Length ; i++)
            {
                if(items[i] != null)
                    continue;

                if(tmp == null){
                    tmp = new List<int>();
                }

                tmp.Add(i);
            }

            index = tmp?.ToArray();
            tmp?.Clear();
            tmp = null;
            return index != null ? index.Length : 0;
        }
        public int GetEmptySlot(){
            var counter = 0;
            for (int i = 0; i < items.Length ; i++)
            {
                if(items[i] != null)
                    continue;

                counter++;
            }
            return counter;
        }
        public int GetIndexOfSlot(ItemSlot item){
            return Array.IndexOf(this.items,item);
        }
        public int GetItemCount(Item item){
            var itemSlot = GetSlots(item);
            var counter = 0;
            
            if(itemSlot?.Length > 0 ){   
                for (int i = 0; i < itemSlot.Length; i++){
                    counter += itemSlot[i].count;
                }
            }

            return counter;
        }
        public int GetRequiredSlot(Item item,int count,bool combine,out int lastRemain){
            lastRemain = 0;
            var result = 0;

            if(count <= 0)
                return result;

            var remain = count;

            if(combine){
                //Check existing item
                var items  = Array.FindAll(this.items,x => x != null && x.item == item && x.count < x.item.maxStack);
                if(item != null && items.Length > 0){
                    for (var i = 0; i < items.Length ;i++)
                    {
                        if(remain <= 0)
                            continue;

                        var avail = items[i].item.maxStack - items[i].count;
                        if(avail >= remain){
                            remain = 0;
                        }else if(avail < remain){
                            remain -= avail;
                        }

                        if(remain <= 0)
                            break;
                    }
                }
            }

            if(remain > 0){
                result   = remain / item.maxStack;
                lastRemain   = remain % item.maxStack;
                result   = lastRemain > 0 ? result + 1 : result;
            }
            
            return result;
        }
        public ItemSlot GetSlot(Item item){
            return GetSlot((x)=> x!= null && x.item == item);
        }
        public ItemSlot GetSlot(Predicate<ItemSlot> predicate){
            return Array.Find(items,predicate);
        }
        public ItemSlot[] GetSlots(Item item){
            if(item == null)
                return default(ItemSlot[]);

            return Array.FindAll(items,(x) => x != null && x.item == item);
        }
        public ItemSlot[] GetSlots(Predicate<ItemSlot> predicate){
            return Array.FindAll(items,predicate);
        }
        // public ItemSlot GetSlotWithUsable<T>() where T: class,IItemUseable
        // {
        //     return GetSlot(x => x != null && x.ContainUseableType<T>());
        // }
        // public ItemSlot[] GetSlotsWithUsable<T>() where T: class,IItemUseable
        // {
        //     return GetSlots(x => x != null && x.ContainUseableType<T>());
        // }
        public bool ContainEmptySlot(out int index){
            index = -1;

            for (int i = 0; i < items.Length; i++)
            {
                if(items[i] == null){
                    index = i;
                    return true;
                }
                    
            }

            return false;
        }
        public bool ContainEmptySlot(){
            for (int i = 0; i < items.Length; i++)
            {
                if(items[i] == null)
                    return true;
            }

            return false;
        }
        public bool CanAddItem(Item item, int count,bool combine){
            var remain  = default(int);
            var reqSlot = GetRequiredSlot(item,count,combine,out remain);
            return reqSlot <= GetEmptySlot();
        }
        public bool CanAddItem(Dictionary<Item,int> items,bool combine){
            var reqSlot = 0;
            var remain  = 0;
            var result  = true;  
            foreach (var v in items){
                reqSlot += GetRequiredSlot(v.Key,v.Value,combine,out remain);
            }

            result &= reqSlot <=  GetEmptySlot();

            if(!result){
                Debug.LogError($"Required {reqSlot} slot to add item");
            }

            return result;
        }
        public bool SplitSlot(ItemSlot slot,int splitSize){
            if(splitSize <= 0)
                return false;
            
            if(slot.count <= splitSize){
                Debug.LogError("Split failed, item count is less than desire");
                return false;
            }
            
            if(GetIndexOfSlot(slot) < 0){
                Debug.LogError("Split failed, item is not part of inventory");
                return false;
            }
            
            var splitQuantities = new List<int>();
            var quantity = slot.count;
            
            while (quantity >= splitSize){
                splitQuantities.Add(splitSize);
                quantity -= splitSize;
            }
            
            if (quantity > 0){
                splitQuantities.Add(quantity);
            }

            var reqSlot = splitQuantities.Count - 1;

            if(reqSlot <= GetEmptySlot(out var emptySlotIndex)){
                for (int i = 1; i < splitQuantities.Count; i++)
                {
                    var stacks = slot.Peek(splitQuantities[i]);
                    slot.Remove(splitQuantities[i]);
                    items[emptySlotIndex[i-1]] = new ItemSlot(slot.item,stacks);
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
            
            if(slot.count <= count){
                Debug.LogError("Split failed, item count is less than desire");
                return false;
            }
            
            if(GetIndexOfSlot(slot) < 0){
                Debug.LogError("Split failed, item is not part of inventory");
                return false;
            }

            if(GetEmptySlot(out var emptySlotIndex) > 0){
                var stacks = slot.Peek(count);
                slot.Remove(count);
                items[emptySlotIndex[0]] = new ItemSlot(slot.item,stacks);
                return true;
            }else{
                Debug.LogError($"Required 1 slot to split item");
                return false;
            }
        }
        public void Sort(){
            Array.Sort(this.items,(x,y)=>{
                var result = 0;
                if(x.item == y.item){
                    result = y.count.CompareTo(x.count);
                }else{
                    result = x.item.id.CompareTo(y.item.id);
                }
                return result;
            });
        }
        
        public void InvokeOnItemInit(ItemStack stack){
            onItemInit?.Invoke(stack);
        }
        public void InvokeOnItemUse(ItemStack stack){
            onItemUse?.Invoke(stack);
        }
        public void InvokeOnItemUnuse(ItemStack stack){
            onItemUnuse?.Invoke(stack);
        }
        public void InvokeOnItemDispose(ItemStack stack){
            onItemDispose?.Invoke(stack);
        }
        #endregion
        
        #region callback
        private void OnDestroy() {
            onItemInit      = null;
            onItemUse       = null;
            onItemUnuse     = null;
            onItemDispose   = null;
        }
        #endregion
    }
}