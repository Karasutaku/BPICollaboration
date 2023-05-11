using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.EPKRS;

namespace BPIWebApplication.Client.Services.EPKRSServices
{
    public interface IEPKRSService
    {
        List<ReportingType> reportingTypes { get; set; }
        List<RiskType> riskTypes { get; set; }
        List<ItemRiskCategory> itemRiskCategories { get; set; }

        List<EPKRSUploadItemCase> itemCases { get; set; }
        List<EPKRSUploadIncidentAccident> incidentAccidents { get; set; }

        List<DocumentDiscussion> documentDiscussions { get; set; }
        List<DocumentDiscussionReadHistory> documentDiscussionReadHistories { get; set; }
        List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileStreams { get; set; }

        Task<ResultModel<ItemCaseStream>> createEPKRSItemCaseDocument(ItemCaseStream data);
        Task<ResultModel<IncidentAccidentStream>> createEPKRSIncidentAccidentDocument(IncidentAccidentStream data);
        Task<ResultModel<DocumentDiscussionStream>> createEPKRSDocumentDiscussion(DocumentDiscussionStream data);
        Task<ResultModel<QueryModel<DocumentDiscussionReadStream>>> createEPKRSDocumentDiscussionReadHistory(QueryModel<DocumentDiscussionReadStream> data);
        Task<ResultModel<QueryModel<DocumentApproval>>> createEPKRSDocumentApprovalData(QueryModel<DocumentApproval> data);
        Task<ResultModel<QueryModel<RISKApprovalExtended>>> createEPKRSDocumentApprovalExtendedData(QueryModel<RISKApprovalExtended> data);

        Task<ResultModel<QueryModel<ItemCase>>> editEPKRSItemCaseData(QueryModel<ItemCase> data);
        Task<ResultModel<QueryModel<IncidentAccident>>> editEPKRSIncidentAccidentData(QueryModel<IncidentAccident> data);

        Task<ResultModel<List<ReportingType>>> getEPRKSReportingType();
        Task<ResultModel<List<RiskType>>> getEPRKSRiskType();
        Task<ResultModel<List<ItemRiskCategory>>> getEPKRSItemRiskCategory();
        Task<ResultModel<List<EPKRSUploadItemCase>>> getEPKRSItemCaseData(string param);
        Task<ResultModel<List<EPKRSUploadIncidentAccident>>> getEPKRSIncidentAccidentData(string param);
        Task<ResultModel<List<DocumentDiscussion>>> getEPKRSDocumentDiscussion(string param);
        Task<ResultModel<List<DocumentDiscussion>>> getEPKRSInitializationDocumentDiscussions(List<DocumentListParams> param);
        Task<ResultModel<List<DocumentDiscussionReadHistory>>> getEPKRSDocumentDiscussionReadHistory(List<DocumentListParams> param);
        Task<ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>> getEPKRSFileStream(string param);
        Task<ResultModel<List<EPKRSDocumentStatistics>>> getEPKRSGeneralStatistics(string param);
        Task<ResultModel<List<EPKRSItemCaseCategoryStatistics>>> getEPKRSItemCaseCategoryStatistics(string param);
        Task<ResultModel<List<EPKRSTopLocationReportStatistics>>> getEPKRSTopLocationReportStatistics(string param);
        Task<ResultModel<List<EPKRSItemCaseItemCategoryStatistics>>> getEPKRSItemCategoriesStatistics(string param);

        Task<int> getEPKRSMaxFileSize();
        Task<ResultModel<int>> getEPKRSModuleNumberOfPage(string param);

    }
}
