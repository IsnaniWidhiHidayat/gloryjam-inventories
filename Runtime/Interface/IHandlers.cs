using System.Collections.Generic;

namespace GloryJam.Inventories
{
    public interface IHandlers<T>
    {
        List<T> GetHandlers();
    }
}