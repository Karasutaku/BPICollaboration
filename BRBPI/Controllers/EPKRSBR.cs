using BPIBR.Models.DbModel;
using BPIBR.Models.MainModel;
using BPIBR.Models.MainModel.EPKRS;
using BPIBR.Models.MainModel.Mailing;
using BPILibrary;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BPIBR.Controllers
{
    [Route("api/BR/EPKRS")]
    [ApiController]
    public class EPKRSBR : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        private readonly string _uploadPath;
        private readonly string _autoEmailUser, _autoEmailPass, _mailHost;
        private readonly int _mailPort;

        public EPKRSBR(HttpClient http, IConfiguration config)
        {
            _http = http;
            _configuration = config;
            _http.BaseAddress = new Uri(_configuration.GetValue<string>("BaseUri:BpiDA"));
            _uploadPath = _configuration.GetValue<string>("File:EPKRS:UploadPath");
            _autoEmailUser = _configuration.GetValue<string>("AutoEmailCreds:Email");
            _autoEmailPass = _configuration.GetValue<string>("AutoEmailCreds:Ticket");
            _mailHost = _configuration.GetValue<string>("AutoEmailCreds:Host");
            _mailPort = _configuration.GetValue<int>("AutoEmailCreds:Port");
        }

        [HttpPost("createEPKRSItemCaseDocument")]
        public async Task<IActionResult> createEPKRSItemCaseDocumentData(ItemCaseStream data)
        {
            ResultModel<ItemCaseStream> res = new ResultModel<ItemCaseStream>();
            res.Data = new();
            res.Data.mainData = new();
            IActionResult actionResult = null;

            try
            {
                var mailing = Task.Run(async () =>
                {
                    string param = CommonLibrary.Base64Encode("EPKRS!_!CreateITC!_!ALL");
                    var dtMail = await _http.GetFromJsonAsync<ResultModel<Mailing>>($"api/DA/PettyCash/getMailingDetails/{param}");

                    return dtMail;
                });
                
                var otherRcv = Task.Run(async () =>
                {
                    string param = CommonLibrary.Base64Encode($"EPKRS!_!CreateITC!_!{data.mainData.Data.itemCase.SiteReporter}");
                    var dtRcv = await _http.GetFromJsonAsync<ResultModel<Mailing>>($"api/DA/PettyCash/getMailingDetails/{param}");

                    return dtRcv;
                });

                var result = await _http.PostAsJsonAsync<QueryModel<EPKRSUploadItemCase>>($"api/DA/EPKRS/createEPKRSItemCaseDocument", data.mainData);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<EPKRSUploadItemCase>>>();

                    if (respBody.isSuccess)
                    {
                        data.files.ForEach(x =>
                        {
                            string path = Path.Combine(_uploadPath, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), respBody.Data.Data.itemCase.DocumentID, x.fileName);

                            CommonLibrary.saveFiletoDirectory(path, x.content);
                        });
                    }

                    res.Data.mainData = respBody.Data;
                    res.Data.files = null;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    if (respBody.isSuccess && mailing.IsCompletedSuccessfully && otherRcv.IsCompletedSuccessfully)
                    {
                        var to = new List<string>() { new string(mailing.Result.Data.Receiver) };
                        List<string>? cc = null;

                        if (otherRcv.Result.ErrorCode.Equals("00"))
                            cc = new List<string>() { new string(otherRcv.Result.Data.Receiver), new string(data.mainData.userEmail) };
                        else
                            cc = new List<string>() { new string(data.mainData.userEmail) };

                        await CommonLibrary.sendEmail(
                            _autoEmailUser
                            , to
                            , cc
                            , new NetworkCredential(_autoEmailUser, _autoEmailPass)
                            , mailing.Result.Data.MailSubject
                            , string.Format(mailing.Result.Data.MailMainBody, respBody.Data.Data.itemCase.DocumentID)
                            , true
                            , _mailPort
                            , _mailHost
                            , true
                        );
                    }

                    actionResult = Ok(res);
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<EPKRSUploadItemCase>>>();

                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpPost("createEPKRSIncidentAccidentDocument")]
        public async Task<IActionResult> createEPKRSIncidentAccidentDocumentData(IncidentAccidentStream data)
        {
            ResultModel<IncidentAccidentStream> res = new ResultModel<IncidentAccidentStream>();
            res.Data = new();
            res.Data.mainData = new();
            IActionResult actionResult = null;

            try
            {
                var mailing = Task.Run(async () =>
                {
                    string param = CommonLibrary.Base64Encode("EPKRS!_!CreateINC!_!ALL");
                    var dtMail = await _http.GetFromJsonAsync<ResultModel<Mailing>>($"api/DA/PettyCash/getMailingDetails/{param}");

                    return dtMail;
                });

                var result = await _http.PostAsJsonAsync<QueryModel<EPKRSUploadIncidentAccident>>($"api/DA/EPKRS/createEPKRSIncidentAccidentDocument", data.mainData);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<EPKRSUploadIncidentAccident>>>();

                    if (respBody.isSuccess)
                    {
                        data.files.ForEach(x =>
                        {
                            string path = Path.Combine(_uploadPath, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), respBody.Data.Data.incidentAccident.DocumentID, x.fileName);

                            CommonLibrary.saveFiletoDirectory(path, x.content);
                        });
                    }

                    res.Data.mainData = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    if (respBody.isSuccess && mailing.IsCompletedSuccessfully)
                    {
                        var to = new List<string>() { new string(data.mainData.Data.incidentAccident.DORMEmail) };
                        var cc = new List<string>() { new string(mailing.Result.Data.Receiver), new string(data.mainData.userEmail), new string(data.mainData.Data.incidentAccident.RiskRPEmail), new string(data.mainData.Data.incidentAccident.PIC) };
                        await CommonLibrary.sendEmail(
                            _autoEmailUser
                            , to
                            , cc
                            , new NetworkCredential(_autoEmailUser, _autoEmailPass)
                            , mailing.Result.Data.MailSubject
                            , string.Format(mailing.Result.Data.MailMainBody, respBody.Data.Data.incidentAccident.DocumentID)
                            , true
                            , _mailPort
                            , _mailHost
                            , true
                        );
                    }

                    actionResult = Ok(res);
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<EPKRSUploadIncidentAccident>>>();

                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpPost("createEPKRSDocumentDiscussion")]
        public async Task<IActionResult> createEPKRSDocumentDiscussionData(DocumentDiscussionStream data)
        {
            ResultModel<DocumentDiscussionStream> res = new ResultModel<DocumentDiscussionStream>();
            res.Data = new();
            res.Data.mainData = new();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<EPKRSUploadDiscussion>>($"api/DA/EPKRS/createEPKRSDocumentDiscussion", data.mainData);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<EPKRSUploadDiscussion>>>();

                    if (respBody.isSuccess)
                    {
                        if (data.files.Count > 0)
                        {
                            data.files.ForEach(x =>
                            {
                                string path = Path.Combine(_uploadPath, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), respBody.Data.Data.discussion.DocumentID, x.fileName);

                                CommonLibrary.saveFiletoDirectory(path, x.content);
                            });
                        }

                        res.Data.mainData = respBody.Data;

                        res.isSuccess = respBody.isSuccess;
                        res.ErrorCode = respBody.ErrorCode;
                        res.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(res);
                    }
                    else
                    {
                        res.Data.mainData = respBody.Data;
                        res.isSuccess = respBody.isSuccess;
                        res.ErrorCode = respBody.ErrorCode;
                        res.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(res);
                    }
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<EPKRSUploadDiscussion>>>();

                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpPost("createEPKRSDocumentDiscussionReadHistory")]
        public async Task<IActionResult> createEPKRSDocumentDiscussionReadHistory(QueryModel<DocumentDiscussionReadStream> data)
        {
            ResultModel<QueryModel<DocumentDiscussionReadStream>> res = new ResultModel<QueryModel<DocumentDiscussionReadStream>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<DocumentDiscussionReadStream>>("api/DA/EPKRS/createEPKRSDocumentDiscussionReadHistory", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<DocumentDiscussionReadStream>>>();

                    if (respBody.isSuccess)
                    {
                        res.Data = respBody.Data;
                        res.isSuccess = respBody.isSuccess;
                        res.ErrorCode = respBody.ErrorCode;
                        res.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(res);
                    }
                    else
                    {
                        res.Data = respBody.Data;
                        res.isSuccess = respBody.isSuccess;
                        res.ErrorCode = respBody.ErrorCode;
                        res.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(res);
                    }
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<DocumentDiscussionReadStream>>>();

                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpPost("createEPKRSDocumentApproval")]
        public async Task<IActionResult> createEPKRSDocumentApprovalData(QueryModel<DocumentApproval> data)
        {
            ResultModel<QueryModel<DocumentApproval>> res = new ResultModel<QueryModel<DocumentApproval>>();
            IActionResult actionResult = null;

            try
            {
                var mailing = Task.Run(async () =>
                {
                    string param = CommonLibrary.Base64Encode("EPKRS!_!Approval!_!ALL");
                    var dtMail = await _http.GetFromJsonAsync<ResultModel<Mailing>>($"api/DA/PettyCash/getMailingDetails/{param}");

                    return dtMail;
                });

                var result = await _http.PostAsJsonAsync<QueryModel<DocumentApproval>>($"api/DA/EPKRS/createEPKRSDocumentApproval", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<DocumentApproval>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    if (respBody.isSuccess && mailing.IsCompletedSuccessfully)
                    {
                        var to = new List<string>() { new string(mailing.Result.Data.Receiver) };
                        var cc = new List<string>() { new string(data.Data.Approver) };
                        var mail = await CommonLibrary.sendEmail(
                            "information.bpi@mitra10.com"
                            , to
                            , null
                            , new NetworkCredential(_autoEmailUser, _autoEmailPass)
                            , mailing.Result.Data.MailSubject
                            , string.Format(mailing.Result.Data.MailMainBody, data.Data.DocumentID, data.Data.ApprovalAction, data.Data.ApproveDate.ToString("dddd, dd MMMM yyyy HH:mm:ss"))
                            , true
                            , _mailPort
                            , _mailHost
                            , true
                        );
                    }

                    actionResult = Ok(res);
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<DocumentApproval>>>();

                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpPost("createEPKRSDocumentApprovalExtended")]
        public async Task<IActionResult> createEPKRSDocumentApprovalExtendedData(QueryModel<RISKApprovalExtended> data)
        {
            ResultModel<QueryModel<RISKApprovalExtended>> res = new ResultModel<QueryModel<RISKApprovalExtended>>();
            IActionResult actionResult = null;

            try
            {
                var mailing = Task.Run(async () =>
                {
                    string param = CommonLibrary.Base64Encode("EPKRS!_!Approval!_!ALL");
                    var dtMail = await _http.GetFromJsonAsync<ResultModel<Mailing>>($"api/DA/PettyCash/getMailingDetails/{param}");

                    return dtMail;
                });

                var result = await _http.PostAsJsonAsync<QueryModel<RISKApprovalExtended>>($"api/DA/EPKRS/createEPKRSDocumentApprovalExtended", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<RISKApprovalExtended>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    mailing.Wait();

                    if (respBody.isSuccess && mailing.IsCompletedSuccessfully)
                    {
                        var to = new List<string>() { new string(mailing.Result.Data.Receiver) };
                        await CommonLibrary.sendEmail(
                            data.Data.approval.Approver
                            , to
                            , null
                            , new NetworkCredential(_autoEmailUser, _autoEmailPass)
                            , mailing.Result.Data.MailSubject
                            , string.Format(mailing.Result.Data.MailMainBody, data.Data.approval.DocumentID, data.Data.approval.ApprovalAction, data.Data.approval.ApproveDate.ToString("dddd, dd MMMM yyyy"))
                            , true
                            , _mailPort
                            , _mailHost
                            , true
                        );
                    }

                    actionResult = Ok(res);
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<RISKApprovalExtended>>>();

                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpPost("editEPKRSItemCaseData")]
        public async Task<IActionResult> editEPKRSItemCase(QueryModel<EPKRSUploadItemCase> data)
        {
            ResultModel<QueryModel<EPKRSUploadItemCase>> res = new ResultModel<QueryModel<EPKRSUploadItemCase>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<EPKRSUploadItemCase>>($"api/DA/EPKRS/editEPKRSItemCaseData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<EPKRSUploadItemCase>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<EPKRSUploadItemCase>>>();

                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpPost("editEPKRSIncidentAccidentData")]
        public async Task<IActionResult> editEPKRSIncidentAccident(QueryModel<IncidentAccident> data)
        {
            ResultModel<QueryModel<IncidentAccident>> res = new ResultModel<QueryModel<IncidentAccident>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<IncidentAccident>>($"api/DA/EPKRS/editEPKRSIncidentAccidentData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<IncidentAccident>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<IncidentAccident>>>();

                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpPost("deleteEPKRSItemCaseDocumentData")]
        public async Task<IActionResult> deleteEPKRSItemCaseDocumentData(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<string>>($"api/DA/EPKRS/deleteEPKRSItemCaseDocumentData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<string>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<string>>>();

                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpPost("deleteEPKRSIncidentAccidentDocumentData")]
        public async Task<IActionResult> deleteEPKRSIncidentAccidentDocumentData(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<string>>($"api/DA/EPKRS/deleteEPKRSIncidentAccidentDocumentData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<string>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<string>>>();

                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }

            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpGet("getEPRKSReportingType")]
        public async Task<IActionResult> getEPRKSReportingType()
        {
            ResultModel<List<ReportingType>> res = new ResultModel<List<ReportingType>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<ReportingType>>>("api/DA/EPKRS/getEPRKSReportingType");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getEPKRSRiskType")]
        public async Task<IActionResult> getEPRKSRiskType()
        {
            ResultModel<List<RiskType>> res = new ResultModel<List<RiskType>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<RiskType>>>("api/DA/EPKRS/getEPKRSRiskType");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getEPKRSRiskSubType")]
        public async Task<IActionResult> getEPKRSRiskSubType()
        {
            ResultModel<List<RiskSubType>> res = new ResultModel<List<RiskSubType>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<RiskSubType>>>("api/DA/EPKRS/getEPKRSRiskSubType");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getEPKRSItemRiskCategory")]
        public async Task<IActionResult> getEPKRSItemRiskCategory()
        {
            ResultModel<List<ItemRiskCategory>> res = new ResultModel<List<ItemRiskCategory>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<ItemRiskCategory>>>("api/DA/EPKRS/getEPKRSItemRiskCategory");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getEPKRSIncidentAccidentInvolverType")]
        public async Task<IActionResult> getEPKRSIncidentAccidentInvolverType()
        {
            ResultModel<List<IncidentAccidentInvolverType>> res = new ResultModel<List<IncidentAccidentInvolverType>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<IncidentAccidentInvolverType>>>("api/DA/EPKRS/getEPKRSIncidentAccidentInvolverType");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getEPKRSItemCase/{param}")]
        public async Task<IActionResult> getEPKRSItemCaseData(string param)
        {
            ResultModel<List<EPKRSUploadItemCase>> res = new ResultModel<List<EPKRSUploadItemCase>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<EPKRSUploadItemCase>>>($"api/DA/EPKRS/getEPKRSItemCase/{param}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getEPKRSIncidentAccident/{param}")]
        public async Task<IActionResult> getEPKRSIncidentAccidentData(string param)
        {
            ResultModel<List<EPKRSUploadIncidentAccident>> res = new ResultModel<List<EPKRSUploadIncidentAccident>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<EPKRSUploadIncidentAccident>>>($"api/DA/EPKRS/getEPKRSIncidentAccident/{param}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getEPKRSDocumentDiscussion/{param}")]
        public async Task<IActionResult> getEPKRSDocumentDiscussion(string param)
        {
            ResultModel<List<DocumentDiscussion>> res = new ResultModel<List<DocumentDiscussion>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<DocumentDiscussion>>>($"api/DA/EPKRS/getEPKRSDocumentDiscussion/{param}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpPost("getEPKRSInitializationDocumentDiscussions")]
        public async Task<IActionResult> getEPKRSInitializationDocumentDiscussions(List<DocumentListParams> param)
        {
            ResultModel<List<DocumentDiscussion>> res = new ResultModel<List<DocumentDiscussion>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<List<DocumentListParams>>("api/DA/EPKRS/getEPKRSInitializationDocumentDiscussions", param);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<DocumentDiscussion>>>();

                    if (respBody.isSuccess)
                    {
                        res.Data = respBody.Data;
                        res.isSuccess = respBody.isSuccess;
                        res.ErrorCode = respBody.ErrorCode;
                        res.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(res);
                    }
                    else
                    {
                        res.Data = respBody.Data;
                        res.isSuccess = respBody.isSuccess;
                        res.ErrorCode = respBody.ErrorCode;
                        res.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(res);
                    }
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<DocumentDiscussion>>>();

                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpPost("getEPKRSDocumentDiscussionReadHistory")]
        public async Task<IActionResult> getEPKRSDocumentDiscussionReadHistory(List<DocumentListParams> param)
        {
            ResultModel<List<DocumentDiscussionReadHistory>> res = new ResultModel<List<DocumentDiscussionReadHistory>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<List<DocumentListParams>>("api/DA/EPKRS/getEPKRSDocumentDiscussionReadHistory", param);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<DocumentDiscussionReadHistory>>>();

                    if (respBody.isSuccess)
                    {
                        res.Data = respBody.Data;
                        res.isSuccess = respBody.isSuccess;
                        res.ErrorCode = respBody.ErrorCode;
                        res.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(res);
                    }
                    else
                    {
                        res.Data = respBody.Data;
                        res.isSuccess = respBody.isSuccess;
                        res.ErrorCode = respBody.ErrorCode;
                        res.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(res);
                    }
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<DocumentDiscussionReadHistory>>>();

                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpGet("getEPKRSFileStream/{param}")]
        public async Task<IActionResult> getEPKRSFileStream(string param)
        {
            ResultModel<List<BPIBR.Models.MainModel.Stream.FileStream>> res = new ResultModel<List<BPIBR.Models.MainModel.Stream.FileStream>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<CaseAttachment>>>($"api/DA/EPKRS/getEPKRSCaseAttachment/{param}");

                if (result.isSuccess)
                {
                    res.Data = new();

                    result.Data.ForEach(x =>
                    {
                        res.Data.Add(new BPIBR.Models.MainModel.Stream.FileStream
                        {
                            type = "Attachment",
                            fileName = x.FilePath,
                            fileType = x.FileExtension,
                            fileSize = 0,
                            content = CommonLibrary.getFileStream(_uploadPath, "", x.FilePath, x.UploadDate, CommonLibrary.Base64Decode(param))
                        });
                    });

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getEPKRSGeneralStatistics/{param}")]
        public async Task<IActionResult> getEPKRSGeneralStatistics(string param)
        {
            ResultModel<List<EPKRSDocumentStatistics>> res = new ResultModel<List<EPKRSDocumentStatistics>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<EPKRSDocumentStatistics>>>($"api/DA/EPKRS/getEPKRSGeneralStatistics/{param}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpPost("getEPKRSGeneralIncidentAccidentStatistics")]
        public async Task<IActionResult> getEPKRSGeneralIncidentAccidentStatistics(QueryModel<string> param)
        {
            ResultModel<List<EPKRSDocumentStatistics>> res = new ResultModel<List<EPKRSDocumentStatistics>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<string>>("api/DA/EPKRS/getEPKRSGeneralIncidentAccidentStatistics", param);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<EPKRSDocumentStatistics>>>();

                    if (respBody.isSuccess)
                    {
                        res.Data = respBody.Data;
                        res.isSuccess = respBody.isSuccess;
                        res.ErrorCode = respBody.ErrorCode;
                        res.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(res);
                    }
                    else
                    {
                        res.Data = respBody.Data;
                        res.isSuccess = respBody.isSuccess;
                        res.ErrorCode = respBody.ErrorCode;
                        res.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(res);
                    }
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<EPKRSDocumentStatistics>>>();

                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getEPKRSItemCaseCategoryStatistics/{param}")]
        public async Task<IActionResult> getEPKRSItemCaseCategoryStatistics(string param)
        {
            ResultModel<List<EPKRSItemCaseCategoryStatistics>> res = new ResultModel<List<EPKRSItemCaseCategoryStatistics>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<EPKRSItemCaseCategoryStatistics>>>($"api/DA/EPKRS/getEPKRSItemCaseCategoryStatistics/{param}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getEPKRSTopLocationReportStatistics/{param}")]
        public async Task<IActionResult> getEPKRSTopLocationReportStatistics(string param)
        {
            ResultModel<List<EPKRSTopLocationReportStatistics>> res = new ResultModel<List<EPKRSTopLocationReportStatistics>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<EPKRSTopLocationReportStatistics>>>($"api/DA/EPKRS/getEPKRSTopLocationReportStatistics/{param}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getEPKRSItemCategoriesStatistics/{param}")]
        public async Task<IActionResult> getEPKRSItemCategoriesStatistics(string param)
        {
            ResultModel<List<EPKRSItemCaseItemCategoryStatistics>> res = new ResultModel<List<EPKRSItemCaseItemCategoryStatistics>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<EPKRSItemCaseItemCategoryStatistics>>>($"api/DA/EPKRS/getEPKRSItemCategoriesStatistics/{param}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpPost("getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmail")]
        public async Task<IActionResult> getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmail(QueryModel<string> param)
        {
            ResultModel<List<EPKRSIncidentAccidentRegionalStatistics>> res = new ResultModel<List<EPKRSIncidentAccidentRegionalStatistics>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<string>>("api/DA/EPKRS/getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmail", param);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<EPKRSIncidentAccidentRegionalStatistics>>>();

                    if (respBody.isSuccess)
                    {
                        res.Data = respBody.Data;
                        res.isSuccess = respBody.isSuccess;
                        res.ErrorCode = respBody.ErrorCode;
                        res.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(res);
                    }
                    else
                    {
                        res.Data = respBody.Data;
                        res.isSuccess = respBody.isSuccess;
                        res.ErrorCode = respBody.ErrorCode;
                        res.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(res);
                    }
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<EPKRSIncidentAccidentRegionalStatistics>>>();

                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpPost("getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPosition")]
        public async Task<IActionResult> getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPosition(QueryModel<string> param)
        {
            ResultModel<List<EPKRSIncidentAccidentInvolverStatisticsbyPosition>> res = new ResultModel<List<EPKRSIncidentAccidentInvolverStatisticsbyPosition>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<string>>("api/DA/EPKRS/getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPosition", param);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<EPKRSIncidentAccidentInvolverStatisticsbyPosition>>>();

                    if (respBody.isSuccess)
                    {
                        res.Data = respBody.Data;
                        res.isSuccess = respBody.isSuccess;
                        res.ErrorCode = respBody.ErrorCode;
                        res.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(res);
                    }
                    else
                    {
                        res.Data = respBody.Data;
                        res.isSuccess = respBody.isSuccess;
                        res.ErrorCode = respBody.ErrorCode;
                        res.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(res);
                    }
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<EPKRSIncidentAccidentInvolverStatisticsbyPosition>>>();

                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpPost("getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDept")]
        public async Task<IActionResult> getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDept(QueryModel<string> param)
        {
            ResultModel<List<EPKRSIncidentAccidentInvolverStatisticsbyDept>> res = new ResultModel<List<EPKRSIncidentAccidentInvolverStatisticsbyDept>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<string>>("api/DA/EPKRS/getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDept", param);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<EPKRSIncidentAccidentInvolverStatisticsbyDept>>>();

                    if (respBody.isSuccess)
                    {
                        res.Data = respBody.Data;
                        res.isSuccess = respBody.isSuccess;
                        res.ErrorCode = respBody.ErrorCode;
                        res.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(res);
                    }
                    else
                    {
                        res.Data = respBody.Data;
                        res.isSuccess = respBody.isSuccess;
                        res.ErrorCode = respBody.ErrorCode;
                        res.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(res);
                    }
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<EPKRSIncidentAccidentInvolverStatisticsbyDept>>>();

                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = null;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getEPKRSModuleNumberOfPage/{param}")]
        public async Task<IActionResult> getEPKRSModuleNumberOfPage(string param)
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/DA/EPKRS/getEPKRSModuleNumberOfPage/{param}");

                if (result.isSuccess)
                {
                    res.Data = result.Data;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = 0;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = 0;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        //
    }
}
