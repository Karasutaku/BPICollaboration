using BPIFacade.Models.DbModel;
using BPIFacade.Models.MainModel;
using BPIFacade.Models.MainModel.Procedure;
using BPIFacade.Models.MainModel.Procedure.Filter;
using BPIFacade.Models.MainModel.Procedure.Report;
using Microsoft.AspNetCore.Mvc;

namespace BPIFacade.Controllers
{
    [Route("api/Facade/Procedure")]
    [ApiController]
    public class ProcedureController : Controller
    {
        private readonly HttpClient _http;

        public ProcedureController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _http.BaseAddress = new Uri(config.GetValue<string>("BaseUri:BpiBR"));
        }


        [HttpGet("getAllProcedureData")]
        public async Task<IActionResult> getAllProcedureDataTable()
        {
            ResultModel<List<Procedure>> res = new ResultModel<List<Procedure>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Procedure>>>("api/BR/Procedure/getAllProcedureData");

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

        [HttpGet("getDepartmentProcedureData")]
        public async Task<IActionResult> getAllDepartmentProcedureDataTable()
        {
            ResultModel<List<DepartmentProcedure>> res = new ResultModel<List<DepartmentProcedure>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<DepartmentProcedure>>>("api/BR/Procedure/getDepartmentProcedureData");

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

        [HttpGet("getDepartmentProcedureDatawithPaging/{param}")]
        public async Task<IActionResult> getAllDepartmentProcedureDataTablebyPaging(string param)
        {
            ResultModel<List<DepartmentProcedure>> res = new ResultModel<List<DepartmentProcedure>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<DepartmentProcedure>>>($"api/BR/Procedure/getDepartmentProcedureDatawithPaging/{param}");

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

        [HttpPost("getDepartmentProcedureDatawithFilterbyPaging")]
        public async Task<IActionResult> getAllDepartmentProcedureDataTablewithFilterbyPaging(DashboardFilter data)
        {
            ResultModel<List<DepartmentProcedure>> res = new ResultModel<List<DepartmentProcedure>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<DashboardFilter>($"api/BR/Procedure/getDepartmentProcedureDatawithFilterbyPaging", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<DepartmentProcedure>>>();

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
                    res.ErrorMessage = "Fail Fetch data from getDepartmentProcedureDatawithFilterbyPaging BR";

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

        [HttpGet("getAllHistoryAccessDatawithPaging/{pageNo}")]
        public async Task<IActionResult> getAllHistoryAccessDataTablewithPaging(int pageNo)
        {
            ResultModel<List<HistoryAccess>> res = new ResultModel<List<HistoryAccess>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<HistoryAccess>>>($"api/BR/Procedure/getAllHistoryAccessDatawithPaging/{pageNo}");

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

        [HttpPost("getAllHistoryAccessDatabyFilterwithPaging")]
        public async Task<IActionResult> getAllHistoryAccessDataTablebyFilterwithPaging(AccessHistoryFilter data)
        {
            ResultModel<List<HistoryAccess>> res = new ResultModel<List<HistoryAccess>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<AccessHistoryFilter>($"api/BR/Procedure/getAllHistoryAccessDatabyFilterwithPaging", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<HistoryAccess>>>();

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
                    res.ErrorMessage = "Fail Fetch data from getAllHistoryAccessDatabyFilterwithPaging BR";

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

        [HttpPost("getAllHistoryAccessDataReportbyFilter")]
        public async Task<IActionResult> getAllHistoryAccessDataTableReportbyFilter(AccessHistoryReport data)
        {
            ResultModel<List<HistoryAccess>> res = new ResultModel<List<HistoryAccess>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<AccessHistoryReport>($"api/BR/Procedure/getAllHistoryAccessDataReportbyFilter", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<List<HistoryAccess>>>();

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
                    res.ErrorMessage = "Fail Fetch data from getAllHistoryAccessDataReportbyFilter BR";

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

        [HttpGet("getDepartmentProcedureNumberofPage/{param}")]
        public async Task<IActionResult> getDepartmentProcedureNumberofPageData(string param)
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/BR/Procedure/getDepartmentProcedureNumberofPage/{param}");

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

        [HttpPost("getDepartmentProcedurewithFilterNumberofPage")]
        public async Task<IActionResult> getDepartmentProcedurewithFilterNumberofPageData(DashboardFilter data)
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<DashboardFilter>($"api/BR/Procedure/getDepartmentProcedurewithFilterNumberofPage", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<int>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = 0;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail Fetch data from getDepartmentProcedurewithFilterNumberofPage BR";

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

        [HttpGet("getHistoryAccessNumberofPage")]
        public async Task<IActionResult> getHistoryAccessNumberofPageData()
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>("api/BR/Procedure/getHistoryAccessNumberofPage");

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

        [HttpPost("getHistoryAccessbyFilterwithPagingNumberofPage")]
        public async Task<IActionResult> getHistoryAccessNumberofPageData(AccessHistoryFilter data)
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<AccessHistoryFilter>($"api/BR/Procedure/getHistoryAccessbyFilterwithPagingNumberofPage", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<int>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = 0;

                    res.isSuccess = result.IsSuccessStatusCode;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail Fetch data from getHistoryAccessbyFilterwithPagingNumberofPage BR";

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

        [HttpGet("getFile/{path}")]
        public async Task<IActionResult> getFiletoDownload(string path)
        {
            ResultModel<BPIFacade.Models.MainModel.Stream.FileStream> res = new ResultModel<BPIFacade.Models.MainModel.Stream.FileStream>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<BPIFacade.Models.MainModel.Stream.FileStream>>($"api/BR/Procedure/getFile/{path}");

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


        [HttpGet("isProcedureDataPresent/{ProcNo}")]
        public async Task<IActionResult> isProcedureDataPresentInTable(string ProcNo)
        {
            ResultModel<string> res = new ResultModel<string>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/BR/Procedure/isProcedureDataPresent/{ProcNo}");

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

        // post

        [HttpPost("createProcedureData")]
        public async Task<IActionResult> createProcedureDataTableandFileSave(ProcedureStream data)
        {
            ResultModel<ProcedureStream> res = new ResultModel<ProcedureStream>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<ProcedureStream>($"api/BR/Procedure/createProcedureData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<ProcedureStream>>();

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
                    res.ErrorMessage = "Fail Fetch data from createProcedureData BR";

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


        [HttpPost("createDepartmentProcedureData")]
        public async Task<IActionResult> createDepartmentProcedureDataTable(QueryModel<List<DepartmentProcedure>> data)
        {
            ResultModel<QueryModel<List<DepartmentProcedure>>> res = new ResultModel<QueryModel<List<DepartmentProcedure>>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<List<DepartmentProcedure>>>($"api/BR/Procedure/createDepartmentProcedureData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<List<DepartmentProcedure>>>>();

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
                    res.ErrorMessage = "Fail Fetch data from createDepartmentProcedureData BR";

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


        [HttpPost("createHistoryAccessData")]
        public async Task<IActionResult> createHistoryAccessDataTable(QueryModel<HistoryAccess> data)
        {
            ResultModel<QueryModel<HistoryAccess>> res = new ResultModel<QueryModel<HistoryAccess>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<HistoryAccess>>($"api/BR/Procedure/createHistoryAccessData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<HistoryAccess>>>();

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
                    res.ErrorMessage = "Fail Fetch data from createHistoryAccessData BR";

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

        [HttpPost("editProcedureData")]
        public async Task<IActionResult> editProcedureDataTable(ProcedureStream data)
        {
            ResultModel<ProcedureStream> res = new ResultModel<ProcedureStream>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<ProcedureStream>($"api/BR/Procedure/editProcedureData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<ProcedureStream>>();

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
                    res.ErrorMessage = "Fail Fetch data from editProcedureData BR";

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


        [HttpPost("deleteDepartmentProcedureData")]
        public async Task<IActionResult> deleteDepartmentProcedureDataTablebyProcNoDeptID(QueryModel<List<DepartmentProcedure>> data)
        {
            ResultModel<QueryModel<List<DepartmentProcedure>>> res = new ResultModel<QueryModel<List<DepartmentProcedure>>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<List<DepartmentProcedure>>>($"api/BR/Procedure/deleteDepartmentProcedureData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<List<DepartmentProcedure>>>>();

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
                    res.ErrorMessage = "Fail Fetch data from deleteDepartmentProcedureData BR";

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

        // OTHER

        [HttpGet("getProcedureMaxSizeUpload")]
        public async Task<IActionResult> getProcedureMaxSizeUpload()
        {
            ResultModel<long> res = new ResultModel<long>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<long>>("api/BR/Procedure/getProcedureMaxSizeUpload");

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
