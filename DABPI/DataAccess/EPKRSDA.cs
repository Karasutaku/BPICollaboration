using BPIDA.Models.DbModel;
using BPIDA.Models.MainModel.EPKRS;
using BPILibrary;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BPIDA.DataAccess
{
    public class EPKRSDA
    {
        private readonly IConfiguration _configuration;
        private readonly string _conString, _moduleConnection;
        private readonly int _rowPerPage, _statRowPerPage;

        public EPKRSDA(IConfiguration config)
        {
            _configuration = config;
            _moduleConnection = _configuration.GetValue<string>("ModuleConnection:EPKRS");
            _conString = _configuration.GetValue<string>($"ConnectionStrings:{_moduleConnection}");
            _rowPerPage = _configuration.GetValue<int>("Paging:EPKRS:RowPerPage");
            _statRowPerPage = _configuration.GetValue<int>("Paging:EPKRS:StatRowPerPage");
        }

        internal bool createEPKRSItemCaseDocument(QueryModel<ItemCase> data, DataTable dataItemLines, DataTable dataAttachments)
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
                    command.CommandText = "[createEPKRSItemCaseDocument]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@SubRiskID", data.Data.SubRiskID);
                    command.Parameters.AddWithValue("@DocumentID", data.Data.DocumentID);
                    command.Parameters.AddWithValue("@SiteReporter", data.Data.SiteReporter);
                    command.Parameters.AddWithValue("@SiteSender", data.Data.SiteSender);
                    command.Parameters.AddWithValue("@ReportDate", data.Data.ReportDate);
                    command.Parameters.AddWithValue("@ItemPickupDate", data.Data.ItemPickupDate);
                    command.Parameters.AddWithValue("@LoadingDocumentID", data.Data.LoadingDocumentID);
                    command.Parameters.AddWithValue("@LoadingDocumentDate", data.Data.LoadingDocumentDate);
                    command.Parameters.AddWithValue("@ExtendedMitigationPlan", data.Data.ExtendedMitigationPlan);
                    command.Parameters.AddWithValue("@ReceiverRiskRPName", data.Data.ReceiverRiskRPName);
                    command.Parameters.AddWithValue("@ReceiverRiskRPEmail", data.Data.ReceiverRiskRPEmail);
                    command.Parameters.AddWithValue("@ReceiverDORMName", data.Data.ReceiverDORMName);
                    command.Parameters.AddWithValue("@ReceiverDORMEmail", data.Data.ReceiverDORMEmail);
                    command.Parameters.AddWithValue("@SenderRiskRPName", data.Data.SenderRiskRPName);
                    command.Parameters.AddWithValue("@SenderRiskRPEmail", data.Data.SenderRiskRPEmail);
                    command.Parameters.AddWithValue("@SenderDORMName", data.Data.SenderDORMName);
                    command.Parameters.AddWithValue("@SenderDORMEmail", data.Data.SenderDORMEmail);
                    command.Parameters.AddWithValue("@DocumentStatus", data.Data.DocumentStatus);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    command.Parameters.AddWithValue("@LinesData", dataItemLines);
                    command.Parameters.AddWithValue("@AttachLines", dataAttachments);

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

        internal bool createEPKRSIncidentAccidentDocument(QueryModel<IncidentAccident> data, DataTable dataAttachments)
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
                    command.CommandText = "[createEPKRSIncidentAccidentDocument]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@SubRiskID", data.Data.SubRiskID);
                    command.Parameters.AddWithValue("@DocumentID", data.Data.DocumentID);
                    command.Parameters.AddWithValue("@ReportDate", data.Data.ReportDate);
                    command.Parameters.AddWithValue("@OccurenceDate", data.Data.OccurenceDate);
                    command.Parameters.AddWithValue("@SiteReporter", data.Data.SiteReporter);
                    command.Parameters.AddWithValue("@DepartmentReporter", data.Data.DepartmentReporter);
                    command.Parameters.AddWithValue("@RiskRPName", data.Data.RiskRPName);
                    command.Parameters.AddWithValue("@RiskRPEmail", data.Data.RiskRPEmail);
                    command.Parameters.AddWithValue("@DORMName", data.Data.DORMName);
                    command.Parameters.AddWithValue("@DORMEmail", data.Data.DORMEmail);
                    command.Parameters.AddWithValue("@CaseDescription", data.Data.CaseDescription);
                    command.Parameters.AddWithValue("@DepartmentAffected", data.Data.DepartmentAffected);
                    command.Parameters.AddWithValue("@Cronology", data.Data.Cronology);
                    command.Parameters.AddWithValue("@RootCause", data.Data.RootCause);
                    command.Parameters.AddWithValue("@LossDescription", data.Data.LossDescription);
                    command.Parameters.AddWithValue("@LossEstimation", data.Data.LossEstimation);
                    command.Parameters.AddWithValue("@ReturnAmount", data.Data.ReturnAmount);
                    command.Parameters.AddWithValue("@RiskDescription", data.Data.RiskDescription);
                    command.Parameters.AddWithValue("@CauseDescription", data.Data.CauseDescription);
                    command.Parameters.AddWithValue("@PIC", data.Data.PIC);
                    command.Parameters.AddWithValue("@ActionPlan", data.Data.ActionPlan);
                    command.Parameters.AddWithValue("@TargetDate", data.Data.TargetDate);
                    command.Parameters.AddWithValue("@MitigationPlan", data.Data.MitigationPlan);
                    command.Parameters.AddWithValue("@MitigationDate", data.Data.MitigationDate);
                    command.Parameters.AddWithValue("@ExtendedRootCause", data.Data.ExtendedRootCause);
                    command.Parameters.AddWithValue("@ExtendedMitigationPlan", data.Data.ExtendedMitigationPlan);
                    command.Parameters.AddWithValue("@DocumentStatus", data.Data.DocumentStatus);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    command.Parameters.AddWithValue("@AttachLines", dataAttachments);

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

        internal bool createEPKRSCaseAttachment(DataTable data)
        {
            bool flag = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    // create and check schema table
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createEPKRSCaseAttachment]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@AttachLines", data);

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

        internal bool createEPKRSDocumentDiscussion(QueryModel<DocumentDiscussion> data, string location)
        {
            bool flag = false;
            int conInt = 0;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    // create and check schema table
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createEPKRSDocumentDiscussionSchema]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", location);

                    var scalar = command.ExecuteScalar();
                    conInt = Convert.ToInt32(scalar);

                    // create data
                    command.CommandText = "[createEPKRSDocumentDiscussion]";

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", location);
                    command.Parameters.AddWithValue("@DocumentID", data.Data.DocumentID);
                    command.Parameters.AddWithValue("@UserName", data.Data.UserName);
                    command.Parameters.AddWithValue("@CommentDate", data.Data.CommentDate);
                    command.Parameters.AddWithValue("@Comment", data.Data.Comment);
                    command.Parameters.AddWithValue("@isEdited", data.Data.isEdited);
                    command.Parameters.AddWithValue("@ReplyRowGuid", data.Data.ReplyRowGuid);
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

        internal bool createEPKRSDocumentDiscussionReadHistoryData(string location, DataTable data)
        {
            bool flag = false;
            int conInt = 0;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    // create and check schema table
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createEPKRSDocumentDiscussionReadHistoryData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", location);
                    command.Parameters.AddWithValue("@ReadHistories", data);

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

        internal bool createEPKRSDocumentApproval(QueryModel<DocumentApproval> data)
        {
            bool flag = false;
            int conInt = 0;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    // create and check schema table
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createEPKRSDocumentApproval]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
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

        internal bool editEPKRSDocumentExtendedandApprovalData(QueryModel<RISKApprovalExtended> data)
        {
            bool flag = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    // create and check schema table
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editEPKRSDocumentExtendedandApprovalData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ReportingType", data.Data.reportingType);
                    command.Parameters.AddWithValue("@DocumentID", data.Data.extendedData.DocumentID);
                    command.Parameters.AddWithValue("@ExtendedRootCause", data.Data.extendedData.ExtendedRootCause);
                    command.Parameters.AddWithValue("@ExtendedMitigationPlan", data.Data.extendedData.ExtendedMitigationPlan);
                    command.Parameters.AddWithValue("@ApprovalAction", data.Data.approval.ApprovalAction);
                    command.Parameters.AddWithValue("@Approver", data.Data.approval.Approver);
                    command.Parameters.AddWithValue("@ApproveDate", data.Data.approval.ApproveDate);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    command.Parameters.AddWithValue("@Involvers", CommonLibrary.ListToDataTable<IncidentAccidentInvolver>(data.Data.involver, data.userEmail, data.userAction, data.userActionDate, "Involver"));

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

        internal bool editEPKRSItemCaseData(QueryModel<EPKRSUploadItemCase> data, DataTable dataItemLines)
        {
            bool flag = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    // create and check schema table
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editEPKRSItemCaseDataAndLine]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@SubRiskID", data.Data.itemCase.SubRiskID);
                    command.Parameters.AddWithValue("@DocumentID", data.Data.itemCase.DocumentID);
                    command.Parameters.AddWithValue("@SiteReporter", data.Data.itemCase.SiteReporter);
                    command.Parameters.AddWithValue("@SiteSender", data.Data.itemCase.SiteSender);
                    command.Parameters.AddWithValue("@ReportDate", data.Data.itemCase.ReportDate);
                    command.Parameters.AddWithValue("@ItemPickupDate", data.Data.itemCase.ItemPickupDate);
                    command.Parameters.AddWithValue("@LoadingDocumentID", data.Data.itemCase.LoadingDocumentID);
                    command.Parameters.AddWithValue("@LoadingDocumentDate", data.Data.itemCase.LoadingDocumentDate);
                    command.Parameters.AddWithValue("@ReceiverRiskRPName", data.Data.itemCase.ReceiverRiskRPName);
                    command.Parameters.AddWithValue("@ReceiverRiskRPEmail", data.Data.itemCase.ReceiverRiskRPEmail);
                    command.Parameters.AddWithValue("@ReceiverDORMName", data.Data.itemCase.ReceiverDORMName);
                    command.Parameters.AddWithValue("@ReceiverDORMEmail", data.Data.itemCase.ReceiverDORMEmail);
                    command.Parameters.AddWithValue("@SenderRiskRPName", data.Data.itemCase.SenderRiskRPName);
                    command.Parameters.AddWithValue("@SenderRiskRPEmail", data.Data.itemCase.SenderRiskRPEmail);
                    command.Parameters.AddWithValue("@SenderDORMName", data.Data.itemCase.SenderDORMName);
                    command.Parameters.AddWithValue("@SenderDORMEmail", data.Data.itemCase.SenderDORMEmail);
                    command.Parameters.AddWithValue("@ExtendedMitigationPlan", data.Data.itemCase.ExtendedMitigationPlan);
                    command.Parameters.AddWithValue("@DocumentStatus", data.Data.itemCase.DocumentStatus);
                    command.Parameters.AddWithValue("@AuditUser", data.userEmail);
                    command.Parameters.AddWithValue("@AuditAction", data.userAction);
                    command.Parameters.AddWithValue("@AuditActionDate", data.userActionDate);

                    command.Parameters.AddWithValue("@LinesData", dataItemLines);

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

        internal bool editEPKRSIncidentAccidentData(QueryModel<IncidentAccident> data)
        {
            bool flag = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    // create and check schema table
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editEPKRSIncidentAccidentData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@SubRiskID", data.Data.SubRiskID);
                    command.Parameters.AddWithValue("@DocumentID", data.Data.DocumentID);
                    command.Parameters.AddWithValue("@ReportDate", data.Data.ReportDate);
                    command.Parameters.AddWithValue("@OccurenceDate", data.Data.OccurenceDate);
                    command.Parameters.AddWithValue("@SiteReporter", data.Data.SiteReporter);
                    command.Parameters.AddWithValue("@DepartmentReporter", data.Data.DepartmentReporter);
                    command.Parameters.AddWithValue("@RiskRPName", data.Data.RiskRPName);
                    command.Parameters.AddWithValue("@RiskRPEmail", data.Data.RiskRPEmail);
                    command.Parameters.AddWithValue("@DORMName", data.Data.DORMName);
                    command.Parameters.AddWithValue("@DORMEmail", data.Data.DORMEmail);
                    command.Parameters.AddWithValue("@CaseDescription", data.Data.CaseDescription);
                    command.Parameters.AddWithValue("@DepartmentAffected", data.Data.DepartmentAffected);
                    command.Parameters.AddWithValue("@Cronology", data.Data.Cronology);
                    command.Parameters.AddWithValue("@RootCause", data.Data.RootCause);
                    command.Parameters.AddWithValue("@LossDescription", data.Data.LossDescription);
                    command.Parameters.AddWithValue("@LossEstimation", data.Data.LossEstimation);
                    command.Parameters.AddWithValue("@ReturnAmount", data.Data.ReturnAmount);
                    command.Parameters.AddWithValue("@RiskDescription", data.Data.RiskDescription);
                    command.Parameters.AddWithValue("@CauseDescription", data.Data.CauseDescription);
                    command.Parameters.AddWithValue("@PIC", data.Data.PIC);
                    command.Parameters.AddWithValue("@ActionPlan", data.Data.ActionPlan);
                    command.Parameters.AddWithValue("@TargetDate", data.Data.TargetDate);
                    command.Parameters.AddWithValue("@MitigationPlan", data.Data.MitigationPlan);
                    command.Parameters.AddWithValue("@MitigationDate", data.Data.MitigationDate);
                    command.Parameters.AddWithValue("@ExtendedRootCause", data.Data.ExtendedRootCause);
                    command.Parameters.AddWithValue("@ExtendedMitigationPlan", data.Data.ExtendedMitigationPlan);
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

        internal bool deleteEPKRSItemCaseDocument(string id, string location, string user, string act, DateTime date)
        {
            bool flag = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    // create and check schema table
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[deleteEPKRSItemCaseDocument]";
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

        internal bool deleteEPKRSIncidentAccidentDocument(string id, string location, string user, string act, DateTime date)
        {
            bool flag = false;

            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    // create and check schema table
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[deleteEPKRSIncidentAccidentDocument]";
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

        internal DataTable getEPKRSReportingTypeData()
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
                    command.CommandText = "[getEPRKSReportingType]";
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

        internal DataTable getEPKRSRiskTypeData()
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
                    command.CommandText = "[getEPRKSRiskType]";
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

        internal DataTable getEPKRSRiskSubTypeData()
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
                    command.CommandText = "[getEPKRSSubRiskType]";
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

        internal DataTable getEPKRSItemRiskCategoryData()
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
                    command.CommandText = "[getEPKRSItemRiskCategory]";
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

        internal DataTable getEPKRSIncidentAccidentInvolerTypeData()
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
                    command.CommandText = "[getEPKRSIncidentAccidentInvolerType]";
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

        internal DataTable getEPKRSItemCaseData(string location, string conditions, int pageNo)
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
                    command.CommandText = "[getEPKRSItemCasebyFilter]";
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

        internal DataTable getEPKRSItemLineData(DataTable itemCaseData)
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
                    command.CommandText = "[getEPKRSItemLines]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ItemCases", itemCaseData);

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

        internal DataTable getEPKRSIncidentAccidentData(string location, string conditions, int pageNo)
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
                    command.CommandText = "[getEPKRSIncidentAccidentbyFilter]";
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

        internal DataTable getEPKRSAttachmentData(DataTable itemCaseData)
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
                    command.CommandText = "[getEPKRSCaseAttachments]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ItemCases", itemCaseData);

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

        internal DataTable getEPKRSApprovalData(DataTable itemCaseData)
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
                    command.CommandText = "[getEPKRSDocumentApprovalData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ItemCases", itemCaseData);

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

        internal DataTable getEPKRSIncidentAccidentInvolverData(DataTable itemCaseData)
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
                    command.CommandText = "[getEPKRSIncidentAccidentInvolverData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ItemCases", itemCaseData);

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

        internal DataTable getEPKRSDiscussionData(string location, string documentId)
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
                    command.CommandText = "[getEPKRSDocumentDiscussions]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", location);
                    command.Parameters.AddWithValue("@DocumentID", documentId);

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

        internal DataTable getEPKRSDocumentDiscussionReadHistoryData(string location, string id)
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
                    command.CommandText = "[getEPKRSDocumentDiscussionReadHistoryData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", location);
                    command.Parameters.AddWithValue("@DocumentID", id);

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

        internal DataTable getEPKRSIncidentAccidentDataforStatistics(string conditions)
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
                    command.CommandText = "[getEPKRSIncidentAccidentDataforStatistics]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Conditions", conditions);

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

        internal DataTable getEPKRSGeneralStatisticsData(string conditions, string ext)
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
                    command.CommandText = "[getEPKRSGeneralStatisticsData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Conditions", conditions);
                    command.Parameters.AddWithValue("@ConditionsExt", ext);

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

        internal DataTable getEPKRSGeneralIncidentAccidentStatisticsData(string conditions, string ext)
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
                    command.CommandText = "[getEPKRSGeneralIncidentAccidentStatisticsData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Conditions", conditions);
                    command.Parameters.AddWithValue("@ConditionsExt", ext);

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

        internal DataTable getEPKRSItemCaseCategoryStatisticsData(string conditions, string ext)
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
                    command.CommandText = "[getEPKRSItemCaseCategoryStatisticsData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Conditions", conditions);
                    command.Parameters.AddWithValue("@ConditionsExt", ext);

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

        internal DataTable getEPKRSTopLocationReportStatisticsData(string conditions, string ext)
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
                    command.CommandText = "[getEPKRSTopLocationReportStatisticsData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Conditions", conditions);
                    command.Parameters.AddWithValue("@ConditionsExt", ext);

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

        internal DataTable getEPKRSItemCategoriesStatisticsData(string conditions, string ext)
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
                    command.CommandText = "[getEPKRSItemCategoriesStatisticsData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Conditions", conditions);
                    command.Parameters.AddWithValue("@ConditionsExt", ext);

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

        internal DataTable getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmailData(string conditions)
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
                    command.CommandText = "[getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmail]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Conditions", conditions);

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

        internal DataTable getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPositionData(string conditions, string ext, int pageNo)
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
                    command.CommandText = "[getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPosition]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Conditions", conditions);
                    command.Parameters.AddWithValue("@ConditionsExt", ext);
                    command.Parameters.AddWithValue("@PageNo", pageNo);
                    command.Parameters.AddWithValue("@RowPerPage", _statRowPerPage);

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

        internal DataTable getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDeptData(string conditions, string ext, int pageNo)
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
                    command.CommandText = "[getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDept]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Conditions", conditions);
                    command.Parameters.AddWithValue("@ConditionsExt", ext);
                    command.Parameters.AddWithValue("@PageNo", pageNo);
                    command.Parameters.AddWithValue("@RowPerPage", _statRowPerPage);

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

        internal DataTable getEPKRSIncidentAccidentLocationStatistics(string conditions, int pageNo)
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
                    command.CommandText = "[getEPKRSIncidentAccidentLocationStatistics]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Conditions", conditions);
                    command.Parameters.AddWithValue("@PageNo", pageNo);
                    command.Parameters.AddWithValue("@RowPerPage", _statRowPerPage);

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

        internal DataTable getEPKRSIncidentAccidentReport(string conditions, string ext)
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
                    command.CommandText = "[getEPKRSIncidentAccidentReport]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Conditions", conditions);
                    command.Parameters.AddWithValue("@ConditionsExt", ext);

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

        internal DataTable getEPKRSItemCaseReport(string conditions, string ext)
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
                    command.CommandText = "[getEPKRSItemCaseReport]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Conditions", conditions);
                    command.Parameters.AddWithValue("@ConditionsExt", ext);

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

        internal DataTable getEPKRSCommentedUsersDiscussion(string loc, string docId, string rowGuid)
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
                    command.CommandText = "[getEPKRSCommentedUsersDiscussion]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@LocationID", loc);
                    command.Parameters.AddWithValue("@DocumentID", docId);
                    command.Parameters.AddWithValue("@rowGuid", rowGuid);

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

        internal int getEPKRSModuleNumberOfPageData(string TbName, string loc, string conditions)
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
                    command.CommandText = "[getEPKRSModulePageSize]";
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
