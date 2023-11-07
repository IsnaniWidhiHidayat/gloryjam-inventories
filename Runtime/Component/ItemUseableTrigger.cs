using System;
using GloryJam.DataAsset;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GloryJam.Inventories
{
    [Serializable]
    
    #if ODIN_INSPECTOR
    [HideReferenceObjectPicker,HideDuplicateReferenceBox]
    [Toggle("Enabled",CollapseOthersOnExpand = false)]
    #endif
    public abstract class ItemUseableTrigger : IInstance<ItemUseableTrigger>
    {
        #region const
        protected const string grpConfig = "Config";
        protected const string grpRequired = "Required";
        protected const string grpRuntime = "Runtime";
        protected const string grpDebug = "Debug";
        #endregion

        #region innerclass
        [Serializable]
        public enum Type{
            Manual,
            Instant,
            Custom,
        }
        #endregion

        #region fields
        public bool Enabled = true;
        #endregion

        #region property
        public Item item => component?.item;
        public ItemSlot slot => component?.slot;
        public ItemStack stack => component?.stack;
        public Inventory inventory => component?.inventory;
        public abstract string triggerName{get;}
        #endregion

        #region events
        public event Action onTrigger;
        #endregion

        #region private
        protected ItemUseableComponent component;
        #endregion

        #region inspector
        #if ODIN_INSPECTOR
        public bool InspectorShowRuntime(){
            return Application.isPlaying && stack != null;
        }
        #endif
        #endregion

        #region methods
        public void SetComponent(ItemUseableComponent component){
            this.component = component;
        }
        public void InvokeOnTrigger(){
            onTrigger?.Invoke();
        }
        #endregion

        #region callback
        public abstract void OnInit();
        public abstract void OnDispose();
        public abstract ItemUseableTrigger CreateInstance();
        #endregion
    }

    [Serializable]
    public abstract class ItemUseableTrigger<T> : ItemUseableTrigger
    where T : ItemUseableTrigger<T>, new()
    {
        public override ItemUseableTrigger CreateInstance()
        {
            var clone = new T(){
                Enabled = Enabled
            };

            return clone;
        }
    }
}