using System;
using System.Collections.Generic;

namespace GloryJam.Inventories
{
    public static class ItemComponentExtend
    {
        public static bool TryGetComponent<T>(this Item item,out T result,Func<T,bool> condition = null)where T : class
        {
            result = item?.GetComponent(condition);
            return result != null;
        }
        public static bool TryGetComponent<T>(this ItemStack stack,out T result,Func<T,bool> condition = null)where T : class
        {
            result = stack?.GetComponent(condition);
            return result != null;
        }
        
        public static bool TryGetComponent<T>(this Item item,string id,out T result)where T : class
        {
            result = item?.GetComponent<T>(id);
            return result != null;
        }
        public static bool TryGetComponent<T>(this ItemStack stack,string id,out T result)where T : class
        {
            result = stack?.GetComponent<T>(id);
            return result != null;
        }

        public static bool TryGetComponents<T>(this Item item,out T[] result,Func<T,bool> condition = null)where T : class
        {
            result = item?.GetComponents(condition);
            return result != null;
        }
        public static bool TryGetComponents<T>(this ItemStack stack,out T[] result,Func<T,bool> condition = null)where T : class
        {
            result = stack?.GetComponents(condition);
            return result != null;
        }

        public static T GetComponent<T>(this Item item,Func<T,bool> condition = null) where T : class
        {
            if(item == null) return default;

            var result = item.component.Find(x => x as T != null && x.Enabled && (condition != null ? condition(x as T) : true)) as T;
            var component = result as ItemComponent;
            
            if(component != null && component.item == null){
                component.SetItem(item);
            }
            
            return result;
        }
        public static T GetComponent<T>(this ItemStack stack,Func<T,bool> condition = null) where T : class
        {
            if(stack == null) return default;
            return stack.component.Find(x => x as T != null && x.Enabled && (condition != null ? condition(x as T) : true)) as T;
        }

        public static T GetComponent<T>(this Item item,string id) where T : class
        {
            if(item == null) return default;
            return item.component.Find(x => x as T != null && x.id == id && x.Enabled) as T;
        }
        public static T GetComponent<T>(this ItemStack stack,string id) where T : class
        {
            if(stack == null) return default;
            return stack.component.Find(x => x as T != null && x.id == id && x.Enabled) as T;
        }
        
        public static ItemComponent GetComponent(this Item item,string id){
            if(item == null) return default;
            return item.component.Find(x => x != null && x.id == id && x.Enabled);
        }
        public static ItemComponent GetComponent(this ItemStack stack,string id){
            if(stack == null) return default;
            return stack.component.Find(x => x != null && x.id == id && x.Enabled);
        }
        
        public static T[] GetComponents<T>(this Item item,Func<T,bool> condition = null) where T : class
        {
            var result = default(List<T>);
            for (int i = 0; i < item.component.Count; i++)
            {
                if(item.component[i] == null || !item.component[i].Enabled) continue;

                var component = item.component[i] as T;

                if(component == null) continue;
                if(condition != null && !condition(component)) continue;
                if(result == null) result = new List<T>();
                
                result.Add(component);
            }

            return result == null ? default : result.ToArray();
        }
        public static T[] GetComponents<T>(this ItemStack stack,Func<T,bool> condition = null) where T : class
        {
            var result = default(List<T>);
            for (int i = 0; i < stack.component.Count; i++)
            {
                if(stack.component[i] == null || !stack.component[i].Enabled) continue;

                var component = stack.component[i] as T;

                if(component == null) continue;
                if(condition != null && !condition(component)) continue;
                if(result == null) result = new List<T>();

                result.Add(component);
            }

            return result == null ? default : result.ToArray();
        }

        public static bool ContainComponent<T>(this Item item,Func<T,bool> condition = null) where T : class
        {
            var component = item.GetComponent(condition);
            return component != null;
        }
        public static bool ContainComponent<T>(this ItemStack stack,Func<T,bool> condition = null) where T : class
        {
            var component = stack.GetComponent(condition);
            return component != null;
        }
    
        public static bool ContainComponent<T>(this Item item,string id) where T : class
        {
            var component = item.GetComponent<T>(id);
            return component != null;
        }
        public static bool ContainComponent<T>(this ItemStack stack,string id) where T : class
        {
            var component = stack.GetComponent<T>(id);
            return component != null;
        }
    
        public static string[] GetComponentsID<T>(this Item item,Func<T,bool> condition = null) where T : ItemComponent
        {
            if(item.TryGetComponents<T>(out var components)){
                var result = new List<string>();

                for (int i = 0; i < components.Length; i++)
                {
                    if(string.IsNullOrEmpty(components[i].id)) continue;
                    if(condition != null && !condition(components[i])) continue;
                    result.Add(components[i].id);
                }

                result.Sort((x,y)=> x.CompareTo(y));

                return result.ToArray();
            }

            return default;
        }
        public static string[] GetComponentsID<T>(this ItemStack stack,Func<T,bool> condition = null) where T : ItemComponent
        {
            if(stack.TryGetComponents<T>(out var components)){
                var result = new List<string>();

                for (int i = 0; i < components.Length; i++)
                {
                    if(string.IsNullOrEmpty(components[i].id)) continue;
                    if(condition != null && !condition(components[i])) continue;
                    result.Add(components[i].id);
                }

                result.Sort((x,y)=> x.CompareTo(y));

                return result.ToArray();
            }

            return default;
        }
    }
}

