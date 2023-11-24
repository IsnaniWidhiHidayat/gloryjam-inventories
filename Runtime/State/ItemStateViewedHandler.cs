using System;
using Sirenix.OdinInspector;

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemStateViewedHandler : ItemStateHandler
    {
        #region property
        [BoxGroup(grpRuntime),DisableInEditorMode]
        public bool isViewed;

        public override bool showID => false;
        public override bool requiredId => false;
        #endregion

        #region property
        public override string name => $"Viewed : {isViewed}";
        #endregion

        #region methods
        public override void SaveState(){}
        public override void LoadState(){}
        #endregion

        #region callback
        public override void OnInit()
        {
            isViewed = false;
        }
        public override void OnPostInit(){}
        public override void OnDispose(){}
        #endregion
    }
}
