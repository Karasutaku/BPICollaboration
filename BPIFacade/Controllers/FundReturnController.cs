using BPIFacade.Models.DbModel;
using BPIFacade.Models.MainModel;
using BPIFacade.Models.MainModel.Company;
using BPIFacade.Models.MainModel.FundReturn;
using BPIFacade.Models.MainModel.POMF;
using Microsoft.AspNetCore.Mvc;

namespace BPIFacade.Controllers
{
    [Route("api/Facade/FundReturn")]
    [ApiController]
    public class FundReturnController : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;

        public FundReturnController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _configuration = config;
            _http.BaseAddress = new Uri(_configuration.GetValue<string>("BaseUri:BpiBR"));
        }

        [HttpPost("createFundReturnDocument")]
        public async Task<IActionResult> createFundReturnDocument(QueryModel<FundReturnDocument> data)
        {
            ResultModel<QueryModel<FundReturnDocument>> res = new ResultModel<QueryModel<FundReturnDocument>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<FundReturnDocument>>("api/BR/FundReturn/createFundReturnDocument", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<FundReturnDocument>>>();

                    res.Data = respBody.Data;
                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<FundReturnDocument>>>();

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

        [HttpPost("createFundReturnApproval")]
        public async Task<IActionResult> createFundReturnApproval(QueryModel<FundReturnApprovalStream> data)
        {
            ResultModel<QueryModel<FundReturnApprovalStream>> res = new ResultModel<QueryModel<FundReturnApprovalStream>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<FundReturnApprovalStream>>("api/BR/FundReturn/createFundReturnApproval", data);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<FundReturnApprovalStream>>>();

                    res.Data = respBody.Data;
                    res.isSuccess = respBody.isSuccess;
                    res.ErrorCode = respBody.ErrorCode;
                    res.ErrorMessage = respBody.ErrorMessage;

                    actionResult = Ok(res);
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

        [HttpPost("deleteFundReturnDocument")]
        public async Task<IActionResult> deleteFundReturnDocument(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<string>>("api/BR/FundReturn/deleteFundReturnDocument", data);

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

        [HttpGet("getFundReturnDocuments/{param}")]
        public async Task<IActionResult> getFundReturnDocuments(string param)
        {
            ResultModel<List<FundReturnDocument>> res = new ResultModel<List<FundReturnDocument>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<FundReturnDocument>>>($"api/BR/FundReturn/getFundReturnDocuments/{param}");

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

        [HttpGet("getFundReturnBankData")]
        public async Task<IActionResult> getFundReturnBank()
        {
            ResultModel<List<Bank>> res = new ResultModel<List<Bank>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<Bank>>>("api/BR/FundReturn/getFundReturnBankData");

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

        [HttpGet("getFundReturnCategory")]
        public async Task<IActionResult> getFundReturnCategory()
        {
            ResultModel<List<FundReturnCategory>> res = new ResultModel<List<FundReturnCategory>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<FundReturnCategory>>>("api/BR/FundReturn/getFundReturnCategory");

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

        [HttpGet("getFundReturnModuleNumberOfPage/{param}")]
        public async Task<IActionResult> getFundReturnModuleNumberOfPage(string param)
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/BR/FundReturn/getFundReturnModuleNumberOfPage/{param}");

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
