namespace GloryJam.Inventories
{
    public static class ItemDismantleComponentExtend
    {
        public static bool GetComponentDismantle(this ItemStack stack,out ItemDismantleComponent result){
            result = default;
            return stack != null ? stack.TryGetComponent(out result) : default;
        }
        public static bool GetComponentDismantle(this Item item,out ItemDismantleComponent result){
            result = default;
            return item != null ? item.TryGetComponent(out result) : default;
        }

        public static bool ContainComponentDismantle(this ItemStack stack){
            return stack.ContainComponent<ItemDismantleComponent>();
        }
        public static bool ContainComponentDismantle(this Item item){
            return item.ContainComponent<ItemDismantleComponent>();
        }
    }
}
