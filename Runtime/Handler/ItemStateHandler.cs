using System;

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemStateHandler : ItemComponentHandler
    {
       public abstract void SaveState();
       public abstract void LoadState();
    }
}
