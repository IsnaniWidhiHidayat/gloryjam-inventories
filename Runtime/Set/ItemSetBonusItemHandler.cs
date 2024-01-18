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
            #if ODIN_INSPECTOR
            [Required]
            [TableColumnWidth(90,false)]
            #endif
            public string inventory = "Main";

            #if ODIN_INSPECTOR
            [TableList]
            #endif
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

        #if ODIN_INSPECTOR
        [ShowIf(nameof(InspectorShowRuntime))]
        [BoxGroup(grpRuntime),ShowInInspector]
        #endif
        [NonSerialized]
        public bool obtained;

        #if ODIN_INSPECTOR
        [ShowIf(nameof(InspectorShowRuntime))]
        [BoxGroup(grpRuntime),ShowInInspector]
        [ListDrawerSettings(IsReadOnly = true,ListElementLabelName = "title")]
        [HideReferenceObjectPicker,HideDuplicateReferenceBox]
        #endif
        [NonSerialized]
        public ItemStack[] itemBonusTracker;
        #endregion
        
        #region property
        public override string name => "Bonus Item";
        #endregion

        #region private
        [NonSerialized]
        private bool _fullMatch;
        #endregion 

        #region inspector
        #if ODIN_INSPECTOR
        public bool InspectorShowRuntime(){
            return Application.isPlaying;
        }
        #endif
        #endregion

        #region methods
        public void TryInitialize()
        {
            if(itemBonusTracker == null){
                
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

                itemBonusTracker = new ItemStack[totalCount];
            }
        }
        public void AddItemBonus(){
            //TODO: Contain bug when bonus given and save inventory then load inventory can cause duplicate item bonus
            
            if(obtained) return;
            
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
                        if(trackerIndex >= itemBonusTracker.Length) continue;

                        //check empty bonus
                        if(itemBonusTracker[trackerIndex] == null){
                            itemBonusTracker[trackerIndex] = item.CreateInstance();
                        }
                        
                        //check item inventory is null
                        if(itemBonusTracker[trackerIndex].inventory == null){
                            if(!targetInventory.AddItem(itemBonusTracker[trackerIndex])){
                                fullAdd = false;
                            }
                        }

                        trackerIndex++;
                    }
                }
            }

            if(!fullAdd){
                Debug.LogError("Inventory is full, some bonus is not given");
            }else{
                obtained = true;
            }
        }
        public void RemoveItemBonus(){
            if(itemBonusTracker?.Length > 0) {
                for (int i = 0; i < itemBonusTracker.Length; i++)
                {
                    itemBonusTracker[i]?.Dispose();
                }
            }

            obtained = false;
        }
        #endregion

        #region callback
        public override void OnItemMatch(Dictionary<string, ItemStack[]> itemTracker)
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