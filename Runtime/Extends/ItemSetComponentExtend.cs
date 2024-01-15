namespace GloryJam.Inventories
{
    public static class ItemSetComponentExtend
    {
        public static bool TryGetComponentSet(this ItemStack stack,out ItemSetComponent result){
            result = default;
            return stack != null ? stack.TryGetComponent(out result) : default;
        }
        public static bool TryGetComponentSet(this Item item,out ItemSetComponent result){
            result = default;
            return item != null ? item.TryGetComponent(out result) : default;
        }

        public static bool ContainComponentSet(this ItemStack stack){
            return stack.ContainComponent<ItemSetComponent>();
        }
        public static bool ContainComponentSet(this Item item){
            return item.ContainComponent<ItemSetComponent>();
        }
    }
}
