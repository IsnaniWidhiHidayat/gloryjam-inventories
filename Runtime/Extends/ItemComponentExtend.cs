using System.Collections.Generic;

namespace GloryJam.Inventories
{
    public static class ItemComponentExtend
    {
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
    }
}

