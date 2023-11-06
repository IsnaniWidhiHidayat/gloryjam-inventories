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
        [OnValueChanged(nameof(InspectorOnComponentValueChange)),ListDrawerSettings(Expanded = true,ShowItemCount = false,DraggableItems = false,ListElementLabelName = "InspectorGetComponentName")]
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
                            Debug.LogError($"Cannot have multiply component {lastType} cause mark {nameof(DisallowMultipleItemComponent)}");
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
                component = component.CreateInstance((x) => x.Enabled),
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