using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.Procedure;
using BPIWebApplication.Shared.PagesModel.AddEditProject;
using Microsoft.AspNetCore.Mvc;

namespace BPIWebApplication.Server.Controllers
{
    [ApiController]
    [Route("api/endUser/BPIBase")]
    public class BpiBaseController : Controller
    {
        private readonly HttpClient _http;
        private readonly string _smsApi, _frameworkApi, _BPIApi;

        public BpiBaseController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _BPIApi = config.GetValue<string>("ConnectionStrings:BpiFacade");
            _smsApi = config.GetValue<string>("ConnectionStrings:SmsApi");
            _frameworkApi = config.GetValue<string>("ConnectionStrings:FrameworkApi");
        }

        [HttpPost("getCompanyLocation")]
        public async Task<IActionResult> authenticateToITFacade(Location data)
        {
            ResultModel<List<LocationResp>> res = new ResultModel<List<LocationResp>>();
            IActionResult actionResult = null;

            try
            {
                //data.ipClient = _httpAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                _http.BaseAddress = new Uri(_frameworkApi);
                var result = await _http.PostAsJsonAsync<Location>("Api/Company/GetLocations", data);

                if (result.IsSuccessStatusCode)
                {
                    //var x = await result.Content.ReadAsStringAsync();
                    //var c = JsonSerializer.Deserialize<FacadeLoginResponse>(x, new JsonSerializerOptions() { PropertyNameCaseInsensitive=true });

                    var respBody = await result.Content.ReadFromJsonAsync<List<LocationResp>>();

                    if (respBody.Any())
                    {
                        res.Data = respBody;

                        res.isSuccess = result.IsSuccessStatusCode;
                        res.ErrorCode = "00";
                        res.ErrorMessage = "Fetch Location Success";

                        actionResult = Ok(res);
                    }
                    else
                    {
                        res.Data = respBody;
                        // res.Data.token = string.Empty;

                        res.isSuccess = false;
                        res.ErrorCode = "01";
                        res.ErrorMessage = "Fetch Location Failed";

                        actionResult = Ok(respBody);
                    }

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

        [HttpGet("getAllBisnisUnitData/{param}")]
        public async Task<IActionResult> getAllBisnisUnitDataTable(string param)
        {
            ResultModel<List<BisnisUnit>> res = new ResultModel<List<BisnisUnit>>();
            IActionResult actionResult = null;

            try
            {
                _http.BaseAddress = new Uri(_BPIApi);
                var result = await _http.GetFromJsonAsync<ResultModel<List<BisnisUnit>>>($"api/Facade/BPIBase/getAllBisnisUnitData/{param}");

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
                _http.BaseAddress = new Uri(_BPIApi);
                var result = await _http.GetFromJsonAsync<ResultModel<List<Department>>>($"api/Facade/BPIBase/getAllDepartmentData/{param}");

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
                _http.BaseAddress = new Uri(_BPIApi);
                var result = await _http.GetFromJsonAsync<ResultModel<List<Project>>>("api/Facade/BPIBase/getAllProjectData");

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
            ResultModel<List<BPIWebApplication.Shared.MainModel.Company.Category>> res = new();
            IActionResult actionResult = null;

            try
            {
                _http.BaseAddress = new Uri(_BPIApi);
                var result = await _http.GetFromJsonAsync<ResultModel<List<BPIWebApplication.Shared.MainModel.Company.Category>>>("api/Facade/BPIBase/getAllCategories");

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
                _http.BaseAddress = new Uri(_BPIApi);
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/Facade/BPIBase/isDepartmentDataPresent/{DeptID}");

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
                _http.BaseAddress = new Uri(_BPIApi);
                var result = await _http.GetFromJsonAsync<ResultModel<string>>($"api/Facade/BPIBase/isProjectPresent/{projectNo}");

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
                _http.BaseAddress = new Uri(_BPIApi);
                var result = await _http.PostAsJsonAsync<QueryModel<Department>>($"api/Facade/BPIBase/createNewDepartmentData", data);

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
                    res.ErrorMessage = "Fail Fetch data from createNewDepartmentData Facade";

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
                _http.BaseAddress = new Uri(_BPIApi);
                var result = await _http.PostAsJsonAsync<QueryModel<Project>>($"api/Facade/BPIBase/createNewProjectData", data);

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
                    res.ErrorMessage = "Fail Fetch data from createNewProjectData Facade";

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
                _http.BaseAddress = new Uri(_BPIApi);
                var result = await _http.PostAsJsonAsync<QueryModel<Department>>($"api/Facade/BPIBase/editDepartmentData", data);

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
                    res.ErrorMessage = "Fail Fetch data from editDepartmentData Facade";

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
                _http.BaseAddress = new Uri(_BPIApi);
                var result = await _http.PostAsJsonAsync<QueryModel<Project>>($"api/Facade/BPIBase/editProjectData", data);

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
                    res.ErrorMessage = "Fail Fetch data from editDepartmentData Facade";

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
