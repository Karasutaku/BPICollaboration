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
        public string LocationID { get; set; } = string.Empty;
        public List<POMFItemLine> pomfItemLines { get; set; } = new();
        public POMFApproval approvalData { get; set; } = new();
    }

    public class POMFItemLinesMaxQuantity
    {
        public string POMFID { get; set; } = string.Empty;
        public string ItemCode { get; set; } = string.Empty;
        public int MaxQuantity { get; set; } = 0;
    }

    public class NPwithReceiptNotoTMS
    {
        public string receiptNo { get; set; } = string.Empty;
        public string npNo { get; set; } = string.Empty;
    }

    public class POMFTMSParam
    {
        public NPwithReceiptNotoTMS tmsParam { get; set; } = new();
        public string facadeParam { get; set; } = string.Empty;
    }

    public class NPwithReceiptNoResp
    {
        public string itemCode { get; set; } = string.Empty;
        public string itemDesc { get; set; } = string.Empty;
        public decimal qtyNP { get; set; } = decimal.Zero;
        public string uom { get; set; } = string.Empty;
        public int type { get; set; } = 0;
    }

    public class NPwithReceiptFetchDetail
    {
        public List<NPwithReceiptNoResp>? tmsResp { get; set; } = new();
        public List<POMFItemLinesMaxQuantity>? itemDetailsMaxQuantity { get; set; } = new();
    }
}
