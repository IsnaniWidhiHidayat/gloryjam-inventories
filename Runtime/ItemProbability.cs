using System;
using GloryJam.Probabilities;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemProbability : Probability
    {
        #region field
        #if ODIN_INSPECTOR
        [InlineEditor]
        #endif
        public ItemAsset item;
        #endregion
    }
}