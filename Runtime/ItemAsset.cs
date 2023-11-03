using System;
using System.Collections.Generic;
using GloryJam.DataAsset;
using UnityEngine;
using Sirenix.Utilities;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace GloryJam.Inventories
{
    [Serializable]
    #if ODIN_INSPECTOR
    [HideReferenceObjectPicker]
    #endif
    public class Item : IInstance<ItemStack>
    {
        #region const
        protected const string grpMain = "Main";
        protected const string grpDismantle = "Dismantle";
        #endregion

        #region fields
        [SerializeField]
        #if ODIN_INSPECTOR
        [BoxGroup(grpMain),HorizontalGroup(grpMain + "/h1"),VerticalGroup(grpMain + "/h1/v1")]
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
        [BoxGroup(grpMain)]
        #endif
        protected string[] _tags = new string[0];

        [SerializeField]
        #if ODIN_INSPECTOR
        //[ValidateInput(nameof(ValidateComponent))]
        [OnValueChanged(nameof(OnComponentValueChange)),ListDrawerSettings(Expanded = true,ShowItemCount = false,DraggableItems = false,ListElementLabelName = "InspectorGetComponentName")]
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
        // private bool ValidateComponent(List<ItemComponent> group)
        // {
        //     if(!Application.isPlaying){
        //         for (int i = 0; i < component.Count; i++)
        //         {
        //             if(component[i] == null) continue;
        //             component[i].SetItem(this);
        //         }
        //     }
        //     return true;
        // }
        private void OnComponentValueChange()
        {
            if(component == null || component.Count == 0) return;

            if(component.Count > 1) {
                var last = component[component.Count - 1];
                if(last == null) return;

                var lastType = last.GetType();
                var allowMultiple = lastType.GetAttribute<DisallowMultipleItemComponent>() == null;
                
                if(!allowMultiple) {
                    for (int i = 0; i < component.Count - 1; i++)
                    {
                        if(component[i] == null) continue;
                        if(component[i].GetType() == lastType){
                            component.Remove(last);
                            break;
                        }
                    }
                }
            }

            //sort by component name
            component.Sort((x,y) =>{
                if(x == null || y == null) return -1;
                return x.ComponentPropertyOrder.CompareTo(y.ComponentPropertyOrder);
            });

            //ValidateComponent(component);
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
        #endregion

        #region methods
        public bool ContainTag(string tag){
            if(string.IsNullOrEmpty(tag) || _tags == null || _tags.Length == 0) return false;
            return Array.IndexOf(_tags,tag) >= 0;
        }
        #endregion

        #region IInstance
        public ItemStack CreateInstance(){
            var value = new ItemStack(){
                component = component.CreateInstance(),
            };

            value.SetItem(this);

            return value;
        }
        #endregion
    }

    [CreateAssetMenu(menuName = "Glory Jam/Inventory/" + nameof(ItemAsset))]
    public class ItemAsset : DataAsset<Item>{
    }
}