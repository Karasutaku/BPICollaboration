using BPILibrary;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.EPKRS;
using BPIWebApplication.Shared.MainModel.POMF;
using System.Net.Http.Json;

namespace BPIWebApplication.Client.Services.POMFServices
{
    public class POMFService : IPOMFService
    {
        private readonly HttpClient _http;

        public POMFService(HttpClient http)
        {
            _http = http;
        }

        public List<POMFDocument> pomfDocuments { get; set; } = new();
        public List<POMFDocument> pomfConfirmedDocuments { get; set; } = new();
        public List<POMFNPType> npTypes { get; set; } = new();

        public async Task<ResultModel<QueryModel<POMFDocument>>> createPOMFDocument(QueryModel<POMFDocument> data)
        {
            ResultModel<QueryModel<POMFDocument>> resData = new ResultModel<QueryModel<POMFDocument>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<POMFDocument>>("api/endUser/POMF/createPOMFDocument", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<POMFDocument>>>();

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
                    resData.Data = null;
                    resData.isSuccess = false;
                    resData.ErrorCode = "01";
                    resData.ErrorMessage = "Failure from createPOMFDocument endUser";
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

        public async Task<ResultModel<QueryModel<POMFApprovalStream>>> createPOMFApproval(QueryModel<POMFApprovalStream> data)
        {
            ResultModel<QueryModel<POMFApprovalStream>> resData = new ResultModel<QueryModel<POMFApprovalStream>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<POMFApprovalStream>>("api/endUser/POMF/createPOMFApproval", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<POMFApprovalStream>>>();

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
                    resData.Data = null;
                    resData.isSuccess = false;
                    resData.ErrorCode = "01";
                    resData.ErrorMessage = "Failure from createPOMFApproval endUser";
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

        public async Task<ResultModel<QueryModel<POMFApprovalStreamExtended>>> createPOMFApprovalExtended(QueryModel<POMFApprovalStreamExtended> data)
        {
            ResultModel<QueryModel<POMFApprovalStreamExtended>> resData = new ResultModel<QueryModel<POMFApprovalStreamExtended>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<POMFApprovalStreamExtended>>("api/endUser/POMF/createPOMFApprovalExtended", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<POMFApprovalStreamExtended>>>();

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
                    resData.Data = null;
                    resData.isSuccess = false;
                    resData.ErrorCode = "01";
                    resData.ErrorMessage = "Failure from createPOMFApprovalExtended endUser";
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

        public async Task<ResultModel<QueryModel<string>>> deletePOMFDocument(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> resData = new ResultModel<QueryModel<string>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<string>>("api/endUser/POMF/deletePOMFDocument", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<string>>>();

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
                    resData.Data = null;
                    resData.isSuccess = false;
                    resData.ErrorCode = "01";
                    resData.ErrorMessage = "Failure from deletePOMFDocument endUser";
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

        public async Task<ResultModel<List<POMFDocument>>> getPOMFDocuments(string param)
        {
            ResultModel<List<POMFDocument>> resData = new ResultModel<List<POMFDocument>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<POMFDocument>>>($"api/endUser/POMF/getPOMFDocuments/{param}");

                if (result.isSuccess)
                {
                    if (CommonLibrary.Base64Decode(param).Split("!_!")[3].Equals("CONF"))
                    {
                        pomfConfirmedDocuments.Clear();
                        pomfConfirmedDocuments = result.Data;
                    }
                    else
                    {
                        pomfDocuments.Clear();
                        pomfDocuments = result.Data;
                    }

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

        public async Task<ResultModel<List<POMFNPType>>> getPOMFNPType()
        {
            ResultModel<List<POMFNPType>> resData = new ResultModel<List<POMFNPType>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<POMFNPType>>>("api/endUser/POMF/getPOMFNPType");

                if (result.isSuccess)
                {
                    npTypes.Clear();
                    npTypes = result.Data;

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

        public async Task<ResultModel<List<POMFItemLinesMaxQuantity>>> getPOMFItemLineMaxQuantity(string param)
        {
            ResultModel<List<POMFItemLinesMaxQuantity>> resData = new ResultModel<List<POMFItemLinesMaxQuantity>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<POMFItemLinesMaxQuantity>>>($"api/endUser/POMF/getPOMFItemLineMaxQuantity/{param}");

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
                resData.Data = null;
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData;
        }

        public async Task<ResultModel<List<NPwithReceiptNoResp>>> getDetailsItemByReceiptNoAndNPNo(NPwithReceiptNotoTMS param, string token)
        {
            ResultModel<List<NPwithReceiptNoResp>> resData = new();

            try
            {
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var result = await _http.PostAsJsonAsync<NPwithReceiptNotoTMS>("api/endUser/TMS/getDetailsItemByReceiptNoAndNPNo", param);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<NPwithReceiptNoResp>>>();

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
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<NPwithReceiptNoResp>>>();

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

        public async Task<ResultModel<int>> getPOMFModuleNumberOfPage(string param)
        {
            ResultModel<int> resData = new ResultModel<int>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/endUser/POMF/getPOMFModuleNumberOfPage/{param}");

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

        public async Task<string[]> getPOMFAcceptedDocPrefix(string param)
        {
            ResultModel<string[]> resData = new ResultModel<string[]>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<string[]>>($"api/endUser/POMF/getPOMFAcceptedDocPrefix/{param}");

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
                resData.Data = new string[] { };
                resData.isSuccess = false;
                resData.ErrorCode = "99";
                resData.ErrorMessage = ex.Message;
            }

            return resData.Data;
        }

        public async Task<int> getPOMFAcceptedDocLength(string param)
        {
            ResultModel<int> resData = new ResultModel<int>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/endUser/POMF/getPOMFAcceptedDocLength/{param}");

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
