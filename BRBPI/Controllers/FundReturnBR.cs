using BPIBR.Models.DbModel;
using BPIBR.Models.MainModel;
using BPIBR.Models.MainModel.Company;
using BPIBR.Models.MainModel.FundReturn;
using BPIBR.Models.MainModel.POMF;
using BPILibrary;
using Microsoft.AspNetCore.Mvc;

namespace BPIBR.Controllers
{
    [Route("api/BR/FundReturn")]
    [ApiController]
    public class FundReturnBR : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        private readonly string _uploadPath;

        public FundReturnBR(HttpClient http, IConfiguration config)
        {
            _http = http;
            _configuration = config;
            _http.BaseAddress = new Uri(_configuration.GetValue<string>("BaseUri:BpiDA"));
            _uploadPath = _configuration.GetValue<string>("File:FundReturn:UploadPath");
        }

        [HttpPost("createFundReturnDocument")]
        public async Task<IActionResult> createFundReturnDocument(FundReturnUploadStream data)
        {
            ResultModel<FundReturnUploadStream> res = new ResultModel<FundReturnUploadStream>();
            res.Data = new();
            res.Data.mainData = new();
            IActionResult actionResult = null;

            try
            {
                HttpResponseMessage? result = null;
                result = new();

                if (data.mainData.Data.dataHeader.FundReturnCategoryID.Equals("XNTF"))
                {
                    result = await _http.PostAsJsonAsync<QueryModel<FundReturnDocument>>("api/DA/FundReturn/createFundReturnDocument", data.mainData);
                }
                else
                {
                    result = await _http.PostAsJsonAsync<QueryModel<FundReturnDocument>>("api/DA/FundReturn/createFundReturnHeader", data.mainData);
                }

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<FundReturnDocument>>>();

                    if (respBody.isSuccess)
                    {
                        data.files.ForEach(x =>
                        {
                            string path = Path.Combine(_uploadPath, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), respBody.Data.Data.dataHeader.DocumentID, x.fileName);

                            CommonLibrary.saveFiletoDirectory(path, x.content);
                        });
                    }

                    res.Data.mainData = respBody.Data;
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
                var result = await _http.PostAsJsonAsync<QueryModel<FundReturnApprovalStream>>("api/DA/FundReturn/createFundReturnApproval", data);

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
                var result = await _http.PostAsJsonAsync<QueryModel<string>>("api/DA/FundReturn/deleteFundReturnDocument", data);

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<FundReturnDocument>>>($"api/DA/FundReturn/getFundReturnDocuments/{param}");

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<Bank>>>("api/DA/FundReturn/getFundReturnBankData");

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<FundReturnCategory>>>("api/DA/FundReturn/getFundReturnCategory");

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

        [HttpGet("getFundReturnFileStream/{param}")]
        public async Task<IActionResult> getEPKRSFileStream(string param)
        {
            ResultModel<List<BPIBR.Models.MainModel.Stream.FileStream>> res = new ResultModel<List<BPIBR.Models.MainModel.Stream.FileStream>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<FundReturnAttachment>>>($"api/DA/FundReturn/getFundReturnAttachment/{param}");

                if (result.isSuccess)
                {
                    res.Data = new();

                    result.Data.ForEach(x =>
                    {
                        res.Data.Add(new BPIBR.Models.MainModel.Stream.FileStream
                        {
                            type = "Attachment",
                            fileName = x.FilePath,
                            fileType = x.FileExtension,
                            fileSize = 0,
                            content = CommonLibrary.getFileStream(_uploadPath, "", x.FilePath, x.UploadDate, CommonLibrary.Base64Decode(param).Split("!_!")[1])
                        });
                    });

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
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/DA/FundReturn/getFundReturnModuleNumberOfPage/{param}");

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
