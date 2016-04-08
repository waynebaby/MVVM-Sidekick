#if NETFX_CORE
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;


namespace MVVMSidekick.Collections
{
    
    public class DependencyCollectionView : DependencyObservableVector<Object, DependencyCollectionView>, ICollectionView
    {


        public DependencyCollectionView()
        {
            CollectionGroups = new DependencyObservableVector();
            base.VectorChanged += DependencyCollectionView_VectorChanged;
        }

        private void DependencyCollectionView_VectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs @event)
        {
            switch (@event.CollectionChange)
            {
                case CollectionChange.ItemChanged:
                    break;
                default:
                    RefreshPositionValues();
                    break;
            }
        }

        public IObservableVector<object> CollectionGroups
        {
            get { return (IObservableVector<object>)GetValue(CollectionGroupsProperty); }
            private set { SetValue(CollectionGroupsProperty, value); }
        }

        public static readonly DependencyProperty CollectionGroupsProperty =
            DependencyProperty.Register(nameof(CollectionGroups), typeof(IObservableVector<object>), typeof(DependencyCollectionView), new PropertyMetadata(null));

        public object CurrentItem
        {
            get { return (object)GetValue(CurrentItemProperty); }
            private set { SetValue(CurrentItemProperty, value); }
        }

        public static readonly DependencyProperty CurrentItemProperty =
            DependencyProperty.Register(
                nameof(CurrentItem),
                typeof(object),
                typeof(DependencyCollectionView),
                new PropertyMetadata(
                    null,
                    (o, e) => (o as DependencyCollectionView)?.CurrentChanged(o, e.NewValue)));


        public int CurrentPosition
        {
            get { return (int)GetValue(CurrentPositionProperty); }
            private set { SetValue(CurrentPositionProperty, value); }
        }

        public static readonly DependencyProperty CurrentPositionProperty =
            DependencyProperty.Register(
                nameof(CurrentPosition),
                typeof(int),
                typeof(DependencyCollectionView),
                new PropertyMetadata(0, (o, e) =>
                    (o as DependencyCollectionView)?.RefreshPositionValues()));

        private void RefreshPositionValues()
        {
            IsCurrentAfterLast = CurrentPosition >= Count;
            IsCurrentBeforeFirst = CurrentPosition < 0;
            CurrentItem = (IsCurrentBeforeFirst || IsCurrentAfterLast) ? null : this[CurrentPosition];

        }

        public bool HasMoreItems
        {
            get { return (bool)GetValue(HasMoreItemsProperty); }
            private set { SetValue(HasMoreItemsProperty, value); }
        }

        public static readonly DependencyProperty HasMoreItemsProperty =
            DependencyProperty.Register(nameof(HasMoreItems), typeof(bool), typeof(DependencyCollectionView), new PropertyMetadata(false));


        public bool IsCurrentAfterLast
        {
            get { return (bool)GetValue(IsCurrentAfterLastProperty); }
            private set { SetValue(IsCurrentAfterLastProperty, value); }
        }

        public static readonly DependencyProperty IsCurrentAfterLastProperty =
         DependencyProperty.Register(nameof(IsCurrentAfterLast), typeof(bool), typeof(DependencyCollectionView), new PropertyMetadata(0));


        public bool IsCurrentBeforeFirst
        {
            get { return (bool)GetValue(IsCurrentBeforeFirstProperty); }
            private set { SetValue(IsCurrentBeforeFirstProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsCurrentBeforeFirst.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCurrentBeforeFirstProperty =
            DependencyProperty.Register(nameof(IsCurrentBeforeFirst), typeof(bool), typeof(DependencyCollectionView), new PropertyMetadata(0));


        public event EventHandler<object> CurrentChanged;

        public event CurrentChangingEventHandler CurrentChanging;

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            throw new NotImplementedException();
        }




        
        public bool MoveCurrentTo(object item)
        {
            return InternalExecuteCancellable(
                () =>
                {
                    var findex = IndexOf(item);
                    if (findex < 0)
                    {
                        return false;
                    }
                    return InternalMoveCurrentToNewPosition(findex);
                });
        }



        public bool MoveCurrentToFirst()
        {
            return InternalExecuteCancellable(
             () =>
             {
                 var newpo = 0;
                 return InternalMoveCurrentToNewPosition(newpo);
             });
        }

        public bool MoveCurrentToLast()
        {
            var newpo = Count - 1;
            return InternalExecuteCancellable(
               () =>
               {
                   return InternalMoveCurrentToNewPosition(newpo);
               });
        }

        public bool MoveCurrentToNext()
        {
            var newpo = CurrentPosition + 1;
            return InternalExecuteCancellable(
               () =>
               {
                   return InternalMoveCurrentToNewPosition(newpo);
               });
        }

        public bool MoveCurrentToPosition(int index)
        {
            var newpo = index;
            return InternalExecuteCancellable(
               () =>
               {
                   return InternalMoveCurrentToNewPosition(newpo);
               });
        }

        public bool MoveCurrentToPrevious()
        {
            var newpo = CurrentPosition-1;
            return InternalExecuteCancellable(
               () =>
               {
                   return InternalMoveCurrentToNewPosition(newpo);
               });
        }
        private bool InternalMoveCurrentToNewPosition(int newpo)
        {
            if (IsCurrentAfterLast || IsCurrentBeforeFirst)
            {
                return false;
            }

            if (newpo == CurrentPosition || newpo < 0 || newpo >= Count)
            {
                return false;
            }

            var position = CurrentPosition;
            _coreCollection.Move(position, newpo);
            CurrentPosition = newpo;

            return true;
        }

        private bool InternalExecuteCancellable(Func<bool> act)
        {
            var rval = false;
            bool isCanceled = false;

            if (CurrentChanging != null)
            {
                var cargs = new CurrentChangingEventArgs(true);
                CurrentChanging(this, cargs);
                isCanceled = cargs.Cancel;
            }
            if (!isCanceled)
            {
                rval = act();
            }

            return rval;
        }

    }
}

#endif

