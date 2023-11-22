﻿using System;
using GloryJam.DataAsset;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{

    [Serializable]
    public class DetailItemIconTertiary : DetailItemIcon
    {
        #region property
        public override string name => "ITEM-ICON-TERTIARY";
        #endregion
    }
}