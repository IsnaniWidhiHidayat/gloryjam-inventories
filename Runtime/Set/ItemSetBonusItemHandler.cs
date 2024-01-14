using System;
using System.Collections.Generic;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    public class ItemSetBonusItemHandler : ItemSetHandler
    {
        #region inner class
        [Serializable]
        public class ItemBonus{
            [Required,TableColumnWidth(90,false)]
            public string inventory = "Main";

            [TableList]
            public ItemReferenceCount[] bonusItems = new ItemReferenceCount[0];
        }
        #endregion

        #region fields
        [Tooltip("Check this to make bonus can retrieve back if item set is unmatch")]
        public bool retrieveAble;
        
        #if ODIN_INSPECTOR
        [TableList(AlwaysExpanded = true)]
        #endif
        public ItemBonus[] itemBonus = new ItemBonus[0];
        #endregion
        
        #region property
        public override string name => "Bonus Item";
        #endregion

        #region private
        [NonSerialized]
        private ItemStack[] _itemBonusTracker;

        [NonSerialized]
        private bool _fullMatch;
        #endregion 

        #region methods
        private void TryInitialize()
        {
            if(_itemBonusTracker == null){
                
                var totalCount = 0;

                for (int i = 0; i < itemBonus.Length; i++)
                {
                    var bonus = itemBonus[i];

                    //check empty
                    if(bonus == null) continue;
                    
                    if(bonus.bonusItems?.Length > 0) {
                        for (int j = 0; j < bonus.bonusItems.Length; j++)
                        {
                            totalCount += bonus.bonusItems[j].count;
                        }
                    }
                }

                _itemBonusTracker = new ItemStack[totalCount];
            }
        }
        private void AddItemBonus(){
            //give bonus
            var trackerIndex = 0;
            var fullAdd = true;

            for (int i = 0; i < itemBonus.Length; i++)
            {
                //check null
                if(itemBonus[i] == null) continue;

                //check target inventory
                var targetInventory = Inventory.GetInventoryById(itemBonus[i].inventory);
                if(targetInventory == null) continue;

                //check empty items
                if(itemBonus[i].bonusItems == null || itemBonus[i].bonusItems.Length == 0) continue;

                for (int j = 0; j < itemBonus[i].bonusItems.Length; j++)
                {
                    //check item
                    var item = itemBonus[i].bonusItems[j].item?.value;
                    if(item == null) continue;

                    //check count
                    var count = itemBonus[i].bonusItems[j].count;
                    if(count <= 0) continue;

                    //add item to target inventory
                    for (int k = 0; k < count; k++)
                    {
                        //check trackerIndex
                        if(trackerIndex >= _itemBonusTracker.Length) continue;

                        //check empty bonus
                        if(_itemBonusTracker[trackerIndex] == null){
                            _itemBonusTracker[trackerIndex] = item.CreateInstance();
                        }
                        
                        //check item inventory is null
                        if(_itemBonusTracker[trackerIndex].inventory == null){
                            if(!targetInventory.AddItem(_itemBonusTracker[trackerIndex])){
                                fullAdd = false;
                            }
                        }

                        trackerIndex++;
                    }
                }
            }

            if(!fullAdd){
                Debug.LogError("Inventory is full, some bonus is not given");
            }
        }
        private void RemoveItemBonus(){
            if(_itemBonusTracker?.Length > 0) {
                for (int i = 0; i < _itemBonusTracker.Length; i++)
                {
                    _itemBonusTracker[i]?.Dispose();
                }
            }
        }
        #endregion

        #region callback
        public override void OnItemMatch(Dictionary<Item, ItemStack[]> itemTracker)
        {
            TryInitialize();

            //check full match or not
            _fullMatch = true;
            foreach (var keyPair in itemTracker)
            {
                //find item tracker that null
                var emptyIndex = Array.FindIndex(keyPair.Value, x => x == null);
                if(emptyIndex >= 0){
                    _fullMatch = false;
                    break;
                }
            }

            if(_fullMatch){
                AddItemBonus();
            }else if(retrieveAble){
                RemoveItemBonus();
            }
        }
        #endregion
    }
}