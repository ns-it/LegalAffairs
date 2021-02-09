using LegalAffairs.Models;
using System;
using System.Collections.Generic;
using LegalAffairs.Dialogs.DialogService;
using LegalAffairs.Commands;
using System.Windows;
using System.ComponentModel;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing.Printing;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Diagnostics.Contracts;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
//using Microsoft.Web.Helpers;

namespace LegalAffairs.ViewModels
{
    //, INotifyDataErrorInfo
    //, IDataErrorInfo
    public class RuleEditViewModel : DialogViewModelBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private string _validationMessages;
        public string ValidationMessages
        {
            get { return _validationMessages; }
            set { _validationMessages = value; OnPropertyChanged("ValidationMessages"); }
        }

        private string _attachementsLabel;
        public string AttachementsLabel
        {
            get { return _attachementsLabel; }
            set { _attachementsLabel = value; OnPropertyChanged("AttachementsLabel"); }

        }

        //private ObservableCollection<string> _vml;
        private ObservableCollection<string> ValidationMessagesList;




        private string FormatMessages(ObservableCollection<string> vs)
        {

            string temp = string.Empty;

            //string temp = "*\n";
            foreach (string s in vs)
            {
                temp += " * " + s + "\n";
            }

            //temp += "*";

            return temp;
        }


        private bool _isEditModeOn;
        public bool IsEditModeOn
        {
            get { return _isEditModeOn; }
            set { _isEditModeOn = value; OnPropertyChanged("IsEditModeOn"); }
        }

        private bool _isAttachEditModeOn;
        public bool IsAttachEditModeOn
        {
            get { return _isAttachEditModeOn; }
            set { _isAttachEditModeOn = value; OnPropertyChanged("IsAttachEditModeOn"); }
        }

        private bool _isSaved;
        public bool IsSaved
        {
            get { return _isSaved; }
            set { _isSaved = value; OnPropertyChanged("IsSaved"); }
        }

        public bool IsEditModeOff { get { return !IsEditModeOn; } }

        private Rule _currentRule;
        public Rule CurrentRule
        {
            get { return _currentRule; }
            set { _currentRule = value; OnPropertyChanged("CurrentRule"); }
        }


