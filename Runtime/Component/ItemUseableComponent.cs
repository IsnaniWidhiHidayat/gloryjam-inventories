using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{   
    #if ODIN_INSPECTOR
    [Serializable,HideReferenceObjectPicker,HideDuplicateReferenceBox]
    [Toggle("Enabled")]
    #endif
    public class ItemUseableComponent : ItemComponent<ItemUsageHandler,ItemUseableComponent>
    {
        #region property
        #if ODIN_INSPECTOR
        [ShowInInspector,BoxGroup(grpRuntime)]
        #endif
        public bool inUse{
            get{
                var result = false;

                for (int i = 0; i < handlers.Count; i++)
                {
                    if(handlers[i] == null)
                        continue;

                    result |= handlers[i].inUse;
                }

                return result;
            }
        }

        public override string ComponentName => "Usage";
        public override int ComponentPropertyOrder => 2;
        #endregion

        #region inspector
        #if ODIN_INSPECTOR
        [Button("Use"),BoxGroup(grpDebug),ShowIf(nameof(ShowButtonDebug))]
        private void InspectorUse(){
            Use();
        }

        [Button("Unuse"),BoxGroup(grpDebug),ShowIf(nameof(ShowButtonDebug))]
        private void InspectorUnUse(){
            Unuse();
        }

        private bool ShowButtonDebug(){
            return stack != null;
        }
        #endif
        #endregion

        #region methods
        public virtual bool Use(){
            var result = true;

            for (int i = 0; i < handlers.Count; i++)
            {
                if(handlers[i] == null)
                    continue;

                result &= handlers[i].Use();
            }

            if(inUse) {
                inventory?.InvokeOnItemUse(_stack);
            }

            inventory?.SaveState();

            return result;
        }
        public virtual bool Unuse(){
            var result = true;

            for (int i = 0; i < handlers.Count; i++)
            {
                if(handlers[i] == null)
                    continue;

                result &= handlers[i].Unuse();
            }
            
            if(!inUse) {
                _stack.slot.inventory?.InvokeOnItemUnuse(_stack);
            }

            _stack.slot.inventory.SaveState();

            return result;
        }

        public override void LoadState()
        {
            var _inUse = inUse;
            
            base.LoadState();

            if(!_inUse && inUse){
                inventory?.InvokeOnItemUse(_stack);
            }
        }
        #endregion
    }

    public static class ItemUseableComponentExtend
    {
        public static bool TryGetComponentUsable(this ItemStack stack,out ItemUseableComponent result){
            result = default;
            return stack != null ? stack.TryGetComponent(out result) : default;
        }
        public static bool TryGetComponentUsable(this Item item ,out ItemUseableComponent result){
            result = default;
            return item != null ? item.TryGetComponent(out result) : default;
        }

        public static bool TryGetComponentsUsable(this ItemStack stack,out ItemUseableComponent[] result){
            result = default;
            return stack != null ? stack.TryGetComponents(out result) : default;
        }
        public static bool TryGetComponentsUsable(this Item item ,out ItemUseableComponent[] result){
            result = default;
            return item != null ? item.TryGetComponents(out result) : default;
        }
    
        public static bool ContainComponentUseable(this ItemStack stack){
            return stack.ContainComponent<ItemUseableComponent>();
        }
        public static bool ContainComponentUseable(this Item item){
            return item.ContainComponent<ItemUseableComponent>();
        }
    
        public static bool InUse(this ItemStack stack){
            if(stack.TryGetComponentsUsable(out var components)){
                var result = false;
                for (int i = 0; i < components.Length; i++)
                {
                    if(components[i] == null) continue;
                    result |= components[i].inUse;
                    if(result) break;
                }
                return result;
            }

            return default;
        }
        public static bool Use(this ItemStack stack){
            if(stack.TryGetComponentsUsable(out var components)){
                var result = false;
                for (int i = 0; i < components.Length; i++)
                {
                    if(components[i] == null) continue;
                    result |= components[i].Use();
                }
                return result;
            }
            return default;
        }
        public static bool Unuse(this ItemStack stack){
            if(stack.TryGetComponentsUsable(out var components)){
                var result = false;
                for (int i = 0; i < components.Length; i++)
                {
                    if(components[i] == null) continue;
                    result |= components[i].Unuse();
                }
                return result;
            }
            return default;
        }
    
        public static bool InUse(this ItemSlot slot) {
            for (int i = 0; i < slot.count; i++)
            {
                if(slot[i].InUse()) return true;
            }
            return default;
        }
        public static bool Use(this ItemSlot slot,int count){
            if(!slot.Available(count)){
                Debug.LogError($"Item {slot.item.name} not available with count : {count}");
                return false;
            }
                
        
            var startIndex = slot.count - 1;
            var result = true;
            for (var i = startIndex; i >= startIndex - (count - 1); i--){
                result |= slot[i].Use();
            }
            return result;
        } 
        public static bool Unuse(this ItemSlot slot,int count){
            if(!slot.Available(count)){
                Debug.LogError($"Item {slot.item.name} not available with count : {count}");
                return false;
            }
                

            var startIndex = slot.count - 1;
            var result = true;
            for (var i = startIndex; i >= startIndex - (count - 1); i--){
                result |= slot[i].Unuse();
            }
            return result;
        }
    }
}