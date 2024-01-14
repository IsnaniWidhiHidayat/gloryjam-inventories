using System;
using System.Collections.Generic;

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemSetHandler
    {
        #region const
        protected const string grpConfig = "Config";
        protected const string grpRequired = "Required";
        protected const string grpRuntime = "Runtime";
        protected const string grpDebug = "Debug";
        #endregion

        #region property
        public string title => name;
        public abstract string name{get;}
        #endregion

        #region callback
        public abstract void OnItemMatch(Dictionary<Item,ItemStack[]> itemTracker);
        #endregion
    }
}
