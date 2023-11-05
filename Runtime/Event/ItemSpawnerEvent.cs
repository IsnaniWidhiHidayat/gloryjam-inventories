using System;
using GloryJam.Event;

namespace GloryJam.Inventories
{
    public class ItemSpawnerEvent : Event<ItemSpawnerEvent>
    {
        #region fields
        public ItemStack stack;
        #endregion
    }
}