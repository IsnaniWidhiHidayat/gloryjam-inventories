using System;
using System.Collections.Generic;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    [DisallowMultipleItemComponent]
    public class ItemDetailComponent : ItemComponent<ItemDetailComponent,ItemDetailHandler>
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

        #region property
        public override string name => "Details";
        public override int propertyOrder => 0;
        public override bool showID => false;
        public override bool requiredId => false;
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
    }
}