using BPILibrary;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.POMF;
using Microsoft.AspNetCore.Mvc;

namespace BPIWebApplication.Server.Controllers
{
    [Route("api/endUser/POMF")]
    [ApiController]
    public class POMFController : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        private readonly string[] _acceptedReqDocPrefix, _acceptedRecDocPrefix;
        private readonly int _minDocLength, _maxDocLength;

        public POMFController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _configuration = config;
            _http.BaseAddress = new Uri(_configuration.GetValue<string>("ConnectionStrings:BpiFacade"));
            _acceptedReqDocPrefix = config.GetValue<string>("File:POMF:AcceptedReqDocPrefix").Split("|");
            _acceptedRecDocPrefix = config.GetValue<string>("File:POMF:AcceptedRecDocPrefix").Split("|");
            _minDocLength = config.GetValue<int>("File:POMF:AcceptedMinDocLength");
            _maxDocLength = config.GetValue<int>("File:POMF:AcceptedMaxDocLength");
        }

        [HttpPost("createPOMFDocument")]
        public async Task<IActionResult> createPOMFDocument(QueryModel<POMFDocument> data)
        {
            ResultModel<QueryModel<POMFDocument>> res = new ResultModel<QueryModel<POMFDocument>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<POMFDocument>>("api/Facade/POMF/createPOMFDocument", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<POMFDocument>>>();

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
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<POMFDocument>>>();

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

        [HttpPost("createPOMFApproval")]
        public async Task<IActionResult> createPOMFApproval(QueryModel<POMFApprovalStream> data)
        {
            ResultModel<QueryModel<POMFApprovalStream>> res = new ResultModel<QueryModel<POMFApprovalStream>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<POMFApprovalStream>>("api/Facade/POMF/createPOMFApproval", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<POMFApprovalStream>>>();

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
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<POMFApprovalStream>>>();

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

        [HttpPost("createPOMFApprovalExtended")]
        public async Task<IActionResult> createPOMFApprovalExtended(QueryModel<POMFApprovalStreamExtended> data)
        {
            ResultModel<QueryModel<POMFApprovalStreamExtended>> res = new ResultModel<QueryModel<POMFApprovalStreamExtended>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<POMFApprovalStreamExtended>>("api/Facade/POMF/createPOMFApprovalExtended", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<POMFApprovalStreamExtended>>>();

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
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<POMFApprovalStreamExtended>>>();

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

        [HttpPost("deletePOMFDocument")]
        public async Task<IActionResult> deletePOMFDocument(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<string>>("api/Facade/POMF/deletePOMFDocument", data);

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

        [HttpGet("getPOMFDocuments/{param}")]
        public async Task<IActionResult> getPOMFDocuments(string param)
        {
            ResultModel<List<POMFDocument>> res = new ResultModel<List<POMFDocument>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<POMFDocument>>>($"api/Facade/POMF/getPOMFDocuments/{param}");

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

        [HttpGet("getPOMFItemLineMaxQuantity/{param}")]
        public async Task<IActionResult> getPOMFItemLineMaxQuantity(string param)
        {
            ResultModel<List<POMFItemLinesMaxQuantity>> res = new ResultModel<List<POMFItemLinesMaxQuantity>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<POMFItemLinesMaxQuantity>>>($"api/Facade/POMF/getPOMFItemLineMaxQuantity/{param}");

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

        [HttpGet("getPOMFNPType")]
        public async Task<IActionResult> getPOMFNPType()
        {
            ResultModel<List<POMFNPType>> res = new ResultModel<List<POMFNPType>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<POMFNPType>>>("api/Facade/POMF/getPOMFNPType");

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

        [HttpGet("getPOMFModuleNumberOfPage/{param}")]
        public async Task<IActionResult> getPOMFModuleNumberOfPage(string param)
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/Facade/POMF/getPOMFModuleNumberOfPage/{param}");

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

        [HttpGet("getPOMFAcceptedDocPrefix/{param}")]
        public async Task<IActionResult> getPOMFAcceptedDocPrefix(string param)
        {
            ResultModel<string[]> res = new ResultModel<string[]>();
            IActionResult actionResult = null;

            try
            {
                string temp = CommonLibrary.Base64Decode(param);

                if (!(_acceptedRecDocPrefix.Count() > 0))
                    throw new Exception("No Document Prefix found !");

                if (temp.Equals("Request"))
                {
                    res.Data = _acceptedReqDocPrefix;
                }
                else if (temp.Equals("Receive"))
                {
                    res.Data = _acceptedRecDocPrefix;
                }

                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);

            }
            catch (Exception ex)
            {
                res.Data = new string[] { };
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("getPOMFAcceptedDocLength/{param}")]
        public async Task<IActionResult> getPOMFAcceptedDocLength(string param)
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                string temp = CommonLibrary.Base64Decode(param);

                if (!(_acceptedRecDocPrefix.Count() > 0))
                    throw new Exception("No Document Prefix found !");

                if (temp.Equals("Min"))
                {
                    res.Data = _minDocLength;
                }
                else if (temp.Equals("Max"))
                {
                    res.Data = _maxDocLength;
                }

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
