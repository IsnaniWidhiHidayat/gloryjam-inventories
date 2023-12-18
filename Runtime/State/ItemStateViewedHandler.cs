using System;
using Sirenix.OdinInspector;
using UnityEngine;

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
        public override void SaveState(){
            Debug.Log($"[Inventory]{inventory?.name} Inventory {inventory?.id} Save State {GetType()?.Name}:{this}, stack:{stack}");
        }
        public override void LoadState(){
            Debug.Log($"[Inventory]{inventory?.name} Inventory {inventory?.id} Load State {GetType()?.Name}:{this}, stack:{stack}");
        }
        public override string ToString()
        {
            return $"{{ inViewed:{isViewed} }}";
        }
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
