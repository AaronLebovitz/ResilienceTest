using System;

using AppKit;
using Foundation;
using ResilienceClasses;

namespace RCTest
{
    public partial class ViewController : NSViewController
    {

        string strTestPath;

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        partial void BramsTextBoxEntered(Foundation.NSObject sender)
        {
        }


        partial void ClickedButton(AppKit.NSButton sender)
        {
            this._TestProperty();
        }

        partial void ClickedTestCashflowButton(NSButton sender)
        {
            this._TestCashflow();
        }

        partial void ClickedTestOthersButton(AppKit.NSButton sender)
        {
//            this._TestProperty();
//            this._TestCashflow();
//            this._TestDocument();
//            this._TestDocumentRecord();
//            this._TestEntity();
            this._TestLoan();
        }

        private void _TestProperty()
        {
            clsProperty property = new clsProperty(14);
            Console.WriteLine(property.PropertyID() + "," + property.Address());

            clsCSVTable tbl = new clsCSVTable(clsProperty.strPropertyPath);
            tbl.SaveAs("/Users/aaronlebovitz/Documents/Professional/Resilience/tblPropertyTest.csv");
            clsProperty badproperty = new clsProperty("no", "no", "no", "no", -1, "no");
            Console.WriteLine(badproperty.Save("/Users/aaronlebovitz/Documents/Professional/Resilience/tblPropertyTest.csv"));

            tbl = new clsCSVTable(clsProperty.strPropertyPath);
            string[] strNewRecord = new string[tbl.Width() - 1];
            for (int i = 0; i < strNewRecord.Length; i++)
            {
                strNewRecord[i] = "new " + i.ToString();
            }
            tbl.New(strNewRecord);
            tbl.SaveAs("/Users/aaronlebovitz/Documents/Professional/Resilience/tblPropertyTest.csv");
        }

        private void _TestCashflow()
        {
            clsCashflow cf = new clsCashflow(47);
            Console.WriteLine(cf.TransactionID() + "," + cf.Amount() + "," + cf.PayDate());

            clsCSVTable testTable = new clsCSVTable(clsCashflow.strCashflowPath);
            testTable.SaveAs("/Users/aaronlebovitz/Documents/Professional/Resilience/tblCashflowTest.csv");

            DateTime pd = DateTime.Today;
            DateTime rd = DateTime.Today;
            DateTime dd = DateTime.MaxValue;
            int pid = 17;
            double a = 100000;
            bool act = false;
            clsCashflow.Type t = clsCashflow.Type.RehabDraw;
            clsCashflow cfTest = new clsCashflow(pd, rd, dd, pid, a, act, t);
            Console.WriteLine(cfTest.Save("/Users/aaronlebovitz/Documents/Professional/Resilience/tblCashflowTest.csv"));
        }

        private void _TestDocument()
        {
            for (int i = 0; i < 25; i++)
            {
                clsDocument document = new clsDocument(i);
                Console.WriteLine(document.Name() + "," + document.PropertyAddress() + "," + document.DocumentType().ToString() + "," + 
                                  ((int)document.DocumentType()).ToString());
            }
            clsCSVTable testTable = new clsCSVTable(clsDocument.strDocumentPath);
            testTable.SaveAs("/Users/aaronlebovitz/Documents/Professional/Resilience/tblDocumentTest.csv");

            clsDocument newDoc = new clsDocument("test", 1, clsDocument.Type.ClosingProtectionLetter);
            Console.WriteLine(newDoc.Save("/Users/aaronlebovitz/Documents/Professional/Resilience/tblDocumentTest.csv"));
        }

        private void _TestDocumentRecord()
        {
            for (int i = 0; i < 25; i++)
            {
                clsDocumentRecord docRec = new clsDocumentRecord(i);
                Console.WriteLine(docRec.StatusType().ToString() + "," + docRec.TransmissionType().ToString() + "," +
                                  docRec.DocumentID().ToString() + "," + docRec.ReceiverID().ToString());
            }

            clsCSVTable testTable = new clsCSVTable(clsDocumentRecord.strDocumentRecordPath);
            testTable.SaveAs("/Users/aaronlebovitz/Documents/Professional/Resilience/tblDocumentRecordTest.csv");
            clsDocumentRecord newRec = new clsDocumentRecord(1, System.DateTime.Now, System.DateTime.Today, 1, 2,
                                                             clsDocumentRecord.Status.Notarized,
                                                             clsDocumentRecord.Transmission.Electronic);
            Console.WriteLine(newRec.Save("/Users/aaronlebovitz/Documents/Professional/Resilience/tblDocumentRecordTest.csv"));
        }

        private void _TestEntity()
        {
            for (int i = 0; i < 3; i++)
            {
                clsEntity entity = new clsEntity(i);
                Console.WriteLine(entity.Name() + "," + entity.Address() + "," + entity.Phone());
            }

            clsCSVTable testTable = new clsCSVTable(clsEntity.strEntityPath);
            testTable.SaveAs("/Users/aaronlebovitz/Documents/Professional/Resilience/tblEntityTest.csv");
            clsEntity newEntity = new clsEntity("test entity", "42 wallaby way", "sydney", "AU", 33000, "312-fnd-nemo", "marlin", "clown@fish.com");
            Console.WriteLine(newEntity.Save("/Users/aaronlebovitz/Documents/Professional/Resilience/tblEntityTest.csv"));
        }

        private void _TestLoan()
        {
            System.IO.StreamWriter sr = new System.IO.StreamWriter("/Users/aaronlebovitz/Documents/Professional/Resilience/DebugLog.txt");
            for (int i = 0; i < 1; i++)
            {
                this._WriteLoan(sr, i);
            }
            sr.Close();
        }

        private void _WriteLoan(System.IO.StreamWriter sr, int loanID)
        {
            clsEntity titleHolder;
            clsEntity coBorrower;
            clsEntity titleCompany;

            clsLoan loan = new clsLoan(loanID);
            titleHolder = new clsEntity(loan.TitleHolderID());
            titleCompany = new clsEntity(loan.TitleCompanyID());
            coBorrower = new clsEntity(loan.CoBorrowerID());

            sr.WriteLine("BEGIN LOAN " + loanID.ToString() + " : " + loan.PropertyID().ToString());

            sr.Write(loan.Property().Address() + "," + loan.Property().State() + ",");
            sr.Write(titleHolder.Name() + ",");
            sr.Write(coBorrower.Name() + ",");
            sr.WriteLine(titleCompany.Name());

            sr.Write(loan.Rate().ToString() + ",");
            sr.Write(loan.PenaltyRate().ToString() + ",");
            sr.Write(loan.OriginationDate().ToLongDateString() + ",");
            sr.WriteLine(loan.MaturityDate().ToLongDateString());

            sr.Write(loan.Status() + ",");
            sr.Write(loan.Balance().ToString() + ",");
            sr.Write(loan.AccruedInterest().ToString() + ",");
            sr.Write(loan.ProjectedHardInterest().ToString() + ",");
            sr.WriteLine(loan.ImpliedAdditionalInterest().ToString());

            sr.Write(loan.Return(false).ToString() + ",");
            sr.WriteLine(loan.IRR(false).ToString());

            sr.Write(loan.Return(true).ToString() +",");
            sr.WriteLine(loan.IRR(true).ToString());

            sr.Write(loan.RehabSpent().ToString() + ",");
            sr.Write(loan.RehabRemain().ToString() + ",");
            sr.WriteLine(loan.FirstRehabEstimate().ToString());

            sr.WriteLine("END OF LOAN");
            sr.Flush();
        }
    }
}
