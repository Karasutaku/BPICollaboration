using BPIDA.Models.DbModel;
using BPIDA.Models.MainModel.Company;
using BPIDA.Models.PagesModel.AddEditProject;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BPIDA.DataAccess
{
    public class GeneralDA
    {
        private readonly IConfiguration _configuration;
        private readonly string _conString, _moduleConnection;

        public GeneralDA(IConfiguration config)
        {
            _configuration = config;
            _moduleConnection = _configuration.GetValue<string>("ModuleConnection:General");
            _conString = _configuration.GetValue<string>($"ConnectionStrings:{_moduleConnection}");
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


        // get DA

        internal DataTable getAllBisnisUnitData(string loc)
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
                    command.CommandText = "[getAllBisnisUnitData]";
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

        internal DataTable getAllDepartmentData(string loc)
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
                    command.CommandText = "[getAllDepartmentData]";
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

        internal DataTable getAllProjectData()
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
                    command.CommandText = "[getAllProjectData]";
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

        internal DataTable getAllCategoriesData()
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
                    command.CommandText = "[getCategories]";
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

        internal DataTable getRegionData()
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
                    command.CommandText = "[getRegionData]";
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

        internal DataTable getMasterUOMData()
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
                    command.CommandText = "[getMasterUOMData]";
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

        // create DA


        internal void createNewDepartmentData(QueryModel<Department> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createDepartmentData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@DepartmentID", data.Data.DepartmentID.ToUpper());
                    command.Parameters.AddWithValue("@DepartmentName", data.Data.DepartmentName.ToUpper());
                    command.Parameters.AddWithValue("@DepartmentLabel", data.Data.DepartmentLabel.ToUpper());
                    command.Parameters.AddWithValue("@BisnisUnitID", data.Data.BisnisUnitID);
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

        internal void createProjectData(QueryModel<Project> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[createProjectData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ProjectName", data.Data.ProjectName);
                    command.Parameters.AddWithValue("@ProjectStatus", data.Data.ProjectStatus);
                    command.Parameters.AddWithValue("@ProjectNote", data.Data.ProjectNote);
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

        // edit DA

        internal void editDepartmentData(QueryModel<Department> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editDepartmentData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@DepartmentID", data.Data.DepartmentID);
                    command.Parameters.AddWithValue("@DepartmentName", data.Data.DepartmentName);
                    command.Parameters.AddWithValue("@DepartmentLabel", data.Data.DepartmentLabel);
                    command.Parameters.AddWithValue("@BisnisUnitID", data.Data.BisnisUnitID);
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

        internal void editProjectData(QueryModel<Project> data)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                con.Open();
                SqlCommand command = new SqlCommand();

                try
                {
                    command.Connection = con;
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "[editProjectData]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ProjectName", data.Data.ProjectName);
                    command.Parameters.AddWithValue("@ProjectStatus", data.Data.ProjectStatus);
                    command.Parameters.AddWithValue("@ProjectNote", data.Data.ProjectNote);
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

        // is check exist DA

        internal bool isDepartmentDataExist(string deptID)
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
                    command.CommandText = "[isDepartmentPresent]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@DepartmentID", deptID);

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

        internal bool isProjectDataExist(string projectName)
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
                    command.CommandText = "[isProjectPresent]";
                    command.CommandTimeout = 1000;

                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ProjectName", projectName);

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

        //
    }
}
