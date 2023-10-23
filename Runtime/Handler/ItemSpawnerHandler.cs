using System;
using UnityEngine;

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemSpawnerHandler : ItemComponentHandler<ItemSpawnerHandler>
    {
        #region methods
        public abstract GameObject Spawn();
        #endregion
    }
}
