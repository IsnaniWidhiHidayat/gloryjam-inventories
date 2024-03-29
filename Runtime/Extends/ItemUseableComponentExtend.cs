using System;
using UnityEngine;

namespace GloryJam.Inventories
{
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
            var result = false;
            
            //use item
            if(stack.TryGetComponentsUsable(out var components)){
                for (int i = 0; i < components.Length; i++)
                {
                    if(components[i] == null) continue;
                    result |= components[i].Use();
                }
            }

            return result;
        }
        public static bool Unuse(this ItemStack stack){
            var result = false;

            //unuse item
            if(stack.TryGetComponentsUsable(out var components)){
                for (int i = 0; i < components.Length; i++)
                {
                    if(components[i] == null) continue;
                    result |= components[i].Unuse();
                }
            }
            
            return result;
        }
        
        public static bool InUse(this ItemSlot slot) {
            for (int i = 0; i < slot.count; i++)
            {
                if(slot.stack[i] == null) continue;
                if(slot.stack[i].InUse()) return true;
            }
            return default;
        }
        public static bool Use(this ItemSlot slot,int count,Func<ItemStack,bool> condition = null){
            if(!slot.Available(count)){
                Debug.LogError($"Item {slot.item.name} not available with count : {count}");
                return false;
            }
                
        
            var startIndex = slot.count - 1;
            var result = true;
            for (var i = startIndex; i >= startIndex - (count - 1); i--){
                if(slot.stack[i] == null) continue;
                if(condition != null && !condition(slot.stack[i])) continue;
                result |= slot.stack[i].Use();
            }
            return result;
        } 
        public static bool Unuse(this ItemSlot slot,int count,Func<ItemStack,bool> condition = null){
            if(!slot.Available(count)){
                Debug.LogError($"Item {slot.item.name} not available with count : {count}");
                return false;
            }
                

            var startIndex = slot.count - 1;
            var result = true;
            for (var i = startIndex; i >= startIndex - (count - 1); i--){
                if(slot.stack[i] == null) continue;
                if(condition != null && !condition(slot.stack[i])) continue;
                result |= slot.stack[i].Unuse();
            }
            return result;
        }
    }
}