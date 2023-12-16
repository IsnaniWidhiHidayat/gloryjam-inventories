using System;
using UnityEngine;
using GloryJam.Extend;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    [DisallowMultipleItemComponent]
    [RequiredItemComponent(typeof(ItemUseableComponent))]
    public class ItemConsumeComponent : ItemComponent<ItemConsumeComponent>
    {
        #region static
        private static ItemConsumeEvent Event = new ItemConsumeEvent();
        #endregion

        #region property
        public override string name => "Consume";
        public override int order => 100;
        public override bool showID => false;
        public override bool requiredId => false;
        #endregion

        #region inspector
#if ODIN_INSPECTOR
        [Button("Consume"),PropertyOrder(100),BoxGroup(grpDebug),ButtonGroup(grpDebug + "/Buttons"),ShowIf(nameof(InspectorShowRuntime))]
        private void InspectorConsume(){
            Consume();
        }
        #endif
        #endregion

        #region methods
        public void Consume(){
            $"Consume {stack}".Log(DebugFilter.Item);

            if(stack == null) return;
            
            //Trigger event
            Event.stack = stack;
            ItemConsumeEvent.Trigger(inventory,Event);

            stack.Dispose();
        }
        #endregion

        #region callback
        public override void OnInit(){}
        public override void OnPostInit(){}
        public override void OnDispose(){}
        #endregion
    }
}
