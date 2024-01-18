using GloryJam.DataAsset;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    #if ODIN_INSPECTOR
    [Toggle("Enabled",CollapseOthersOnExpand = false)]
    #endif
    public struct ItemUseableAutoUse : IInstance<ItemUseableAutoUse>
    {
        #region const
        const string grpConfig = "Config";
        const string grpRuntime = "Runtime";
        #endregion

        #region fields
        public bool Enabled;

        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        #endif
        public bool autoUse;
        #endregion

        #region property
        public string name => "Auto Use";

        public string title {
            get{
                return name;
            }
        }
        #endregion

        #region private
        private ItemComponent _component;
        #endregion

        #region methods
        public void SetComponent(ItemUseableComponent component)
        {
            _component = component;
        }
        public ItemUseableAutoUse CreateInstance()
        {
            return new ItemUseableAutoUse(){
                Enabled = Enabled,
                autoUse = autoUse
            };
        }
        #endregion
    }
}