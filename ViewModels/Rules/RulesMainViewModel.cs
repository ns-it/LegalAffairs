using LegalAffairs.Commands;
using LegalAffairs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace LegalAffairs.ViewModels
{
    public class RulesMainViewModel
    {
        private readonly LegalAffairsDbContext db;
        public short SearchRuleYear { get; set; }
        public int SearchASN { get; set; }
        public List<short> YearsList { get; set; }
        public ObservableCollection<Rule> RulesList { get; set; }

        public ObservableCollection<TreeViewItemViewModel> TopicsList { get; set; }
        public ObservableCollection<TreeViewItemViewModel> IssuersList { get; set; }


        public RelayCommand SearchCommand { get; set; }

        ObservableCollection<string> SelectedTopics { get; set; }
        ObservableCollection<string> SelectedIssuers { get; set; }

        public RulesMainViewModel()
        {
            db = new LegalAffairsDbContext();
            YearsList = new List<short>();
            RulesList = new ObservableCollection<Rule>();
            //RulesList = new ObservableCollection<Rule>(db.Rules.ToList());

            for (short y = 2000; y < DateTime.Today.Year; y++) YearsList.Add(y);

            SearchRuleYear = YearsList.FirstOrDefault();

            InitializeTopicsTree();
            InitializeIssuersTree();


            SearchCommand = new RelayCommand(SearchAction, null);

        }

        ObservableCollection<string> tempList;
        private void SearchAction(object o)
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

            foreach (Rule rule in db.Rules.Where(r => SelectedTopics.Contains(r.Topic.TopicName) && SelectedIssuers.Contains(r.Issuer.IssuerName))) 
            {
                RulesList.Add(rule);
            }




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

        private void InitializeTopicsTree()
        {
            TopicsList = new ObservableCollection<TreeViewItemViewModel>()
            {
                new TreeViewItemViewModel()
                {
                    Header = "جميع المواضيع",
                    ChildItems = new ObservableCollection<TreeViewItemViewModel>()
                    {
                        new TreeViewItemViewModel()
                        {
                            Header= "عقود" ,
                            ChildItems = new ObservableCollection<TreeViewItemViewModel>
                            {
                                new TreeViewItemViewModel(){Header ="إنشائية"},
                                new TreeViewItemViewModel(){Header ="استثمارية"}
                            }
                        },
                        new TreeViewItemViewModel(){ Header= "طلاب" } ,
                        new TreeViewItemViewModel()
                        {
                            Header= "عاملين" ,
                            ChildItems = new ObservableCollection<TreeViewItemViewModel>
                            {
                                new TreeViewItemViewModel(){Header ="إداريين"},
                                new TreeViewItemViewModel()
                                {
                                    Header ="أعضاء هيئة تعليمية" ,
                                    ChildItems = new ObservableCollection<TreeViewItemViewModel>
                                    {
                                        new TreeViewItemViewModel(){Header ="أعضاء هيئة تدريسية"},
                                        new TreeViewItemViewModel(){Header ="أعضاء هيئة فنية"},
                                        new TreeViewItemViewModel(){Header ="معيدون"}
                                    }
                                }
                            }
                        } ,
                        new TreeViewItemViewModel(){ Header= "مواضيع أخرى" }
                    }
                }
            };
        }
        private void InitializeIssuersTree()
        {
            IssuersList = new ObservableCollection<TreeViewItemViewModel>()
            {
                new TreeViewItemViewModel()
                {
                    Header = "جميع الجهات",
                    ChildItems=new ObservableCollection<TreeViewItemViewModel>()
                    {
                        new TreeViewItemViewModel(){ Header= "مراسيم" } ,
                        new TreeViewItemViewModel(){ Header= "رئاسة وزراء" } ,
                        new TreeViewItemViewModel(){ Header= "مجلس تعليم عالي" }
                    }
                }
            };
        }


    }
}
