using System;
using System.Collections.Generic;

namespace GloryJam.Inventories
{
    public static class ItemComponentExtend
    {
        public static bool TryGetComponent<T>(this Item item,out T result)where T : class
        {
            result = item?.GetComponent<T>();
            return result != null;
        }
        public static bool TryGetComponent<T>(this ItemStack stack,out T result)where T : class
        {
            result = stack?.GetComponent<T>();
            return result != null;
        }
        
        public static bool TryGetComponents<T>(this Item item,out T[] result)where T : class
        {
            result = item?.GetComponents<T>();
            return result != null;
        }
        public static bool TryGetComponents<T>(this ItemStack stack,out T[] result)where T : class
        {
            result = stack?.GetComponents<T>();
            return result != null;
        }

        public static T GetComponent<T>(this Item item) where T : class
        {
            var result = item.component.Find(x => x as T != null && x.Enabled) as T;
            var component = result as ItemComponent;
            
            if(component != null && component.item == null){
                component.SetItem(item);
            }
            
            return result;
        }
        public static T GetComponent<T>(this ItemStack stack) where T : class
        {
            var result = stack.component.Find(x => x as T != null && x.Enabled) as T;
            return result;
        }

        public static T[] GetComponents<T>(this Item item) where T : class
        {
            var result = default(List<T>);
            for (int i = 0; i < item.component.Count; i++)
            {
                if(item.component[i] == null || !item.component[i].Enabled) continue;

                var component = item.component[i] as T;

                if(component == null) continue;
                if(result == null) result = new List<T>();
                
                result.Add(component);
            }

            return result == null ? default : result.ToArray();
        }
        public static T[] GetComponents<T>(this ItemStack stack) where T : class
        {
            var result = default(List<T>);
            for (int i = 0; i < stack.component.Count; i++)
            {
                if(stack.component[i] == null || !stack.component[i].Enabled) continue;

                var component = stack.component[i] as T;

                if(component == null) continue;
                if(result == null) result = new List<T>();

                result.Add(component);
            }

            return result == null ? default : result.ToArray();
        }

        public static bool ContainComponent<T>(this Item item) where T : class
        {
            var component = item.GetComponent<T>();
            return component != null;
        }
        public static bool ContainComponent<T>(this ItemStack stack) where T : class
        {
            var component = stack.GetComponent<T>();
            return component != null;
        }
    }
}

