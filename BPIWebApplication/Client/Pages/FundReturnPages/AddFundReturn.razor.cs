using BPILibrary;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel.FundReturn;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.PagesModel.FundReturn;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

namespace BPIWebApplication.Client.Pages.FundReturnPages
{
    public partial class AddFundReturn : ComponentBase
    {
        private ActiveUser activeUser = new();
        private UserPrivileges privilegeDataParam = new();
        private List<string> userPriv = new();

        private FundReturnHeaderForm fundReturnHeader = new();
        private List<FundReturnItemLineForm> fundReturnLine = new();

        private string fundReturnCategorySelected { get; set; } = string.Empty;

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
                int moduleid = Convert.ToInt32(LoginService.moduleList.SingleOrDefault(x => x.url.Equals("/fundreturn/addfundreturn")).moduleId);
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

            fundReturnCategorySelected = "BLANK";
            fundReturnHeader.RequestDate = DateTime.Now;
            fundReturnHeader.LocationID = loc;

            resetFundReturnForm();

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/FundReturnPages/AddFundReturn.razor.js");
        }

        private void resetFundReturnForm()
        {
            fundReturnHeader.CommercialType = "BLANK";
            fundReturnHeader.CustomerName = string.Empty;
            fundReturnHeader.CustomerType = "BLANK";
            fundReturnHeader.CustomerMemberID = string.Empty;
            fundReturnHeader.BankHolderName = string.Empty;
            fundReturnHeader.BankAccount = string.Empty;
            fundReturnHeader.BankAccount = string.Empty;
            fundReturnHeader.BankID = "BLANK";
            fundReturnHeader.ReceiptDocument = string.Empty;
            fundReturnHeader.ExternalDocument = string.Empty;
            fundReturnHeader.RefundAmount = 0.ToString();
            fundReturnHeader.TransactionAmount = 0.ToString();
            fundReturnHeader.Reason = string.Empty;
        }

        private void fundReturnCategoryonChange(ChangeEventArgs e)
        {
            fundReturnCategorySelected = e.Value.ToString();
            fundReturnHeader.FundReturnCategoryID = fundReturnCategorySelected;

            resetFundReturnForm();
            StateHasChanged();
        }

        private async Task createFundReturn()
        {
            try
            {
                isLoading = true;

                QueryModel<FundReturnDocument> uploadData = new();
                uploadData.Data = new();
                uploadData.Data.dataHeader = new();
                uploadData.Data.dataItemLines = new();

                uploadData.Data.dataHeader.DocumentID = "";
                uploadData.Data.dataHeader.RequestDate = fundReturnHeader.RequestDate;
                uploadData.Data.dataHeader.LocationID = fundReturnHeader.LocationID;
                uploadData.Data.dataHeader.CommercialType = fundReturnHeader.CommercialType;
                uploadData.Data.dataHeader.CustomerName = fundReturnHeader.CustomerName;
                uploadData.Data.dataHeader.CustomerType = fundReturnHeader.CustomerType;
                uploadData.Data.dataHeader.CustomerMemberID = fundReturnHeader.CustomerMemberID;
                uploadData.Data.dataHeader.CustomerContactNo = fundReturnHeader.CustomerContactNo;
                uploadData.Data.dataHeader.FundReturnCategoryID = fundReturnHeader.FundReturnCategoryID;
                uploadData.Data.dataHeader.BankHolderName = fundReturnHeader.BankHolderName;
                uploadData.Data.dataHeader.BankAccount = fundReturnHeader.BankAccount;
                uploadData.Data.dataHeader.BankID = fundReturnHeader.BankID;
                uploadData.Data.dataHeader.ReceiptDocument = fundReturnHeader.ReceiptDocument;
                uploadData.Data.dataHeader.ExternalDocument = fundReturnHeader.ExternalDocument;
                uploadData.Data.dataHeader.RefundAmount = Convert.ToDecimal(fundReturnHeader.RefundAmount);
                uploadData.Data.dataHeader.TransactionAmount = Convert.ToDecimal(fundReturnHeader.TransactionAmount);
                uploadData.Data.dataHeader.Reason = fundReturnHeader.Reason;
                uploadData.Data.dataHeader.DocumentStatus = "Open";

                uploadData.userEmail = activeUser.userName;
                uploadData.userAction = "I";
                uploadData.userActionDate = DateTime.Now;

                if (fundReturnCategorySelected.Equals("XNTF"))
                {
                    int n = 0;

                    fundReturnLine.ForEach(x =>
                    {
                        n++;
                        uploadData.Data.dataItemLines.Add(new FundReturnItemLine
                        {
                            DocumentID = "",
                            LineNum = n,
                            ItemCode = x.ItemCode,
                            ItemDescription = x.ItemDescription,
                            ItemQuantity = x.ItemQuantity,
                            UOM = x.UOM,
                            ItemAmount = Convert.ToDecimal(x.ItemAmount),
                            ItemDiscount = x.ItemDiscount,
                        });
                    });
                }

                var res = await FundReturnService.createFundReturnDocument(uploadData);

                if (res.isSuccess)
                {
                    successUpload = true;
                    fundReturnHeader.DocumentID = res.Data.Data.dataHeader.DocumentID;
                    await _jsModule.InvokeVoidAsync("showAlert", "Data Creation Success !");
                }
                else
                {
                    successUpload = false;
                    await _jsModule.InvokeVoidAsync("showAlert", $"Failed : {res.ErrorCode} - {res.ErrorMessage} !");
                }

                isLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                isLoading = false;
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} !");
            }
        }

        private bool checkFundReturnCategory()
        {
            try
            {
                if (FundReturnService.fundReturnCategories.Any())
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

        private bool checkBank()
        {
            try
            {
                if (FundReturnService.banks.Any())
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

        private bool checkFundReturnLine()
        {
            try
            {
                if (fundReturnLine.Any())
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
