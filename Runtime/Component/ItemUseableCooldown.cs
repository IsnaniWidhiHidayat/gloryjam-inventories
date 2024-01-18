using System.Collections;
using GloryJam.DataAsset;
using UnityEngine;
using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    #if ODIN_INSPECTOR
    [Toggle("Enabled",CollapseOthersOnExpand = false)]
    #endif
    public struct ItemUseableCooldown : IInstance<ItemUseableCooldown>
    {
        #region const
        const string grpConfig = "Config";
        const string grpRuntime = "Runtime";
        #endregion

        #region fields
        public bool Enabled;

        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        #endif
        public int duration;
        #endregion

        #region property
        #if ODIN_INSPECTOR
        [BoxGroup(grpRuntime),DisplayAsString]
        [ShowIf(nameof(InspectorShowRuntime))]
        #endif
        public bool isCanUse => CR_Cooldown == null;

        #if ODIN_INSPECTOR
        [BoxGroup(grpRuntime),DisplayAsString]
        [ShowIf(nameof(InspectorShowRuntime))]
        #endif
        public float runtimeCooldown => _runtimeCooldown;

        public string name => "Cooldown";

        public string title {
            get{
                if(!Enabled) return name;

                if(Application.isPlaying){
                    return $"{name} ({_runtimeCooldown}s)";
                }else{
                    return $"{name} ({duration}s)";
                }
            }
        }
        
        #endregion

        #region private
        private float _runtimeCooldown;
        private Coroutine CR_Cooldown;
        private ItemComponent _component;
        private MonoBehaviour _coroutineRunner;
        #endregion

        #region inspector
        private bool InspectorShowRuntime(){
            return Application.isPlaying;
        }
        #endregion

        #region methods
        public void SetComponent(ItemComponent component){
            _component = component;
        }
        public void RunCooldown(){
            StopCooldown();

            if(duration <= 0) return;

            Debug.Log($"[Inventory]{_component?.stack?.inventory?.name} Usage Run Cooldown {_component?.stack?.item?.id}, stack:{_component?.stack}");
            _coroutineRunner = _component?.inventory;
            CR_Cooldown = _coroutineRunner?.StartCoroutine(CoroutineCooldown());
        }
        public void StopCooldown(){
            if(CR_Cooldown == null) return;

             Debug.Log($"[Inventory]{_component?.stack?.inventory?.name} Usage Stop Cooldown {_component?.stack?.item?.id}, stack:{_component?.stack}");
            _coroutineRunner?.StopCoroutine(CR_Cooldown);
        }
        public ItemUseableCooldown CreateInstance()
        {
            return new ItemUseableCooldown(){
                Enabled = Enabled,
                duration = duration
            };
        }
        #endregion

        #region coroutine
        private IEnumerator CoroutineCooldown()
        {
            _runtimeCooldown = duration;
            while (_runtimeCooldown > 0)
            {
                _runtimeCooldown -= Time.deltaTime;
                yield return null;
            }

            _runtimeCooldown = 0;

            CR_Cooldown = null;
        }
        #endregion
    }
}
