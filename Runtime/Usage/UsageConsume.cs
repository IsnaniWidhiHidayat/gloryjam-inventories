using System;

namespace GloryJam.Inventories
{
    [Serializable]
    public class UsageConsume : ItemUsageHandler
    {
        #region property
        public override bool inUse => _inUse;
        #endregion

        #region private
        private bool _inUse;
        #endregion

        #region methods
        public override bool Use()
        {
            return _inUse = slot.inventory.DisposeSlot(slot,1);
        }
        public override bool Unuse()
        {
            return true;
        }
        
        protected override void OnInit(){}
        protected override void OnDispose(){}
        #endregion
    }
}