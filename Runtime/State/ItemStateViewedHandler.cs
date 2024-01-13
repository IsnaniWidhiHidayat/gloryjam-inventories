using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemStateViewedHandler : ItemStateHandler
    {
        #region inner class
        [Serializable]
        public class SaveData :  ItemStateSaveData{
            public bool isViewed;
        }
        #endregion

        #region property
        [BoxGroup(grpRuntime),DisableInEditorMode]
        public bool isViewed;
        #endregion

        #region property
        public override string name => $"Viewed : {isViewed}";
        public override ItemStateSaveData saveData { 
            get {
                if(_saveData == null) _saveData = new SaveData();
                
                _saveData.id = id;
                _saveData.isViewed = isViewed;
                return _saveData;
            }
            set {
                _saveData = value as SaveData;
                isViewed = _saveData.isViewed;
            }
        }
        #endregion

        #region private
        private SaveData _saveData;
        #endregion

        #region constructor
        public ItemStateViewedHandler(){
            id = "Viewed";
        }
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
