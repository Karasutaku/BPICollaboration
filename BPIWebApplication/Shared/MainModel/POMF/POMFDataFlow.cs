namespace BPIWebApplication.Shared.MainModel.POMF
{
    public class POMFDocument
    {
        public POMFHeader dataHeader { get; set; } = new();
        public List<POMFItemLine> dataItemLines { get; set; } = new();
        public List<POMFApproval> dataApproval { get; set; } = new();
    }

    public class POMFApprovalStream
    {
        public string LocationID { get; set; } = string.Empty;
        public POMFApproval Data { get; set; } = new();
    }


    public class POMFApprovalStreamExtended
    {
        public POMFHeader pomfHeader { get; set; } = new();
        public POMFApproval approvalData { get; set; } = new();
    }

    public class NPwithReceiptNotoTMS
    {
        public string receiptNo { get; set; } = string.Empty;
        public string npNo { get; set; } = string.Empty;
    }

    public class NPwithReceiptNoResp
    {
        private int qtyrcp = 0;

        public string itemCode { get; set; } = string.Empty;
        public string itemDesc { get; set; } = string.Empty;
        public string qtyNP
        {
            get => qtyrcp.ToString("N0");
            set
            {
                var x = 0;

                x = Convert.ToInt32(Math.Round(Convert.ToDecimal(value)));

                qtyrcp = Math.Abs(x);
            }
        }
        public string uom { get; set; } = string.Empty;
    }
}
