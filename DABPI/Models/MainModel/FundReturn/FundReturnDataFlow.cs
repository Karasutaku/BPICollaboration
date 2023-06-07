namespace BPIDA.Models.MainModel.FundReturn
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
}
