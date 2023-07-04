using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.Company;
using BPIWebApplication.Shared.MainModel.FundReturn;
using System.Net.Http.Json;

namespace BPIWebApplication.Client.Services.FundReturnServices
{
    public class FundReturnService : IFundReturnService
    {
        private readonly HttpClient _http;

        public FundReturnService(HttpClient http)
        {
            _http = http;
        }

        public List<FundReturnDocument> fundReturns { get; set; } = new();
        public List<Bank> banks { get; set; } = new();
        public List<FundReturnCategory> fundReturnCategories { get; set; } = new();
        public List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileStreams { get; set; } = new();

        public async Task<ResultModel<FundReturnUploadStream>> createFundReturnDocument(FundReturnUploadStream data)
        {
            ResultModel<FundReturnUploadStream> resData = new ResultModel<FundReturnUploadStream>();

            try
            {
                var result = await _http.PostAsJsonAsync<FundReturnUploadStream>("api/endUser/FundReturn/createFundReturnDocument", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<FundReturnUploadStream>>();

                    resData.Data = respBody.Data;
                    resData.isSuccess = respBody.isSuccess;
                    resData.ErrorCode = respBody.ErrorCode;
                    resData.ErrorMessage = respBody.ErrorMessage;
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<FundReturnUploadStream>>();

                    resData.Data = respBody.Data;
                    resData.isSuccess = respBody.isSuccess;
                    resData.ErrorCode = respBody.ErrorCode;
                    resData.ErrorMessage = respBody.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<ResultModel<QueryModel<FundReturnApprovalStream>>> createFundReturnApproval(QueryModel<FundReturnApprovalStream> data)
        {
            ResultModel<QueryModel<FundReturnApprovalStream>> resData = new ResultModel<QueryModel<FundReturnApprovalStream>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<FundReturnApprovalStream>>("api/endUser/FundReturn/createFundReturnApproval", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<FundReturnApprovalStream>>>();

                    resData.Data = respBody.Data;
                    resData.isSuccess = respBody.isSuccess;
                    resData.ErrorCode = respBody.ErrorCode;
                    resData.ErrorMessage = respBody.ErrorMessage;
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<FundReturnApprovalStream>>>();

                    resData.Data = respBody.Data;
                    resData.isSuccess = respBody.isSuccess;
                    resData.ErrorCode = respBody.ErrorCode;
                    resData.ErrorMessage = respBody.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<ResultModel<QueryModel<string>>> deleteFundReturnDocument(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> resData = new ResultModel<QueryModel<string>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<string>>("api/endUser/FundReturn/deleteFundReturnDocument", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<string>>>();

                    resData.Data = respBody.Data;
                    resData.isSuccess = respBody.isSuccess;
                    resData.ErrorCode = respBody.ErrorCode;
                    resData.ErrorMessage = respBody.ErrorMessage;
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<string>>>();

                    resData.Data = respBody.Data;
                    resData.isSuccess = respBody.isSuccess;
                    resData.ErrorCode = respBody.ErrorCode;
                    resData.ErrorMessage = respBody.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<ResultModel<List<FundReturnDocument>>> getFundReturnDocuments(string param)
        {
            ResultModel<List<FundReturnDocument>> resData = new ResultModel<List<FundReturnDocument>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<FundReturnDocument>>>($"api/endUser/FundReturn/getFundReturnDocuments/{param}");

                if (result.isSuccess)
                {
                    fundReturns.Clear();
                    fundReturns = result.Data;

                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<ResultModel<List<Bank>>> getFundReturnBank()
        {
            ResultModel<List<Bank>> resData = new ResultModel<List<Bank>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Bank>>>("api/endUser/FundReturn/getFundReturnBankData");

                if (result.isSuccess)
                {
                    banks.Clear();
                    banks = result.Data;

                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<ResultModel<List<FundReturnCategory>>> getFundReturnCategory()
        {
            ResultModel<List<FundReturnCategory>> resData = new ResultModel<List<FundReturnCategory>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<FundReturnCategory>>>("api/endUser/FundReturn/getFundReturnCategory");

                if (result.isSuccess)
                {
                    fundReturnCategories.Clear();
                    fundReturnCategories = result.Data;

                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>> getFundReturnFileStream(string param)
        {
            ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>> resData = new ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>>($"api/endUser/FundReturn/getFundReturnFileStream/{param}");

                if (result.isSuccess)
                {
                    fileStreams.AddRange(result.Data);

                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }
            return resData;
        }

        public async Task<ResultModel<List<ReceiptNoResp>>> getFundReturnDetailsItemByReceiptNo(ReceiptNotoTMS param, string token)
        {
            ResultModel<List<ReceiptNoResp>> resData = new();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var result = await _http.PostAsJsonAsync<ReceiptNotoTMS>("api/endUser/TMS/getDetailsItemByReceiptNo", param);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<ReceiptNoResp>>>();

                    if (respBody.isSuccess)
                    {
                        resData.Data = respBody.Data;
                        resData.isSuccess = respBody.isSuccess;
                        resData.ErrorCode = respBody.ErrorCode;
                        resData.ErrorMessage = respBody.ErrorMessage;
                    }
                    else
                    {
                        resData.Data = respBody.Data;
                        resData.isSuccess = respBody.isSuccess;
                        resData.ErrorCode = respBody.ErrorCode;
                        resData.ErrorMessage = respBody.ErrorMessage;
                    }
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<ReceiptNoResp>>>();

                    resData.Data = respBody.Data;
                    resData.isSuccess = respBody.isSuccess;
                    resData.ErrorCode = respBody.ErrorCode;
                    resData.ErrorMessage = respBody.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<ResultModel<int>> getFundReturnModuleNumberOfPage(string param)
        {
            ResultModel<int> resData = new ResultModel<int>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/endUser/FundReturn/getFundReturnModuleNumberOfPage/{param}");

                if (result.isSuccess)
                {
                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
                else
                {
                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = 0;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<int> getFundReturnMaxFileSize()
        {
            ResultModel<int> resData = new ResultModel<int>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/endUser/FundReturn/getFundReturnMaxSizeUpload");

                if (result.isSuccess)
                {
                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;

                }
                else
                {
                    resData.Data = result.Data;
                    resData.isSuccess = result.isSuccess;
                    resData.ErrorCode = result.ErrorCode;
                    resData.ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                resData.Data = 0;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData.Data;
        }

        //
    }
}
