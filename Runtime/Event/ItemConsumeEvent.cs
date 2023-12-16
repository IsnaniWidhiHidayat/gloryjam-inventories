using System;
using GloryJam.Event;

namespace GloryJam.Inventories
{
    public class ItemConsumeEvent : Event<ItemConsumeEvent>
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