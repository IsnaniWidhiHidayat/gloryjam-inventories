using UnityEngine;
using Sirenix.OdinInspector;

namespace GloryJam.Inventories.Test
{
    public class TestInventory : MonoBehaviour {
        public Inventory inventory;
        public InventorySaveState saveState;

        [Button]
        private void Save(){
            saveState.Save("main");
        }

        [Button]
        private void load(){
            saveState.Load("main");
        }
    }
}