using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BPIWebApplication.Shared.PagesModel.FundReturn
{
    public class FundReturnHeaderForm
    {
        private decimal refAmt = decimal.Zero;
        private decimal tranAmt = decimal.Zero;

        public string DocumentID { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public string LocationID { get; set; } = string.Empty;
        public string CommercialType { get; set; } = string.Empty;
        [Required(ErrorMessage = "Customer Name Data is required")]
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerType { get; set; } = string.Empty;
        public string CustomerMemberID { get; set; } = string.Empty;
        [Required(ErrorMessage = "Customer Contact No Data is required")]
        public string CustomerContactNo { get; set; } = string.Empty;
        public string FundReturnCategoryID { get; set; } = string.Empty;
        [Required(ErrorMessage = "Bank Holder Name Data is required")]
        public string BankHolderName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Bank Account Data is required")]
        public string BankAccount { get; set; } = string.Empty;
        public string BankID { get; set; } = string.Empty;
        public string ReceiptDocument { get; set; } = string.Empty;
        public string ExternalDocument { get; set; } = string.Empty;
        public string RefundAmount
        {
            get => refAmt.ToString("N0");
            set
            {
                if (value.Length <= 0)
                {
                    refAmt = Math.Round(decimal.Zero);
                }
                else
                {
                    string res = new string((from c in value where char.IsLetterOrDigit(c) select c).ToArray());
                    if (Decimal.TryParse(res, (NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign), CultureInfo.CreateSpecificCulture("en-US"), out var number))
                        refAmt = Math.Round(number);
                }
            }
        }
        public string TransactionAmount {
            get => tranAmt.ToString("N0");
            set
            {
                if (value.Length <= 0)
                {
                    tranAmt = Math.Round(decimal.Zero);
                }
                else
                {
                    string res = new string((from c in value where char.IsLetterOrDigit(c) select c).ToArray());
                    if (Decimal.TryParse(res, (NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign), CultureInfo.CreateSpecificCulture("en-US"), out var number))
                        tranAmt = Math.Round(number);
                }
            }
        }
        [Required(ErrorMessage = "Reason Data is required")]
        public string Reason { get; set; } = string.Empty;
        public string DocumentStatus { get; set; } = string.Empty;
    }

    public class FundReturnItemLineForm
    {
        private decimal itemAmt = decimal.Zero;

        public string DocumentID { get; set; } = string.Empty;
        public int LineNum { get; set; } = 0;
        public string ItemCode { get; set; } = string.Empty;
        public string ItemDescription { get; set; } = string.Empty;
        public int ItemQuantity { get; set; } = 0;
        public string UOM { get; set; } = string.Empty;
        public decimal ItemAmount { get; set; } = decimal.Zero;
        public int ItemDiscount { get; set; } = 0;
    }
}
