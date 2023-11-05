using System;
using GloryJam.Event;

namespace GloryJam.Inventories
{
    public class InventoryEvent : Event<InventoryEvent>
    {
        #region inner class
        [Serializable]
        public enum Type
        {
            Init,
            Dispose
        }
        #endregion

        #region fields
        public Type type;
        public ItemStack stack;
        #endregion
    }
}