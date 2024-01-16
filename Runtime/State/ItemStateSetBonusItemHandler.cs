using System;
using UnityEngine;
using System.Collections.Generic;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector; 
#endif


namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemStateSetBonusItemHandler : ItemStateSetItemHandler
    {
        #region inner class
        [Serializable]
        public class SaveData : SetSaveData
        {
            public bool obtained;
            public List<int> itemStacksHash = new List<int>();
        }
        #endregion

        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpRuntime)]
        [ShowIf(nameof(InspectorShowRuntime))]
        #endif
        public bool obtained;

        #if ODIN_INSPECTOR
        [BoxGroup(grpRuntime)]
        [ShowIf(nameof(InspectorShowRuntime))]
        [ListDrawerSettings(IsReadOnly = true,ListElementLabelName = "title")]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        #endif
        [NonSerialized]
        public List<ItemStack> itemBonusTracker = new List<ItemStack>();
        #endregion

        #region property
        public override ItemStateSaveData saveData { 
            get {
                if(_saveData == null){
                    _saveData = new SaveData();
                }

                _saveData.id = id;
                _saveData.type = type;
                _saveData.setID = setID;
                _saveData.obtained = obtained;

                if(_saveData.itemStacksHash == null){
                    _saveData.itemStacksHash = new List<int>();
                }

                _saveData.itemStacksHash.Clear();

                if(itemBonusTracker?.Count > 0) {
                    for (int i = 0; i < itemBonusTracker.Count; i++)
                    {
                        if(itemBonusTracker[i] == null) continue;
                        _saveData.itemStacksHash.Add(itemBonusTracker[i].hash);
                    }
                }

                return _saveData;
            }
            set {
                
                _saveData = value as SaveData;
                
                type  = _saveData.type ; 
                setID = _saveData.setID ; 
                obtained = _saveData.obtained;

                if(itemBonusTracker == null){
                    itemBonusTracker = new List<ItemStack>();
                }

                itemBonusTracker.Clear();

                if(_saveData.itemStacksHash?.Count > 0) {
                    for (int i = 0; i < _saveData.itemStacksHash.Count; i++)
                    {
                        var hash  = _saveData.itemStacksHash[0];
                        var stack = ItemStack.GetByHash(hash);

                        if(itemBonusTracker.Contains(stack)) continue;

                        itemBonusTracker.Add(stack);
                    }
                }
            }
        }
        public override string name => $"Set [{obtained}]";
        #endregion

        #region private
        private SaveData _saveData;
        #endregion

        #region constructor
        public ItemStateSetBonusItemHandler(){
            id = "Set Bonus Item";
        }
        #endregion

        #region methods
        public override void SaveState(){

            var setComponent = GetSetComponent();

            //check empty
            if(setComponent == null) return;

            //check handler
            var handler = GetSetHandler<ItemSetBonusItemHandler>();
            if(handler == null) return;

            Debug.Log($"[Inventory]{inventory?.name} Inventory {inventory?.id} Save State {GetType()?.Name}:{this}, stack:{stack}");

            obtained = handler.obtained;

            if(itemBonusTracker == null){
                itemBonusTracker = new List<ItemStack>();
            }

            itemBonusTracker.Clear();

            if(handler.itemBonusTracker?.Length > 0) {
                for (int i = 0; i < handler.itemBonusTracker.Length; i++)
                {
                    var stack = handler.itemBonusTracker[i];
                    if(stack == null) continue;
                    itemBonusTracker.Add(stack);
                }
            }
        }
        public override void LoadState(){

            var setComponent = GetSetComponent();

            //check empty
            if(setComponent == null) return;

            //check handler
            var handler = GetSetHandler<ItemSetBonusItemHandler>();
            if(handler == null) return;

            Debug.Log($"[Inventory]{inventory?.name} Inventory {inventory?.id} Load State {GetType()?.Name}:{this}, stack:{stack}");

            if(itemBonusTracker == null){
                itemBonusTracker = new List<ItemStack>();
            }

            handler.TryInitialize();

            handler.obtained = obtained;

            if(handler.itemBonusTracker?.Length > 0) {
                for (int i = 0; i < handler.itemBonusTracker.Length; i++)
                {
                    handler.itemBonusTracker[i] = null;
                    
                    if(i < itemBonusTracker.Count){
                        handler.itemBonusTracker[i] = itemBonusTracker[i];
                    }
                }
            }
        }
        public override string ToString()
        {
            return $"{{ inUse:{obtained} }}";
        }
        #endregion

        #region callback
        public override void OnInit(){}
        public override void OnPostInit(){}
        public override void OnDispose(){}
        #endregion
    }
}
