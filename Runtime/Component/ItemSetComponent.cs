using UnityEngine;
using System.Collections;
using System;
using GloryJam.DataAsset;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    public class ItemSetComponent : ItemComponent<ItemSetComponent>
    {
        #region fields
        #if ODIN_INSPECTOR
        [InlineEditor,Required]
        #endif
        public ItemSetDataAsset itemSet;
        #endregion

        #region property
        public override string name => "Set";
        public override int order => 102;
        #endregion

        #region methods
        public override ItemComponent CreateInstance()
        {
            var clone = base.CreateInstance() as ItemSetComponent;
                clone.itemSet = itemSet;
            return clone;
        }
        #endregion

        #region callback
        public override void OnInit()
        {
            //persistent itemset
            DataAssetPresistent.AddObject(itemSet);

            if(inventory == null) return;
            if(inventory.inited){
                itemSet?.value.OnItemStackInit(stack);
            }else{
                inventory.StartCoroutine(CoroutineWaitUntilInventoryInited());
            }
        }
        public override void OnPostInit(){}
        public override void OnDispose()
        {
            itemSet?.value.OnItemStackDispose(stack);
        }
        #endregion

        #region coroutine
        private IEnumerator CoroutineWaitUntilInventoryInited(){
            yield return new WaitUntil(()=> inventory.inited);
            itemSet?.value.OnItemStackInit(stack);
        }
        #endregion
    }
}
