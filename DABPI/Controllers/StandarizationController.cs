using BPIDA.DataAccess;
using BPIDA.Models.DbModel;
using BPIDA.Models.MainModel;
using BPIDA.Models.MainModel.Standarizations;
using BPILibrary;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BPIDA.Controllers
{
    [Route("api/DA/Standarization")]
    [ApiController]
    public class StandarizationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StandarizationController(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpPost("createStandarizationData")]
        public async Task<IActionResult> createStandarizationDataTable(QueryModel<Standarizations> data)
        {
            ResultModel<QueryModel<Standarizations>> res = new ResultModel<QueryModel<Standarizations>>();
            StandarizationDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                List<StandarizationTag> tags = new();

                foreach (var tag in data.Data.Tags)
                {
                    tags.Add(new StandarizationTag
                    {
                        rowGuid = Guid.NewGuid(),
                        StandarizationID = tag.StandarizationID,
                        TagDescriptions = tag.TagDescriptions
                    });
                }

                if (da.createStandarizationDocument(
                    data
                    , CommonLibrary.ListToDataTable<StandarizationTag>(tags, data.userEmail, data.userAction, data.userActionDate, "Tags")
                    , CommonLibrary.ListToDataTable<StandarizationAttachment>(data.Data.Attachments, data.userEmail, data.userAction, data.userActionDate, "Attachments"))
                )
                {
                    res.Data = data;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = data;
                    res.isSuccess = false;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "SP Failed to Execute !";

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
        public async Task<IActionResult> editStandarizationDataTable(QueryModel<Standarizations> data)
        {
            ResultModel<QueryModel<Standarizations>> res = new ResultModel<QueryModel<Standarizations>>();
            StandarizationDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                da.deleteStandarizationData(data.Data.StandarizationID, data.userEmail, data.userAction, data.userActionDate);

                List<StandarizationTag> tags = new();

                foreach (var tag in data.Data.Tags)
                {
                    tags.Add(new StandarizationTag
                    {
                        rowGuid = Guid.NewGuid(),
                        StandarizationID = tag.StandarizationID,
                        TagDescriptions = tag.TagDescriptions
                    });
                }

                if (da.createStandarizationDocument(
                    data
                    , CommonLibrary.ListToDataTable<StandarizationTag>(tags, data.userEmail, data.userAction, data.userActionDate, "Tags")
                    , CommonLibrary.ListToDataTable<StandarizationAttachment>(data.Data.Attachments, data.userEmail, data.userAction, data.userActionDate, "Attachments"))
                )
                {
                    res.Data = data;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = data;
                    res.isSuccess = false;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "SP Failed to Execute !";

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
            StandarizationDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                string temp = CommonLibrary.Base64Decode(data.Data);

                bool flag = da.deleteStandarizationData(temp.Split("!_!")[0], data.userEmail, data.userAction, data.userActionDate);

                if (flag)
                {
                    res.Data = data;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = data;
                    res.isSuccess = false;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Data Might Already Deleted";

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
            StandarizationDA da = new(_configuration);
            List<StandarizationType> typeLines = new List<StandarizationType>();
            DataTable dtType = new DataTable("Shift");
            IActionResult actionResult = null;

            try
            {
                dtType = da.getStandarizationTypeData();

                if (dtType.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtType.Rows)
                    {
                        StandarizationType temp = new StandarizationType();

                        temp.TypeID = dt["TypeID"].ToString();
                        temp.Descriptions = dt["Descriptions"].ToString();

                        typeLines.Add(temp);
                    }

                    res.Data = typeLines;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fetch Empty";

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
            StandarizationDA da = new(_configuration);
            List<Standarizations> standarizationLines = new List<Standarizations>();
            List<StandarizationTag> tagLines = new List<StandarizationTag>();
            List<StandarizationAttachment> attachLines = new List<StandarizationAttachment>();
            DataTable dtStandarization = new DataTable("Standarizations");
            DataTable dtTag = new DataTable("Tags");
            DataTable dtAttach = new DataTable("Attachments");
            IActionResult actionResult = null;

            try
            {
                string temp = CommonLibrary.Base64Decode(param);

                //string id = temp.Split("!_!")[0];
                string cond = temp.Split("!_!")[0];
                int pageNo = Convert.ToInt32(temp.Split("!_!")[1]);

                dtStandarization = da.getStandarizationData(cond, pageNo);

                if (dtStandarization.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtStandarization.Rows)
                    {
                        Standarizations temp1 = new Standarizations();

                        temp1.TypeID = dt["TypeID"].ToString();
                        temp1.StandarizationID = dt["StandarizationID"].ToString();
                        temp1.StandarizationDetails = dt["StandarizationDetails"].ToString();
                        temp1.StandarizationDate = Convert.ToDateTime(dt["StandarizationDate"]);

                        temp1.Tags = new();
                        temp1.Attachments = new();

                        dtTag = da.getStandarizationTagData(dt["StandarizationID"].ToString());

                        if (dtTag.Rows.Count > 0)
                        {
                            foreach (DataRow rTag in dtTag.Rows)
                            {
                                StandarizationTag temp2 = new();

                                temp2.StandarizationID = rTag["StandarizationID"].ToString();
                                temp2.TagDescriptions = rTag["TagDescriptions"].ToString();

                                temp1.Tags.Add(temp2);
                            }
                        }

                        dtAttach = da.getStandarizationAttachmentData(dt["StandarizationID"].ToString());

                        if (dtAttach.Rows.Count > 0)
                        {
                            foreach (DataRow rAttach in dtAttach.Rows)
                            {
                                StandarizationAttachment temp3 = new();

                                temp3.StandarizationID = rAttach["StandarizationID"].ToString();
                                temp3.Descriptions = rAttach["Descriptions"].ToString();
                                temp3.UploadDate = Convert.ToDateTime(rAttach["UploadDate"]);
                                temp3.FileExtention = rAttach["FileExtention"].ToString();
                                temp3.FilePath = rAttach["FilePath"].ToString();

                                temp1.Attachments.Add(temp3);
                            }
                        }

                        standarizationLines.Add(temp1);
                    }

                    res.Data = standarizationLines;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fetch Empty";

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
        public async Task<IActionResult> getStandarizationAttachmentDataTable(string param)
        {
            ResultModel<List<StandarizationAttachment>> res = new ResultModel<List<StandarizationAttachment>>();
            StandarizationDA da = new(_configuration);
            List<StandarizationAttachment> standarizationLines = new List<StandarizationAttachment>();
            DataTable dtAttach = new DataTable("Attachment");
            IActionResult actionResult = null;

            try
            {
                string temp = CommonLibrary.Base64Decode(param);

                dtAttach = da.getStandarizationAttachmentData(temp.Split("!_!")[0]);

                if (dtAttach.Rows.Count > 0)
                {
                    foreach (DataRow rAttach in dtAttach.Rows)
                    {
                        StandarizationAttachment temp1 = new();

                        temp1.StandarizationID = rAttach["StandarizationID"].ToString();
                        temp1.Descriptions = rAttach["Descriptions"].ToString();
                        temp1.UploadDate = Convert.ToDateTime(rAttach["UploadDate"]);
                        temp1.FileExtention = rAttach["FileExtention"].ToString();
                        temp1.FilePath = rAttach["FilePath"].ToString();

                        standarizationLines.Add(temp1);
                    }

                    res.Data = standarizationLines;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fetch Empty";

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
            StandarizationDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                string tbname = CommonLibrary.Base64Decode(Table).Split("!_!")[0];
                string loc = CommonLibrary.Base64Decode(Table).Split("!_!")[1];
                string cond = CommonLibrary.Base64Decode(Table).Split("!_!")[2];

                res.Data = da.getModuleNumberOfPage(tbname, loc, cond);

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
