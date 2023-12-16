using System;
using GloryJam.Extend;

namespace GloryJam.Inventories
{
    [Serializable]
    public class UsageConsume : ItemUsageHandler
    {
        #region property
        public override bool inUse => _inUse;
        public override string name => "Consume";
        #endregion

        #region private
        private bool _inUse;
        #endregion

        #region methods
        public override bool Use()
        {
            $"Consume {stack}".Log(DebugFilter.Item);
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