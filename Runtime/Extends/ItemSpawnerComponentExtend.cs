namespace GloryJam.Inventories
{
    public static class ItemSpawnerComponentExtend
    {
        public static bool TryGetComponentSpawner(this ItemStack stack,out ItemSpawnerComponent result){
            result = default;
            return stack != null ? stack.TryGetComponent(out result) : default;
        }
        public static bool TryGetComponentSpawner(this Item item,out ItemSpawnerComponent result){
            result = default;
            return item != null ? item.TryGetComponent(out result) : default;
        }

        public static bool ContainComponentSpawner(this ItemStack stack){
            return stack.ContainComponent<ItemSpawnerComponent>();
        }
        public static bool ContainComponentSpawner(this Item item){
            return item.ContainComponent<ItemSpawnerComponent>();
        }
    
    }
}