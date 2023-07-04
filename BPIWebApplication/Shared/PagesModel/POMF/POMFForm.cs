using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace BPIWebApplication.Shared.PagesModel.POMF
{
    public class POMFHeaderForm
    {
        public string POMFID { get; set; } = string.Empty;
        public DateTime POMFDate { get; set; } = DateTime.Now;
        public string LocationID { get; set; } = string.Empty;
        [Required(ErrorMessage = "Customer Name Data is required")]
        public string CustomerName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Receipt No Data is required")]
        public string ReceiptNo { get; set; } = string.Empty;
        [Required(ErrorMessage = "NP No Data is required")]
        public string NPNo { get; set; } = string.Empty;
        [Required(ErrorMessage = "NP Type Data is required")]
        public string NPTypeID { get; set; } = string.Empty;
        public string Requester { get; set; } = string.Empty;
        public string DocumentStatus { get; set; } = string.Empty;
    }

    public class POMFItemLineForm
    {
        private decimal acc = decimal.Zero;
        private string id = string.Empty;

        public string POMFID { get; set; } = string.Empty;
        public int LineNum { get; set; } = 0;
        public string ItemCode {
            get => id;
            set
            {
                string res = new string((from c in value where char.IsDigit(c) select c).ToArray());
                id = res;
            }
        }
        public string ItemDescription { get; set; } = string.Empty;
        public int RequestQuantity { get; set; } = 0;
        public int NPQuantity { get; set; } = 0;
        public string ItemUOM { get; set; } = string.Empty;
        public string ItemValue { 
            get => acc.ToString("N0"); 
            set {
                if (value.Length <= 0)
                {
                    acc = Math.Round(decimal.Zero);
                }
                else
                {
                    string res = new string((from c in value where char.IsLetterOrDigit(c) select c).ToArray());
                    if (Decimal.TryParse(res, (NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign), CultureInfo.CreateSpecificCulture("en-US"), out var number))
                        acc = Math.Round(number);
                }
            }
        }
        public string RequestToSite { get; set; } = string.Empty;
        public string ExternalRequestDocument { get; set; } = string.Empty;
        public DateTime RequestDocumentDate { get; set; } = DateTime.Now;
        public string ExternalReceiveDocument { get; set; } = string.Empty;
        public DateTime ReceiveDocumentDate { get; set; } = DateTime.Now;
    }

    public class NPwithReceiptNoForm
    {
        public string itemCode { get; set; } = string.Empty;
        public string itemDesc { get; set; } = string.Empty;
        public int qtyNP { get; set; } = 0;
        public int maxQty { get; set; } = 0;
        public string uom { get; set; } = string.Empty;
        public int type { get; set; } = 0;
    }
}
