using System;
using GloryJam.Event;
using UnityEngine;

namespace GloryJam.Inventories
{
    public class ItemSpawnerEvent : Event<ItemSpawnerEvent>
    {
        #region fields
        public ItemStack stack;
        public GameObject Object;
        #endregion
    }
}