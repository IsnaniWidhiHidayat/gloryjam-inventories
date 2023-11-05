using System;
using System.Collections.Generic;
using GloryJam.DataAsset;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemComponent : IInstance<ItemComponent>
    {
        #region const
        protected const string grpConfig = "Config";
        protected const string grpRequired = "Required";
        protected const string grpRuntime = "Runtime";
        protected const string grpDebug = "Debug";
        #endregion

        #region fields
        public bool Enabled;
        
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        #endif
        public string id;
        #endregion

        #region property
        public Item item => _stack != null ? _stack.item : _item;
        public ItemSlot slot => _stack?.slot;
        public ItemStack stack => _stack;
        public Inventory inventory => slot?.inventory;
        
        public abstract string ComponentName{get;}
        public abstract int ComponentPropertyOrder{get;}
        #endregion

        #region protected
        protected ItemStack _stack;
        private Item _item;
        #endregion

        #region inspector
        #if ODIN_INSPECTOR
        public string InspectorGetComponentName(){
            return string.IsNullOrEmpty(id) ? ComponentName : $"{ComponentName} [{id}]";
        }
        public bool InspectorShowRuntime(){
            return Application.isPlaying;
        }
        #endif
        #endregion

        #region methods
        public virtual void SetStack(ItemStack stack){
            _stack = stack;
        }
        public virtual void SetItem(Item item){
            _item = item;
        }
        public abstract void SaveState();
        public abstract void LoadState();
        public abstract ItemComponent CreateInstance();
        #endregion

        #region callback
        public abstract void OnInit();
        public abstract void OnPostInit();
        public abstract void OnDispose();
        #endregion
    }

    [Serializable]
    public abstract class ItemComponent<T> : ItemComponent
    where T : ItemComponent<T>,new()
    {
        public override ItemComponent CreateInstance()
        {
            return new T(){
                id = id
            };
        }
    }

    [Serializable]
    public abstract class ItemComponent<T,H> : ItemComponent<T> 
    where T : ItemComponent<T,H>,new()
    where H : ItemComponentHandler
    {
        #region fields
        #if ODIN_INSPECTOR
        [ListDrawerSettings(DraggableItems = false)]
        #endif
        public List<H> handlers = new List<H>();
        #endregion

        #region property
        public H this[int index]
        {
            get { return handlers[index]; }
        }
        #endregion

        #region constructor
        public ItemComponent(){
            handlers = new List<H>();
        }
        #endregion

        #region methods
        public override void SetStack(ItemStack stack)
        {
            base.SetStack(stack);

            if(handlers?.Count > 0) {
                for (int i = 0; i < handlers.Count; i++)
                {
                    if(handlers[i] == null) continue;
                    handlers[i].SetComponent(this);
                }
            }
        }
        public override void SetItem(Item item)
        {
            base.SetItem(item);

            if(handlers?.Count > 0) {
                for (int i = 0; i < handlers.Count; i++)
                {
                    if(handlers[i] == null || handlers[i].item != null) continue;
                    handlers[i].SetComponent(this);
                }
            }
        }
        public override void SaveState(){
            if(handlers?.Count > 0) {
                for (int i = 0; i < handlers.Count; i++)
                {
                    if(handlers[i] == null) continue;
                    handlers[i].SaveState();
                }
            }
        }
        public override void LoadState(){
            if(handlers?.Count > 0) {
                for (int i = 0; i < handlers.Count; i++)
                {
                    if(handlers[i] == null) continue;
                    handlers[i].LoadState();
                }
            }
        }
        public bool ContainHandler<T1>() where T1 : H
        {
            return handlers.Exists(x => x != null && x is T1);
        }
        public T1 GetHandler<T1>() where T1 : H
        {
            if(TryGetHandler<T1>(out var result)) return result;
            return default;
        }
        public T1[] GetHandlers<T1>() where T1 : H
        {
            if(TryGetHandlers<T1>(out var result)) return result;
            return default;
        }
        public bool TryGetHandler<T1>(out T1 result) where T1 : H
        {
            result = handlers.Find(x => x != null && x is T1) as T1;
            return result != null;
        }
        public bool TryGetHandlers<T1>(out T1[] result) where T1 : H
        {
            result = default;

            var tmp = handlers.FindAll(x => x != null && x is T1);
            
            if(tmp?.Count > 0){
                result = new T1[tmp.Count];
                for (int i = 0; i < result.Length; i++)
                {
                    result[i] = tmp[i] as T1;
                }

                return result != null;
            }

            return default;
        }
        public override ItemComponent CreateInstance()
        {
            var clone = base.CreateInstance() as T;
                clone.Enabled = Enabled;
            
            if(handlers?.Count > 0){
                if(clone.handlers == null) clone.handlers = new List<H>();

                for (int i = 0; i < handlers.Count; i++){
                    if(handlers[i] == null) continue;
                    clone.handlers.Add(handlers[i].CreateInstance() as H);
                }
            }

            return clone;
        }
        #endregion

        #region callback
        public override void OnInit()
        {
            if(handlers?.Count > 0) {
                for (int i = 0; i < handlers.Count; i++)
                {
                    if(handlers[i] == null) continue;
                    handlers[i].OnInit();
                }
            }
        }
        public override void OnPostInit()
        {
            if(handlers?.Count > 0) {
                for (int i = 0; i < handlers.Count; i++)
                {
                    if(handlers[i] == null) continue;
                    handlers[i].OnPostInit();
                }
            }
        }
        public override void OnDispose()
        {
            if(handlers?.Count > 0) {
                for (int i = 0; i < handlers.Count; i++)
                {
                    if(handlers[i] == null) continue;
                    handlers[i].OnDispose();
                }
            }
        }
        #endregion
    }

    [Serializable]
    public abstract class ItemComponent<T,H,S> : ItemComponent<T,H>
    where T : ItemComponent<T,H,S>,new()
    where H : ItemComponentHandler
    where S : ItemComponentState,new()
    {
        #region fields
        #if ODIN_INSPECTOR
        [ShowIf(nameof(state)),BoxGroup("State"),HideLabel,PropertyOrder(-1)]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        #endif
        public S state;
        #endregion

        #region methods
        public override ItemComponent CreateInstance()
        {
            var r =  base.CreateInstance() as T;
                r.state = new S();
            return r;
        }
        #endregion
    }
}

