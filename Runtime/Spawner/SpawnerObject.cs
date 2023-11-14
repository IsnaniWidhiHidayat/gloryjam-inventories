using System;
using UnityEngine;
using GloryJam.Inventories;

namespace GloryJam.GT.Gameplay.Inventories
{
    [Serializable]
    public class SpawnerObject : ItemSpawnerHandler
    {
        #region property
        public override string name => "Spawner Object";
        #endregion

        #region callback
        public override void OnInit(){}
        public override void OnPostInit(){}
        public override void OnDispose(){}
        #endregion
    }
}