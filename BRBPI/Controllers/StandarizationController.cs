using BPIBR.Models.DbModel;
using BPIBR.Models.MainModel;
using BPIBR.Models.MainModel.Standarizations;
using BPILibrary;
using Microsoft.AspNetCore.Mvc;

namespace BPIBR.Controllers
{
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

        // data processing

        internal byte[] getFileStream(string type, string filename, DateTime date)
        {
            string path = Path.Combine(_uploadPath, type, date.Year.ToString(), date.Month.ToString(), date.Day.ToString(), Path.GetFileName(filename));

            return System.IO.File.ReadAllBytes(path);
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

                    if (respBody.isSuccess)
                    {
                        foreach (var file in data.files)
                        {
                            if (_compressedFileExtensions.Any(y => y.Equals(file.fileType)))
                            //if (file.fileName.Contains(".mp4"))
                            {
                                byte[] tempContent = new byte[0];
                                string oriPath = Path.Combine(_uploadPath, data.standarizationDetails.Data.TypeID, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), file.fileName);
                                string zipPath = Path.ChangeExtension(oriPath, ".zip");

                                tempContent = CommonLibrary.decompressData(file.content);

                                CommonLibrary.saveFiletoDirectoryAsZip(zipPath, oriPath, tempContent);
                            }
                            else
                            {
                                string path = Path.Combine(_uploadPath, data.standarizationDetails.Data.TypeID, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), file.fileName);

                                CommonLibrary.saveFiletoDirectory(path, file.content);
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
                        res.Data = respBody.Data;
                        res.isSuccess = respBody.isSuccess;
                        res.ErrorCode = respBody.ErrorCode;
                        res.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(res);
                    }
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Standarizations>>>();

                    res.Data = respBody.Data;
                    res.isSuccess = respBody.isSuccess;
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

        [HttpPost("editStandarizationData")]
        public async Task<IActionResult> editStandarizationDataTable(StandarizationStream data)
        {
            ResultModel<QueryModel<Standarizations>> res = new ResultModel<QueryModel<Standarizations>>();
            IActionResult actionResult = null;

            try
            {
                string param = CommonLibrary.Base64Encode(data.standarizationDetails.Data.StandarizationID + "!_!" + data.standarizationDetails.Data.TypeID);

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

                    if (respBody.isSuccess)
                    {
                        foreach (var file in data.files)
                        {
                            if (_compressedFileExtensions.Any(y => y.Equals(file.fileType)))
                            {
                                byte[] tempContent = new byte[0];
                                string oriPath = Path.Combine(_uploadPath, data.standarizationDetails.Data.TypeID, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), file.fileName);
                                string zipPath = Path.ChangeExtension(oriPath, ".zip");

                                tempContent = CommonLibrary.decompressData(file.content);

                                CommonLibrary.saveFiletoDirectoryAsZip(zipPath, oriPath, tempContent);
                            }
                            else
                            {
                                string path = Path.Combine(_uploadPath, data.standarizationDetails.Data.TypeID, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), file.fileName);

                                CommonLibrary.saveFiletoDirectory(path, file.content);
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
                        res.Data = respBody.Data;

                        res.isSuccess = respBody.isSuccess;
                        res.ErrorCode = respBody.ErrorCode;
                        res.ErrorMessage = respBody.ErrorMessage;

                        actionResult = Ok(res);
                    }
                }
                else
                {
                    var respBody = await result.Content.ReadFromJsonAsync<ResultModel<QueryModel<Standarizations>>>();

                    res.Data = respBody.Data;

                    res.isSuccess = respBody.isSuccess;
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

                    string tp = CommonLibrary.Base64Decode(data.Data);

                    foreach (var dt in result1.Data)
                    {
                        deleteFilefromDirectory(tp.Split("!_!")[1], dt.FilePath, dt.UploadDate);
                    }
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = false;
                    res.ErrorCode = "99";
                    res.ErrorMessage = "Fail Fetch File Data Path";

                    actionResult = BadRequest(res);
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
                string dcParam = CommonLibrary.Base64Decode(param);

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
                        {
                            string oriPath = dt.FilePath;
                            string zipPath = Path.ChangeExtension(oriPath, ".zip");

                            byte[] tempContent = new byte[0];
                            tempContent = getFileStream(dcParam.Split("!_!")[1], zipPath, dt.UploadDate);

                            temp.content = CommonLibrary.getFileStreamfromZip(zipPath, oriPath, tempContent);
                            temp.content = CommonLibrary.compressData(temp.content);
                        }
                        else
                        {
                            temp.content = getFileStream(dcParam.Split("!_!")[1], dt.FilePath, dt.UploadDate);
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
}