        private static void Copy(string inputFilePath, string outputFilePath)
        {


          

            using (var inputFile = new FileStream(
    inputFilePath,
    FileMode.Open,
    FileAccess.Read,
    FileShare.ReadWrite))
            {
                using (var outputFile = new FileStream(outputFilePath, FileMode.Create))
                {
                    var buffer = new byte[0x10000];
                    int bytes;

                    while ((bytes = inputFile.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outputFile.Write(buffer, 0, bytes);
                    }
                }
            }
            //int bufferSize = 1024 * 1024;

            //using FileStream outStream = new FileStream(outputFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            //FileStream inStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.ReadWrite,FileShare.ReadWrite);
            //outStream.SetLength(inStream.Length);
            //int bytesRead = -1;
            //byte[] bytes = new byte[bufferSize];

            //while ((bytesRead = inStream.Read(bytes, 0, bufferSize)) > 0)
            //{
            //    outStream.Write(bytes, 0, bytesRead);
            //}

            //outStream.Flush();
            //outStream.Close();
            //outStream.Dispose();

            //inStream.Flush();
            //inStream.Close();
            //inStream.Dispose();

        }






        private static string tempPath = string.Empty;
        private string _attachementPath;
        public string AttachementPath
        {
            get { return _attachementPath; }
            set 
            {
                if (value != null)
                {
                    //Path.GetExtension(value);
                    //tempPath = "D:\\attachements\\temp";

                    tempPath = Path.GetTempFileName();            
                    File.Copy(value, tempPath, true);
                    _attachementPath = tempPath;

                    //Directory.CreateDirectory(Path.GetDirectoryName(temp));
                    //if (File.Exists(tempPath))
                    //{

                    //FileStream s = new FileStream(tempPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    ////s.Lock();
                    //using (s)
                    //{


                    //    //File.Delete(tempPath);
                    //    //File.Copy(value, tempPath, true);


                    //    _attachementPath = tempPath;
                    //}
                    //s.Close();
                    //s.Dispose();
                    //}
                    //else
                    //{
                    //    File.Copy(value, tempPath, true);
                    //    FileStream s = new FileStream(tempPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                    //    s.Close();
                    //    s.Dispose();

                    //    _attachementPath = tempPath;
                    //}




                }
                else
                    _attachementPath = "";
                OnPropertyChanged("AttachementPath");

            }
        }

        //[ MaxLength (3)]

        //public string CurrentRuleNumber
        //{
        //    get { return CurrentRule.AnnualSerialNumber.ToString(); }
        //    set
        //    {

        //        //if (!int.TryParse(days.Text, out _numValue))
        //        //    days.Text = _numValue.ToString();

        //        CurrentRule.AnnualSerialNumber = int.Parse(value); OnPropertyChanged("CurrentRuleNumber");
        //    }
        //}



        LinkedList<RuleAttachement> RuleAttachementsList { get; set; }

        private LinkedListNode<RuleAttachement> _currentNode;
        public LinkedListNode<RuleAttachement> CurrentNode
        {
            get { return _currentNode; }
            set 
            { 
                _currentNode = value; OnPropertyChanged("CurrentNode");
                if (value == null)
                    AttachementsLabel = "لا يوجد مرفقات";
                else
                    AttachementsLabel = "الصورة " + CurrentNode.Value.AttachmentNumber + " من " + RuleAttachementsList.Count;
            }
        }


        public List<short> YearsList { get; private set; }
        public List<Issuer> IssuersList { get; private set; }
        public List<Topic> TopicsList { get; private set; }


        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand OpenFileCommand { get; private set; }
        public RelayCommand PrintCommand { get; private set; }
        public RelayCommand NextCommand { get; private set; }
        public RelayCommand PreviousCommand { get; private set; }
        //public RelayCommand ConfirmAddCommand { get; private set; }
        public RelayCommand ModifyAttachementsCommand { get; private set; }
        public RelayCommand DeleteAttachementCommand { get; private set; }
        public RelayCommand EditAttachementCommand { get; private set; }
        public RelayCommand AddAttachementCommand { get; private set; }
        public RelayCommand SaveAttachementsCommand { get; private set; }







        //public string Error
        //{
        //    get
        //    {
        //        var propertyInfos = GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

        //        foreach (var propertyInfo in propertyInfos)
        //        {
        //            var errorMsg = this[propertyInfo.Name];
        //            if (null != errorMsg)
        //            {
        //                return errorMsg;
        //            }
        //        }

        //        return null;
        //    }
        //}

        //public string this[string columnName]
        //{
        //    get
        //    {
        //        var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

        //        var property = GetType().GetProperty(columnName);
        //        Contract.Assert(null != property);

        //        var validationContext = new ValidationContext(this)
        //        {
        //            MemberName = columnName
        //        };

        //        var isValid = Validator.TryValidateProperty(property.GetValue(this), validationContext, validationResults);
        //        if (isValid)
        //        {
        //            return null;
        //        }

        //        return validationResults.First().ErrorMessage;
        //    }
        //}

        //public bool HasErrors => throw new NotImplementedException();

        //private string defaultAttachementPath = "D:\\Nawzat\\Desktop\\New folder\\website.png";

        private void InitializeUIComponents()
        {
            ValidationMessagesList = new ObservableCollection<string>();
            ValidationMessages = string.Empty;


            ValidationMessagesList.CollectionChanged += (sender, e) =>
            {

                ValidationMessages = FormatMessages(ValidationMessagesList);

            };


            //  IsEditModeOn = false;

            YearsList = new List<short>();
            for (short y = 2000; y < DateTime.Today.Year; y++) YearsList.Add(y);

            TopicsList = RulesMainFullViewModel.db.Topics.ToList();
            IssuersList = RulesMainFullViewModel.db.Issuers.ToList();
            /*new List<string>() { "مراسيم", "رئاسة وزراء", "مجلس تعليم عالي" };*/

            SaveCommand = new RelayCommand(SaveAction, CanExecuteSave);
            CancelCommand = new RelayCommand(CancelAction, CanExecuteCancel);
            OpenFileCommand = new RelayCommand(OpenFileAction, CanExecuteOpenFile);
            PrintCommand = new RelayCommand(PrintFileAction, CanExecutePrint);
            NextCommand = new RelayCommand(NextAction, CanExecuteNext);
            PreviousCommand = new RelayCommand(PreviousAction, CanExecutePrevious);
            //ConfirmAddCommand = new RelayCommand(ConfirmAddAction, CanExecuteConfirmAdd);

            ModifyAttachementsCommand = new RelayCommand(ModifyAttachementsAction, CanExecuteModifyAttachements);
            DeleteAttachementCommand = new RelayCommand(DeleteAttachementAction, CanExecuteDeleteAttachement);
            EditAttachementCommand = new RelayCommand(EditAttachementAction, CanExecuteEditAttachement);
            AddAttachementCommand = new RelayCommand(AddAttachementAction, CanExecuteAddAttachement);
            SaveAttachementsCommand = new RelayCommand(SaveAttachementsAction, CanExecuteSaveAttachements);



            RuleAttachementsList = new LinkedList<RuleAttachement>(CurrentRule.RuleAttachements);

            //foreach (RuleAttachement a in CurrentRule.RuleAttachements)
            //{
            //    RuleAttachementsList.AddLast(new LinkedListNode<RuleAttachement>(a));
            //}
            if (RuleAttachementsList.Count != 0)
                CurrentNode = RuleAttachementsList.First;
            //else
            //    CurrentNode = new LinkedListNode<RuleAttachement>(
            //        new RuleAttachement()
            //        {
            //            RuleId = CurrentRule.RuleId,
            //            AttachmentNumber = 1
            //        });
        }

        public RuleEditViewModel()
        {
            IsEditModeOn = false;

            Message = "إضافة قانون جديد";
            CurrentRule = new Rule();
            InitializeUIComponents();
            //AttachementPath = "D:\\Nawzat\\Desktop\\New folder\\website.png";
        }

        public RuleEditViewModel(Rule rule)
        {
            IsEditModeOn = true;

            Message = "تعديل قانون";
            CurrentRule = rule;

            InitializeUIComponents();

            if (CurrentNode != null)
            {
                AttachementPath = CurrentNode.Value.Path;
            }
        }

        string sourcePath = "";
        string fileExtension = "";
        string destinationPath = "";
        bool uploadedNewFile = false;
        public void PrintFileAction(object o)
        {
            PrintDialog pDialog = new PrintDialog();
            if (pDialog.ShowDialog() == true)
            {
                var queue = pDialog.PrintQueue;

                // Contains extents and offsets
                var area = queue.GetPrintCapabilities(pDialog.PrintTicket).PageImageableArea;



                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.UriSource = new Uri(AttachementPath);
                bi.EndInit();

                double rectWidth = bi.Width;
                double rectHeight = bi.Height;
                double widthFactor = bi.Width / area.ExtentWidth;
                double heightFactor = bi.Height / area.ExtentHeight;

                //if (widthFactor > 1 | heightFactor > 1)
                //{
                if (widthFactor > heightFactor)
                {
                    rectWidth /= widthFactor;
                    rectHeight /= widthFactor;
                }
                else
                {
                    rectWidth /= heightFactor;
                    rectHeight /= heightFactor;
                }
                //}
                DrawingVisual vis = new DrawingVisual();
                using (var dc = vis.RenderOpen())
                {
                    dc.DrawImage(bi, new Rect { X = area.OriginWidth, Y = area.OriginHeight, Width = rectWidth, Height = rectHeight });
                }

                pDialog.PrintVisual(vis, "My Image");
            }

        }
        public void OpenFileAction(object o)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file

                sourcePath = openFileDialog.FileName;
                fileExtension = Path.GetExtension(sourcePath);
                //destinationPath = @"\\10.10.46.103\attachements\" + CurrentRule.RuleYear + "-" + CurrentRule.AnnualSerialNumber + "\\" + (CurrentRule.RuleAttachements.Count + 1) + fileExtension;

                //destinationPath = @"\\10.10.46.103\attachements\" + CurrentRule.RuleYear + "-" + CurrentRule.AnnualSerialNumber + fileExtension;
                destinationPath = @"D:\\attachements\" + CurrentRule.RuleYear + "-" + CurrentRule.AnnualSerialNumber + "\\" + (CurrentRule.RuleAttachements.Count + 1) + fileExtension;

                AttachementPath = sourcePath;

                CurrentNode.Value.Path = destinationPath;

                uploadedNewFile = true;

            }
        }

        public void ModifyAttachementsAction(object o)
        {

            if (CurrentRule.RuleId == 0)
            {
                MessageBoxResult result = MessageBox.Show("سيتم تثبيت بيانات القانون لتتمكن من إضافة مرفقات", "تأكيد", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                if (result.Equals(MessageBoxResult.Cancel))
                {
                    return;
                }
                else if (result.Equals(MessageBoxResult.OK))
                {
                    IsAttachEditModeOn = SaveRuleChanges();
                }
            }
            else
                IsAttachEditModeOn = SaveRuleChanges();

        }

        public bool CanExecuteModifyAttachements(object o)
        {
            return !IsAttachEditModeOn;
        }

        public void AddAttachementAction(object o)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file

                sourcePath = openFileDialog.FileName;
                fileExtension = Path.GetExtension(sourcePath);


                //destinationPath = @"\\10.10.46.103\attachements\" + CurrentRule.IssuerId + "\\" + CurrentRule.RuleYear + "-" + CurrentRule.AnnualSerialNumber + "\\" + (RuleAttachementsList.Count + 1) + fileExtension;

                destinationPath = @"D:\\attachements\" + CurrentRule.IssuerId + "\\" + CurrentRule.RuleYear + "-" + CurrentRule.AnnualSerialNumber + "\\" + (RuleAttachementsList.Count + 1) + fileExtension;

                AttachementPath = sourcePath;

               RuleAttachementsList.AddLast(new LinkedListNode<RuleAttachement>(
                    new RuleAttachement
                    {
                        RuleId = CurrentRule.RuleId,
                        AttachmentNumber = Convert.ToInt16(RuleAttachementsList.Count+1),
                        Path=destinationPath
                    }));

                CurrentNode = RuleAttachementsList.Last;

                //uploadedNewFile = true;

                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                File.Copy(sourcePath, destinationPath, true);
                


            }
        }

        public bool CanExecuteAddAttachement(object o)
        {
            return IsAttachEditModeOn;
        }
        public void EditAttachementAction(object o)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
               

                //Get the path of specified file

                sourcePath = openFileDialog.FileName;
                fileExtension = Path.GetExtension(sourcePath);

                //destinationPath = @"\\10.10.46.103\attachements\" + CurrentRule.IssuerId + "\\" + CurrentRule.RuleYear + "-" + CurrentRule.AnnualSerialNumber + "\\" + CurrentNode.Value.AttachmentNumber + fileExtension;
                destinationPath = @"D:\\attachements\" + CurrentRule.IssuerId + "\\" + CurrentRule.RuleYear + "-" + CurrentRule.AnnualSerialNumber + "\\" + CurrentNode.Value.AttachmentNumber + fileExtension;

                string path = CurrentNode.Value.Path;
                if(!string.IsNullOrEmpty(path))
                File.Delete(path);


                Directory.CreateDirectory(Path.GetDirectoryName(destinationPath));
                File.Copy(sourcePath, destinationPath, true);

                

                CurrentNode.Value.Path = destinationPath;
                AttachementPath = destinationPath;

                //uploadedNewFile = true;

            }
        }

        public bool CanExecuteEditAttachement(object o)
        {
            return IsAttachEditModeOn && CurrentNode != null;
        }
        public void DeleteAttachementAction(object o)
        {
            string path = CurrentNode.Value.Path;
            
            AttachementPath = null;


            var node = CurrentNode;
            RuleAttachementsList.Remove(CurrentNode);
            CurrentNode = node.Next;
            //CurrentNode = CurrentNode.Next;
            File.Delete(path);


            CurrentNode = RuleAttachementsList.First;

            ReArrangeList();

            CurrentNode = RuleAttachementsList.Last;
            AttachementPath = CurrentNode.Value.Path;



        


            
        }

        private void ReArrangeList()
        {
            int i = 1;
           while (CurrentNode != null)
            {
                CurrentNode.Value.AttachmentNumber = (short)(i);

                fileExtension = Path.GetExtension(CurrentNode.Value.Path);

                string newPath = @"D:\\attachements\" + CurrentRule.IssuerId + "\\" + CurrentRule.RuleYear + "-" + CurrentRule.AnnualSerialNumber + "\\" + i + fileExtension;
                File.Move(CurrentNode.Value.Path, newPath, true);
                CurrentNode.Value.Path = newPath;

                i += 1;

                CurrentNode = CurrentNode.Next;

            } 
        }

        public bool CanExecuteDeleteAttachement(object o)
        {
            return IsAttachEditModeOn && CurrentNode != null;
        }
        public void SaveAttachementsAction(object o)
        {
            CurrentRule.RuleAttachements.Clear();
            //short i = 1;
            foreach (var node in RuleAttachementsList)
            {
                //var attachement = new RuleAttachement()
                //{
                //    RuleId = node.RuleId,
                //    AttachmentNumber = i,

                //};
               

                //string v = Path.GetFileNameWithoutExtension(node.Path);
                //if (v.Equals(node.AttachmentNumber.ToString()))
                //{
                //    attachement.Path = node.Path;
                //}
                //else
                //{
                //    destinationPath = @"D:\\attachements\" + CurrentRule.IssuerId + "\\" + CurrentRule.RuleYear + "-" + CurrentRule.AnnualSerialNumber + "\\" + node.AttachmentNumber + fileExtension;
                //    File.Move(node.Path, destinationPath, true);
                //    attachement.Path = destinationPath;
                //}

                CurrentRule.RuleAttachements.Add(node);
                //if()
                //if( if Path.GetFileNameWithoutExtension)

                //i++;
            }


            //RulesMainFullViewModel.db.Rules.Update(CurrentRule);

            RulesMainFullViewModel.db.SaveChanges();

            IsAttachEditModeOn = false;
        }

        public bool CanExecuteSaveAttachements(object o)
        {
            return IsAttachEditModeOn;
        }

        public void UpdateAttachements(object o)
        {
            Rule r = RulesMainFullViewModel.db.Rules.Find(CurrentRule.RuleId);
            r.RuleAttachements.Add(new RuleAttachement()
            {
                RuleId = CurrentRule.RuleId,
                AttachmentNumber = (short)(r.RuleAttachements.Count + 1),
                Path = destinationPath
            });


            RulesMainFullViewModel.db.SaveChanges();
        }

        public void NextAction(object o)
        {
            //LinkedListNode<RuleAttachement> selectedNode = CurrentNode;
            CurrentNode = CurrentNode.Next;




            AttachementPath = CurrentNode.Value.Path;
        }

        public void PreviousAction(object o)
        {
            CurrentNode = CurrentNode.Previous;
            AttachementPath = CurrentNode.Value.Path;
        }

        public bool CanExecuteNext(object o)
        {
            if(CurrentNode!=null)
            return CurrentNode.Next != null;

            return false;
        }

        public bool CanExecutePrevious(object o)
        {
            if (CurrentNode != null)
                return CurrentNode.Previous != null;

            return false;
        }

        public bool CanExecuteSave(object o)
        {
            return !IsAttachEditModeOn;
        }

        public bool CanExecuteOpenFile(object o)
        {
            return IsSaved;
        }
        public bool CanExecutePrint(object o)
        {
            return IsEditModeOn;
        }
        public void ConfirmAddAction(object o)
        {
            if (CurrentRule.LatestUpdateTimestamp == null)
            {
                Rule r = RulesMainFullViewModel.db.Rules.Add(new Rule()
                {
                    AnnualSerialNumber = CurrentRule.AnnualSerialNumber,
                    RuleYear = CurrentRule.RuleYear,
                    LatestUpdateTimestamp = DateTime.Now
                }).Entity;

                r.LatestUpdateUserId = MainUIViewModel.CurrentUser.Id;
                RulesMainFullViewModel.db.SaveChanges();
                IsEditModeOn = true;
            }
        }

        public bool CanExecuteConfirmAdd(object o)
        {
            return IsEditModeOff;
        }

        private bool ValidateRule(Rule rule)
        {
            ValidationMessagesList.Clear();

            bool validYear = true, validSN = true, validIssuer = true, validTopic = true;


            if (rule.RuleYear == 0)
            {
                ValidationMessagesList.Add("الرجاء تحديد عام إصدار القانون");
                validYear = false;
            }
            if (rule.AnnualSerialNumber == 0)
            {
                ValidationMessagesList.Add("الرجاء إدخال الرقم التسلسلي للقانون");
                validSN = false;
            }
            if (rule.IssuerId == 0)
            {
                ValidationMessagesList.Add("الرجاء تحديد الجهة المصدرة للقانون");
                validIssuer = false;
            }
            if (rule.TopicId == 0)
            {
                ValidationMessagesList.Add("الرجاء تحديد  موضوع القانون");
                validIssuer = false;
            }



            // if new Rule
            if (CurrentRule.LatestUpdateTimestamp == null)
            {
                using LegalAffairsDbContext db = new LegalAffairsDbContext();
                var r = db.Rules.Where(x => x.RuleYear == rule.RuleYear && x.AnnualSerialNumber == rule.AnnualSerialNumber && x.IssuerId == rule.IssuerId).ToList();
                if (r.Count() != 0)
                {
                    ValidationMessagesList.Add("القانون موجود مسبقا");
                    return false;
                }
            }
            return validIssuer && validTopic && validSN && validYear;

        }


        private bool SaveRuleChanges()
        {
            if (!ValidateRule(CurrentRule))
                return false;

            // IF NEW ENTITY
            if (CurrentRule.LatestUpdateTimestamp == null)
            {
                CurrentRule = RulesMainFullViewModel.db.Rules.Add(new Rule()
                {
                    AnnualSerialNumber = CurrentRule.AnnualSerialNumber,
                    RuleYear = CurrentRule.RuleYear,
                    IssuerId = CurrentRule.IssuerId,
                    TopicId = CurrentRule.TopicId,
                    Attachement = CurrentRule.Attachement,
                    LatestUpdateTimestamp = DateTime.Now,
                    LatestUpdateUserId = MainUIViewModel.CurrentUser.Id

            }).Entity;


                RulesMainFullViewModel.db.SaveChanges();

            }
            else
            {

                Rule r = RulesMainFullViewModel.db.Rules.Find(CurrentRule.RuleId);

                r.IssuerId = CurrentRule.IssuerId;
                r.TopicId = CurrentRule.TopicId;
                //r.Attachement = CurrentRule.Attachement;
                r.LatestUpdateTimestamp = DateTime.Now;
                r.LatestUpdateUserId = MainUIViewModel.CurrentUser.Id;

                RulesMainFullViewModel.db.Rules.Update(r);
                RulesMainFullViewModel.db.SaveChanges();

            }

            return true;

        }

        public void SaveAction(object o)
        {


            //if (uploadedNewFile)
            //{
            //    (new FileInfo(destinationPath)).Directory.Create();
            //    File.Copy(sourcePath, destinationPath, true);
            //    uploadedNewFile = false;
            //    sourcePath = "";
            //    fileExtension = "";
            //    destinationPath = "";
            //}

            if (SaveRuleChanges())
                this.CloseDialogWithResult(o as Window, DialogResult.Yes);

            //if (string.IsNullOrEmpty(propertyName))
            //{
            //    throw new ArgumentException("Invalid property name", propertyName);
            //}

            //string error = string.Empty;
            ////var value = GetValue(propertyName);
            //var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>(1);
            //var result = Validator.TryValidateProperty(
            //    CurrentRuleNumber,
            //    new ValidationContext(this, null, null)
            //    {
            //        MemberName = "CurrentRuleNumber"
            //    },
            //    results);

            //if (!result)
            //{
            //    var validationResult = results.First();
            //    error = validationResult.ErrorMessage;
            //}



            //IEnumerable<EntityEntry> modified = RulesMainFullViewModel.db.ChangeTracker.Entries().Where(entry =>
            //entry.State == EntityState.Deleted ||
            //entry.State == EntityState.Modified ||
            //entry.State == EntityState.Added);

            //foreach (var entityEntry in modified)
            //{
            //    var rule = (Rule)entityEntry.Entity;

            //    EntityValidationResult validationResult = ValidationHelper.ValidateEntity(rule);
            //    if (validationResult.HasError)
            //    {
            //        InspectEntities(entityEntry);
            //        builderMessages.AppendLine($"{rule.AnnualSerialNumber} - {validationResult.ErrorMessageList()}");
            //    }
            //}

            //List<ValidationResult> results = new List<ValidationResult>();

            //bool valid = Validator.TryValidateObject(CurrentRule, new ValidationContext(CurrentRule), results);

            //RulesMainFullViewModel.db.




        }



        public void CancelAction(object o)
        {
            this.CloseDialogWithResult(o as Window, DialogResult.No);

        }
        public bool CanExecuteCancel(object o)
        {
            return !IsAttachEditModeOn;
        }

        //public IEnumerable GetErrors(string propertyName)
        //{
        //    if (!HasErrors)
        //        return null;
        //    return new List<string>() { "Invalid Input" };
        //}

        //public bool ValidateInput()
        //{
        //    HasErrors = !new 
        //}

    }
}
