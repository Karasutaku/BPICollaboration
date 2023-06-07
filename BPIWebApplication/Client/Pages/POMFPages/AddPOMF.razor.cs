using BPILibrary;
using BPIWebApplication.Client.Services.EPKRSServices;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.MainModel.POMF;
using BPIWebApplication.Shared.PagesModel.POMF;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.ComponentModel;

namespace BPIWebApplication.Client.Pages.POMFPages
{
    public partial class AddPOMF : ComponentBase
    {
        private ActiveUser activeUser = new();
        private UserPrivileges privilegeDataParam = new();
        private List<string> userPriv = new();

        private POMFHeaderForm pomfData = new();
        private List<POMFItemLineForm> pomfLines = new();

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
                int moduleid = Convert.ToInt32(LoginService.moduleList.SingleOrDefault(x => x.url.Equals("/pomf/addpomf")).moduleId);
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

            pomfData.LocationID = loc;
            pomfData.NPTypeID = "";

            await POMFService.getPOMFNPType();

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/POMFPages/AddPOMF.razor.js");
        }

        private async void createPOMFDocument()
        {
            try
            {
                if (!validateForm())
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "Form Validation: Please ReCheck Your Input Field !");
                }
                else
                {
                    if (!LoginService.activeUser.userPrivileges.Contains("CR"))
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", "You Have no Access to Create Document ! Please Contact IT Ops");
                    }
                    else
                    {
                        isLoading = true;

                        QueryModel<POMFDocument> uploadData = new();
                        uploadData.Data = new();
                        uploadData.Data.dataHeader = new();
                        uploadData.Data.dataItemLines = new();

                        uploadData.Data.dataHeader.POMFID = pomfData.POMFID;
                        uploadData.Data.dataHeader.POMFDate = pomfData.POMFDate;
                        uploadData.Data.dataHeader.LocationID = pomfData.LocationID;
                        uploadData.Data.dataHeader.CustomerName = pomfData.CustomerName.ToUpper();
                        uploadData.Data.dataHeader.ReceiptNo = pomfData.ReceiptNo;
                        uploadData.Data.dataHeader.NPNo = pomfData.NPNo;
                        uploadData.Data.dataHeader.NPTypeID = pomfData.NPTypeID;
                        uploadData.Data.dataHeader.ExternalRequestDocument = "";
                        uploadData.Data.dataHeader.ExternalReceiveDocument = "";
                        uploadData.Data.dataHeader.Requester = activeUser.userName;
                
                        var doc = POMFService.npTypes.FirstOrDefault(x => x.NPTypeID.Equals(pomfData.NPTypeID));
                
                        if (doc != null)
                        {
                            if (doc.isAutomaticApproval)
                                uploadData.Data.dataHeader.DocumentStatus = "Confirmed";
                            else
                                uploadData.Data.dataHeader.DocumentStatus = "Open";
                        }
                        else
                        {
                            throw new Exception("NP TYPE NOT DEFINED, PLEASE REFRESH THE PAGE !");
                        }

                        uploadData.userEmail = activeUser.userName;
                        uploadData.userAction = "I";
                        uploadData.userActionDate = DateTime.Now;

                        int n = 0;

                        pomfLines.ForEach(x =>
                        {
                            n++;
                            string res = new string((from c in x.ItemValue where char.IsLetterOrDigit(c) select c).ToArray());

                            uploadData.Data.dataItemLines.Add(new POMFItemLine
                            {
                                POMFID = x.POMFID,
                                LineNum = n,
                                ItemCode = x.ItemCode,
                                ItemDescription = x.ItemDescription.ToUpper(),
                                RequestQuantity = x.RequestQuantity,
                                NPQuantity = x.NPQuantity,
                                ItemUOM = x.ItemUOM.ToUpper(),
                                ItemValue = Convert.ToDecimal(res)
                            });
                        });

                        var res = await POMFService.createPOMFDocument(uploadData);

                        if (res.isSuccess)
                        {
                            successUpload = true;
                            pomfData.POMFID= res.Data.Data.dataHeader.POMFID;
                            await _jsModule.InvokeVoidAsync("showAlert", "Data Creation Success !");
                        }
                        else
                        {
                            await _jsModule.InvokeVoidAsync("showAlert", $"Failed : {res.ErrorCode} - {res.ErrorMessage} !");
                        }

                        isLoading = false;
                        StateHasChanged();
                    }
                }
            }
            catch (Exception ex)
            {
                isLoading = false;
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} !");
            }
        }

        private bool validateForm()
        {
            if (pomfData.CustomerName.IsNullOrEmpty()) return false;

            if (pomfData.ReceiptNo.IsNullOrEmpty()) return false;

            if (pomfData.NPNo.IsNullOrEmpty()) return false;

            if (pomfData.NPTypeID.IsNullOrEmpty()) return false;

            return true;
        }

        private bool checkPOMFLine()
        {
            try
            {
                if (pomfLines.Any())
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

        private bool checkPOMFNPType()
        {
            try
            {
                if (POMFService.npTypes.Any())
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
