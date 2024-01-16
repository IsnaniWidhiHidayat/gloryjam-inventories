using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector; 
#endif


namespace GloryJam.Inventories
{
    [Serializable]
    public abstract class ItemStateSetItemHandler : ItemStateHandler
    {
        #region inner class
        [Serializable]
        public enum Type
        {
            Default,
            ID
        }
        
        [Serializable]
        public abstract class SetSaveData : ItemStateSaveData
        {
            public Type type;

            [ShowIf(nameof(type),Type.ID)]
            public string setID;
        }
        #endregion

        #region fields
        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        #endif
        public Type type;

        #if ODIN_INSPECTOR
        [BoxGroup(grpConfig)]
        [ShowIf(nameof(type),Type.ID)]
        [ValueDropdown(nameof(InspectorGetComponentsID))]
        [ValidateInput(nameof(InspectorValidateComponentID),"ID not found",InfoMessageType.Error)]
        #endif
        public string setID;
        #endregion

        #region inspector
        protected string[] InspectorGetComponentsID()
        {
            if(stack != null) return stack.GetComponentsID<ItemSetComponent>();
            if(item != null) return item.GetComponentsID<ItemSetComponent>();

            return default;
        }
        protected bool InspectorValidateComponentID()
        {
            if(stack != null) return stack.ContainComponent<ItemSetComponent>(setID);
            if(item != null) return item.ContainComponent<ItemSetComponent>(setID);

            return default;
        }
        #endregion

        #region methods
        protected ItemSetComponent GetSetComponent(){
            var component = default(ItemSetComponent);
            switch(type)
            {
                case Type.Default:{
                    if(!stack.TryGetComponentSet(out component)){
                        Debug.LogError($"No {nameof(ItemSetComponent)} was found in item {item?.id}");
                    }
                    break;
                }

                case Type.ID:{
                    if(!stack.TryGetComponent(setID,out component)){
                        Debug.LogError($"No {nameof(ItemSetComponent)} with ID {setID} was found in item {item?.id}");
                    }
                    break;
                }
            }

            return component;
        }
        protected T GetSetHandler<T>() where T : ItemSetHandler
        {
            var component = GetSetComponent();
            return component?.itemSet?.value?.handler as T;
        }
        #endregion
    }
}
