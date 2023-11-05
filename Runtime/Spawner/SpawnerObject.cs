using System;
using UnityEngine;
using GloryJam.Inventories;

namespace GloryJam.GT.Gameplay.Inventories
{
    [Serializable]
    public class SpawnerObject : ItemSpawnerHandler
    {
        #region fields
        public GameObject Object;

        #endregion

        #region methods
        public override GameObject Spawn()
        {
            if(Object == null) return null;
            return GameObject.Instantiate(Object,Vector3.zero,Quaternion.identity).gameObject;
        }
        #endregion

        #region callback
        public override void OnInit(){}
        public override void OnDispose(){}
        #endregion
    }
}