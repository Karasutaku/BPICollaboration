using BPIFacade.Models.DbModel;
using System.Data;

namespace BPIFacade.Models.MainModel.EPKRS
{
    public class EPKRSUploadItemCase
    {
        public ItemCase itemCase { get; set; } = new();
        public List<ItemLine> itemLine { get; set; } = new();
        public List<CaseAttachment> attachment { get; set; } = new();
    }

    public class EPKRSUploadIncidentAccident
    {
        public IncidentAccident incidentAccident { get; set; } = new();
        public List<CaseAttachment> attachment { get; set; } = new();
    }

    public class EPKRSUploadDiscussion
    {
        public DocumentDiscussion discussion { get; set; } = new();
        public List<CaseAttachment> attachment { get; set; } = new();
        public string LocationID { get; set; } = string.Empty;
    }

    public class ItemCaseStream
    {
        public QueryModel<EPKRSUploadItemCase> mainData { get; set; } = new();
        public List<BPIFacade.Models.MainModel.Stream.FileStream> files { get; set; } = new();
    }

    public class IncidentAccidentStream
    {
        public QueryModel<EPKRSUploadIncidentAccident> mainData { get; set; } = new();
        public List<BPIFacade.Models.MainModel.Stream.FileStream> files { get; set; } = new();
    }

    public class DocumentDiscussionStream
    {
        public QueryModel<EPKRSUploadDiscussion> mainData { get; set; } = new();
        public List<BPIFacade.Models.MainModel.Stream.FileStream> files { get; set; } = new();
    }

    public class RISKApprovalExtended
    {
        public string reportingType { get; set; } = string.Empty;
        public DocumentApproval approval { get; set; } = new();
        public ReportingExtended extendedData { get; set; } = new();
    }

    public class DocumentDiscussionReadStream
    {
        public string LocationID { get; set; } = string.Empty;
        public List<DocumentDiscussionReadHistory> Data { get; set; } = new();
    }

    public class DocumentListParams
    {
        public string LocationID { get; set; } = string.Empty;
        public string DocumentID { get; set; } = string.Empty;
    }
}
