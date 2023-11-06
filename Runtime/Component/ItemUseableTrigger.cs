using System;
using GloryJam.DataAsset;

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemUseableTrigger : IInstance<ItemUseableTrigger>
    {
        #region innerclass
        [Serializable]
        public enum Type{
            Manual,
            Instant,
            Custom,
        }
        #endregion

        #region events
        public event Action onTrigger;
        #endregion

        #region private
        protected ItemUseableComponent component;
        #endregion

        #region methods
        public void SetComponent(ItemUseableComponent component){
            this.component = component;
        }
        public void InvokeOnTrigger(){
            onTrigger?.Invoke();
        }
        #endregion

        #region callback
        public abstract void OnInit();
        public abstract void OnDispose();
        public abstract ItemUseableTrigger CreateInstance();
        #endregion
    }
}