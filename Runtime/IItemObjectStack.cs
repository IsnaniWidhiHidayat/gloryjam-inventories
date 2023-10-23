namespace GloryJam.Inventories
{
    public interface IItemObjectStack
    {
        #region property
        ItemStack item{get;}
        #endregion

        #region methods
        void SetItem(ItemStack item);
        #endregion
    }
}
