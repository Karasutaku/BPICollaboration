using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.Login;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace BPIWebApplication.Server.Controllers
{
    [ApiController]
    [Route("api/endUser/Login")]
    public class LoginController : Controller
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly string _smsApi, _frameworkApi;
        private readonly int _appId;

        public LoginController(HttpClient http, IConfiguration config, IHttpContextAccessor httpAccessor)
        {
            _http = http;
            _httpAccessor = httpAccessor;
            _smsApi = config.GetValue<string>("ConnectionStrings:SmsApi");
            _frameworkApi = config.GetValue<string>("ConnectionStrings:FrameworkApi");
            _appId = config.GetValue<int>("AppIdentity:ID");
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> authenticateToITFacade(FacadeLogin data)
        {
            ResultModel<FacadeLoginResponse> res = new ResultModel<FacadeLoginResponse>();
            IActionResult actionResult = null;

            try
            {
                //data.ipClient = _httpAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                FacadeLogin loginData = new();
                loginData = data;
                loginData.fromApplicationId = _appId;

                _http.BaseAddress = new Uri(_smsApi);
                var result = await _http.PostAsJsonAsync<FacadeLogin>("Api/Login/AuthenticatebyUserNamePassword", loginData);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<FacadeLoginResponse>();

                    if (!respBody.token.IsNullOrEmpty())
                    {
                        res.Data = respBody;
                        res.isSuccess = true;
                        res.ErrorCode = "00";
                        res.ErrorMessage = "Login Success";

                        actionResult = Ok(res);
                    }
                    else
                    {
                        res.Data = respBody;
                        res.isSuccess = false;
                        res.ErrorCode = "01";
                        res.ErrorMessage = "Login Failed";

                        actionResult = Ok(respBody);
                    }
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<FacadeLoginResponse>();

                    res.Data = respBody;
                    res.isSuccess = false;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Failed HTTP Request to Framework !";

                    actionResult = Ok(respBody);
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

        [HttpPost("getUserModule")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> getModuleToITFacade([FromHeader(Name = "Authorization")] string token, FacadeUserModule data)
        {
            ResultModel<List<FacadeUserModuleResp>> res = new ResultModel<List<FacadeUserModuleResp>>();
            IActionResult actionResult = null;

            try
            {
                //data.SoapHeader.ipclient = _httpAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

                _http.BaseAddress = new Uri(_frameworkApi);
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token.Split(" ")[0], token.Split(" ")[1]);
                var result = await _http.PostAsJsonAsync<FacadeUserModule>("Api/Module/GetUserModule", data);

                if (result.IsSuccessStatusCode)
                {

                    var respBody = await result.Content.ReadFromJsonAsync<List<FacadeUserModuleResp>>();

                    if (respBody.Any())
                    {
                        res.Data = respBody;

                        res.isSuccess = result.IsSuccessStatusCode;
                        res.ErrorCode = "00";
                        res.ErrorMessage = "Fetch Module Success";

                        actionResult = Ok(res);
                    }
                    else
                    {
                        res.Data = respBody;
                        // res.Data.token = string.Empty;

                        res.isSuccess = false;
                        res.ErrorCode = "01";
                        res.ErrorMessage = "Fetch Module Failed";

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

        [HttpPost("getUserPrivileges")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> getPrivilegesToITFacade([FromHeader(Name = "Authorization")] string token, UserPrivileges data)
        {
            ResultModel<UserPrivilegesResp> res = new ResultModel<UserPrivilegesResp>();
            IActionResult actionResult = null;

            try
            {
                _http.BaseAddress = new Uri(_frameworkApi);
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token.Split(" ")[0], token.Split(" ")[1]);
                var result = await _http.PostAsJsonAsync<UserPrivileges>("Api/Module/UserAuthorize", data);

                if (result.IsSuccessStatusCode)
                {
                    //var x = await result.Content.ReadAsStringAsync();
                    //var c = JsonSerializer.Deserialize<FacadeLoginResponse>(x, new JsonSerializerOptions() { PropertyNameCaseInsensitive=true });

                    var respBody = await result.Content.ReadFromJsonAsync<UserPrivilegesResp>();

                    if (respBody.privileges.Any())
                    {
                        res.Data = respBody;

                        res.isSuccess = result.IsSuccessStatusCode;
                        res.ErrorCode = "00";
                        res.ErrorMessage = "Fetch Privileges Success";

                        actionResult = Ok(res);
                    }
                    else
                    {
                        res.Data = respBody;
                        // res.Data.token = string.Empty;

                        res.isSuccess = false;
                        res.ErrorCode = "01";
                        res.ErrorMessage = "No Privileges Available";

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

    }
}
