﻿using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.Company;
using BPIWebApplication.Shared.MainModel.Procedure;
using BPIWebApplication.Shared.PagesModel.AddEditProject;
using BPIWebApplication.Shared.PagesModel.AddEditUser;

namespace BPIWebApplication.Client.Services.ManagementServices
{
    public interface IManagementService
    {
        // data pool
        List<BisnisUnit> bisnisUnits { get; set; }
        List<Department> departments { get; set; }
        List<UserAdmin> users { get; set; }
        List<Project> projects { get; set; }
        List<LocationResp> locations { get; set; }
        List<BPIWebApplication.Shared.MainModel.Company.Category> categories { get; set; }

        // get
        Task<ResultModel<List<BisnisUnit>>> GetAllBisnisUnit(string param);
        Task<ResultModel<List<Department>>> GetAllDepartment(string param);
        Task<ResultModel<List<UserAdmin>>> GetAllUserAdmin();
        Task<ResultModel<List<Project>>> GetAllProject();
        Task<ResultModel<List<BPIWebApplication.Shared.MainModel.Company.Category>>> getAllCategories();
        Task<ResultModel<List<LocationResp>>> GetCompanyLocations(Location data);

        // create

        Task<ResultModel<QueryModel<Department>>> createNewDepartment(QueryModel<Department> data);
        Task<ResultModel<QueryModel<UserAdmin>>> createNewUserAdmin(QueryModel<UserAdmin> data);
        Task<ResultModel<QueryModel<Project>>> createNewProject(QueryModel<Project> data);

        // edit

        Task<ResultModel<QueryModel<Department>>> editDepartment(QueryModel<Department> data);
        Task<ResultModel<QueryModel<UserAdmin>>> editUser(QueryModel<UserAdmin> data);
        Task<ResultModel<QueryModel<Project>>> editProject(QueryModel<Project> data);

        // check existing

        Task<bool> checkDepartmentExisting(string DeptID);
        Task<bool> checkUserAdminExisting(string userEmail);
        Task<bool> checkProjectExisting(string projectNo);
    }
}
