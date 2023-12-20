using GloryJam.Event;
using GloryJam.Inventories;

namespace GloryJam.Inventories
{
    public abstract class ItemUsageEventTrigger<T> : ItemUsageHandler where T : Event<T>, new()
    {
        #region property
        public override string name => "Usage Event Trigger";
        public override bool inUse => false;
        public override string title {
            get{
                return $"{base.title} [{typeof(T).Name}]";
            }
        }
        #endregion

        #region methods
        public override bool Use()
        {
            Event<T>.Trigger(this,CreateEventOnUse());
            return true;
        }
        public override bool Unuse(){
            Event<T>.Trigger(this,CreateEventOnUnuse());
            return true;
        }
        protected abstract T CreateEventOnUse();
        protected abstract T CreateEventOnUnuse();
        #endregion
        
        #region callback
        public override void OnInit(){}
        public override void OnPostInit(){}
        public override void OnDispose(){}
        #endregion
    }
}