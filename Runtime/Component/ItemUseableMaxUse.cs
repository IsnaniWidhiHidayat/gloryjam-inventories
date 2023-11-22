using GloryJam.DataAsset;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
     #if ODIN_INSPECTOR
    [Toggle("Enabled",CollapseOthersOnExpand = false)]
    #endif
    public struct ItemUseableMaxUse : IInstance<ItemUseableMaxUse>
    {
        #region const
        const string grpConfig = "Config";
        const string grpRuntime = "Runtime";
        #endregion

        #region fields
        public bool Enabled;

        [BoxGroup(grpConfig),MinValue(1)]
        public int count;

        [BoxGroup(grpConfig)]
        public bool dispose;
        #endregion

        #region property
        #if ODIN_INSPECTOR
        [BoxGroup(grpRuntime),DisplayAsString]
        [ShowIf(nameof(InspectorShowRuntime))]
        #endif
        public bool isCanUse => _runtimeUsed < count;

        #if ODIN_INSPECTOR
        [BoxGroup(grpRuntime),DisplayAsString]
        [ShowIf(nameof(InspectorShowRuntime))]
        #endif
        public int runtimeUsed => _runtimeUsed;

        public string name => "Max Use";

        public string title {
            get{
                if(!Enabled) return name;

                if(Application.isPlaying){
                    return $"{name} ({_runtimeUsed})";
                }else{
                    return $"{name} ({count})";
                }
            }
        }
        #endregion

        #region inspector
        private bool InspectorShowRuntime(){
            return Application.isPlaying;
        }
        #endregion

        #region private
        private int _runtimeUsed;
        private ItemComponent _component;
        #endregion

        #region methods
        public void SetComponent(ItemUseableComponent component)
        {
            _component = component;
        }
        public void IncreaseUse(){
            _runtimeUsed++;

            //dispose;
            if(_runtimeUsed >= count && dispose){
                _component?.stack?.Dispose();
            }
        }
        public ItemUseableMaxUse CreateInstance()
        {
            return new ItemUseableMaxUse(){
                Enabled = Enabled,
                count = count
            };
        }
        #endregion
    }
}