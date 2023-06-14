using BPIDA.DataAccess;
using BPIDA.Models.DbModel;
using BPIDA.Models.MainModel;
using BPIDA.Models.MainModel.Company;
using BPIDA.Models.MainModel.Procedure;
using BPIDA.Models.MainModel.Procedure.Filter;
using BPIDA.Models.MainModel.Procedure.Report;
using BPILibrary;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BPIDA.Controllers
{
    [Route("api/DA/Procedure")]
    [ApiController]
    public class ProcedureController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _uploadPath, _archivePath;

        public ProcedureController(IConfiguration configuration)
        {
            _configuration = configuration;
            _uploadPath = _configuration.GetValue<string>("File:Procedure:UploadPath");
            _archivePath = _configuration.GetValue<string>("File:Procedure:ArchivePath");
        }

        internal Byte[] getFileStream(string dataPath, string procNo)
        {
            ProcedureDA da = new(_configuration);
            var date = da.getProcedureUploadDate(procNo);

            string type = string.Empty;

            if (dataPath.Contains("WI"))
            {
                type = "WI";
            }
            else if (dataPath.Contains("SOP"))
            {
                type = "SOP";
            }

            string path = Path.Combine(_uploadPath, type, date.Year.ToString(), date.Month.ToString(), date.Day.ToString(), Path.GetFileName(dataPath));

            return System.IO.File.ReadAllBytes(path);
        }

        // save file
        internal void saveFiletoDirectory(string path, Byte[] content)
        {
            string dir = Path.GetDirectoryName(path);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using FileStream fs = new(path, FileMode.Create);
            Stream stream = new MemoryStream(content);
            stream.CopyTo(fs);
        }

        [HttpGet("getAllProcedureData")]
        public async Task<IActionResult> getAllProcedureDataTable()
        {
            ResultModel<List<Procedure>> res = new ResultModel<List<Procedure>>();
            ProcedureDA da = new(_configuration);
            List<Procedure> procedure = new List<Procedure>();
            DataTable dtProcedure = new DataTable("ProcedureData");
            IActionResult actionResult = null;

            try
            {
                dtProcedure = da.getAllProcedureData();

                if (dtProcedure.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtProcedure.Rows)
                    {
                        Procedure temp = new Procedure();

                        temp.ProcedureNo = dt["procedureNo"].ToString();
                        temp.ProcedureName = dt["procedureName"].ToString();
                        temp.ProcedureDate = Convert.ToDateTime(dt["procedureDate"]);
                        temp.ProcedureWi = dt["procedureWiPath"].ToString();
                        temp.ProcedureSop = dt["procedureSopPath"].ToString();

                        procedure.Add(temp);
                    }

                    res.Data = procedure;
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

        [HttpGet("getDepartmentProcedureData")]
        public async Task<IActionResult> getAllDepartmentProcedureDataTable()
        {
            ResultModel<List<DepartmentProcedure>> res = new ResultModel<List<DepartmentProcedure>>();
            ProcedureDA da = new(_configuration);
            List<DepartmentProcedure> departmentProcedure = new List<DepartmentProcedure>();
            DataTable dtDepartmentProcedure = new DataTable("appliedProcedureData");
            IActionResult actionResult = null;

            try
            {
                dtDepartmentProcedure = da.getDepartmentProcedureData();

                if (dtDepartmentProcedure.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtDepartmentProcedure.Rows)
                    {
                        DepartmentProcedure temp = new DepartmentProcedure();

                        temp.ProcedureNo = dt["procedureNo"].ToString();
                        temp.DepartmentID = dt["departmentID"].ToString();

                        temp.Procedure = new Procedure();
                        temp.Procedure.ProcedureNo = dt["procedureNo"].ToString();
                        temp.Procedure.ProcedureName = dt["procedureName"].ToString();
                        temp.Procedure.ProcedureDate = Convert.ToDateTime(dt["procedureDate"]);
                        temp.Procedure.ProcedureWi = dt["procedureWiPath"].ToString();
                        temp.Procedure.ProcedureSop = dt["procedureSopPath"].ToString();

                        temp.Department = new Department();
                        temp.Department.DepartmentID = dt["departmentID"].ToString();
                        temp.Department.DepartmentName = dt["departmentName"].ToString();
                        temp.Department.DepartmentLabel = dt["departmentLabel"].ToString();

                        temp.Department.BisnisUnit = new BisnisUnit();
                        temp.Department.BisnisUnit.BisnisUnitID = dt["bisnisUnitID"].ToString();
                        temp.Department.BisnisUnit.BisnisUnitName = dt["bisnisUnitName"].ToString();
                        temp.Department.BisnisUnit.BisnisUnitLabel = dt["bisnisUnitLabel"].ToString();

                        departmentProcedure.Add(temp);
                    }

                    res.Data = departmentProcedure;
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

        [HttpGet("getDepartmentProcedureDatawithPaging/{param}")]
        public async Task<IActionResult> getAllDepartmentProcedureDataTablebyPaging(string param)
        {
            ResultModel<List<DepartmentProcedure>> res = new ResultModel<List<DepartmentProcedure>>();
            ProcedureDA da = new(_configuration);
            List<DepartmentProcedure> departmentProcedure = new List<DepartmentProcedure>();
            DataTable dtDepartmentProcedure = new DataTable("appliedProcedureData");
            IActionResult actionResult = null;

            try
            {
                string tempz = CommonLibrary.Base64Decode(param);

                string loc = tempz.Split("!_!")[0].Equals("") ? "HO" : tempz.Split("!_!")[0];
                int pageNo = Convert.ToInt32(tempz.Split("!_!")[1]);

                dtDepartmentProcedure = da.getDepartmentProcedureDatawithPaging(loc, pageNo);

                if (dtDepartmentProcedure.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtDepartmentProcedure.Rows)
                    {
                        DepartmentProcedure temp = new DepartmentProcedure();

                        temp.ProcedureNo = dt["procedureNo"].ToString();
                        temp.DepartmentID = dt["departmentID"].ToString();

                        temp.Procedure = new Procedure();
                        temp.Procedure.ProcedureNo = dt["procedureNo"].ToString();
                        temp.Procedure.ProcedureName = dt["procedureName"].ToString();
                        temp.Procedure.ProcedureDate = Convert.ToDateTime(dt["procedureDate"]);
                        temp.Procedure.ProcedureWi = dt["procedureWiPath"].ToString();
                        temp.Procedure.ProcedureSop = dt["procedureSopPath"].ToString();

                        temp.Department = new Department();
                        temp.Department.DepartmentID = dt["departmentID"].ToString();
                        temp.Department.DepartmentName = dt["departmentName"].ToString();
                        temp.Department.DepartmentLabel = dt["departmentLabel"].ToString();

                        temp.Department.BisnisUnit = new BisnisUnit();
                        temp.Department.BisnisUnit.BisnisUnitID = dt["bisnisUnitID"].ToString();
                        temp.Department.BisnisUnit.BisnisUnitName = dt["bisnisUnitName"].ToString();
                        temp.Department.BisnisUnit.BisnisUnitLabel = dt["bisnisUnitLabel"].ToString();

                        departmentProcedure.Add(temp);
                    }

                    res.Data = departmentProcedure;
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

        [HttpPost("getDepartmentProcedureDatawithFilterbyPaging")]
        public async Task<IActionResult> getAllDepartmentProcedureDataTablewithFilterbyPaging(DashboardFilter data)
        {
            ResultModel<List<DepartmentProcedure>> res = new ResultModel<List<DepartmentProcedure>>();
            ProcedureDA da = new(_configuration);
            List<DepartmentProcedure> departmentProcedure = new List<DepartmentProcedure>();
            DataTable dtDepartmentProcedure = new DataTable("appliedProcedureData");
            IActionResult actionResult = null;

            try
            {
                dtDepartmentProcedure = da.getDepartmentProcedureDatawithFilter(data);

                if (dtDepartmentProcedure.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtDepartmentProcedure.Rows)
                    {
                        DepartmentProcedure temp = new DepartmentProcedure();

                        temp.ProcedureNo = dt["procedureNo"].ToString();
                        temp.DepartmentID = dt["departmentID"].ToString();

                        temp.Procedure = new Procedure();
                        temp.Procedure.ProcedureNo = dt["procedureNo"].ToString();
                        temp.Procedure.ProcedureName = dt["procedureName"].ToString();
                        temp.Procedure.ProcedureDate = Convert.ToDateTime(dt["procedureDate"]);
                        temp.Procedure.ProcedureWi = dt["procedureWiPath"].ToString();
                        temp.Procedure.ProcedureSop = dt["procedureSopPath"].ToString();

                        temp.Department = new Department();
                        temp.Department.DepartmentID = dt["departmentID"].ToString();
                        temp.Department.DepartmentName = dt["departmentName"].ToString();
                        temp.Department.DepartmentLabel = dt["departmentLabel"].ToString();

                        temp.Department.BisnisUnit = new BisnisUnit();
                        temp.Department.BisnisUnit.BisnisUnitID = dt["bisnisUnitID"].ToString();
                        temp.Department.BisnisUnit.BisnisUnitName = dt["bisnisUnitName"].ToString();
                        temp.Department.BisnisUnit.BisnisUnitLabel = dt["bisnisUnitLabel"].ToString();

                        departmentProcedure.Add(temp);
                    }

                    res.Data = departmentProcedure;
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

        [HttpGet("getAllHistoryAccessDatawithPaging/{pageNo}")]
        public async Task<IActionResult> getAllHistoryAccessDataTablewithPaging(int pageNo)
        {
            ResultModel<List<HistoryAccess>> res = new ResultModel<List<HistoryAccess>>();
            ProcedureDA da = new(_configuration);
            List<HistoryAccess> historyAccess = new List<HistoryAccess>();
            DataTable dtHistoryAccess = new DataTable("HistoryAccessData");
            IActionResult actionResult = null;

            try
            {
                dtHistoryAccess = da.getAllHistoryAccessDatawithPaging(pageNo);

                if (dtHistoryAccess.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtHistoryAccess.Rows)
                    {
                        HistoryAccess temp = new HistoryAccess();

                        temp.ProcedureNo = dt["procedureNo"].ToString();
                        temp.ProcedureName = dt["procedureName"].ToString();
                        temp.UserEmail = dt["userEmail"].ToString();
                        temp.HistoryAccessDate = Convert.ToDateTime(dt["historyAccessDate"]);

                        historyAccess.Add(temp);
                    }

                    res.Data = historyAccess;
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

        [HttpPost("getAllHistoryAccessDatabyFilterwithPaging")]
        public async Task<IActionResult> getAllHistoryAccessDataTablebyFilterwithPaging(AccessHistoryFilter data)
        {
            ResultModel<List<HistoryAccess>> res = new ResultModel<List<HistoryAccess>>();
            ProcedureDA da = new(_configuration);
            List<HistoryAccess> historyAccess = new List<HistoryAccess>();
            DataTable dtHistoryAccess = new DataTable("HistoryAccessData");
            IActionResult actionResult = null;

            try
            {
                dtHistoryAccess = da.getAllHistoryAccessDatabyFilterwithPaging(data);

                if (dtHistoryAccess.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtHistoryAccess.Rows)
                    {
                        HistoryAccess temp = new HistoryAccess();

                        temp.ProcedureNo = dt["procedureNo"].ToString();
                        temp.ProcedureName = dt["procedureName"].ToString();
                        temp.UserEmail = dt["userEmail"].ToString();
                        temp.HistoryAccessDate = Convert.ToDateTime(dt["historyAccessDate"]);

                        historyAccess.Add(temp);
                    }

                    res.Data = historyAccess;
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

        [HttpPost("getAllHistoryAccessDataReportbyFilter")]
        public async Task<IActionResult> getAllHistoryAccessDataTableReportbyFilter(AccessHistoryReport data)
        {
            ResultModel<List<HistoryAccess>> res = new ResultModel<List<HistoryAccess>>();
            ProcedureDA da = new(_configuration);
            List<HistoryAccess> historyAccess = new List<HistoryAccess>();
            DataTable dtHistoryAccess = new DataTable("HistoryAccessData");
            IActionResult actionResult = null;

            try
            {
                dtHistoryAccess = da.getAllHistoryAccessDataReportbyFilter(data);

                if (dtHistoryAccess.Rows.Count > 0)
                {
                    foreach (DataRow dt in dtHistoryAccess.Rows)
                    {
                        HistoryAccess temp = new HistoryAccess();

                        temp.ProcedureNo = dt["procedureNo"].ToString();
                        temp.ProcedureName = dt["procedureName"].ToString();
                        temp.UserEmail = dt["userEmail"].ToString();
                        temp.HistoryAccessDate = Convert.ToDateTime(dt["historyAccessDate"]);

                        historyAccess.Add(temp);
                    }

                    res.Data = historyAccess;
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

        [HttpGet("getDepartmentProcedureNumberofPage/{param}")]
        public async Task<IActionResult> getDepartmentProcedureNumberofPageData(string param)
        {
            ResultModel<int> res = new ResultModel<int>();
            ProcedureDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                //string loc = param.Equals("") ? "HO" : param;
                string temp = CommonLibrary.Base64Decode(param);

                res.Data = da.getDepartmentProcedureNumberofPage(temp);

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

        [HttpPost("getDepartmentProcedurewithFilterNumberofPage")]
        public async Task<IActionResult> getDepartmentProcedurewithFilterNumberofPageData(DashboardFilter data)
        {
            ResultModel<int> res = new ResultModel<int>();
            ProcedureDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                res.Data = da.getDepartmentProcedurewithFilterNumberofPage(data);

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

        [HttpGet("getHistoryAccessNumberofPage")]
        public async Task<IActionResult> getHistoryAccessNumberofPageData()
        {
            ResultModel<int> res = new ResultModel<int>();
            ProcedureDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                res.Data = da.getHistoryAccessNumberofPage();

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

        [HttpPost("getHistoryAccessbyFilterwithPagingNumberofPage")]
        public async Task<IActionResult> getHistoryAccessNumberofPageData(AccessHistoryFilter data)
        {
            ResultModel<int> res = new ResultModel<int>();
            ProcedureDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                res.Data = da.getHistoryAccessbyFilterwithPagingNumberofPage(data);

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

        [HttpGet("getFile/{path}")]
        public async Task<IActionResult> getFiletoDownload(string path)
        {
            ResultModel<BPIDA.Models.MainModel.Stream.FileStream> res = new ResultModel<BPIDA.Models.MainModel.Stream.FileStream>();
            IActionResult actionResult = null;

            var temp = CommonLibrary.Base64Decode(path);
            try
            {
                // string[] file = Directory.GetFiles("F:\\BPI\\MainData\\WI\\2022\\08\\01");

                string[] splt = temp.Split("!_!");

                res.Data = new BPIDA.Models.MainModel.Stream.FileStream();
                res.Data.content = getFileStream(splt[0], splt[1]);
                res.Data.fileType = "PDF";
                res.Data.fileSize = res.Data.content.Length;

                // foreach (string f in file)
                res.Data.fileName = Path.GetFileName(splt[0]);

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


        [HttpGet("isProcedureDataPresent/{ProcNo}")]
        public async Task<IActionResult> isProcedureDataPresentInTable(string ProcNo)
        {
            ResultModel<string> res = new ResultModel<string>();
            ProcedureDA da = new(_configuration);
            bool present = false;
            DataTable dtProcedure = new DataTable("BisnisUnitData");
            IActionResult actionResult = null;

            string temp = CommonLibrary.Base64Decode(ProcNo);

            try
            {
                present = da.isProcedureDataPresent(temp);

                if (present)
                {
                    res.Data = ProcNo;
                    res.isSuccess = true;
                    res.ErrorCode = "00";
                    res.ErrorMessage = "";

                    actionResult = Ok(res);
                }
                else
                {
                    res.Data = ProcNo;
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

        // post

        [HttpPost("createProcedureData")]
        public async Task<IActionResult> createProcedureDataTableandFileSave(ProcedureStream data)
        {
            ResultModel<ProcedureStream> res = new ResultModel<ProcedureStream>();
            ProcedureDA da = new(_configuration);
            IActionResult actionResult = null;

            // create procedure data to db
            try
            {
                // save file

                string pathwi = "";
                string pathsop = "";

                foreach (var f in data.files)
                {
                    if (f.type == "WI")
                    {
                        // process for wi

                        string trustedFileName = Path.GetRandomFileName() + "_" + f.fileName;

                        pathwi = Path.Combine(_uploadPath, "WI", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), trustedFileName);

                        saveFiletoDirectory(pathwi, f.content);

                    }
                    else if (f.type == "SOP")
                    {
                        // process for sop

                        string trustedFileName = Path.GetRandomFileName() + "_" + f.fileName;

                        pathsop = Path.Combine(_uploadPath, "SOP", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), trustedFileName);

                        saveFiletoDirectory(pathsop, f.content);

                    }
                    else
                    {
                        res.Data = new ProcedureStream();
                        res.Data.procedureDetails = data.procedureDetails;
                        res.Data.files = data.files;
                        res.isSuccess = false;
                        res.ErrorCode = "01";
                        res.ErrorMessage = "Data Type is not Supported";

                        actionResult = Ok(res);
                    }
                }

                // insert data
                data.procedureDetails.Data.ProcedureWi = pathwi;
                data.procedureDetails.Data.ProcedureSop = pathsop;

                da.createProcedureData(data.procedureDetails);

                res.Data = new ProcedureStream();
                res.Data.procedureDetails = data.procedureDetails;
                res.Data.files = data.files;
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


        [HttpPost("createDepartmentProcedureData")]
        public async Task<IActionResult> createDepartmentProcedureDataTable(QueryModel<List<DepartmentProcedure>> data)
        {
            ResultModel<QueryModel<List<DepartmentProcedure>>> res = new ResultModel<QueryModel<List<DepartmentProcedure>>>();
            ProcedureDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                foreach (var deptapl in data.Data)
                {
                    if (da.isDepartmentProcedureDataPresent(deptapl.ProcedureNo, deptapl.DepartmentID))
                        continue;

                    QueryModel<DepartmentProcedure> dt = new QueryModel<DepartmentProcedure>();
                    dt.Data = new DepartmentProcedure();

                    dt.Data.ProcedureNo = deptapl.ProcedureNo;
                    dt.Data.DepartmentID = deptapl.DepartmentID;
                    dt.userEmail = data.userEmail;
                    dt.userAction = data.userAction;
                    dt.userActionDate = data.userActionDate;

                    da.createDepartmentProcedureData(dt);
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


        [HttpPost("createHistoryAccessData")]
        public async Task<IActionResult> createHistoryAccessDataTable(QueryModel<HistoryAccess> data)
        {
            ResultModel<QueryModel<HistoryAccess>> res = new ResultModel<QueryModel<HistoryAccess>>();
            ProcedureDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                da.createHistoryAccessData(data);

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

        [HttpPost("editProcedureData")]
        public async Task<IActionResult> editProcedureDataTable(ProcedureStream data)
        {
            ResultModel<ProcedureStream> res = new ResultModel<ProcedureStream>();
            ProcedureDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                string pathwi = "";
                string pathsop = "";

                if (!string.IsNullOrEmpty(data.procedureDetails.Data.ProcedureWi))
                {
                    // migrating wi file to archive

                    if (System.IO.File.Exists(data.procedureDetails.Data.ProcedureWi))
                    {
                        // check if exist
                        string trustedFileNameArc = Path.GetFileName(data.procedureDetails.Data.ProcedureWi);

                        string pathArchive = Path.Combine(_archivePath, "WI", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), trustedFileNameArc);

                        saveFiletoDirectory(pathArchive, getFileStream(data.procedureDetails.Data.ProcedureWi, data.procedureDetails.Data.ProcedureNo));
                        System.IO.File.Delete(data.procedureDetails.Data.ProcedureWi);
                    }
                }

                if (!string.IsNullOrEmpty(data.procedureDetails.Data.ProcedureSop))
                {
                    // migrating sop file to archive

                    if (System.IO.File.Exists(data.procedureDetails.Data.ProcedureSop))
                    {
                        // check if exist
                        string trustedFileNameArc = Path.GetFileName(data.procedureDetails.Data.ProcedureSop);

                        string pathArchive = Path.Combine(_archivePath, "SOP", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), trustedFileNameArc);

                        saveFiletoDirectory(pathArchive, getFileStream(data.procedureDetails.Data.ProcedureSop, data.procedureDetails.Data.ProcedureNo));
                        System.IO.File.Delete(data.procedureDetails.Data.ProcedureSop);
                    }
                }

                foreach (var f in data.files)
                {
                    if (f.type == "WI")
                    {
                        // process for wi

                        string trustedFileName = Path.GetRandomFileName() + "_" + f.fileName;

                        pathwi = Path.Combine(_uploadPath, "WI", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), trustedFileName);

                        saveFiletoDirectory(pathwi, f.content);

                    }
                    else if (f.type == "SOP")
                    {
                        // process for sop

                        string trustedFileName = Path.GetRandomFileName() + "_" + f.fileName;

                        pathsop = Path.Combine(_uploadPath, "SOP", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), trustedFileName);

                        saveFiletoDirectory(pathsop, f.content);

                    }
                    else
                    {
                        res.Data = new ProcedureStream();
                        res.Data.procedureDetails = data.procedureDetails;
                        res.Data.files = data.files;
                        res.isSuccess = false;
                        res.ErrorCode = "01";
                        res.ErrorMessage = "Data Type is not Supported";

                        actionResult = Ok(res);
                    }
                }

                // insert data
                data.procedureDetails.Data.ProcedureWi = pathwi;
                data.procedureDetails.Data.ProcedureSop = pathsop;

                da.editProcedureData(data.procedureDetails);

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


        [HttpPost("deleteDepartmentProcedureData")]
        public async Task<IActionResult> deleteDepartmentProcedureDataTablebyProcNoDeptID(QueryModel<List<DepartmentProcedure>> data)
        {
            ResultModel<QueryModel<List<DepartmentProcedure>>> res = new ResultModel<QueryModel<List<DepartmentProcedure>>>();
            ProcedureDA da = new(_configuration);
            IActionResult actionResult = null;

            try
            {
                foreach (var deptapl in data.Data)
                {
                    QueryModel<DepartmentProcedure> dt = new QueryModel<DepartmentProcedure>();
                    dt.Data = new DepartmentProcedure();

                    dt.Data.ProcedureNo = deptapl.ProcedureNo;
                    dt.Data.DepartmentID = deptapl.DepartmentID;
                    dt.userEmail = data.userEmail;
                    dt.userAction = data.userAction;
                    dt.userActionDate = data.userActionDate;

                    da.deleteDepartmentProcedureDatabyProcNoDeptID(dt);
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

        //
    }
}
