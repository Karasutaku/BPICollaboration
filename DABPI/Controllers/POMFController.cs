using BPIDA.DataAccess;
using BPIDA.Models.DbModel;
using BPIDA.Models.MainModel;
using BPIDA.Models.MainModel.POMF;
using BPILibrary;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlTypes;

namespace BPIDA.Controllers
{
    [Route("api/DA/POMF")]
    [ApiController]
    public class POMFController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public POMFController(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpPost("createPOMFDocument")]
        public async Task<IActionResult> createPOMFDocument(QueryModel<POMFDocument> data)
        {
            ResultModel<QueryModel<POMFDocument>> res = new ResultModel<QueryModel<POMFDocument>>();
            GeneralDA generalDA = new(_configuration);
            POMFDA da = new(_configuration);
            DataTable dtMainIdentity = new DataTable("Identity");
            string id = string.Empty;
            IActionResult actionResult = null;

            try
            {

                dtMainIdentity = generalDA.createIDData("POMF");
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

                QueryModel<POMFHeader> dtMain = new();
                dtMain.Data = new();

                dtMain.Data = data.Data.dataHeader;
                dtMain.Data.POMFID = id;
                dtMain.userEmail = data.userEmail;
                dtMain.userAction = data.userAction;
                dtMain.userActionDate = data.userActionDate;

                List<POMFItemLine> tempItemLine = new();
                data.Data.dataItemLines.ForEach(x =>
                {
                    tempItemLine.Add(new POMFItemLine
                    {
                        POMFID = id,
                        LineNum = x.LineNum,
                        ItemCode = x.ItemCode,
                        ItemDescription = x.ItemDescription,
                        RequestQuantity = x.RequestQuantity,
                        NPQuantity = x.NPQuantity,
                        ItemUOM = x.ItemUOM,
                        ItemValue = x.ItemValue,
                        RequestToSite = x.RequestToSite,
                        ExternalRequestDocument = x.ExternalRequestDocument,
                        RequestDocumentDate = null,
                        ExternalReceiveDocument = x.ExternalReceiveDocument,
                        ReceiveDocumentDate = null
                    });
                });

                var dtLines = CommonLibrary.ListToDataTable<POMFItemLine>(tempItemLine, data.userEmail, data.userAction, data.userActionDate, "POMFItemLines");

                if (da.createPOMFDocumentData(dtMain, dtLines))
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

        [HttpPost("createPOMFApproval")]
        public async Task<IActionResult> createPOMFApproval(QueryModel<POMFApprovalStream> data)
        {
            ResultModel<QueryModel<POMFApprovalStream>> res = new ResultModel<QueryModel<POMFApprovalStream>>();
            POMFDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                QueryModel<POMFApproval> temp = new();
                temp.Data = new();

                temp.Data = data.Data.Data;
                temp.userEmail = data.userEmail;
                temp.userAction = data.userAction;
                temp.userActionDate = data.userActionDate;

                if (da.createPOMFApprovalData(temp, data.Data.LocationID))
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

        [HttpPost("createPOMFApprovalExtended")]
        public async Task<IActionResult> createPOMFApprovalExtended(QueryModel<POMFApprovalStreamExtended> data)
        {
            ResultModel<QueryModel<POMFApprovalStreamExtended>> res = new ResultModel<QueryModel<POMFApprovalStreamExtended>>();
            POMFDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                if (da.editPOMFApprovalExtendedData(
                    data.Data.LocationID
                    , CommonLibrary.ListToDataTable<POMFItemLine>(data.Data.pomfItemLines, data.userEmail, data.userAction, data.userActionDate, "items")
                    , data.Data.approvalData
                    , data.userEmail
                    , data.userAction
                    , data.userActionDate))
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

        [HttpPost("deletePOMFDocument")]
        public async Task<IActionResult> deletePOMFDocument(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            POMFDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                string[] temp = CommonLibrary.Base64Decode(data.Data).Split("!_!");

                if (da.deletePOMFDocument(temp[0], temp[1], data.userEmail, data.userAction, data.userActionDate))
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

        [HttpGet("getPOMFDocuments/{param}")]
        public async Task<IActionResult> getPOMFDocuments(string param)
        {
            ResultModel<List<POMFDocument>> res = new ResultModel<List<POMFDocument>>();
            POMFDA da = new(_configuration);
            List<POMFDocument> pomfDocuments = new List<POMFDocument>();
            DataTable dtPOMFDocument = new DataTable("POMFDocument");
            DataTable dtPOMFItemLine = new DataTable("POMFItemLine");
            DataTable dtPOMFApproval = new DataTable("POMFApproval");
            DataTable dtParam = new DataTable("Parameter");
            IActionResult actionResult = null;

            try
            {
                string[] temp = CommonLibrary.Base64Decode(param).Split("!_!");
                string loc = temp[0].Equals("") ? "HO" : temp[0];

                dtPOMFDocument = da.getPOMFHeaderbyFilterData(loc, temp[1], Convert.ToInt32(temp[2]));

                dtParam = dtPOMFDocument.Copy();

                foreach (var removedCol in new[] {
                    "POMFDate",
                    "LocationID",
                    "CustomerName",
                    "ReceiptNo",
                    "NPNo",
                    "NPTypeID",
                    "Requester",
                    "DocumentStatus",
                    "AuditUser",
                    "AuditAction",
                    "AuditActionDate" })
                {
                    if (dtParam.Columns.Contains(removedCol))
                        dtParam.Columns.Remove(removedCol);
                }

                if (dtPOMFDocument.Rows.Count <= 0)
                    throw new Exception("Fail Fetch POMFHeader Data");

                dtPOMFItemLine = da.getPOMFItemLinesData(loc, dtParam);

                if (dtPOMFItemLine.Rows.Count <= 0)
                    throw new Exception("Fail Fetch POMFItemLine Data");

                dtPOMFApproval = da.getPOMFApprovalsData(loc, dtParam);

                //if (dtPOMFApproval.Rows.Count <= 0)
                //    throw new Exception("Fail Fetch POMFApproval Data");

                if (dtPOMFDocument.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtPOMFDocument.Rows)
                    {
                        POMFDocument temp1 = new POMFDocument();

                        temp1.dataHeader.POMFID = dt["POMFID"].ToString();
                        temp1.dataHeader.POMFDate = Convert.ToDateTime(dt["POMFDate"]);
                        temp1.dataHeader.LocationID = dt["LocationID"].ToString();
                        temp1.dataHeader.CustomerName = dt["CustomerName"].ToString();
                        temp1.dataHeader.ReceiptNo = dt["ReceiptNo"].ToString();
                        temp1.dataHeader.NPNo = dt["NPNo"].ToString();
                        temp1.dataHeader.NPTypeID = dt["NPTypeID"].ToString();
                        temp1.dataHeader.Requester = dt["Requester"].ToString();
                        temp1.dataHeader.DocumentStatus = dt["DocumentStatus"].ToString();

                        temp1.dataItemLines = dtPOMFItemLine.AsEnumerable().Where(y => y["POMFID"].ToString().Equals(dt["POMFID"].ToString())).Select(x => new POMFItemLine
                        {
                            POMFID = x["POMFID"].ToString(),
                            LineNum = Convert.ToInt32(x["LineNum"]),
                            ItemCode = x["ItemCode"].ToString(),
                            ItemDescription = x["ItemDescription"].ToString(),
                            RequestQuantity = Convert.ToInt32(x["RequestQuantity"]),
                            NPQuantity = Convert.ToInt32(x["NPQuantity"]),
                            ItemUOM = x["ItemUOM"].ToString(),
                            ItemValue = Convert.ToDecimal(x["ItemValue"]),
                            RequestToSite = x["RequestToSite"].ToString(),
                            ExternalRequestDocument = x.IsNull("ExternalRequestDocument") ? "" : x["ExternalRequestDocument"].ToString(),
                            RequestDocumentDate = x.IsNull("RequestDocumentDate") ? new DateTime(DateTime.Now.Year, 1, 1) : Convert.ToDateTime(x["RequestDocumentDate"]),
                            ExternalReceiveDocument = x.IsNull("ExternalReceiveDocument") ? "" : x["ExternalReceiveDocument"].ToString(),
                            ReceiveDocumentDate = x.IsNull("ReceiveDocumentDate") ? new DateTime(DateTime.Now.Year, 1, 1) : Convert.ToDateTime(x["ReceiveDocumentDate"])
                        }).ToList();

                        if (dtPOMFApproval.AsEnumerable().Where(y => y["POMFID"].ToString().Equals(dt["POMFID"].ToString())).ToList().Count > 0)
                        {
                            temp1.dataApproval = dtPOMFApproval.AsEnumerable().Where(y => y["POMFID"].ToString().Equals(dt["POMFID"].ToString())).Select(x => new POMFApproval
                            {
                                POMFID = x["POMFID"].ToString(),
                                ApprovalAction = x["ApprovalAction"].ToString(),
                                Approver = x["Approver"].ToString(),
                                ApproveDate = Convert.ToDateTime(x["ApproveDate"])
                            }).ToList();
                        }

                        pomfDocuments.Add(temp1);
                    }

                    res.Data = pomfDocuments;
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

        [HttpGet("getPOMFNPType")]
        public async Task<IActionResult> getPOMFNPType()
        {
            ResultModel<List<POMFNPType>> res = new ResultModel<List<POMFNPType>>();
            POMFDA da = new(_configuration);
            List<POMFNPType> npTypes = new List<POMFNPType>();
            DataTable dtNPType = new DataTable("NPTypes");
            IActionResult actionResult = null;

            try
            {
                dtNPType = da.getPOMFNPTypeData();

                if (dtNPType.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtNPType.Rows)
                    {
                        POMFNPType temp1 = new();

                        temp1.NPTypeID = dt["NPTypeID"].ToString();
                        temp1.NPTypeDescription = dt["NPTypeDescription"].ToString();
                        temp1.isAutomaticApproval = Convert.ToBoolean(dt["isAutomaticApproval"]);

                        npTypes.Add(temp1);
                    }

                    res.Data = npTypes;
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

        [HttpGet("getPOMFModuleNumberOfPage/{param}")]
        public async Task<IActionResult> getEPKRSModuleNumberOfPage(string param)
        {
            ResultModel<int> res = new ResultModel<int>();
            POMFDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                string[] temp = CommonLibrary.Base64Decode(param).Split("!_!");
                string loc = temp[1].Equals("") ? "HO" : temp[1];

                res.Data = da.getPOMFModuleNumberOfPageData(temp[0], loc, temp[2]);

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
