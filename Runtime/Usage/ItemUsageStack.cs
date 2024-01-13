using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemUsageStack : ItemUsageHandler
    {
        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        [ValueDropdown(nameof(InspectorGetUsageID))]
        [ValidateInput(nameof(InspectorValidateUsageID))]
        [ListDrawerSettings(ShowIndexLabels = true,Expanded = true)]
        #endif
        public string[] usageID = new string[0];
        #endregion

        #region property
        [BoxGroup(grpRuntime),ShowInInspector,DisplayAsString]
        public string currentStack {
            get {
                if(currentIndex >= 0 && currentIndex < usageID.Length){
                    return usageID[currentIndex];
                }

                return default;
            }
        }

        [BoxGroup(grpRuntime),ShowInInspector,DisplayAsString]
        public int currentIndex => _currentIndex;

        public override bool inUse{
            get{
                var result = false;

                if(_useables?.Length > 0){
                    for (int i = 0; i < _useables.Length; i++)
                    {
                        result |= _useables[i].inUse;
                        if(result) break;
                    }
                }

                return result;
            }
        }
        public override string name => "Usage Stack";
        #endregion

        #region inspector
        #if ODIN_INSPECTOR
        private string[] InspectorGetUsageID()
        {
            if(stack != null) return stack.GetComponentsID<ItemUseableComponent>((x)=> x != component);
            if(item != null) return item.GetComponentsID<ItemUseableComponent>((x)=> x != component);
            return default;
        }
        private bool InspectorValidateUsageID(string ID)
        {
            Debug.Log(ID);
            if(stack != null) return stack.ContainComponent<ItemUseableComponent>(ID);
            if(item != null) return item.ContainComponent<ItemUseableComponent>(ID);
            return default;
        }
        #endif
        #endregion

        #region private
        private ItemUseableComponent[] _useables;
        private int _currentIndex = -1;
        #endregion

        #region methods
        public override bool Use()
        {
            var result = false;
            if(_currentIndex + 1 >= 0 && _currentIndex + 1 < usageID.Length){
                if(_useables[currentIndex] != null){
                    result = _useables[_currentIndex].Use();
                }

                if(result){
                    _currentIndex++;
                }
            }

            return result;
        }
        public override bool Unuse()
        {
            var result = false;
            if(_useables?.Length > 0) {
                for (int i = 0; i < _useables.Length; i++)
                {
                    if(_useables[i] == null) continue;
                    result |= _useables[i].Unuse();
                }
            }
            return result;
        }
        public override ItemComponentHandler CreateInstance()
        {
            var clone = base.CreateInstance() as ItemUsageStack;
                clone.usageID = usageID;
            
            return clone;
        }
        #endregion

        #region callback
        public override void OnInit(){
            _useables = new ItemUseableComponent[usageID.Length];
            for (int i = 0; i < _useables.Length; i++)
            {
                stack.TryGetComponent(usageID[i],out _useables[i]);
            }
        }
        public override void OnPostInit(){}
        public override void OnDispose(){}
        #endregion
    }
}