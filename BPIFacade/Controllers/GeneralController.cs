using BPIFacade.Models.DbModel;
using BPIFacade.Models.MainModel;
using BPIFacade.Models.MainModel.Company;
using BPIFacade.Models.PagesModel.AddEditProject;
using Microsoft.AspNetCore.Mvc;

namespace BPIFacade.Controllers
{
    [ApiController]
    [Route("api/Facade/BPIBase")]
    public class BPIBaseController : Controller
    {
        private readonly HttpClient _http;

        public BPIBaseController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _http.BaseAddress = new Uri(config.GetValue<string>("BaseUri:BpiBR"));
        }

        [HttpGet("getAllBisnisUnitData/{param}")]
        public async Task<IActionResult> getAllBisnisUnitDataTable(string param)
        {
            ResultModel<List<BisnisUnit>> res = new ResultModel<List<BisnisUnit>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<BisnisUnit>>>($"api/BR/BPIBase/getAllBisnisUnitData/{param}");

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<Department>>>($"api/BR/BPIBase/getAllDepartmentData/{param}");

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<Project>>>("api/BR/BPIBase/getAllProjectData");

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

        [HttpGet("getAllCategories")]
        public async Task<IActionResult> getAllCategories()
        {
            ResultModel<List<BPIFacade.Models.MainModel.Company.Category>> res = new();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<BPIFacade.Models.MainModel.Company.Category>>>("api/BR/BPIBase/getAllCategories");

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
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/BR/BPIBase/isDepartmentDataPresent/{DeptID}");

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
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/BR/BPIBase/isProjectPresent/{projectNo}");

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
                var result = await _http.PostAsJsonAsync<QueryModel<Department>>($"api/BR/BPIBase/createNewDepartmentData", data);

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
                    res.ErrorMessage = "Fail Fetch data from createNewDepartmentData BR";

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
                var result = await _http.PostAsJsonAsync<QueryModel<Project>>($"api/BR/BPIBase/createNewProjectData", data);

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
                    res.ErrorMessage = "Fail Fetch data from createNewProjectData BR";

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
                var result = await _http.PostAsJsonAsync<QueryModel<Department>>($"api/BR/BPIBase/editDepartmentData", data);

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
                    res.ErrorMessage = "Fail Fetch data from editDepartmentData BR";

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
                var result = await _http.PostAsJsonAsync<QueryModel<Project>>($"api/BR/BPIBase/editProjectData", data);

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
                    res.ErrorMessage = "Fail Fetch data from editDepartmentData BR";

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
}
