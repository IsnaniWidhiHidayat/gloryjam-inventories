using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemDetailHandler : ItemComponentHandler<ItemDetailHandler>
    {
        #region const
        protected const string grpConfig = "Config";
        protected const string grpKey = "Key";
        #endregion

        #region property
        #if ODIN_INSPECTOR
        [BoxGroup(grpKey),ShowInInspector]
        #endif
        public abstract string name{get;}
        #endregion

        #region methods
        public abstract object GetValueObject();
        public T GetUsageHandler<T>() where T : ItemUsageHandler
        {
            var usableComponent = default(ItemUseableComponent);

            //Get usage component from stack
            if(stack != null){
                stack.TryGetComponentUsable(out usableComponent);
            }else if(item != null){
                item.TryGetComponentUsable(out usableComponent);
            }

            var usage = usableComponent?.GetHandler<T>();
            if(usage == null){
                if(item != null){
                    Debug.LogError($"Item : {item.id} did't contain usage type of {nameof(T)}");
                }
                return default;
            }

            return usage;
        }
        #endregion
    }
    
    [Serializable]
    public abstract class ItemDetailHandler<T> : ItemDetailHandler
    {
        #region private
        const string grpValue = "Value";
        #endregion

        #region property
        #if ODIN_INSPECTOR
        [ShowInInspector,BoxGroup(grpValue),HideLabel,HideReferenceObjectPicker]
        #endif
        public T value => GetValue();
        #endregion

        #region methods
        public override object GetValueObject()
        {
            return GetValue();
        }
        public abstract T GetValue();
        #endregion
    }
}