using BPILibrary;
using BPIWebApplication.Client.Services.CashierLogbookServices;
using BPIWebApplication.Client.Services.EPKRSServices;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.MainModel.POMF;
using BPIWebApplication.Shared.PagesModel.POMF;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

namespace BPIWebApplication.Client.Pages.POMFPages
{
    public partial class POMFDashboard : ComponentBase
    {
        private ActiveUser activeUser = new();
        private UserPrivileges privilegeDataParam = new();
        private List<string> userPriv = new();

        private POMFDocument previewPOMFDocument = new();
        private POMFDocument previewPOMFConfirmDocument = new();
        private POMFHeader editPOMFDocumentHeader = new();
        private POMFApproval previewPOMFApproval = new();

        private List<POMFDocument> selectedPOMFDocument = new();

        private int pomfNumberofPage = 0;
        private int pomfPageActive = 0;

        private int pomfConfirmedNumberofPage = 0;
        private int pomfConfirmedPageActive = 0;

        private string previousDocumentLocation = string.Empty;
        private string previousDocumentID = string.Empty;

        private string pomfDocumentFilterType { get; set; } = string.Empty;
        private string pomfDocumentFilterValue { get; set; } = string.Empty;

        private bool pomfFilterActive { get; set; } = false;

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
                int moduleid = Convert.ToInt32(LoginService.moduleList.SingleOrDefault(x => x.url.Equals("/pomf/dashboard")).moduleId);
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

            string getPOMFDocumentParam = activeUser.location + "!_!!_!1!_!";
            await POMFService.getPOMFDocuments(CommonLibrary.Base64Encode(getPOMFDocumentParam));

            string getPOMFConfDocumentParam = activeUser.location + "!_!WHERE DocumentStatus = \'Confirmed\'!_!1!_!CONF";
            await POMFService.getPOMFDocuments(CommonLibrary.Base64Encode(getPOMFConfDocumentParam));

            string getPOMFDocumentpz = "POMFHeader!_!" + activeUser.location + "!_!";
            var pomfpz = await POMFService.getPOMFModuleNumberOfPage(CommonLibrary.Base64Encode(getPOMFDocumentpz));
            pomfNumberofPage = pomfpz.Data;
            pomfPageActive = 1;

            string getPOMFCFDocumentpz = "POMFHeader!_!" + activeUser.location + "!_!WHERE DocumentStatus = \'Confirmed\'";
            var pomfCFpz = await POMFService.getPOMFModuleNumberOfPage(CommonLibrary.Base64Encode(getPOMFCFDocumentpz));
            pomfConfirmedNumberofPage = pomfCFpz.Data;
            pomfConfirmedPageActive = 1;

            await POMFService.getPOMFNPType();

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/POMFPages/POMFDashboard.razor.js");
        }

        private void setPOMFDocument(POMFDocument data)
        {
            previewPOMFDocument = data;
            previousDocumentLocation = data.dataHeader.LocationID;
            previousDocumentID = data.dataHeader.POMFID;

            editPOMFDocumentHeader = data.dataHeader;

            StateHasChanged();
        }

        private void setPOMFConfirmDocument(POMFDocument data)
        {
            previewPOMFConfirmDocument = data;
            previousDocumentLocation = data.dataHeader.LocationID;
            previousDocumentID = data.dataHeader.POMFID;

            StateHasChanged();
        }

        private void setPOMFApproval(string approveType)
        {
            previewPOMFApproval.POMFID = previousDocumentID;
            previewPOMFApproval.ApprovalAction = approveType;
            previewPOMFApproval.Approver = activeUser.userName;
            previewPOMFApproval.ApproveDate = DateTime.Now;

            StateHasChanged();
        }

        private void appendPOMFConfirmed(POMFDocument data)
        {
            if (selectedPOMFDocument.FirstOrDefault(x => x.dataHeader.POMFID.Equals(data.dataHeader.POMFID)) == null)
            {
                selectedPOMFDocument.Add(new POMFDocument { 
                    dataHeader = data.dataHeader,
                    dataItemLines = data.dataItemLines,
                    dataApproval = data.dataApproval
                });
            }
            else
            {
                var dt = selectedPOMFDocument.SingleOrDefault(x => x.dataHeader.POMFID.Equals(data.dataHeader.POMFID));
                if (dt != null)
                {
                    selectedPOMFDocument.Remove(dt);
                }
            }
        }

