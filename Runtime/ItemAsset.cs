using System;
using System.Collections.Generic;
using GloryJam.DataAsset;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    #if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
    #endif
    public class Item : IInstance<ItemStack> , ISort
    {
        #region const
        protected const string grpMain = "Main";
        protected const string grpDismantle = "Dismantle";
        #endregion

        #region fields
        [SerializeField]
        #if ODIN_INSPECTOR
        [BoxGroup(grpMain),HorizontalGroup(grpMain + "/h1"),VerticalGroup(grpMain + "/h1/v1"),Required]
        #endif
        protected string _id,_name;

        [SerializeField]
        #if ODIN_INSPECTOR
        [VerticalGroup(grpMain + "/h1/v1"),Multiline]
        #endif
        protected string _description;

        [SerializeField]
        #if ODIN_INSPECTOR
        [PreviewField(90,ObjectFieldAlignment.Right),HorizontalGroup(grpMain + "/h1",Width = 90),HideLabel]
        #endif
        protected Sprite _icon;

        [SerializeField]
        #if ODIN_INSPECTOR
        [BoxGroup(grpMain),MinValue(1)]
        #endif
        protected int _maxStack = 1;

        [SerializeField]
        #if ODIN_INSPECTOR
        [ListDrawerSettings(Expanded = true)]
        [ValidateInput(nameof(InspectorValidateTags),"Please remove empty tag")]
        #endif
        protected string[] _tags = new string[0];

        [SerializeField]
        #if ODIN_INSPECTOR
        [ValidateInput(nameof(InspectorValidateComponent))]
        [OnValueChanged(nameof(InspectorOnComponentValueChange))]
        [ListDrawerSettings(Expanded = true,ShowItemCount = false,DraggableItems = false,ListElementLabelName = "title",CustomRemoveElementFunction = nameof(InspectorComponentRemove))]
        [HideInPlayMode]
        #endif
        public List<ItemComponent> component;
        #endregion

        #region constructor
        public Item(){
            component  = new List<ItemComponent>();
        }
        #endregion

        #region inspector
        #if ODIN_INSPECTOR
        private void InspectorComponentRemove(ItemComponent component){
            var type = component.GetType();
            for (int i = 0; i < this.component.Count; i++)
            {
                if(this.component[i] == null) continue;
                if(RequiredItemComponent.IsTypeRequiredBy(type,this.component[i])){
                    #if UNITY_EDITOR
                        UnityEditor.EditorUtility.DisplayDialog("Message",$"Required by {this.component[i].name}","Ok");
                        return;
                    #endif
                }
            }
            this.component.Remove(component);
        }
        private bool InspectorValidateTags(string[] tags)
        {
             return tags == null ? true : !Array.Exists(tags, x => string.IsNullOrEmpty(x));
        }
        private bool InspectorValidateComponent(List<ItemComponent> component)
        {
            if(!Application.isPlaying){
                for (int i = 0; i < component.Count; i++)
                {
                    if(component[i] == null || component[i].item != null) continue;
                    component[i].SetItem(this);
                }
            }
            return true;
        }
        private void InspectorOnComponentValueChange()
        {
            if(component == null || component.Count == 0) return;

            var lastIndex = component.Count - 1;
            component[lastIndex].SetItem(this);
            DisallowMultipleItemComponent.ResolveAttribute(component[lastIndex]);

            lastIndex = component.Count - 1;
            component[lastIndex].SetItem(this);
            RequiredItemComponent.ResolveAttribute(component[lastIndex]);

            Sort();

            InspectorValidateComponent(component);
        }
        #endif
        #endregion

        #region property
        public string id => _id;
        public string name => _name;
        public string description => _description;

        public Sprite icon => _icon;
        public int maxStack {
            get{
                if(_maxStack < 1){
                    _maxStack = 1;
                }

                return _maxStack;
            }
        }
        public string[] tags => _tags;

        public int hash{
            get => GetHashCode();
        }
        #endregion

        #region methods
        public bool ContainTag(string tag){
            if(string.IsNullOrEmpty(tag) || _tags == null || _tags.Length == 0) return false;
            return Array.IndexOf(_tags,tag) >= 0;
        }
        public void Sort(){
            //sort component by order
            component?.Sort((x,y) =>{
                if(x == null || y == null) return -1;
                return x.order.CompareTo(y.order);
            });

            if(component?.Count > 0){
                for (int i = 0; i < component.Count; i++)
                {
                    var ISort = component[i] as ISort;
                    if(ISort == null) continue;
                    ISort.Sort();
                }
            }
        }
        public override string ToString()
        {
            return id;
        }
        #endregion

        #region IInstance
        public ItemStack CreateInstance(){
            var stack = new ItemStack(component.CreateInstance((x) => x.Enabled));
            stack.SetItem(this);
            stack.SetSlot(null);

            return stack;
        }
        #endregion
    }

    [CreateAssetMenu(menuName = "Glory Jam/Inventory/Item Asset")]
    public class ItemAsset : DataAsset<Item>{
    }
}