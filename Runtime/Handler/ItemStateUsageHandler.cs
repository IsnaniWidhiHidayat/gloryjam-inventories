using System;
using GloryJam.Event;

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemStateUsageHandler : ItemStateHandler,EventListener<ItemUseableEvent>
    {
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
            if(useableComponent != null){
                inUse = useableComponent.inUse;
            }
        }
        public override void LoadState(){
            if(inUse){
                useableComponent?.Use();
            }else{
                useableComponent?.Unuse();
            }
        }
        #endregion

        #region callback
        public override void OnInit()
        {
            this.RegisterEvent();
            stack.TryGetComponentUsable(out useableComponent);
        }
        public override void OnPostInit()
        {
        }
        public override void OnDispose()
        {
            this.UnregisterEvent();
        }

        public void OnEvent(object sender, ItemUseableEvent Event)
        {
            switch(Event.type){
                case ItemUseableEvent.Type.Use:{
                    SaveState();
                    break;
                }

                case ItemUseableEvent.Type.Unuse:{
                    SaveState();
                    break;
                }
            }
        }
        #endregion
    }
}
