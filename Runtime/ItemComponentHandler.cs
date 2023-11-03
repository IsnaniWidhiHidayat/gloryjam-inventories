
using Sirenix.OdinInspector;


namespace GloryJam.Inventories
{
    public abstract class ItemComponentHandler : IInstance<ItemComponentHandler>
    {
        #region property
        public Item item => component?.item;
        public ItemSlot slot => component?.slot;
        public ItemStack stack => component?.stack;
        public Inventory inventory => component?.inventory;
        #endregion

        #region protected
        protected ItemComponent component;

        [ShowInInspector,ShowIf(nameof(state)),BoxGroup("State"),HideLabel]
        protected ItemComponentHandlerState state;
        #endregion

        #region methods
        public virtual void Init(ItemComponent component){
            SetComponent(component);
            OnInit();
        }
        public void SetComponent(ItemComponent component){
            this.component = component;
        }
        public virtual void SaveState(){}
        public virtual void LoadState(){}
        public virtual void Dispose(){
            OnDispose();
        }
        public virtual ItemComponentHandler CreateInstance(){
            var r = (ItemComponentHandler)MemberwiseClone();
            r.state = CreateState();
            return r; 
        }
        protected virtual ItemComponentHandlerState CreateState(){
            return default;
        }
        #endregion

        #region callback
        protected virtual void OnInit(){}
        protected virtual void OnDispose(){}
        #endregion
    }
}