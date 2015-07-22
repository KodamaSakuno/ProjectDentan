﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Moen
{
    public abstract class DependencyModelBase : DependencyObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected virtual void OnPropertyChanged([CallerMemberName] string rpPropertyName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(rpPropertyName));
        }
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> rpExpression)
        {
            if (rpExpression == null)
                throw new ArgumentNullException("rpExpression");

            var rMemberExpression = rpExpression.Body as MemberExpression;
            if (rMemberExpression == null)
                throw new NotSupportedException();

            OnPropertyChanged(rMemberExpression.Member.Name);
        }
    }
}
