using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public enum DetailItemRarityType
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        SetOrUnique
    }

    [Serializable]
    public class DetailItemRarity : ItemDetailHandler<DetailItemRarityType>
    {
        #region const
        public const string KEY = "ITEM-RARITY";
        #endregion

        #region property
        public override string name => KEY;
        #endregion

        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        #endif
        public DetailItemRarityType type;
        #endregion

        #region methods
        public override DetailItemRarityType GetValue()
        {
            return type;
        }
        #endregion
    }
}