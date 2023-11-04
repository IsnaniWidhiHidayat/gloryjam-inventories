using System.Collections.Generic;

namespace GloryJam.Inventories
{
    public static class IInstanceExtend
    {
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
        public static T[] CreateInstance<T>(this T[] items) where T : IInstance<T>
        {
            var result = new T[items.Length];
            for (int i = 0; i < items.Length; i++)
            {
                if(items[i] == null) continue;
                result[i] = items[i].CreateInstance(); 
            }
            return result;
        }
    }
}