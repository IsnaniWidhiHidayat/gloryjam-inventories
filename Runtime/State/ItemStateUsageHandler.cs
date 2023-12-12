using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemStateUsageHandler : ItemStateHandler
    {
        #region property
        [BoxGroup(grpRuntime)]
        public bool inUse;

        public override bool showID => false;
        public override bool requiredId => false;
        #endregion

        #region property
        public override string name => $"In Use [{inUse}]";
        public override int order => 1000;
        #endregion

        #region private
        private ItemUseableComponent useableComponent;
        #endregion

        #region methods
        public override void SaveState(){
            
            if(useableComponent == null) return;

            inUse = useableComponent.inUse;
        }
        public override void LoadState(){
            if(useableComponent == null) return;

            var useableInUse = useableComponent.inUse;
            Debug.Log($"Load state usage : {item.id}");
            if(inUse && !useableInUse){
                useableComponent.Use();
            }else if (!inUse && useableInUse){
                useableComponent.Unuse();
            }
        }
        #endregion

        #region callback
        public override void OnInit()
        {
            stack.TryGetComponentUsable(out useableComponent);
        }
        public override void OnPostInit(){}
        public override void OnDispose(){}
        #endregion
    }
}
