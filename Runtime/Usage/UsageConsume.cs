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
            stack.Dispose();
            return true;
        }
        public override bool Unuse()
        {
            return true;
        }
        
        public override void OnInit(){}
        public override void OnPostInit(){}
        public override void OnDispose(){}
        #endregion
    }
}