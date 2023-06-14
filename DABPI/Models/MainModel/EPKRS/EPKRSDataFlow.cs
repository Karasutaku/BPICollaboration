﻿using System.Data;

namespace BPIDA.Models.MainModel.EPKRS
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
}
