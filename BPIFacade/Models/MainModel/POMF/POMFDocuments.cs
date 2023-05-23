namespace BPIFacade.Models.MainModel.POMF
{
    public class POMFHeader
    {
        public string POMFID { get; set; } = string.Empty;
        public DateTime POMFDate { get; set; } = DateTime.Now;
        public string LocationID { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string ReceiptNo { get; set; } = string.Empty;
        public string NPNo { get; set; } = string.Empty;
        public string NPTypeID { get; set; } = string.Empty;
        public string ExternalRequestDocument { get; set; } = string.Empty;
        public DateTime RequestDocumentDate { get; set; } = DateTime.Now;
        public string ExternalReceiveDocument { get; set; } = string.Empty;
        public DateTime ReceiveDocumentDate { get; set; } = DateTime.Now;
        public string Requester { get; set; } = string.Empty;
        public string DocumentStatus { get; set; } = string.Empty;
    }

    public class POMFItemLine
    {
        public string POMFID { get; set; } = string.Empty;
        public int LineNum { get; set; } = 0;
        public string ItemCode { get; set; } = string.Empty;
        public string ItemDescription { get; set; } = string.Empty;
        public int RequestQuantity { get; set; } = 0;
        public int NPQuantity { get; set; } = 0;
        public string ItemUOM { get; set; } = string.Empty;
        public decimal ItemValue { get; set; } = decimal.Zero;
    }

    public class POMFApproval
    {
        public string POMFID { get; set; } = string.Empty;
        public string ApprovalAction { get; set; } = string.Empty;
        public string Approver { get; set; } = string.Empty;
        public DateTime ApproveDate { get; set; } = DateTime.Now;
    }

    public class POMFNPType
    {
        public string NPTypeID { get; set; } = string.Empty;
        public string NPTypeDescription { get; set; } = string.Empty;
        public bool isAutomaticApproval { get; set; } = false;
    }
}
