using System;
using UnityEngine;

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemSpawnerHandler : ItemComponentHandler
    {
        public GameObject Object;

        #region methods
        public virtual GameObject Spawn(ItemStack stack){
            var clone = GameObject.Instantiate(Object);

            if(stack != null) {
                var itemObject = clone.GetComponent<ItemObject>();
                if(itemObject == null) itemObject = clone.AddComponent<ItemObject>();
                itemObject.SetStack(stack);
            }
            return clone;
        }
        #endregion
    }
}
