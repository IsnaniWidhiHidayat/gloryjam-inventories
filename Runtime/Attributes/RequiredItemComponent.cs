using System;
using System.Collections.Generic;
using Sirenix.Utilities;

namespace GloryJam.Inventories
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class RequiredItemComponent : Attribute
    {
        public Type type;

        public RequiredItemComponent(Type itemComponent){
            type = itemComponent;
        }

        public static void CheckAttribute(ItemComponent component){
            if(component == null) return;
            
            var baseType = typeof(ItemComponent);
            var type = component.GetType();
            var att  = type.GetAttribute<RequiredItemComponent>();
            if(att == null) return;
            if(!baseType.IsAssignableFrom(att.type)) return;
            if(component.item.component.Exists(x => x != null && x.GetType() == att.type)) return;

            //create instance
            var newComponent = Activator.CreateInstance(att.type) as ItemComponent;
            if(newComponent == null) return;

            component.item.component.Add(newComponent);
        }
        public static void CheckAttribute(ItemComponentHandler handler){
            var att = handler?.GetType()?.GetAttribute<RequiredItemComponent>();
            if(att == null) return;
            if(handler.item.component.Exists(x => x != null && x.GetType() == att.type)) return;
            var newComponent = Activator.CreateInstance(att.type) as ItemComponent;
            if(newComponent == null) return;
            handler.item.component.Add(newComponent);
        }
    }
}

