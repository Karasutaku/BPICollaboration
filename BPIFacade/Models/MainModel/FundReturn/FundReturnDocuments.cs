namespace BPIFacade.Models.MainModel.FundReturn
{
    public class FundReturnHeader
    {
        public string DocumentID { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; } = DateTime.Now;
        public string LocationID { get; set; } = string.Empty;
        public string CommercialType { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerType { get; set; } = string.Empty;
        public string CustomerMemberID { get; set; } = string.Empty;
        public string CustomerContactNo { get; set; } = string.Empty;
        public string FundReturnCategoryID { get; set; } = string.Empty;
        public string BankHolderName { get; set; } = string.Empty;
        public string BankAccount { get; set; } = string.Empty;
        public string BankID { get; set; } = string.Empty;
        public string ReceiptDocument { get; set; } = string.Empty;
        public string ExternalDocument { get; set; } = string.Empty;
        public decimal RefundAmount { get; set; } = decimal.Zero;
        public decimal TransactionAmount { get; set; } = decimal.Zero;
        public string Reason { get; set; } = string.Empty;
        public string DocumentStatus { get; set; } = string.Empty;
    }

    public class FundReturnItemLine
    {
        public string DocumentID { get; set; } = string.Empty;
        public int LineNum { get; set; } = 0;
        public string ItemCode { get; set; } = string.Empty;
        public string ItemDescription { get; set; } = string.Empty;
        public int ItemQuantity { get; set; } = 0;
        public string UOM { get; set; } = string.Empty;
        public decimal ItemAmount { get; set; } = decimal.Zero;
        public int ItemDiscount { get; set; } = 0;
    }

    public class FundReturnApproval
    {
        public string DocumentID { get; set; } = string.Empty;
        public string ApprovalAction { get; set; } = string.Empty;
        public string Approver { get; set; } = string.Empty;
        public DateTime ApproveDate { get; set; } = DateTime.Now;
    }

    public class FundReturnCategory
    {
        public string FundReturnCategoryID { get; set; } = string.Empty;
        public string FundReturnCategoryDescription { get; set; } = string.Empty;
    }
}
