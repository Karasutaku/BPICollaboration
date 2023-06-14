using BPILibrary;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.Standarizations;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;

namespace BPIWebApplication.Server.Controllers
{
    [Route("api/endUser/Standarization")]
    [ApiController]
    public class StandarizationController : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        private readonly int _maxFileSize;
        private readonly string[] _compressedFileExtensions, _acceptedFileExtensions;

        public StandarizationController(HttpClient http, IConfiguration config)
        {
            _http = http;
            _configuration = config;
            _http.BaseAddress = new Uri(_configuration.GetValue<string>("ConnectionStrings:BpiFacade"));
            _maxFileSize = config.GetValue<int>("File:Standarizations:MaxUpload");
            _compressedFileExtensions = config.GetValue<string>("File:Standarizations:CompressedFileExtensions").Split("|");
            _acceptedFileExtensions = config.GetValue<string>("File:Standarizations:AcceptedFileExtensions").Split("|");
        }

        [HttpPost("createStandarizationData")]
        public async Task<IActionResult> createStandarizationDataTable(StandarizationStream data)
        {
            ResultModel<QueryModel<Standarizations>> res = new ResultModel<QueryModel<Standarizations>>();
            IActionResult actionResult = null;

            try
            {
                // compress file

                StandarizationStream processed = new();
                processed = data;

                processed.files.ForEach(x =>
                {
                    if (_compressedFileExtensions.Any(y => y.Equals(x.fileType)))
                    {
                        x.content = CommonLibrary.compressData(x.content);
                    }
                });

                var result = await _http.PostAsJsonAsync<StandarizationStream>($"api/Facade/Standarization/createStandarizationData", processed);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Standarizations>>>();

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
                    res.ErrorMessage = $"Fail settle from createStandarizationData Facade";

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
                StandarizationStream processed = new();
                processed = data;

                processed.files.ForEach(x =>
                {
                    //if (x.fileName.Contains(".mp4"))
                    //{
                    //    x.content = await compressData(x.content);
                    //}

                    if (_compressedFileExtensions.Any(y => y.Equals(x.fileType)))
                    {
                        x.content = CommonLibrary.compressData(x.content);
                    }
                });

                var result = await _http.PostAsJsonAsync<StandarizationStream>($"api/Facade/Standarization/editStandarizationData", processed);

                if (result.IsSuccessStatusCode)
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Standarizations>>>();

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
                    res.ErrorMessage = $"Fail settle from editStandarizationData Facade";

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
                var result = await _http.PostAsJsonAsync<QueryModel<string>>($"api/Facade/Standarization/deleteStandarizationData", data);

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
                    res.ErrorMessage = $"Fail settle from deleteStandarizationData Facade";

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<StandarizationType>>>("api/Facade/Standarization/getStandarizationTypes");

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<Standarizations>>>($"api/Facade/Standarization/getStandarizationData/{param}");

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
            ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>> res = new ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.GetFromJsonAsync<ResultModel<List<BPIWebApplication.Shared.MainModel.Stream.FileStream>>>($"api/Facade/Standarization/getStandarizationAttachment/{param}");

                if (result.isSuccess)
                {
                    List<BPIWebApplication.Shared.MainModel.Stream.FileStream> temp = new();
                    temp = result.Data;

                    temp.ForEach(async x =>
                    {
                        FileInfo fi = new FileInfo(x.fileName);

                        if (_compressedFileExtensions.Any(y => y.Equals(fi.Extension)))
                        {
                            x.content = CommonLibrary.decompressData(x.content);
                        }
                    });

                    res.Data = temp;

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
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/Facade/Standarization/getModulePageSize/{Table}");

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

        [HttpGet("getStandarizationMaxSizeUpload")]
        public async Task<IActionResult> getStandarizationMaxSizeUpload()
        {
            ResultModel<int> res = new ResultModel<int>();
            IActionResult actionResult = null;

            try
            {

                res.Data = _maxFileSize;

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

        [HttpGet("getStandarizationAcceptedFileExtension")]
        public async Task<IActionResult> getStandarizationAcceptedFileExtension()
        {
            ResultModel<string[]> res = new ResultModel<string[]>();
            IActionResult actionResult = null;

            try
            {

                res.Data = _acceptedFileExtensions;

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

        //
    }
}
