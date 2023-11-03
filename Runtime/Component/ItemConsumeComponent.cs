using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    #if ODIN_INSPECTOR
    [Serializable,HideReferenceObjectPicker,HideDuplicateReferenceBox]
    [Toggle("Enabled")]
    #endif
    [DisallowMultipleItemComponent]
    public class ItemConsumeComponent : ItemComponent
    {
        #region property
        public override string ComponentName => "Consume";
        public override int ComponentPropertyOrder => 100;
        #endregion

        #region methods
        public void Consume(){
            stack.Dispose();
        }
        public override void LoadState(){}
        public override void SaveState(){}
        public override void Dispose(){}
        public override ItemComponent CreateInstance()
        {
            return this;
        }
        #endregion
    }

    public static class ItemConsumeComponentExtend
    {
        public static bool TryGetComponentSpawner(this ItemStack stack,out ItemConsumeComponent result){
            result = default;
            return stack != null ? stack.TryGetComponent(out result) : default;
        }
        public static bool TryGetComponentSpawner(this Item item,out ItemConsumeComponent result){
            result = default;
            return item != null ? item.TryGetComponent(out result) : default;
        }

        public static bool ContainComponentSpawner(this ItemStack stack){
            return stack.ContainComponent<ItemConsumeComponent>();
        }
        public static bool ContainComponentSpawner(this Item item){
            return item.ContainComponent<ItemConsumeComponent>();
        }
    
    }
}
