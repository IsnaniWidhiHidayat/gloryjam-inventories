using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemReferenceCount 
    {
        public string title {
            get{
                if(item != null && item.value != null){
                    return $"{item.value.id} ({count})";
                }

                return default;
            }
        }
        
        #if ODIN_INSPECTOR
        [InlineEditor]
        #endif
        public ItemAsset item;
        
        [TableColumnWidth(80,false)]
        public int count;
    }
}