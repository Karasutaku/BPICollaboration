using BPIBR.Models.DbModel;
using BPIBR.Models.MainModel;
using BPIBR.Models.MainModel.Company;
using BPIBR.Models.MainModel.Mailing;
using BPIBR.Models.MainModel.PettyCash;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace BPIBR.Controllers
{
    [Route("api/BR/PettyCash")]
    [ApiController]
    public class PettyCashController : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        private readonly string _uploadPath, _archivePath;
        private readonly string _autoEmailUser, _autoEmailPass, _mailHost;
        private readonly int _mailPort;

        public PettyCashController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _configuration = config;
            _http.BaseAddress = new Uri(_configuration.GetValue<string>("BaseUri:BpiDA"));
            _uploadPath = _configuration.GetValue<string>("File:PettyCash:UploadPath");
            _archivePath = _configuration.GetValue<string>("File:PettyCash:ArchivePath");
            _autoEmailUser = _configuration.GetValue<string>("AutoEmailCreds:Email");
            _autoEmailPass = _configuration.GetValue<string>("AutoEmailCreds:Ticket");
            _mailHost = _configuration.GetValue<string>("AutoEmailCreds:Host");
            _mailPort = _configuration.GetValue<int>("AutoEmailCreds:Port");
        }
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        // save file
        internal async Task saveFiletoDirectory(string path, Byte[] content)
        {
            string dir = Path.GetDirectoryName(path);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            await using FileStream fs = new(path, FileMode.Create);
            Stream stream = new MemoryStream(content);
            await stream.CopyToAsync(fs);
        }

        internal Byte[] getFileStream(string id, string filename)
        {
            string type = string.Empty;

            string path = Path.Combine(_uploadPath, "ExpenseAttach", Convert.ToInt32(id.Substring(1, 4)).ToString(), Convert.ToInt32(id.Substring(5, 2)).ToString(), Convert.ToInt32(id.Substring(7, 2)).ToString(), id, Path.GetFileName(filename));

            return System.IO.File.ReadAllBytes(path);
        }

        internal bool deleteFilefromDirectory(string id, string filename)
        {
            string path = Path.Combine(_uploadPath, "ExpenseAttach", Convert.ToInt32(id.Substring(1, 4)).ToString(), Convert.ToInt32(id.Substring(5, 2)).ToString(), Convert.ToInt32(id.Substring(7, 2)).ToString(), id, Path.GetFileName(filename));

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);

                return true;
            }

            return false;
        }

        // http

        // is

        [HttpGet("isAdvanceDataPresent/{AdvanceId}")]
        public async Task<IActionResult> isAdvanceDataPresentInTable(string AdvanceId)
        {
            ResultModel<string> res = new ResultModel<string>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/DA/PettyCash/isAdvanceDataPresent/{AdvanceId}");

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
                res.Data = "";
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        // create

        [HttpGet("createID/{docType}")]
        public async Task<IActionResult> createDocumentID(string docType)
        {
            ResultModel<string> res = new ResultModel<string>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/DA/PettyCash/createID/{docType}");

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
                res.Data = "";
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpPost("createAdvance")]
        public async Task<IActionResult> createAdvanceDataTable(QueryModel<Advance> data)
        {
            ResultModel<QueryModel<Advance>> finRes = new ResultModel<QueryModel<Advance>>();
            IActionResult actionResult = null;

            try
            {
                var result1 = await _http.PostAsJsonAsync<QueryModel<Advance>>($"api/DA/PettyCash/createAdvanceData", data);

                if (result1.IsSuccessStatusCode)
                {
                    var respBody = await result1.Content.ReadFromJsonAsync<ResultModel<QueryModel<Advance>>>();

                    if (respBody.isSuccess)
                    {
                        finRes.Data = respBody.Data;

                        finRes.isSuccess = respBody.isSuccess;
                        finRes.ErrorCode = respBody.ErrorCode;
                        finRes.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(finRes);
                    }
                    else
                    {
                        finRes.Data = respBody.Data;

                        finRes.isSuccess = respBody.isSuccess;
                        finRes.ErrorCode = respBody.ErrorCode;
                        finRes.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(finRes);
                    }
                }
                else
                {
                    var respBody = await result1.Content.ReadFromJsonAsync<ResultModel<QueryModel<Advance>>>();

                    finRes.Data = respBody.Data;
                    finRes.isSuccess = respBody.isSuccess;
                    finRes.ErrorCode = respBody.ErrorCode;
                    finRes.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(finRes);
                }
            }
            catch (Exception ex)
            {
                finRes.Data = null;
                finRes.isSuccess = false;
                finRes.ErrorCode = "99";
                finRes.ErrorMessage = ex.Message;

                actionResult = BadRequest(finRes);
            }
            return actionResult;
        }

        [HttpPost("createExpense")]
        public async Task<IActionResult> createExpenseDataTable(ExpenseStream data)
        {
            ResultModel<QueryModel<Expense>> finRes = new ResultModel<QueryModel<Expense>>();
            IActionResult actionResult = null;

            try
            {
                var result1 = await _http.PostAsJsonAsync<QueryModel<Expense>>($"api/DA/PettyCash/createExpenseData", data.expenseDetails);

                if (result1.IsSuccessStatusCode)
                {
                    var respBody = await result1.Content.ReadFromJsonAsync<ResultModel<QueryModel<Expense>>>();

                    if (respBody.isSuccess)
                    {
                        foreach (var file in data.files)
                        {
                            string path = Path.Combine(_uploadPath, "ExpenseAttach", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), data.expenseDetails.Data.ExpenseID, file.fileName);

                            await saveFiletoDirectory(path, file.content);
                        }

                        finRes.Data = respBody.Data;

                        finRes.isSuccess = respBody.isSuccess;
                        finRes.ErrorCode = respBody.ErrorCode;
                        finRes.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(finRes);

                    }
                    else
                    {
                        finRes.Data = respBody.Data;

                        finRes.isSuccess = respBody.isSuccess;
                        finRes.ErrorCode = respBody.ErrorCode;
                        finRes.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(finRes);
                    }
                }
                else
                {
                    var respBody = await result1.Content.ReadFromJsonAsync<ResultModel<QueryModel<Expense>>>();

                    finRes.Data = null;

                    finRes.isSuccess = respBody.isSuccess;
                    finRes.ErrorCode = respBody.ErrorCode;
                    finRes.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(finRes);
                }
            }
            catch (Exception ex)
            {
                finRes.Data = null;
                finRes.isSuccess = false;
                finRes.ErrorCode = "99";
                finRes.ErrorMessage = ex.Message;

                actionResult = BadRequest(finRes);
            }
            return actionResult;
        }

        [HttpPost("createReimburse")]
        public async Task<IActionResult> createReimburseDataTable(ReimburseStream data)
        {
            ResultModel<QueryModel<Reimburse>> finRes = new ResultModel<QueryModel<Reimburse>>();
            IActionResult actionResult = null;

            try
            {
                var result1 = await _http.PostAsJsonAsync<QueryModel<Reimburse>>($"api/DA/PettyCash/createReimburseData", data.reimburseDetails);

                if (result1.IsSuccessStatusCode)
                {
                    var respBody = await result1.Content.ReadFromJsonAsync<ResultModel<QueryModel<Reimburse>>>();

                    if (respBody.isSuccess)
                    {
                        finRes.Data = respBody.Data;
                        finRes.isSuccess = respBody.isSuccess;
                        finRes.ErrorCode = respBody.ErrorCode;
                        finRes.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(finRes);

                        //QueryModel<List<BPIBR.Models.MainModel.Stream.FileStream>> files = new();
                        //files.Data = new();

                        //files.Data = data.files;
                        //files.userEmail = data.reimburseDetails.userEmail;
                        //files.userAction = data.reimburseDetails.userAction;
                        //files.userActionDate = data.reimburseDetails.userActionDate;

                        //var result2 = await _http.PostAsJsonAsync<QueryModel<List<BPIBR.Models.MainModel.Stream.FileStream>>>($"api/DA/PettyCash/createReimburseAttachLineData", files);

                        //if (result2.IsSuccessStatusCode)
                        //{
                        //    var respBody2 = await result2.Content.ReadFromJsonAsync<ResultModel<QueryModel<List<BPIBR.Models.MainModel.Stream.FileStream>>>>();

                        //    if (respBody2.isSuccess)
                        //    {
                        //        finRes.Data = respBody.Data;

                        //        finRes.isSuccess = respBody.isSuccess;
                        //        finRes.ErrorCode = respBody.ErrorCode;
                        //        finRes.ErrorMessage = respBody.ErrorMessage;

                        //        actionResult = Ok(finRes);
                        //    }
                        //    else
                        //    {
                        //        finRes.Data = respBody.Data;

                        //        finRes.isSuccess = respBody.isSuccess;
                        //        finRes.ErrorCode = respBody.ErrorCode;
                        //        finRes.ErrorMessage = respBody.ErrorMessage;

                        //        actionResult = Ok(finRes);
                        //    }
                        //}
                        //else
                        //{
                        //    finRes.Data = null;

                        //    finRes.isSuccess = false;
                        //    finRes.ErrorCode = "01";
                        //    finRes.ErrorMessage = "createReimburseAttachLineData Connect to DA API Fail";

                        //    actionResult = Ok(finRes);
                        //}
                    }
                    else
                    {
                        finRes.Data = respBody.Data;
                        finRes.isSuccess = respBody.isSuccess;
                        finRes.ErrorCode = respBody.ErrorCode;
                        finRes.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(finRes);
                    }
                }
                else
                {
                    var respBody = await result1.Content.ReadFromJsonAsync<ResultModel<QueryModel<Reimburse>>>();

                    finRes.Data = null;
                    finRes.isSuccess = respBody.isSuccess;
                    finRes.ErrorCode = respBody.ErrorCode;
                    finRes.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(finRes);
                }

            }
            catch (Exception ex)
            {
                finRes.Data = null;
                finRes.isSuccess = false;
                finRes.ErrorCode = "99";
                finRes.ErrorMessage = ex.Message;

                actionResult = BadRequest(finRes);
            }
            return actionResult;
        }

        // get

        [HttpPost("AdvanceSettlement")]
        public async Task<IActionResult> updateAdvanceDataSettlement(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<string>>($"api/DA/PettyCash/AdvanceSettlement", data);

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
                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail settle from AdvanceSettlement DA";

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

        [HttpPost("ExpenseSettlement")]
        public async Task<IActionResult> updateExpenseDataSettlement(QueryModel<List<string>> data)
        {
            ResultModel<QueryModel<List<string>>> res = new ResultModel<QueryModel<List<string>>>();
            IActionResult actionResult = null;

            try
            {

                // settling
                var result = await _http.PostAsJsonAsync<QueryModel<List<string>>>($"api/DA/PettyCash/ExpenseSettlement", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<List<string>>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail settle from ExpenseSettlement DA";

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

        [HttpPost("ReimburseSettlement")]
        public async Task<IActionResult> updateReimburseDataSettlement(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<string>>($"api/DA/PettyCash/ReimburseSettlement", data);

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
                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail settle from ReimburseSettlement DA";

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

        [HttpPost("editDocumentStatus")]
        public async Task<IActionResult> editDocumentStatusData(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            IActionResult actionResult = null;

            try
            {
                if (data.Data.Split("!_!")[1].Contains("E") && data.Data.Split("!_!")[4].Equals("Open") && data.Data.Split("!_!")[2].Equals("Rejected"))
                {
                    string temp = data.Data.Split("!_!")[1] + "!_!MASTER";

                    var result1 = await _http.GetFromJsonAsync<ResultModel<List<AttachmentLine>>>($"api/DA/PettyCash/getAttachmentLines/{Base64Encode(temp)}");

                    if (result1.isSuccess && result1.Data != null)
                    {
                        foreach (var dt in result1.Data)
                        {
                            deleteFilefromDirectory(data.Data.Split("!_!")[1], dt.PathFile);
                        }
                    }
                }

                var result = await _http.PostAsJsonAsync<QueryModel<string>>($"api/DA/PettyCash/editDocumentStatus", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<string>>>();

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
                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail edit from editDocumentStatus DA";

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

        [HttpPost("editMultiSelectDocumentStatus")]
        public async Task<IActionResult> editMultiSelectDocumentStatus(List<ReimbursementMultiSelectStatusUpdate> data)
        {
            List<ResultModel<ReimbursementMultiSelectStatusUpdate>> res = new List<ResultModel<ReimbursementMultiSelectStatusUpdate>>();
            IActionResult actionResult = null;

            try
            {

                await Parallel.ForEachAsync(data, new ParallelOptions { MaxDegreeOfParallelism = 4 }, async (dt, i) =>
                {
                    ResultModel<ReimbursementMultiSelectStatusUpdate> taskResult = new();
                    QueryModel<string> param = new();

                    if (dt.statusValue.Equals("Released"))
                    {
                        param.Data = "Reimburse!_!" + dt.documentID + "!_!" + dt.statusValue + "!_!";
                        param.userEmail = dt.approver;
                        param.userAction = "U";
                        param.userActionDate = DateTime.Now;

                        var result = await _http.PostAsJsonAsync<QueryModel<string>>($"api/DA/PettyCash/editDocumentStatus", param);

                        if (result.IsSuccessStatusCode)
                        {
                            var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<string>>>();

                            taskResult.Data = dt;
                            taskResult.isSuccess = respBody.isSuccess;
                            taskResult.ErrorCode = respBody.ErrorCode;
                            taskResult.ErrorMessage = respBody.ErrorMessage;

                            res.Add(taskResult);

                            string emailParam = "PettyCash!_!StatusRelease!_!" + dt.documentLocation + "!_!" + dt.approver + "!_!" + dt.documentID;
                            await getMailingDetailsData(Base64Encode(emailParam));
                        }
                        else
                        {
                            taskResult.Data = dt;
                            taskResult.isSuccess = result.IsSuccessStatusCode;
                            taskResult.ErrorCode = "01";
                            taskResult.ErrorMessage = "Fail Update Document Status DA";

                            res.Add(taskResult);
                        }
                    }

                });

                actionResult = Ok(res);
            }
            catch (Exception ex)
            {
                res.Add(new ResultModel<ReimbursementMultiSelectStatusUpdate>
                {
                    Data = null,
                    isSuccess = false,
                    ErrorCode = "99",
                    ErrorMessage = "PARALLEL TASK FORCED STOP : REASON " + ex.Message
                });

                actionResult = BadRequest(res);
            }
            return actionResult;
        }

        [HttpPost("editReimburseLine")]
        public async Task<IActionResult> editReimburseLineData(QueryModel<Reimburse> data)
        {
            ResultModel<QueryModel<Reimburse>> res = new ResultModel<QueryModel<Reimburse>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<Reimburse>>($"api/DA/PettyCash/editReimburseLine", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Reimburse>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail edit from editReimburseLine DA";

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

        [HttpGet("getAttachFileStream/{Id}")]
        public async Task<IActionResult> getAttachFileStream(string Id)
        {
            ResultModel<List<BPIBR.Models.MainModel.Stream.FileStream>> res = new ResultModel<List<BPIBR.Models.MainModel.Stream.FileStream>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<AttachmentLine>>>($"api/DA/PettyCash/getAttachmentLines/{Id}");

                if (result.isSuccess && result.Data != null)
                {
                    res.Data = new();

                    foreach (var dt in result.Data)
                    {
                        BPIBR.Models.MainModel.Stream.FileStream temp = new BPIBR.Models.MainModel.Stream.FileStream();

                        temp.type = dt.ExpenseID;
                        temp.fileName = dt.PathFile;
                        temp.fileType = "";
                        temp.fileSize = 0;
                        temp.content = getFileStream(dt.ExpenseID, dt.PathFile);

                        res.Data.Add(temp);
                    }

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

        [HttpGet("getAdvanceDatabyLocation/{locPage}")]
        public async Task<IActionResult> getAdvanceDatabyLocation(string locPage)
        {
            ResultModel<List<Advance>> res = new ResultModel<List<Advance>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Advance>>>($"api/DA/PettyCash/getAdvanceDatabyLocation/{locPage}");

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

        [HttpGet("getAdvanceDatabyUser/{user}")]
        public async Task<IActionResult> getAdvanceDatabyUser(string user)
        {
            ResultModel<List<Advance>> res = new ResultModel<List<Advance>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Advance>>>($"api/DA/PettyCash/getAdvanceDatabyUser/{user}");

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

        [HttpGet("getExpenseDatabyLocation/{locPage}")]
        public async Task<IActionResult> getExpenseDatabyLocation(string locPage)
        {
            ResultModel<List<Expense>> res = new ResultModel<List<Expense>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Expense>>>($"api/DA/PettyCash/getExpenseDatabyLocation/{locPage}");

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

        [HttpGet("getReimburseDatabyLocation/{locPage}")]
        public async Task<IActionResult> getReimburseDatabyLocation(string locPage)
        {
            ResultModel<List<Reimburse>> res = new ResultModel<List<Reimburse>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Reimburse>>>($"api/DA/PettyCash/getReimburseDatabyLocation/{locPage}");

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

        [HttpGet("getAdvanceLinesbyID/{AdvanceId}")]
        public async Task<IActionResult> getAdvanceLinesbyID(string AdvanceId)
        {
            ResultModel<List<AdvanceLine>> res = new ResultModel<List<AdvanceLine>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<AdvanceLine>>>($"api/DA/PettyCash/getAdvanceLinesbyID/{AdvanceId}");

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

        [HttpGet("getPettyCashOutstandingAmountandLocBalanceDetails/{loc}")]
        public async Task<IActionResult> getPettyCashOutstandingAmount(string loc)
        {
            ResultModel<LocationBalanceDetails> res = new ResultModel<LocationBalanceDetails>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<LocationBalanceDetails>>($"api/DA/PettyCash/getPettyCashOutstandingAmountandLocBalanceDetails/{loc}");

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

        [HttpPost("getLedgerDataEntriesbyDate")]
        public async Task<IActionResult> getLedgerDataEntriesbyDate(List<ledgerParam> data)
        {
            ResultModel<BPIBR.Models.MainModel.Stream.FileStream> res = new ResultModel<BPIBR.Models.MainModel.Stream.FileStream>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<List<ledgerParam>>($"api/DA/PettyCash/getLedgerDataEntriesbyDate", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<PettyCashLedger>>>();

                    res.Data = new();

                    using (var workbook = new XLWorkbook())
                    {
                        int headerFontSize = 10;
                        workbook.Properties.Author = "BPI App Report";
                        workbook.Properties.Title = "PettyCash Transaction";

                        var worksheet = workbook.AddWorksheet("Report");

                        worksheet.Cell("A1").Value = "PT. CATUR MITRA SEJATI SENTOSA";
                        worksheet.Cell("A1").Style.Font.SetBold(true);
                        worksheet.Cell("A1").Style.Font.SetFontSize(headerFontSize);

                        worksheet.Cell("A2").Value = "TRANSACTION LIST";
                        worksheet.Cell("A2").Style.Font.SetBold(true);
                        worksheet.Cell("A2").Style.Font.SetFontSize(headerFontSize);

                        int startLine = 4;

                        foreach (var param in data)
                        {
                            worksheet.Cell($"A{startLine}").Value = $"LOCATION : {param.locationID}";
                            worksheet.Cell($"A{startLine}").Style.Font.SetBold(true);
                            worksheet.Cell($"A{startLine}").Style.Font.SetFontSize(headerFontSize);

                            startLine++;

                            worksheet.Cell($"A{startLine}").Value = $"START DATE : {param.startDate.ToString("dd MMMM yyyy")}";
                            worksheet.Cell($"A{startLine}").Style.Font.SetBold(true);
                            worksheet.Cell($"A{startLine}").Style.Font.SetFontSize(headerFontSize);

                            startLine++;

                            worksheet.Cell($"A{startLine}").Value = $"END DATE : {param.endDate.ToString("dd MMMM  yyyy")}";
                            worksheet.Cell($"A{startLine}").Style.Font.SetBold(true);
                            worksheet.Cell($"A{startLine}").Style.Font.SetFontSize(headerFontSize);

                            startLine++;
                            startLine++;

                            worksheet.Cell($"A{startLine}").Value = "DATA";
                            worksheet.Cell($"A{startLine}").Style.Font.SetBold(true);
                            worksheet.Cell($"A{startLine}").Style.Font.SetFontSize(headerFontSize);

                            startLine++;

                            worksheet.Cell($"A{startLine}").Value = "TRANSACTION DATE";//
                            worksheet.Cell($"A{startLine}").Style.Font.SetBold(true);
                            worksheet.Cell($"A{startLine}").Style.Font.SetFontSize(headerFontSize);
                            worksheet.Cell($"B{startLine}").Value = "TRANSACTION TYPE";//
                            worksheet.Cell($"B{startLine}").Style.Font.SetBold(true);
                            worksheet.Cell($"B{startLine}").Style.Font.SetFontSize(headerFontSize);
                            worksheet.Cell($"C{startLine}").Value = "DOCUMENT ID";//
                            worksheet.Cell($"C{startLine}").Style.Font.SetBold(true);
                            worksheet.Cell($"C{startLine}").Style.Font.SetFontSize(headerFontSize);
                            worksheet.Cell($"D{startLine}").Value = "LOCATION";//
                            worksheet.Cell($"D{startLine}").Style.Font.SetBold(true);
                            worksheet.Cell($"D{startLine}").Style.Font.SetFontSize(headerFontSize);
                            worksheet.Cell($"E{startLine}").Value = "AMOUNT";//
                            worksheet.Cell($"E{startLine}").Style.Font.SetBold(true);
                            worksheet.Cell($"E{startLine}").Style.Font.SetFontSize(headerFontSize);
                            worksheet.Cell($"F{startLine}").Value = "ACTOR";//
                            worksheet.Cell($"F{startLine}").Style.Font.SetBold(true);
                            worksheet.Cell($"F{startLine}").Style.Font.SetFontSize(headerFontSize);
                            worksheet.Cell($"G{startLine}").Value = "EXT. DOCUMENT";//
                            worksheet.Cell($"G{startLine}").Style.Font.SetBold(true);
                            worksheet.Cell($"G{startLine}").Style.Font.SetFontSize(headerFontSize);
                            worksheet.Cell($"H{startLine}").Value = "APPLICANT";//
                            worksheet.Cell($"H{startLine}").Style.Font.SetBold(true);
                            worksheet.Cell($"H{startLine}").Style.Font.SetFontSize(headerFontSize);
                            worksheet.Cell($"I{startLine}").Value = "DOCUMENT DATE";//
                            worksheet.Cell($"I{startLine}").Style.Font.SetBold(true);
                            worksheet.Cell($"I{startLine}").Style.Font.SetFontSize(headerFontSize);
                            worksheet.Cell($"J{startLine}").Value = "DEPARTMENT";//
                            worksheet.Cell($"J{startLine}").Style.Font.SetBold(true);
                            worksheet.Cell($"J{startLine}").Style.Font.SetFontSize(headerFontSize);
                            worksheet.Cell($"K{startLine}").Value = "NOTE";//
                            worksheet.Cell($"K{startLine}").Style.Font.SetBold(true);
                            worksheet.Cell($"K{startLine}").Style.Font.SetFontSize(headerFontSize);
                            worksheet.Cell($"L{startLine}").Value = "NIK";//
                            worksheet.Cell($"L{startLine}").Style.Font.SetBold(true);
                            worksheet.Cell($"L{startLine}").Style.Font.SetFontSize(headerFontSize);
                            worksheet.Cell($"M{startLine}").Value = "TYPE";//
                            worksheet.Cell($"M{startLine}").Style.Font.SetBold(true);
                            worksheet.Cell($"M{startLine}").Style.Font.SetFontSize(headerFontSize);
                            worksheet.Cell($"N{startLine}").Value = "BANK ACCOUNT";//
                            worksheet.Cell($"N{startLine}").Style.Font.SetBold(true);
                            worksheet.Cell($"N{startLine}").Style.Font.SetFontSize(headerFontSize);
                            worksheet.Cell($"O{startLine}").Value = "STATUS";//
                            worksheet.Cell($"O{startLine}").Style.Font.SetBold(true);
                            worksheet.Cell($"O{startLine}").Style.Font.SetFontSize(headerFontSize);

                            startLine++;

                            foreach (var dat in respBody.Data)
                            {
                                worksheet.Cell($"A{startLine}").Value = dat.TransactionDate;//
                                worksheet.Cell($"B{startLine}").Value = dat.TransactionType;//
                                worksheet.Cell($"C{startLine}").Value = dat.DocumentID;//
                                worksheet.Cell($"D{startLine}").Value = dat.LocationID;//
                                worksheet.Cell($"E{startLine}").Value = dat.Amount;//
                                worksheet.Cell($"F{startLine}").Value = dat.Actor;//
                                worksheet.Cell($"G{startLine}").Value = dat.ExternalDocument;//
                                worksheet.Cell($"H{startLine}").Value = dat.Applicant;//
                                worksheet.Cell($"I{startLine}").Value = dat.DocumentDate;//
                                worksheet.Cell($"J{startLine}").Value = dat.DepartmentID;//
                                worksheet.Cell($"K{startLine}").Value = dat.Note;//
                                worksheet.Cell($"L{startLine}").Value = dat.NIK;//
                                worksheet.Cell($"M{startLine}").Value = dat.Type;//
                                worksheet.Cell($"N{startLine}").Value = dat.BankAccount;//
                                worksheet.Cell($"O{startLine}").Value = dat.Status;//

                                startLine++;
                            }

                            startLine++;
                            startLine++;
                            startLine++;
                        }

                        MemoryStream ms = new MemoryStream();
                        workbook.SaveAs(ms);

                        res.Data.content = ms.ToArray();
                        res.Data.type = "Report";
                        res.Data.fileSize = 0;
                        res.Data.fileName = "Export Ledger Data.xlsx";
                        res.Data.fileType = ".xlsx";

                    }

                    //res.Data = respBody.Data;
                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail fetch from getLedgerDataEntriesbyDate DA";

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

        [HttpPost("updateLocationBudget")]
        public async Task<IActionResult> updateLocationBudget(QueryModel<BalanceDetails> data)
        {
            ResultModel<QueryModel<BalanceDetails>> res = new ResultModel<QueryModel<BalanceDetails>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<BalanceDetails>>($"api/DA/PettyCash/updateLocationBudget", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<BalanceDetails>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail edit from updateLocationBudget DA";

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

        [HttpPost("updateLocationCutoffDate")]
        public async Task<IActionResult> updateLocationCutoffDate(QueryModel<CutoffDetails> data)
        {
            ResultModel<QueryModel<CutoffDetails>> res = new ResultModel<QueryModel<CutoffDetails>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<CutoffDetails>>($"api/DA/PettyCash/updateLocationCutoffDate", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<CutoffDetails>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail edit from updateLocationCutoffDate DA";

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

        [HttpGet("getModulePageSize/{Table}")]
        public async Task<IActionResult> getModulePageSize(string Table)
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/DA/PettyCash/getModulePageSize/{Table}");

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

        [HttpGet("getCoabyModule/{moduleName}")]
        public async Task<IActionResult> getCoabyModuleData(string moduleName)
        {
            ResultModel<List<Account>> res = new ResultModel<List<Account>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Account>>>($"api/DA/PettyCash/getCoabyModule/{moduleName}");

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

        [HttpGet("getMailingDetails/{param}")]
        public async Task<IActionResult> getMailingDetailsData(string param)
        {
            ResultModel<Mailing> res = new ResultModel<Mailing>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<Mailing>>($"api/DA/PettyCash/getMailingDetails/{param}");

                if (result.isSuccess)
                {
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        MailMessage msg = new MailMessage();

                        string act = Base64Decode(param).Split("!_!")[1];
                        string user = Base64Decode(param).Split("!_!")[3];
                        string id = Base64Decode(param).Split("!_!")[4];

                        msg.From = new MailAddress(_autoEmailUser);

                        if (act.Contains("StatusReject") || act.Contains("DirectEmail"))
                        {
                            string requestor = Base64Decode(param).Split("!_!")[5];
                            msg.To.Add(requestor);
                        }
                        else
                        {
                            msg.To.Add(result.Data.Receiver);
                        }

                        msg.Subject = string.Format(result.Data.MailSubject);

                        string body = result.Data.MailBeginningBody + "<br/><br/>" + string.Format(result.Data.MailMainBody, id, user) + "<br/><br/>" + result.Data.MailFooter + "<br/><br/>" + result.Data.MailNote;

                        msg.Body = body;
                        msg.IsBodyHtml = true;
                        smtp.Credentials = new NetworkCredential(_autoEmailUser, _autoEmailPass);
                        smtp.Port = _mailPort;
                        smtp.Host = _mailHost;
                        smtp.EnableSsl = true;

                        smtp.Send(msg);
                    }

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

        //
    }
}
