using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories.Test
{
    public class TestInventory : MonoBehaviour {
        public Inventory inventory;
        public InventorySaveState saveState;

        #if ODIN_INSPECTOR
        [Button]
        #endif
        private void Save(){
            saveState.Save("main");
        }
        
        #if ODIN_INSPECTOR
        [Button]
        #endif
        private void load(){
            saveState.Load("main");
        }
    }
}