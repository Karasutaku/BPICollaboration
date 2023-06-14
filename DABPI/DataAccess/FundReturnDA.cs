using BPIDA.Models.DbModel;
using BPIDA.Models.MainModel.FundReturn;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace BPIDA.DataAccess
{
    public class FundReturnDA
    {
        private readonly IConfiguration _configuration;
        private readonly string _conString, _moduleConnection;

        public FundReturnDA(IConfiguration config)
        {
            _configuration = config;
            _moduleConnection = _configuration.GetValue<string>("ModuleConnection:FundReturn");
            _conString = _configuration.GetValue<string>($"ConnectionStrings:{_moduleConnection}");
        }

        internal bool createFundReturnDocumentData(QueryModel<FundReturnHeader> data, DataTable dataItemLines)
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
                    command.CommandText = "[createFundReturnSchemaTables]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);

                    command.ExecuteNonQuery();

                    command.CommandText = "[createFundReturnDocument]";

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@DocumentID", data.Data.DocumentID);
                    command.Parameters.AddWithValue("@RequestDate", data.Data.RequestDate);
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@CommercialType", data.Data.CommercialType);
                    command.Parameters.AddWithValue("@CustomerName", data.Data.CustomerName);
                    command.Parameters.AddWithValue("@CustomerType", data.Data.CustomerType);
                    command.Parameters.AddWithValue("@CustomerMemberID", data.Data.CustomerMemberID);
                    command.Parameters.AddWithValue("@CustomerContactNo", data.Data.CustomerContactNo);
                    command.Parameters.AddWithValue("@FundReturnCategoryID", data.Data.FundReturnCategoryID);
                    command.Parameters.AddWithValue("@BankHolderName", data.Data.BankHolderName);
                    command.Parameters.AddWithValue("@BankAccount", data.Data.BankAccount);
                    command.Parameters.AddWithValue("@BankID", data.Data.BankID);
                    command.Parameters.AddWithValue("@ReceiptDocument", data.Data.ReceiptDocument);
                    command.Parameters.AddWithValue("@ExternalDocument", data.Data.ExternalDocument);
                    command.Parameters.AddWithValue("@RefundAmount", data.Data.RefundAmount);
                    command.Parameters.AddWithValue("@TransactionAmount", data.Data.TransactionAmount);
                    command.Parameters.AddWithValue("@Reason", data.Data.Reason);
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

        internal bool createFundReturnHeaderData(QueryModel<FundReturnHeader> data)
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
                    command.CommandText = "[createFundReturnSchemaTables]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);

                    command.ExecuteNonQuery();

                    command.CommandText = "[createFundReturnHeaderData]";

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@DocumentID", data.Data.DocumentID);
                    command.Parameters.AddWithValue("@RequestDate", data.Data.RequestDate);
                    command.Parameters.AddWithValue("@LocationID", data.Data.LocationID);
                    command.Parameters.AddWithValue("@CommercialType", data.Data.CommercialType);
                    command.Parameters.AddWithValue("@CustomerName", data.Data.CustomerName);
                    command.Parameters.AddWithValue("@CustomerType", data.Data.CustomerType);
                    command.Parameters.AddWithValue("@CustomerMemberID", data.Data.CustomerMemberID);
                    command.Parameters.AddWithValue("@CustomerContactNo", data.Data.CustomerContactNo);
                    command.Parameters.AddWithValue("@FundReturnCategoryID", data.Data.FundReturnCategoryID);
                    command.Parameters.AddWithValue("@BankHolderName", data.Data.BankHolderName);
                    command.Parameters.AddWithValue("@BankAccount", data.Data.BankAccount);
                    command.Parameters.AddWithValue("@BankID", data.Data.BankID);
                    command.Parameters.AddWithValue("@ReceiptDocument", data.Data.ReceiptDocument);
                    command.Parameters.AddWithValue("@ExternalDocument", data.Data.ExternalDocument);
                    command.Parameters.AddWithValue("@RefundAmount", data.Data.RefundAmount);
                    command.Parameters.AddWithValue("@TransactionAmount", data.Data.TransactionAmount);
                    command.Parameters.AddWithValue("@Reason", data.Data.Reason);
                    command.Parameters.AddWithValue("@DocumentStatus", data.Data.DocumentStatus);
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

        internal bool createFundReturnApprovalData(QueryModel<FundReturnApproval> data, string location)
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
                    command.CommandText = "[createFundReturnApprovalData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", location);
                    command.Parameters.AddWithValue("@DocumentID", data.Data.DocumentID);
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

        internal bool deleteFundReturnDocument(string id, string location, string user, string act, DateTime date)
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
                    command.CommandText = "[deleteFundReturnDocument]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@DocumentID", id);
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

        internal DataTable getFundReturnHeaderbyFilterData(string location, string conditions, int pageNo, int rowPerPage)
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
                    command.CommandText = "[getFundReturnHeaderbyFilter]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", location);
                    command.Parameters.AddWithValue("@Conditions", conditions);
                    command.Parameters.AddWithValue("@PageNo", pageNo);
                    command.Parameters.AddWithValue("@RowPerPage", rowPerPage);

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

        internal DataTable getFundReturnItemLinesData(string location, DataTable param)
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
                    command.CommandText = "[getFundReturnItemLines]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", location);
                    command.Parameters.AddWithValue("@FundReturnIds", param);

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

        internal DataTable getFundReturnApprovalsData(string location, DataTable param)
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
                    command.CommandText = "[getFundReturnApprovals]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", location);
                    command.Parameters.AddWithValue("@FundReturnIds", param);

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

        internal DataTable getFundReturnBankData()
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
                    command.CommandText = "[getFundReturnBankData]";
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

        internal DataTable getFundReturnCategoryData()
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
                    command.CommandText = "[getFundReturnCategory]";
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

        internal int getFundReturnModuleNumberOfPageData(string TbName, string loc, string conditions, int rowPerPage)
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
                    command.CommandText = "[getFundReturnModulePageSize]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@TbName", TbName);
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@Conditions", conditions);
                    command.Parameters.AddWithValue("@RowPerPage", rowPerPage);

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
