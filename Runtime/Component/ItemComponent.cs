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
    #if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
    [HideDuplicateReferenceBox]
    [Toggle("Enabled",CollapseOthersOnExpand = false)]
    #endif
    public abstract class ItemComponent : IInstance<ItemComponent>
    {
        #region const
        protected const string grpConfig = "Config";
        protected const string grpRequired = "Required";
        protected const string grpRuntime = "Runtime";
        protected const string grpDebug = "Debug";
        protected const string grpOptions = "Options";
        #endregion

        #region fields
        public bool Enabled = true;
        
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig),PropertyOrder(-1)]
        [ShowIf(nameof(showID))]
        #endif
        public string id;
        #endregion

        #region property
        public Item item => _stack != null ? _stack.item : _item;
        public ItemSlot slot => _stack?.slot;
        public ItemStack stack => _stack;
        public Inventory inventory => slot?.inventory;
        
        public abstract string name{get;}
        public virtual string title{
            get{
                return string.IsNullOrEmpty(id) ? name : $"{name} [{id}]";
            }
        }
        public virtual int order => 0;
        public virtual bool requiredId => false;
        public virtual bool showID => true;
        #endregion

        #region protected
        protected ItemStack _stack;
        private Item _item;
        #endregion

        #region inspector
        #if ODIN_INSPECTOR
        public bool InspectorShowRuntime(){
            return Application.isPlaying && stack != null;
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
        public virtual void SetStack(ItemStack stack){
            _stack = stack;
        }
        public virtual void SetItem(Item item){
            _item = item;
        }
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
                id = id,
                Enabled = Enabled
            };
        }
    }

    [Serializable]
    public abstract class ItemComponent<T,H> : ItemComponent<T> ,ISort
    where T : ItemComponent<T,H>,new()
    where H : ItemComponentHandler
    {
        #region fields
        #if ODIN_INSPECTOR
        [ListDrawerSettings(DraggableItems = false,Expanded = true,ListElementLabelName = "title")]
        [HideDuplicateReferenceBox,HideReferenceObjectPicker]
        [ValidateInput(nameof(InspectorValidateHandlers),"Please remove empty handler")]
        [OnValueChanged(nameof(InspectorOnComponentValueChange))]
        [Space(1)]
        #endif
        public List<H> handlers = new List<H>();
        #endregion

        #region property
        public H this[int index]
        {
            get { return handlers[index]; }
        }
        #endregion

        #region inspector
        private bool InspectorValidateHandlers(List<H> handlers)
        {
            return handlers == null ? true : !handlers.Exists(x => x == null);
        }
        private void InspectorOnComponentValueChange()
        {
            Sort();
            SetItem(item);
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

        public void Sort(){
            handlers.Sort((x,y) =>{
                if(x == null || y == null) return -1;
                return x.order.CompareTo(y.order);
            });
        }
        
        public bool ContainHandler<T1>(Func<T1,bool> condition = null,bool deep = false) where T1 : H
        {
            return TryGetHandler(out var handler,condition,deep);
        }

        public T1 GetHandler<T1>(Func<T1,bool> condition = null,bool deep = false) where T1 : H
        {
            if(TryGetHandler(out var result,condition,deep)) return result;
            return default;
        }
        public T1[] GetHandlers<T1>(Func<T1,bool> condition = null,bool deep = false) where T1 : H
        {
            if(TryGetHandlers(out var result,condition,deep)) return result;
            return default;
        }
        
        public bool TryGetHandler<T1>(out T1 result,Func<T1,bool> condition = null,bool deep = false) where T1 : H
        {
            result = default;

            for (int i = 0; i < handlers.Count; i++)
            {
                //handler
                if(handlers[i] is T1){

                    var handler = handlers[i] as T1;

                    //check null
                    if(handler == null) continue;

                    //check condition
                    if(condition != null){
                        if(condition(handler)){
                            result = handler;
                            break;
                        }
                    }else{
                        result = handler;
                        break;
                    }
                }
                //check deep
                else if(deep && handlers[i] is IHandlers<H>){
                    //get deep handlers
                    var deepHandlers = (handlers[i] as IHandlers<H>).GetHandlers();

                    //check handlers
                    if(deepHandlers != null && deepHandlers?.Count > 0) {
                        for (int j = 0; j < deepHandlers.Count; j++)
                        {
                            var handler = deepHandlers[j] as T1;

                            //check null
                            if(handler == null) continue;

                            //check condition
                            if(condition != null){
                                if(condition(handler)){
                                    result = handler;
                                    break;
                                }
                            }else{
                                result = handler;
                                break;
                            }
                        }
                    }
                }

                if(result != null) break;
            }

            return result != null;
        }
        public bool TryGetHandlers<T1>(out T1[] result,Func<T1,bool> condition = null,bool deep = false) where T1 : H
        {
            result = default;

            var listResult = new List<T1>();

            for (int i = 0; i < handlers.Count; i++)
            {
                //handler
                if(handlers[i] is T1){

                    var handler = handlers[i] as T1;

                    //check null
                    if(handler == null) continue;

                    //check condition
                    if(condition != null){
                        if(condition(handler)){
                            listResult.Add(handler);
                        }
                    }else{
                        listResult.Add(handler);
                    }
                }
                //check deep
                else if(deep && handlers[i] is IHandlers<H>){
                    //get deep handlers
                    var deepHandlers = (handlers[i] as IHandlers<H>).GetHandlers();

                    //check handlers
                    if(deepHandlers != null && deepHandlers?.Count > 0) {
                        for (int j = 0; j < deepHandlers.Count; j++)
                        {
                            var handler = deepHandlers[j] as T1;

                            //check null
                            if(handler == null) continue;

                            //check condition
                            if(condition != null){
                                if(condition(handler)){
                                    listResult.Add(handler);
                                }
                            }else{
                                listResult.Add(handler);
                            }
                        }
                    }
                }
            }

            //check list result
            if(listResult?.Count > 0){
                result = listResult.ToArray();
            }

            return result != null && result.Length > 0;
        }
        
        public string[] GetHandlersID(Func<H,bool> condition = null,bool deep = false)
        {
            if(TryGetHandlers(out var handlers,condition,deep))
            {
                var result = new List<string>();
                for (int i = 0; i < handlers.Length; i++)
                {
                    if(string.IsNullOrEmpty(handlers[i].id)) continue;
                    result.Add(handlers[i].id);
                }

                result.Sort((x,y)=> x.CompareTo(y));
                return result.ToArray();
            }

            return default;
        }
        public string[] GetHandlersID<T1>(Func<T1,bool> condition = null,bool deep = false) where T1: H
        {
            if(TryGetHandlers(out T1[] handlers,condition,deep))
            {
                var result = new List<string>();
                for (int i = 0; i < handlers.Length; i++)
                {
                    if(string.IsNullOrEmpty(handlers[i].id)) continue;
                    result.Add(handlers[i].id);
                }

                result.Sort((x,y)=> x.CompareTo(y));
                return result.ToArray();
            }

            return default;
        }
        
        public override ItemComponent CreateInstance()
        {
            var clone = base.CreateInstance() as T;
            
            if(handlers?.Count > 0){
                if(clone.handlers == null) clone.handlers = new List<H>();

                for (int i = 0; i < handlers.Count; i++){
                    if(handlers[i] == null) continue;
                    clone.handlers.Add(handlers[i].CreateInstance() as H);
                }
            }

            clone?.Sort();

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
}

