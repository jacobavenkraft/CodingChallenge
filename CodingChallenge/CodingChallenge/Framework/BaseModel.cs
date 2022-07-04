using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CodingChallenge.Framework
{
    public class BaseModel : INotifyPropertyChanged
    {
        //property table
        protected readonly Dictionary<string, object> propertyTable = new Dictionary<string, object>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged([CallerMemberName] string property = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        protected T Get<T>([CallerMemberName] string property = "")
        {
            if (!TryGetValue(property, out object value))
            {
                value = default(T);
            }

            return (T)value;
        }

        public virtual bool TryGetValue(string property, out object value)
        {
            lock (propertyTable)
            {
                return propertyTable.TryGetValue(property, out value);
            }
        }

        protected virtual void Set(object value, [CallerMemberName] string property = "") => TrySetValue(value, property);

        protected virtual bool TrySetValue(object value, [CallerMemberName] string property = "")
        {
            if (!TryGetValue(property, out object previousValue) ||
                value == null || !value.Equals(previousValue))
            {
                lock (propertyTable)
                {
                    propertyTable[property] = value;
                }

                RaisePropertyChanged(property);

                return true;
            }

            return false;
        }        

        public object GetValue(string property)
        {
            return Get<object>(property);
        }

        public T GetValue<T>(string property)
        {
            return Get<T>(property);
        }

        public void SetValue(string property, object value)
        {
            Set(value, property);
        }
    }
}
