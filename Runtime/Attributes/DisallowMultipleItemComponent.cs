using System;

namespace GloryJam.Inventories
{
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class DisallowMultipleItemComponent : Attribute
    {
        
    }
}

