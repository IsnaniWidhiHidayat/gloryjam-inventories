using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemUsageStack : ItemUsageHandler
    {
        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        [ValueDropdown(nameof(InspectorGetUsageID))]
        [ValidateInput(nameof(InspectorValidateUsageID),"Contain Invalid Usage ID",InfoMessageType.Error)]
        [ListDrawerSettings(Expanded = true)]
        #endif
        public string[] usageID = new string[0];
        #endregion

        #region property
        #if ODIN_INSPECTOR
        [ShowIf(nameof(InspectorShowRuntime))]
        [BoxGroup(grpRuntime)]
        [ShowInInspector]
        [DisplayAsString]
        #endif
        public string currentUsage {
            get {
                if(currentUsageIndex >= 0 && currentUsageIndex < usageID.Length){
                    return usageID[currentUsageIndex];
                }

                return default;
            }
        }

        #if ODIN_INSPECTOR
        [ShowIf(nameof(InspectorShowRuntime))]
        [BoxGroup(grpRuntime)]
        [ShowInInspector]
        [DisplayAsString]
        #endif
        public string nextUsage{
            get {
                if(nextUsageIndex >= 0 && nextUsageIndex < usageID.Length){
                    return usageID[nextUsageIndex];
                }

                return default;
            }
        }

        //[BoxGroup(grpRuntime),ShowInInspector,DisplayAsString]
        public int currentUsageIndex => nextUsageIndex - 1;
        public int nextUsageIndex => _nextUsageIndex;

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
        private bool InspectorValidateUsageID()
        {
            var isValid = true;
            if(usageID?.Length > 0) {
                for (int i = 0; i < usageID.Length; i++)
                {
                    if(string.IsNullOrEmpty(usageID[i])) continue;

                    if(stack != null && !stack.ContainComponent<ItemUseableComponent>(usageID[i])){
                        isValid = false;
                        break;
                    }

                    if(item != null && !item.ContainComponent<ItemUseableComponent>(usageID[i])){
                        isValid = false;
                        break;
                    }
                }
            }
            
            return isValid;
        }
        #endif
        #endregion

        #region private
        private ItemUseableComponent[] _useables;
        private int _nextUsageIndex;
        #endregion

        #region methods
        public override bool Use()
        {
            var result = false;
            if(_useables?.Length > 0 && _nextUsageIndex >= 0 && _nextUsageIndex < usageID.Length){
                if(_useables[_nextUsageIndex] != null && !_useables[_nextUsageIndex].inUse){
                    result = _useables[_nextUsageIndex].Use();
                }

                if(result){
                    _nextUsageIndex++;
                }
            }

            return result;
        }
        public override bool Unuse()
        {
            var result = false;
            if(_useables?.Length > 0 && currentUsageIndex >= 0 && currentUsageIndex < usageID.Length) {
                if(_useables[currentUsageIndex] != null && _useables[_nextUsageIndex].inUse){
                    result = _useables[currentUsageIndex].Unuse();
                }

                if(result){
                    _nextUsageIndex--;
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