using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Hellevator.Simulator.ViewModels
{
    public abstract class ViewModelBase
    {
        /// <summary>
        /// Warns the developer if this object does not have
        /// a public property with the specified name. This 
        /// method does not exist in a Release build.
        /// </summary>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] != null)
                return;
            
            var msg = "Invalid property name: " + propertyName;
            throw new ArgumentException(msg);
        }

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);
            
            var handler = PropertyChanged;
            if(handler == null)
                return;

            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }
    }
}