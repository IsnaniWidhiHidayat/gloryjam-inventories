using UnityEngine;

namespace GloryJam.Inventories
{
    [DisallowMultipleItemComponent]
    public class ItemStateComponent : ItemComponent<ItemStateComponent, ItemStateHandler>
    {
        #region property
        public override string name => "State";
        public override int order => 101;
        public override bool showID => false;
        public override bool requiredId => false;
        #endregion

        #region methods
        public void SaveState(){
            Debug.Log($"[Inventory]{inventory?.name} Inventory {inventory?.id} Save State {stack?.item?.id}, stack:{stack}");

            for (int i = 0; i < handlers.Count; i++)
            {
                handlers[i]?.SaveState();
            }
        }
        public void LoadState(){
            Debug.Log($"[Inventory]{inventory?.name} Inventory {inventory?.id} Load State {stack?.item?.id}, stack:{stack}");

            for (int i = 0; i < handlers.Count; i++)
            {
                handlers[i]?.LoadState();
            }
        }
        #endregion
    }
}
