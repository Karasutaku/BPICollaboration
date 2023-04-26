﻿using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Data;
using BPIBR.Models.DbModel;
using BPIBR.Models.MainModel;
using BPIBR.Models.MainModel.Company;
using BPIBR.Models.PagesModel.AddEditProject;
using BPIBR.Models.MainModel.Procedure;
using BPIBR.Models.MainModel.Procedure.Filter;
using BPIBR.Models.MainModel.Procedure.Report;
using BPIBR.Models.MainModel.PettyCash;
using System.Net.Http.Json;
using System.IO;
using System.IO.Pipelines;
using BPIBR.Models.MainModel.Mailing;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using BPIBR.Models.MainModel.CashierLogbook;
using BPIBR.Models.MainModel.Standarizations;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.IO.Compression;
using System.Threading.Tasks;
using BPILibrary;
using DocumentFormat.OpenXml.Office2013.Excel;
using BPIBR.Models.MainModel.EPKRS;

namespace BPIBR.Controllers
{
    [Route("api/BR/BPIBase")]
    [ApiController]
    public class BPIBaseController : Controller
    {
        private readonly HttpClient _http;

        public BPIBaseController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _http.BaseAddress = new Uri(config.GetValue<string>("BaseUri:BpiDA"));
        }

        [HttpGet("getAllBisnisUnitData/{param}")]
        public async Task<IActionResult> getAllBisnisUnitDataTable(string param)
        {
            ResultModel<List<BisnisUnit>> res = new ResultModel<List<BisnisUnit>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<BisnisUnit>>>($"api/DA/BPIBase/getAllBisnisUnitData/{param}");

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

        [HttpGet("getAllDepartmentData/{param}")]
        public async Task<IActionResult> getAllDepartmentDataTable(string param)
        {
            ResultModel<List<Department>> res = new ResultModel<List<Department>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Department>>>($"api/DA/BPIBase/getAllDepartmentData/{param}");

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

        [HttpGet("getAllProjectData")]
        public async Task<IActionResult> getAllProjectDataTable()
        {
            ResultModel<List<Project>> res = new ResultModel<List<Project>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Project>>>("api/DA/BPIBase/getAllProjectData");

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

        // http get (check exist)

        [HttpGet("isDepartmentDataPresent/{DeptID}")]
        public async Task<IActionResult> isDepartmentDataPresentInTable(string DeptID)
        {
            ResultModel<string> res = new ResultModel<string>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/DA/BPIBase/isDepartmentDataPresent/{DeptID}");

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

        [HttpGet("isProjectPresent/{projectNo}")]
        public async Task<IActionResult> isProjectDataPresentInTable(string projectNo)
        {
            ResultModel<string> res = new ResultModel<string>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/DA/BPIBase/isProjectPresent/{projectNo}");

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

        // http post (create)

        [HttpPost("createNewDepartmentData")]
        public async Task<IActionResult> createNewDepartmentDataTable(QueryModel<Department> data)
        {
            ResultModel<QueryModel<Department>> res = new ResultModel<QueryModel<Department>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<Department>>($"api/DA/BPIBase/createNewDepartmentData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Department>>>();

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
                    res.ErrorMessage = "Fail Fetch data from createNewDepartmentData DA";

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

        [HttpPost("createNewProjectData")]
        public async Task<IActionResult> createProjectDataTable(QueryModel<Project> data)
        {
            ResultModel<QueryModel<Project>> res = new ResultModel<QueryModel<Project>>();
            IActionResult actionResult = null;

            // create procedure data to db
            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<Project>>($"api/DA/BPIBase/createNewProjectData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Project>>>();

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
                    res.ErrorMessage = "Fail Fetch data from createNewProjectData DA";

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

        // http post (edit)

        [HttpPost("editDepartmentData")]
        public async Task<IActionResult> editDepartmentDataTable(QueryModel<Department> data)
        {
            ResultModel<QueryModel<Department>> res = new ResultModel<QueryModel<Department>>();
            IActionResult actionResult = null;

            // create procedure data to db
            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<Department>>($"api/DA/BPIBase/editDepartmentData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Department>>>();

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
                    res.ErrorMessage = "Fail Fetch data from editDepartmentData DA";

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


        [HttpPost("editProjectData")]
        public async Task<IActionResult> editProjectDataTable(QueryModel<Project> data)
        {
            ResultModel<QueryModel<Project>> res = new ResultModel<QueryModel<Project>>();
            IActionResult actionResult = null;

            // create procedure data to db
            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<Project>>($"api/DA/BPIBase/editProjectData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Project>>>();

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
                    res.ErrorMessage = "Fail Fetch data from editDepartmentData DA";

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
    }

    [Route("api/BR/Procedure")]
    [ApiController]
    public class ProcedureController : Controller
    {
        private readonly HttpClient _http;

        public ProcedureController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _http.BaseAddress = new Uri(config.GetValue<string>("BaseUri:BpiDA"));
        }


        [HttpGet("getAllProcedureData")]
        public async Task<IActionResult> getAllProcedureDataTable()
        {
            ResultModel<List<Procedure>> res = new ResultModel<List<Procedure>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Procedure>>>("api/DA/Procedure/getAllProcedureData");

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<DepartmentProcedure>>>("api/DA/Procedure/getDepartmentProcedureData");

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<DepartmentProcedure>>>($"api/DA/Procedure/getDepartmentProcedureDatawithPaging/{param}");

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
                var result = await _http.PostAsJsonAsync<DashboardFilter>($"api/DA/Procedure/getDepartmentProcedureDatawithFilterbyPaging", data);

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
                    res.ErrorMessage = "Fail Fetch data from getDepartmentProcedureDatawithFilterbyPaging DA";

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<HistoryAccess>>>($"api/DA/Procedure/getAllHistoryAccessDatawithPaging/{pageNo}");

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
                var result = await _http.PostAsJsonAsync<AccessHistoryFilter>($"api/DA/Procedure/getAllHistoryAccessDatabyFilterwithPaging", data);

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
                    res.ErrorMessage = "Fail Fetch data from getAllHistoryAccessDatabyFilterwithPaging DA";

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
                var result = await _http.PostAsJsonAsync<AccessHistoryReport>($"api/DA/Procedure/getAllHistoryAccessDataReportbyFilter", data);

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
                    res.ErrorMessage = "Fail Fetch data from getAllHistoryAccessDataReportbyFilter DA";

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
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/DA/Procedure/getDepartmentProcedureNumberofPage/{param}");

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
                var result = await _http.PostAsJsonAsync<DashboardFilter>($"api/DA/Procedure/getDepartmentProcedurewithFilterNumberofPage", data);

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
                    res.ErrorMessage = "Fail Fetch data from getDepartmentProcedurewithFilterNumberofPage DA";

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
                var result = await _http.GetFromJsonAsync<ResultModel<int>>("api/DA/Procedure/getHistoryAccessNumberofPage");

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
                var result = await _http.PostAsJsonAsync<AccessHistoryFilter>($"api/DA/Procedure/getHistoryAccessbyFilterwithPagingNumberofPage", data);

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
                    res.ErrorMessage = "Fail Fetch data from getHistoryAccessbyFilterwithPagingNumberofPage DA";

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
            ResultModel<BPIBR.Models.MainModel.Stream.FileStream> res = new ResultModel<BPIBR.Models.MainModel.Stream.FileStream>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<BPIBR.Models.MainModel.Stream.FileStream>>($"api/DA/Procedure/getFile/{path}");

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
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/DA/Procedure/isProcedureDataPresent/{ProcNo}");

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
                var result = await _http.PostAsJsonAsync<ProcedureStream>($"api/DA/Procedure/createProcedureData", data);

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
                    res.ErrorMessage = "Fail Fetch data from createProcedureData DA";

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
                var result = await _http.PostAsJsonAsync<QueryModel<List<DepartmentProcedure>>>($"api/DA/Procedure/createDepartmentProcedureData", data);

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
                    res.ErrorMessage = "Fail Fetch data from createDepartmentProcedureData DA";

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
                var result = await _http.PostAsJsonAsync<QueryModel<HistoryAccess>>($"api/DA/Procedure/createHistoryAccessData", data);

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
                    res.ErrorMessage = "Fail Fetch data from createHistoryAccessData DA";

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
                var result = await _http.PostAsJsonAsync<ProcedureStream>($"api/DA/Procedure/editProcedureData", data);

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
                    res.ErrorMessage = "Fail Fetch data from editProcedureData DA";

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
                var result = await _http.PostAsJsonAsync<QueryModel<List<DepartmentProcedure>>>($"api/DA/Procedure/deleteDepartmentProcedureData", data);

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
                    res.ErrorMessage = "Fail Fetch data from deleteDepartmentProcedureData DA";

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
                var result = await _http.GetFromJsonAsync<ResultModel<long>>("api/DA/Procedure/getProcedureMaxSizeUpload");

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
                        QueryModel<List<AdvanceLine>> lines = new();
                        lines.Data = new();

                        lines.Data = data.Data.lines;
                        lines.userEmail = data.userEmail;
                        lines.userAction = data.userAction;
                        lines.userActionDate = data.userActionDate;

                        var result2 = await _http.PostAsJsonAsync<QueryModel<List<AdvanceLine>>>($"api/DA/PettyCash/createAdvanceLineData", lines);

                        if (result2.IsSuccessStatusCode)
                        {
                            finRes.Data = data;

                            finRes.isSuccess = true;
                            finRes.ErrorCode = "00";
                            finRes.ErrorMessage = "Request Create Advance Success";

                            actionResult = Ok(finRes);
                        }
                        else
                        {
                            finRes.Data = null;

                            finRes.isSuccess = false;
                            finRes.ErrorCode = "01";
                            finRes.ErrorMessage = "Request Create Advance Line Failed";

                            actionResult = Ok(finRes);
                        }
                    }
                    else
                    {
                        finRes.Data = data;

                        finRes.isSuccess = false;
                        finRes.ErrorCode = "01";
                        finRes.ErrorMessage = "Create Advance Failed";

                        actionResult = Ok(finRes);
                    }
                }
                else
                {
                    finRes.Data = data;

                    finRes.isSuccess = false;
                    finRes.ErrorCode = "01";
                    finRes.ErrorMessage = "Request Create Advance Failed";

                    actionResult = Ok(finRes);
                }

                //Task<ResultModel<QueryModel<Advance>>> res1 = Task.Run(async () =>
                //{
                //    ResultModel<QueryModel<Advance>> retResult = new ResultModel<QueryModel<Advance>>();
                //    var result1 = await _http.PostAsJsonAsync<QueryModel<Advance>>($"api/DA/PettyCash/createAdvanceData", data);

                //    if (result1.IsSuccessStatusCode)
                //    {
                //        var respBody = await result1.Content.ReadFromJsonAsync<ResultModel<QueryModel<Advance>>>();

                //        retResult.Data = respBody.Data;

                //        retResult.isSuccess = respBody.isSuccess;
                //        retResult.ErrorCode = respBody.ErrorCode;
                //        retResult.ErrorMessage = respBody.ErrorMessage;

                //        return retResult;
                //    }
                //    else
                //    {
                //        retResult.Data = null;

                //        retResult.isSuccess = result1.IsSuccessStatusCode;
                //        retResult.ErrorCode = "01";
                //        retResult.ErrorMessage = "Fail Create data from createAdvanceData DA";

                //        return retResult;
                //    }

                //});
                //Task<ResultModel<QueryModel<List<AdvanceLine>>>> res2 = Task.Run(async () =>
                //{
                //    ResultModel<QueryModel<List<AdvanceLine>>> retResult = new ResultModel<QueryModel<List<AdvanceLine>>>();
                //    QueryModel<List<AdvanceLine>> lines = new();
                //    lines.Data = new();

                //    lines.Data = data.Data.lines;
                //    lines.userEmail = data.userEmail;
                //    lines.userAction = data.userAction;
                //    lines.userActionDate = data.userActionDate;

                //    var result2 = await _http.PostAsJsonAsync<QueryModel<List<AdvanceLine>>>($"api/DA/PettyCash/createAdvanceLineData", lines);

                //    if (result2.IsSuccessStatusCode)
                //    {
                //        var respBody = await result2.Content.ReadFromJsonAsync<ResultModel<QueryModel<List<AdvanceLine>>>>();

                //        retResult.Data = respBody.Data;

                //        retResult.isSuccess = respBody.isSuccess;
                //        retResult.ErrorCode = respBody.ErrorCode;
                //        retResult.ErrorMessage = respBody.ErrorMessage;

                //        return retResult;
                //    }
                //    else
                //    {
                //        retResult.Data = null;

                //        retResult.isSuccess = result2.IsSuccessStatusCode;
                //        retResult.ErrorCode = "01";
                //        retResult.ErrorMessage = "Fail Create data from createAdvanceLineData DA";

                //        return retResult;
                //    }
                //});

                //await Task.WhenAll(res1, res2);

                //if (!res1.Result.isSuccess)
                //{
                //    throw new Exception(res1.Result.ErrorMessage);
                //}

                //if (!res2.Result.isSuccess)
                //{
                //    throw new Exception(res2.Result.ErrorMessage);
                //}


                //if (res1.Result.isSuccess && res2.Result.isSuccess)
                //{
                //    finRes.Data = data;

                //    finRes.isSuccess = true;
                //    finRes.ErrorCode = "00";
                //    finRes.ErrorMessage = "Create Advance Success";

                //    actionResult = Ok(finRes);
                //}
                //else
                //{
                //    finRes.Data = null;

                //    finRes.isSuccess = false;
                //    finRes.ErrorCode = "01";
                //    finRes.ErrorMessage = $"Create Advance Failed (advanceData: {res1.Result.ErrorMessage}, advanceLine: {res2.Result.ErrorMessage}";

                //    actionResult = Ok(finRes);
                //}

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
                        QueryModel<List<ExpenseLine>> lines = new();
                        lines.Data = new();

                        lines.Data = data.expenseDetails.Data.lines;
                        lines.userEmail = data.expenseDetails.userEmail;
                        lines.userAction = data.expenseDetails.userAction;
                        lines.userActionDate = data.expenseDetails.userActionDate;

                        var result2 = await _http.PostAsJsonAsync<QueryModel<List<ExpenseLine>>>($"api/DA/PettyCash/createExpenseLineData", lines);

                        if (result2.IsSuccessStatusCode)
                        {
                            QueryModel<List<BPIBR.Models.MainModel.Stream.FileStream>> files = new();
                            files.Data = new();

                            files.Data = data.files;
                            files.userEmail = data.expenseDetails.userEmail;
                            files.userAction = data.expenseDetails.userAction;
                            files.userActionDate = data.expenseDetails.userActionDate;

                            var result3 = await _http.PostAsJsonAsync<QueryModel<List<BPIBR.Models.MainModel.Stream.FileStream>>>($"api/DA/PettyCash/createExpenseAttachLineData", files);

                            if (result3.IsSuccessStatusCode)
                            {
                                foreach (var file in data.files)
                                {
                                    string path = Path.Combine(_uploadPath, "ExpenseAttach", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), data.expenseDetails.Data.ExpenseID, file.fileName);

                                    await saveFiletoDirectory(path, file.content);
                                }
                            }
                            else
                            {
                                finRes.Data = null;

                                finRes.isSuccess = false;
                                finRes.ErrorCode = "01";
                                finRes.ErrorMessage = "Request Create Expense Attach Line Failed";

                                actionResult = Ok(finRes);
                            }

                            finRes.Data = data.expenseDetails;

                            finRes.isSuccess = true;
                            finRes.ErrorCode = "00";
                            finRes.ErrorMessage = "Request Create Expense Success";

                            actionResult = Ok(finRes);

                        }
                        else
                        {
                            finRes.Data = null;

                            finRes.isSuccess = false;
                            finRes.ErrorCode = "01";
                            finRes.ErrorMessage = "Request Create Expense Line Failed";

                            actionResult = Ok(finRes);
                        }
                    }
                    else
                    {
                        finRes.Data = null;

                        finRes.isSuccess = false;
                        finRes.ErrorCode = "01";
                        finRes.ErrorMessage = "Create Expense Failed";

                        actionResult = Ok(finRes);
                    }
                }
                else
                {
                    finRes.Data = null;

                    finRes.isSuccess = false;
                    finRes.ErrorCode = "01";
                    finRes.ErrorMessage = "Request Create Expense Failed";

                    actionResult = Ok(finRes);
                }

                //Task<ResultModel<QueryModel<Expense>>> res1 = Task.Run(async () =>
                //{
                //    ResultModel<QueryModel<Expense>> retResult = new ResultModel<QueryModel<Expense>>();
                //    var result1 = await _http.PostAsJsonAsync<QueryModel<Expense>>($"api/DA/PettyCash/createExpenseData", data.expenseDetails);

                //    if (result1.IsSuccessStatusCode)
                //    {
                //        var respBody = await result1.Content.ReadFromJsonAsync<ResultModel<QueryModel<Expense>>>();

                //        retResult.Data = respBody.Data;

                //        retResult.isSuccess = respBody.isSuccess;
                //        retResult.ErrorCode = respBody.ErrorCode;
                //        retResult.ErrorMessage = respBody.ErrorMessage;

                //        return retResult;
                //    }
                //    else
                //    {
                //        retResult.Data = null;

                //        retResult.isSuccess = result1.IsSuccessStatusCode;
                //        retResult.ErrorCode = "01";
                //        retResult.ErrorMessage = "Fail Create data from createExpenseData DA";

                //        return retResult;
                //    }

                //});
                //Task<ResultModel<QueryModel<List<ExpenseLine>>>> res2 = Task.Run(async () =>
                //{
                //    ResultModel<QueryModel<List<ExpenseLine>>> retResult = new ResultModel<QueryModel<List<ExpenseLine>>>();
                //    QueryModel<List<ExpenseLine>> lines = new();
                //    lines.Data = new();

                //    lines.Data = data.expenseDetails.Data.lines;
                //    lines.userEmail = data.expenseDetails.userEmail;
                //    lines.userAction = data.expenseDetails.userAction;
                //    lines.userActionDate = data.expenseDetails.userActionDate;

                //    var result2 = await _http.PostAsJsonAsync<QueryModel<List<ExpenseLine>>>($"api/DA/PettyCash/createExpenseLineData", lines);

                //    if (result2.IsSuccessStatusCode)
                //    {
                //        foreach (var file in data.files)
                //        {
                //            string path = Path.Combine(_uploadPath, "ExpenseAttach", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), data.expenseDetails.Data.ExpenseID, file.fileName);

                //            await saveFiletoDirectory(path, file.content);
                //        }

                //        var respBody = await result2.Content.ReadFromJsonAsync<ResultModel<QueryModel<List<ExpenseLine>>>>();

                //        retResult.Data = respBody.Data;

                //        retResult.isSuccess = respBody.isSuccess;
                //        retResult.ErrorCode = respBody.ErrorCode;
                //        retResult.ErrorMessage = respBody.ErrorMessage;

                //        return retResult;
                //    }
                //    else
                //    {
                //        retResult.Data = null;

                //        retResult.isSuccess = result2.IsSuccessStatusCode;
                //        retResult.ErrorCode = "01";
                //        retResult.ErrorMessage = "Fail Create data from createExpenseLineData DA";

                //        return retResult;
                //    }
                //});

                //await Task.WhenAll(res1, res2);

                //if (!res1.Result.isSuccess)
                //{
                //    throw new Exception(res1.Result.ErrorMessage);
                //}

                //if (!res2.Result.isSuccess)
                //{
                //    throw new Exception(res2.Result.ErrorMessage);
                //}


                //if (res1.Result.isSuccess && res2.Result.isSuccess)
                //{
                //    finRes.Data = data.expenseDetails;

                //    finRes.isSuccess = true;
                //    finRes.ErrorCode = "00";
                //    finRes.ErrorMessage = "Create Expense Success";

                //    actionResult = Ok(finRes);
                //}
                //else
                //{
                //    finRes.Data = null;

                //    finRes.isSuccess = false;
                //    finRes.ErrorCode = "01";
                //    finRes.ErrorMessage = $"Create Advance Failed (expenseData: {res1.Result.ErrorMessage}, expenseLine: {res2.Result.ErrorMessage}";

                //    actionResult = Ok(finRes);
                //}

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
                        QueryModel<List<ReimburseLine>> lines = new();
                        lines.Data = new();

                        lines.Data = data.reimburseDetails.Data.lines;
                        lines.userEmail = data.reimburseDetails.userEmail;
                        lines.userAction = data.reimburseDetails.userAction;
                        lines.userActionDate = data.reimburseDetails.userActionDate;

                        var result2 = await _http.PostAsJsonAsync<QueryModel<List<ReimburseLine>>>($"api/DA/PettyCash/createReimburseLineData", lines);

                        if (result2.IsSuccessStatusCode)
                        {
                            QueryModel<List<BPIBR.Models.MainModel.Stream.FileStream>> files = new();
                            files.Data = new();

                            files.Data = data.files;
                            files.userEmail = data.reimburseDetails.userEmail;
                            files.userAction = data.reimburseDetails.userAction;
                            files.userActionDate = data.reimburseDetails.userActionDate;

                            var result3 = await _http.PostAsJsonAsync<QueryModel<List<BPIBR.Models.MainModel.Stream.FileStream>>>($"api/DA/PettyCash/createReimburseAttachLineData", files);

                            if (result3.IsSuccessStatusCode)
                            {
                                //foreach (var file in data.files)
                                //{
                                //    string path = Path.Combine(_uploadPath, "ReimburseAttach", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), data.reimburseDetails.Data.ReimburseID, file.fileName);

                                //    await saveFiletoDirectory(path, file.content);
                                //}

                                finRes.Data = data.reimburseDetails;

                                finRes.isSuccess = true;
                                finRes.ErrorCode = "00";
                                finRes.ErrorMessage = "Request Create Reimburse Success";

                                actionResult = Ok(finRes);
                            }
                            else
                            {
                                finRes.Data = null;

                                finRes.isSuccess = false;
                                finRes.ErrorCode = "01";
                                finRes.ErrorMessage = "Request Create Reimburse Attach Line Failed";

                                actionResult = Ok(finRes);
                            }

                            finRes.Data = data.reimburseDetails;

                            finRes.isSuccess = true;
                            finRes.ErrorCode = "00";
                            finRes.ErrorMessage = "Request Create Reimburse Success";

                            actionResult = Ok(finRes);

                        }
                        else
                        {
                            finRes.Data = null;

                            finRes.isSuccess = false;
                            finRes.ErrorCode = "01";
                            finRes.ErrorMessage = "Request Create Reimburse Line Failed";

                            actionResult = Ok(finRes);
                        }
                    }
                    else
                    {
                        finRes.Data = null;

                        finRes.isSuccess = false;
                        finRes.ErrorCode = "01";
                        finRes.ErrorMessage = "Create Reimburse Failed";

                        actionResult = Ok(finRes);
                    }
                }
                else
                {
                    finRes.Data = null;

                    finRes.isSuccess = false;
                    finRes.ErrorCode = "01";
                    finRes.ErrorMessage = "Request Create Reimburse Failed";

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
                // archiving file
                //foreach (var Id in data)
                //{
                //    var result1 = await _http.GetFromJsonAsync<ResultModel<List<AttachmentLine>>>($"api/DA/PettyCash/getAttachmentLines/{Id}");

                //    if (result1.isSuccess)
                //    {
                //        foreach (var line in result1.Data)
                //        {
                //            string arcPath = Path.Combine(_archivePath, "ExpenseAttach", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), line.ExpenseID, line.PathFile);
                //            string upPath = Path.Combine(_uploadPath, "ExpenseAttach", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), line.ExpenseID, line.PathFile);

                //            if (System.IO.File.Exists(upPath))
                //            {
                //                await saveFiletoDirectory(arcPath, getFileStream(line.ExpenseID, line.PathFile));

                //                System.IO.File.Delete(upPath);
                //            }
                            
                //        }
                //    }
                //}
                
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

                    if (result1.isSuccess)
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
                //if (data.Data.Split("!_!")[1].Contains("E") && data.Data.Split("!_!")[4].Equals("Open") && data.Data.Split("!_!")[2].Equals("Rejected"))
                //{
                //    string temp = data.Data.Split("!_!")[1] + "!_!MASTER";

                //    var result1 = await _http.GetFromJsonAsync<ResultModel<List<AttachmentLine>>>($"api/DA/PettyCash/getAttachmentLines/{Base64Encode(temp)}");

                //    if (result1.isSuccess)
                //    {
                //        foreach (var dt in result1.Data)
                //        {
                //            deleteFilefromDirectory(data.Data.Split("!_!")[1], dt.PathFile);
                //        }
                //    }
                //}

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
                    ErrorMessage = "PARALEL TASK FORCED STOP : REASON " + ex.Message
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

                if (result.isSuccess)
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

        //[HttpGet("getLocationBudgetDetails/{loc}")]
        //public async Task<IActionResult> getLocationBudgetDetails(string loc)
        //{
        //    ResultModel<BalanceDetails> res = new ResultModel<BalanceDetails>();
        //    IActionResult actionResult = null;

        //    try
        //    {
        //        var result = await _http.GetFromJsonAsync<ResultModel<BalanceDetails>>($"api/DA/PettyCash/getLocationBudgetDetails/{loc}");

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

    [Route("api/BR/CashierLogbook")]
    [ApiController]
    public class CashierLogbookController : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;

        public CashierLogbookController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _configuration = config;
            _http.BaseAddress = new Uri(_configuration.GetValue<string>("BaseUri:BpiDA"));
        }


        [HttpPost("createLogData")]
        public async Task<IActionResult> createLogData(QueryModel<CashierLogData> data)
        {
            ResultModel<QueryModel<CashierLogData>> res = new ResultModel<QueryModel<CashierLogData>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<CashierLogData>>($"api/DA/CashierLogbook/createLogData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<CashierLogData>>>();

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
                    res.ErrorMessage = "Fail settle from createLogData DA";

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

        [HttpPost("editLogData")]
        public async Task<IActionResult> editLogData(QueryModel<CashierLogData> data)
        {
            ResultModel<QueryModel<CashierLogData>> res = new ResultModel<QueryModel<CashierLogData>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<CashierLogData>>($"api/DA/CashierLogbook/editLogData", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<CashierLogData>>>();

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
                    res.ErrorMessage = "Fail settle from editLogData DA";

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

        [HttpPost("editBrankasDocumentStatus")]
        public async Task<IActionResult> updateBrankasDocumentStatusDataTable(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<string>>($"api/DA/CashierLogbook/editBrankasDocumentStatus", data);

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
                    res.ErrorMessage = "Fail settle from editBrankasDocumentStatus DA";

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

        [HttpGet("getLogData/{locPage}")]
        public async Task<IActionResult> getLogDataTable(string locPage)
        {
            ResultModel<List<CashierLogData>> res = new ResultModel<List<CashierLogData>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<CashierLogData>>>($"api/DA/CashierLogbook/getLogData/{locPage}");

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

        [HttpGet("getBrankasActionLogData/{locPage}")]
        public async Task<IActionResult> getBrankasActionLogDataTable(string locPage)
        {
            ResultModel<List<CashierLogAction>> res = new ResultModel<List<CashierLogAction>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<CashierLogAction>>>($"api/DA/CashierLogbook/getBrankasActionLogData/{locPage}");

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

        [HttpGet("getShiftbyModule/{moduleName}")]
        public async Task<IActionResult> getShiftbyModuleData(string moduleName)
        {
            ResultModel<List<Shift>> res = new ResultModel<List<Shift>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Shift>>>($"api/DA/CashierLogbook/getShiftbyModule/{moduleName}");

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

        [HttpGet("getCashierLogbookCategories")]
        public async Task<IActionResult> getCashierLogbookCategories()
        {
            ResultModel<CashierLogbookCategories> res = new ResultModel<CashierLogbookCategories>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<CashierLogbookCategories>>($"api/DA/CashierLogbook/getCashierLogbookCategories");

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

        [HttpGet("getModulePageSize/{Table}")]
        public async Task<IActionResult> getModulePageSize(string Table)
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/DA/CashierLogbook/getModulePageSize/{Table}");

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

        [HttpGet("getNumberofLogExisting/{param}")]
        public async Task<IActionResult> getNumberofLogExisting(string param)
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/DA/CashierLogbook/getNumberofLogExisting/{param}");

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

        [HttpPost("editBrankasApproveLogOnConfirm")]
        public async Task<IActionResult> editBrankasApproveLogOnConfirm(QueryModel<CashierLogApproval> data)
        {
            ResultModel<QueryModel<CashierLogApproval>> res = new ResultModel<QueryModel<CashierLogApproval>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<CashierLogApproval>>($"api/DA/CashierLogbook/editBrankasApproveLogOnConfirm", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<CashierLogApproval>>>();

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
                    res.ErrorMessage = $"Fail settle from editBrankasApproveLogOnConfirm DA";

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

    [Route("api/BR/Standarization")]
    [ApiController]
    public class StandarizationController : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        private readonly string _uploadPath;
        private readonly string[] _compressedFileExtensions;

        public StandarizationController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _configuration = config;
            _http.BaseAddress = new Uri(_configuration.GetValue<string>("BaseUri:BpiDA"));
            _uploadPath = _configuration.GetValue<string>("File:Standarizations:UploadPath");
            _compressedFileExtensions = config.GetValue<string>("File:Standarizations:CompressedFileExtensions").Split("|");
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

        internal async Task saveFiletoDirectoryAsZip(string path, string originalName, Byte[] content)
        {
            string dir = Path.GetDirectoryName(path);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (var compressedStream = new MemoryStream())
            {
                using (ZipArchive archive = new ZipArchive(compressedStream, ZipArchiveMode.Create, false))
                {
                    FileInfo fi = new FileInfo(originalName);
                    var entry = archive.CreateEntry(fi.Name);

                    using (var original = new MemoryStream(content))
                    {
                        using (var zipEntryStream = entry.Open())
                        {
                            original.CopyTo(zipEntryStream);
                        }
                    }
                }

                using FileStream fs = new(path, FileMode.Create);
                Stream stream = new MemoryStream(compressedStream.ToArray());
                stream.CopyTo(fs);
            }
        }

        // data processing
        private async Task<Byte[]> compressData(byte[] data)
        {
            byte[] compressedData = new byte[0];

            using (var compressedStream = new MemoryStream())
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                zipStream.Write(data, 0, data.Length);
                zipStream.Close();
                compressedData = compressedStream.ToArray();
            }

            return compressedData;
        }

        private async Task<Byte[]> decompressData(byte[] data)
        {
            byte[] decompressedData = new byte[0];

            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                decompressedData = resultStream.ToArray();
            }

            return decompressedData;
        }

        internal async Task<Byte[]> getFileStream(string type, string filename, DateTime date)
        {
            string path = Path.Combine(_uploadPath, type, date.Year.ToString(), date.Month.ToString(), date.Day.ToString(), Path.GetFileName(filename));

            return await System.IO.File.ReadAllBytesAsync(path);
        }

        private async Task<Byte[]> getFileStreamfromZip(string filename, string originalName, byte[] data)
        {
            byte[] compressedData = new byte[0];

            Stream stream = new MemoryStream(data);
            ZipArchive archive = new ZipArchive(stream);

            FileInfo fi = new FileInfo(originalName);
            var entryData = archive.GetEntry(fi.Name);
            var content = entryData.Open();

            using (var ms = new MemoryStream())
            {
                await content.CopyToAsync(ms);
                compressedData = ms.ToArray();
            }

            return compressedData;
        }

        internal bool deleteFilefromDirectory(string type, string filename, DateTime date)
        {
            string path = Path.Combine(_uploadPath, type, date.Year.ToString(), date.Month.ToString(), date.Day.ToString(), Path.GetFileName(filename));

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);

                return true;
            }

            return false;
        }

        [HttpPost("createStandarizationData")]
        public async Task<IActionResult> createStandarizationDataTable(StandarizationStream data)
        {
            ResultModel<QueryModel<Standarizations>> res = new ResultModel<QueryModel<Standarizations>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<Standarizations>>($"api/DA/Standarization/createStandarizationData", data.standarizationDetails);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Standarizations>>>();

                    foreach (var file in data.files)
                    {
                        if (_compressedFileExtensions.Any(y => y.Equals(file.fileType)))
                        //if (file.fileName.Contains(".mp4"))
                        {
                            byte[] tempContent = new byte[0];
                            string oriPath = Path.Combine(_uploadPath, data.standarizationDetails.Data.TypeID, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), file.fileName);
                            string zipPath = Path.ChangeExtension(oriPath, ".zip");

                            tempContent = await decompressData(file.content);

                            await saveFiletoDirectoryAsZip(zipPath, oriPath, tempContent);
                        }
                        else
                        {
                            string path = Path.Combine(_uploadPath, data.standarizationDetails.Data.TypeID, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), file.fileName);

                            await saveFiletoDirectory(path, file.content);
                        }
                    }

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
                    res.ErrorMessage = $"Fail Create from createStandarizationData DA";

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

        [HttpPost("editStandarizationData")]
        public async Task<IActionResult> editStandarizationDataTable(StandarizationStream data)
        {
            ResultModel<QueryModel<Standarizations>> res = new ResultModel<QueryModel<Standarizations>>();
            IActionResult actionResult = null;

            try
            {
                string param = Base64Encode(data.standarizationDetails.Data.StandarizationID + "!_!" + data.standarizationDetails.Data.TypeID);

                var result1 = await _http.GetFromJsonAsync<ResultModel<List<StandarizationAttachment>>>($"api/DA/Standarization/getStandarizationAttachment/{param}");

                if (result1.isSuccess && result1.ErrorCode.Equals("00"))
                {
                    res.Data = new();

                    foreach (var dt in result1.Data)
                    {
                        deleteFilefromDirectory(data.standarizationDetails.Data.TypeID, dt.FilePath, dt.UploadDate);
                    }
                }
                else
                {
                    throw new Exception("Fail Fetch File Data Path");
                }

                var result = await _http.PostAsJsonAsync<QueryModel<Standarizations>>($"api/DA/Standarization/editStandarizationData", data.standarizationDetails);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Standarizations>>>();

                    foreach (var file in data.files)
                    {
                        //string path = Path.Combine(_uploadPath, data.standarizationDetails.Data.TypeID, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), file.fileName);

                        //await saveFiletoDirectory(path, file.content);

                        if (_compressedFileExtensions.Any(y => y.Equals(file.fileType)))
                        //if (file.fileName.Contains(".mp4"))
                        {
                            byte[] tempContent = new byte[0];
                            string oriPath = Path.Combine(_uploadPath, data.standarizationDetails.Data.TypeID, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), file.fileName);
                            string zipPath = Path.ChangeExtension(oriPath, ".zip");

                            tempContent = await decompressData(file.content);

                            await saveFiletoDirectoryAsZip(zipPath, oriPath, tempContent);
                        }
                        else
                        {
                            string path = Path.Combine(_uploadPath, data.standarizationDetails.Data.TypeID, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), file.fileName);

                            await saveFiletoDirectory(path, file.content);
                        }
                    }

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
                    res.ErrorMessage = $"Fail settle from editStandarizationData DA";

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

        [HttpPost("deleteStandarizationData")]
        public async Task<IActionResult> deleteStandarizationDataTable(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            IActionResult actionResult = null;

            try
            {
                var result1 = await _http.GetFromJsonAsync<ResultModel<List<StandarizationAttachment>>>($"api/DA/Standarization/getStandarizationAttachment/{data.Data}");

                if (result1.isSuccess)
                {
                    res.Data = new();

                    string tp = Base64Decode(data.Data);

                    foreach (var dt in result1.Data)
                    {
                        deleteFilefromDirectory(tp.Split("!_!")[1], dt.FilePath, dt.UploadDate);
                    }
                }
                else
                {
                    throw new Exception("Fail Fetch File Data Path");
                }

                var result = await _http.PostAsJsonAsync<QueryModel<string>>($"api/DA/Standarization/deleteStandarizationData", data);

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
                    res.ErrorMessage = $"Fail settle from deleteStandarizationData DA";

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

        [HttpGet("getStandarizationTypes")]
        public async Task<IActionResult> getStandarizationTypesDataTable()
        {
            ResultModel<List<StandarizationType>> res = new ResultModel<List<StandarizationType>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<StandarizationType>>>("api/DA/Standarization/getStandarizationTypes");

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

        [HttpGet("getStandarizationData/{param}")]
        public async Task<IActionResult> getStandarizationDataTable(string param)
        {
            ResultModel<List<Standarizations>> res = new ResultModel<List<Standarizations>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Standarizations>>>($"api/DA/Standarization/getStandarizationData/{param}");

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

        [HttpGet("getStandarizationAttachment/{param}")]
        public async Task<IActionResult> getAttachFileStream(string param)
        {
            ResultModel<List<BPIBR.Models.MainModel.Stream.FileStream>> res = new ResultModel<List<BPIBR.Models.MainModel.Stream.FileStream>>();
            IActionResult actionResult = null;

            try
            {
                string dcParam = Base64Decode(param);

                var result = await _http.GetFromJsonAsync<ResultModel<List<StandarizationAttachment>>>($"api/DA/Standarization/getStandarizationAttachment/{param}");

                if (result.isSuccess)
                {
                    res.Data = new();

                    foreach (var dt in result.Data)
                    {
                        BPIBR.Models.MainModel.Stream.FileStream temp = new BPIBR.Models.MainModel.Stream.FileStream();

                        temp.type = dt.StandarizationID;
                        temp.fileName = dt.FilePath;
                        temp.fileDesc = dt.Descriptions;
                        temp.fileType = dt.FileExtention;
                        temp.fileSize = 0;

                        if (_compressedFileExtensions.Any(y => y.Equals(dt.FileExtention)))
                        //if (dt.FileExtention.Equals(".mp4"))
                        {
                            string oriPath = dt.FilePath;
                            string zipPath = Path.ChangeExtension(oriPath, ".zip");

                            byte[] tempContent = new byte[0];
                            tempContent = await getFileStream(dcParam.Split("!_!")[1], zipPath, dt.UploadDate);

                            temp.content = await getFileStreamfromZip(zipPath, oriPath, tempContent);
                            temp.content = await compressData(temp.content);
                        }
                        else
                        {
                            temp.content = await getFileStream(dcParam.Split("!_!")[1], dt.FilePath, dt.UploadDate);
                        }

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

        [HttpGet("getModulePageSize/{Table}")]
        public async Task<IActionResult> getModulePageSize(string Table)
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/DA/Standarization/getModulePageSize/{Table}");

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

    [Route("api/BR/EPKRS")]
    [ApiController]
    public class EPKRSController : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        private readonly string _uploadPath;

        public EPKRSController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _configuration = config;
            _http.BaseAddress = new Uri(_configuration.GetValue<string>("BaseUri:BpiDA"));
            _uploadPath = _configuration.GetValue<string>("File:EPKRS:UploadPath");
        }

        [HttpPost("createEPKRSItemCaseDocument")]
        public async Task<IActionResult> createEPKRSItemCaseDocumentData(ItemCaseStream data)
        {
            ResultModel<ItemCaseStream> res = new ResultModel<ItemCaseStream>();
            IActionResult actionResult = null;

            try
            {

                var result = await _http.PostAsJsonAsync<QueryModel<EPKRSUploadItemCase>>($"api/DA/EPKRS/createEPKRSItemCaseDocument", data.mainData);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<EPKRSUploadItemCase>>>();

                    data.files.ForEach(async x =>
                    {
                        string path = Path.Combine(_uploadPath, data.mainData.Data.itemCase.DocumentID, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), x.fileName);

                        await CommonLibrary.saveFiletoDirectory(path, x.content);
                    });

                    res.Data.mainData = respBody.Data;
                    res.Data.files = null;

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

        [HttpPost("createEPKRSIncidentAccidentDocument")]
        public async Task<IActionResult> createEPKRSIncidentAccidentDocumentData(IncidentAccidentStream data)
        {
            ResultModel<IncidentAccidentStream> res = new ResultModel<IncidentAccidentStream>();
            IActionResult actionResult = null;

            try
            {

                var result = await _http.PostAsJsonAsync<QueryModel<EPKRSUploadIncidentAccident>>($"api/DA/EPKRS/createEPKRSIncidentAccidentDocument", data.mainData);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<EPKRSUploadIncidentAccident>>>();

                    data.files.ForEach(async x =>
                    {
                        string path = Path.Combine(_uploadPath, data.mainData.Data.incidentAccident.DocumentID, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), x.fileName);

                        await CommonLibrary.saveFiletoDirectory(path, x.content);
                    });

                    res.Data.mainData = respBody.Data;

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

        [HttpPost("createEPKRSDocumentDiscussion")]
        public async Task<IActionResult> createEPKRSDocumentDiscussionData(QueryModel<EPKRSUploadDiscussion> data)
        {
            ResultModel<QueryModel<EPKRSUploadDiscussion>> res = new ResultModel<QueryModel<EPKRSUploadDiscussion>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<EPKRSUploadDiscussion>>($"api/DA/EPKRS/createEPKRSDocumentDiscussion", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<EPKRSUploadDiscussion>>>();

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

        //
    }

}
