#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

using UnityEngine;
using UnityEngine.Events;

namespace GloryJam.Inventories.Test
{
    public class TimeCounter : MonoBehaviour
    {
        public int counter;

        public UnityEvent<int> onCounterChange;

        #if ODIN_INSPECTOR
        [Button]
        #endif
        public void Add(){
            counter ++;
            onCounterChange?.Invoke(counter);
        }
    }
}