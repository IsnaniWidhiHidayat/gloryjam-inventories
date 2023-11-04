using System;

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemComponentHandler : IInstance<ItemComponentHandler>
    {
        #region const
        protected const string grpConfig = "Config";
        protected const string grpRuntime = "Runtime";
        protected const string grpRequired = "Required";
        protected const string grpDebug = "Debug";
        #endregion

        #region property
        public Item item => component?.item;
        public ItemSlot slot => component?.slot;
        public ItemStack stack => component?.stack;
        public Inventory inventory => component?.inventory;
        #endregion

        #region protected
        protected ItemComponent component;
        #endregion

        #region methods
        public virtual void Init(ItemComponent component){
            SetComponent(component);
            OnInit();
        }
        public virtual void Dispose(){
            OnDispose();
        }
        
        public void SetComponent(ItemComponent component){
            this.component = component;
        }
        
        public void SaveState(){}
        public void LoadState(){}
        
        public virtual ItemComponentHandler CreateInstance(){
            return (ItemComponentHandler)MemberwiseClone();
        }
        #endregion

        #region callback
        protected virtual void OnInit(){}
        protected virtual void OnDispose(){}
        #endregion
    }
}