        private void selectAllPOMFConfirmed ()
        { 
            if (selectedPOMFDocument.Any()) 
            {
                var temp = POMFService.pomfConfirmedDocuments.ExceptBy(selectedPOMFDocument.Select(x => x.dataHeader.POMFID), f => f.dataHeader.POMFID).ToList();
                
                if (temp.Count > 0)
                {
                    selectedPOMFDocument.AddRange(temp);
                }

            } 
            else 
            {
                POMFService.pomfConfirmedDocuments.ForEach(x => {
                    selectedPOMFDocument.Add(new POMFDocument {
                        dataHeader = x.dataHeader,
                        dataItemLines = x.dataItemLines,
                        dataApproval = x.dataApproval
                    }); 
                }); 
            }

            StateHasChanged();
        }

        private async Task exportCSV()
        {
            try
            {
                if (selectedPOMFDocument.Count > 0)
                {
                    List<POMFExportCSVModel> exportData = new();

                    foreach (var dt in selectedPOMFDocument.SelectMany(x => x.dataItemLines))
                    {
                        exportData.Add(new POMFExportCSVModel
                        {
                            ItemCode = dt.ItemCode,
                            ItemBonus = 0,
                            Quantity = dt.RequestQuantity,
                            UOM = dt.ItemUOM,
                            Price = decimal.Zero,
                            Discount = 0,
                            VAT = "PPN11"
                        });
                    }

                    var streamdt = CommonLibrary.ListToCSV<POMFExportCSVModel>(exportData, ";");
                    using var streamRef = new DotNetStreamReference(stream: streamdt);

                    await _jsModule.InvokeVoidAsync("downloadFileFromStream", $"POMF Export {DateTime.Now.ToString("ddMMyyyy")}.csv", streamRef);

                    selectedPOMFDocument.Clear();
                    StateHasChanged();
                }
                else
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "Select Document to Export !");
                }
            }
            catch (Exception ex)
            {
                StateHasChanged();
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task createDocumentApprove(string approveType)
        {
            try
            {
                if (!new[] { "Release", "Receive" }.Contains(approveType))
                {
                    isLoading = true;

                    QueryModel<POMFApprovalStream> uploadData = new();
                    uploadData.Data = new();
                    uploadData.Data.Data = new();

                    //previewPOMFApproval.ApproveDate = DateTime.Now;

                    uploadData.Data.LocationID = previousDocumentLocation;
                    uploadData.Data.Data = previewPOMFApproval;
                    uploadData.Data.Data.ApproveDate = DateTime.Now;
                    uploadData.userEmail = activeUser.userName;
                    uploadData.userAction = "I";
                    uploadData.userActionDate = DateTime.Now;

                    var res = await POMFService.createPOMFApproval(uploadData);

                    if (res.isSuccess)
                    {
                        string param = approveType.Equals("Verify") ? "Verified" : approveType.Equals("Confirm") ? "Confirmed" : approveType.Equals("Release") ? "Released" : approveType.Equals("Receive") ? "Received" : "NAN";

                        previewPOMFDocument.dataHeader.DocumentStatus = param;
                        previewPOMFDocument.dataApproval.Add(res.Data.Data.Data);
                        POMFService.pomfDocuments.SingleOrDefault(x => x.dataHeader.POMFID.Equals(previewPOMFApproval.POMFID)).dataHeader.DocumentStatus = param;

                        var dt = POMFService.pomfDocuments.SingleOrDefault(x => x.dataHeader.POMFID.Equals(previewPOMFApproval.POMFID));

                        if (approveType.Equals("Confirm") && dt != null)
                            POMFService.pomfConfirmedDocuments.Add(dt);

                        await _jsModule.InvokeVoidAsync("showAlert", "Update Status Success !");
                    }
                    else
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", $"Failed : {res.ErrorCode} - {res.ErrorMessage} !");
                    }
                }
                else
                {
                    if (approveType.Equals("Release") && editPOMFDocumentHeader.ExternalRequestDocument.IsNullOrEmpty())
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", "Request Document Input Field is Empty !");
                        return;
                    }

                    if (approveType.Equals("Receive") && editPOMFDocumentHeader.ExternalReceiveDocument.IsNullOrEmpty())
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", "Receive Document Input Field is Empty !");
                        return;
                    }

                    isLoading = true;

                    QueryModel<POMFApprovalStreamExtended> uploadData = new();
                    uploadData.Data = new();
                    uploadData.Data.pomfHeader = new();
                    uploadData.Data.approvalData = new();

                    //previewPOMFApproval.ApproveDate = DateTime.Now;

                    uploadData.Data.pomfHeader = editPOMFDocumentHeader;
                    uploadData.Data.approvalData = previewPOMFApproval;
                    uploadData.Data.approvalData.ApproveDate = DateTime.Now;

                    if (approveType.Equals("Release"))
                    {
                        uploadData.Data.pomfHeader.RequestDocumentDate = DateTime.Now;
                        uploadData.Data.pomfHeader.ReceiveDocumentDate = DateTime.MinValue.AddYears(1800);
                    }

                    if (approveType.Equals("Receive"))
                        uploadData.Data.pomfHeader.ReceiveDocumentDate = DateTime.Now;

                    uploadData.userEmail = activeUser.userName;
                    uploadData.userAction = "I";
                    uploadData.userActionDate = DateTime.Now;

                    var res = await POMFService.createPOMFApprovalExtended(uploadData);

                    if (res.isSuccess)
                    {
                        string param = approveType.Equals("Verify") ? "Verified" : approveType.Equals("Confirm") ? "Confirmed" : approveType.Equals("Release") ? "Released" : approveType.Equals("Receive") ? "Received" : "NAN";

                        previewPOMFDocument.dataHeader = res.Data.Data.pomfHeader;
                        previewPOMFDocument.dataApproval.Add(res.Data.Data.approvalData);
                        POMFService.pomfDocuments.SingleOrDefault(x => x.dataHeader.POMFID.Equals(previewPOMFApproval.POMFID)).dataHeader.DocumentStatus = param;

                        await _jsModule.InvokeVoidAsync("showAlert", "Update Status Success !");
                    }
                    else
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", $"Failed : {res.ErrorCode} - {res.ErrorMessage} !");
                    }
                }

                isLoading = false;
                StateHasChanged();

            }
            catch (Exception ex)
            {
                isLoading = false;
                StateHasChanged();
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task deleteDocument()
        {
            try
            {
                isLoading = true;

                QueryModel<string> paramData = new();

                string temp = previewPOMFDocument.dataHeader.POMFID + "!_!" + previewPOMFDocument.dataHeader.LocationID;

                paramData.Data = CommonLibrary.Base64Encode(temp);
                paramData.userEmail = activeUser.userName;
                paramData.userAction = "D";
                paramData.userActionDate = DateTime.Now;

                var res = await POMFService.deletePOMFDocument(paramData);

                if (res.isSuccess)
                {
                    previewPOMFDocument.dataHeader.DocumentStatus = "DELETED";
                    POMFService.pomfDocuments.SingleOrDefault(x => x.dataHeader.POMFID.Equals(previewPOMFDocument.dataHeader.POMFID)).dataHeader.DocumentStatus = "DELETED";
                    await _jsModule.InvokeVoidAsync("showAlert", "Delete Document Success, Please REFRESH Your Page !");
                }
                else
                {
                    await _jsModule.InvokeVoidAsync("showAlert", $"Failed : {res.ErrorCode} - {res.ErrorMessage} !");
                }

                isLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                isLoading = false;
                StateHasChanged();
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task pomfPageSelect(int currPage)
        {
            pomfPageActive = currPage;
            isLoading = true;
            string getPOMFDocumentParam = string.Empty;

            if (pomfFilterActive)
            {
                if (pomfDocumentFilterType.Equals("ID"))
                {
                    string paramGetPOMF = activeUser.location + $"!_!WHERE POMFID LIKE \'%{pomfDocumentFilterValue}%\'!_!{pomfPageActive.ToString()}!_!";
                    await POMFService.getPOMFDocuments(CommonLibrary.Base64Encode(paramGetPOMF));
                }
                else if (pomfDocumentFilterType.Equals("NPN"))
                {
                    string paramGetPOMF = activeUser.location + $"!_!WHERE NPNo LIKE \'%{pomfDocumentFilterValue}%\'!_!{pomfPageActive.ToString()}!_!";
                    await POMFService.getPOMFDocuments(CommonLibrary.Base64Encode(paramGetPOMF));
                }
                else if (pomfDocumentFilterType.Equals("RCN"))
                {
                    string paramGetPOMF = activeUser.location + $"!_!WHERE ReceiptNo LIKE \'%{pomfDocumentFilterValue}%\'!_!{pomfPageActive.ToString()}!_!";
                    await POMFService.getPOMFDocuments(CommonLibrary.Base64Encode(paramGetPOMF));
                }
                else if (pomfDocumentFilterType.Equals("CUST"))
                {
                    string paramGetPOMF = activeUser.location + $"!_!WHERE CustomerName LIKE \'%{pomfDocumentFilterValue}%\'!_!{pomfPageActive.ToString()}!_!";
                    await POMFService.getPOMFDocuments(CommonLibrary.Base64Encode(paramGetPOMF));
                }
                else if (pomfDocumentFilterType.Equals("STATUS"))
                {
                    string paramGetPOMF = activeUser.location + $"!_!WHERE DocumentStatus LIKE \'%{pomfDocumentFilterValue}%\'!_!{pomfPageActive.ToString()}!_!";
                    await POMFService.getPOMFDocuments(CommonLibrary.Base64Encode(paramGetPOMF));
                }
            }
            else
            {
                getPOMFDocumentParam = activeUser.location + "!_!!_!" + pomfPageActive.ToString() + "!_!";
            }

            await POMFService.getPOMFDocuments(CommonLibrary.Base64Encode(getPOMFDocumentParam));

            isLoading = false;
            StateHasChanged();
        }

        private async Task pomfConfirmedPageSelect(int currPage)
        {
            pomfConfirmedPageActive = currPage;
            isLoading = true;
            string getPOMFDocumentParam = string.Empty;

            getPOMFDocumentParam = activeUser.location + "!_!WHERE DocumentStatus = \'Confirmed\'!_!" + pomfConfirmedPageActive.ToString() + "!_!CONF";

            await POMFService.getPOMFDocuments(CommonLibrary.Base64Encode(getPOMFDocumentParam));

            isLoading = false;
            StateHasChanged();
        }

        private async void pomfFilter()
        {
            if (pomfDocumentFilterType.Length > 0)
            {
                pomfFilterActive = true;
                isLoading = true;
                POMFService.pomfDocuments.Clear();
                pomfPageActive = 1;

                if (pomfDocumentFilterType.Equals("ID"))
                {
                    string pomfParampz = $"POMFHeader!_!{activeUser.location}!_!WHERE POMFID LIKE \'%{pomfDocumentFilterValue}%\'";
                    var pomfpz = await POMFService.getPOMFModuleNumberOfPage(CommonLibrary.Base64Encode(pomfParampz));
                    pomfNumberofPage = pomfpz.Data;

                    string paramGetPOMF = activeUser.location + $"!_!WHERE POMFID LIKE \'%{pomfDocumentFilterValue}%\'!_!1!_!";
                    await POMFService.getPOMFDocuments(CommonLibrary.Base64Encode(paramGetPOMF));
                }
                else if (pomfDocumentFilterType.Equals("NPN"))
                {
                    string pomfParampz = $"POMFHeader!_!{activeUser.location}!_!WHERE NPNo LIKE \'%{pomfDocumentFilterValue}%\'";
                    var pomfpz = await POMFService.getPOMFModuleNumberOfPage(CommonLibrary.Base64Encode(pomfParampz));
                    pomfNumberofPage = pomfpz.Data;

                    string paramGetPOMF = activeUser.location + $"!_!WHERE NPNo LIKE \'%{pomfDocumentFilterValue}%\'!_!1!_!";
                    await POMFService.getPOMFDocuments(CommonLibrary.Base64Encode(paramGetPOMF));
                }
                else if (pomfDocumentFilterType.Equals("RCN"))
                {
                    string pomfParampz = $"POMFHeader!_!{activeUser.location}!_!WHERE ReceiptNo LIKE \'%{pomfDocumentFilterValue}%\'";
                    var pomfpz = await POMFService.getPOMFModuleNumberOfPage(CommonLibrary.Base64Encode(pomfParampz));
                    pomfNumberofPage = pomfpz.Data;

                    string paramGetPOMF = activeUser.location + $"!_!WHERE ReceiptNo LIKE \'%{pomfDocumentFilterValue}%\'!_!1!_!";
                    await POMFService.getPOMFDocuments(CommonLibrary.Base64Encode(paramGetPOMF));
                }
                else if (pomfDocumentFilterType.Equals("CUST"))
                {
                    string pomfParampz = $"POMFHeader!_!{activeUser.location}!_!WHERE CustomerName LIKE \'%{pomfDocumentFilterValue}%\'";
                    var pomfpz = await POMFService.getPOMFModuleNumberOfPage(CommonLibrary.Base64Encode(pomfParampz));
                    pomfNumberofPage = pomfpz.Data;

                    string paramGetPOMF = activeUser.location + $"!_!WHERE CustomerName LIKE \'%{pomfDocumentFilterValue}%\'!_!1!_!";
                    await POMFService.getPOMFDocuments(CommonLibrary.Base64Encode(paramGetPOMF));
                }
                else if (pomfDocumentFilterType.Equals("STATUS"))
                {
                    string pomfParampz = $"POMFHeader!_!{activeUser.location}!_!WHERE DocumentStatus LIKE \'%{pomfDocumentFilterValue}%\'";
                    var pomfpz = await POMFService.getPOMFModuleNumberOfPage(CommonLibrary.Base64Encode(pomfParampz));
                    pomfNumberofPage = pomfpz.Data;

                    string paramGetPOMF = activeUser.location + $"!_!WHERE DocumentStatus LIKE \'%{pomfDocumentFilterValue}%\'!_!1!_!";
                    await POMFService.getPOMFDocuments(CommonLibrary.Base64Encode(paramGetPOMF));
                }

                isLoading = false;
                StateHasChanged();
            }
            else
            {
                await _jsModule.InvokeVoidAsync("showAlert", "Please Select Filter Type !");
            }
        }

        private async void resetPOMFFilter()
        {
            isLoading = true;
            pomfPageActive = 1;

            string getPOMFDocumentParam = activeUser.location + "!_!!_!1!_!";
            await POMFService.getPOMFDocuments(CommonLibrary.Base64Encode(getPOMFDocumentParam));

            string getPOMFDocumentpz = "POMFHeader!_!" + activeUser.location + "!_!";
            var pomfpz = await POMFService.getPOMFModuleNumberOfPage(CommonLibrary.Base64Encode(getPOMFDocumentpz));
            pomfNumberofPage = pomfpz.Data;

            pomfFilterActive = false;
            isLoading = false;
            StateHasChanged();
        }

        private bool checkPOMFDocuments()
        {
            try
            {
                if (POMFService.pomfDocuments.Any())
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

        private bool checkPOMFConfirmedDocuments()
        {
            try
            {
                if (POMFService.pomfConfirmedDocuments.Any())
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

        private bool checkPreviewPOMFItemLine()
        {
            try
            {
                if (previewPOMFDocument.dataItemLines.Any())
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

        private bool checkPreviewPOMFConfirmedItemLine()
        {
            try
            {
                if (previewPOMFConfirmDocument.dataItemLines.Any())
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

        private bool checkPreviewPOMFApprovalLine()
        {
            try
            {
                if (previewPOMFDocument.dataApproval.Any())
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

        private bool checkPreviewPOMFConfirmedApprovalLine()
        {
            try
            {
                if (previewPOMFConfirmDocument.dataApproval.Any())
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
