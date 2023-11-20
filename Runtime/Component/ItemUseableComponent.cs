using System;
using UnityEngine;
using GloryJam.DataAsset;
using System.Collections;



#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{   
    [Serializable,DisallowMultipleItemComponent]
    public class ItemUseableComponent : ItemComponent<ItemUseableComponent,ItemUsageHandler>
    {
        #region static
        private static ItemUseableEvent Event = new ItemUseableEvent();
        #endregion

        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        #endif
        public int maxUse;

        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        #endif
        public float cooldown;

        #if ODIN_INSPECTOR
        [BoxGroup("Trigger"),HideReferenceObjectPicker,HideDuplicateReferenceBox,HideLabel]
        [PropertyOrder(-1)]
        #endif
        public ItemTrigger trigger = new ItemTrigger();
        #endregion

        #region property
        #if ODIN_INSPECTOR
        [ShowInInspector,ShowIf(nameof(InspectorShowRuntime)),BoxGroup(grpRuntime),DisplayAsString]
        #endif
        public bool inUse{
            get{
                var result = false;

                for (int i = 0; i < handlers.Count; i++)
                {
                    if(handlers[i] == null)
                        continue;

                    result |= handlers[i].inUse;
                    if(result) break;
                }

                return result;
            }
        }

        #if ODIN_INSPECTOR
        [ShowInInspector,ShowIf(nameof(InspectorShowRuntime)),BoxGroup(grpRuntime),DisplayAsString]
        #endif
        public int runtimeUsed => _runtimeUsed;

        #if ODIN_INSPECTOR
        [ShowInInspector,ShowIf(nameof(InspectorShowCooldown)),BoxGroup(grpRuntime),DisplayAsString]
        #endif
        public float runtimeCooldown => _runtimeCooldown;

        public override string name => "Usage";
        public override int propertyOrder => 2;
        #endregion

        #region inspector
        #if ODIN_INSPECTOR
        [Button("Use"),PropertyOrder(100),BoxGroup(grpDebug),ButtonGroup(grpDebug + "/Buttons"),ShowIf(nameof(InspectorShowRuntime))]
        private void InspectorUse(){
            Use();
        }

        [Button("Unuse"),ButtonGroup(grpDebug + "/Buttons"),ShowIf(nameof(InspectorShowRuntime))]
        private void InspectorUnUse(){
            Unuse();
        }
        private bool InspectorValidateTriggers(ItemTriggerHandler[] triggers)
        {
            return triggers == null ? true : !Array.Exists(triggers, x => x == null);
        }
        private bool InspectorShowCooldown(){
            return cooldown > 0 && InspectorShowRuntime();
        }
        #endif
        #endregion

        #region private
        private Coroutine CR_Cooldown;
        private float _runtimeCooldown;
        private int _runtimeUsed;
        private bool _isComponentRearMost; 
        private IItemUseable[] _useables;
        private ItemConsumeComponent _consume;
        #endregion

        #region methods
        public override void SetStack(ItemStack stack)
        {
            base.SetStack(stack);

            //set trigger component
            trigger?.SetComponent(this);
        }

        public virtual bool Use(){
            //chekc max use
            if(maxUse > 0 && _runtimeUsed >= maxUse) return false;

            //cooldown
            if(cooldown > 0 && CR_Cooldown != null) return false;

            var prevInUse = inUse;
            
            for (int i = 0; i < handlers.Count; i++)
            {
                if(handlers[i] == null)
                    continue;

                handlers[i].Use();
            }

            var result = !prevInUse && inUse;

            //Trigger use
            if(result) {
                Event.type  = ItemUseableEvent.Type.Use;
                Event.stack = stack;
                ItemUseableEvent.Trigger(inventory,Event);
            }

            //running cooldown
            if(result && cooldown > 0) {
                CR_Cooldown = inventory?.StartCoroutine(CoroutineCooldown());
            }

            //increase used
            if(result){
                _runtimeUsed++;
            }

            //check if component is rear most in this stack
            if(result && _isComponentRearMost) {
                //Get IItem Useable and invoke Use
                if(_useables?.Length > 0){
                    for (int i = 0; i < _useables.Length; i++)
                    {
                        _useables[i]?.OnUse();
                    }
                }

                //consume item
                if(maxUse > 0){
                    if(_runtimeUsed >= maxUse){
                        _consume?.Consume();
                    }
                }else{
                    _consume?.Consume();
                }
            }

            return result;
        }

        public virtual bool Unuse(){
            var prevInUse = inUse;
            var result = false;

            for (int i = 0; i < handlers.Count; i++)
            {
                if(handlers[i] == null)
                    continue;

                result |= handlers[i].Unuse();
            }
            
            //Trigger unuse
            if(prevInUse && !inUse){
                Event.type  = ItemUseableEvent.Type.Unuse;
                Event.stack = stack;
                ItemUseableEvent.Trigger(inventory,Event);
            }

            var useableComponents = stack.GetComponents<ItemUseableComponent>();
            var isRearmost = Array.IndexOf(useableComponents,this) == useableComponents.Length - 1;

            if(isRearmost) {
                //Get IItem Useable and invoke Unuse
                if(result && stack.TryGetComponents<IItemUseable>(out var useables)){
                    for (int i = 0; i < useables.Length; i++)
                    {
                        if(useables[i] == null) continue;
                        useables[i].OnUnuse();
                    }
                }
            }
            
            return result;
        }

        public override ItemComponent CreateInstance()
        {
            var clone = base.CreateInstance() as ItemUseableComponent;
                clone.maxUse    = maxUse;
                clone.cooldown  = cooldown;
                clone.trigger   = trigger?.CreateInstance();

            return clone;
        }
        #endregion

        #region coroutine
        private IEnumerator CoroutineCooldown()
        {
            _runtimeCooldown = cooldown;
            while (runtimeCooldown > 0)
            {
                _runtimeCooldown -= Time.deltaTime;
                yield return null;
            }

            _runtimeCooldown = 0;

            CR_Cooldown = null;
        }
        #endregion

        #region callback
        public override void OnInit()
        {
            base.OnInit();

            //listening trigger
            if(trigger != null) {
                trigger.onTrigger -= OnTrigger;
                trigger.onTrigger += OnTrigger;
                trigger.StartListenTrigger();
            }

            //check if component is rear most in this stack
            var useableComponents = stack.GetComponents<ItemUseableComponent>();
            _isComponentRearMost  = Array.IndexOf(useableComponents,this) == useableComponents.Length - 1;

            //component useables & consume
            if(_isComponentRearMost){
                _useables = stack.GetComponents<IItemUseable>();
                _consume  = stack.GetComponent<ItemConsumeComponent>();
            }
        }
        public override void OnPostInit(){
            if(trigger != null && trigger.type == ItemTrigger.Type.Instant && inventory != null){
                OnTrigger();
            }
        }
        public override void OnDispose()
        {
            if(inUse) Unuse();

            base.OnDispose();

            //iunlisten trigger
            if(trigger != null) {
                trigger.onTrigger -= OnTrigger;
                trigger.StopListenTrigger();
            }
        }
        private void OnTrigger()
        {
            Use();
        }
        #endregion
    }
}