namespace GloryJam.Inventories
{
    public interface IItemObjectStack
    {
        #region property
        ItemStack stack{get;}
        #endregion

        #region methods
        void SetStack(ItemStack item);
        #endregion
    }
}
