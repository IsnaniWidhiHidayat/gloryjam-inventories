namespace GloryJam.Inventories
{
    [DisallowMultipleItemComponent]
    public class ItemStateComponent : ItemComponent<ItemStateComponent, ItemStateHandler>
    {
        #region property
        public override string name => "State";
        public override int propertyOrder => 101;
        public override bool showID => false;
        public override bool requiredId => false;
        #endregion

        #region methods
        public void SaveState(){
            for (int i = 0; i < handlers.Count; i++)
            {
                handlers[i]?.SaveState();
            }
        }
        public void LoadState(){
            for (int i = 0; i < handlers.Count; i++)
            {
                handlers[i]?.LoadState();
            }
        }
        #endregion
    }
}
