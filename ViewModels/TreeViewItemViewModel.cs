using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace LegalAffairs.ViewModels
{
    public class TreeViewItemViewModel : ObservableObject
    {

        public short NodeId { get; set; }

        public TreeViewItemViewModel ParentNode { get; set; }

        private bool? _isSelected;
        public bool? IsSelected
        {
            get { return _isSelected; }
            set
            {
                SetIsSelected(value, true, true);
                //_isSelected = value;
                //foreach (TreeViewItemViewModel child in ChildItems)
                //    child.IsSelected = value;
                //if (ParentNode != null)
                //    SetParentNodeSelectedValue();


                //OnPropertyChanged("IsSelected");
            }
        }
        public string Header { get; set; }
        public ObservableCollection<TreeViewItemViewModel> ChildItems { get; set; }
        private void CollectionChangedMethod(object sender, NotifyCollectionChangedEventArgs e)
        {
            //different kind of changes that may have occurred in collection
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                ParentNode = this;
            }
            //if (e.Action == NotifyCollectionChangedAction.Replace)
            //{
            //    //your code
            //}
            //if (e.Action == NotifyCollectionChangedAction.Remove)
            //{
            //    //your code
            //}
            //if (e.Action == NotifyCollectionChangedAction.Move)
            //{
            //    //your code
            //}
        }

        //private void SetParentNodeSelectedValue()
        //{
        //    bool temp = false;
        //    foreach(TreeViewItemViewModel sibiling in ParentNode.ChildItems)
        //    {
        //        temp &= sibiling.IsSelected;
        //    }
        //    ParentNode.IsSelected = temp;
        //}
        void SetIsSelected(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == _isSelected)
                return;

            _isSelected = value;

            if (updateChildren && _isSelected.HasValue)
                foreach (var c in ChildItems)
                {
                    c.SetIsSelected(_isSelected, true, false);
                }


            if (updateParent && ParentNode != null)
                ParentNode.VerifyCheckState();

            this.OnPropertyChanged("IsSelected");
        }
        void VerifyCheckState()
        {
            bool? state = null;
            for (int i = 0; i < this.ChildItems.Count; ++i)
            {
                bool? current = this.ChildItems[i].IsSelected;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }
            this.SetIsSelected(state, false, true);
        }

        public TreeViewItemViewModel()
        {
            ChildItems = new ObservableCollection<TreeViewItemViewModel>();
            IsSelected = true;
            ChildItems.CollectionChanged += 
                new NotifyCollectionChangedEventHandler(CollectionChangedMethod);
        }



    }
}
