using System;
using GloryJam.DataAsset;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{

    [Serializable]
    public class DetailItemIcon : ItemDetailHandler<Sprite>
    {
        #region property
        public override string name => "ITEM-ICON";
        #endregion

        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        #endif
        public DataReference<SpriteData> color = new DataReference<SpriteData>();
        #endregion

        #region methods
        public override Sprite GetValue()
        {
            return color.value.value;
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