using LegalAffairs.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LegalAffairs.ViewModels
{
    public class CaseViewModel
    {
        private Case model;
        public CaseViewModel(Case value) { Model = value; }
        public CaseViewModel() : this(new Case()) { }
        public Case Model { get => model; set => model = value; }

        //public DateTime GetLatestUpdateDate() 
        //{
        //    System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
        //    dtDateTime = dtDateTime.AddSeconds(model.LatestUpdateTimestamp)
        //        .ToLocalTime();
        //    return dtDateTime;

        //}


    }
}
