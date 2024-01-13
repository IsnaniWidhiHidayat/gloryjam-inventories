using System;
using System.Collections.Generic;
using UnityEngine;

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

        #region methods
        public abstract void OnItemMatch(Dictionary<Item,ItemSetProgress> match,Inventory inventory);
        #endregion
    }
}
