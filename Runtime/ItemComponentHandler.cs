using Sirenix.OdinInspector;

namespace GloryJam.Inventories
{
    public abstract class ItemComponentHandler : IInstance<ItemComponentHandler>
    {
        #region property
        protected Item item => component?.item;
        protected ItemStack stack => component?.stack;
        protected ItemSlot slot => stack?.slot;
        protected Inventory inventory => stack?.slot.inventory;
        #endregion

        #region protected
        protected ItemComponent component;
        #endregion

        #region methods
        public virtual void Init(ItemComponent component){
            SetComponent(component);
            OnInit();
        }
        public void SetComponent(ItemComponent component){
            this.component = component;
        }
        public virtual void Dispose(){
            OnDispose();
        }
        public abstract ItemComponentHandler CreateInstance();
        #endregion

        #region callback
        protected virtual void OnInit(){}
        protected virtual void OnDispose(){}
        #endregion
    }

    public abstract class ItemComponentHandler<T> : ItemComponentHandler
    where T : ItemComponentHandler
    {
        #region methods
        public override ItemComponentHandler CreateInstance(){
            return (T)MemberwiseClone();
        }
        #endregion
    }

    public abstract class ItemComponentHandler<T1,T2> : ItemComponentHandler<T1>
    where T1 : ItemComponentHandler
    where T2 : ItemComponentState, new()
    {
        #region protected
        [ShowInInspector,ShowIf(nameof(state)),BoxGroup("State")]
        protected T2 state;
        #endregion

        #region methods
        public override void Init(ItemComponent component)
        {
            base.Init(component);
            LoadState();
        }
        public abstract void SaveState();
        public abstract void LoadState();
        public override ItemComponentHandler CreateInstance()
        {
            var result = base.CreateInstance() as ItemComponentHandler<T1,T2>;
                result.state = new T2();
    
            return result;
        }
        #endregion
    }
}