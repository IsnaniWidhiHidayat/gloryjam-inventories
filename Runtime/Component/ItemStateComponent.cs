using GloryJam.Extend;

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
            $"Save State {stack}".Log(DebugFilter.Item);

            for (int i = 0; i < handlers.Count; i++)
            {
                handlers[i]?.SaveState();
            }
        }
        public void LoadState(){
            $"Load State {stack}".Log(DebugFilter.Item);

            for (int i = 0; i < handlers.Count; i++)
            {
                handlers[i]?.LoadState();
            }
        }
        #endregion
    }
}
