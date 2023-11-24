using UnityEngine;

namespace GloryJam.Inventories
{
    public static class ItemStateViewedHandlerExtend
    {
        public static void SetMarkViewed(this ItemStack stack, bool value){
            if(stack.TryGetComponentState(out var component) && component.TryGetHandler<ItemStateViewedHandler>(out var state)){
                state.isViewed = value;
            }else{
                Debug.LogError($"Item {stack.item.id} didn't have {nameof(ItemStateViewedHandler)}");
            }
        }
        public static void SetMarkViewed(this ItemSlot slot, bool value) {
            for (int i = 0; i < slot.count; i++)
            {
                slot.stack[i]?.SetMarkViewed(value);
            }
        }
        
        public static bool GetMarkViewed(this ItemStack stack){
            if(stack.TryGetComponentState(out var component) && component.TryGetHandler<ItemStateViewedHandler>(out var state)){
                return state.isViewed;
            }else{
                Debug.LogError($"Item {stack.item.id} didn't have {nameof(ItemStateViewedHandler)}");
            }

            return default;
        }   
        public static bool GetMarkViewed(this ItemSlot slot){
            var result = false;
            
            for (int i = 0; i < slot.stack.Count; i++)
            {
                if(slot.stack[i] == null) continue;
                result |= slot.stack[i].GetMarkViewed();
                if(result) break;
            }

            return result;
        }
    }
}