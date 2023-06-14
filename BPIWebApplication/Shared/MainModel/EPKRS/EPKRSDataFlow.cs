using BPIWebApplication.Shared.DbModel;
using System.Data;

namespace BPIWebApplication.Shared.MainModel.EPKRS
{
    public class EPKRSUploadItemCase
    {
        public ItemCase itemCase { get; set; } = new();
        public List<ItemLine> itemLine { get; set; } = new();
        public List<CaseAttachment> attachment { get; set; } = new();
        public List<DocumentApproval> Approval { get; set; } = new();
    }

    public class EPKRSUploadIncidentAccident
    {
        public IncidentAccident incidentAccident { get; set; } = new();
        public List<CaseAttachment> attachment { get; set; } = new();
        public List<DocumentApproval> Approval { get; set; } = new();
        public List<IncidentAccidentInvolver> Involver { get; set; } = new();
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
        public List<BPIWebApplication.Shared.MainModel.Stream.FileStream> files { get; set; } = new();
    }

    public class IncidentAccidentStream
    {
        public QueryModel<EPKRSUploadIncidentAccident> mainData { get; set; } = new();
        public List<BPIWebApplication.Shared.MainModel.Stream.FileStream> files { get; set; } = new();
    }

    public class DocumentDiscussionStream
    {
        public QueryModel<EPKRSUploadDiscussion> mainData { get; set; } = new();
        public List<BPIWebApplication.Shared.MainModel.Stream.FileStream> files { get; set; } = new();
    }

    public class RISKApprovalExtended
    {
        public string reportingType { get; set; } = string.Empty;
        public DocumentApproval approval { get; set; } = new();
        public ReportingExtended extendedData { get; set; } = new();
        public List<IncidentAccidentInvolver> involver { get; set; } = new();
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

    public class LMNotoTMS
    {
        public string lmNo { get; set; } = string.Empty;
    }

    public class LoadingManifestResp
    {
        private string loc = string.Empty;
        private DateTime date;

        public string lmNo { get; set; } = string.Empty;
        public string siteNo {
            get => loc;
            set {
                loc = new string(value.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());
            }
        }
        public string trNo { get; set; } = string.Empty;
        public string itemCode { get; set; } = string.Empty;
        public string itemDesc { get; set; } = string.Empty;
        public decimal itemValue { get; set; } = decimal.Zero;
        public string uom { get; set; } = string.Empty;
        public string sentRequestDate { 
            get => date.ToString("yyyy-MM-dd");
            set { 
                date = Convert.ToDateTime(new string(value.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray()));
            } 
        }
    }

}
