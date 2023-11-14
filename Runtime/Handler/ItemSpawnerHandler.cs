using System;
using UnityEngine;

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemSpawnerHandler : ItemComponentHandler
    {
        #region methods
        public abstract GameObject Spawn(ItemStack stack);
        #endregion
    }
}
