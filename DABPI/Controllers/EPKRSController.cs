using BPIDA.DataAccess;
using BPIDA.Models.DbModel;
using BPIDA.Models.MainModel;
using BPIDA.Models.MainModel.EPKRS;
using BPILibrary;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BPIDA.Controllers
{
    [Route("api/DA/EPKRS")]
    [ApiController]
    public class EPKRSController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EPKRSController(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpPost("createEPKRSItemCaseDocument")]
        public async Task<IActionResult> createEPKRSItemCaseDocumentData(QueryModel<EPKRSUploadItemCase> data)
        {
            ResultModel<QueryModel<EPKRSUploadItemCase>> res = new ResultModel<QueryModel<EPKRSUploadItemCase>>();
            GeneralDA generalDA = new(_configuration);
            EPKRSDA da = new(_configuration);
            DataTable dtMainIdentity = new DataTable("Identity");
            string id = string.Empty;
            IActionResult actionResult = null;

            try
            {

                dtMainIdentity = generalDA.createIDData("EPKRS");
                var x = dtMainIdentity.AsEnumerable().First();

                // 8 for date yyyyMMdd
                string zero = string.Empty;
                int idLength = x["Code"].ToString().Length + x["DocNumber"].ToString().Length + x["Parity"].ToString().Length + 8;
                int maxLength = 18;

                zero = new String('0', maxLength - idLength);

                id = x["Code"].ToString() +
                    DateTime.Now.Year.ToString() +
                    DateTime.Now.ToString("MM") +
                    DateTime.Now.ToString("dd") +
                    zero +
                    x["DocNumber"].ToString() +
                    x["Parity"].ToString();

                QueryModel<ItemCase> dtMain = new();
                dtMain.Data = new();

                dtMain.Data = data.Data.itemCase;
                dtMain.Data.DocumentID = id;
                dtMain.userEmail = data.userEmail;
                dtMain.userAction = data.userAction;
                dtMain.userActionDate = data.userActionDate;

                List<ItemLine> tempLine = new();
                data.Data.itemLine.ForEach(x =>
                {
                    tempLine.Add(new ItemLine
                    {
                        DocumentID = id,
                        LineNum = x.LineNum,
                        TRID = x.TRID,
                        TRDate = x.TRDate,
                        ItemCode = x.ItemCode,
                        ItemDescription = x.ItemDescription,
                        ItemRiskCategoryID = x.ItemRiskCategoryID,
                        CategoryID = x.CategoryID,
                        ItemQuantity = x.ItemQuantity,
                        UOM = x.UOM,
                        ItemValue = x.ItemValue,
                        ItemStock = x.ItemStock,
                        VarianceDate = x.VarianceDate,
                        isLate = x.isLate,
                        isCCTVCoverable = x.isCCTVCoverable,
                        isReportedtoSender = x.isReportedtoSender
                    });
                });

                List<CaseAttachment> dtAttachTemp = new();
                data.Data.attachment.ForEach(x =>
                {
                    dtAttachTemp.Add(new CaseAttachment
                    {
                        DocumentID = id,
                        LineNum = x.LineNum,
                        UploadDate = x.UploadDate,
                        FileExtension = x.FileExtension,
                        FilePath = x.FilePath
                    });
                });

                var dtLines = CommonLibrary.ListToDataTable<ItemLine>(tempLine, data.userEmail, data.userAction, data.userActionDate, "ItemCaseLines");
                var dtAttach = CommonLibrary.ListToDataTable<CaseAttachment>(dtAttachTemp, data.userEmail, data.userAction, data.userActionDate, "ItemCaseAttachment");

                if (da.createEPKRSItemCaseDocument(dtMain, dtLines, dtAttach))
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
                    res.ErrorMessage = "Create Data Failed ! SP Fail to Execute";

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

        [HttpPost("createEPKRSIncidentAccidentDocument")]
        public async Task<IActionResult> createEPKRSIncidentAccidentDocumentData(QueryModel<EPKRSUploadIncidentAccident> data)
        {
            ResultModel<QueryModel<EPKRSUploadIncidentAccident>> res = new ResultModel<QueryModel<EPKRSUploadIncidentAccident>>();
            GeneralDA generalDA = new(_configuration);
            EPKRSDA da = new(_configuration);
            DataTable dtMainIdentity = new DataTable("Identity");
            string id = string.Empty;
            IActionResult actionResult = null;

            try
            {

                dtMainIdentity = generalDA.createIDData("EPKRS");
                var x = dtMainIdentity.AsEnumerable().First();

                // 8 for date yyyyMMdd
                string zero = string.Empty;
                int idLength = x["Code"].ToString().Length + x["DocNumber"].ToString().Length + x["Parity"].ToString().Length + 8;
                int maxLength = 18;

                zero = new String('0', maxLength - idLength);

                id = x["Code"].ToString() +
                    DateTime.Now.Year.ToString() +
                    DateTime.Now.ToString("MM") +
                    DateTime.Now.ToString("dd") +
                    zero +
                    x["DocNumber"].ToString() +
                    x["Parity"].ToString();

                QueryModel<IncidentAccident> dtMain = new();
                dtMain.Data = new();

                dtMain.Data = data.Data.incidentAccident;
                dtMain.Data.DocumentID = id;
                dtMain.userEmail = data.userEmail;
                dtMain.userAction = data.userAction;
                dtMain.userActionDate = data.userActionDate;

                List<CaseAttachment> dtAttachTemp = new();
                data.Data.attachment.ForEach(x =>
                {
                    dtAttachTemp.Add(new CaseAttachment
                    {
                        DocumentID = id,
                        LineNum = x.LineNum,
                        UploadDate = x.UploadDate,
                        FileExtension = x.FileExtension,
                        FilePath = x.FilePath
                    });
                });
                var dtAttach = CommonLibrary.ListToDataTable<CaseAttachment>(dtAttachTemp, data.userEmail, data.userAction, data.userActionDate, "ItemCaseAttachment");

                if (da.createEPKRSIncidentAccidentDocument(dtMain, dtAttach))
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
                    res.ErrorMessage = "Create Data Failed ! SP Fail to Execute";

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

        [HttpPost("createEPKRSDocumentDiscussion")]
        public async Task<IActionResult> createEPKRSDocumentDiscussionData(QueryModel<EPKRSUploadDiscussion> data)
        {
            ResultModel<QueryModel<EPKRSUploadDiscussion>> res = new ResultModel<QueryModel<EPKRSUploadDiscussion>>();
            EPKRSDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                QueryModel<DocumentDiscussion> dt = new();
                dt.Data = new();

                dt.Data = data.Data.discussion;
                dt.userEmail = data.userEmail;
                dt.userAction = data.userAction;
                dt.userActionDate = data.userActionDate;

                if (da.createEPKRSDocumentDiscussion(dt, data.Data.LocationID))
                {
                    if (data.Data.attachment.Count > 0)
                    {
                        var dtAttach = CommonLibrary.ListToDataTable<CaseAttachment>(data.Data.attachment, data.userEmail, data.userAction, data.userActionDate, "Attachment");

                        if (da.createEPKRSCaseAttachment(dtAttach))
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
                            res.ErrorMessage = "Fail To Create Attachment data createEPKRSCaseAttachment to DB";

                            actionResult = Ok(res);
                        }
                    }
                    else
                    {
                        res.Data = data;
                        res.isSuccess = true;
                        res.ErrorCode = "00";
                        res.ErrorMessage = "";

                        actionResult = Ok(res);
                    }
                }
                else
                {
                    res.Data = data;
                    res.isSuccess = false;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Create Data Failed ! SP Fail to Execute";

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

        [HttpPost("createEPKRSDocumentDiscussionReadHistory")]
        public async Task<IActionResult> createEPKRSDocumentDiscussionReadHistory(QueryModel<DocumentDiscussionReadStream> data)
        {
            ResultModel<QueryModel<DocumentDiscussionReadStream>> res = new ResultModel<QueryModel<DocumentDiscussionReadStream>>();
            EPKRSDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                if (da.createEPKRSDocumentDiscussionReadHistoryData(data.Data.LocationID, CommonLibrary.ListToDataTable<DocumentDiscussionReadHistory>(data.Data.Data, data.userEmail, data.userAction, data.userActionDate, "tbParam")))
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
                    res.ErrorMessage = "Create Data Failed ! SP Fail to Execute";

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

        [HttpPost("createEPKRSDocumentApproval")]
        public async Task<IActionResult> createEPKRSDocumentApprovalData(QueryModel<DocumentApproval> data)
        {
            ResultModel<QueryModel<DocumentApproval>> res = new ResultModel<QueryModel<DocumentApproval>>();
            EPKRSDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                if (da.createEPKRSDocumentApproval(data))
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
                    res.ErrorMessage = "Create Data Failed ! SP Fail to Execute";

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

        [HttpPost("createEPKRSDocumentApprovalExtended")]
        public async Task<IActionResult> createEPKRSDocumentApprovalData(QueryModel<RISKApprovalExtended> data)
        {
            ResultModel<QueryModel<RISKApprovalExtended>> res = new ResultModel<QueryModel<RISKApprovalExtended>>();
            EPKRSDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                if (da.editEPKRSDocumentExtendedandApprovalData(data))
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
                    res.ErrorMessage = "Create Data Failed ! SP Fail to Execute";

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

        [HttpPost("editEPKRSItemCaseData")]
        public async Task<IActionResult> editEPKRSItemCase(QueryModel<EPKRSUploadItemCase> data)
        {
            ResultModel<QueryModel<EPKRSUploadItemCase>> res = new ResultModel<QueryModel<EPKRSUploadItemCase>>();
            EPKRSDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                var dtLines = CommonLibrary.ListToDataTable<ItemLine>(data.Data.itemLine, data.userEmail, data.userAction, data.userActionDate, "itemLines");

                if (da.editEPKRSItemCaseData(data, dtLines))
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
                    res.ErrorMessage = "Create Data Failed ! SP Fail to Execute";

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

        [HttpPost("editEPKRSIncidentAccidentData")]
        public async Task<IActionResult> editEPKRSIncidentAccident(QueryModel<IncidentAccident> data)
        {
            ResultModel<QueryModel<IncidentAccident>> res = new ResultModel<QueryModel<IncidentAccident>>();
            EPKRSDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                if (da.editEPKRSIncidentAccidentData(data))
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
                    res.ErrorMessage = "Edit Data Failed ! SP Fail to Execute";

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

        [HttpPost("deleteEPKRSItemCaseDocumentData")]
        public async Task<IActionResult> deleteEPKRSItemCaseDocumentData(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            EPKRSDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                string[] temp = CommonLibrary.Base64Decode(data.Data).Split("!_!");

                if (da.deleteEPKRSItemCaseDocument(temp[0], temp[1], data.userEmail, data.userAction, data.userActionDate))
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
                    res.ErrorMessage = "Edit Data Failed ! SP Fail to Execute";

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

        [HttpPost("deleteEPKRSIncidentAccidentDocumentData")]
        public async Task<IActionResult> deleteEPKRSIncidentAccidentDocumentData(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            EPKRSDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                string[] temp = CommonLibrary.Base64Decode(data.Data).Split("!_!");

                if (da.deleteEPKRSIncidentAccidentDocument(temp[0], temp[1], data.userEmail, data.userAction, data.userActionDate))
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
                    res.ErrorMessage = "Edit Data Failed ! SP Fail to Execute";

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

        [HttpGet("getEPRKSReportingType")]
        public async Task<IActionResult> getEPRKSReportingType()
        {
            ResultModel<List<ReportingType>> res = new ResultModel<List<ReportingType>>();
            EPKRSDA da = new(_configuration);
            List<ReportingType> reportingTypeLines = new List<ReportingType>();
            DataTable dtReportingType = new DataTable("ReportingType");
            IActionResult actionResult = null;

            try
            {
                dtReportingType = da.getEPKRSReportingTypeData();

                if (dtReportingType.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtReportingType.Rows)
                    {
                        ReportingType temp = new ReportingType();

                        temp.ReportingID = dt["ReportingID"].ToString();
                        temp.ReportingDescription = dt["ReportingDescription"].ToString();

                        reportingTypeLines.Add(temp);
                    }

                    res.Data = reportingTypeLines;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = false;
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

        [HttpGet("getEPKRSRiskType")]
        public async Task<IActionResult> getEPKRSRiskType()
        {
            ResultModel<List<RiskType>> res = new ResultModel<List<RiskType>>();
            EPKRSDA da = new(_configuration);
            List<RiskType> riskTypeLines = new List<RiskType>();
            DataTable dtRiskType = new DataTable("RiskType");
            IActionResult actionResult = null;

            try
            {
                dtRiskType = da.getEPKRSRiskTypeData();

                if (dtRiskType.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtRiskType.Rows)
                    {
                        RiskType temp = new RiskType();

                        temp.RiskID = dt["RiskID"].ToString();
                        temp.RiskDescription = dt["RiskDescription"].ToString();
                        temp.ReportingID = dt["ReportingID"].ToString();

                        riskTypeLines.Add(temp);
                    }

                    res.Data = riskTypeLines;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = false;
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

        [HttpGet("getEPKRSRiskSubType")]
        public async Task<IActionResult> getEPKRSRiskSubType()
        {
            ResultModel<List<RiskSubType>> res = new ResultModel<List<RiskSubType>>();
            EPKRSDA da = new(_configuration);
            List<RiskSubType> riskSubTypeLines = new List<RiskSubType>();
            DataTable dtRiskSubType = new DataTable("RiskSubType");
            IActionResult actionResult = null;

            try
            {
                dtRiskSubType = da.getEPKRSRiskSubTypeData();

                if (dtRiskSubType.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtRiskSubType.Rows)
                    {
                        RiskSubType temp = new RiskSubType();

                        temp.RiskID = dt["RiskID"].ToString();
                        temp.SubRiskID = dt["SubRiskID"].ToString();
                        temp.SubRiskDescription = dt["SubRiskDescription"].ToString();

                        riskSubTypeLines.Add(temp);
                    }

                    res.Data = riskSubTypeLines;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = false;
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

        [HttpGet("getEPKRSItemRiskCategory")]
        public async Task<IActionResult> getEPKRSItemRiskCategory()
        {
            ResultModel<List<ItemRiskCategory>> res = new ResultModel<List<ItemRiskCategory>>();
            EPKRSDA da = new(_configuration);
            List<ItemRiskCategory> itemRiskCategoryLines = new List<ItemRiskCategory>();
            DataTable dtItemRiskCategory = new DataTable("itemRiskCategory");
            IActionResult actionResult = null;

            try
            {
                dtItemRiskCategory = da.getEPKRSItemRiskCategoryData();

                if (dtItemRiskCategory.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtItemRiskCategory.Rows)
                    {
                        ItemRiskCategory temp = new ItemRiskCategory();

                        temp.ItemRiskCategoryID = dt["ItemRiskCategoryID"].ToString();
                        temp.CategoryDescription = dt["CategoryDescription"].ToString();

                        itemRiskCategoryLines.Add(temp);
                    }

                    res.Data = itemRiskCategoryLines;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = false;
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

        [HttpGet("getEPKRSIncidentAccidentInvolverType")]
        public async Task<IActionResult> getEPKRSIncidentAccidentInvolverType()
        {
            ResultModel<List<IncidentAccidentInvolverType>> res = new ResultModel<List<IncidentAccidentInvolverType>>();
            EPKRSDA da = new(_configuration);
            List<IncidentAccidentInvolverType> involverLines = new List<IncidentAccidentInvolverType>();
            DataTable dtInvolver = new DataTable("IncidentAccidentInvolverType");
            IActionResult actionResult = null;

            try
            {
                dtInvolver = da.getEPKRSIncidentAccidentInvolerTypeData();

                if (dtInvolver.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtInvolver.Rows)
                    {
                        IncidentAccidentInvolverType temp = new IncidentAccidentInvolverType();

                        temp.InvolverTypeID = dt["InvolverTypeID"].ToString();
                        temp.InvolverTypeDescription = dt["InvolverTypeDescription"].ToString();

                        involverLines.Add(temp);
                    }

                    res.Data = involverLines;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = false;
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

        [HttpGet("getEPKRSItemCase/{param}")]
        public async Task<IActionResult> getEPKRSItemCaseData(string param)
        {
            ResultModel<List<EPKRSUploadItemCase>> res = new ResultModel<List<EPKRSUploadItemCase>>();
            EPKRSDA da = new(_configuration);
            List<EPKRSUploadItemCase> itemCaseLines = new List<EPKRSUploadItemCase>();
            DataTable dtItemCase = new DataTable("EPKRSUploadItemCase");
            DataTable dtItemLine = new DataTable("ItemLine");
            DataTable dtCaseAttachment = new DataTable("CaseAttachment");
            DataTable dtCaseApproval = new DataTable("CaseApproval");
            DataTable dtParam = new DataTable("Parameter");
            IActionResult actionResult = null;

            try
            {
                string[] temp = CommonLibrary.Base64Decode(param).Split("!_!");
                string loc = temp[0].Equals("") ? "HO" : temp[0];

                dtItemCase = da.getEPKRSItemCaseData(loc, temp[1], Convert.ToInt32(temp[2]));

                dtParam = dtItemCase.Copy();

                foreach (var removedCol in new[] {
                    "ReportingID",
                    "SiteReporter",
                    "SiteSender",
                    "ReportDate",
                    "ItemPickupDate",
                    "LoadingDocumentID",
                    "LoadingDocumentDate",
                    "ExtendedMitigationPlan",
                    "DocumentStatus" })
                {
                    if (dtParam.Columns.Contains(removedCol))
                        dtParam.Columns.Remove(removedCol);
                }

                if (dtItemCase.Rows.Count <= 0)
                    throw new Exception("Fail Fetch Item Case Data");

                dtItemLine = da.getEPKRSItemLineData(dtParam);

                if (dtItemLine.Rows.Count <= 0)
                    throw new Exception("Fail Fetch Item Line Data");

                dtCaseAttachment = da.getEPKRSAttachmentData(dtParam);

                if (dtCaseAttachment.Rows.Count <= 0)
                    throw new Exception("Fail Fetch Attachment Data");

                dtCaseApproval = da.getEPKRSApprovalData(dtParam);

                if (dtItemCase.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtItemCase.Rows)
                    {
                        EPKRSUploadItemCase temp1 = new EPKRSUploadItemCase();

                        temp1.itemCase.SubRiskID = dt["SubRiskID"].ToString();
                        temp1.itemCase.DocumentID = dt["DocumentID"].ToString();
                        temp1.itemCase.SiteReporter = dt["SiteReporter"].ToString();
                        temp1.itemCase.SiteSender = dt["SiteSender"].ToString();
                        temp1.itemCase.ReportDate = Convert.ToDateTime(dt["ReportDate"]);
                        temp1.itemCase.ItemPickupDate = Convert.ToDateTime(dt["ItemPickupDate"]);
                        temp1.itemCase.LoadingDocumentID = dt["LoadingDocumentID"].ToString();
                        temp1.itemCase.LoadingDocumentDate = Convert.ToDateTime(dt["LoadingDocumentDate"]);
                        temp1.itemCase.ExtendedMitigationPlan = dt.IsNull("ExtendedMitigationPlan") ? "" : dt["ExtendedMitigationPlan"].ToString();
                        temp1.itemCase.DocumentStatus = dt["DocumentStatus"].ToString();

                        temp1.itemLine = dtItemLine.AsEnumerable().Where(y => y["DocumentID"].ToString().Equals(dt["DocumentID"].ToString())).Select(x => new ItemLine
                        {
                            DocumentID = x["DocumentID"].ToString(),
                            LineNum = Convert.ToInt32(x["LineNum"]),
                            TRID = x["TRID"].ToString(),
                            TRDate = Convert.ToDateTime(x["TRDate"]),
                            ItemCode = x["ItemCode"].ToString(),
                            ItemDescription = x["ItemDescription"].ToString(),
                            ItemRiskCategoryID = x["ItemRiskCategoryID"].ToString(),
                            CategoryID = x["CategoryID"].ToString(),
                            ItemQuantity = Convert.ToInt32(x["ItemQuantity"]),
                            UOM = x["UOM"].ToString(),
                            ItemValue = Convert.ToDecimal(x["ItemValue"]),
                            ItemStock = Convert.ToInt32(x["ItemStock"].ToString()),
                            VarianceDate = Convert.ToInt32(x["VarianceDate"]),
                            isLate = Convert.ToBoolean(x["isLate"]),
                            isCCTVCoverable = Convert.ToBoolean(x["isCCTVCoverable"]),
                            isReportedtoSender = Convert.ToBoolean(x["isReportedtoSender"])
                        }).ToList();

                        temp1.attachment = dtCaseAttachment.AsEnumerable().Where(y => y["DocumentID"].ToString().Equals(dt["DocumentID"].ToString())).Select(x => new CaseAttachment
                        {
                            DocumentID = x["DocumentID"].ToString(),
                            LineNum = Convert.ToInt32(x["LineNum"]),
                            UploadDate = Convert.ToDateTime(x["UploadDate"]),
                            FileExtension = x["FileExtention"].ToString(),
                            FilePath = x["FilePath"].ToString()
                        }).ToList();

                        if (dtCaseApproval.Rows.Count > 0)
                        {
                            temp1.Approval = dtCaseApproval.AsEnumerable().Where(y => y["DocumentID"].ToString().Equals(dt["DocumentID"].ToString())).Select(x => new DocumentApproval
                            {
                                DocumentID = x["DocumentID"].ToString(),
                                ApprovalAction = x["ApprovalAction"].ToString(),
                                Approver = x["Approver"].ToString(),
                                ApproveDate = Convert.ToDateTime(x["ApproveDate"])
                            }).ToList();
                        }

                        itemCaseLines.Add(temp1);
                    }

                    res.Data = itemCaseLines;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = false;
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

        [HttpGet("getEPKRSIncidentAccident/{param}")]
        public async Task<IActionResult> getEPKRSIncidentAccidentData(string param)
        {
            ResultModel<List<EPKRSUploadIncidentAccident>> res = new ResultModel<List<EPKRSUploadIncidentAccident>>();
            EPKRSDA da = new(_configuration);
            List<EPKRSUploadIncidentAccident> incidentAccidentLines = new List<EPKRSUploadIncidentAccident>();
            DataTable dtIncidentAccident = new DataTable("EPKRSUploadIncidentAccident");
            DataTable dtCaseAttachment = new DataTable("CaseAttachment");
            DataTable dtCaseApproval = new DataTable("CaseApproval");
            DataTable dtCaseInvolver = new DataTable("CaseInvolver");
            DataTable dtParam = new DataTable("Parameter");
            IActionResult actionResult = null;

            try
            {
                string[] temp = CommonLibrary.Base64Decode(param).Split("!_!");
                string loc = temp[0].Equals("") ? "HO" : temp[0];

                dtIncidentAccident = da.getEPKRSIncidentAccidentData(loc, temp[1], Convert.ToInt32(temp[2]));

                dtParam = dtIncidentAccident.Copy();

                foreach (var removedCol in new[] {
                    "ReportingID",
                    "ReportDate",
                    "OccurenceDate",
                    "SiteReporter",
                    "DepartmentReporter",
                    "RiskRPName",
                    "RiskRPEmail",
                    "DORMName",
                    "DORMEmail",
                    "CaseDescription",
                    "DepartmentAffected",
                    "Cronology",
                    "RootCause",
                    "LossDescription",
                    "LossEstimation",
                    "ReturnAmount",
                    "RiskDescription",
                    "CauseDescription",
                    "PIC",
                    "ActionPlan",
                    "TargetDate",
                    "MitigationPlan",
                    "MitigationDate",
                    "ExtendedRootCause" ,
                    "ExtendedMitigationPlan",
                    "DocumentStatus" })
                {
                    if (dtParam.Columns.Contains(removedCol))
                        dtParam.Columns.Remove(removedCol);
                }

                if (dtIncidentAccident.Rows.Count <= 0)
                    throw new Exception("Fail Fetch Item Case Data");

                dtCaseAttachment = da.getEPKRSAttachmentData(dtParam);

                if (dtCaseAttachment.Rows.Count <= 0)
                    throw new Exception("Fail Fetch Attachment Data");

                dtCaseApproval = da.getEPKRSApprovalData(dtParam);

                dtCaseInvolver = da.getEPKRSIncidentAccidentInvolverData(dtParam);

                if (dtIncidentAccident.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtIncidentAccident.Rows)
                    {
                        EPKRSUploadIncidentAccident temp1 = new EPKRSUploadIncidentAccident();

                        temp1.incidentAccident.SubRiskID = dt["SubRiskID"].ToString();
                        temp1.incidentAccident.DocumentID = dt["DocumentID"].ToString();
                        temp1.incidentAccident.ReportDate = Convert.ToDateTime(dt["ReportDate"]);
                        temp1.incidentAccident.OccurenceDate = Convert.ToDateTime(dt["OccurenceDate"]);
                        temp1.incidentAccident.SiteReporter = dt["SiteReporter"].ToString();
                        temp1.incidentAccident.DepartmentReporter = dt["DepartmentReporter"].ToString();
                        temp1.incidentAccident.RiskRPName = dt["RiskRPName"].ToString();
                        temp1.incidentAccident.RiskRPEmail = dt["RiskRPEmail"].ToString();
                        temp1.incidentAccident.DORMName = dt["DORMName"].ToString();
                        temp1.incidentAccident.DORMEmail = dt["DORMEmail"].ToString();
                        temp1.incidentAccident.CaseDescription = dt["CaseDescription"].ToString();
                        temp1.incidentAccident.DepartmentAffected = dt["DepartmentAffected"].ToString();
                        temp1.incidentAccident.Cronology = dt["Cronology"].ToString();
                        temp1.incidentAccident.RootCause = dt["RootCause"].ToString();
                        temp1.incidentAccident.LossDescription = dt["LossDescription"].ToString();
                        temp1.incidentAccident.LossEstimation = Convert.ToDecimal(dt["LossEstimation"]);
                        temp1.incidentAccident.ReturnAmount = Convert.ToDecimal(dt["ReturnAmount"]);
                        temp1.incidentAccident.RiskDescription = dt["RiskDescription"].ToString();
                        temp1.incidentAccident.CauseDescription = dt["CauseDescription"].ToString();
                        temp1.incidentAccident.PIC = dt["PIC"].ToString();
                        temp1.incidentAccident.ActionPlan = dt["ActionPlan"].ToString();
                        temp1.incidentAccident.TargetDate = Convert.ToDateTime(dt["TargetDate"]);
                        temp1.incidentAccident.MitigationPlan = dt["MitigationPlan"].ToString();
                        temp1.incidentAccident.MitigationDate = Convert.ToDateTime(dt["MitigationDate"]);
                        temp1.incidentAccident.ExtendedRootCause = dt.IsNull("ExtendedRootCause") ? "" : dt["ExtendedRootCause"].ToString();
                        temp1.incidentAccident.ExtendedMitigationPlan = dt.IsNull("ExtendedMitigationPlan") ? "" : dt["ExtendedMitigationPlan"].ToString();
                        temp1.incidentAccident.DocumentStatus = dt["DocumentStatus"].ToString();

                        temp1.attachment = dtCaseAttachment.AsEnumerable().Where(y => y["DocumentID"].ToString().Equals(dt["DocumentID"].ToString())).Select(x => new CaseAttachment
                        {
                            DocumentID = x["DocumentID"].ToString(),
                            LineNum = Convert.ToInt32(x["LineNum"]),
                            UploadDate = Convert.ToDateTime(x["UploadDate"]),
                            FileExtension = x["FileExtention"].ToString(),
                            FilePath = x["FilePath"].ToString()
                        }).ToList();

                        if (dtCaseApproval.Rows.Count > 0)
                        {
                            temp1.Approval = dtCaseApproval.AsEnumerable().Where(y => y["DocumentID"].ToString().Equals(dt["DocumentID"].ToString())).Select(x => new DocumentApproval
                            {
                                DocumentID = x["DocumentID"].ToString(),
                                ApprovalAction = x["ApprovalAction"].ToString(),
                                Approver = x["Approver"].ToString(),
                                ApproveDate = Convert.ToDateTime(x["ApproveDate"])
                            }).ToList();
                        }

                        if (dtCaseInvolver.Rows.Count > 0)
                        {
                            temp1.Involver = dtCaseInvolver.AsEnumerable().Where(y => y["DocumentID"].ToString().Equals(dt["DocumentID"].ToString())).Select(x => new IncidentAccidentInvolver
                            {
                                DocumentID = x["DocumentID"].ToString(),
                                InvolverName = x["InvolverName"].ToString(),
                                InvolverDept = x["InvolverDept"].ToString(),
                                InvolverPosition = x["InvolverPosition"].ToString(),
                                InvolverTypeID = x["InvolverTypeID"].ToString()
                            }).ToList();
                        }

                        incidentAccidentLines.Add(temp1);
                    }

                    res.Data = incidentAccidentLines;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = false;
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

        [HttpGet("getEPKRSDocumentDiscussion/{param}")]
        public async Task<IActionResult> getEPKRSDocumentDiscussion(string param)
        {
            ResultModel<List<DocumentDiscussion>> res = new ResultModel<List<DocumentDiscussion>>();
            EPKRSDA da = new(_configuration);
            List<DocumentDiscussion> documentDiscussions = new List<DocumentDiscussion>();
            DataTable dtDocumentDiscussion = new DataTable("DocumentDiscussion");
            IActionResult actionResult = null;

            try
            {
                string[] temp = CommonLibrary.Base64Decode(param).Split("!_!");

                dtDocumentDiscussion = da.getEPKRSDiscussionData(temp[0], temp[1]);

                if (dtDocumentDiscussion.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtDocumentDiscussion.Rows)
                    {
                        DocumentDiscussion temp1 = new DocumentDiscussion();

                        temp1.rowGuid = dt["rowGuid"].ToString();
                        temp1.DocumentID = dt["DocumentID"].ToString();
                        temp1.UserName = dt["UserName"].ToString();
                        temp1.CommentDate = Convert.ToDateTime(dt["CommentDate"]);
                        temp1.Comment = dt["Comment"].ToString();
                        temp1.isEdited = Convert.ToBoolean(dt["isEdited"]);
                        temp1.ReplyRowGuid = dt["ReplyRowGuid"].ToString();

                        documentDiscussions.Add(temp1);
                    }

                    res.Data = documentDiscussions;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = false;
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

        [HttpPost("getEPKRSInitializationDocumentDiscussions")]
        public async Task<IActionResult> getEPKRSInitializationDocumentDiscussions(List<DocumentListParams> param)
        {
            ResultModel<List<DocumentDiscussion>> res = new ResultModel<List<DocumentDiscussion>>();
            EPKRSDA da = new(_configuration);
            res.Data = new();
            IActionResult actionResult = null;

            try
            {
                var exec = Parallel.ForEach(param, new ParallelOptions { MaxDegreeOfParallelism = 4 }, (dt, i) =>
                {
                    List<DocumentDiscussion> documentDiscussions = new List<DocumentDiscussion>();
                    DataTable dtDocumentDiscussion = new DataTable($"{dt.DocumentID}");

                    dtDocumentDiscussion = da.getEPKRSDiscussionData(dt.LocationID, dt.DocumentID);

                    if (dtDocumentDiscussion.Rows.Count > 0)
                    {
                        foreach (DataRow dta in dtDocumentDiscussion.Rows)
                        {
                            DocumentDiscussion temp1 = new DocumentDiscussion();

                            temp1.rowGuid = dta["rowGuid"].ToString();
                            temp1.DocumentID = dta["DocumentID"].ToString();
                            temp1.UserName = dta["UserName"].ToString();
                            temp1.CommentDate = Convert.ToDateTime(dta["CommentDate"]);
                            temp1.Comment = dta["Comment"].ToString();
                            temp1.isEdited = Convert.ToBoolean(dta["isEdited"]);
                            temp1.ReplyRowGuid = dta["ReplyRowGuid"].ToString();

                            documentDiscussions.Add(temp1);
                        }

                        res.Data.AddRange(documentDiscussions);
                    }
                });

                if (exec.IsCompleted)
                {
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.isSuccess = false;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "PARALLEL STOPED PREMATURELY";

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

        [HttpPost("getEPKRSDocumentDiscussionReadHistory")]
        public async Task<IActionResult> getEPKRSDocumentDiscussionReadHistory(List<DocumentListParams> param)
        {
            ResultModel<List<DocumentDiscussionReadHistory>> res = new ResultModel<List<DocumentDiscussionReadHistory>>();
            EPKRSDA da = new(_configuration);
            res.Data = new();
            IActionResult actionResult = null;

            try
            {
                var exec = Parallel.ForEach(param, new ParallelOptions { MaxDegreeOfParallelism = 4 }, (dt, i) =>
                {
                    List<DocumentDiscussionReadHistory> documentDiscussionReadHistories = new List<DocumentDiscussionReadHistory>();
                    DataTable dtDocumentDiscussionReadHistory = new DataTable($"{dt.DocumentID}");

                    dtDocumentDiscussionReadHistory = da.getEPKRSDocumentDiscussionReadHistoryData(dt.LocationID, dt.DocumentID);

                    if (dtDocumentDiscussionReadHistory.Rows.Count > 0)
                    {
                        foreach (DataRow dta in dtDocumentDiscussionReadHistory.Rows)
                        {
                            DocumentDiscussionReadHistory temp1 = new DocumentDiscussionReadHistory();

                            temp1.rowGuid = dta["rowGuid"].ToString();
                            temp1.DocumentID = dta["DocumentID"].ToString();
                            temp1.UserName = dta["UserName"].ToString();
                            temp1.ReadDate = Convert.ToDateTime(dta["ReadDate"]);

                            documentDiscussionReadHistories.Add(temp1);
                        }

                        res.Data.AddRange(documentDiscussionReadHistories);
                    }
                });

                if (exec.IsCompleted)
                {
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.isSuccess = false;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "PARALLEL STOPED PREMATURELY";

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

        [HttpGet("getEPKRSCaseAttachment/{param}")]
        public async Task<IActionResult> getEPKRSCaseAttachment(string param)
        {
            ResultModel<List<CaseAttachment>> res = new ResultModel<List<CaseAttachment>>();
            EPKRSDA da = new(_configuration);
            List<CaseAttachment> caseAttachments = new List<CaseAttachment>();
            DataTable dtCaseAttachment = new DataTable("CaseAttachment");
            IActionResult actionResult = null;

            try
            {
                string temp = CommonLibrary.Base64Decode(param);

                DataTable tableParam = new("Param");
                tableParam.Columns.Add(new DataColumn("RiskID", typeof(string)));
                tableParam.Columns.Add(new DataColumn("DocumentID", typeof(string)));
                tableParam.Columns.Add(new DataColumn("AuditUser", typeof(string)));
                tableParam.Columns.Add(new DataColumn("AuditAction", typeof(string)));
                tableParam.Columns.Add(new DataColumn("AuditActionDate", typeof(DateTime)));

                DataRow tableRowParam = tableParam.NewRow();
                tableRowParam["RiskID"] = "BLANK";
                tableRowParam["DocumentID"] = temp;
                tableRowParam["AuditUser"] = "BLANK";
                tableRowParam["AuditAction"] = "I";
                tableRowParam["AuditActionDate"] = DateTime.Now;
                tableParam.Rows.Add(tableRowParam);

                dtCaseAttachment = da.getEPKRSAttachmentData(tableParam);

                if (dtCaseAttachment.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtCaseAttachment.Rows)
                    {
                        CaseAttachment temp1 = new CaseAttachment();

                        temp1.DocumentID = dt["DocumentID"].ToString();
                        temp1.LineNum = Convert.ToInt32(dt["LineNum"]);
                        temp1.UploadDate = Convert.ToDateTime(dt["UploadDate"]);
                        temp1.FileExtension = dt["FileExtention"].ToString();
                        temp1.FilePath = dt["FilePath"].ToString();

                        caseAttachments.Add(temp1);
                    }

                    res.Data = caseAttachments;
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

        [HttpGet("getEPKRSGeneralStatistics/{param}")]
        public async Task<IActionResult> getEPKRSGeneralStatistics(string param)
        {
            ResultModel<List<EPKRSDocumentStatistics>> res = new ResultModel<List<EPKRSDocumentStatistics>>();
            EPKRSDA da = new(_configuration);
            List<EPKRSDocumentStatistics> documentStatistics = new List<EPKRSDocumentStatistics>();
            DataTable dtDocumentStatistics = new DataTable("documentStatistics");
            IActionResult actionResult = null;

            try
            {
                string temp = CommonLibrary.Base64Decode(param);

                dtDocumentStatistics = da.getEPKRSGeneralStatisticsData(temp);

                if (dtDocumentStatistics.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtDocumentStatistics.Rows)
                    {
                        EPKRSDocumentStatistics temp1 = new EPKRSDocumentStatistics();

                        temp1.RiskID = dt["RiskID"].ToString();
                        temp1.ReportTotalDocuments = Convert.ToInt32(dt["TotalDocuments"]);
                        temp1.ReportTotalOpenDocuments = Convert.ToInt32(dt["OpenDocuments"]);
                        temp1.ReportTotalApprovedDocuments = Convert.ToInt32(dt["ApprovedDocuments"]);
                        temp1.ReportTotalOnProgressDocuments = Convert.ToInt32(dt["OnProgressDocuments"]);
                        temp1.ReportTotalClosedDocuments = Convert.ToInt32(dt["ClosedDocuments"]);
                        temp1.ReportTotalValue = Convert.ToDecimal(dt["TotalValues"]);
                        temp1.ReportingID = dt["ReportingType"].ToString();
                        temp1.ReportReturnValue = Convert.ToDecimal(dt["ReturnValues"]);

                        documentStatistics.Add(temp1);
                    }

                    res.Data = documentStatistics;
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

        [HttpPost("getEPKRSGeneralIncidentAccidentStatistics")]
        public async Task<IActionResult> getEPKRSGeneralIncidentAccidentStatistics(QueryModel<string> param)
        {
            ResultModel<List<EPKRSDocumentStatistics>> res = new ResultModel<List<EPKRSDocumentStatistics>>();
            EPKRSDA da = new(_configuration);
            List<EPKRSDocumentStatistics> documentStatistics = new List<EPKRSDocumentStatistics>();
            DataTable dtDocumentStatistics = new DataTable("documentStatistics");
            IActionResult actionResult = null;

            try
            {
                string[] temp = CommonLibrary.Base64Decode(param.Data).Split("!_!");

                dtDocumentStatistics = da.getEPKRSGeneralIncidentAccidentStatisticsData(temp[0], temp[1]);

                if (dtDocumentStatistics.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtDocumentStatistics.Rows)
                    {
                        EPKRSDocumentStatistics temp1 = new EPKRSDocumentStatistics();

                        temp1.RiskID = dt["RiskID"].ToString();
                        temp1.ReportTotalDocuments = Convert.ToInt32(dt["TotalDocuments"]);
                        temp1.ReportTotalOpenDocuments = Convert.ToInt32(dt["OpenDocuments"]);
                        temp1.ReportTotalApprovedDocuments = Convert.ToInt32(dt["ApprovedDocuments"]);
                        temp1.ReportTotalOnProgressDocuments = Convert.ToInt32(dt["OnProgressDocuments"]);
                        temp1.ReportTotalClosedDocuments = Convert.ToInt32(dt["ClosedDocuments"]);
                        temp1.ReportTotalValue = Convert.ToDecimal(dt["TotalValues"]);
                        temp1.ReportingID = dt["ReportingType"].ToString();
                        temp1.ReportReturnValue = Convert.ToDecimal(dt["ReturnValues"]);

                        documentStatistics.Add(temp1);
                    }

                    res.Data = documentStatistics;
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

        [HttpGet("getEPKRSItemCaseCategoryStatistics/{param}")]
        public async Task<IActionResult> getEPKRSItemCaseCategoryStatistics(string param)
        {
            ResultModel<List<EPKRSItemCaseCategoryStatistics>> res = new ResultModel<List<EPKRSItemCaseCategoryStatistics>>();
            EPKRSDA da = new(_configuration);
            List<EPKRSItemCaseCategoryStatistics> itemCaseCategoryStats = new List<EPKRSItemCaseCategoryStatistics>();
            DataTable dtItemCaseCategoryStats = new DataTable("itemCaseCategoryStats");
            IActionResult actionResult = null;

            try
            {
                string[] temp = CommonLibrary.Base64Decode(param).Split("!_!");

                dtItemCaseCategoryStats = da.getEPKRSItemCaseCategoryStatisticsData(temp[0], temp[1]);

                if (dtItemCaseCategoryStats.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtItemCaseCategoryStats.Rows)
                    {
                        EPKRSItemCaseCategoryStatistics temp1 = new EPKRSItemCaseCategoryStatistics();

                        temp1.ItemRiskCategoryID = dt["ItemRiskCategory"].ToString();
                        temp1.TotalItemQty = Convert.ToInt32(dt["TotalItemQty"]);
                        temp1.TotalItemValue = Convert.ToDecimal(dt["TotalItemValue"]);

                        itemCaseCategoryStats.Add(temp1);
                    }

                    res.Data = itemCaseCategoryStats;
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

        [HttpGet("getEPKRSTopLocationReportStatistics/{param}")]
        public async Task<IActionResult> getEPKRSTopLocationReportStatistics(string param)
        {
            ResultModel<List<EPKRSTopLocationReportStatistics>> res = new ResultModel<List<EPKRSTopLocationReportStatistics>>();
            EPKRSDA da = new(_configuration);
            List<EPKRSTopLocationReportStatistics> topLocationReports = new List<EPKRSTopLocationReportStatistics>();
            DataTable dtTopLocationReports = new DataTable("topLocationReports");
            IActionResult actionResult = null;

            try
            {
                string[] temp = CommonLibrary.Base64Decode(param).Split("!_!");

                dtTopLocationReports = da.getEPKRSTopLocationReportStatisticsData(temp[0], temp[1]);

                if (dtTopLocationReports.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtTopLocationReports.Rows)
                    {
                        EPKRSTopLocationReportStatistics temp1 = new EPKRSTopLocationReportStatistics();

                        temp1.LocationID = dt["LocationID"].ToString();
                        temp1.TotalDocuments = Convert.ToInt32(dt["TotalDocuments"]);

                        topLocationReports.Add(temp1);
                    }

                    res.Data = topLocationReports;
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

        [HttpGet("getEPKRSItemCategoriesStatistics/{param}")]
        public async Task<IActionResult> getEPKRSItemCategoriesStatistics(string param)
        {
            ResultModel<List<EPKRSItemCaseItemCategoryStatistics>> res = new ResultModel<List<EPKRSItemCaseItemCategoryStatistics>>();
            EPKRSDA da = new(_configuration);
            List<EPKRSItemCaseItemCategoryStatistics> itemCategoryStats = new List<EPKRSItemCaseItemCategoryStatistics>();
            DataTable dtItemCategoryStats = new DataTable("topLocationReports");
            IActionResult actionResult = null;

            try
            {
                string[] temp = CommonLibrary.Base64Decode(param).Split("!_!");

                dtItemCategoryStats = da.getEPKRSItemCategoriesStatisticsData(temp[0], temp[1]);

                if (dtItemCategoryStats.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtItemCategoryStats.Rows)
                    {
                        EPKRSItemCaseItemCategoryStatistics temp1 = new EPKRSItemCaseItemCategoryStatistics();

                        temp1.CategoryID = dt["CategoryID"].ToString();
                        temp1.TotalDocuments = Convert.ToInt32(dt["TotalDocuments"]);

                        itemCategoryStats.Add(temp1);
                    }

                    res.Data = itemCategoryStats;
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

        [HttpPost("getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmail")]
        public async Task<IActionResult> getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmail(QueryModel<string> param)
        {
            ResultModel<List<EPKRSIncidentAccidentRegionalStatistics>> res = new ResultModel<List<EPKRSIncidentAccidentRegionalStatistics>>();
            EPKRSDA da = new(_configuration);
            List<EPKRSIncidentAccidentRegionalStatistics> regionalStats = new List<EPKRSIncidentAccidentRegionalStatistics>();
            DataTable dtRegionalStats = new DataTable("EPKRSIncidentAccidentRegionalStatistics");
            IActionResult actionResult = null;

            try
            {
                string temp = CommonLibrary.Base64Decode(param.Data);

                dtRegionalStats = da.getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmailData(temp);

                if (dtRegionalStats.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtRegionalStats.Rows)
                    {
                        EPKRSIncidentAccidentRegionalStatistics temp1 = new();

                        temp1.DORMEmail = dt["DORMEmail"].ToString();
                        temp1.TotalDocuments = Convert.ToInt32(dt["TotalDocuments"]);
                        temp1.TotalValues = Convert.ToDecimal(dt["TotalValues"]);
                        temp1.ReturnValues = Convert.ToDecimal(dt["ReturnValues"]);

                        regionalStats.Add(temp1);
                    }

                    res.Data = regionalStats;
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

        [HttpPost("getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPosition")]
        public async Task<IActionResult> getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPosition(QueryModel<string> param)
        {
            ResultModel<List<EPKRSIncidentAccidentInvolverStatisticsbyPosition>> res = new ResultModel<List<EPKRSIncidentAccidentInvolverStatisticsbyPosition>>();
            EPKRSDA da = new(_configuration);
            List<EPKRSIncidentAccidentInvolverStatisticsbyPosition> involverStats = new List<EPKRSIncidentAccidentInvolverStatisticsbyPosition>();
            DataTable dtInvolverStats = new DataTable("EPKRSIncidentAccidentInvolverStatistics");
            IActionResult actionResult = null;

            try
            {
                string[] temp = CommonLibrary.Base64Decode(param.Data).Split("!_!");

                dtInvolverStats = da.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPositionData(temp[0], temp[1]);

                if (dtInvolverStats.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtInvolverStats.Rows)
                    {
                        EPKRSIncidentAccidentInvolverStatisticsbyPosition temp1 = new();

                        temp1.InvolverPosition = dt["InvolverPosition"].ToString();
                        temp1.TotalDocuments = Convert.ToInt32(dt["TotalDocuments"]);
                        temp1.TotalInvolver = Convert.ToInt32(dt["TotalInvolver"]);

                        involverStats.Add(temp1);
                    }

                    res.Data = involverStats;
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

        [HttpPost("getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDept")]
        public async Task<IActionResult> getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDept(QueryModel<string> param)
        {
            ResultModel<List<EPKRSIncidentAccidentInvolverStatisticsbyDept>> res = new ResultModel<List<EPKRSIncidentAccidentInvolverStatisticsbyDept>>();
            EPKRSDA da = new(_configuration);
            List<EPKRSIncidentAccidentInvolverStatisticsbyDept> involverStats = new List<EPKRSIncidentAccidentInvolverStatisticsbyDept>();
            DataTable dtInvolverStats = new DataTable("EPKRSIncidentAccidentInvolverStatistics");
            IActionResult actionResult = null;

            try
            {
                string[] temp = CommonLibrary.Base64Decode(param.Data).Split("!_!");

                dtInvolverStats = da.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDeptData(temp[0], temp[1]);

                if (dtInvolverStats.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtInvolverStats.Rows)
                    {
                        EPKRSIncidentAccidentInvolverStatisticsbyDept temp1 = new();

                        temp1.InvolverDept = dt["InvolverDept"].ToString();
                        temp1.TotalDocuments = Convert.ToInt32(dt["TotalDocuments"]);
                        temp1.TotalInvolver = Convert.ToInt32(dt["TotalInvolver"]);

                        involverStats.Add(temp1);
                    }

                    res.Data = involverStats;
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

        [HttpGet("getEPKRSModuleNumberOfPage/{param}")]
        public async Task<IActionResult> getEPKRSModuleNumberOfPage(string param)
        {
            ResultModel<int> res = new ResultModel<int>();
            EPKRSDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                string[] temp = CommonLibrary.Base64Decode(param).Split("!_!");
                string loc = temp[1].Equals("") ? "HO" : temp[1];

                res.Data = da.getEPKRSModuleNumberOfPageData(temp[0], loc, temp[2]);

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
