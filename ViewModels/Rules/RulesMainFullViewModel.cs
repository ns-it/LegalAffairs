using LegalAffairs.Commands;
using LegalAffairs.Dialogs.DialogService;
using LegalAffairs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace LegalAffairs.ViewModels
{
    public class RulesMainFullViewModel : ObservableObject
    {
        public static LegalAffairsDbContext db;
        public short SearchRuleYear { get; set; }
        public int SearchASN { get; set; }
        public List<short> YearsList { get; set; }
        public static ObservableCollection<Rule> RulesList { get; set; }

        public ObservableCollection<TreeViewItemViewModel> TopicsList { get; set; }
        public ObservableCollection<TreeViewItemViewModel> IssuersList { get; set; }

        private Rule _selectedRule;
        public Rule SelectedRule
        {
            get { return _selectedRule; }
            set { _selectedRule = value; OnPropertyChanged("SelectedRule"); }

        }

        public RelayCommand SearchByTopicAndIssuerCommand { get; set; }
        public RelayCommand SearchBySNCommand { get; set; }

        public RelayCommand OpenDialogCommand { get; set; }

        public RelayCommand UpdateModalCommand { get; set; }
        public RelayCommand AddModalCommand { get; set; }



        ObservableCollection<string> SelectedTopics { get; set; }
        ObservableCollection<string> SelectedIssuers { get; set; }

        public RulesMainFullViewModel()
        {
            db = new LegalAffairsDbContext();
            YearsList = new List<short>();
            RulesList = new ObservableCollection<Rule>();
            //RulesList = new ObservableCollection<Rule>(db.Rules.ToList());

            for (short y = 2000; y < DateTime.Today.Year; y++) YearsList.Add(y);

            SearchRuleYear = YearsList.FirstOrDefault();


            TopicsList = new ObservableCollection<TreeViewItemViewModel> { InitializeTopicsTree(db.Topics.ToList()) };
            IssuersList = new ObservableCollection<TreeViewItemViewModel> { InitializeIssuersTree(db.Issuers.ToList()) };

            SearchByTopicAndIssuerCommand = new RelayCommand(SearchByTopicAnsIssuerAction);
            SearchBySNCommand = new RelayCommand(SearchBySNAction);

            OpenDialogCommand = new RelayCommand(OpenAddDialogAction);

            UpdateModalCommand = new RelayCommand(OpenUpdateDialogAction);
            AddModalCommand = new RelayCommand(OpenAddDialogAction);

        }

        private void OpenAddDialogAction(object o)
        {
            //Dialogs.DialogService.DialogViewModelBase vm = new Dialogs.DialogYesNo.DialogYesNoViewModel("Question");
            //Dialogs.DialogService.DialogResult result = Dialogs.DialogService.DialogService.OpenDialog(vm, o as Window);

            RuleEditViewModel vm = new RuleEditViewModel();
            DialogResult result = DialogService.OpenDialog(vm, o as Window);
       


        }
        private void OpenUpdateDialogAction(object o)
        {

            RuleEditViewModel vm = new RuleEditViewModel(SelectedRule);
            DialogResult result = DialogService.OpenDialog(vm, o as Window);

        }


        ObservableCollection<string> tempList;
        private void SearchByTopicAnsIssuerAction(object o)
        {
            RulesList.Clear();
            tempList = new ObservableCollection<string>();
            SelectedTopics = AddTreeItemsToList(TopicsList);

            tempList = new ObservableCollection<string>();
            SelectedIssuers = AddTreeItemsToList(IssuersList);

            //foreach (string topic in SelectedTopics)
            //{
            //    foreach (Rule rule in db.Rules.Where(r => r.Topic.Equals(topic)).ToList())
            //        RulesList.Add(rule);
            //}

            foreach (Rule rule in db.Rules.Include(r=>r.LatestUpdateUser)
                .Include(r => r.RuleAttachements).Include(r=>r.Issuer).Include(r=>r.Topic)
                .Where(r => SelectedTopics.Contains(r.Topic.TopicName) 
                && SelectedIssuers.Contains(r.Issuer.IssuerName)))
            {
                RulesList.Add(rule);
            }




        }
        private void SearchBySNAction(object o)
        {
            RulesList.Clear();
            foreach( Rule rule in  db.Rules.Where(r=>r.RuleYear==SearchRuleYear&& r.AnnualSerialNumber == SearchASN))
                RulesList.Add(rule);
        }



        private ObservableCollection<string> AddTreeItemsToList(ObservableCollection<TreeViewItemViewModel> topics)
        {
            foreach (var item in topics)
            {
                if (item.IsSelected==true)
                {
                    tempList.Add(item.Header);
                }
                if (item.ChildItems.Count() > 0)
                {
                    AddTreeItemsToList(item.ChildItems);
                }
            }
            return tempList;
        }

        private TreeViewItemViewModel InitializeTopicsTree(List<Topic> topics)
        {
            // LeafNodes

            TreeViewItemViewModel tree = new TreeViewItemViewModel() { Header = "جميع المواضيع" };

            foreach(Topic t in topics)
            {
                // first level
                if (!t.ParentTopicId.HasValue)
                {
                    TreeViewItemViewModel level1node = new TreeViewItemViewModel()
                    {
                        Header = t.TopicName,
                        NodeId = t.TopicId
                    };

                    //second level
                    if (t.Children!=null)
                    {
                        foreach(Topic childTopic in t.Children)
                        {
                            TreeViewItemViewModel level2node = new TreeViewItemViewModel()
                            {
                                NodeId = childTopic.TopicId,
                                Header = childTopic.TopicName,
                                ParentNode = level1node                            
                            };

                            if (childTopic.Children != null)
                            {
                                foreach(Topic thirdLevelTopic in childTopic.Children)
                                {
                                    TreeViewItemViewModel level3node = new TreeViewItemViewModel()
                                    {
                                        NodeId = thirdLevelTopic.TopicId,
                                        Header = thirdLevelTopic.TopicName,
                                        ParentNode = level2node
                                        
                                    };
                                    level2node.ChildItems.Add(level3node);
                                }
                            }

                            level1node.ChildItems.Add(level2node);
                        }
                    }
                    tree.ChildItems.Add(level1node);

                }
                
            }
            return tree;
            //TreeViewItemViewModel node1 = new TreeViewItemViewModel() { Header = "عقود إنشائية" };
            //TreeViewItemViewModel node2 = new TreeViewItemViewModel() { Header = "عقود استثمارية" };
            //TreeViewItemViewModel node3 = new TreeViewItemViewModel() { Header = "طلاب" };
            //TreeViewItemViewModel node4 = new TreeViewItemViewModel() { Header = "عاملين إداريين" };
            //TreeViewItemViewModel node5 = new TreeViewItemViewModel() { Header = "أعضاء هيئة تدريسية" };                                  
            //TreeViewItemViewModel node6 = new TreeViewItemViewModel() { Header = "أعضاء هيئة فنية" };
            //TreeViewItemViewModel node7 = new TreeViewItemViewModel() { Header = "معيدون" };
            //TreeViewItemViewModel node8 = new TreeViewItemViewModel() { Header = "مواضيع أخرى" };


            //TopicsList = new ObservableCollection<TreeViewItemViewModel>()
            //{
            //    new TreeViewItemViewModel()
            //    {
            //        Header = "جميع المواضيع",
            //        ChildItems = new ObservableCollection<TreeViewItemViewModel>()
            //        {
            //            new TreeViewItemViewModel()
            //            {
            //                Header= "عقود" ,
            //                ChildItems = new ObservableCollection<TreeViewItemViewModel>
            //                {
            //                    node1,
            //                    node2,
            //                }
            //            },
            //            node3 ,
            //            new TreeViewItemViewModel()
            //            {
            //                Header= "عاملين" ,
            //                ChildItems = new ObservableCollection<TreeViewItemViewModel>
            //                {
            //                    node4,
            //                    new TreeViewItemViewModel()
            //                    {
            //                        Header ="أعضاء هيئة تعليمية" ,
            //                        ChildItems = new ObservableCollection<TreeViewItemViewModel>
            //                        {
            //                            node5,node6,node7
            //                        }
            //                    }
            //                }
            //            } ,
            //            node8
            //        }
            //    }
            //};
        }
        private TreeViewItemViewModel InitializeIssuersTree(List<Issuer>issuers)
        {

            // LeafNodes

            TreeViewItemViewModel tree = new TreeViewItemViewModel() { Header = "جميع الجهات" };

            foreach (Issuer i in issuers)
            {
                // first level

                    TreeViewItemViewModel level1node = new TreeViewItemViewModel()
                    {
                        Header = i.IssuerName,
                        NodeId = i.IssuerId
                    };
                tree.ChildItems.Add(level1node);


            }
            return tree;




            //IssuersList = new ObservableCollection<TreeViewItemViewModel>()
            //{
            //    new TreeViewItemViewModel()
            //    {
            //        Header = "جميع الجهات",
            //        ChildItems=new ObservableCollection<TreeViewItemViewModel>()
            //        {
            //            new TreeViewItemViewModel(){ Header= "مراسيم" } ,
            //            new TreeViewItemViewModel(){ Header= "رئاسة وزراء" } ,
            //            new TreeViewItemViewModel(){ Header= "مجلس تعليم عالي" }
            //        }
            //    }
            //};
        }


    }
}
