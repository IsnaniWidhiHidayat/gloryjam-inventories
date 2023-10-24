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
        #region fields
        public bool Enabled;
        #endregion

        #region property
        public ItemStack stack => _stack;
        public Item item => _item;
        #endregion

        #region protected
        protected ItemStack _stack;
        protected Item _item;
        #endregion

        #region property
        public abstract string ComponentName{get;}
        #endregion

        #region methods
        public virtual void Init(ItemStack stack){
            SetItemStack(stack);
            SetItem(stack.slot.item);
        }
        public virtual void SetItem(Item item){
            this._item = item;
        }
        public void SetItemStack(ItemStack stack){
            this._stack = stack;
        }
        public abstract void Dispose();
        public abstract ItemComponent CreateInstance();
        #endregion
    
        // public virtual void Resolve(){}
    }

    [Serializable]
    public abstract class ItemComponent<T,C> : ItemComponent 
    where T : ItemComponentHandler
    where C : ItemComponent<T,C> , new()
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
                    if(handlers[i] == null) continue;
                    handlers[i].SetComponent(this);
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
            var clone = new C();
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
        
        public static T GetComponent<T>(this Item item) where T : ItemComponent
        {
            var result = item.component.Find(x => x is T && x.Enabled) as T;
            result?.SetItem(item);
            return result;
        }
        public static T GetComponent<T>(this ItemStack stack) where T : ItemComponent
        {
            var result = stack.component.Find(x => x is T && x.Enabled) as T;
            return result;
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

