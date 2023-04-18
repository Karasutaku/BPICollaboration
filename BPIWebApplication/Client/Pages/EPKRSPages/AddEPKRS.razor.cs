using BPILibrary;
using BPIWebApplication.Client.Services.ManagementServices;
using BPIWebApplication.Client.Services.PettyCashServices;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.PagesModel.EPKRS;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

namespace BPIWebApplication.Client.Pages.EPKRSPages
{
    public partial class AddEPKRS : ComponentBase
    {
        private ActiveUser activeUser = new();
        private UserPrivileges privilegeDataParam = new();
        private List<string> userPriv = new();

        private ItemCaseForm itemCaseData = new();
        private List<ItemLineForm> itemLineData = new();
        private IncidentAccidentForm incidentaccidentData = new();
        private List<CaseAttachmentForm> caseAttachments = new();

        private string RiskID { get; set; } = string.Empty;
        private string isLate { get; set; } = string.Empty;
        private string isCCTVCovered { get; set; } = string.Empty;
        private string isReportedtoSender { get; set; } = string.Empty;

        private int formPage { get; set; } = 0;
        private int maxFileSize { get; set; } = 0;
        private string previewLossEstimationAmount { get; set; } = string.Empty;
        private string previewReturnAmount { get; set; } = string.Empty;

        private Dictionary<string, Dictionary<int, string>> inputPageMapping = new()
        {
            {
                "ITEMC", 
                new Dictionary<int, string> {
                    { 1, "Report Header" },
                    { 2, "Item Details and Attachment" }
                }
            },
            {
                "INCAC",
                new Dictionary<int, string> {
                    { 1, "Report Header" },
                    { 2, "Case Details" },
                    { 3, "Company Loss" },
                    { 4, "Mitigation" }
                }
            }
        };

        private bool isLoading = false;

        private bool alertTrigger = false;
        private bool successAlert = false;
        private string alertBody = string.Empty;
        private string alertMessage = string.Empty;

        private IJSObjectReference _jsModule;

        protected override async Task OnInitializedAsync()
        {
            if (!LoginService.activeUser.userPrivileges.IsNullOrEmpty())
                LoginService.activeUser.userPrivileges.Clear();

            if (syncSessionStorage.ContainKey("PagePrivileges"))
                syncSessionStorage.RemoveItem("PagePrivileges");

            string tkn = syncSessionStorage.GetItem<string>("token");

            if (syncSessionStorage.ContainKey("userName"))
            {
                int moduleid = Convert.ToInt32(LoginService.moduleList.SingleOrDefault(x => x.url.Equals("/epkrs/dashboard")).moduleId);
                privilegeDataParam.moduleId = moduleid;
                privilegeDataParam.UserName = CommonLibrary.Base64Decode(syncSessionStorage.GetItem<string>("userName"));
                privilegeDataParam.userLocationParam = new();
                privilegeDataParam.userLocationParam.SessionId = syncSessionStorage.GetItem<string>("SessionId");
                privilegeDataParam.userLocationParam.MacAddress = "";
                privilegeDataParam.userLocationParam.IpClient = "";
                privilegeDataParam.userLocationParam.ApplicationId = Convert.ToInt32(CommonLibrary.Base64Decode(syncSessionStorage.GetItem<string>("AppV")));
                privilegeDataParam.userLocationParam.LocationId = CommonLibrary.Base64Decode(syncSessionStorage.GetItem<string>("CompLoc")).Split("_")[1];
                privilegeDataParam.userLocationParam.Name = CommonLibrary.Base64Decode(syncSessionStorage.GetItem<string>("userName"));
                privilegeDataParam.userLocationParam.CompanyId = Convert.ToInt32(CommonLibrary.Base64Decode(syncSessionStorage.GetItem<string>("CompLoc")).Split("_")[0]);
                privilegeDataParam.userLocationParam.PageIndex = 1;
                privilegeDataParam.userLocationParam.PageSize = 100;
                privilegeDataParam.privileges = new();
            }

            var res = await LoginService.frameworkApiFacadePrivilege(privilegeDataParam, tkn);

            userPriv.Clear();

            if (res.isSuccess)
            {
                if (res.Data.privileges.Any())
                {
                    foreach (var priv in res.Data.privileges)
                    {
                        userPriv.Add(priv.privilegeId);
                    }
                }

                syncSessionStorage.SetItem("PagePrivileges", userPriv);

                LoginService.activeUser.userPrivileges = userPriv;
                syncSessionStorage.RemoveItem("ModuleId");

            }
            //

            activeUser.token = await sessionStorage.GetItemAsync<string>("token");
            activeUser.userName = CommonLibrary.Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            activeUser.company = CommonLibrary.Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[0];
            activeUser.location = CommonLibrary.Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];
            activeUser.sessionId = await sessionStorage.GetItemAsync<string>("SessionId");
            activeUser.appV = Convert.ToInt32(CommonLibrary.Base64Decode(await sessionStorage.GetItemAsync<string>("AppV")));
            activeUser.userPrivileges = await sessionStorage.GetItemAsync<List<string>>("PagePrivileges");

            LoginService.activeUser.userPrivileges = activeUser.userPrivileges;

            string loc = activeUser.location.Equals("") ? "HO" : activeUser.location;

            await EPKRSService.getEPRKSReportingType();
            await EPKRSService.getEPRKSRiskType();
            await ManagementService.GetAllDepartment(CommonLibrary.Base64Encode(loc));

            formPage = 1;
            RiskID = "BLANK";
            isLate = "BLANK";
            isCCTVCovered = "BLANK";
            isReportedtoSender = "BLANK";
            incidentaccidentData.SiteReporter = loc;

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/EPKRSPages/addEPKRS.razor.js");
        }

        private void selectItembyLoadingDocumentID()
        {

        }

        private void submitIncidentAccident()
        {

        }

        private void reportingTypeonChange(ChangeEventArgs e)
        {
            RiskID = e.Value.ToString();

            formPage = 1;
            caseAttachments.Clear();
            itemLineData.Clear();

            itemCaseData = new();
            incidentaccidentData = new();

            StateHasChanged();
        }

        private bool checkItemLine()
        {
            try
            {
                if (itemLineData.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool checkCaseAttachment()
        {
            try
            {
                if (caseAttachments.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool checkLocation()
        {
            try
            {
                if (ManagementService.locations.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool checkReportingType()
        {
            try
            {
                if (EPKRSService.reportingTypes.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool checkRiskType()
        {
            try
            {
                if (EPKRSService.riskTypes.Any())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //
    }
}
