using BPIDA.Models.DbModel;
using BPIDA.Models.MainModel.CashierLogbook;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BPIDA.DataAccess
{
    public class CashierLogbookDA
    {
        private readonly IConfiguration _configuration;
        private readonly string _conString;
        private readonly int _rowPerPage;

        public CashierLogbookDA(IConfiguration config)
        {
            _configuration = config;
            _conString = _configuration.GetValue<string>("ConnectionStrings:Bpi");
            _rowPerPage = _configuration.GetValue<int>("Paging:CashierLogbook:RowPerPage");
        }

        internal DataTable createBrankasActionLogData(QueryModel<CashierLogAction> data)
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
                    command.CommandText = "[createBrankasActionLogData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LogID", data.Data.LogID);
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@UserEmail", data.Data.UserEmail);
                    command.Parameters.AddWithValue("@LogAction", data.Data.LogAction);
                    command.Parameters.AddWithValue("@ActionDate", data.Data.ActionDate);
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

            return dt;
        }

        internal bool createCashierLogbookTableSchema(string objName)
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
                    command.CommandText = "[createBrankasSchemaTables]";
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

        internal DataTable createIDData(string docType)
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
                    command.CommandText = "[createID]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@DocumentName", docType);

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

        internal DataTable getShiftbyModule(string moduleName)
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
                    command.CommandText = "[getShiftbyModule]";
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

        internal DataTable getAmountCategories()
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
                    command.CommandText = "[getBrankasAmountCategoriesData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();

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

        internal DataTable getAmountSubCategories()
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
                    command.CommandText = "[getBrankasAmountSubCategoriesData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();

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

        internal DataTable getAmountType()
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
                    command.CommandText = "[getBrankasAmountTypeData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();

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

        internal void createLogData(QueryModel<CashierLogDataConv> data, string type)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createBrankasLogbookData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LogType", type);
                    command.Parameters.AddWithValue("@LogID", data.Data.LogID);
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@Applicant", data.Data.Applicant);
                    command.Parameters.AddWithValue("@LogDate", data.Data.LogDate);
                    command.Parameters.AddWithValue("@LogStatus", data.Data.LogStatus);
                    command.Parameters.AddWithValue("@LogStatusDate", data.Data.LogStatusDate);
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

        internal void createLogHeaderData(DataTable data, string loc)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createBrankasCategoryDetails]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@BrankasCategoryDetailData", data);

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

        internal void createLogLineData(DataTable data, string loc, string logId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createBrankasCategoryLine]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@LogID", logId);
                    command.Parameters.AddWithValue("@BrankasCategoryLineData", data);

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

        internal void createBrankasApproveLogData(DataTable data, string loc)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createBrankasApproveLogData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@BrankasApproveLogData", data);

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

        internal void editBrankasApproveLogOnConfirmData(QueryModel<CashierLogApproval> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editBrankasApproveLogOnConfirmData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LogID", data.Data.LogID);
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@ShiftID", data.Data.ShiftID);
                    command.Parameters.AddWithValue("@CreateUser", data.Data.CreateUser);
                    command.Parameters.AddWithValue("@CreateDate", data.Data.CreateDate);
                    command.Parameters.AddWithValue("@ConfirmUser", data.Data.ConfirmUser);
                    command.Parameters.AddWithValue("@ConfirmDate", data.Data.ConfirmDate);
                    command.Parameters.AddWithValue("@ApproveNote", data.Data.ApproveNote);
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

        internal void deleteBrankasDetailsandLinesByLogIDData(string logId, string loc)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[deleteBrankasDetailsandLinesByLogID]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LogID", logId);
                    command.Parameters.AddWithValue("@LocationID", loc);

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

        internal void updateApproveLogData(DataTable data, string loc, string logId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createBrankasCategoryLine]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@LogID", logId);
                    command.Parameters.AddWithValue("@BrankasCategoryLineData", data);

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

        internal void updateBrankasDocumentStatusData(string logId, string loc, string statVal, string user, string act, DateTime actDate)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editBrankasDocumentStatus]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LogID", logId);
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@StatusValue", statVal);
                    command.Parameters.AddWithValue("@AuditUser", user);
                    command.Parameters.AddWithValue("@AuditAction", act);
                    command.Parameters.AddWithValue("@AuditActionDate", actDate);

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

        internal DataTable getLogData(string logType, string loc, string statValue, string conditions, int pageNo)
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
                    command.CommandText = "[getBrankasLogData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LogType", logType);
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@StatusValue", statValue);
                    command.Parameters.AddWithValue("@Conditions", conditions);
                    command.Parameters.AddWithValue("@PageNo", pageNo);
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

        internal DataTable getLogHeaderData(string logID, string loc)
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
                    command.CommandText = "[getBrankasLogHeaderDetailbyLogID]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LogID", logID);
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

        internal DataTable getLogLineData(string logCatID, string loc)
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
                    command.CommandText = "[getBrankasHeaderLineDetailbyCategoryID]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@BrankasCategoryID", logCatID);
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

        internal DataTable getLogApprovalData(string logID, string loc)
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
                    command.CommandText = "[getBrankasApproveDatabyLogID]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LogID", logID);
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

        internal DataTable getBrankasActionLogData(string loc, string orderBy, string filType, string filVal, int pageNo)
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
                    command.CommandText = "[getBrankasActionLogData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@OrderBy", orderBy);
                    command.Parameters.AddWithValue("@FilterType", filType);
                    command.Parameters.AddWithValue("@FilterValue", filVal);
                    command.Parameters.AddWithValue("@PageNo", pageNo);
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

        internal int getModuleNumberOfPage(string TbName, string loc, string conditions)
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
                    command.CommandText = "[getBrankasModulePageSize]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@TbName", TbName);
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@Conditions", conditions);
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

        internal int getNumberofLogExisting(string loc, string conditions)
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
                    command.CommandText = "[getNumberofLogExist]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@Conditions", conditions);

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

        //
    }
}
