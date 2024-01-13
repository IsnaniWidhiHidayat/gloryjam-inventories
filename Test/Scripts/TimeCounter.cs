using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GloryJam.Inventories.Test
{
    public class TimeCounter : MonoBehaviour
    {
        public int counter;

        public UnityEvent<int> onCounterChange;

        [Button]
        public void Add(){
            counter ++;
            onCounterChange?.Invoke(counter);
        }
    }
}