﻿using BPIBR.Models.DbModel;
using BPIBR.Models.MainModel;
using BPIBR.Models.MainModel.POMF;
using Microsoft.AspNetCore.Mvc;

namespace BPIBR.Controllers
{
    [Route("api/BR/POMF")]
    [ApiController]
    public class POMFBR : ControllerBase
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;
        //private readonly string _uploadPath;

        public POMFBR(HttpClient http, IConfiguration config)
        {
            _http = http;
            _configuration = config;
            _http.BaseAddress = new Uri(_configuration.GetValue<string>("BaseUri:BpiDA"));
            //_uploadPath = _configuration.GetValue<string>("File:EPKRS:UploadPath");
        }

        [HttpPost("createPOMFDocument")]
        public async Task<IActionResult> createPOMFDocument(QueryModel<POMFDocument> data)
        {
            ResultModel<QueryModel<POMFDocument>> res = new ResultModel<QueryModel<POMFDocument>>();
            IActionResult actionResult = null;

            try
            {
                var result = await _http.PostAsJsonAsync<QueryModel<POMFDocument>>("api/DA/POMF/createPOMFDocument", data);

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
                var result = await _http.PostAsJsonAsync<QueryModel<POMFApprovalStream>>("api/DA/POMF/createPOMFApproval", data);

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
                var result = await _http.PostAsJsonAsync<QueryModel<POMFApprovalStreamExtended>>("api/DA/POMF/createPOMFApprovalExtended", data);

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
                var result = await _http.PostAsJsonAsync<QueryModel<string>>("api/DA/POMF/deletePOMFDocument", data);

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<POMFDocument>>>($"api/DA/POMF/getPOMFDocuments/{param}");

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<POMFItemLinesMaxQuantity>>>($"api/DA/POMF/getPOMFItemLineMaxQuantity/{param}");

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
                var result = await _http.GetFromJsonAsync<ResultModel<List<POMFNPType>>>("api/DA/POMF/getPOMFNPType");

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
                var result = await _http.GetFromJsonAsync<ResultModel<int>>($"api/DA/POMF/getPOMFModuleNumberOfPage/{param}");

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
