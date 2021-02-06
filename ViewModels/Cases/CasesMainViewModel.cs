using LegalAffairs.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace LegalAffairs.ViewModels
{
    public class CasesMainViewModel 
    {

        private readonly LegalAffairsDbContext db;

        public short SearchCaseYear { get; set; }
        public int SearchASN { get; set; }
        public CaseOwner SearchCaseOwner { get; set; }

        public List<short> YearsList { get; set; }
        public ObservableCollection<CaseOwner> CaseOwnersList { get; set; }
        public List<Case> CasesList { get; set; }   

        public CasesMainViewModel()
        {

            db = new LegalAffairsDbContext();
            YearsList = new List<short>();
            CaseOwnersList = new ObservableCollection<CaseOwner>(db.CaseOwners.ToList());
            CasesList = db.Cases.Include(c=>c.CaseOwner).ToList();

            for (short y=2000; y < DateTime.Today.Year; y++) YearsList.Add(y);

            SearchCaseYear = YearsList.FirstOrDefault();
            SearchCaseOwner = CaseOwnersList.FirstOrDefault();

        }

    }
}
