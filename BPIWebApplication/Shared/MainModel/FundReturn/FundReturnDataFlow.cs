using BPIWebApplication.Shared.DbModel;

namespace BPIWebApplication.Shared.MainModel.FundReturn
{
    public class FundReturnDocument
    {
        public FundReturnHeader dataHeader { get; set; } = new();
        public List<FundReturnItemLine> dataItemLines { get; set; } = new();
        public List<FundReturnAttachment> dataAttachmentLines { get; set; } = new();
        public List<FundReturnApproval> dataApproval { get; set; } = new();
    }

    public class FundReturnApprovalStream
    {
        public string LocationID { get; set; } = string.Empty;
        public FundReturnApproval Data { get; set; } = new();
    }

    public class FundReturnUploadStream
    {
        public QueryModel<FundReturnDocument> mainData { get; set; } = new();
        public List<BPIWebApplication.Shared.MainModel.Stream.FileStream> files { get; set; } = new();
    }

    public class ReceiptNotoTMS
    {
        public string receiptNo { get; set; } = string.Empty;
    }

    public class ReceiptNoResp
    {
        public string itemCode { get; set; } = string.Empty;
        public string itemDesc { get; set; } = string.Empty;
        public decimal qtyReceipt { get; set; } = 0;
        public decimal unitAmount { get; set; } = decimal.Zero;
        public decimal unitAmountNet { get; set; } = decimal.Zero;
        public string uom { get; set; } = string.Empty;
    }
}
