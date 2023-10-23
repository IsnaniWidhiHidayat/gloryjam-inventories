using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    #if ODIN_INSPECTOR
    [Serializable,HideReferenceObjectPicker]
    [Toggle("Enabled",CollapseOthersOnExpand = false)]
    #endif
    public class ItemDetailComponent : ItemComponent<ItemDetailHandler,ItemDetailComponent>
    {
        #region inner class
        [Serializable]
        public struct Handler 
        {
            #region fields
            #if ODIN_INSPECTOR
            [ShowInInspector,DisplayAsString(false),PropertyOrder(-1),TableColumnWidth(80)]
            #endif
            public string name {
                get{
                    if(handler == null) return null;
                    return handler.name;
                }
            }
            
            #if ODIN_INSPECTOR
            [HideDuplicateReferenceBox]
            #endif
            public ItemDetailHandler handler;
            #endregion

            #region constructor
            public Handler(ItemDetailHandler handler){
                this.handler = handler;
            }
            #endregion
        }
        #endregion

        #region fields
        public override string ComponentName => "Details";
        #endregion

        #region property
        #if ODIN_INSPECTOR
        [HideReferenceObjectPicker,ShowInInspector,TableList()]
        #endif
        private List<Handler> summary{
            get{
                var result = new List<Handler>();
                if(handlers?.Count > 0) {
                    for (int i = 0; i < handlers.Count; i++)
                    {
                        result.Add(new Handler(handlers[i]));
                    }
                }
                return result;
            }
        }
        #endregion

        #region constructor
        public ItemDetailComponent(){
            handlers = new List<ItemDetailHandler>();
        }
        #endregion

        #region methods
        public T2 GetDetailValue<T1, T2>() where T1 : ItemDetailHandler<T2>
        {
            var handler = GetHandler<T1>();
            if(handler == null) return default;
            return handler.GetValue();
        }
        public Dictionary<string,T> GetDetailsValue<T>(){
            var result = default(Dictionary<string,T>);
            for (int i = 0; i < handlers.Count; i++)
            {
                if(handlers[i] == null || string.IsNullOrEmpty(handlers[i].name)) continue;
                
                if(result == null) result = new Dictionary<string,T>();

                var obj = handlers[i].GetValueObject();

                result[handlers[i].name] = obj is T ? (T)obj : default;
            }

            return result;
        }
        #endregion

        // public override void Resolve()
        // {
        //     handlers = new List<ItemDetailHandler>();

        //     if(handlers?.Count > 0) {
        //         for (int i = 0; i < handlers.Count; i++)
        //         {
        //             handlers.Add(handlers[i]);
        //         }
        //     }
        // }
    }

    public static class ItemDetailComponentExtend
    {
        public static bool TryGetComponentDetail(this ItemStack stack,out ItemDetailComponent result){
            result = stack?.GetComponent<ItemDetailComponent>();
            return result != null;
        }
        public static bool TryGetComponentDetail(this Item item,out ItemDetailComponent result){
            result = item?.GetComponent<ItemDetailComponent>();
            result?.SetItem(item);
            return result != null;
        }
    
        public static bool ContainComponentDetail(this ItemStack stack){
            return stack.ContainComponent<ItemDetailComponent>();
        }
        public static bool ContainComponentDetail(this Item item){
            return item.ContainComponent<ItemDetailComponent>();
        }
    
    }
}