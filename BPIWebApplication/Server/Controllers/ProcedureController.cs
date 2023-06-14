using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.FileUploadModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.Procedure;
using BPIWebApplication.Shared.PagesModel.AccessHistory;
using BPIWebApplication.Shared.PagesModel.Dashboard;
using BPIWebApplication.Shared.ReportModel;
using Microsoft.AspNetCore.Mvc;

namespace BPIWebApplication.Server.Controllers
{
    [Route("api/endUser/Procedure")]
    [ApiController]
    public class endProcedureController : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly long _maxFileSize;

        public endProcedureController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _http.BaseAddress = new Uri(config.GetValue<string>("ConnectionStrings:BpiFacade"));
            _maxFileSize = config.GetValue<long>("File:Procedure:MaxUpload");
        }

        [HttpGet("getAllProcedureData")]
        public async Task<IActionResult> getAllProcedureDataTable()
        {
            ResultModel<List<Procedure>> res = new ResultModel<List<Procedure>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Procedure>>>("api/Facade/Procedure/getAllProcedureData");

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<DepartmentProcedure>>>("api/Facade/Procedure/getDepartmentProcedureData");

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<DepartmentProcedure>>>($"api/Facade/Procedure/getDepartmentProcedureDatawithPaging/{param}");

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
                var result = await _http.PostAsJsonAsync<DashboardFilter>($"api/Facade/Procedure/getDepartmentProcedureDatawithFilterbyPaging", data);

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
                    res.ErrorMessage = "Fail Fetch data from getDepartmentProcedureDatawithFilterbyPaging Facade";

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<HistoryAccess>>>($"api/Facade/Procedure/getAllHistoryAccessDatawithPaging/{pageNo}");

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
                var result = await _http.PostAsJsonAsync<AccessHistoryFilter>($"api/Facade/Procedure/getAllHistoryAccessDatabyFilterwithPaging", data);

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
                    res.ErrorMessage = "Fail Fetch data from getAllHistoryAccessDatabyFilterwithPaging Facade";

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
                var result = await _http.PostAsJsonAsync<AccessHistoryReport>($"api/Facade/Procedure/getAllHistoryAccessDataReportbyFilter", data);

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
                    res.ErrorMessage = "Fail Fetch data from getAllHistoryAccessDataReportbyFilter Facade";

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
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/Facade/Procedure/getDepartmentProcedureNumberofPage/{param}");

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
                var result = await _http.PostAsJsonAsync<DashboardFilter>($"api/Facade/Procedure/getDepartmentProcedurewithFilterNumberofPage", data);

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
                    res.ErrorMessage = "Fail Fetch data from getDepartmentProcedurewithFilterNumberofPage Facade";

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
                var result = await _http.GetFromJsonAsync<ResultModel<int>>("api/Facade/Procedure/getHistoryAccessNumberofPage");

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
                var result = await _http.PostAsJsonAsync<AccessHistoryFilter>($"api/Facade/Procedure/getHistoryAccessbyFilterwithPagingNumberofPage", data);

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
                    res.ErrorMessage = "Fail Fetch data from getHistoryAccessbyFilterwithPagingNumberofPage Facade";

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

        //temp comment

        [HttpGet("getFile/{path}")]
        public async Task<IActionResult> getFiletoDownload(string path)
        {
            ResultModel<BPIWebApplication.Shared.MainModel.Stream.FileStream> res = new ResultModel<BPIWebApplication.Shared.MainModel.Stream.FileStream>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<BPIWebApplication.Shared.MainModel.Stream.FileStream>>($"api/Facade/Procedure/getFile/{path}");

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
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/Facade/Procedure/isProcedureDataPresent/{ProcNo}");

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
                var result = await _http.PostAsJsonAsync<ProcedureStream>($"api/Facade/Procedure/createProcedureData", data);

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
                    res.ErrorMessage = "Fail Fetch data from createProcedureData Facade";

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
                var result = await _http.PostAsJsonAsync<QueryModel<List<DepartmentProcedure>>>($"api/Facade/Procedure/createDepartmentProcedureData", data);

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
                    res.ErrorMessage = "Fail Fetch data from createDepartmentProcedureData Facade";

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
                var result = await _http.PostAsJsonAsync<QueryModel<HistoryAccess>>($"api/Facade/Procedure/createHistoryAccessData", data);

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
                    res.ErrorMessage = "Fail Fetch data from createHistoryAccessData Facade";

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
                var result = await _http.PostAsJsonAsync<ProcedureStream>($"api/Facade/Procedure/editProcedureData", data);

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
                    res.ErrorMessage = "Fail Fetch data from editProcedureData Facade";

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
                var result = await _http.PostAsJsonAsync<QueryModel<List<DepartmentProcedure>>>($"api/Facade/Procedure/deleteDepartmentProcedureData", data);

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
                    res.ErrorMessage = "Fail Fetch data from deleteDepartmentProcedureData Facade";

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
                //var result = await _http.GetFromJsonAsync<ResultModel<long>>("api/Facade/Procedure/getProcedureMaxSizeUpload");

                res.Data = 1024 * 1024 * _maxFileSize;

                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);

                //if (result.isSuccess)
                //{
                //    //res.Data = result.Data;

                //    res.Data = 1024 * 1024 * _maxFileSize;

                //    res.isSuccess = result.isSuccess;
                //    res.ErrorCode = result.ErrorCode;
                //    res.ErrorMessage = result.ErrorMessage;

                //    actionResult = Ok(res);
                //}
                //else
                //{
                //    res.Data = 0;

                //    res.isSuccess = result.isSuccess;
                //    res.ErrorCode = result.ErrorCode;
                //    res.ErrorMessage = result.ErrorMessage;

                //    actionResult = Ok(res);
                //}
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
