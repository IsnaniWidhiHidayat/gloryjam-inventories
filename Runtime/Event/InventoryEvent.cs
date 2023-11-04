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
            Use,
            Unuse,
            Dispose
        }
        #endregion

        #region fields
        public Type type;
        public string id;
        public ItemStack stack;
        #endregion
    }
}