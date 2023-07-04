using BPILibrary;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel.EPKRS;
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

        private List<ReceiptNoResp>? receiptData = null;
        private List<ReceiptNoResp> selectedReceiptData = new();

        private List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileUpload = new();

        private string fundReturnCategorySelected { get; set; } = string.Empty;

        private string previousLoadedReceipt = string.Empty;

        private int maxFileSize = 0;

        private bool isLoading = false;
        private bool isMiniLoading = false;
        private bool successUpload = false;

        private bool alertTrigger = false;
        private bool successAlert = false;
        private string alertBody = string.Empty;
        private string alertMessage = string.Empty;

        private IJSObjectReference _jsModule;
        private IReadOnlyList<IBrowserFile>? listFileUpload;

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
            maxFileSize = await FundReturnService.getFundReturnMaxFileSize();

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
            fundReturnHeader.BankID = "BLANK";
            fundReturnHeader.ReceiptDocument = string.Empty;
            fundReturnHeader.ExternalDocument = string.Empty;
            fundReturnHeader.RefundAmount = 0.ToString();
            fundReturnHeader.TransactionAmount = 0.ToString();
            fundReturnHeader.Reason = string.Empty;

            listFileUpload = null;
            fileUpload.Clear();
        }

        //private bool validateForm()
        //{
        //    if (fundReturnHeader.CommercialType.Equals("BLANK")) return false;

        //    if (fundReturnHeader.CustomerName.IsNullOrEmpty()) return false;

        //    if (fundReturnHeader.CustomerType.Equals("BLANK"))
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        if (fundReturnHeader.CustomerType.Equals("MEMBER"))
        //            if (fundReturnHeader.CustomerMemberID.IsNullOrEmpty()) return false;
        //    }

        //    if (fundReturnHeader.BankHolderName.IsNullOrEmpty()) return false;

        //    if (fundReturnHeader.BankAccount.IsNullOrEmpty()) return false;

        //    if (fundReturnHeader.BankID.Equals("BLANK")) return false;

        //    if (fundReturnHeader.ReceiptDocument.IsNullOrEmpty()) return false;

        //    if (fundReturnHeader.ExternalDocument.IsNullOrEmpty()) return false;

        //    if (fundReturnHeader.RefundAmount.Equals("0")) return false;

        //    if (fundReturnHeader.TransactionAmount.Equals("0")) return false;

        //    if (fundReturnHeader.Reason.IsNullOrEmpty()) return false;

        //    return true;
        //}

        private void fundReturnCategoryonChange(ChangeEventArgs e)
        {
            fundReturnCategorySelected = e.Value.ToString();
            fundReturnHeader.FundReturnCategoryID = fundReturnCategorySelected;
            fundReturnLine.Clear();
            fileUpload.Clear();

            resetFundReturnForm();
            StateHasChanged();
        }

        private async Task selectItembyReceiptNo()
        {
            try
            { 
                if (previousLoadedReceipt == fundReturnHeader.ReceiptDocument)
                    return;

                if (!(fundReturnHeader.ReceiptDocument.Length > 0))
                {
                    await _jsModule.InvokeVoidAsync("showAlert", $"Input Receipt No !");
                }
                else
                {
                    isMiniLoading = true;

                    ReceiptNotoTMS param = new();
                    param.receiptNo = fundReturnHeader.ReceiptDocument;

                    #pragma warning disable CS4014
                    Task.Run(async () =>
                    {
                        var res = await FundReturnService.getFundReturnDetailsItemByReceiptNo(param, activeUser.token);

                        if (res.isSuccess)
                        {
                            receiptData = new();
                            receiptData = res.Data;

                            previousLoadedReceipt = fundReturnHeader.ReceiptDocument;
                        }
                        else
                        {
                            await _jsModule.InvokeVoidAsync("showAlert", $"Failed : {res.ErrorCode} - {res.ErrorMessage} !");
                        }

                        isMiniLoading = false;
                        StateHasChanged();
                    });
                    #pragma warning restore CS4014
                }
            }
            catch (Exception ex)
            {
                isLoading = false;
                isMiniLoading = false;
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private void appendSelectedReceiptItem(ReceiptNoResp data)
        {
            if (selectedReceiptData.FirstOrDefault(x => x.itemCode.Equals(data.itemCode)) == null)
            {
                selectedReceiptData.Add(new ReceiptNoResp
                {
                    itemCode = data.itemCode,
                    itemDesc = data.itemDesc,
                    qtyReceipt = data.qtyReceipt,
                    unitAmount = data.unitAmount,
                    unitAmountNet = data.unitAmountNet,
                    uom = data.uom
                });
            }
            else
            {
                var dt = selectedReceiptData.SingleOrDefault(x => x.itemCode.Equals(data.itemCode));
                if (dt != null)
                {
                    selectedReceiptData.Remove(dt);
                }
            }
        }

        private void selectItembyReceipt()
        {
            if (selectedReceiptData.Count > 0)
            {
                fundReturnLine.Clear();

                int n = 0;
                decimal z = decimal.Zero;
                selectedReceiptData.ForEach(x =>
                {
                    n++;
                    fundReturnLine.Add(new FundReturnItemLineForm
                    {
                        DocumentID = "",
                        LineNum = n,
                        ItemCode = x.itemCode,
                        ItemDescription = x.itemDesc,
                        ItemQuantity = Convert.ToInt32(Math.Abs(x.qtyReceipt)),
                        UOM = x.uom,
                        ItemAmount = x.unitAmountNet,
                        ItemDiscount = 0
                    });

                    z += (Convert.ToInt32(Math.Abs(x.qtyReceipt)) * x.unitAmountNet);
                });

                fundReturnHeader.RefundAmount = z.ToString("N0");
            }

            StateHasChanged();
        }

        private async void UploadHandleSelection(InputFileChangeEventArgs files)
        {
            fileUpload.Clear();

            string trustedFilename = string.Empty;

            if (maxFileSize == 0)
            {
                successAlert = false;
                alertTrigger = true;
                alertMessage = "Fail Fetch Max File Size !";
                alertBody = "Please Check Your Connection";

                await _jsModule.InvokeVoidAsync("showAlert", "File Size Parameter is Invalid !");

                isLoading = false;
            }
            else
            {
                listFileUpload = files.GetMultipleFiles();

                if (listFileUpload != null)
                {
                    foreach (var file in listFileUpload)
                    {
                        FileInfo fi = new FileInfo(file.Name);
                        string ext = fi.Extension;

                        if (file.Size > (1024 * 1024 * maxFileSize))
                        {
                            successAlert = false;
                            alertTrigger = true;
                            alertMessage = "File Selection Failed !";
                            alertBody = "File Extention / File Size is not Supported";

                            await _jsModule.InvokeVoidAsync("showAlert", "Invalid File Extensions / File Size Exceeded Limit !");

                            StateHasChanged();
                        }
                        else
                        {
                            Stream stream = file.OpenReadStream(file.Size);
                            MemoryStream ms = new MemoryStream();
                            await stream.CopyToAsync(ms);

                            fileUpload.Add(new BPIWebApplication.Shared.MainModel.Stream.FileStream
                            {
                                type = "Upload",
                                fileName = Path.GetRandomFileName() + "!_!" + fi.Name,
                                fileDesc = "Attachment",
                                fileType = ext,
                                fileSize = Convert.ToInt32(file.Size),
                                content = ms.ToArray()
                            });

                            stream.Close();
                        }
                    }
                }
            }

            this.StateHasChanged();
        }

        private async Task createFundReturn()
        {
            try
            {
                //if (!validateForm())
                //{
                //    await _jsModule.InvokeVoidAsync("showAlert", "Form Validation: Please ReCheck Your Input Field !");
                //    return;
                //}

                if (!LoginService.activeUser.userPrivileges.Contains("CR"))
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "You Have no Access to Create Document ! Please Contact IT Ops !");
                }
                else
                {
                    isLoading = true;

                    FundReturnUploadStream uploadData = new();
                    uploadData.mainData = new();
                    uploadData.mainData.Data = new();
                    uploadData.mainData.Data.dataHeader = new();
                    uploadData.mainData.Data.dataItemLines = new();
                    uploadData.mainData.Data.dataAttachmentLines = new();

                    uploadData.mainData.Data.dataHeader.DocumentID = "";
                    uploadData.mainData.Data.dataHeader.RequestDate = fundReturnHeader.RequestDate;
                    uploadData.mainData.Data.dataHeader.LocationID = fundReturnHeader.LocationID;
                    uploadData.mainData.Data.dataHeader.CommercialType = fundReturnHeader.CommercialType;
                    uploadData.mainData.Data.dataHeader.CustomerName = fundReturnHeader.CustomerName;
                    uploadData.mainData.Data.dataHeader.CustomerType = fundReturnHeader.CustomerType;
                    uploadData.mainData.Data.dataHeader.CustomerMemberID = fundReturnHeader.CustomerMemberID;
                    uploadData.mainData.Data.dataHeader.CustomerContactNo = CommonLibrary.Base64Encode(fundReturnHeader.CustomerContactNo);
                    uploadData.mainData.Data.dataHeader.FundReturnCategoryID = fundReturnHeader.FundReturnCategoryID;
                    uploadData.mainData.Data.dataHeader.BankHolderName = fundReturnHeader.BankHolderName;
                    uploadData.mainData.Data.dataHeader.BankAccount = fundReturnHeader.BankAccount;
                    uploadData.mainData.Data.dataHeader.BankID = fundReturnHeader.BankID;
                    uploadData.mainData.Data.dataHeader.ReceiptDocument = fundReturnHeader.ReceiptDocument;
                    uploadData.mainData.Data.dataHeader.ExternalDocument = fundReturnHeader.ExternalDocument;
                    uploadData.mainData.Data.dataHeader.RefundAmount = Convert.ToDecimal(fundReturnHeader.RefundAmount);
                    uploadData.mainData.Data.dataHeader.TransactionAmount = Convert.ToDecimal(fundReturnHeader.TransactionAmount);
                    uploadData.mainData.Data.dataHeader.Reason = fundReturnHeader.Reason;
                    uploadData.mainData.Data.dataHeader.DocumentStatus = "Open";

                    uploadData.mainData.userEmail = activeUser.userName;
                    uploadData.mainData.userAction = "I";
                    uploadData.mainData.userActionDate = DateTime.Now;

                    if (fundReturnCategorySelected.Equals("XNTF"))
                    {
                        int n = 0;

                        fundReturnLine.ForEach(x =>
                        {
                            n++;
                            uploadData.mainData.Data.dataItemLines.Add(new FundReturnItemLine
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

                    if (fileUpload.Count > 0)
                    {
                        int n = 0;

                        fileUpload.ForEach(x =>
                        {
                            n++;
                            uploadData.mainData.Data.dataAttachmentLines.Add(new FundReturnAttachment
                            {
                                DocumentID = "",
                                LineNum = n,
                                UploadDate = DateTime.Now,
                                FileExtension = x.fileType,
                                FilePath = x.fileName
                            });
                        });
                    }
                    else
                    {
                        throw new Exception("File Upload Empty");
                    }

                    uploadData.files = new();
                    uploadData.files = fileUpload;

                    var res = await FundReturnService.createFundReturnDocument(uploadData);

                    if (res.isSuccess)
                    {
                        successUpload = true;
                        fundReturnHeader.DocumentID = res.Data.mainData.Data.dataHeader.DocumentID;
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
            }
            catch (Exception ex)
            {
                isLoading = false;
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} {ex.InnerException}!");
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

        private bool checkReceiptData()
        {
            try
            {
                if (receiptData != null)
                {
                    if (receiptData.Any())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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

        private bool checkFileUpload()
        {
            try
            {
                if (fileUpload.Any())
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
