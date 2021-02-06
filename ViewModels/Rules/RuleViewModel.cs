using LegalAffairs.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegalAffairs.ViewModels
{
    public class RuleViewModel : ObservableObject
    {
        private Rule model;
        public RuleViewModel(Rule value) { Model = value; }
        public RuleViewModel() : this(new Rule()) { }
        public Rule Model { get => model; set => model = value; }

        public short RuleYear
        {
            get { return Model.RuleYear; }
            set { Model.RuleYear = value; OnPropertyChanged("RuleYear"); }
        }


    }
}
