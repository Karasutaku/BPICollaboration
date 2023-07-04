using BPILibrary;
using BPIWebApplication.Client.Services.EPKRSServices;
using BPIWebApplication.Client.Services.FundReturnServices;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.FundReturn;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.PagesModel.FundReturn;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

namespace BPIWebApplication.Client.Pages.FundReturnPages
{
    public partial class FundReturnDashboard : ComponentBase
    {
        private ActiveUser activeUser = new();
        private UserPrivileges privilegeDataParam = new();
        private List<string> userPriv = new();

        private Location paramGetCompanyLocation = new();

        private FundReturnDocument previewRefundDocument = new();

        private int fundReturnNumberofPage = 0;
        private int fundReturnPageActive = 0;

        private string fundReturnDocumentFilterType { get; set; } = string.Empty;
        private string fundReturnDocumentFilterValue { get; set; } = string.Empty;

        private bool fundReturnFilterActive { get; set; } = false;

        private bool isLoading = false;
        private bool successUpload = false;

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
                int moduleid = Convert.ToInt32(LoginService.moduleList.SingleOrDefault(x => x.url.Equals("/fundreturn/dashboard")).moduleId);
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

            await FundReturnService.getFundReturnBank();
            await FundReturnService.getFundReturnCategory();

            paramGetCompanyLocation.Condition = $"a.CompanyId={Convert.ToInt32(activeUser.company)}";
            paramGetCompanyLocation.PageIndex = 1;
            paramGetCompanyLocation.PageSize = 100;
            paramGetCompanyLocation.FieldOrder = "a.CompanyId";
            paramGetCompanyLocation.MethodOrder = "ASC";

            await ManagementService.GetCompanyLocations(paramGetCompanyLocation);

            fundReturnPageActive = 1;

            string paramGetFundReturn = activeUser.location + "!_!!_!1";
            await FundReturnService.getFundReturnDocuments(CommonLibrary.Base64Encode(paramGetFundReturn));

            string fundReturnParampz = $"FundReturnHeader!_!{activeUser.location}!_!";
            var fundReturnpz = await FundReturnService.getFundReturnModuleNumberOfPage(CommonLibrary.Base64Encode(fundReturnParampz));
            fundReturnNumberofPage = fundReturnpz.Data;

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/FundReturnPages/FundReturnDashboard.razor.js");
        }

        private Stream GetFileStream(byte[] data)
        {
            var fileBinData = data;
            var fileStream = new MemoryStream(fileBinData);

            return fileStream;
        }

        private async Task HandleViewDocument(Byte[] content, string filename)
        {
            var filestream = GetFileStream(content);

            using var streamRef = new DotNetStreamReference(stream: filestream);

            await _jsModule.InvokeVoidAsync("previewFileFromStream", filename, streamRef);
        }

        private async Task setRefundDocument(FundReturnDocument data)
        {
            previewRefundDocument = data;
            FundReturnService.fileStreams.Clear();

            #pragma warning disable CS4014
            Task.Run(async () =>
            {
                await FundReturnService.getFundReturnFileStream(CommonLibrary.Base64Encode(data.dataHeader.LocationID + "!_!" + data.dataHeader.DocumentID));
                StateHasChanged();
            });
            #pragma warning restore CS4014

            StateHasChanged();
        }

        private async Task fundReturnPageSelect(int currPage)
        {
            fundReturnPageActive = currPage;
            isLoading = true;
            string getFundReturnDocumentParam = string.Empty;

            if (fundReturnFilterActive)
            {

            }
            else
            {
                getFundReturnDocumentParam = activeUser.location + "!_!!_!" + fundReturnPageActive.ToString();
            }

            await FundReturnService.getFundReturnDocuments(CommonLibrary.Base64Encode(getFundReturnDocumentParam));

            isLoading = false;
            StateHasChanged();
        }

        private bool checkFundReturnDocument()
        {
            try
            {
                if (FundReturnService.fundReturns.Any())
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

        private bool checkPreviewRefundLines()
        {
            try
            {
                if (previewRefundDocument.dataItemLines.Any())
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

        private bool checkPreviewRefundApprovalLine()
        {
            try
            {
                if (previewRefundDocument.dataApproval.Any())
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

        private bool checkPreviewRefundAttachment()
        {
            try
            {
                if (previewRefundDocument.dataAttachmentLines.Any())
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
