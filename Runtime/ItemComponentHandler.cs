using System;
using GloryJam.DataAsset;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

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

        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        [ValidateInput(nameof(InspectorValidateID),"Required")]
        [ShowIf(nameof(showID))]
        #endif
        public string id;

        #endregion

        #region property
        public Item item => component?.item;
        public ItemSlot slot => component?.slot;
        public ItemStack stack => component?.stack;
        public Inventory inventory => component?.inventory;
        public abstract string name{get;}
        public virtual string title {
            get{
                if(!string.IsNullOrEmpty(id)){
                    return $"{name} [{id}]";
                }else{
                    return name;
                }
            }
        }
        public virtual bool requiredId => false;
        public virtual bool showID => true;
        public virtual int order => 0;
        #endregion

        #region protected
        protected ItemComponent component;
        #endregion

        #region inspector
        #if ODIN_INSPECTOR
        public bool InspectorShowRuntime(){
            return Application.isPlaying;
        }
        private bool InspectorValidateID(string id)
        {
            if(requiredId && string.IsNullOrEmpty(id)){
                return false;
            }

            return true;
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