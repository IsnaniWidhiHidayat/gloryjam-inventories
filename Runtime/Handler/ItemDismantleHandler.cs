using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemDismantleHandler : ItemComponentHandler
    {
        #region field
        [SerializeField]
        #if ODIN_INSPECTOR
        [InlineEditor]
        #endif
        private ItemAsset _item;

        [Range(0,1f)]
        public float probability; // from 0 to 1

        #if ODIN_INSPECTOR
        [TableColumnWidth(60,false)]
        #endif
        public int min,max; // min a7 max value
        #endregion

        #region methods
        public bool Dismantle(ref Dictionary<Item, int> result)
        {
            var r = true;
            var amount = 0;

            var random = Random.value;
            if (random <= probability)
            {
                //amount += Mathf.CeilToInt(Mathf.Lerp(min,max,random));
                amount += max;
            }

            if(amount > 0){
                if(result == null){
                    result = new Dictionary<Item, int>();
                }
                
                result[_item.value] = amount;
            }
            return r;
        }
        #endregion

        #region callback
        public override void OnInit(){}
        public override void OnDispose(){}
        #endregion
    }
}
