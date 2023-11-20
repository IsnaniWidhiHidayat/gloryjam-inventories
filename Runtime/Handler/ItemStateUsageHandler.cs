using System;
using UnityEngine;
using GloryJam.Event;

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemStateUsageHandler : ItemStateHandler//,EventListener<ItemUseableEvent>
    {
        public bool log;

        #region property
        public bool inUse;
        #endregion

        #region property
        public override string name => $"In Use : {inUse}";
        #endregion

        #region private
        private ItemUseableComponent useableComponent;
        #endregion

        #region methods
        public override void SaveState(){
            
            if(useableComponent == null) return;

            inUse = useableComponent.inUse;

            if(log){
                Debug.Log($"Save State - {stack.item.id} {inUse}");
            }
        }
        public override void LoadState(){
            if(useableComponent == null) return;

            var useableInUse = useableComponent.inUse;

            if(inUse && !useableInUse){
                useableComponent.Use();
            }else if (!inUse && useableInUse){
                useableComponent.Unuse();
            }

            if(log){
                Debug.Log($"Load State - {stack.item.id} {inUse}");
            }
        }
        #endregion

        #region callback
        public override void OnInit()
        {
            //this.RegisterEvent();
            stack.TryGetComponentUsable(out useableComponent);
        }
        public override void OnPostInit()
        {
        }
        public override void OnDispose()
        {   
            //this.UnregisterEvent();
        }

        // public void OnEvent(object sender, ItemUseableEvent Event)
        // {
        //     switch(Event.type){
        //         case ItemUseableEvent.Type.Use:{
        //             SaveState();
        //             break;
        //         }

        //         case ItemUseableEvent.Type.Unuse:{
        //             SaveState();
        //             break;
        //         }
        //     }
        // }
        #endregion
    }
}
