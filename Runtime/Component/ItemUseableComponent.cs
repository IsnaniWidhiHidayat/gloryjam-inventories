using System;
using UnityEngine;

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
        [BoxGroup(grpOptions),PropertyOrder(-1)]
        [LabelText("@maxUse.title")]
        #endif
        public ItemUseableMaxUse maxUse;

        #if ODIN_INSPECTOR
        [BoxGroup(grpOptions)]
        [LabelText("@cooldown.title")]
        #endif
        public ItemUseableCooldown cooldown;

        #if ODIN_INSPECTOR
        [BoxGroup(grpOptions)]
        [LabelText("@trigger.title")]
        #endif
        public  ItemUseableTrigger trigger;
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

        public override string name => "Usage";
        public override int order => 2;

        public override bool showID => false;
        public override bool requiredId => false;
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
            return cooldown.Enabled && cooldown.duration > 0 && InspectorShowRuntime();
        }
        #endif
        #endregion

        #region private
        private IItemUseable[] _itemUseables;
        private ItemConsumeComponent _consume;
        private ItemStateUsageHandler _stateHandler;
        private Func<ItemUsageHandler,bool> _handlesCondition;
        #endregion

        #region methods
        public override void SetStack(ItemStack stack)
        {
            base.SetStack(stack);

            maxUse.SetComponent(this);
            cooldown.SetComponent(this);
            trigger.SetComponent(this);
        }

        public virtual bool Use(Func<ItemUsageHandler,bool> condition = null){
            Debug.Log($"[Inventory]{stack?.item?.id} Use, stack:{stack}");

            //chekc max use
            if(maxUse.Enabled && !maxUse.isCanUse) return false;

            //cooldown
            if(cooldown.Enabled && !cooldown.isCanUse) return false;

            var prevInUse = inUse;
            var used = false;

            for (int i = 0; i < handlers.Count; i++)
            {
                if(handlers[i] == null)
                    continue;

                if(condition != null && !condition(handlers[i]))
                    continue;

                used |= handlers[i].Use();
            }

            var result = !prevInUse && used;

            if(result) {

                //Trigger Event
                Event.type  = ItemUseableEvent.Type.Use;
                Event.stack = stack;
                ItemUseableEvent.Trigger(inventory,Event);

                //ItemUseable
                if(_itemUseables?.Length > 0) {
                    for (int i = 0; i < _itemUseables.Length; i++)
                    {
                        _itemUseables[i]?.OnUse();
                    }
                }

                //running cooldown
                if(cooldown.Enabled) cooldown.RunCooldown();

                //increase used
                if(maxUse.Enabled) maxUse.IncreaseUse();

                //consume
                _consume?.Consume();
            }

            //save state
            if(_consume == null) _stateHandler?.SaveState();

            return result;
        }

        public virtual bool Unuse(Func<ItemUsageHandler,bool> condition = null){
            Debug.Log($"[Inventory]{stack?.item?.id} Unuse {stack}");
            
            var prevInUse = inUse;

            for (int i = 0; i < handlers.Count; i++)
            {
                if(handlers[i] == null)
                    continue;

                if(condition != null && !condition(handlers[i]))
                    continue;

                handlers[i].Unuse();
            }
            
            var result = prevInUse && !inUse;

            if(result){
                //Trigger Event
                Event.type  = ItemUseableEvent.Type.Unuse;
                Event.stack = stack;
                ItemUseableEvent.Trigger(inventory,Event);

                //ItemUseable
                if(_itemUseables?.Length > 0) {
                    for (int i = 0; i < _itemUseables.Length; i++)
                    {
                        _itemUseables[i]?.OnUnuse();
                    }
                }

                //stop cooldown
                if(cooldown.Enabled) cooldown.StopCooldown();
            }

            //save state
            if(_consume == null) _stateHandler?.SaveState();

            return result;
        }

        public override ItemComponent CreateInstance()
        {
            var clone = base.CreateInstance() as ItemUseableComponent;
                clone.maxUse    = maxUse.CreateInstance();
                clone.cooldown  = cooldown.CreateInstance();
                clone.trigger   = trigger.CreateInstance();

            return clone;
        }
        #endregion

        #region coroutine
        
        #endregion

        #region callback
        public override void OnInit()
        {
            base.OnInit();

            //get consume component
            _consume = stack.GetComponent<ItemConsumeComponent>();

            //get item useable
            _itemUseables = stack.GetComponents<IItemUseable>();

            //Get usage state handler
            if(stack.TryGetComponent<ItemStateComponent>(out var stateComponent)){
                _stateHandler = stateComponent.GetHandler<ItemStateUsageHandler>();
            }

            //listening trigger
            if(trigger.Enabled) {
                trigger.onTrigger -= OnTrigger;
                trigger.onTrigger += OnTrigger;
                trigger.StartListenTrigger();
            }
        }
        public override void OnPostInit(){
            
        }
        public override void OnDispose()
        {
            if(inUse){
                Unuse(x=> !x.persistent);
            }

            base.OnDispose();

            //iunlisten trigger
            if(trigger.Enabled) {
                trigger.onTrigger -= OnTrigger;
                trigger.StopListenTrigger();
            }
        }
        private void OnTrigger()
        {
            Debug.Log($"[Inventory]{stack?.item?.id} Usage Trigger, stack:{stack}");
            Use();
        }
        #endregion
    }
}