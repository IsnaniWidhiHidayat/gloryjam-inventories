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

        #if ODIN_INSPECTOR
        [TableColumnWidth(60,false)]
        #endif
        public int count = 1;
    }
}