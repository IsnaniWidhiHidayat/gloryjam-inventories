
using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace GloryJam.Inventories.Test
{
    [Serializable]
    public class TestUsageItem : ItemUsageHandler
    {
        #region property
        public override bool inUse => _inUse;
        public override string name => "Usage Test";
        #endregion

        #region private
        private bool _inUse;
        #endregion

        #region methods
        public override bool Use()
        {
            _inUse = true;
            return true;
        }
        public override bool Unuse()
        {
            _inUse = false;
            return true;
        }
        
        public override void OnInit(){}
        public override void OnPostInit(){}
        public override void OnDispose(){}
        #endregion
    }
}