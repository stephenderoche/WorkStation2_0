using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using DevExpress.Xpf.Grid;
using System.Collections.Specialized;
using System.Windows.Interactivity;

namespace Linedata.Framework.Client.NavDashBoardViewerPublisher
{
    public class FocusNullRowBehavior : Behavior<TableView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += ResetRowHandle;
            AssociatedObject.DataControl.FilterChanged += DataControl_FilterChanged;
            AssociatedObject.DataControl.ItemsSourceChanged += DataControl_ItemsSourceChanged;
        }

        void DataControl_FilterChanged(object sender, RoutedEventArgs e)
        {
            GridControl grid = (GridControl)AssociatedObject.DataControl;
            AssociatedObject.FocusedRowHandle = GridControl.InvalidRowHandle;
        }

        void ResetRowHandle(object sender, RoutedEventArgs e)
        {
            AssociatedObject.FocusedRowHandle = GridControl.InvalidRowHandle;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Loaded -= ResetRowHandle;
            AssociatedObject.DataControl.FilterChanged -= ResetRowHandle;
            AssociatedObject.DataControl.ItemsSourceChanged -= DataControl_ItemsSourceChanged;
            base.OnDetaching();
        }

        void FocusNullRowBehavior_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
                AssociatedObject.FocusedRowHandle = GridControl.InvalidRowHandle;
        }

        void DataControl_ItemsSourceChanged(object sender, ItemsSourceChangedEventArgs e)
        {
            AssociatedObject.FocusedRowHandle = GridControl.InvalidRowHandle;
            if (e.OldItemsSource is INotifyCollectionChanged)
                ((INotifyCollectionChanged)e.OldItemsSource).CollectionChanged -= FocusNullRowBehavior_CollectionChanged;
            if (e.NewItemsSource is INotifyCollectionChanged)
                ((INotifyCollectionChanged)e.NewItemsSource).CollectionChanged += FocusNullRowBehavior_CollectionChanged;
        }
    }
}
