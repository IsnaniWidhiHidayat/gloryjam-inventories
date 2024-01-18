using System;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
using UnityEngine;

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemStateUsageHandler : ItemStateHandler
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

            #if ODIN_INSPECTOR
            [ShowIf(nameof(type),Type.ID)]
            #endif
            public string UsageID;
            public bool inUse;
        }
        #endregion

        #region property
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        #endif
        public Type type;

        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        [ShowIf(nameof(type),Type.ID)]
        [ValueDropdown(nameof(InspectorGetComponentsID))]
        [ValidateInput(nameof(InspectorValidateComponentID),"ID not found",InfoMessageType.Error)]
        #endif
        public string UsageID;

        #if ODIN_INSPECTOR
        [BoxGroup(grpRuntime)]
        #endif
        public bool inUse;

        public override ItemStateSaveData saveData{
            get{
                if(_saveData == null) _saveData = new SaveData();

                _saveData.id = id;
                _saveData.type = type;
                _saveData.UsageID = UsageID;
                _saveData.inUse = inUse;

                return _saveData;
            }
            set{
                _saveData = value as SaveData;

                type      = _saveData.type;
                UsageID   = _saveData.UsageID;
                inUse     = _saveData.inUse;

            }
        }

        public override string name => $"In Use [{inUse}]";
        public override int order => 1000;
        #endregion

        #region private
        SaveData _saveData;
        #endregion

        #region inspector
        private string[] InspectorGetComponentsID()
        {
            if(stack != null) return stack.GetComponentsID<ItemUseableComponent>();
            if(item != null) return item.GetComponentsID<ItemUseableComponent>();

            return default;
        }
        private bool InspectorValidateComponentID()
        {
            if(stack != null) return stack.ContainComponent<ItemUseableComponent>(UsageID);
            if(item != null) return item.ContainComponent<ItemUseableComponent>(UsageID);

            return default;
        }
        #endregion

        #region constructor
        public ItemStateUsageHandler(){
            id = "Usage";
        }
        #endregion

        #region methods
        public override void SaveState(){

            var useableComponent = GetComponent();

            if(useableComponent == null) return;

            Debug.Log($"[Inventory]{inventory?.name} Inventory {inventory?.id} Save State {GetType()?.Name}:{this}, stack:{stack}");

            inUse = useableComponent.inUse;
        }
        public override void LoadState(){

            var useableComponent = GetComponent();

            //check empty
            if(useableComponent == null) return;

            Debug.Log($"[Inventory]{inventory?.name} Inventory {inventory?.id} Load State {GetType()?.Name}:{this}, stack:{stack}");

            var useableInUse = useableComponent.inUse;
            if(inUse && !useableInUse){
                useableComponent.Use();
            }else if (!inUse && useableInUse){
                useableComponent.Unuse();
            }
        }
        public override string ToString()
        {
            return $"{{ inUse:{inUse} }}";
        }
        private ItemUseableComponent GetComponent(){
            var component = default(ItemUseableComponent);
            switch(type)
            {
                case Type.Default:{
                    if(!stack.TryGetComponentUsable(out component)){
                        Debug.LogError($"No {nameof(ItemUseableComponent)} was found in item {item?.id}");
                    }
                    break;
                }

                case Type.ID:{
                    if(!stack.TryGetComponent(UsageID,out component)){
                        Debug.LogError($"No {nameof(ItemUseableComponent)} with ID {UsageID} was found in item {item?.id}");
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
