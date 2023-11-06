using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    #if ODIN_INSPECTOR
    [Serializable,HideReferenceObjectPicker,HideDuplicateReferenceBox]
    [Toggle("Enabled",CollapseOthersOnExpand = false)]
    #endif
    [DisallowMultipleItemComponent]
    public class ItemConsumeComponent : ItemComponent<ItemConsumeComponent>
    {
        #region static
        private static ItemConsumeEvent Event = new ItemConsumeEvent();
        #endregion

        #region property
        public override string ComponentName => "Consume";
        public override int ComponentPropertyOrder => 100;
        #endregion

        #region inspector
        #if ODIN_INSPECTOR
        [Button("Consume"),BoxGroup(grpDebug),ButtonGroup(grpDebug + "/Buttons"),ShowIf(nameof(InspectorShowRuntime))]
        private void InspectorConsume(){
            Consume();
        }
        #endif
        #endregion

        #region methods
        public void Consume(){
            stack.Dispose();

            //Trigger event
            Event.stack = stack;
            ItemConsumeEvent.Trigger(Event);
        }
        public override void LoadState(){}
        public override void SaveState(){}
        #endregion

        #region callback
        public override void OnInit(){}
        public override void OnPostInit(){}
        public override void OnDispose(){}
        #endregion
    }
}
