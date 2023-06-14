using BPIDA.DataAccess;
using BPIDA.Models.DbModel;
using BPIDA.Models.MainModel;
using BPIDA.Models.MainModel.Company;
using BPIDA.Models.MainModel.Mailing;
using BPIDA.Models.MainModel.PettyCash;
using BPILibrary;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BPIDA.Controllers
{
    [Route("api/DA/PettyCash")]
    [ApiController]
    public class PettyCashController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PettyCashController(IConfiguration config)
        {
            _configuration = config;
        }

        [HttpGet("createID/{docType}")]
        public async Task<IActionResult> createID(string docType)
        {
            ResultModel<string> res = new ResultModel<string>();
            GeneralDA generalDA = new(_configuration);
            string code = string.Empty;
            DataTable dtIdentity = new DataTable("Identity");
            IActionResult actionResult = null;

            try
            {

                dtIdentity = generalDA.createIDData(docType);

                if (dtIdentity.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtIdentity.Rows)
                    {
                        string zero = string.Empty;

                        for (int i = 0; i < (7 - Convert.ToInt32(dt["IDLength"])); i++)
                        {
                            zero = zero + "0";
                        }

                        code = dt["Code"].ToString() + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + zero + dt["DocNumber"].ToString() + dt["Parity"].ToString();

                    }

                    res.Data = code;
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

        [HttpPost("createAdvanceData")]
        public async Task<IActionResult> createAdvanceDataTable(QueryModel<Advance> data)
        {
            ResultModel<QueryModel<Advance>> res = new ResultModel<QueryModel<Advance>>();
            PettyCashDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {

                if (da.createPettycashAdvanceDocument(data, CommonLibrary.ListToDataTable<AdvanceLine>(data.Data.lines, data.userEmail, data.userAction, data.userActionDate, "AdvanceLine")))
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

        [HttpPost("createExpenseData")]
        public async Task<IActionResult> createExpenseDataTable(QueryModel<Expense> data)
        {
            ResultModel<QueryModel<Expense>> res = new ResultModel<QueryModel<Expense>>();
            PettyCashDA da = new(_configuration);
            List<ExpenseAttachmentLine> dtLine = new();
            IActionResult actionResult = null;

            try
            {
                if (da.createPettycashExpenseDocument(
                    data
                    , CommonLibrary.ListToDataTable<ExpenseLine>(data.Data.lines, data.userEmail, data.userAction, data.userActionDate, "ExpenseLine")
                    , CommonLibrary.ListToDataTable<ExpenseAttachmentLine>(data.Data.attach, data.userEmail, data.userAction, data.userActionDate, "ExpenseAttach"))
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

        [HttpPost("createExpenseAttachLineData")]
        public async Task<IActionResult> createExpenseLineDataTable(QueryModel<List<BPIDA.Models.MainModel.Stream.FileStream>> data)
        {
            ResultModel<QueryModel<List<BPIDA.Models.MainModel.Stream.FileStream>>> res = new ResultModel<QueryModel<List<BPIDA.Models.MainModel.Stream.FileStream>>>();
            PettyCashDA da = new(_configuration);
            List<ExpenseAttachmentLine> dtLine = new();
            IActionResult actionResult = null;

            try
            {
                
                foreach (var dt in data.Data)
                {
                    ExpenseAttachmentLine temp = new();

                    temp.ExpenseID = dt.type;
                    temp.PathFile = dt.fileName;

                    dtLine.Add(temp);
                }

                da.createExpenseAttachLine(CommonLibrary.ListToDataTable<ExpenseAttachmentLine>(dtLine, data.userEmail, data.userAction, data.userActionDate, "ExpenseAttach"));

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

        [HttpPost("createReimburseData")]
        public async Task<IActionResult> createReimburseDataTable(QueryModel<Reimburse> data)
        {
            ResultModel<QueryModel<Reimburse>> res = new ResultModel<QueryModel<Reimburse>>();
            PettyCashDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                List<ExpenseIds> temp = new();
                data.Data.lines.DistinctBy(y => y.ExpenseID).ToList().ForEach(x =>
                {
                    temp.Add(new ExpenseIds
                    {
                        ExpenseID = x.ExpenseID
                    });
                });

                if (da.createPettycashReimburseDocument(
                    data
                    , CommonLibrary.ListToDataTable<ExpenseIds>(temp, data.userEmail, data.userAction, data.userActionDate, "ExpenseId")
                    , CommonLibrary.ListToDataTable<ReimburseLine>(data.Data.lines, data.userEmail, data.userAction, data.userActionDate, "ReimburseLine")
                    , CommonLibrary.ListToDataTable<ReimburseAttachmentLine>(data.Data.attach, data.userEmail, data.userAction, data.userActionDate, "ReimburseAttach")
                ))
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

        [HttpPost("createReimburseLineData")]
        public async Task<IActionResult> createReimburseLineDataTable(QueryModel<List<ReimburseLine>> data)
        {
            ResultModel<QueryModel<List<ReimburseLine>>> res = new ResultModel<QueryModel<List<ReimburseLine>>>();
            PettyCashDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                da.createReimburseLine(CommonLibrary.ListToDataTable<ReimburseLine>(data.Data, data.userEmail, data.userAction, data.userActionDate, "ReimburseLine"));

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

        [HttpPost("createReimburseAttachLineData")]
        public async Task<IActionResult> createReimburseLineDataTable(QueryModel<List<BPIDA.Models.MainModel.Stream.FileStream>> data)
        {
            ResultModel<QueryModel<List<BPIDA.Models.MainModel.Stream.FileStream>>> res = new ResultModel<QueryModel<List<BPIDA.Models.MainModel.Stream.FileStream>>>();
            PettyCashDA da = new(_configuration);
            List<ReimburseAttachmentLine> dtLine = new();
            IActionResult actionResult = null;

            try
            {
                foreach (var dt in data.Data)
                {
                    ReimburseAttachmentLine temp = new();

                    string exp = dt.type.Split("!_!")[0];
                    string rmb = dt.type.Split("!_!")[1];

                    temp.ReimburseID = rmb;
                    temp.ExpenseID = exp;
                    temp.PathFile = dt.fileName;

                    dtLine.Add(temp);
                }

                da.createReimburseAttachLine(CommonLibrary.ListToDataTable<ReimburseAttachmentLine>(dtLine, data.userEmail, data.userAction, data.userActionDate, "ReimburseAttach"));

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

        [HttpPost("AdvanceSettlement")]
        public async Task<IActionResult> updateAdvanceDataSettlement(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            PettyCashDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                da.updateSettleAdvance(data);

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

        [HttpPost("ExpenseSettlement")]
        public async Task<IActionResult> updateExpenseDataSettlement(QueryModel<List<string>> data)
        {
            ResultModel<QueryModel<List<string>>> res = new ResultModel<QueryModel<List<string>>>();
            PettyCashDA da = new(_configuration);

            IActionResult actionResult = null;

            try
            {
                foreach (var id in data.Data)
                {
                    da.updateSettleExpense(id, data.userEmail, data.userAction, data.userActionDate);
                }

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

        [HttpPost("ReimburseSettlement")]
        public async Task<IActionResult> updateReimburseDataSettlement(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            PettyCashDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                da.updateSettleReimburse(data);

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

        [HttpPost("editDocumentStatus")]
        public async Task<IActionResult> editDocumentStatusData(QueryModel<string> data)
        {
            ResultModel<QueryModel<string>> res = new ResultModel<QueryModel<string>>();
            PettyCashDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                string TbName = data.Data.Split("!_!")[0];
                string id = data.Data.Split("!_!")[1];
                string value = data.Data.Split("!_!")[2];
                string note = data.Data.Split("!_!")[3];

                bool flag = da.updateDocumentStatus(TbName, id, value, note, data.userEmail, data.userAction, data.userActionDate);

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
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Data Might have been Approved";

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

        [HttpPost("editReimburseLine")]
        public async Task<IActionResult> editReimburseLineData(QueryModel<Reimburse> data)
        {
            ResultModel<QueryModel<Reimburse>> res = new ResultModel<QueryModel<Reimburse>>();
            PettyCashDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                foreach (var id in data.Data.lines)
                {
                    QueryModel<ReimburseLine> temp = new();

                    temp.Data = id;
                    temp.userEmail = data.userEmail;
                    temp.userAction = data.userAction;
                    temp.userActionDate = data.userActionDate;

                    da.updateReimburseLine(temp);
                }

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

        [HttpPost("getLedgerDataEntriesbyDate")]
        public async Task<IActionResult> getLedgerDataEntriesbyDate(List<ledgerParam> data)
        {
            ResultModel<List<PettyCashLedger>> res = new ResultModel<List<PettyCashLedger>>();
            PettyCashDA da = new(_configuration);
            List<PettyCashLedger> pettyCashLedgers = new List<PettyCashLedger>();
            DataTable dtPettyCashLedger = new DataTable("PettyCashLedger");
            bool flag = false;
            IActionResult actionResult = null;

            try
            {
                foreach (var dat in data)
                {
                    dtPettyCashLedger = da.getPettyCashLedgerDatabyDate(dat);

                    if (dtPettyCashLedger.Rows.Count > 0)
                    {
                        flag = true;

                        foreach (DataRow dt in dtPettyCashLedger.Rows)
                        {
                            PettyCashLedger temp1 = new PettyCashLedger();

                            temp1.TransactionDate = Convert.ToDateTime(dt["TransactionDate"]);
                            temp1.TransactionType = dt["TransactionType"].ToString();
                            temp1.DocumentID = dt["DocumentID"].ToString();
                            temp1.LocationID = dt["LocationID"].ToString();
                            temp1.Amount = Convert.ToDecimal(dt["Amount"]);
                            temp1.Actor = dt["Actor"].ToString();
                            temp1.ExternalDocument = dt.IsNull("ExternalDocument") ? "" : dt["ExternalDocument"].ToString();
                            temp1.Applicant = dt["Applicant"].ToString();
                            temp1.DocumentDate = Convert.ToDateTime(dt["DocumentDate"]);
                            temp1.DepartmentID = dt.IsNull("DepartmentID") ? "" : dt["DepartmentID"].ToString();
                            temp1.Note = dt.IsNull("Note") ? "" : dt["Note"].ToString();
                            temp1.NIK = dt.IsNull("NIK") ? "" : dt["NIK"].ToString();
                            temp1.Type = dt.IsNull("Type") ? "" : dt["Type"].ToString();
                            temp1.BankAccount = dt.IsNull("BankAccount") ? "" : dt["BankAccount"].ToString();
                            temp1.Status = dt["Status"].ToString();

                            pettyCashLedgers.Add(temp1);
                        }
                    }
                    else
                    {
                        flag = false;
                    }
                }

                if (flag)
                {
                    res.Data = pettyCashLedgers;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = pettyCashLedgers;
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "At Least 1 Location Have 0 Data";

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

        [HttpGet("getAttachmentLines/{Id}")]
        public async Task<IActionResult> getAttachmentLinesbyID(string Id)
        {
            ResultModel<List<AttachmentLine>> res = new ResultModel<List<AttachmentLine>>();
            PettyCashDA da = new(_configuration);
            List<AttachmentLine> attachmentLines = new List<AttachmentLine>();
            DataTable dtAttachmentLine = new DataTable("AttachmentLine");
            IActionResult actionResult = null;

            try
            {
                string temp = CommonLibrary.Base64Decode(Id);

                dtAttachmentLine = da.getAttachmentLines(temp.Split("!_!")[0], temp.Split("!_!")[1]);

                if (dtAttachmentLine.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtAttachmentLine.Rows)
                    {
                        AttachmentLine temp1 = new AttachmentLine();

                        temp1.ExpenseID = dt["ExpenseID"].ToString();
                        temp1.PathFile = dt["PathFile"].ToString();

                        attachmentLines.Add(temp1);
                    }

                    res.Data = attachmentLines;
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

        [HttpGet("getAdvanceDatabyLocation/{locPage}")]
        public async Task<IActionResult> getAdvanceDataTablebyLocation(string locPage)
        {
            ResultModel<List<Advance>> res = new ResultModel<List<Advance>>();
            PettyCashDA da = new(_configuration);
            List<Advance> advance = new List<Advance>();
            DataTable dtAdvance = new DataTable("AdvanceData");
            IActionResult actionResult = null;

            try
            {
                var temp = CommonLibrary.Base64Decode(locPage);

                string denom = temp.Split("!_!")[0];
                string loc = temp.Split("!_!")[1].Equals("") ? "HO" : temp.Split("!_!")[1];
                string val = temp.Split("!_!")[2];
                string tp = temp.Split("!_!")[3];
                string filVal = temp.Split("!_!")[4];
                int page = Convert.ToInt32(temp.Split("!_!")[5]);

                dtAdvance = da.getAdvanceDatabyLocation(denom, loc, val, tp, filVal, page);

                if (dtAdvance.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtAdvance.Rows)
                    {
                        Advance temp1 = new Advance();
                        temp1.statusDetails = new();

                        temp1.AdvanceID = dt["AdvanceID"].ToString();
                        temp1.LocationID = dt["LocationID"].ToString();
                        temp1.Approver = dt["Approver"].ToString();
                        temp1.AdvanceDate = Convert.ToDateTime(dt["AdvanceDate"]);
                        temp1.DepartmentID = dt["DepartmentID"].ToString();
                        temp1.AdvanceNIK = dt["AdvanceNIK"].ToString();
                        temp1.AdvanceNote = dt["AdvanceNote"].ToString();
                        temp1.AdvanceType = dt["AdvanceType"].ToString();
                        temp1.TypeAccount = dt["TypeAccount"].ToString();
                        temp1.AdvanceStatus = dt["AdvanceStatus"].ToString();
                        temp1.Applicant = dt["Applicant"].ToString();

                        temp1.statusDetails.submitDate = dt.IsNull("SubmitDate") ? DateTime.MinValue : Convert.ToDateTime(dt["SubmitDate"]);
                        temp1.statusDetails.submitUser = dt.IsNull("SubmitUser") ? "" : dt["SubmitUser"].ToString();
                        temp1.statusDetails.confirmDate = dt.IsNull("ConfirmDate") ? DateTime.MinValue : Convert.ToDateTime(dt["ConfirmDate"]);
                        temp1.statusDetails.confirmUser = dt.IsNull("ConfirmUser") ? "" : dt["ConfirmUser"].ToString();
                        temp1.statusDetails.rejectDate = dt.IsNull("RejectDate") ? DateTime.MinValue : Convert.ToDateTime(dt["RejectDate"]);
                        temp1.statusDetails.rejectUser = dt.IsNull("RejectUser") ? "" : dt["RejectUser"].ToString();

                        List<AdvanceLine> advanceLines = new List<AdvanceLine>();
                        DataTable dtAdvanceLine = new DataTable("AdvanceLine");

                        dtAdvanceLine = da.getAdvanceLinesbyID(dt["AdvanceID"].ToString(), denom);

                        if (dtAdvanceLine.Rows.Count > 0)
                        {
                            foreach (DataRow linedt in dtAdvanceLine.Rows)
                            {
                                AdvanceLine temp2 = new AdvanceLine();

                                temp2.AdvanceID = linedt["AdvanceID"].ToString();
                                temp2.LineNo = Convert.ToInt32(linedt["LineNum"]);
                                temp2.Details = linedt["Details"].ToString();
                                temp2.Amount = Convert.ToDecimal(linedt["Amount"]);
                                temp2.Status = linedt["AStatus"].ToString();

                                advanceLines.Add(temp2);
                            }
                        }

                        temp1.lines = new List<AdvanceLine>();
                        temp1.lines = advanceLines;

                        advance.Add(temp1);
                    }

                    res.Data = advance;
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

        [HttpGet("getAdvanceDatabyUser/{user}")]
        public async Task<IActionResult> getAdvanceDataTablebyUser(string user)
        {
            ResultModel<List<Advance>> res = new ResultModel<List<Advance>>();
            PettyCashDA da = new(_configuration);
            List<Advance> advance = new List<Advance>();
            DataTable dtAdvance = new DataTable("AdvanceData");
            IActionResult actionResult = null;

            try
            {
                var temp = CommonLibrary.Base64Decode(user);

                dtAdvance = da.getAdvanceDatabyUser(temp.Split("!_!")[0]);

                if (dtAdvance.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtAdvance.Rows)
                    {
                        Advance temp1 = new Advance();
                        temp1.statusDetails = new();

                        temp1.AdvanceID = dt["AdvanceID"].ToString();
                        temp1.LocationID = dt["LocationID"].ToString();
                        temp1.AdvanceDate = Convert.ToDateTime(dt["AdvanceDate"]);
                        temp1.DepartmentID = dt["DepartmentID"].ToString();
                        temp1.AdvanceNIK = dt["AdvanceNIK"].ToString();
                        temp1.AdvanceNote = dt["AdvanceNote"].ToString();
                        temp1.AdvanceType = dt["AdvanceType"].ToString();
                        temp1.TypeAccount = dt["TypeAccount"].ToString();
                        temp1.AdvanceStatus = dt["AdvanceStatus"].ToString();
                        temp1.Applicant = dt["Applicant"].ToString();

                        temp1.statusDetails.submitDate = dt.IsNull("SubmitDate") ? DateTime.MinValue : Convert.ToDateTime(dt["SubmitDate"]);
                        temp1.statusDetails.submitUser = dt.IsNull("SubmitUser") ? "" : dt["SubmitUser"].ToString();
                        temp1.statusDetails.confirmDate = dt.IsNull("ConfirmDate") ? DateTime.MinValue : Convert.ToDateTime(dt["ConfirmDate"]);
                        temp1.statusDetails.confirmUser = dt.IsNull("ConfirmUser") ? "" : dt["ConfirmUser"].ToString();
                        temp1.statusDetails.rejectDate = dt.IsNull("RejectDate") ? DateTime.MinValue : Convert.ToDateTime(dt["RejectDate"]);
                        temp1.statusDetails.rejectUser = dt.IsNull("RejectUser") ? "" : dt["RejectUser"].ToString();

                        List<AdvanceLine> advanceLines = new List<AdvanceLine>();
                        DataTable dtAdvanceLine = new DataTable("AdvanceLine");

                        dtAdvanceLine = da.getAdvanceLinesbyID(dt["AdvanceID"].ToString(), temp.Split("!_!")[1]);

                        if (dtAdvanceLine.Rows.Count > 0)
                        {
                            foreach (DataRow linedt in dtAdvanceLine.Rows)
                            {
                                AdvanceLine temp2 = new AdvanceLine();

                                temp2.AdvanceID = linedt["AdvanceID"].ToString();
                                temp2.LineNo = Convert.ToInt32(linedt["LineNum"]);
                                temp2.Details = linedt["Details"].ToString();
                                temp2.Amount = Convert.ToDecimal(linedt["Amount"]);
                                temp2.Status = linedt["AStatus"].ToString();

                                advanceLines.Add(temp2);
                            }
                        }

                        temp1.lines = new List<AdvanceLine>();
                        temp1.lines = advanceLines;

                        advance.Add(temp1);
                    }

                    res.Data = advance;
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

        [HttpGet("getExpenseDatabyLocation/{locPage}")]
        public async Task<IActionResult> getExpenseDataTablebyLocation(string locPage)
        {
            ResultModel<List<Expense>> res = new ResultModel<List<Expense>>();
            PettyCashDA da = new(_configuration);
            List<Expense> expense = new List<Expense>();
            DataTable dtExpense = new DataTable("ExpenseData");
            IActionResult actionResult = null;

            try
            {
                var temp = CommonLibrary.Base64Decode(locPage);

                string denom = temp.Split("!_!")[0];
                string loc = temp.Split("!_!")[1].Equals("") ? "HO" : temp.Split("!_!")[1];
                string val = temp.Split("!_!")[2];
                string tp = temp.Split("!_!")[3];
                string filVal = temp.Split("!_!")[4];
                int page = Convert.ToInt32(temp.Split("!_!")[5]);

                dtExpense = da.getExpenseDatabyLocation(denom, loc, val, tp, filVal, page);

                if (dtExpense.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtExpense.Rows)
                    {
                        Expense temp1 = new Expense();
                        temp1.statusDetails = new();

                        temp1.ExpenseID = dt["ExpenseID"].ToString();
                        temp1.AdvanceID = dt["AdvanceID"].ToString();
                        temp1.LocationID = dt["LocationID"].ToString();
                        temp1.Approver = dt["Approver"].ToString();
                        temp1.ExpenseDate = Convert.ToDateTime(dt["ExpenseDate"]);
                        temp1.DepartmentID = dt["DepartmentID"].ToString();
                        temp1.ExpenseNIK = dt["ExpenseNIK"].ToString();
                        temp1.ExpenseNote = dt["ExpenseNote"].ToString();
                        temp1.ExpenseType = dt["ExpenseType"].ToString();
                        temp1.TypeAccount = dt["TypeAccount"].ToString();
                        temp1.ExpenseStatus = dt["ExpenseStatus"].ToString();
                        temp1.Applicant = dt["Applicant"].ToString();

                        temp1.statusDetails.submitDate = dt.IsNull("SubmitDate") ? DateTime.MinValue : Convert.ToDateTime(dt["SubmitDate"]);
                        temp1.statusDetails.submitUser = dt.IsNull("SubmitUser") ? "" : dt["SubmitUser"].ToString();
                        temp1.statusDetails.confirmDate = dt.IsNull("ConfirmDate") ? DateTime.MinValue : Convert.ToDateTime(dt["ConfirmDate"]);
                        temp1.statusDetails.confirmUser = dt.IsNull("ConfirmUser") ? "" : dt["ConfirmUser"].ToString();
                        temp1.statusDetails.rejectDate = dt.IsNull("RejectDate") ? DateTime.MinValue : Convert.ToDateTime(dt["RejectDate"]);
                        temp1.statusDetails.rejectUser = dt.IsNull("RejectUser") ? "" : dt["RejectUser"].ToString();

                        List<ExpenseLine> expenseLines = new List<ExpenseLine>();
                        DataTable dtExpenseLine = new DataTable("ExpenseLine");

                        dtExpenseLine = da.getExpenseLinesbyID(dt["ExpenseID"].ToString(), denom);

                        if (dtExpenseLine.Rows.Count > 0)
                        {
                            foreach (DataRow linedt in dtExpenseLine.Rows)
                            {
                                ExpenseLine temp2 = new ExpenseLine();

                                temp2.ExpenseID = linedt["ExpenseID"].ToString();
                                temp2.LineNo = Convert.ToInt32(linedt["LineNum"]);
                                temp2.Details = linedt["Details"].ToString();
                                temp2.Amount = Convert.ToDecimal(linedt["Amount"]);
                                temp2.ActualAmount = Convert.ToDecimal(linedt["ActualAmount"]);
                                temp2.Status = linedt["EStatus"].ToString();

                                expenseLines.Add(temp2);
                            }
                        }

                        temp1.lines = new List<ExpenseLine>();
                        temp1.lines = expenseLines;

                        expense.Add(temp1);
                    }

                    res.Data = expense;
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

        [HttpGet("getReimburseDatabyLocation/{locPage}")]
        public async Task<IActionResult> getReimburseDataTablebyLocation(string locPage)
        {
            ResultModel<List<Reimburse>> res = new ResultModel<List<Reimburse>>();
            PettyCashDA da = new(_configuration);
            List<Reimburse> reimburse = new List<Reimburse>();
            DataTable dtReimburse = new DataTable("ReimburseData");
            IActionResult actionResult = null;

            try
            {
                var temp = CommonLibrary.Base64Decode(locPage);

                string denom = temp.Split("!_!")[0];
                string loc = temp.Split("!_!")[1].Equals("") ? "HO" : temp.Split("!_!")[1];
                string val = temp.Split("!_!")[2];
                string tp = temp.Split("!_!")[3];
                string filVal = temp.Split("!_!")[4];
                int page = Convert.ToInt32(temp.Split("!_!")[5]);

                dtReimburse = da.getReimburseDatabyLocation(denom, loc, val, tp, filVal, page);

                if (dtReimburse.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtReimburse.Rows)
                    {
                        Reimburse temp1 = new Reimburse();
                        temp1.statusDetails = new();

                        temp1.ReimburseID = dt["ReimburseID"].ToString();
                        temp1.LocationID = dt["LocationID"].ToString();
                        temp1.ReimburseNote = dt["ReimburseNote"].ToString();
                        temp1.ReimburseDate = Convert.ToDateTime(dt["ReimburseDate"]);
                        temp1.ReimburseStatus = dt["ReimburseStatus"].ToString();
                        temp1.Applicant = dt["Applicant"].ToString();

                        temp1.statusDetails.confirmDate = dt.IsNull("ConfirmDate") ? DateTime.MinValue : Convert.ToDateTime(dt["ConfirmDate"]);
                        temp1.statusDetails.confirmUser = dt.IsNull("ConfirmUser") ? "" : dt["ConfirmUser"].ToString();
                        temp1.statusDetails.verifyDate = dt.IsNull("VerifyDate") ? DateTime.MinValue : Convert.ToDateTime(dt["VerifyDate"]);
                        temp1.statusDetails.verifyUser = dt.IsNull("VerifyUser") ? "" : dt["VerifyUser"].ToString();
                        temp1.statusDetails.releaseDate = dt.IsNull("ReleaseDate") ? DateTime.MinValue : Convert.ToDateTime(dt["ReleaseDate"]);
                        temp1.statusDetails.releaseUser = dt.IsNull("ReleaseUser") ? "" : dt["ReleaseUser"].ToString();
                        temp1.statusDetails.approveDate = dt.IsNull("ApproveDate") ? DateTime.MinValue : Convert.ToDateTime(dt["ApproveDate"]);
                        temp1.statusDetails.approveUser = dt.IsNull("ApproveUser") ? "" : dt["ApproveUser"].ToString();
                        temp1.statusDetails.claimDate = dt.IsNull("ClaimDate") ? DateTime.MinValue : Convert.ToDateTime(dt["ClaimDate"]);
                        temp1.statusDetails.claimUser = dt.IsNull("ClaimUser") ? "" : dt["ClaimUser"].ToString();
                        temp1.statusDetails.resolveDate = dt.IsNull("ResolveDate") ? DateTime.MinValue : Convert.ToDateTime(dt["ResolveDate"]);
                        temp1.statusDetails.resolveUser = dt.IsNull("ResolveUser") ? "" : dt["ResolveUser"].ToString();
                        temp1.statusDetails.rejectDate = dt.IsNull("RejectDate") ? DateTime.MinValue : Convert.ToDateTime(dt["RejectDate"]);
                        temp1.statusDetails.rejectUser = dt.IsNull("RejectUser") ? "" : dt["RejectUser"].ToString();


                        List<ReimburseLine> reimburseLines = new List<ReimburseLine>();
                        DataTable dtReimburseLine = new DataTable("ReimburseLine");

                        dtReimburseLine = da.getReimburseLinesbyID(dt["ReimburseID"].ToString(), denom);

                        if (dtReimburseLine.Rows.Count > 0)
                        {
                            foreach (DataRow linedt in dtReimburseLine.Rows)
                            {
                                ReimburseLine temp2 = new ReimburseLine();

                                temp2.ReimburseID = linedt["ReimburseID"].ToString();
                                temp2.ExpenseID = linedt["ExpenseID"].ToString();
                                temp2.LineNo = Convert.ToInt32(linedt["LineNum"]);
                                temp2.AccountNo = linedt["AccountNo"].ToString();
                                temp2.Details = linedt["Details"].ToString();
                                temp2.Amount = Convert.ToDecimal(linedt["Amount"]);
                                temp2.ApprovedAmount = Convert.ToDecimal(linedt["ApprovedAmount"]);
                                temp2.Status = linedt["RStatus"].ToString();

                                reimburseLines.Add(temp2);
                            }
                        }

                        temp1.lines = new List<ReimburseLine>();
                        temp1.lines = reimburseLines;

                        reimburse.Add(temp1);
                    }

                    res.Data = reimburse;
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

        [HttpGet("getPettyCashOutstandingAmountandLocBalanceDetails/{loc}")]
        public async Task<IActionResult> getPettyCashOutstandingAmount(string loc)
        {
            ResultModel<LocationBalanceDetails> res = new ResultModel<LocationBalanceDetails>();
            PettyCashDA da = new(_configuration);
            DataTable dtReimburseOutstanding = new DataTable("ReimburseOutstanding");
            DataTable dtBalanceDetails = new DataTable("BalanceDetails");
            IActionResult actionResult = null;

            try
            {
                res.Data = new();
                res.Data.outstandingBalance = new();
                res.Data.balanceDetails = new();

                // outstanding
                res.Data.outstandingBalance.locationID = loc;
                res.Data.outstandingBalance.locationOnhandAmount = da.getLocationOnhandAmount(loc);
                res.Data.outstandingBalance.advanceOutstandingAmount = da.getAdvanceOutstandingAmount(loc);
                res.Data.outstandingBalance.expenseOutstandingAmount = da.getExpenseOutstandingAmount(loc);
                res.Data.outstandingBalance.advanceApprovedAmount = da.getAdvanceApprovedAmount(loc);
                res.Data.outstandingBalance.expenseApprovedAmount = da.getExpenseApprovedAmount(loc);
                res.Data.CutOffDate = da.getCutoffDate(loc);

                dtReimburseOutstanding = da.getReimburseOutstandingAmount(loc);

                if (dtReimburseOutstanding.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtReimburseOutstanding.Rows)
                    {
                        res.Data.outstandingBalance.reimbursementReqOutstandingAmount = Convert.ToDecimal(dt["amtRequest"]);
                        res.Data.outstandingBalance.reimbursementApvOutstandingAmount = Convert.ToDecimal(dt["amtApv"]);
                        res.Data.outstandingBalance.reimbursementApvRejectedAmount = Convert.ToDecimal(dt["amtRejected"]);
                    }
                }

                res.Data.outstandingBalance.lastFetch = DateTime.Now;

                // location balance details
                res.Data.balanceDetails.LocationID = loc;

                dtBalanceDetails = da.getLocationBudgetDetails(loc);

                if (dtBalanceDetails.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtBalanceDetails.Rows)
                    {
                        res.Data.balanceDetails.BudgetAmount = Convert.ToDecimal(dt["Budget"]);
                        res.Data.balanceDetails.LatestAuditUser = dt["AuditUser"].ToString();
                        res.Data.balanceDetails.AuditDate = Convert.ToDateTime(dt["AuditActionDate"]);
                    }
                }

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

        [HttpPost("updateLocationBudget")]
        public async Task<IActionResult> updateLocationBudgetData(QueryModel<BalanceDetails> data)
        {
            ResultModel<QueryModel<BalanceDetails>> res = new ResultModel<QueryModel<BalanceDetails>>();
            PettyCashDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                da.createPettyCashTableSchema(data.Data.LocationID);
                da.updateLocationBudget(data);

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

        [HttpPost("updateLocationCutoffDate")]
        public async Task<IActionResult> updateLocationCutoffDateData(QueryModel<CutoffDetails> data)
        {
            ResultModel<QueryModel<CutoffDetails>> res = new ResultModel<QueryModel<CutoffDetails>>();
            PettyCashDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                da.updateLocationCutoffDate(data);

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

        [HttpGet("getModulePageSize/{Table}")]
        public async Task<IActionResult> getModulePageSize(string Table)
        {
            ResultModel<int> res = new ResultModel<int>();
            PettyCashDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                string tbname = CommonLibrary.Base64Decode(Table).Split("!_!")[0];
                string tbcol = CommonLibrary.Base64Decode(Table).Split("!_!")[1];
                string value = CommonLibrary.Base64Decode(Table).Split("!_!")[2];
                string tp = CommonLibrary.Base64Decode(Table).Split("!_!")[3];
                string filVal = CommonLibrary.Base64Decode(Table).Split("!_!")[4];
                string loc = CommonLibrary.Base64Decode(Table).Split("!_!")[5];

                res.Data = da.getModuleNumberOfPage(tbname, tbcol, value, tp, filVal, loc);

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

        [HttpGet("getCoabyModule/{moduleName}")]
        public async Task<IActionResult> getCoabyModuleData(string moduleName)
        {
            ResultModel<List<Account>> res = new ResultModel<List<Account>>();
            PettyCashDA da = new(_configuration);
            List<Account> accountLines = new List<Account>();
            DataTable dtAccount = new DataTable("Account");
            IActionResult actionResult = null;

            try
            {
                dtAccount = da.getCoabyModule(moduleName);

                if (dtAccount.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtAccount.Rows)
                    {
                        Account temp = new Account();

                        temp.AccountCode = dt["AccountNo"].ToString();
                        temp.AccountDescription = dt["AccDescription"].ToString();

                        accountLines.Add(temp);
                    }

                    res.Data = accountLines;
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

        [HttpGet("getMailingDetails/{param}")]
        public async Task<IActionResult> getMailingDetailsData(string param)
        {
            ResultModel<Mailing> res = new ResultModel<Mailing>();
            PettyCashDA da = new(_configuration);
            DataTable dtMailingList = new DataTable("Mailing");
            IActionResult actionResult = null;

            try
            {
                string temp = CommonLibrary.Base64Decode(param);

                string mod = temp.Split("!_!")[0];
                string actName = temp.Split("!_!")[1];
                string loc = temp.Split("!_!")[2].Equals("") ? "HO" : temp.Split("!_!")[2];

                dtMailingList = da.getMailingDetails(mod, actName, loc);

                if (dtMailingList.Rows.Count > 0)
                {
                    res.Data = new();

                    foreach (DataRow dt in dtMailingList.Rows)
                    {
                        res.Data.ModuleName = dt["ModuleName"].ToString();
                        res.Data.ActionName = dt["ActionName"].ToString();
                        res.Data.Receiver = dt.IsNull("Receiver") ? "" : dt["Receiver"].ToString();
                        res.Data.LocationID = dt.IsNull("LocationID") ? "" : dt["LocationID"].ToString();
                        res.Data.MailSubject = dt["MailSubject"].ToString();
                        res.Data.MailBeginningBody = dt.IsNull("BeginningBody") ? "" : dt["BeginningBody"].ToString();
                        res.Data.MailMainBody = dt.IsNull("MainBody") ? "" : dt["MainBody"].ToString();
                        res.Data.MailFooter = dt.IsNull("Footer") ? "" : dt["Footer"].ToString();
                        res.Data.MailNote = dt.IsNull("MailNote") ? "" : dt["MailNote"].ToString();
                    }

                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.isSuccess = true;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Empty Data";

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

        //
    }
}
