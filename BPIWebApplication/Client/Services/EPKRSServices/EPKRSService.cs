using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.EPKRS;
using System.Net.Http.Json;

namespace BPIWebApplication.Client.Services.EPKRSServices
{
    public class EPKRSService : IEPKRSService
    {
        private readonly HttpClient _http;

        public EPKRSService(HttpClient http)
        {
            _http = http;
        }

        public List<ReportingType> reportingTypes { get; set; } = new();
        public List<RiskType> riskTypes { get; set; } = new();
        public List<EPKRSUploadItemCase> itemCases { get; set; } = new();
        public List<EPKRSUploadIncidentAccident> incidentAccidents { get; set; } = new();
        public List<DocumentDiscussion> documentDiscussions { get; set; } = new();
        public List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileStreams { get; set; } = new();

        public async Task<ResultModel<ItemCaseStream>> createEPKRSItemCaseDocument(ItemCaseStream data)
        {
            ResultModel<ItemCaseStream> resData = new ResultModel<ItemCaseStream>();

            try
            {
                var result = await _http.PostAsJsonAsync<ItemCaseStream>("api/endUser/EPKRS/createEPKRSItemCaseDocument", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<ItemCaseStream>>();

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
                    resData.ErrorMessage = "Failure from createEPKRSItemCaseDocument endUser";
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

        public async Task<ResultModel<IncidentAccidentStream>> createEPKRSIncidentAccidentDocument(IncidentAccidentStream data)
        {
            ResultModel<IncidentAccidentStream> resData = new ResultModel<IncidentAccidentStream>();

            try
            {
                var result = await _http.PostAsJsonAsync<IncidentAccidentStream>("api/endUser/EPKRS/createEPKRSIncidentAccidentDocument", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<IncidentAccidentStream>>();

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
                    resData.ErrorMessage = "Failure from createEPKRSIncidentAccidentDocument endUser";
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

        public async Task<ResultModel<DocumentDiscussionStream>> createEPKRSDocumentDiscussion(DocumentDiscussionStream data)
        {
            ResultModel<DocumentDiscussionStream> resData = new ResultModel<DocumentDiscussionStream>();

            try
            {
                var result = await _http.PostAsJsonAsync<DocumentDiscussionStream>("api/endUser/EPKRS/createEPKRSDocumentDiscussion", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<DocumentDiscussionStream>>();

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
                    resData.ErrorMessage = "Failure from createEPKRSDocumentDiscussion endUser";
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

        public async Task<ResultModel<QueryModel<DocumentApproval>>> createEPKRSDocumentApprovalData(QueryModel<DocumentApproval> data)
        {
            ResultModel<QueryModel<DocumentApproval>> resData = new ResultModel<QueryModel<DocumentApproval>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<DocumentApproval>>("api/endUser/EPKRS/createEPKRSDocumentApproval", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<DocumentApproval>>>();

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
                    resData.ErrorMessage = "Failure from createEPKRSDocumentApprovalData endUser";
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

        public async Task<ResultModel<QueryModel<RISKApprovalExtended>>> createEPKRSDocumentApprovalExtendedData(QueryModel<RISKApprovalExtended> data)
        {
            ResultModel<QueryModel<RISKApprovalExtended>> resData = new ResultModel<QueryModel<RISKApprovalExtended>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<RISKApprovalExtended>>("api/endUser/EPKRS/createEPKRSDocumentApprovalExtended", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<RISKApprovalExtended>>>();

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
                    resData.ErrorMessage = "Failure from createEPKRSDocumentApprovalExtended endUser";
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

        public async Task<ResultModel<QueryModel<ItemCase>>> editEPKRSItemCaseData(QueryModel<ItemCase> data)
        {
            ResultModel<QueryModel<ItemCase>> resData = new ResultModel<QueryModel<ItemCase>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<ItemCase>>("api/endUser/EPKRS/editEPKRSItemCaseData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<ItemCase>>>();

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
                    resData.ErrorMessage = "Failure from editEPKRSItemCaseData endUser";
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

        public async Task<ResultModel<QueryModel<IncidentAccident>>> editEPKRSIncidentAccidentData(QueryModel<IncidentAccident> data)
        {
            ResultModel<QueryModel<IncidentAccident>> resData = new ResultModel<QueryModel<IncidentAccident>>();

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<IncidentAccident>>("api/endUser/EPKRS/editEPKRSIncidentAccidentData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<IncidentAccident>>>();

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
                    resData.ErrorMessage = "Failure from editEPKRSIncidentAccidentData endUser";
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

        public async Task<ResultModel<List<ReportingType>>> getEPRKSReportingType()
        {
            ResultModel<List<ReportingType>> resData = new ResultModel<List<ReportingType>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<ReportingType>>>("api/endUser/EPKRS/getEPRKSReportingType");

                if (result.isSuccess)
                {
                    reportingTypes.Clear();
                    reportingTypes = result.Data;

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

        public async Task<ResultModel<List<RiskType>>> getEPRKSRiskType()
        {
            ResultModel<List<RiskType>> resData = new ResultModel<List<RiskType>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<RiskType>>>("api/endUser/EPKRS/getEPKRSRiskType");

                if (result.isSuccess)
                {
                    riskTypes.Clear();
                    riskTypes = result.Data;

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

        public async Task<ResultModel<List<EPKRSUploadItemCase>>> getEPKRSItemCaseData(string param)
        {
            ResultModel<List<EPKRSUploadItemCase>> resData = new ResultModel<List<EPKRSUploadItemCase>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<EPKRSUploadItemCase>>>($"api/endUser/EPKRS/getEPKRSItemCase/{param}");

                if (result.isSuccess)
                {
                    itemCases.Clear();
                    itemCases = result.Data;

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

        public async Task<ResultModel<List<EPKRSUploadIncidentAccident>>> getEPKRSIncidentAccidentData(string param)
        {
            ResultModel<List<EPKRSUploadIncidentAccident>> resData = new ResultModel<List<EPKRSUploadIncidentAccident>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<EPKRSUploadIncidentAccident>>>($"api/endUser/EPKRS/getEPKRSIncidentAccident/{param}");

                if (result.isSuccess)
                {
                    incidentAccidents.Clear();
                    incidentAccidents = result.Data;

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

        public async Task<ResultModel<List<DocumentDiscussion>>> getEPKRSDocumentDiscussion(string param)
        {
            ResultModel<List<DocumentDiscussion>> resData = new ResultModel<List<DocumentDiscussion>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<DocumentDiscussion>>>($"api/endUser/EPKRS/getEPKRSDocumentDiscussion/{param}");

                if (result.isSuccess)
                {
                    documentDiscussions.Clear();
                    documentDiscussions = result.Data;

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

        public async Task<ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>> getEPKRSFileStream(string param)
        {
            ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>> resData = new ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>>($"api/endUser/EPKRS/getEPKRSFileStream/{param}");

                if (result.isSuccess)
                {
                    //fileStreams = result.Data;
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

        public async Task<int> getEPKRSMaxFileSize()
        {
            ResultModel<int> resData = new ResultModel<int>();

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/endUser/EPKRS/getEPKRSMaxSizeUpload");

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
