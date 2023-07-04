using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.EPKRS;
using BPIWebApplication.Shared.MainModel.POMF;

namespace BPIWebApplication.Client.Services.POMFServices
{
    public interface IPOMFService
    {
        List<POMFDocument> pomfDocuments { get; set; }
        List<POMFDocument> pomfConfirmedDocuments { get; set; }
        List<POMFNPType> npTypes { get; set; }

        Task<ResultModel<QueryModel<POMFDocument>>> createPOMFDocument(QueryModel<POMFDocument> data);
        Task<ResultModel<QueryModel<POMFApprovalStream>>> createPOMFApproval(QueryModel<POMFApprovalStream> data);
        Task<ResultModel<QueryModel<POMFApprovalStreamExtended>>> createPOMFApprovalExtended(QueryModel<POMFApprovalStreamExtended> data);
        Task<ResultModel<QueryModel<string>>> deletePOMFDocument(QueryModel<string> data);

        Task<ResultModel<List<POMFDocument>>> getPOMFDocuments(string param);
        Task<ResultModel<List<POMFNPType>>> getPOMFNPType();
        Task<ResultModel<List<POMFItemLinesMaxQuantity>>> getPOMFItemLineMaxQuantity(string param);

        // TMS

        Task<ResultModel<List<NPwithReceiptNoResp>>> getDetailsItemByReceiptNoAndNPNo(NPwithReceiptNotoTMS param, string token);

        Task<ResultModel<int>> getPOMFModuleNumberOfPage(string param);
        Task<string[]> getPOMFAcceptedDocPrefix(string param);
        Task<int> getPOMFAcceptedDocLength(string param);
    }
}
