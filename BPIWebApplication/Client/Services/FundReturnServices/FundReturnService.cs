﻿using BPIWebApplication.Shared.DbModel;
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

        public async Task<ResultModel<QueryModel<FundReturnDocument>>> createFundReturnDocument(QueryModel<FundReturnDocument> data)
        {
            ResultModel<QueryModel<FundReturnDocument>> resData = new ResultModel<QueryModel<FundReturnDocument>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<FundReturnDocument>>("api/endUser/FundReturn/createFundReturnDocument", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<FundReturnDocument>>>();

                    resData.Data = respBody.Data;
                    resData.isSuccess = respBody.isSuccess;
                    resData.ErrorCode = respBody.ErrorCode;
                    resData.ErrorMessage = respBody.ErrorMessage;
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<FundReturnDocument>>>();

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

        //
    }
}
