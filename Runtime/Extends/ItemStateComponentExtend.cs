namespace GloryJam.Inventories
{
    public static class ItemStateComponentExtend 
    {
        public static bool TryGetComponentState(this ItemStack stack,out ItemStateComponent result){
            result = default;
            return stack != null ? stack.TryGetComponent(out result) : default;
        }
        public static bool TryGetComponentState(this Item item,out ItemStateComponent result){
            result = default;
            return item != null ? item.TryGetComponent(out result) : default;
        }

        public static bool ContainComponentState(this ItemStack stack){
            return stack.ContainComponent<ItemStateComponent>();
        }
        public static bool ContainComponentState(this Item item){
            return item.ContainComponent<ItemStateComponent>();
        }

        public static void SaveState(this ItemStack stack){
            //use item
            if(stack.TryGetComponentState(out var component)){
                component.SaveState();
            }
        }
        public static void LoadState(this ItemStack stack){
            //unuse item
            if(stack.TryGetComponentState(out var component)){
                component.LoadState();
            }
        }
        
        public static void SaveState(this ItemSlot slot){
            for (int i = 0; i < slot.stack.Count; i++)
            {
                slot.stack[i]?.SaveState();
            }
        }
        public static void LoadState(this ItemSlot slot){
            for (int i = 0; i < slot.stack.Count; i++)
            {
                slot.stack[i]?.LoadState();
            }
        }
        
        public static void SaveState(this Inventory inventory){
            if(inventory == null) return;
            if(inventory.slots == null) return;

            for (int i = 0; i < inventory.slots.Length; i++)
            {
                inventory.slots[i]?.SaveState();
            }
        }
        public static void LoadState(this Inventory inventory){
            if(inventory == null) return;
            if(inventory.slots == null) return;
            
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                inventory.slots[i]?.LoadState();
            }
        }
        
    }
}
