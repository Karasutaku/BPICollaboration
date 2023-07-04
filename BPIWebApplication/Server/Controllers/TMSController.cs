using BPIWebApplication.Shared.MainModel.EPKRS;
using BPIWebApplication.Shared.MainModel.FundReturn;
using BPIWebApplication.Shared.MainModel.POMF;
using BPIWebApplication.Shared.MainModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Net.Http.Json;

namespace BPIWebApplication.Server.Controllers
{
    [ApiController]
    [Route("api/endUser/TMS")]
    public class TMSController : Controller
    {
        private readonly HttpClient _http;
        private readonly string _smsApi, _frameworkApi, _facade;
        private readonly int _appId;

        public TMSController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _smsApi = config.GetValue<string>("ConnectionStrings:SmsApi");
            _frameworkApi = config.GetValue<string>("ConnectionStrings:FrameworkApi");
            _facade = config.GetValue<string>("ConnectionStrings:BpiFacade");
            _appId = config.GetValue<int>("AppIdentity:ID");
        }

        [HttpPost("getDetailsItembyLMNo")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> getDetailsItembyLMNo([FromHeader(Name = "Authorization")] string token, LMNotoTMS data)
        {
            ResultModel<List<LoadingManifestResp>> res = new ResultModel<List<LoadingManifestResp>>();
            IActionResult actionResult = null;

            try
            {
                _http.BaseAddress = new Uri(_smsApi);
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token.Split(" ")[0], token.Split(" ")[1]);
                var result = await _http.PostAsJsonAsync<LMNotoTMS>("TMS/GetDetailsItemByLMNo", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<TMSResultModel>();

                    if (respBody.errorResult.errorCode.Equals("00"))
                    {
                        //var temp = respBody.data.Replace(@"\", string.Empty);

                        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(respBody.data)))
                        {
                            // Deserialization from string JSON
                            //DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(List<LoadingManifestResp>));
                            //var dt = (List<LoadingManifestResp>) deserializer.ReadObject(ms);

                            res.Data = JsonSerializer.Deserialize<List<LoadingManifestResp>>(ms);

                            //res.Data = dt == null ? new() : dt;
                        }

                        res.isSuccess = true;
                        res.ErrorCode = respBody.errorResult.errorCode;
                        res.ErrorMessage = respBody.errorResult.errorMessage;

                        actionResult = Ok(res);
                    }
                    else
                    {
                        res.Data = new();
                        res.isSuccess = false;
                        res.ErrorCode = respBody.errorResult.errorCode;
                        res.ErrorMessage = respBody.errorResult.errorMessage;

                        actionResult = Ok(respBody);
                    }
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<TMSResultModel>();

                    res.Data = null;
                    res.isSuccess = false;
                    res.ErrorCode = respBody.errorResult.errorCode;
                    res.ErrorMessage = respBody.errorResult.errorMessage;

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

        [HttpPost("getDetailsItemByReceiptNo")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> getDetailsItemByReceiptNo([FromHeader(Name = "Authorization")] string token, ReceiptNotoTMS data)
        {
            ResultModel<List<ReceiptNoResp>> res = new ResultModel<List<ReceiptNoResp>>();
            IActionResult actionResult = null;

            try
            {
                _http.BaseAddress = new Uri(_smsApi);
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token.Split(" ")[0], token.Split(" ")[1]);
                var result = await _http.PostAsJsonAsync<ReceiptNotoTMS>("TMS/GetDetailsItemByReceiptNo", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<TMSResultModel>();

                    if (respBody.errorResult.errorCode.Equals("00"))
                    {
                        //var temp = respBody.data.Replace(@"\", string.Empty);

                        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(respBody.data)))
                        {
                            // Deserialization from string JSON
                            //DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(List<ReceiptNoResp>));
                            //var dt = (List<ReceiptNoResp>)deserializer.ReadObject(ms);

                            res.Data = JsonSerializer.Deserialize<List<ReceiptNoResp>>(ms);

                            //res.Data = dt == null ? new() : dt;
                        }

                        res.isSuccess = true;
                        res.ErrorCode = respBody.errorResult.errorCode;
                        res.ErrorMessage = respBody.errorResult.errorMessage;

                        actionResult = Ok(res);
                    }
                    else
                    {
                        res.Data = new();
                        res.isSuccess = false;
                        res.ErrorCode = respBody.errorResult.errorCode;
                        res.ErrorMessage = respBody.errorResult.errorMessage;

                        actionResult = Ok(respBody);
                    }
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<TMSResultModel>();

                    res.Data = null;
                    res.isSuccess = false;
                    res.ErrorCode = respBody.errorResult.errorCode;
                    res.ErrorMessage = respBody.errorResult.errorMessage;

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

        [HttpPost("getDetailsItemByReceiptNoAndNPNo")]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> getDetailsItemByReceiptNoAndNPNo([FromHeader(Name = "Authorization")] string token, NPwithReceiptNotoTMS data)
        {
            ResultModel<List<NPwithReceiptNoResp>> res = new ResultModel<List<NPwithReceiptNoResp>>();
            res.Data = new();
            IActionResult actionResult = null;

            try
            {
                _http.BaseAddress = new Uri(_smsApi);
                _http.DefaultRequestHeaders.Clear();
                _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token.Split(" ")[0], token.Split(" ")[1]);
                var result = await _http.PostAsJsonAsync<NPwithReceiptNotoTMS>("TMS/GetDetailsItemByReceiptNoAndNPNo", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<TMSResultModel>();

                    if (respBody.errorResult.errorCode.Equals("00"))
                    {
                        //var temp = respBody.data.Replace(@"\", string.Empty);

                        using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(respBody.data)))
                        {
                            // Deserialization from string JSON
                            //DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(List<NPwithReceiptNoResp>));
                            //var dt = (List<NPwithReceiptNoResp>)deserializer.ReadObject(ms);

                            res.Data = JsonSerializer.Deserialize<List<NPwithReceiptNoResp>>(ms);

                            //res.Data = dt == null ? new() : dt;
                        }

                        res.isSuccess = true;
                        res.ErrorCode = respBody.errorResult.errorCode;
                        res.ErrorMessage = respBody.errorResult.errorMessage;

                        actionResult = Ok(res);
                    }
                    else
                    {
                        res.Data = new();
                        res.isSuccess = false;
                        res.ErrorCode = respBody.errorResult.errorCode;
                        res.ErrorMessage = respBody.errorResult.errorMessage;

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

        //
    }
}
