namespace BPIWebApplication.Shared.MainModel.FundReturn
{
    public class FundReturnDocument
    {
        public FundReturnHeader dataHeader { get; set; } = new();
        public List<FundReturnItemLine> dataItemLines { get; set; } = new();
        public List<FundReturnApproval> dataApproval { get; set; } = new();
    }

    public class FundReturnApprovalStream
    {
        public string LocationID { get; set; } = string.Empty;
        public FundReturnApproval Data { get; set; } = new();
    }

    public class ReceiptNotoTMS
    {
        public string receiptNo { get; set; } = string.Empty;
    }

    public class ReceiptNoResp
    {
        int qtyrcp = 0;

        public string itemCode { get; set; } = string.Empty;
        public string itemDesc { get; set; } = string.Empty;
        public int qtyReceipt
        {
            get => qtyrcp;
            set
            {
                var x = Convert.ToInt32(Math.Round(Convert.ToDecimal(value)));

                qtyrcp = Math.Abs(x);
            }
        }
        public decimal unitValue { get; set; } = decimal.Zero;
        public string uom { get; set; } = string.Empty;
    }
}
