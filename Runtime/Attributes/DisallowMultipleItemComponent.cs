using System;
using System.Collections.Generic;
using Sirenix.Utilities;

namespace GloryJam.Inventories
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class DisallowMultipleItemComponent : Attribute
    {
        public static void ResolveAttribute(ItemComponent component){
            if(component == null) return;

            var type  = component.GetType();
            var allow = type.GetAttribute<DisallowMultipleItemComponent>() == null;
            if(allow) return;

            var exist = component.item.component.FindAll(x => x!= null && x.GetType() == type);
            if(exist.Count == 1) return;

            for (int i = exist.Count - 1; i >= 1 ; i--)
            {
                component.item.component.Remove(exist[i]);
            }
        }
    }
}

