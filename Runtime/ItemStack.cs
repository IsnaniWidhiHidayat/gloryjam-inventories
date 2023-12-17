using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemStack : ISort
    {
        #region static
        private static InventoryEvent Event = new InventoryEvent();
        #endregion

        #region const
        const string grpDebug = "Debug";
        const string grpProperty = "Properties";
        #endregion

        #region fields
        [SerializeField]
        #if ODIN_INSPECTOR
        [HideReferenceObjectPicker]
        [ListDrawerSettings(Expanded = true,IsReadOnly = true,ListElementLabelName = "title")]
        #endif
        public List<ItemComponent> component;
        #endregion

        #region property
        #if ODIN_INSPECTOR
        [BoxGroup(grpProperty)]
        [ShowInInspector,DisplayAsString,HideInEditorMode,LabelText("Inventory")]
        #endif
        private string inventoryName => inventory?.id;

        #if ODIN_INSPECTOR
        [BoxGroup(grpProperty)]
        [ShowInInspector,DisplayAsString,HideInEditorMode,LabelText("Slot Index")]
        #endif
        private string slotIndex => slot != null? slot.index.ToString() : default;

        #if ODIN_INSPECTOR
        [BoxGroup(grpProperty)]
        [ShowInInspector,DisplayAsString,HideInEditorMode,LabelText("Stack Index")]
        #endif
        public int index => _slot != null ? _slot.GetStackIndex(this) : -1;

        public Item item => _item;
        public ItemSlot slot => _slot;
        public Inventory inventory => slot?.inventory;
        public bool inUse => this.InUse();
        public string title{
            get{
                var result = $"{index}";

                if(inUse){
                    result += " (In Use)";
                }

                return result;
            }
        }
        #endregion
        
        #region private
        private ItemSlot _slot;
        private Item _item;
        #endregion

        #region inspector
        #if ODIN_INSPECTOR
        [Button("Dispose"),BoxGroup(grpDebug),ButtonGroup(grpDebug + "/Buttons"),ShowIf(nameof(InspectorShowRuntime))]
        private void InspectorDispose(){
            Dispose();
        }
        public bool InspectorShowRuntime(){
            return Application.isPlaying;
        }
        #endif
        #endregion

        #region constructor
        public ItemStack(List<ItemComponent> component){
            this.component = component;
            Sort();
        }
        #endregion

        #region methods
        public void Init()
        {
            if(component?.Count > 0)
            {   
                for (int i = 0; i < component.Count; i++)
                {
                    component[i].OnInit();
                }
            }

            if(component?.Count > 0)
            {   
                for (int i = 0; i < component.Count; i++)
                {
                    component[i].OnPostInit();
                }
            }
            
            Debug.Log($"[Inventory]{inventory?.name} Inventory {inventory?.id} Init {item?.id}, stack:{this}");

            //Trigger event
            Event.type  = InventoryEvent.Type.Init;
            Event.stack = this;
            InventoryEvent.Trigger(inventory,Event);
        }
        public void SetSlot(ItemSlot slot){
            _slot = slot; 

            //init component
            if(component?.Count > 0)
            {   
                for (int i = 0; i < component.Count; i++)
                {
                    component[i].SetStack(this);
                }
            }
        }
        public void SetItem(Item item){
            _item = item;
        }
        public void Sort(){
            //sort component by order
            component?.Sort((x,y) =>{
                if(x == null || y == null) return -1;
                return x.order.CompareTo(y.order);
            });

            if(component?.Count > 0){
                for (int i = 0; i < component.Count; i++)
                {
                    var ISort = component[i] as ISort;
                    if(ISort == null) continue;
                    ISort.Sort();
                }
            }
        }
        public void Dispose(){
            var inventory = this.inventory;
            
            Debug.Log($"[Inventory]{inventory?.name} Inventory {inventory?.id} Dispose {item?.id}, stack:{this}");

            _slot.Dispose(this);

            if(component?.Count > 0)
            {   
                for (int i = 0; i < component.Count; i++)
                {
                    component[i].OnDispose();
                }
            }

            //Trigger event
            Event.type  = InventoryEvent.Type.Dispose;
            Event.stack = this;
            InventoryEvent.Trigger(inventory,Event);
        }
        #endregion

        public override string ToString()
        {
            return $"{{ id:{item?.id}, inventory:{inventoryName}, slotIndex:{slotIndex}, index:{index} }}";
        }
    }
}