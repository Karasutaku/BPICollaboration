using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.EPKRS;

namespace BPIWebApplication.Client.Services.EPKRSServices
{
    public interface IEPKRSSevice
    {
        List<ReportingType> reportingTypes { get; set; }
        List<RiskType> riskTypes { get; set; }
        
        List<EPKRSUploadItemCase> itemCases { get; set; }
        List<EPKRSUploadIncidentAccident> incidentAccidents { get; set; }

        List<DocumentDiscussion> documentDiscussions { get; set; }

        Task<ResultModel<ItemCaseStream>> createEPKRSItemCaseDocument(ItemCaseStream data);
        Task<ResultModel<IncidentAccidentStream>> createEPKRSIncidentAccidentDocument(IncidentAccidentStream data);
        Task<ResultModel<QueryModel<EPKRSUploadDiscussion>>> createEPKRSDocumentDiscussion(QueryModel<EPKRSUploadDiscussion> data);

        Task<ResultModel<List<ReportingType>>> getEPRKSReportingType();
        Task<ResultModel<List<RiskType>>> getEPRKSRiskType();
        Task<ResultModel<List<EPKRSUploadItemCase>>> getEPKRSItemCaseData(string param);
        Task<ResultModel<List<DocumentDiscussion>>> getEPKRSDocumentDiscussion(string param);

    }
}
