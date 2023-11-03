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
        #region fields
        [SerializeField]
        #if ODIN_INSPECTOR
        [HideReferenceObjectPicker,ListDrawerSettings(Expanded = true,DraggableItems = false,ListElementLabelName = "ComponentName")]
        #endif
        public List<ItemComponent> component;
        #endregion

        #region property
        public int index => _slot != null ? _slot.GetStackIndex(this) : -1;
        public ItemSlot slot => _slot;
        public Inventory inventory => slot?.inventory;
        #endregion
        
        #region private
        private ItemSlot _slot;
        #endregion

        #region methods
        public void Init(ItemSlot slot)
        {
            //set slot
            SetSlot(slot);

            //init component
            if(component?.Count > 0)
            {   
                //remove null
                component.RemoveAll(x => x == null);

                for (int i = 0; i < component.Count; i++)
                {
                    component[i].Init(this);
                }
            }
            
            inventory?.InvokeOnItemInit(this);
        }
        public void Dispose(){
            _slot.Dispose(this);

            if(component?.Count > 0)
            {   
                for (int i = 0; i < component.Count; i++)
                {
                    component[i].Dispose();
                }
            }

            inventory?.InvokeOnItemDispose(this);
        }
        public void SetSlot(ItemSlot slot){
            _slot = slot; 
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
        #endregion
    }
}