using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public class DetailItemEffect : ItemDetailHandler<List<string>>
    {
        #region const
        public const string KEY = "ITEM-EFFECT";
        #endregion

        #region property
        public override string name => KEY;
        #endregion

        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        #endif
        public List<string> effects;
        #endregion

        #region constructor
        public DetailItemEffect(){
            effects = new List<string>();
        }
        #endregion

        #region methods
        public override List<string> GetValue()
        {
            return effects;
        }
        #endregion
    }
}