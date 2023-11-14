namespace GloryJam.Inventories
{
    public interface IItemObject
    {
        #region property
        Item item{get;}
        int count{get;}
        #endregion

        #region methods
        void SetItem(Item item);
        void SetCount(int count);
        #endregion
    }
}
