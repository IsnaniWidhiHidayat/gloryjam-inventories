using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [AddComponentMenu("Glory Jam/Inventory/Item Database")]
    public class ItemDatabase : MonoBehaviour
    {
        #region static
        public static ItemDatabaseData current => _current;
        private static ItemDatabaseData _current;
        #endregion

        #region fields
        #if ODIN_INSPECTOR
        [InlineEditor]
        #endif
        public ItemDatabaseDataAsset databaseData;
        #endregion

        #region methods
        private void Awake() {
            _current = databaseData.value;
        }
        #endregion
    }
}