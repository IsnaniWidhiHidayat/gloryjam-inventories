using GloryJam.Event;

namespace GloryJam.Inventories
{
    public class ItemDismantleEvent : Event<ItemDismantleEvent>
    {
        #region fields
        public ItemStack stack;
        #endregion

        public override string ToString()
        {
            return $"{{ stack:{stack} }}";
        }
    }
}