using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemDetailHandler : ItemComponentHandler
    {
        #region const
        protected const string grpKey = "Key";
        #endregion

        // #region property
        // #if ODIN_INSPECTOR
        // [BoxGroup(grpKey),ShowInInspector]
        // #endif
        // public abstract string name{get;}
        // #endregion

        #region methods
        public abstract object GetValueObject();
        #endregion
    }
    
    [Serializable]
    public abstract class ItemDetailHandler<T> : ItemDetailHandler
    {
        #region private
        protected const string grpValue = "Value";
        public override bool showID => false;
        public override bool requiredId => false;
        #endregion

        #region property
        #if ODIN_INSPECTOR
        [ShowInInspector,BoxGroup(grpValue),HideLabel,HideReferenceObjectPicker,HideDuplicateReferenceBox]
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