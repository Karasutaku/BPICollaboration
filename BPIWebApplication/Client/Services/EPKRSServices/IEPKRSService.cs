using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.EPKRS;

namespace BPIWebApplication.Client.Services.EPKRSServices
{
    public interface IEPKRSService
    {
        List<ReportingType> reportingTypes { get; set; }
        List<RiskType> riskTypes { get; set; }
        List<RiskSubType> riskSubTypes { get; set; }
        List<ItemRiskCategory> itemRiskCategories { get; set; }
        List<IncidentAccidentInvolverType> involverType { get; set; }

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

        Task<ResultModel<QueryModel<EPKRSUploadItemCase>>> editEPKRSItemCaseData(QueryModel<EPKRSUploadItemCase> data);
        Task<ResultModel<QueryModel<IncidentAccident>>> editEPKRSIncidentAccidentData(QueryModel<IncidentAccident> data);

        Task<ResultModel<QueryModel<string>>> deleteEPKRSItemCaseDocumentData(QueryModel<string> data);
        Task<ResultModel<QueryModel<string>>> deleteEPKRSIncidentAccidentDocumentData(QueryModel<string> data);

        Task<ResultModel<List<ReportingType>>> getEPRKSReportingType();
        Task<ResultModel<List<RiskType>>> getEPRKSRiskType();
        Task<ResultModel<List<RiskSubType>>> getEPRKSRiskSubType();
        Task<ResultModel<List<ItemRiskCategory>>> getEPKRSItemRiskCategory();
        Task<ResultModel<List<IncidentAccidentInvolverType>>> getEPKRSIncidentAccidentInvolverType();
        Task<ResultModel<List<EPKRSUploadItemCase>>> getEPKRSItemCaseData(QueryModel<string> param);
        Task<ResultModel<List<EPKRSUploadIncidentAccident>>> getEPKRSIncidentAccidentData(QueryModel<string> param);
        Task<ResultModel<List<DocumentDiscussion>>> getEPKRSDocumentDiscussion(string param);
        Task<ResultModel<List<DocumentDiscussion>>> getEPKRSInitializationDocumentDiscussions(List<DocumentListParams> param);
        Task<ResultModel<List<DocumentDiscussionReadHistory>>> getEPKRSDocumentDiscussionReadHistory(List<DocumentListParams> param);
        Task<ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>> getEPKRSFileStream(string param);
        Task<ResultModel<List<EPKRSDocumentStatistics>>> getEPKRSGeneralStatistics(QueryModel<string> param);
        Task<ResultModel<List<EPKRSIncidentAccidentforStats>>> getEPKRSIncidentAccidentDataforStatistics(QueryModel<string> param);
        Task<ResultModel<List<EPKRSDocumentStatistics>>> getEPKRSGeneralIncidentAccidentStatistics(QueryModel<string> param);
        Task<ResultModel<List<EPKRSItemCaseCategoryStatistics>>> getEPKRSItemCaseCategoryStatistics(QueryModel<string> param);
        Task<ResultModel<List<EPKRSTopLocationReportStatistics>>> getEPKRSTopLocationReportStatistics(QueryModel<string> param);
        Task<ResultModel<List<EPKRSItemCaseItemCategoryStatistics>>> getEPKRSItemCategoriesStatistics(QueryModel<string> param);
        Task<ResultModel<List<EPKRSIncidentAccidentRegionalStatistics>>> getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmail(QueryModel<string> param);
        Task<ResultModel<List<EPKRSIncidentAccidentInvolverStatisticsbyPosition>>> getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPosition(QueryModel<string> param);
        Task<ResultModel<List<EPKRSIncidentAccidentInvolverStatisticsbyDept>>> getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDept(QueryModel<string> param);
        Task<ResultModel<List<EPKRSIncidentAccidentLocationStatistics>>> getEPKRSIncidentAccidentLocationStatistics(QueryModel<string> param);

        // Reporting

        Task<ResultModel<BPIWebApplication.Shared.MainModel.Stream.FileStream>> getEPKRSIncidentAccidentReport(QueryModel<string> param);
        Task<ResultModel<BPIWebApplication.Shared.MainModel.Stream.FileStream>> getEPKRSItemCaseReport(QueryModel<string> param);

        // TMS
        Task<ResultModel<List<LoadingManifestResp>>> getEPKRSItemDetailsbyLMNo(LMNotoTMS param, string token);

        Task<int> getEPKRSMaxFileSize();
        Task<int> getEPKRSMaxStatisticsRow();
        Task<ResultModel<int>> getEPKRSModuleNumberOfPage(QueryModel<string> param);

    }
}
