using System;
using GloryJam.DataAsset;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{

    [Serializable]
    public class DetailItemColor : ItemDetailHandler<Color>
    {
        #region property
        public override string name => "ITEM-COLOR";
        #endregion

        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        #endif
        public DataReference<ColorData> color = new DataReference<ColorData>();
        #endregion

        #region methods
        public override Color GetValue()
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