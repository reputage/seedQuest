using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeedQuest.Utils
{
    public class Observable<T>
    {
        public T Value
        {
            get { return getter(); }
            set { setter(value); }
        }

        private System.Func<T> getter;
        private System.Action<T> setter;
        private T lastValue;

        public Observable(System.Func<T> getter, System.Action<T> setter)
        {
            this.getter = getter;
            this.setter = setter;
        }

        public void Change()
        {
            lastValue = Value;
        }

        public bool CheckChange()
        {
            return (!EqualityComparer<T>.Default.Equals(Value, lastValue));
        }

        public void onChange(System.Action action)
        {
            if (CheckChange())
            {
                action();
                Change();
            }
        }
    }

    public abstract class ObserverCopy
    {
        public ObserverCopy Clone()
        {
            return (ObserverCopy)this.MemberwiseClone();
        }
    }

    public class Observer
    {
        public ObserverCopy instance = null;
        public ObserverCopy last = null;

        public void Watch(ObserverCopy instance)
        {
            this.instance = instance;
            this.last = instance.Clone();
        }

        public bool CheckChange()
        {
            var fields = instance.GetType().GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                string test = fields[i].ToString() + fields[i].GetValue(instance).ToString();
            }

            //var fields = instance.GetType().GetFields();
            var lastFields = last.GetType().GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].GetValue(instance) != lastFields[i].GetValue(last)) {
                    Debug.Log("Test: " + fields[i].Name + " :" + fields[i].GetValue(instance).ToString() + " ... " + lastFields[i].GetValue(last));
                    return true;
                }
            }

            return false;
        }

        public void Change()
        {
            this.last = instance.Clone();
        }

        public void onChange(System.Action action)
        {
            if (instance == null) return;

            if (CheckChange())
            {
                action();
                Change();
            }
        }
    }
}