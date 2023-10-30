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
        public int index => slot != null ? slot.GetStackIndex(this) : -1;
        public ItemSlot slot => _slot;
        #endregion
        
        #region private
        private ItemSlot _slot;
        #endregion

        #region methods
        public void Init(ItemSlot slot)
        {
            //set slot
            _slot = slot; 

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
            
            slot.inventory?.InvokeOnItemInit(this);
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
            slot.RemoveStack(this);

            if(component?.Count > 0)
            {   
                for (int i = 0; i < component.Count; i++)
                {
                    component[i].Dispose();
                }
            }

            slot.inventory?.InvokeOnItemDispose(this);
        }
        #endregion
    }
}