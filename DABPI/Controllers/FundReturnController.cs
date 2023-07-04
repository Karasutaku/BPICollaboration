using BPIDA.DataAccess;
using BPIDA.Models.DbModel;
using BPIDA.Models.MainModel;
using BPIDA.Models.MainModel.Company;
using BPIDA.Models.MainModel.FundReturn;
using BPILibrary;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BPIDA.Controllers
{
    [Route("api/DA/FundReturn")]
    [ApiController]
    public class FundReturnController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly int _rowPerPage;

        public FundReturnController(IConfiguration config)
        {
            _configuration = config;
            _rowPerPage = _configuration.GetValue<int>("Paging:FundReturn:RowPerPage");
        }

        [HttpPost("createFundReturnDocument")]
        public async Task<IActionResult> createFundReturnDocument(QueryModel<FundReturnDocument> data)
        {
            ResultModel<QueryModel<FundReturnDocument>> res = new ResultModel<QueryModel<FundReturnDocument>>();
            GeneralDA generalDA = new(_configuration);
            FundReturnDA da = new(_configuration);
            DataTable dtMainIdentity = new DataTable("Identity");
            string id = string.Empty;
            IActionResult actionResult = null;

            try
            {
                dtMainIdentity = generalDA.createIDData("FundReturn");
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

                QueryModel<FundReturnHeader> dtMain = new();
                dtMain.Data = new();

                dtMain.Data = data.Data.dataHeader;
                dtMain.Data.DocumentID = id;
                dtMain.userEmail = data.userEmail;
                dtMain.userAction = data.userAction;
                dtMain.userActionDate = data.userActionDate;

                List<FundReturnItemLine> tempItemLine = new();
                data.Data.dataItemLines.ForEach(x =>
                {
                    tempItemLine.Add(new FundReturnItemLine
                    {
                        DocumentID = id,
                        LineNum = x.LineNum,
                        ItemCode = x.ItemCode,
                        ItemDescription = x.ItemDescription,
                        ItemQuantity = x.ItemQuantity,
                        UOM = x.UOM,
                        ItemAmount = x.ItemAmount,
                        ItemDiscount = x.ItemDiscount
                    });
                });

                List<FundReturnAttachment> tempAttachLine = new();
                data.Data.dataAttachmentLines.ForEach(x =>
                {
                    tempAttachLine.Add(new FundReturnAttachment
                    {
                        DocumentID = id,
                        LineNum = x.LineNum,
                        UploadDate = x.UploadDate,
                        FileExtension = x.FileExtension,
                        FilePath = x.FilePath
                    });
                });

                var dtLines = CommonLibrary.ListToDataTable<FundReturnItemLine>(tempItemLine, data.userEmail, data.userAction, data.userActionDate, "FRItemLines");
                var dtAttach = CommonLibrary.ListToDataTable<FundReturnAttachment>(tempAttachLine, data.userEmail, data.userAction, data.userActionDate, "FRAttachLines");

                if (da.createFundReturnDocumentData(dtMain, dtLines, dtAttach))
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

        [HttpPost("createFundReturnHeader")]
        public async Task<IActionResult> createFundReturnHeader(QueryModel<FundReturnDocument> data)
        {
            ResultModel<QueryModel<FundReturnDocument>> res = new ResultModel<QueryModel<FundReturnDocument>>();
            GeneralDA generalDA = new(_configuration);
            FundReturnDA da = new(_configuration);
            DataTable dtMainIdentity = new DataTable("Identity");
            string id = string.Empty;
            IActionResult actionResult = null;

            try
            {

                dtMainIdentity = generalDA.createIDData("FundReturn");
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

                QueryModel<FundReturnHeader> dtMain = new();
                dtMain.Data = new();

                dtMain.Data = data.Data.dataHeader;
                dtMain.Data.DocumentID = id;
                dtMain.userEmail = data.userEmail;
                dtMain.userAction = data.userAction;
                dtMain.userActionDate = data.userActionDate;

                List<FundReturnAttachment> tempAttachLine = new();
                data.Data.dataAttachmentLines.ForEach(x =>
                {
                    tempAttachLine.Add(new FundReturnAttachment
                    {
                        DocumentID = id,
                        LineNum = x.LineNum,
                        UploadDate = x.UploadDate,
                        FileExtension = x.FileExtension,
                        FilePath = x.FilePath
                    });
                });

                var dtAttach = CommonLibrary.ListToDataTable<FundReturnAttachment>(tempAttachLine, data.userEmail, data.userAction, data.userActionDate, "FRAttachLines");

                if (da.createFundReturnHeaderData(dtMain, dtAttach))
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

        [HttpPost("createFundReturnApproval")]
        public async Task<IActionResult> createFundReturnApproval(QueryModel<FundReturnApprovalStream> data)
        {
            ResultModel<QueryModel<FundReturnApprovalStream>> res = new ResultModel<QueryModel<FundReturnApprovalStream>>();
            FundReturnDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                QueryModel<FundReturnApproval> temp = new();
                temp.Data = new();

                temp.Data = data.Data.Data;
                temp.userEmail = data.userEmail;
                temp.userAction = data.userAction;
                temp.userActionDate = data.userActionDate;

                if (da.createFundReturnApprovalData(temp, data.Data.LocationID))
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

        [HttpPost("deleteFundReturnDocument")]
        public async Task<IActionResult> deleteFundReturnDocument(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            FundReturnDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                string[] temp = CommonLibrary.Base64Decode(data.Data).Split("!_!");

                if (da.deleteFundReturnDocument(temp[0], temp[1], data.userEmail, data.userAction, data.userActionDate))
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

        [HttpGet("getFundReturnDocuments/{param}")]
        public async Task<IActionResult> getFundReturnDocuments(string param)
        {
            ResultModel<List<FundReturnDocument>> res = new ResultModel<List<FundReturnDocument>>();
            FundReturnDA da = new(_configuration);
            List<FundReturnDocument> fundReturnDocuments = new List<FundReturnDocument>();
            DataTable dtFundReturnDocument = new DataTable("FundReturnDocument");
            DataTable dtFundReturnItemLine = new DataTable("FundReturnItemLine");
            DataTable dtFundReturnApproval = new DataTable("FundReturnApproval");
            DataTable dtFundReturnAttachment = new DataTable("FundReturnAttachment");
            DataTable dtParam = new DataTable("Parameter");
            IActionResult actionResult = null;

            try
            {
                string[] temp = CommonLibrary.Base64Decode(param).Split("!_!");
                string loc = temp[0].Equals("") ? "HO" : temp[0];

                dtFundReturnDocument = da.getFundReturnHeaderbyFilterData(loc, temp[1], Convert.ToInt32(temp[2]), _rowPerPage);

                dtParam = dtFundReturnDocument.Copy();

                foreach (var removedCol in new[] {
                    "RequestDate",
                    "LocationID",
                    "CommercialType",
                    "CustomerName",
                    "CustomerType",
                    "CustomerMemberID",
                    "CustomerContactNo",
                    "FundReturnCategoryID",
                    "BankHolderName",
                    "BankAccount",
                    "BankID",
                    "ReceiptDocument",
                    "ExternalDocument",
                    "RefundAmount",
                    "TransactionAmount",
                    "Reason",
                    "DocumentStatus",
                    "AuditUser",
                    "AuditAction",
                    "AuditActionDate" })
                {
                    if (dtParam.Columns.Contains(removedCol))
                        dtParam.Columns.Remove(removedCol);
                }

                if (dtFundReturnDocument.Rows.Count <= 0)
                    throw new Exception("Fail Fetch POMFHeader Data");

                dtFundReturnItemLine = da.getFundReturnItemLinesData(loc, dtParam);

                //if (dtFundReturnItemLine.Rows.Count <= 0)
                //    throw new Exception("Fail Fetch POMFItemLine Data");

                dtFundReturnApproval = da.getFundReturnApprovalsData(loc, dtParam);

                //if (dtPOMFApproval.Rows.Count <= 0)
                //    throw new Exception("Fail Fetch POMFApproval Data");

                dtFundReturnAttachment = da.getFundReturnAttachmentData(loc, dtParam);

                if (dtFundReturnDocument.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtFundReturnDocument.Rows)
                    {
                        FundReturnDocument temp1 = new FundReturnDocument();

                        temp1.dataHeader.DocumentID = dt["DocumentID"].ToString();
                        temp1.dataHeader.RequestDate = Convert.ToDateTime(dt["RequestDate"]);
                        temp1.dataHeader.LocationID = dt["LocationID"].ToString();
                        temp1.dataHeader.CommercialType = dt["CommercialType"].ToString();
                        temp1.dataHeader.CustomerName = dt["CustomerName"].ToString();
                        temp1.dataHeader.CustomerType = dt["CustomerType"].ToString();
                        temp1.dataHeader.CustomerMemberID = dt["CustomerMemberID"].ToString();
                        temp1.dataHeader.CustomerContactNo = dt["CustomerContactNo"].ToString();
                        temp1.dataHeader.FundReturnCategoryID = dt["FundReturnCategoryID"].ToString();
                        temp1.dataHeader.BankHolderName = dt["BankHolderName"].ToString();
                        temp1.dataHeader.BankAccount = dt["BankAccount"].ToString();
                        temp1.dataHeader.BankID = dt["BankID"].ToString();
                        temp1.dataHeader.ReceiptDocument = dt.IsNull("ReceiptDocument") ? string.Empty : dt["ReceiptDocument"].ToString();
                        temp1.dataHeader.ExternalDocument = dt.IsNull("ExternalDocument") ? string.Empty : dt["ExternalDocument"].ToString();
                        temp1.dataHeader.RefundAmount = dt.IsNull("RefundAmount") ? decimal.Zero : Convert.ToDecimal(dt["RefundAmount"]);
                        temp1.dataHeader.TransactionAmount = dt.IsNull("TransactionAmount") ? decimal.Zero : Convert.ToDecimal(dt["TransactionAmount"]);
                        temp1.dataHeader.Reason = dt["Reason"].ToString();
                        temp1.dataHeader.DocumentStatus = dt["DocumentStatus"].ToString();

                        if (dtFundReturnItemLine.AsEnumerable().Where(y => y["DocumentID"].ToString().Equals(dt["DocumentID"].ToString())).ToList().Count > 0)
                        {
                            temp1.dataItemLines = dtFundReturnItemLine.AsEnumerable().Where(y => y["DocumentID"].ToString().Equals(dt["DocumentID"].ToString())).Select(x => new FundReturnItemLine
                            {
                                DocumentID = x["DocumentID"].ToString(),
                                LineNum = Convert.ToInt32(x["LineNum"]),
                                ItemCode = x["ItemCode"].ToString(),
                                ItemDescription = x["ItemDescription"].ToString(),
                                ItemQuantity = Convert.ToInt32(x["ItemQuantity"]),
                                UOM = x["UOM"].ToString(),
                                ItemAmount = Convert.ToDecimal(x["ItemAmount"]),
                                ItemDiscount = Convert.ToInt32(x["ItemDiscount"])
                            }).ToList();
                        }

                        if (dtFundReturnApproval.AsEnumerable().Where(y => y["DocumentID"].ToString().Equals(dt["DocumentID"].ToString())).ToList().Count > 0)
                        {
                            temp1.dataApproval = dtFundReturnApproval.AsEnumerable().Where(y => y["DocumentID"].ToString().Equals(dt["DocumentID"].ToString())).Select(x => new FundReturnApproval
                            {
                                DocumentID = x["DocumentID"].ToString(),
                                ApprovalAction = x["ApprovalAction"].ToString(),
                                Approver = x["Approver"].ToString(),
                                ApproveDate = Convert.ToDateTime(x["ApproveDate"])
                            }).ToList();
                        }

                        temp1.dataAttachmentLines = dtFundReturnAttachment.AsEnumerable().Where(y => y["DocumentID"].ToString().Equals(dt["DocumentID"].ToString())).Select(x => new FundReturnAttachment
                        {
                            DocumentID = x["DocumentID"].ToString(),
                            LineNum = Convert.ToInt32(x["LineNum"]),
                            UploadDate = Convert.ToDateTime(x["UploadDate"]),
                            FileExtension = x["FileExtention"].ToString(),
                            FilePath = x["FilePath"].ToString(),
                        }).ToList();

                        fundReturnDocuments.Add(temp1);
                    }

                    res.Data = fundReturnDocuments;
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

        [HttpGet("getFundReturnAttachment/{param}")]
        public async Task<IActionResult> getFundReturnAttachment(string param)
        {
            ResultModel<List<FundReturnAttachment>> res = new ResultModel<List<FundReturnAttachment>>();
            FundReturnDA da = new(_configuration);
            List<FundReturnAttachment> attachments = new List<FundReturnAttachment>();
            DataTable dtAttachment = new DataTable("FundReturnAttachment");
            IActionResult actionResult = null;

            try
            {
                string[] temp = CommonLibrary.Base64Decode(param).Split("!_!");

                DataTable tableParam = new("Param");
                tableParam.Columns.Add(new DataColumn("DocumentID", typeof(string)));

                DataRow tableRowParam = tableParam.NewRow();
                tableRowParam["DocumentID"] = temp[1];
                tableParam.Rows.Add(tableRowParam);

                dtAttachment = da.getFundReturnAttachmentData(temp[0], tableParam);

                if (dtAttachment.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtAttachment.Rows)
                    {
                        FundReturnAttachment temp1 = new FundReturnAttachment();

                        temp1.DocumentID = dt["DocumentID"].ToString();
                        temp1.LineNum = Convert.ToInt32(dt["LineNum"]);
                        temp1.UploadDate = Convert.ToDateTime(dt["UploadDate"]);
                        temp1.FileExtension = dt["FileExtention"].ToString();
                        temp1.FilePath = dt["FilePath"].ToString();

                        attachments.Add(temp1);
                    }

                    res.Data = attachments;
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

        [HttpGet("getFundReturnBankData")]
        public async Task<IActionResult> getFundReturnBank()
        {
            ResultModel<List<Bank>> res = new ResultModel<List<Bank>>();
            FundReturnDA da = new(_configuration);
            List<Bank> banks = new List<Bank>();
            DataTable dtBank = new DataTable("Banks");
            IActionResult actionResult = null;

            try
            {
                dtBank = da.getFundReturnBankData();

                if (dtBank.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtBank.Rows)
                    {
                        Bank temp1 = new();

                        temp1.BankID = dt["BankID"].ToString();
                        temp1.BankDescription = dt["BankDescription"].ToString();
                        temp1.BankShortName = dt["BankShortName"].ToString();
                        temp1.BankType = dt["BankType"].ToString();

                        banks.Add(temp1);
                    }

                    res.Data = banks;
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

        [HttpGet("getFundReturnCategory")]
        public async Task<IActionResult> getFundReturnCategory()
        {
            ResultModel<List<FundReturnCategory>> res = new ResultModel<List<FundReturnCategory>>();
            FundReturnDA da = new(_configuration);
            List<FundReturnCategory> fundReturnCategories = new List<FundReturnCategory>();
            DataTable dtFundReturnCategory = new DataTable("fundReturnCategory");
            IActionResult actionResult = null;

            try
            {
                dtFundReturnCategory = da.getFundReturnCategoryData();

                if (dtFundReturnCategory.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtFundReturnCategory.Rows)
                    {
                        FundReturnCategory temp1 = new();

                        temp1.FundReturnCategoryID = dt["FundReturnCategoryID"].ToString();
                        temp1.FundReturnCategoryDescription = dt["FundReturnCategoryDescription"].ToString();

                        fundReturnCategories.Add(temp1);
                    }

                    res.Data = fundReturnCategories;
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

        [HttpGet("getFundReturnModuleNumberOfPage/{param}")]
        public async Task<IActionResult> getFundReturnModuleNumberOfPage(string param)
        {
            ResultModel<int> res = new ResultModel<int>();
            FundReturnDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                string[] temp = CommonLibrary.Base64Decode(param).Split("!_!");
                string loc = temp[1].Equals("") ? "HO" : temp[1];

                res.Data = da.getFundReturnModuleNumberOfPageData(temp[0], loc, temp[2], _rowPerPage);

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
    }
}
