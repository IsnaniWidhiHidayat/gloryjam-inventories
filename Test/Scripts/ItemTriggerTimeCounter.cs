using System;
using UnityEngine;

namespace GloryJam.Inventories.Test
{
    [Serializable]
    public class ItemTriggerTimeCounter : ItemTriggerHandler
    {
        public int every = 10;

        private TimeCounter timeCounter;

        public override string name => "Counter";

        public override ItemComponentHandler CreateInstance()
        {
            var clone = base.CreateInstance() as ItemTriggerTimeCounter  ;
                clone.every = every;
            return clone;
        }

        public override void OnInit()
        {
            Debug.Log("OnInit trigger");
            timeCounter = inventory?.GetComponent<TimeCounter>();
            if(timeCounter){
                timeCounter.onCounterChange.AddListener(OnCounterChange);
            }
        }

        public override void OnDispose()
        {
            Debug.Log("OnDispose trigger");
            if(timeCounter){
                timeCounter.onCounterChange.RemoveListener(OnCounterChange);
            }
        }

        private void OnCounterChange(int value)
        {
            if(value % every != 0) return;
            InvokeOnTrigger();
            Debug.Log("Ontrigger invoked");
        }

        public override void OnPostInit()
        {
           
        }
    }
}