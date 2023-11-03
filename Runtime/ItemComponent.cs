using System;
using System.Collections.Generic;

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
        #endif
        #endregion

        #region methods
        public virtual void Init(ItemStack stack){
            _stack = stack;
        }
        public virtual void SetItem(Item item){
            _item = item;
        }
        public abstract void SaveState();
        public abstract void LoadState();
        public abstract void Dispose();
        public virtual ItemComponent CreateInstance(){
            return (ItemComponent)MemberwiseClone();
        }
        #endregion
    }

    [Serializable]
    public abstract class ItemComponent<T> : ItemComponent 
    where T : ItemComponentHandler
    {
        #region fields
        #if ODIN_INSPECTOR
        [ListDrawerSettings(DraggableItems = false)]
        #endif
        public List<T> handlers = new List<T>();
        #endregion

        #region property
        public T this[int index]
        {
            get { return handlers[index]; }
        }
        #endregion

        #region constructor
        public ItemComponent(){
            handlers = new List<T>();
        }
        #endregion

        #region methods
        public override void Init(ItemStack stack)
        {
            base.Init(stack);

            if(handlers?.Count > 0) {
                for (int i = 0; i < handlers.Count; i++)
                {
                    if(handlers[i] == null) continue;
                    handlers[i].Init(this);
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
        public override void Dispose()
        {
            if(handlers?.Count > 0) {
                for (int i = 0; i < handlers.Count; i++)
                {
                    if(handlers[i] == null) continue;
                    handlers[i].Dispose();
                }
            }
        }
        public bool ContainHandler<T1>() where T1 : T
        {
            return handlers.Exists(x => x != null && x is T1);
        }
        public T1 GetHandler<T1>() where T1 : T
        {
            if(TryGetHandler<T1>(out var result)) return result;
            return default;
        }
        public T1[] GetHandlers<T1>() where T1 : T
        {
            if(TryGetHandlers<T1>(out var result)) return result;
            return default;
        }
        public bool TryGetHandler<T1>(out T1 result) where T1 : T
        {
            result = handlers.Find(x => x != null && x is T1) as T1;
            return result != null;
        }
        public bool TryGetHandlers<T1>(out T1[] result) where T1 : T
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
            var clone = base.CreateInstance() as ItemComponent<T>;
                clone.Enabled = Enabled;
            
            if(handlers?.Count > 0){
                if(clone.handlers == null) clone.handlers = new List<T>();

                for (int i = 0; i < handlers.Count; i++){
                    if(handlers[i] == null) continue;
                    clone.handlers.Add(handlers[i].CreateInstance() as T);
                }
            }

            return clone;
        }
        #endregion
    }

    [Serializable]
    public abstract class ItemComponent<T,S> : ItemComponent<T>
    where T : ItemComponentHandler
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
            var r =  base.CreateInstance() as ItemComponent<T,S>;
                r.state = new S();
            return r;
        }
        #endregion
    }

    public static class ItemComponentExtend
    {
        #region static
        public static bool TryGetComponent<T>(this Item item,out T result)where T : ItemComponent
        {
            result = item?.GetComponent<T>();
            return result != null;
        }
        public static bool TryGetComponent<T>(this ItemStack stack,out T result)where T : ItemComponent
        {
            result = stack?.GetComponent<T>();
            return result != null;
        }
        
        public static bool TryGetComponents<T>(this Item item,out T[] result)where T : ItemComponent
        {
            result = item?.GetComponents<T>();
            return result != null;
        }
        public static bool TryGetComponents<T>(this ItemStack stack,out T[] result)where T : ItemComponent
        {
            result = stack?.GetComponents<T>();
            return result != null;
        }

        public static T GetComponent<T>(this Item item) where T : ItemComponent
        {
            var result = item.component.Find(x => x as T != null && x.Enabled) as T;
            if(result != null && result.item == null){
                result?.SetItem(item);
            }
            return result;
        }
        public static T GetComponent<T>(this ItemStack stack) where T : ItemComponent
        {
            var result = stack.component.Find(x => x as T != null && x.Enabled) as T;
            return result;
        }

        public static T[] GetComponents<T>(this Item item) where T : ItemComponent
        {
            var result = default(List<T>);
            for (int i = 0; i < item.component.Count; i++)
            {
                var component = item.component[i] as T;
                if(component == null || !component.Enabled) continue;
                if(result == null) result = new List<T>();
                result.Add(component);
            }

            return result == null ? default : result.ToArray();
        }
        public static T[] GetComponents<T>(this ItemStack stack) where T : ItemComponent
        {
            var result = default(List<T>);
            for (int i = 0; i < stack.component.Count; i++)
            {
                var component = stack.component[i] as T;
                if(component == null || !component.Enabled) continue;
                if(result == null) result = new List<T>();
                result.Add(component);
            }

            return result == null ? default : result.ToArray();
        }


        public static bool ContainComponent<T>(this Item item) where T : ItemComponent
        {
            var component = item.GetComponent<T>();
            return component != null && component.Enabled;
        }
        public static bool ContainComponent<T>(this ItemStack stack) where T : ItemComponent
        {
            var component = stack.GetComponent<T>();
            return component != null && component.Enabled;
        }

        public static List<T> CreateInstance<T>(this List<T> items) where T : IInstance<T>
        {
            var result = new List<T>();
            for (int i = 0; i < items.Count; i++)
            {
                if(items[i] == null) continue;
                result.Add(items[i].CreateInstance()); 
            }
            return result;
        }
        #endregion
    }
}

