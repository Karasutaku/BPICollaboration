using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.Company;
using BPIWebApplication.Shared.MainModel.FundReturn;

namespace BPIWebApplication.Client.Services.FundReturnServices
{
    public interface IFundReturnService
    {
        List<FundReturnDocument> fundReturns { get; set; }
        List<Bank> banks { get; set; }
        List<FundReturnCategory> fundReturnCategories { get; set; }
        List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileStreams { get; set; }

        Task<ResultModel<FundReturnUploadStream>> createFundReturnDocument(FundReturnUploadStream data);
        Task<ResultModel<QueryModel<FundReturnApprovalStream>>> createFundReturnApproval(QueryModel<FundReturnApprovalStream> data);
        Task<ResultModel<QueryModel<string>>> deleteFundReturnDocument(QueryModel<string> data);

        Task<ResultModel<List<FundReturnDocument>>> getFundReturnDocuments(string param);
        Task<ResultModel<List<Bank>>> getFundReturnBank();
        Task<ResultModel<List<FundReturnCategory>>> getFundReturnCategory();
        Task<ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>> getFundReturnFileStream(string param);

        // TMS
        Task<ResultModel<List<ReceiptNoResp>>> getFundReturnDetailsItemByReceiptNo(ReceiptNotoTMS param, string token);


        Task<ResultModel<int>> getFundReturnModuleNumberOfPage(string param);
        Task<int> getFundReturnMaxFileSize();
    }
}
