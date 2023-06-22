using BPIDA.Models.DbModel;
using BPIDA.Models.MainModel.PettyCash;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BPIDA.DataAccess
{
    public class PettyCashDA
    {
        private readonly IConfiguration _configuration;
        private readonly string _conString, _moduleConnection;
        private readonly int _rowPerPage;

        public PettyCashDA(IConfiguration config)
        {
            _configuration = config;
            _moduleConnection = _configuration.GetValue<string>("ModuleConnection:PettyCash");
            _conString = _configuration.GetValue<string>($"ConnectionStrings:{_moduleConnection}");
            _rowPerPage = _configuration.GetValue<int>("Paging:PettyCash:RowPerPage");
        }

        internal bool isAdvanceDataPresent(string AdvanceId)
        {
            var conBool = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[isAdvancePresent]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@AdvanceID", AdvanceId);
                    var data = command.ExecuteScalar();
                    conBool = Convert.ToBoolean(data);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return conBool;
        }

        // create

        internal bool createPettyCashTableSchema(string objName)
        {
            bool conBool = false;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createPettyCashSchemaTables]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@objectName", objName);

                    int ret = command.ExecuteNonQuery();

                    if (ret > 0)
                        conBool = true;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return conBool;
        }

        internal bool createPettycashAdvanceDocument(QueryModel<Advance> data, DataTable lines)
        {
            bool flag = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createPettycashAdvanceDocument]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@AdvanceID", data.Data.AdvanceID);
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@Approver", data.Data.Approver);
                    command.Parameters.AddWithValue("@AdvanceDate", data.Data.AdvanceDate);
                    command.Parameters.AddWithValue("@DepartmentID", data.Data.DepartmentID);
                    command.Parameters.AddWithValue("@AdvanceNIK", data.Data.AdvanceNIK);
                    command.Parameters.AddWithValue("@AdvanceNote", data.Data.AdvanceNote);
                    command.Parameters.AddWithValue("@AdvanceType", data.Data.AdvanceType);
                    command.Parameters.AddWithValue("@TypeAccount", data.Data.TypeAccount);
                    command.Parameters.AddWithValue("@AdvanceStatus", data.Data.AdvanceStatus);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    command.Parameters.AddWithValue("@AdvanceLinesData", lines);

                    int ret = command.ExecuteNonQuery();

                    if (ret > 0)
                        flag = true;

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }

            return flag;
        }

        internal bool createPettycashExpenseDocument(QueryModel<Expense> data, DataTable lines, DataTable attach)
        {
            bool flag = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createPettycashExpenseDocument]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ExpenseID", data.Data.ExpenseID);
                    command.Parameters.AddWithValue("@AdvanceID", data.Data.AdvanceID);
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@Approver", data.Data.Approver);
                    command.Parameters.AddWithValue("@ExpenseDate", data.Data.ExpenseDate);
                    command.Parameters.AddWithValue("@DepartmentID", data.Data.DepartmentID);
                    command.Parameters.AddWithValue("@ExpenseNIK", data.Data.ExpenseNIK);
                    command.Parameters.AddWithValue("@ExpenseNote", data.Data.ExpenseNote);
                    command.Parameters.AddWithValue("@ExpenseType", data.Data.ExpenseType);
                    command.Parameters.AddWithValue("@TypeAccount", data.Data.TypeAccount);
                    command.Parameters.AddWithValue("@ExpenseStatus", data.Data.ExpenseStatus);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    command.Parameters.AddWithValue("@ExpenseLinesData", lines);
                    command.Parameters.AddWithValue("@ExpenseAttachData", attach);

                    int ret = command.ExecuteNonQuery();

                    if (ret > 0)
                        flag = true;

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }

            return flag;
        }

        internal void createExpenseAttachLine(DataTable data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createExpenseAttachData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ExpenseAttachData", data);

                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        internal void createReimburseData(QueryModel<Reimburse> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createReimburseData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ReimburseID", data.Data.ReimburseID);
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@ReimburseNote", data.Data.ReimburseNote);
                    command.Parameters.AddWithValue("@ReimburseDate", data.Data.ReimburseDate);
                    command.Parameters.AddWithValue("@ReimburseStatus", data.Data.ReimburseStatus);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        internal void createReimburseLine(DataTable data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createReimburseLines]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ReimburseLinesData", data);

                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        internal bool createPettycashReimburseDocument(QueryModel<Reimburse> data, DataTable expId, DataTable lines, DataTable attach)
        {
            bool flag = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createPettycashReimburseDocument]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ReimburseID", data.Data.ReimburseID);
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@ReimburseNote", data.Data.ReimburseNote);
                    command.Parameters.AddWithValue("@ReimburseDate", data.Data.ReimburseDate);
                    command.Parameters.AddWithValue("@ReimburseStatus", data.Data.ReimburseStatus);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    command.Parameters.AddWithValue("@Expenses", expId);
                    command.Parameters.AddWithValue("@ReimburseLinesData", lines);
                    command.Parameters.AddWithValue("@ReimburseAttachData", attach);

                    int ret = command.ExecuteNonQuery();

                    if (ret > 0)
                        flag = true;

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }

            return flag;
        }

        internal void createReimburseAttachLine(DataTable data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createReimburseAttachData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ReimburseAttachData", data);

                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        // update

        internal void updateSettleAdvance(QueryModel<string> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editAdvanceSettlement]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@AdvanceID", data.Data);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    command.ExecuteNonQuery();

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        internal bool updateSettleExpense(string Id, string user, string act, DateTime actdate)
        {
            bool flag = false;
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editExpenseSettlement]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ExpenseID", Id);
                    command.Parameters.AddWithValue("@AuditUser", user);
                    command.Parameters.AddWithValue("@AuditAction", act);
                    command.Parameters.AddWithValue("@AuditActionDate", actdate);

                    int ret = command.ExecuteNonQuery();

                    if (ret <= 0)
                        flag = true;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }

            return flag;
        }

        internal void updateSettleReimburse(QueryModel<string> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editReimburseSettlement]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ReimburseID", data.Data);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    command.ExecuteNonQuery();

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        internal bool updateDocumentStatus(string TbName, string Id, string statusValue, string reimburseNote, string user, string act, DateTime actdate)
        {
            bool conBool = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editPettyCashDocumentStatus]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@TbName", TbName);
                    command.Parameters.AddWithValue("@DocID", Id);
                    command.Parameters.AddWithValue("@StatusValue", statusValue);
                    command.Parameters.AddWithValue("@ReimburseNote", reimburseNote);
                    command.Parameters.AddWithValue("@AuditUser", user);
                    command.Parameters.AddWithValue("@AuditAction", act);
                    command.Parameters.AddWithValue("@AuditActionDate", actdate);

                    int ret = command.ExecuteNonQuery();

                    if (ret >= 0)
                        conBool = true;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }

            return conBool;
        }

        internal void updateReimburseLine(QueryModel<ReimburseLine> line)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editReimburseLine]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ReimburseID", line.Data.ReimburseID);
                    command.Parameters.AddWithValue("@ExpenseID", line.Data.ExpenseID);
                    command.Parameters.AddWithValue("@LineNum", line.Data.LineNo);
                    command.Parameters.AddWithValue("@AccountNo", line.Data.AccountNo);
                    command.Parameters.AddWithValue("@Details", line.Data.Details);
                    command.Parameters.AddWithValue("@Amount", line.Data.Amount);
                    command.Parameters.AddWithValue("@ApprovedAmount", line.Data.ApprovedAmount);
                    command.Parameters.AddWithValue("@RStatus", line.Data.Status);
                    command.Parameters.AddWithValue("@AuditUser", line.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", line.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", line.userActionDate);

                    command.ExecuteNonQuery();

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        internal void updateLocationBudget(QueryModel<BalanceDetails> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editLocationBudget]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@Budget", data.Data.BudgetAmount);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    command.ExecuteNonQuery();

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        internal void updateLocationCutoffDate(QueryModel<CutoffDetails> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editTableCutoffDate]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@ModuleLedgerName", data.Data.ModuleLedgerName);
                    command.Parameters.AddWithValue("@CutoffDate", data.Data.CutoffDate);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    command.ExecuteNonQuery();

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
        }

        // get

        internal DataTable getPettyCashLedgerDatabyDate(ledgerParam data)
        {
            DataTable dt = new DataTable("Data");

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getPettyCashLedgerDataEntry]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@StartDate", data.startDate);
                    command.Parameters.AddWithValue("@EndDate", data.endDate);
                    command.Parameters.AddWithValue("@LocationID", data.locationID);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return dt;
        }

        internal DataTable getAttachmentLines(string Id, string denom)
        {
            DataTable dt = new DataTable("Data");

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getAttachmentLines]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ExpenseID", Id);
                    command.Parameters.AddWithValue("@TbDenom", denom);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return dt;
        }

        internal DataTable getLocationBudgetDetails(string loc)
        {
            DataTable dt = new DataTable("Data");

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getLocationBudgetDetails]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return dt;
        }

        internal DataTable getAdvanceDatabyLocation(string denom, string location, string value, string type, string filValue, int PageNo)
        {
            DataTable dt = new DataTable("Data");

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getAdvanceDatabyLocation]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@TbDenom", denom);
                    command.Parameters.AddWithValue("@LocationID", location);
                    command.Parameters.AddWithValue("@StatusValue", value);
                    command.Parameters.AddWithValue("@FilterType", type);
                    command.Parameters.AddWithValue("@FilterValue", filValue);
                    command.Parameters.AddWithValue("@PageNo", PageNo);
                    command.Parameters.AddWithValue("@RowPerPage", _rowPerPage);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return dt;
        }

        internal DataTable getAdvanceDatabyUser(string userName)
        {
            DataTable dt = new DataTable("Data");

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getAdvanceDatabyUser]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@AuditUser", userName);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return dt;
        }

        internal DataTable getExpenseDatabyLocation(string denom, string location, string value, string type, string filValue, int PageNo)
        {
            DataTable dt = new DataTable("Data");

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getExpenseDatabyLocation]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@TbDenom", denom);
                    command.Parameters.AddWithValue("@LocationID", location);
                    command.Parameters.AddWithValue("@StatusValue", value);
                    command.Parameters.AddWithValue("@FilterType", type);
                    command.Parameters.AddWithValue("@FilterValue", filValue);
                    command.Parameters.AddWithValue("@PageNo", PageNo);
                    command.Parameters.AddWithValue("@RowPerPage", _rowPerPage);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return dt;
        }

        internal DataTable getReimburseDatabyLocation(string denom, string location, string value, string type, string filValue, int PageNo)
        {
            DataTable dt = new DataTable("Data");

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getReimburseDatabyLocation]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@TbDenom", denom);
                    command.Parameters.AddWithValue("@LocationID", location);
                    command.Parameters.AddWithValue("@StatusValue", value);
                    command.Parameters.AddWithValue("@FilterType", type);
                    command.Parameters.AddWithValue("@FilterValue", filValue);
                    command.Parameters.AddWithValue("@PageNo", PageNo);
                    command.Parameters.AddWithValue("@RowPerPage", _rowPerPage);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return dt;
        }

        internal DataTable getAdvanceLinesbyID(string AdvanceId, string denom)
        {
            DataTable dt = new DataTable("Data");

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getAdvanceLinesbyID]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@AdvanceID", AdvanceId);
                    command.Parameters.AddWithValue("@TbDenom", denom);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return dt;
        }

        internal DataTable getExpenseLinesbyID(string ExpenseId, string denom)
        {
            DataTable dt = new DataTable("Data");

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getExpenseLinesbyID]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ExpenseID", ExpenseId);
                    command.Parameters.AddWithValue("@TbDenom", denom);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return dt;
        }

        internal DataTable getReimburseLinesbyID(string ReimburseId, string denom)
        {
            DataTable dt = new DataTable("Data");

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getReimburseLinesbyID]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ReimburseID", ReimburseId);
                    command.Parameters.AddWithValue("@TbDenom", denom);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return dt;
        }

        internal int getModuleNumberOfPage(string TbName, string TbCol, string value, string type, string filValue, string loc)
        {
            int conInt = 0;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getPettyCashModulePageSize]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@TbName", TbName);
                    command.Parameters.AddWithValue("@TbCol", TbCol);
                    command.Parameters.AddWithValue("@StatusValue", value);
                    command.Parameters.AddWithValue("@FilterType", type);
                    command.Parameters.AddWithValue("@FilterValue", filValue);
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@RowPerPage", _rowPerPage);

                    var data = command.ExecuteScalar();
                    conInt = Convert.ToInt32(data);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return conInt;
        }

        internal decimal getAdvanceOutstandingAmount(string loc)
        {
            decimal conInt = decimal.Zero;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getAdvanceOutstandingAmount]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);

                    var data = command.ExecuteScalar();
                    conInt = Convert.ToDecimal(data);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return conInt;
        }

        internal decimal getAdvanceApprovedAmount(string loc)
        {
            decimal conInt = decimal.Zero;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getAdvanceApprovedAmount]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);

                    var data = command.ExecuteScalar();
                    conInt = Convert.ToDecimal(data);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return conInt;
        }

        internal decimal getExpenseOutstandingAmount(string loc)
        {
            decimal conInt = decimal.Zero;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getExpenseOutstandingAmount]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);

                    var data = command.ExecuteScalar();
                    conInt = Convert.ToDecimal(data);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return conInt;
        }

        internal decimal getExpenseApprovedAmount(string loc)
        {
            decimal conInt = decimal.Zero;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getExpenseApprovedAmount]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);

                    var data = command.ExecuteScalar();
                    conInt = Convert.ToDecimal(data);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return conInt;
        }

        internal DataTable getReimburseOutstandingAmount(string loc)
        {
            DataTable dt = new DataTable("Data");

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getReimburseOutstandingAmount]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }

            return dt;
        }

        internal decimal getLocationOnhandAmount(string loc)
        {
            decimal conInt = decimal.Zero;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getLocationOnhandAmount]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);

                    var data = command.ExecuteScalar();
                    conInt = Convert.ToDecimal(data);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }

            return conInt;
        }

        internal DateTime getCutoffDate(string loc)
        {
            DateTime temp;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getTableCutoffDate]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);
                    var data = command.ExecuteScalar();
                    temp = Convert.ToDateTime(data);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return temp;
        }

        internal DataTable getCoabyModule(string moduleName)
        {
            DataTable dt = new DataTable("Data");

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getCOAbyModule]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ModuleName", moduleName);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return dt;
        }

        internal DataTable getMailingDetails(string moduleName, string ActionName, string loc)
        {
            DataTable dt = new DataTable("Data");

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[getMailingDetails]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ModuleName", moduleName);
                    command.Parameters.AddWithValue("@ActionName", ActionName);
                    command.Parameters.AddWithValue("@LocationID", loc);

                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = command;
                    da.Fill(dt);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }
            return dt;
        }

        //
    }
}
