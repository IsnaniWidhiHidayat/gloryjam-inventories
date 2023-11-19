using System;
using GloryJam.DataAsset;
using UnityEngine;

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemComponentHandler : IInstance<ItemComponentHandler>
    {
        #region const
        protected const string grpConfig = "Config";
        protected const string grpRuntime = "Runtime";
        protected const string grpRequired = "Required";
        protected const string grpDebug = "Debug";
        #endregion

        #region property
        public Item item => component?.item;
        public ItemSlot slot => component?.slot;
        public ItemStack stack => component?.stack;
        public Inventory inventory => component?.inventory;
        public abstract string name{get;}
        #endregion

        #region protected
        protected ItemComponent component;
        #endregion

        #region inspector
        #if ODIN_INSPECTOR
        public bool InspectorShowRuntime(){
            return Application.isPlaying;
        }
        #endif
        #endregion

        #region methods
        public virtual void SetComponent(ItemComponent component){
            this.component = component;
        }
        
        public virtual ItemComponentHandler CreateInstance(){
            return (ItemComponentHandler)MemberwiseClone();
        }
        #endregion

        #region callback
        public abstract void OnInit();
        public abstract void OnPostInit();
        public abstract void OnDispose();
        #endregion
    }
}