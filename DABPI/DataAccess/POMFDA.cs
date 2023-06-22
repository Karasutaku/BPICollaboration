using BPIDA.Models.DbModel;
using BPIDA.Models.MainModel.POMF;
using BPILibrary;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BPIDA.DataAccess
{
    public class POMFDA
    {
        private readonly IConfiguration _configuration;
        private readonly string _conString, _moduleConnection;
        private readonly int _rowPerPage;

        public POMFDA(IConfiguration config)
        {
            _configuration = config;
            _moduleConnection = _configuration.GetValue<string>("ModuleConnection:POMF");
            _conString = _configuration.GetValue<string>($"ConnectionStrings:{_moduleConnection}");
            _rowPerPage = _configuration.GetValue<int>("Paging:POMF:RowPerPage");
        }

        internal bool createPOMFDocumentData(QueryModel<POMFHeader> data, DataTable dataItemLines)
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
                    command.CommandText = "[createPOMFSchemaTables]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);

                    command.ExecuteNonQuery();

                    command.CommandText = "[createPOMFDocument]";

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@POMFID", data.Data.POMFID);
                    command.Parameters.AddWithValue("@POMFDate", data.Data.POMFDate);
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@CustomerName", data.Data.CustomerName);
                    command.Parameters.AddWithValue("@ReceiptNo", data.Data.ReceiptNo);
                    command.Parameters.AddWithValue("@NPNo", data.Data.NPNo);
                    command.Parameters.AddWithValue("@NPTypeID", data.Data.NPTypeID);
                    command.Parameters.AddWithValue("@Requester", data.Data.Requester);
                    command.Parameters.AddWithValue("@DocumentStatus", data.Data.DocumentStatus);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    command.Parameters.AddWithValue("@ItemLines", dataItemLines);

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

        internal bool createPOMFApprovalData(QueryModel<POMFApproval> data, string location)
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
                    command.CommandText = "[createPOMFApprovalData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", location);
                    command.Parameters.AddWithValue("@POMFID", data.Data.POMFID);
                    command.Parameters.AddWithValue("@ApprovalAction", data.Data.ApprovalAction);
                    command.Parameters.AddWithValue("@Approver", data.Data.Approver);
                    command.Parameters.AddWithValue("@ApproveDate", data.Data.ApproveDate);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

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

        internal bool deletePOMFDocument(string id, string location, string user, string act, DateTime date)
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
                    command.CommandText = "[deletePOMFDocument]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@POMFID", id);
                    command.Parameters.AddWithValue("@LocationID", location);
                    command.Parameters.AddWithValue("@AuditUser", user);
                    command.Parameters.AddWithValue("@AuditAction", act);
                    command.Parameters.AddWithValue("@AuditActionDate", date);

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

        internal bool editPOMFApprovalExtendedData(string loc, DataTable itemLines, POMFApproval approval, string audituser, string auditaction, DateTime auditdate)
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
                    command.CommandText = "[editPOMFDocumentExtendedData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@POMFID", approval.POMFID);
                    command.Parameters.AddWithValue("@ItemLines", itemLines);
                    command.Parameters.AddWithValue("@ApprovalAction", approval.ApprovalAction);
                    command.Parameters.AddWithValue("@Approver", approval.Approver);
                    command.Parameters.AddWithValue("@ApproveDate", approval.ApproveDate);
                    command.Parameters.AddWithValue("@AuditUser", audituser);
                    command.Parameters.AddWithValue("@AuditAction", auditaction);
                    command.Parameters.AddWithValue("@AuditActionDate", auditdate);

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

        internal DataTable getPOMFHeaderbyFilterData(string location, string conditions, int pageNo)
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
                    command.CommandText = "[getPOMFHeaderbyFilter]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", location);
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

        internal DataTable getPOMFItemLinesData(string location, DataTable param)
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
                    command.CommandText = "[getPOMFItemLines]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", location);
                    command.Parameters.AddWithValue("@POMFDocs", param);

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

        internal DataTable getPOMFApprovalsData(string location, DataTable param)
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
                    command.CommandText = "[getPOMFApprovals]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", location);
                    command.Parameters.AddWithValue("@POMFDocs", param);

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

        internal DataTable getPOMFNPTypeData()
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
                    command.CommandText = "[getPOMFNPTypeData]";
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

        internal int getPOMFModuleNumberOfPageData(string TbName, string loc, string conditions)
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
                    command.CommandText = "[getPOMFModulePageSize]";
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

        //
    }
}
