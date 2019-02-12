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
        public ObserverCopy instance;
        public ObserverCopy last;

        public void Watch(ObserverCopy instance)
        {
            this.instance = instance;
            this.last = instance.Clone();

            var fields = instance.GetType().GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                Debug.Log(fields[i]);
            }
        }

        public bool CheckChange()
        {
            var fields = instance.GetType().GetFields();
            var lastFields = last.GetType().GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i] != lastFields[i])
                    return false;
            }

            return true;
        }

        public void Change()
        {
            this.last = instance.Clone();
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
}