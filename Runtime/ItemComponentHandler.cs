namespace GloryJam.Inventories
{
    public abstract class ItemComponentHandler : IInstance<ItemComponentHandler>
    {
        #region fields
        protected Item item => component?.item;
        protected ItemStack stack => component?.stack;
        protected ItemSlot slot => stack?.slot;
        protected Inventory inventory => stack?.slot.inventory;
        protected ItemComponent component;
        #endregion

        #region methods
        public void Init(ItemComponent component){
            this.component = component;
            OnInit();
        }
        public void Dispose(){
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
}