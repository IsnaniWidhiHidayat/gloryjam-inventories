namespace GloryJam.Inventories
{
    public static class ItemDetailComponentExtend
    {
        public static bool TryGetComponentDetail(this ItemStack stack,out ItemDetailComponent result){
            result = default;
            return stack != null ? stack.TryGetComponent(out result) : default;
        }
        public static bool TryGetComponentDetail(this Item item,out ItemDetailComponent result){
            result = default;
            return item != null ? item.TryGetComponent(out result) : default;
        }
    
        public static bool ContainComponentDetail(this ItemStack stack){
            return stack.ContainComponent<ItemDetailComponent>();
        }
        public static bool ContainComponentDetail(this Item item){
            return item.ContainComponent<ItemDetailComponent>();
        }
    
    }
}