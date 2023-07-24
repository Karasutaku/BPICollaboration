using BPIDA.DataAccess;
using BPIDA.Models.DbModel;
using BPIDA.Models.MainModel;
using BPIDA.Models.MainModel.Company;
using BPIDA.Models.PagesModel.AddEditProject;
using BPILibrary;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BPIDA.Controllers
{
    [Route("api/DA/BPIBase")]
    [ApiController]
    public class GeneralController : Controller
    {
        private readonly IConfiguration _configuration;

        public GeneralController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("getAllBisnisUnitData/{param}")]
        public async Task<IActionResult> getAllBisnisUnitDataTable(string param)
        {
            ResultModel<List<BisnisUnit>> res = new ResultModel<List<BisnisUnit>>();
            GeneralDA da = new(_configuration);
            List<BisnisUnit> bisnisUnit = new List<BisnisUnit>();
            DataTable dtBisnisUnit = new DataTable("BisnisUnitData");
            IActionResult actionResult = null;

            try
            {
                dtBisnisUnit = da.getAllBisnisUnitData(CommonLibrary.Base64Decode(param));

                if (dtBisnisUnit.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtBisnisUnit.Rows)
                    {
                        BisnisUnit temp = new BisnisUnit();

                        temp.BisnisUnitID = dt["bisnisUnitID"].ToString();
                        temp.BisnisUnitName = dt["bisnisUnitName"].ToString();
                        temp.BisnisUnitLabel = dt["bisnisUnitLabel"].ToString();

                        bisnisUnit.Add(temp);
                    }

                    res.Data = bisnisUnit;
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

        [HttpGet("getAllDepartmentData/{param}")]
        public async Task<IActionResult> getAllDepartmentDataTable(string param)
        {
            ResultModel<List<Department>> res = new ResultModel<List<Department>>();
            GeneralDA da = new(_configuration);
            List<Department> department = new List<Department>();
            DataTable dtDepartment = new DataTable("DepartmentData");
            IActionResult actionResult = null;

            try
            {
                dtDepartment = da.getAllDepartmentData(CommonLibrary.Base64Decode(param));

                if (dtDepartment.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtDepartment.Rows)
                    {
                        Department temp = new Department();

                        temp.DepartmentID = dt["departmentID"].ToString();
                        temp.DepartmentName = dt["departmentName"].ToString();
                        temp.DepartmentLabel = dt["departmentLabel"].ToString();
                        temp.BisnisUnitID = dt["bisnisUnitID"].ToString();

                        department.Add(temp);
                    }

                    res.Data = department;
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

        [HttpGet("getAllProjectData")]
        public async Task<IActionResult> getAllProjectDataTable()
        {
            ResultModel<List<Project>> res = new ResultModel<List<Project>>();
            GeneralDA da = new(_configuration);
            List<Project> project = new List<Project>();
            DataTable dtProject = new DataTable("ProjectData");
            IActionResult actionResult = null;

            try
            {
                dtProject = da.getAllProjectData();

                if (dtProject.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtProject.Rows)
                    {
                        Project temp = new Project();

                        temp.ProjectName = dt["projectName"].ToString();
                        temp.ProjectStatus = dt["projectStatus"].ToString();
                        temp.ProjectNote = dt["projectNote"].ToString();

                        project.Add(temp);
                    }

                    res.Data = project;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = null;
                    res.isSuccess = false;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "No Data";

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

        [HttpGet("getAllCategories")]
        public async Task<IActionResult> getAllCategories()
        {
            ResultModel<List<Category>> res = new ResultModel<List<Category>>();
            GeneralDA da = new(_configuration);
            List<Category> categoryLines = new List<Category>();
            DataTable dtCategory = new DataTable("Category");
            IActionResult actionResult = null;

            try
            {
                dtCategory = da.getAllCategoriesData();

                if (dtCategory.Rows.Count > 0)
                {
                    categoryLines = dtCategory.AsEnumerable().Select(x => new Category
                    {
                        CategoryID = x["CategoryID"].ToString(),
                        CategoryDescription = x["CategoryDescription"].ToString()
                    }).ToList();

                    res.Data = categoryLines;
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

        [HttpGet("getRegionData")]
        public async Task<IActionResult> getRegionData()
        {
            ResultModel<List<Region>> res = new ResultModel<List<Region>>();
            GeneralDA da = new(_configuration);
            List<Region> regionLines = new List<Region>();
            DataTable dtRegion = new DataTable("Region");
            IActionResult actionResult = null;

            try
            {
                dtRegion = da.getRegionData();

                if (dtRegion.Rows.Count > 0)
                {
                    regionLines = dtRegion.AsEnumerable().Select(x => new Region
                    {
                        RegionID = x["RegionID"].ToString(),
                        RegionDescription = x["RegionDescription"].ToString(),
                        RegionPIC = x["RegionPIC"].ToString(),
                        RegionPICEmail = x["RegionPICEmail"].ToString(),
                    }).ToList();

                    res.Data = regionLines;
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

        [HttpGet("getMasterUOMData")]
        public async Task<IActionResult> getMasterUOMData()
        {
            ResultModel<List<UOM>> res = new ResultModel<List<UOM>>();
            GeneralDA da = new(_configuration);
            List<UOM> uomLines = new List<UOM>();
            DataTable dtUOM = new DataTable("UOM");
            IActionResult actionResult = null;

            try
            {
                dtUOM = da.getMasterUOMData();

                if (dtUOM.Rows.Count > 0)
                {
                    uomLines = dtUOM.AsEnumerable().Select(x => new UOM
                    {
                        UOMID = Convert.ToInt32(x["UOMID"]),
                        UOMDescription = x["UOMDescription"].ToString(),
                        UOMPreview = x["UOMPreview"].ToString()
                    }).ToList();

                    res.Data = uomLines;
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

        // http get (check exist)

        [HttpGet("isDepartmentDataPresent/{DeptID}")]
        public async Task<IActionResult> isDepartmentDataPresentInTable(string DeptID)
        {
            ResultModel<string> res = new ResultModel<string>();
            GeneralDA da = new(_configuration);
            bool present = false;
            IActionResult actionResult = null;

            try
            {
                present = da.isDepartmentDataExist(CommonLibrary.Base64Decode(DeptID));

                if (present)
                {
                    res.Data = DeptID;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = DeptID;
                    res.isSuccess = false;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Success Fetch - Procedure Not Found";

                    actionResult = Ok(res);
                    //throw new Exception($"{}")
                }

            }
            catch (Exception ex)
            {
                res.Data = "";
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        [HttpGet("isProjectPresent/{projectNo}")]
        public async Task<IActionResult> isProjectDataPresentInTable(string projectNo)
        {
            ResultModel<string> res = new ResultModel<string>();
            GeneralDA da = new(_configuration);
            bool present = false;
            IActionResult actionResult = null;

            try
            {
                present = da.isProjectDataExist(CommonLibrary.Base64Decode(projectNo));

                if (present)
                {
                    res.Data = projectNo;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = projectNo;
                    res.isSuccess = false;
                    res.ErrorCode = "01";
                    res.ErrorMessage = "Success Fetch - Procedure Not Found";

                    actionResult = Ok(res);
                    //throw new Exception($"{}")
                }

            }
            catch (Exception ex)
            {
                res.Data = "";
                res.isSuccess = false;
                res.ErrorCode = "99";
                res.ErrorMessage = ex.Message;

                actionResult = BadRequest(res);
            }

            return actionResult;
        }

        // http post (create)

        [HttpPost("createNewDepartmentData")]
        public async Task<IActionResult> createNewDepartmentDataTable(QueryModel<Department> data)
        {
            ResultModel<QueryModel<Department>> res = new ResultModel<QueryModel<Department>>();
            GeneralDA da = new(_configuration);
            IActionResult actionResult = null;

            // create procedure data to db
            try
            {
                da.createNewDepartmentData(data);

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

        [HttpPost("createNewProjectData")]
        public async Task<IActionResult> createProjectDataTable(QueryModel<Project> data)
        {
            ResultModel<QueryModel<Project>> res = new ResultModel<QueryModel<Project>>();
            GeneralDA da = new(_configuration);
            IActionResult actionResult = null;

            // create procedure data to db
            try
            {
                da.createProjectData(data);

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

        // http post (edit)

        [HttpPost("editDepartmentData")]
        public async Task<IActionResult> editDepartmentDataTable(QueryModel<Department> data)
        {
            ResultModel<QueryModel<Department>> res = new ResultModel<QueryModel<Department>>();
            GeneralDA da = new(_configuration);
            IActionResult actionResult = null;

            // create procedure data to db
            try
            {
                da.editDepartmentData(data);

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


        [HttpPost("editProjectData")]
        public async Task<IActionResult> editProjectDataTable(QueryModel<Project> data)
        {
            ResultModel<QueryModel<Project>> res = new ResultModel<QueryModel<Project>>();
            GeneralDA da = new(_configuration);
            IActionResult actionResult = null;

            // create procedure data to db
            try
            {
                da.editProjectData(data);

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
