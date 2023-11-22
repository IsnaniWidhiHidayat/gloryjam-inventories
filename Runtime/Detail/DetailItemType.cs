using System;
using GloryJam.DataAsset;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{

    [Serializable]
    public class DetailItemType : ItemDetailHandler<ScriptableObject>
    {
        #region property
        public override string name => "ITEM-TYPE";
        #endregion

        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        #endif
        public ScriptableObject category;
        #endregion

        #region methods
        public override ScriptableObject GetValue()
        {
            return category;
        }
        public override ItemComponentHandler CreateInstance()
        {
            return this;
        }
        #endregion

        #region callback
        public override void OnInit(){}
        public override void OnPostInit(){}
        public override void OnDispose(){}
        #endregion
    }
}