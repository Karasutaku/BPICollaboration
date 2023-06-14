using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.Company;
using BPIWebApplication.Shared.MainModel.Mailing;
using BPIWebApplication.Shared.MainModel.PettyCash;
using BPIWebApplication.Shared.PagesModel.PettyCash;
using Microsoft.AspNetCore.Mvc;

namespace BPIWebApplication.Server.Controllers
{
    [Route("api/endUser/PettyCash")]
    [ApiController]
    public class PettyCashController : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        private readonly int _maxFileSize;

        public PettyCashController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _configuration = config;
            _http.BaseAddress = new Uri(_configuration.GetValue<string>("ConnectionStrings:BpiFacade"));
            _maxFileSize = config.GetValue<int>("File:PettyCash:MaxUpload");
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
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/Facade/PettyCash/isAdvanceDataPresent/{AdvanceId}");

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
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/Facade/PettyCash/createID/{docType}");

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
            ResultModel<QueryModel<Advance>> res = new ResultModel<QueryModel<Advance>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<Advance>>($"api/Facade/PettyCash/createAdvance", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Advance>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Advance>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
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

        [HttpPost("createExpense")]
        public async Task<IActionResult> createAdvanceDataTable(ExpenseStream data)
        {
            ResultModel<QueryModel<Expense>> res = new ResultModel<QueryModel<Expense>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<ExpenseStream>($"api/Facade/PettyCash/createExpense", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Expense>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Expense>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
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

        [HttpPost("createReimburse")]
        public async Task<IActionResult> createReimburseDataTable(ReimburseStream data)
        {
            ResultModel<QueryModel<Reimburse>> res = new ResultModel<QueryModel<Reimburse>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<ReimburseStream>($"api/Facade/PettyCash/createReimburse", data);

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
                    res.ErrorMessage = "Fail Create Reimburse from createReimburse BR";

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

        // get

        [HttpPost("AdvanceSettlement")]
        public async Task<IActionResult> updateAdvanceDataSettlement(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<string>>($"api/Facade/PettyCash/AdvanceSettlement", data);

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
                    res.ErrorMessage = "Fail settle from AdvanceSettlement Facade";

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
                var result = await _http.PostAsJsonAsync<QueryModel<List<string>>>($"api/Facade/PettyCash/ExpenseSettlement", data);

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
                    res.ErrorMessage = "Fail settle from ExpenseSettlement Facade";

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
                var result = await _http.PostAsJsonAsync<QueryModel<string>>($"api/Facade/PettyCash/ReimburseSettlement", data);

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
                    res.ErrorMessage = "Fail settle from ReimburseSettlement Facade";

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
                var result = await _http.PostAsJsonAsync<QueryModel<string>>($"api/Facade/PettyCash/editDocumentStatus", data);

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
                    res.ErrorMessage = "Fail edit from editDocumentStatus Facade";

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
                var result = await _http.PostAsJsonAsync<List<ReimbursementMultiSelectStatusUpdate>>($"api/Facade/PettyCash/editMultiSelectDocumentStatus", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<List<ResultModel<ReimbursementMultiSelectStatusUpdate>>>();

                    res = respBody;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Add(new ResultModel<ReimbursementMultiSelectStatusUpdate>
                    {
                        Data = null,
                        isSuccess = false,
                        ErrorCode = "01",
                        ErrorMessage = "CONNECTION FAIL TO Facade"
                    });

                    actionResult = Ok(res);
                }

            }
            catch (Exception ex)
            {
                res.Add(new ResultModel<ReimbursementMultiSelectStatusUpdate>
                {
                    Data = null,
                    isSuccess = false,
                    ErrorCode = "99",
                    ErrorMessage = "ERROR : REASON " + ex.Message
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
                var result = await _http.PostAsJsonAsync<QueryModel<Reimburse>>($"api/Facade/PettyCash/editReimburseLine", data);

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
                    res.ErrorMessage = "Fail edit from editReimburseLine Facade";

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
            ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>> res = new ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>>($"api/Facade/PettyCash/getAttachFileStream/{Id}");

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

        [HttpGet("getAdvanceDatabyLocation/{locPage}")]
        public async Task<IActionResult> getAdvanceDatabyLocation(string locPage)
        {
            ResultModel<List<Advance>> res = new ResultModel<List<Advance>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Advance>>>($"api/Facade/PettyCash/getAdvanceDatabyLocation/{locPage}");

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<Advance>>>($"api/Facade/PettyCash/getAdvanceDatabyUser/{user}");

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<Expense>>>($"api/Facade/PettyCash/getExpenseDatabyLocation/{locPage}");

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<Reimburse>>>($"api/Facade/PettyCash/getReimburseDatabyLocation/{locPage}");

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<AdvanceLine>>>($"api/Facade/PettyCash/getAdvanceLinesbyID/{AdvanceId}");

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
                var result = await _http.GetFromJsonAsync<ResultModel<LocationBalanceDetails>>($"api/Facade/PettyCash/getPettyCashOutstandingAmountandLocBalanceDetails/{loc}");

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

        //[HttpGet("getLocationBudgetDetails/{loc}")]
        //public async Task<IActionResult> getLocationBudgetDetails(string loc)
        //{
        //    ResultModel<BalanceDetails> res = new ResultModel<BalanceDetails>();
        //    IActionResult actionResult = null;

        //    try
        //    {
        //        var result = await _http.GetFromJsonAsync<ResultModel<BalanceDetails>>($"api/Facade/PettyCash/getLocationBudgetDetails/{loc}");

        //        if (result.isSuccess)
        //        {
        //            res.Data = result.Data;

        //            res.isSuccess = result.isSuccess;
        //            res.ErrorCode = result.ErrorCode;
        //            res.ErrorMessage = result.ErrorMessage;

        //            actionResult = Ok(res);
        //        }
        //        else
        //        {
        //            res.Data = null;

        //            res.isSuccess = result.isSuccess;
        //            res.ErrorCode = result.ErrorCode;
        //            res.ErrorMessage = result.ErrorMessage;

        //            actionResult = Ok(res);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.Data = null;
        //        res.isSuccess = false;
        //        res.ErrorCode = "99";
        //        res.ErrorMessage = ex.Message;

        //        actionResult = BadRequest(res);
        //    }

        //    return actionResult;
        //}

        [HttpPost("getLedgerDataEntriesbyDate")]
        public async Task<IActionResult> getLedgerDataEntriesbyDate(List<ledgerParam> data)
        {
            ResultModel<BPIWebApplication.Shared.MainModel.Stream.FileStream> res = new ResultModel<BPIWebApplication.Shared.MainModel.Stream.FileStream>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<List<ledgerParam>>($"api/Facade/PettyCash/getLedgerDataEntriesbyDate", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<BPIWebApplication.Shared.MainModel.Stream.FileStream>>();

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
                    res.ErrorMessage = "Fail fetch from getLedgerDataEntriesbyDate Facade";

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
                var result = await _http.PostAsJsonAsync<QueryModel<BalanceDetails>>($"api/Facade/PettyCash/updateLocationBudget", data);

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
                    res.ErrorMessage = "Fail edit from updateLocationBudget Facade";

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
                var result = await _http.PostAsJsonAsync<QueryModel<CutoffDetails>>($"api/Facade/PettyCash/updateLocationCutoffDate", data);

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
                    res.ErrorMessage = "Fail edit from updateLocationCutoffDate Facade";

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
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/Facade/PettyCash/getModulePageSize/{Table}");

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<Account>>>($"api/Facade/PettyCash/getCoabyModule/{moduleName}");

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
            ResultModel<bool> res = new ResultModel<bool>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<Mailing>>($"api/Facade/PettyCash/getMailingDetails/{param}");

                if (result.isSuccess)
                {
                    res.Data = true;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = false;

                    res.isSuccess = result.isSuccess;
                    res.ErrorCode = result.ErrorCode;
                    res.ErrorMessage = result.ErrorMessage;

                    actionResult = Ok(res);
                }
            }
            catch (Exception ex)
            {
                res.Data = false;
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getPettyCashMaxSizeUpload")]
        public async Task<IActionResult> getPettyCashMaxSizeUpload()
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {

                res.Data = _maxFileSize;

                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);

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
