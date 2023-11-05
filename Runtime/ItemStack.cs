using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemStack
    {
        #region const
        const string grpDebug = "Debug";
        #endregion

        #region fields
        [SerializeField]
        #if ODIN_INSPECTOR
        [HideReferenceObjectPicker,ListDrawerSettings(Expanded = true,DraggableItems = false,ListElementLabelName = "ComponentName")]
        #endif
        public List<ItemComponent> component;
        #endregion

        #region property
        public int index => _slot != null ? _slot.GetStackIndex(this) : -1;
        public Item item => _item;
        public ItemSlot slot => _slot;
        public Inventory inventory => slot?.inventory;
        #endregion
        
        #region private
        private ItemSlot _slot;
        private Item _item;
        #endregion

        #region inspector
        #if ODIN_INSPECTOR
        [Button("Use"),FoldoutGroup(grpDebug),ShowIf(nameof(ShowButtonDebug))]
        private void InspectorUse(){
            this.Use();
        }

        [Button("Unuse"),FoldoutGroup(grpDebug),ShowIf(nameof(ShowButtonDebug))]
        private void InspectorUnUse(){
            this.Unuse();
        }

        private bool ShowButtonDebug(){
            return this.ContainComponent<ItemUseableComponent>();
        }
        #endif
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
            
            inventory?.InvokeOnItemInit(this);
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
        public void SaveState(){
            for (int i = 0; i < component.Count; i++)
            {
                if(component[i] == null) continue;
                component[i].SaveState();
            }
        }
        public void LoadState(){
            for (int i = 0; i < component.Count; i++)
            {
                if(component[i] == null) continue;
                component[i].LoadState();
            }
        }
        public void Dispose(){
            var inventory = this.inventory;
            
            _slot.Dispose(this);

            if(component?.Count > 0)
            {   
                for (int i = 0; i < component.Count; i++)
                {
                    component[i].OnDispose();
                }
            }

            inventory?.InvokeOnItemDispose(this);
        }
        #endregion
    }
}