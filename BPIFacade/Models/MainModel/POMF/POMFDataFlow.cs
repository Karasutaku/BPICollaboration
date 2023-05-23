namespace BPIFacade.Models.MainModel.POMF
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
}
