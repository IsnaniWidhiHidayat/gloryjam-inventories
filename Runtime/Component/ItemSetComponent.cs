using UnityEngine;
using System.Collections;
using System;



#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    public class ItemSetComponent : ItemComponent<ItemSetComponent>
    {
        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpRequired),InlineEditor,Required]
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
            return this;
        }
        #endregion

        #region callback
        public override void OnInit()
        {
            if(inventory == null) return;
            if(inventory.inited){
                itemSet?.value.OnItemStackInit(stack);
            }else{
                inventory.StartCoroutine(CoroutineWaitUntilInventoryInited(()=>{
                    itemSet?.value.OnItemStackInit(stack);
                }));
            }
        }
        public override void OnPostInit(){}
        public override void OnDispose()
        {
            itemSet?.value.OnItemStackDispose(stack);
        }
        #endregion

        #region coroutine
        private IEnumerator CoroutineWaitUntilInventoryInited(Action callback = null){
            yield return new WaitUntil(()=> inventory.inited);
            callback?.Invoke();
        }
        #endregion
    }
}
