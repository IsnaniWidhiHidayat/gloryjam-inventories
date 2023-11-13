using UnityEngine;
using Sirenix.OdinInspector;

namespace GloryJam.Inventories
{
    public class TestInventory : MonoBehaviour {
        public Inventory inventory_1,inventory_2;

        [Button]
        public void Swap(int idx1,int idx2)
        {
            inventory_1.Swap (idx1,inventory_2,idx2);
        }

        [Button]
        public void SwapSelf(int idx1,int idx2)
        {
           inventory_1.Swap(idx1,idx2);
        }
    }
}