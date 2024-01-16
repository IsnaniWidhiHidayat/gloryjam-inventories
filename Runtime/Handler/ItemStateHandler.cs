using System;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemStateHandler : ItemComponentHandler
    {
        #if ODIN_INSPECTOR
        [ShowInInspector,BoxGroup("Save Data"),HideLabel]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        [ReadOnly]
        #endif
        public abstract ItemStateSaveData saveData{get;set;}

        public override bool requiredId => true;
        public override bool showID => true;

        #if ODIN_INSPECTOR
        [Button,BoxGroup(grpDebug),HorizontalGroup(grpDebug + "/h1")]
        #endif
        public abstract void SaveState();

        #if ODIN_INSPECTOR
        [Button,HorizontalGroup(grpDebug + "/h1")]
        #endif
        public abstract void LoadState();
    }
}
