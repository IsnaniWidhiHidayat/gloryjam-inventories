using System;
using GloryJam.Event;

namespace GloryJam.Inventories
{
    public class ItemUseableEvent : Event<ItemUseableEvent>
    {
        #region inner class
        [Serializable]
        public enum Type
        {
            Use,
            Unuse
        }
        #endregion

        #region fields
        public Type type;
        public ItemStack stack;
        #endregion
    }
}