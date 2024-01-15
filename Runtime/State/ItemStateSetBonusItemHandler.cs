using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemStateSetBonusItemHandler : ItemStateHandler
    {
        #region inner class
        [Serializable]
        public enum Type
        {
            Default,
            ID
        }
        
        [Serializable]
        public class SaveData : ItemStateSaveData
        {
            public Type type;

            [ShowIf(nameof(type),Type.ID)]
            public string setID;

            public bool obtained;
        }
        #endregion

        #region property
        [BoxGroup(grpConfig)]
        public Type type;

        [BoxGroup(grpConfig)]
        [ShowIf(nameof(type),Type.ID)]
        [ValueDropdown(nameof(InspectorGetComponentsID))]
        [ValidateInput(nameof(InspectorValidateComponentID),"ID not found",InfoMessageType.Error)]
        public string setID;

        [BoxGroup(grpRuntime)]
        public bool obtained;

        public override ItemStateSaveData saveData{
            get{
                if(_saveData == null) _saveData = new SaveData();

                _saveData.id = id;
                _saveData.type = type;
                _saveData.setID = setID;
                _saveData.obtained = obtained;

                return _saveData;
            }
            set{
                _saveData = value as SaveData;

                type      = _saveData.type;
                setID     = _saveData.setID;
                obtained  = _saveData.obtained;
            }
        }

        public override string name => $"Set [{obtained}]";
        public override int order => 1000;
        #endregion

        #region private
        SaveData _saveData;
        #endregion

        #region inspector
        private string[] InspectorGetComponentsID()
        {
            if(stack != null) return stack.GetComponentsID<ItemSetComponent>();
            if(item != null) return item.GetComponentsID<ItemSetComponent>();

            return default;
        }
        private bool InspectorValidateComponentID()
        {
            if(stack != null) return stack.ContainComponent<ItemSetComponent>(setID);
            if(item != null) return item.ContainComponent<ItemSetComponent>(setID);

            return default;
        }
        #endregion

        #region constructor
        public ItemStateSetBonusItemHandler(){
            id = "Set";
        }
        #endregion

        #region methods
        public override void SaveState(){

            var setComponent = GetComponent();

            if(setComponent == null) return;

            Debug.Log($"[Inventory]{inventory?.name} Inventory {inventory?.id} Save State {GetType()?.Name}:{this}, stack:{stack}");

            obtained = setComponent.itemSet.value.obtained;
        }
        public override void LoadState(){

            var setComponent = GetComponent();

            //check empty
            if(setComponent == null) return;

            Debug.Log($"[Inventory]{inventory?.name} Inventory {inventory?.id} Load State {GetType()?.Name}:{this}, stack:{stack}");

            setComponent.itemSet.value.obtained = obtained;
        }
        public override string ToString()
        {
            return $"{{ inUse:{obtained} }}";
        }
        private ItemSetComponent GetComponent(){
            var component = default(ItemSetComponent);
            switch(type)
            {
                case Type.Default:{
                    if(!stack.TryGetComponentSet(out component)){
                        Debug.LogError($"No {nameof(ItemSetComponent)} was found in item {item?.id}");
                    }
                    break;
                }

                case Type.ID:{
                    if(!stack.TryGetComponent(setID,out component)){
                        Debug.LogError($"No {nameof(ItemSetComponent)} with ID {setID} was found in item {item?.id}");
                    }
                    break;
                }
            }

            return component;
        }
        #endregion

        #region callback
        public override void OnInit(){}
        public override void OnPostInit(){}
        public override void OnDispose(){}
        #endregion
    }
}
