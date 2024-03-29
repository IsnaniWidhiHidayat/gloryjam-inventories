﻿using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public enum ItemRarityType
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        SetOrUnique
    }

    [Serializable]
    public class DetailItemRarity : ItemDetailHandler<ItemRarityType>
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
        public ItemRarityType type;
        #endregion

        #region methods
        public override ItemRarityType GetValue()
        {
            return type;
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