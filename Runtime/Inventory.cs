using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GloryJam.DataAsset;
using GloryJam.Event;


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
    , EventListener<InventoryEvent>,EventListener<ItemUseableEvent>
    {
        #region const
        const string grpConfig = "Config";
        const string grpRequired = "Required";
        const string grpRuntime = "Runtime";
        const string grpEvent = "Events";
        const string grpDebug = "Debug";
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

        [SerializeField]
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        [ShowIf(nameof(InspectorShowLoadState))]
        #endif
        protected bool _loadState;

        #if ODIN_INSPECTOR
        [TableList,BoxGroup(grpConfig)]
        #endif
        public ItemPair[] initialItems;

        #if ODIN_INSPECTOR
        [TableList,BoxGroup(grpConfig)]
        #endif
        public ItemPair[] initialUseItems;

        #if ODIN_INSPECTOR
        [BoxGroup(grpRuntime),SerializeField,HideLabel]
        #endif
        protected DataReference<InventoryData> data;
        #endregion

        #region property
        public string id => _id;
        public int count => slots != null? slots.Length : 0;
        public bool inited => _inited;
        protected ItemSlot[] slots {
            get{
                if(data != null){
                    return data.value.slots;
                }

                return default;
            }
            set{
                data.value.slots = value;
            }
        }
        
        public ItemSlot this[int index]
        {
            get { return  slots[index]; }
            set{
                //if(slots[index] != null) value?.SetInventory(null);
                slots[index] = value;
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

        #region inspector
        #if ODIN_INSPECTOR
        private bool InspectorShowLoadState(){
            return data.useReference;
        }

        [SerializeField,BoxGroup(grpDebug),LabelText("Item")]
        private ItemAsset _debugItem;

        [SerializeField,BoxGroup(grpDebug),LabelText("Count")]
        private int _debugCount;

        [Button("Add Item"),BoxGroup(grpDebug)]
        private void DebugAddItem(){
            AddItem(_debugItem.value,_debugCount,true);
        }

        [Button("Dispose Item"),BoxGroup(grpDebug)]
        private void DebugDisposeItem(){
            DisposeItem(_debugItem.value,_debugCount);
        }
        #endif
        #endregion

        #region methods
        private void OnEnable() {
            AddInventory(this);
            this.RegisterEvent<InventoryEvent>();
            this.RegisterEvent<ItemUseableEvent>();
        }
        private void OnDisable() {
            RemoveInventory(this);
            this.UnregisterEvent<InventoryEvent>();
            this.UnregisterEvent<ItemUseableEvent>();
        }
        
        private void Start() {
            Initialize();
        }
        private void Initialize()
        {
            if(slots == null) slots = new ItemSlot[_maxSlot];

            //add default items
            if(slots.Length != _maxSlot) Array.Resize(ref data.value.slots,_maxSlot);
            
            //set inventory
            for (int i = 0; i < slots.Length; i++)
            {
                if(slots[i] == null) continue;
                slots[i]?.SetInventory(this);
            }

            //load state
            if(data.useReference && _loadState) LoadState();

            foreach (var item in initialItems)
            {
                if(item == null)
                    continue;
                
                var count = item.count - GetItemCount(item.item.value);
                if(count <= 0) continue;

                AddItem(item.item.value,count,true);
            }

            //use default item
            foreach (var item in initialUseItems)
            {
                if(item == null)
                    continue;

                var itemSlots = GetSlots(item.item.value);
                var useRemain = item.count;
               
                if (itemSlots?.Length > 0 && useRemain > 0)
                {
                    for (int i = 0; i < itemSlots.Length; i++)
                    {
                        if(itemSlots[i].InUse()) continue;

                        if(itemSlots[i].count >= useRemain ){
                            itemSlots[i].Use(useRemain);
                            useRemain = 0;
                            break;
                        }else{
                            useRemain -= itemSlots[i].count;
                            itemSlots[i].Use(itemSlots[i].count);
                        }
                    }
                }
            }

            _inited = true;
        }
        
        public bool AddItem(Item item,int count,bool combine)
        {
            if(count <= 0) return false;
            if(!CanAddItem(item,count,combine)) return false;

            //Check existing item
            var remain  = count;
            if(combine) {
                var items = Array.FindAll(this.slots,x => x != null && x.item == item && x.count < x.item.maxStack);
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
                for (int i = 0; i < this.slots.Length; i++)
                {
                    if(this.slots[i] != null) continue;

                    var addCount = remain - item.maxStack > 0 ? item.maxStack : remain;
                    this.slots[i] = new ItemSlot(item,addCount,this);
                    remain -= addCount;

                    if(remain <= 0) break;
                }
            }

            return remain == 0;
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
                        remain -= slots[i].count;
                        slots[i].Dispose();
                    }else if(remain < slots[i].count){
                        slots[i].Dispose(remain);
                        remain = 0;
                    }
                }

                return remain <= 0;
            }

            return false;
        }     
        
        public int GetEmptySlot(){
            var counter = 0;
            for (int i = 0; i < slots.Length ; i++)
            {
                if(slots[i] != null)
                    continue;

                counter++;
            }
            return counter;
        }
        public int GetEmptySlot(out int[] index){
            index = default;
            
            var tmp = default(List<int>);
            for (int i = 0; i < slots.Length ; i++)
            {
                if(slots[i] != null)
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
        
        public int GetIndexOfSlot(ItemSlot item){
            return Array.IndexOf(this.slots,item);
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
                var items  = Array.FindAll(this.slots,x => x != null && x.item == item && x.count < x.item.maxStack);
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
            return Array.Find(slots,predicate);
        }
        
        public ItemSlot[] GetSlots(Item item){
            if(item == null)
                return default(ItemSlot[]);

            return Array.FindAll(slots,(x) => x != null && x.item == item);
        }
        public ItemSlot[] GetSlots(Predicate<ItemSlot> predicate){
            return Array.FindAll(slots,predicate);
        }

        public bool ContainEmptySlot(){
            for (int i = 0; i < slots.Length; i++)
            {
                if(slots[i] == null)
                    return true;
            }

            return false;
        }
        public bool ContainEmptySlot(out int index){
            index = -1;

            for (int i = 0; i < slots.Length; i++)
            {
                if(slots[i] == null){
                    index = i;
                    return true;
                }
                    
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
        
        public void Sort(){
            Array.Sort(this.slots,(x,y)=>{
                var result = 0;
                if(x.item == y.item){
                    result = y.count.CompareTo(x.count);
                }else{
                    result = x.item.id.CompareTo(y.item.id);
                }
                return result;
            });
        }
        public void SaveState(){
            if(slots == null) return;

            for (int i = 0; i < slots.Length; i++)
            {
                slots[i]?.SaveState();
            }
        }
        public void LoadState(){
            if(slots == null) return;

            for (int i = 0; i < slots.Length; i++)
            {
                slots[i]?.LoadState();
            }
        }

        // public void InvokeOnItemInit(ItemStack stack){
        //     onItemInit?.Invoke(stack);
        //     InventoryEvent.Trigger(new InventoryEvent(){
        //         id = _id,
        //         type = InventoryEvent.Type.Init,
        //         stack = stack,
        //     });
        // }
        // public void InvokeOnItemUse(ItemStack stack){
        //     onItemUse?.Invoke(stack);
        //     InventoryEvent.Trigger(new InventoryEvent(){
        //         id = _id,
        //         type = InventoryEvent.Type.Use,
        //         stack = stack,
        //     });
        // }
        // public void InvokeOnItemUnuse(ItemStack stack){
        //     onItemUnuse?.Invoke(stack);
        //     InventoryEvent.Trigger(new InventoryEvent(){
        //         id = _id,
        //         type = InventoryEvent.Type.Unuse,
        //         stack = stack,
        //     });
        // }
        // public void InvokeOnItemDispose(ItemStack stack){
        //     onItemDispose?.Invoke(stack);
        //     InventoryEvent.Trigger(new InventoryEvent(){
        //         id = _id,
        //         type = InventoryEvent.Type.Dispose,
        //         stack = stack,
        //     });
        // }
        #endregion
        
        #region callback
        private void OnDestroy() {
            onItemInit      = null;
            onItemUse       = null;
            onItemUnuse     = null;
            onItemDispose   = null;

            //set all slot inventory to null
            if(slots?.Length > 0){
                for (int i = 0; i < slots.Length; i++)
                {
                    slots[i]?.SetInventory(null);
                }
            }
        }
        public void OnEvent(InventoryEvent Event)
        {
           switch(Event.type){
                case InventoryEvent.Type.Init:{
                    onItemInit?.Invoke(Event.stack);
                    break;
                }

                case InventoryEvent.Type.Dispose:{
                    onItemDispose?.Invoke(Event.stack);
                    break;
                }
           }
        }
        public void OnEvent(ItemUseableEvent Event)
        {
            switch(Event.type){
                case ItemUseableEvent.Type.Use:{
                    onItemUse?.Invoke(Event.stack);
                    break;
                }

                case ItemUseableEvent.Type.Unuse:{
                    onItemUnuse?.Invoke(Event.stack);
                    break;
                }
            }
        }
        #endregion
    }
}