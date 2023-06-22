using BPIDA.DataAccess;
using BPIDA.Models.DbModel;
using BPIDA.Models.MainModel;
using BPIDA.Models.MainModel.CashierLogbook;
using BPIDA.Models.MainModel.Company;
using BPILibrary;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BPIDA.Controllers
{
    [Route("api/DA/CashierLogbook")]
    [ApiController]
    public class CashierLogbookController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CashierLogbookController(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpPost("createLogData")]
        public async Task<IActionResult> createLogDataTable(QueryModel<CashierLogData> data)
        {
            ResultModel<QueryModel<CashierLogData>> res = new ResultModel<QueryModel<CashierLogData>>();
            GeneralDA generalDA = new(_configuration);
            CashierLogbookDA da = new(_configuration);
            DataTable dtMainIdentity = new DataTable("Identity");
            string logID = string.Empty;
            string headerID = string.Empty;
            IActionResult actionResult = null;

            try
            {
                da.createCashierLogbookTableSchema(data.Data.LocationID);

                dtMainIdentity = generalDA.createIDData("CashierLogbook");

                if (dtMainIdentity.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtMainIdentity.Rows)
                    {
                        string zero = string.Empty;

                        for (int i = 0; i < (5 - Convert.ToInt32(dt["IDLength"])); i++)
                        {
                            zero = zero + "0";
                        }

                        logID = dt["Code"].ToString() + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + zero + dt["DocNumber"].ToString() + dt["Parity"].ToString();

                    }
                }

                data.Data.LogID = logID;

                QueryModel<CashierLogDataConv> temp1 = new();
                temp1.Data = new();

                temp1.Data.LogID = data.Data.LogID;
                temp1.Data.LocationID = data.Data.LocationID;
                temp1.Data.Applicant = data.Data.Applicant;
                temp1.Data.LogDate = data.Data.LogDate;
                temp1.Data.LogStatus = data.Data.LogStatus;
                temp1.Data.LogStatusDate = data.Data.LogStatusDate;
                temp1.userEmail = data.userEmail;
                temp1.userAction = data.userAction;
                temp1.userActionDate = data.userActionDate;

                da.createLogData(temp1, data.Data.LogType);

                List<CashierLogLineDetailConv> lines = new();
                List<CashierLogCategoryDetailConv> headers = new();

                foreach (var hd in data.Data.header)
                {
                    DataTable dtHeaderIdentity = new DataTable("HIdentity");
                    dtHeaderIdentity = generalDA.createIDData("CashierLogHeader");

                    if (dtHeaderIdentity.Rows.Count > 0)
                    {
                        foreach (DataRow dt in dtHeaderIdentity.Rows)
                        {
                            string zero = string.Empty;

                            for (int i = 0; i < (5 - Convert.ToInt32(dt["IDLength"])); i++)
                            {
                                zero = zero + "0";
                            }

                            headerID = dt["Code"].ToString() + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + zero + dt["DocNumber"].ToString() + dt["Parity"].ToString();

                        }
                    }

                    CashierLogCategoryDetailConv temp2 = new();

                    temp2.LogID = data.Data.LogID;
                    temp2.BrankasCategoryID = headerID;
                    temp2.AmountCategoryID = hd.AmountCategoryID;
                    temp2.HeaderAmount = hd.lines.Sum(x => x.LineAmount);
                    temp2.ActualAmount = hd.ActualAmount;
                    temp2.CategoryNote = hd.CategoryNote;

                    headers.Add(temp2);

                    int c = 1;

                    foreach (var dt in hd.lines)
                    {
                        CashierLogLineDetailConv temp3 = new();

                        temp3.BrankasCategoryID = headerID;
                        temp3.LineNum = c;
                        temp3.AmountSubCategoryID = dt.AmountSubCategoryID;
                        temp3.AmountType = dt.AmountType;
                        temp3.ShiftID = dt.ShiftID;
                        temp3.LineAmount = dt.LineAmount;

                        c++;
                        lines.Add(temp3);
                    }
                }

                da.createLogHeaderData(CommonLibrary.ListToDataTable<CashierLogCategoryDetailConv>(headers, data.userEmail, data.userAction, data.userActionDate, "Headers"), data.Data.LocationID);

                da.createLogLineData(CommonLibrary.ListToDataTable<CashierLogLineDetailConv>(lines, data.userEmail, data.userAction, data.userActionDate, "Lines"), data.Data.LocationID, logID);

                List<CashierLogApproval> apvLines = new();

                foreach (var apv in data.Data.approvals)
                {
                    apvLines.Add(new CashierLogApproval
                    {
                        LogID = logID,
                        LocationID = apv.LocationID,
                        ShiftID = apv.ShiftID,
                        CreateUser = apv.CreateUser,
                        CreateDate = apv.CreateDate,
                        ConfirmUser = apv.ConfirmUser,
                        ConfirmDate = apv.ConfirmDate,
                        ApproveNote = apv.ApproveNote
                    });
                }

                da.createBrankasApproveLogData(CommonLibrary.ListToDataTable<CashierLogApproval>(apvLines, data.userEmail, data.userAction, data.userActionDate, "Apv"), data.Data.LocationID);

                QueryModel<CashierLogAction> logAction = new();
                logAction.Data = new();

                logAction.Data.LogID = logID;
                logAction.Data.LocationID = data.Data.LocationID;
                logAction.Data.UserEmail = data.userEmail;
                logAction.Data.LogAction = "Created";
                logAction.Data.ActionDate = DateTime.Now;
                logAction.userEmail = data.userEmail;
                logAction.userAction = data.userAction;
                logAction.userActionDate = data.userActionDate;

                da.createBrankasActionLogData(logAction);

                res.Data = data;
                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);

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

        [HttpPost("editLogData")]
        public async Task<IActionResult> editLogData(QueryModel<CashierLogData> data)
        {
            ResultModel<QueryModel<CashierLogData>> res = new ResultModel<QueryModel<CashierLogData>>();
            GeneralDA generalDA = new(_configuration);
            CashierLogbookDA da = new(_configuration);
            string headerID = string.Empty;
            IActionResult actionResult = null;

            try
            {
               
                QueryModel<CashierLogDataConv> temp1 = new();
                temp1.Data = new();

                temp1.Data.LogID = data.Data.LogID;
                temp1.Data.LocationID = data.Data.LocationID;
                temp1.Data.Applicant = data.Data.Applicant;
                temp1.Data.LogDate = data.Data.LogDate;
                temp1.Data.LogStatus = data.Data.LogStatus;
                temp1.Data.LogStatusDate = data.Data.LogStatusDate;
                temp1.userEmail = data.userEmail;
                temp1.userAction = data.userAction;
                temp1.userActionDate = data.userActionDate;

                da.createLogData(temp1, data.Data.LogType);

                da.deleteBrankasDetailsandLinesByLogIDData(data.Data.LogID, data.Data.LocationID);

                List<CashierLogLineDetailConv> lines = new();
                List<CashierLogCategoryDetailConv> header = new();

                foreach (var hd in data.Data.header)
                {
                    DataTable dtHeaderIdentity = new DataTable("HIdentity");
                    dtHeaderIdentity = generalDA.createIDData("CashierLogHeader");

                    if (dtHeaderIdentity.Rows.Count > 0)
                    {
                        foreach (DataRow dt in dtHeaderIdentity.Rows)
                        {
                            string zero = string.Empty;

                            for (int i = 0; i < (5 - Convert.ToInt32(dt["IDLength"])); i++)
                            {
                                zero = zero + "0";
                            }

                            headerID = dt["Code"].ToString() + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + zero + dt["DocNumber"].ToString() + dt["Parity"].ToString();

                        }
                    }

                    CashierLogCategoryDetailConv temp2 = new();

                    temp2.LogID = data.Data.LogID;
                    temp2.BrankasCategoryID = headerID;
                    temp2.AmountCategoryID = hd.AmountCategoryID;
                    temp2.HeaderAmount = hd.lines.Sum(x => x.LineAmount);
                    temp2.ActualAmount = hd.ActualAmount;
                    temp2.CategoryNote = hd.CategoryNote;

                    header.Add(temp2);

                    int c = 1;

                    foreach (var dt in hd.lines)
                    {
                        CashierLogLineDetailConv temp3 = new();

                        temp3.BrankasCategoryID = headerID;
                        temp3.LineNum = c;
                        temp3.AmountSubCategoryID = dt.AmountSubCategoryID;
                        temp3.AmountType = dt.AmountType;
                        temp3.ShiftID = dt.ShiftID;
                        temp3.LineAmount = dt.LineAmount;

                        c++;
                        lines.Add(temp3);
                    }
                }

                da.createLogHeaderData(CommonLibrary.ListToDataTable<CashierLogCategoryDetailConv>(header, data.userEmail, data.userAction, data.userActionDate, "Headers"), data.Data.LocationID);
                da.createLogLineData(CommonLibrary.ListToDataTable<CashierLogLineDetailConv>(lines, data.userEmail, data.userAction, data.userActionDate, "Lines"), data.Data.LocationID, data.Data.LogID);

                List<CashierLogApproval> apvLines = new();

                foreach (var apv in data.Data.approvals)
                {
                    apvLines.Add(new CashierLogApproval
                    {
                        LogID = data.Data.LogID,
                        LocationID = apv.LocationID,
                        ShiftID = apv.ShiftID,
                        CreateUser = apv.CreateUser,
                        CreateDate = apv.CreateDate,
                        ConfirmUser = apv.ConfirmUser,
                        ConfirmDate = apv.ConfirmDate,
                        ApproveNote = apv.ApproveNote
                    });
                }

                da.createBrankasApproveLogData(CommonLibrary.ListToDataTable<CashierLogApproval>(apvLines, data.userEmail, data.userAction, data.userActionDate, "Apv"), data.Data.LocationID);

                QueryModel<CashierLogAction> logAction = new();
                logAction.Data = new();

                logAction.Data.LogID = data.Data.LogID;
                logAction.Data.LocationID = data.Data.LocationID;
                logAction.Data.UserEmail = data.userEmail;
                logAction.Data.LogAction = "Edited";
                logAction.Data.ActionDate = DateTime.Now;
                logAction.userEmail = data.userEmail;
                logAction.userAction = data.userAction;
                logAction.userActionDate = data.userActionDate;

                da.createBrankasActionLogData(logAction);

                res.Data = data;
                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);

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

        [HttpPost("editBrankasDocumentStatus")]
        public async Task<IActionResult> updateBrankasDocumentStatusDataTable(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            CashierLogbookDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                string temp = CommonLibrary.Base64Decode(data.Data);

                string logid = temp.Split("!_!")[0];
                string loc = temp.Split("!_!")[1].Equals("") ? "HO" : temp.Split("!_!")[1];
                string statVal = temp.Split("!_!")[2];

                da.updateBrankasDocumentStatusData(logid, loc, statVal, data.userEmail, data.userAction, data.userActionDate);

                QueryModel<CashierLogAction> logAction = new();
                logAction.Data = new();

                logAction.Data.LogID = logid;
                logAction.Data.LocationID = loc;
                logAction.Data.UserEmail = data.userEmail;
                logAction.Data.LogAction = "Archived";
                logAction.Data.ActionDate = DateTime.Now;
                logAction.userEmail = data.userEmail;
                logAction.userAction = data.userAction;
                logAction.userActionDate = data.userActionDate;

                da.createBrankasActionLogData(logAction);

                res.Data = data;
                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);

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

        [HttpGet("getLogData/{locPage}")]
        public async Task<IActionResult> getLogDataTable(string locPage)
        {
            ResultModel<List<CashierLogData>> res = new ResultModel<List<CashierLogData>>();
            CashierLogbookDA da = new(_configuration);
            List<CashierLogData> cashierLogbooks = new List<CashierLogData>();
            DataTable dtCashierLogbook = new DataTable("CashierLogbook");
            bool flag = true;
            IActionResult actionResult = null;

            try
            {
                string temp = CommonLibrary.Base64Decode(locPage);

                string type = temp.Split("!_!")[0];
                string loc = temp.Split("!_!")[1].Equals("") ? "HO" : temp.Split("!_!")[1];
                string status = temp.Split("!_!")[2];
                string cond = temp.Split("!_!")[3];
                int page = Convert.ToInt32(temp.Split("!_!")[4]);

                dtCashierLogbook = da.getLogData(type, loc, status, cond, page);

                if (dtCashierLogbook.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtCashierLogbook.Rows)
                    {
                        CashierLogData temp1 = new();

                        temp1.LogType = dt["LogType"].ToString();
                        temp1.LogID = dt["LogID"].ToString();
                        temp1.LocationID = dt["LocationID"].ToString();
                        temp1.Applicant = dt["Applicant"].ToString();
                        temp1.LogDate = Convert.ToDateTime(dt["LogDate"]);
                        temp1.LogStatus = dt["LogStatus"].ToString();
                        temp1.LogStatusDate = dt.IsNull("LogStatusDate") ? DateTime.MinValue : Convert.ToDateTime(dt["LogStatusDate"]);

                        DataTable dtHeader = new DataTable("Header");
                        List<CashierLogCategoryDetail> cashierLogCategoryDetail = new();

                        dtHeader = da.getLogHeaderData(dt["LogID"].ToString(), loc);
                        temp1.header = new();

                        if (dtCashierLogbook.Rows.Count > 0)
                        {
                            foreach (DataRow rHeader in dtHeader.Rows)
                            {
                                CashierLogCategoryDetail temp2 = new();

                                temp2.LogID = rHeader["LogID"].ToString();
                                temp2.BrankasCategoryID = rHeader["BrankasCategoryID"].ToString();
                                temp2.AmountCategoryID = rHeader["AmountCategoryID"].ToString();
                                temp2.AmountCategoryName = rHeader["CategoryName"].ToString();
                                temp2.HeaderAmount = Convert.ToDecimal(rHeader["HeaderAmount"]);
                                temp2.ActualAmount = Convert.ToDecimal(rHeader["ActualAmount"]);
                                temp2.CategoryNote = rHeader.IsNull("CategoryNote") ? "" : rHeader["CategoryNote"].ToString();

                                DataTable dtLines = new DataTable("Lines");
                                List<CashierLogLineDetail> cashierLogLineDetail = new();
                                dtLines = da.getLogLineData(rHeader["BrankasCategoryID"].ToString(), loc);
                                temp2.lines = new();

                                if (dtLines.Rows.Count > 0)
                                {
                                    foreach (DataRow rLine in dtLines.Rows)
                                    {
                                        CashierLogLineDetail temp3 = new();

                                        temp3.BrankasCategoryID = rLine["BrankasCategoryID"].ToString();
                                        temp3.LineNo = Convert.ToInt32(rLine["LineNum"]);
                                        temp3.AmountSubCategoryID = rLine["AmountSubCategoryID"].ToString();
                                        temp3.AmountSubCategoryName = rLine["CategoryName"].ToString();
                                        temp3.AmountType = rLine["AmountType"].ToString();
                                        temp3.AmountDesc = rLine["AmountDesc"].ToString();
                                        temp3.ShiftID = Convert.ToInt32(rLine["ShiftID"]);
                                        temp3.ShiftDesc = rLine["ShiftDesc"].ToString();
                                        temp3.LineAmount = Convert.ToDecimal(rLine["LineAmount"]);

                                        cashierLogLineDetail.Add(temp3);
                                    }

                                    temp2.lines = cashierLogLineDetail;
                                }
                                else
                                {
                                    flag = false;
                                }

                                cashierLogCategoryDetail.Add(temp2);
                            }

                            temp1.header = cashierLogCategoryDetail;
                        }
                        else
                        {
                            flag = false;
                        }

                        DataTable dtApv = new DataTable("Approval");
                        List<CashierLogApproval> cashierLogApproval = new();

                        dtApv = da.getLogApprovalData(dt["LogID"].ToString(), loc);

                        if (dtApv.Rows.Count > 0)
                        {
                            foreach (DataRow rApproval in dtApv.Rows)
                            {
                                CashierLogApproval temp4 = new();

                                temp4.LogID = rApproval["LogID"].ToString();
                                temp4.ShiftID = Convert.ToInt32(rApproval["ShiftID"]);
                                temp4.LocationID = rApproval["LocationID"].ToString();
                                temp4.CreateUser = rApproval["CreateUser"].ToString();
                                temp4.CreateDate = Convert.ToDateTime(rApproval["CreateDate"]);
                                temp4.ConfirmUser = rApproval.IsNull("ConfirmUser") ? "" : rApproval["ConfirmUser"].ToString();
                                temp4.ConfirmDate = rApproval.IsNull("ConfirmDate") ? DateTime.MinValue : Convert.ToDateTime(rApproval["ConfirmDate"]);
                                temp4.ApproveNote = rApproval.IsNull("ApproveNote") ? "" : rApproval["ApproveNote"].ToString();

                                cashierLogApproval.Add(temp4);
                            }

                            temp1.approvals = cashierLogApproval;
                        }
                        else
                        {
                            temp1.approvals = null;
                            flag = false;
                        }

                        cashierLogbooks.Add(temp1);
                    }
                }
                else
                {
                    flag = false;
                }

                if (flag)
                {
                    res.Data = cashierLogbooks;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = cashierLogbooks;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Fail Fetch Data";

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

        [HttpGet("getShiftbyModule/{moduleName}")]
        public async Task<IActionResult> getShiftbyModuleData(string moduleName)
        {
            ResultModel<List<Shift>> res = new ResultModel<List<Shift>>();
            CashierLogbookDA da = new(_configuration);
            List<Shift> shiftLines = new List<Shift>();
            DataTable dtShift = new DataTable("Shift");
            IActionResult actionResult = null;

            try
            {
                dtShift = da.getShiftbyModule(moduleName);

                if (dtShift.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtShift.Rows)
                    {
                        Shift temp = new Shift();

                        temp.ShiftID = Convert.ToInt32(dt["ShiftID"]);
                        temp.ShiftDesc = dt["ShiftDesc"].ToString();

                        shiftLines.Add(temp);
                    }

                    res.Data = shiftLines;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

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

        [HttpGet("getCashierLogbookCategories")]
        public async Task<IActionResult> getCashierLogbookCategories()
        {
            ResultModel<CashierLogbookCategories> res = new ResultModel<CashierLogbookCategories>();
            CashierLogbookDA da = new(_configuration);
            List<AmountCategories> categoryLines = new List<AmountCategories>();
            List<AmountSubCategories> subCategoryLines = new List<AmountSubCategories>();
            List<AmountTypes> typeLines = new List<AmountTypes>();
            DataTable dtCategory = new DataTable("Category");
            DataTable dtSubCategory = new DataTable("SubCategory");
            DataTable dtType = new DataTable("Type");
            IActionResult actionResult = null;

            try
            {
                dtCategory = da.getAmountCategories();
                dtSubCategory = da.getAmountSubCategories();
                dtType = da.getAmountType();

                if (dtCategory.Rows.Count > 0 && dtSubCategory.Rows.Count > 0 && dtType.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtCategory.Rows)
                    {
                        AmountCategories temp1 = new AmountCategories();

                        temp1.AmountCategoryID = dt["AmountCategoryID"].ToString();
                        temp1.AmountCategoryName = dt["CategoryName"].ToString();

                        categoryLines.Add(temp1);
                    }

                    foreach (DataRow dt in dtSubCategory.Rows)
                    {
                        AmountSubCategories temp2 = new AmountSubCategories();

                        temp2.AmountSubCategoryID = dt["AmountSubCategoryID"].ToString();
                        temp2.AmountSubCategoryName = dt["CategoryName"].ToString();
                        temp2.AmountType = dt["AmountType"].ToString();

                        subCategoryLines.Add(temp2);
                    }

                    foreach (DataRow dt in dtType.Rows)
                    {
                        AmountTypes temp3 = new AmountTypes();

                        temp3.AmountType = dt["AmountType"].ToString();
                        temp3.AmountDesc = dt["AmountDesc"].ToString();

                        typeLines.Add(temp3);
                    }

                    res.Data = new();
                    res.Data.categories = new();
                    res.Data.subCategories = new();
                    res.Data.types = new();

                    res.Data.categories = categoryLines;
                    res.Data.subCategories = subCategoryLines;
                    res.Data.types = typeLines;

                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

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

        [HttpGet("getBrankasActionLogData/{locPage}")]
        public async Task<IActionResult> getBrankasActionLogDataTable(string locPage)
        {
            ResultModel<List<CashierLogAction>> res = new ResultModel<List<CashierLogAction>>();
            CashierLogbookDA da = new(_configuration);
            List<CashierLogAction> actionLines = new List<CashierLogAction>();
            DataTable dtAction = new DataTable("Action");
            IActionResult actionResult = null;

            try
            {
                string temp = CommonLibrary.Base64Decode(locPage);

                string loc = temp.Split("!_!")[0].Equals("") ? "HO" : temp.Split("!_!")[0];
                string orderby = temp.Split("!_!")[1];
                string filType = temp.Split("!_!")[2];
                string filVal = temp.Split("!_!")[3];
                int page = Convert.ToInt32(temp.Split("!_!")[4]);

                dtAction = da.getBrankasActionLogData(loc, orderby, filType, filVal, page);

                if (dtAction.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtAction.Rows)
                    {
                        CashierLogAction temp1 = new CashierLogAction();

                        temp1.LogType = dt["LogType"].ToString();
                        temp1.LogID = dt["LogID"].ToString();
                        temp1.LocationID = dt["LocationID"].ToString();
                        temp1.UserEmail = dt["UserEmail"].ToString();
                        temp1.LogAction = dt["LogAction"].ToString();
                        temp1.ActionDate = Convert.ToDateTime(dt["ActionDate"]);

                        actionLines.Add(temp1);
                    }

                    res.Data = actionLines;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

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
            CashierLogbookDA da = new(_configuration);
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

        [HttpGet("getNumberofLogExisting/{param}")]
        public async Task<IActionResult> getNumberofLogExistingData(string param)
        {
            ResultModel<int> res = new ResultModel<int>();
            CashierLogbookDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                string loc = CommonLibrary.Base64Decode(param).Split("!_!")[0];
                string cond = CommonLibrary.Base64Decode(param).Split("!_!")[1];

                res.Data = da.getNumberofLogExisting(loc, cond);

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

        [HttpPost("editBrankasApproveLogOnConfirm")]
        public async Task<IActionResult> editBrankasApproveLogOnConfirm(QueryModel<CashierLogApproval> data)
        {
            ResultModel<QueryModel<CashierLogApproval>> res = new ResultModel<QueryModel<CashierLogApproval>>();
            CashierLogbookDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                da.editBrankasApproveLogOnConfirmData(data);

                QueryModel<CashierLogAction> logAction = new();
                logAction.Data = new();

                logAction.Data.LogID = data.Data.LogID;
                logAction.Data.LocationID = data.Data.LocationID;
                logAction.Data.UserEmail = data.userEmail;
                logAction.Data.LogAction = "Confirmed";
                logAction.Data.ActionDate = DateTime.Now;
                logAction.userEmail = data.userEmail;
                logAction.userAction = data.userAction;
                logAction.userActionDate = data.userActionDate;

                da.createBrankasActionLogData(logAction);

                res.Data = data;
                res.isSuccess = true;
                res.ErrorCode = "00";
                res.ErrorMessage = "";

                actionResult = Ok(res);

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

        //
    }
}
