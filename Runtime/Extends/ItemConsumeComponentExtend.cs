namespace GloryJam.Inventories
{
    public static class ItemConsumeComponentExtend
    {
        public static bool TryGetComponentConsume(this ItemStack stack,out ItemConsumeComponent result){
            result = default;
            return stack != null ? stack.TryGetComponent(out result) : default;
        }
        public static bool TryGetComponentConsume(this Item item,out ItemConsumeComponent result){
            result = default;
            return item != null ? item.TryGetComponent(out result) : default;
        }

        public static bool ContainComponentConsume(this ItemStack stack){
            return stack.ContainComponent<ItemConsumeComponent>();
        }
        public static bool ContainComponentConsume(this Item item){
            return item.ContainComponent<ItemConsumeComponent>();
        }
    }
}
