using System;

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemTriggerHandler : ItemComponentHandler
    {
        #region events
        public event Action onTrigger;
        #endregion

        #region methods
        public void InvokeOnTrigger(){
            onTrigger?.Invoke();
        }
        #endregion
    }
}