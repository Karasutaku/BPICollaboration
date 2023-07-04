using BPIFacade.Models.DbModel;

namespace BPIFacade.Models.MainModel.FundReturn
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
        public List<BPIFacade.Models.MainModel.Stream.FileStream> files { get; set; } = new();
    }
}
