using BPILibrary;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.MainModel.POMF;
using BPIWebApplication.Shared.PagesModel.POMF;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

namespace BPIWebApplication.Client.Pages.POMFPages
{
    public partial class AddPOMF : ComponentBase
    {
        private ActiveUser activeUser = new();
        private UserPrivileges privilegeDataParam = new();
        private List<string> userPriv = new();

        private Location paramGetCompanyLocation = new();

        private POMFHeaderForm pomfData = new();
        private List<POMFItemLineForm> pomfLines = new();

        private List<NPwithReceiptNoResp> npReceiptData = new();
        private List<NPwithReceiptNoResp> selectedNPReceiptData = new();
        private List<POMFItemLinesMaxQuantity>? maxThresholdQty = null;

        private string previousLoadedNP = string.Empty;
        private string previousLoadedReceipt = string.Empty;

        private bool itemSelected = false;

        private bool isLoading = false;
        private bool isMiniLoading = false;
        private bool isMaxQtyLoading = false;
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

            paramGetCompanyLocation.Condition = $"a.CompanyId={Convert.ToInt32(activeUser.company)}";
            paramGetCompanyLocation.PageIndex = 1;
            paramGetCompanyLocation.PageSize = 100;
            paramGetCompanyLocation.FieldOrder = "a.CompanyId";
            paramGetCompanyLocation.MethodOrder = "ASC";

            await ManagementService.GetCompanyLocations(paramGetCompanyLocation);

            await POMFService.getPOMFNPType();

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/POMFPages/AddPOMF.razor.js");
        }

        private async Task selectItembyNPandReceiptID()
        {
            try
            {
                if (previousLoadedNP == pomfData.NPNo && previousLoadedReceipt == pomfData.ReceiptNo)
                    return;

                if (!(pomfData.NPNo.Length > 0) || !(pomfData.ReceiptNo.Length > 0))
                {
                    await _jsModule.InvokeVoidAsync("showAlert", $"Input Loading Manifest ID !");
                }
                else
                {
                    isMiniLoading = true;

                    NPwithReceiptNotoTMS param = new();
                    param.npNo = pomfData.NPNo;
                    param.receiptNo = pomfData.ReceiptNo;

                    #pragma warning disable CS4014
                    Task.Run(async () =>
                    {
                        var res = await POMFService.getDetailsItemByReceiptNoAndNPNo(param, activeUser.token);

                        if (res.isSuccess)
                        {
                            npReceiptData = new();
                            npReceiptData = res.Data;

                            previousLoadedNP = pomfData.NPNo;
                            previousLoadedReceipt = pomfData.ReceiptNo;
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

        private void appendSelectedItem(NPwithReceiptNoResp data)
        {
            if (selectedNPReceiptData.FirstOrDefault(x => x.itemCode.Equals(data.itemCode)) == null)
            {
                selectedNPReceiptData.Add(new NPwithReceiptNoResp
                {
                    itemCode = data.itemCode,
                    itemDesc = data.itemDesc,
                    qtyNP = data.qtyNP,
                    uom = data.uom,
                    type = data.type
                });
            }
            else
            {
                var dt = selectedNPReceiptData.SingleOrDefault(x => x.itemCode.Equals(data.itemCode));
                if (dt != null)
                {
                    selectedNPReceiptData.Remove(dt);
                }
            }
        }

        private async void selectItembyNPandReceipt()
        {
            if (selectedNPReceiptData.Count > 0)
            {
                pomfLines.Clear();

                bool flag = false;
                int n = 0;

                var tp = selectedNPReceiptData.DistinctBy(x => x.type);

                if (POMFService.npTypes.SingleOrDefault(z => z.NPTypeID.Equals(tp.First().type.ToString())).isAutomaticApproval)
                {
                    selectedNPReceiptData.ForEach(x =>
                    {
                        n++;
                        pomfLines.Add(new POMFItemLineForm
                        {
                            POMFID = "",
                            LineNum = n,
                            ItemCode = x.itemCode,
                            ItemDescription = x.itemDesc,
                            RequestQuantity = Convert.ToInt32(x.qtyNP),
                            NPQuantity = Convert.ToInt32(x.qtyNP),
                            ItemUOM = x.uom,
                            ItemValue = "0",
                            RequestToSite = "",
                            ExternalRequestDocument = "",
                            RequestDocumentDate = DateTime.MinValue,
                            ExternalReceiveDocument = "",
                            ReceiveDocumentDate = DateTime.MinValue
                        });
                    });
                }
                else
                {
                    selectedNPReceiptData.ForEach(x =>
                    {
                        n++;
                        pomfLines.Add(new POMFItemLineForm
                        {
                            POMFID = "",
                            LineNum = n,
                            ItemCode = x.itemCode,
                            ItemDescription = x.itemDesc,
                            RequestQuantity = 0,
                            NPQuantity = Convert.ToInt32(x.qtyNP),
                            ItemUOM = x.uom,
                            ItemValue = "0",
                            RequestToSite = "",
                            ExternalRequestDocument = "",
                            RequestDocumentDate = DateTime.MinValue,
                            ExternalReceiveDocument = "",
                            ReceiveDocumentDate = DateTime.MinValue
                        });
                    });
                }

                if (tp.Count() > 1)
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "NP Type is More than 1 Type : Please Contact IT OPS !");
                    return;
                }

                pomfData.NPTypeID = tp.First().type.ToString();

                #pragma warning disable CS4014
                Task.Run(async () =>
                {
                    if (pomfData.NPNo.Length > 0)
                    {
                        isMaxQtyLoading = true;

                        var dt = await POMFService.getPOMFItemLineMaxQuantity(CommonLibrary.Base64Encode(activeUser.location + "!_!" + pomfData.NPNo));

                        if (dt.isSuccess)
                        {
                            maxThresholdQty = new();
                            maxThresholdQty = dt.Data;
                        }
                        else
                        {
                            await _jsModule.InvokeVoidAsync("showAlert", $"Failed : {dt.ErrorCode} - {dt.ErrorMessage} !");
                        }

                        isMaxQtyLoading = false;
                        StateHasChanged();
                    }
                    else
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", "Input NP No !");
                    }
                });
                #pragma warning restore CS4014

                itemSelected = true;
            }

            selectedNPReceiptData.Clear();
            StateHasChanged();
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
                                ItemValue = Convert.ToDecimal(res),
                                RequestToSite = x.RequestToSite,
                                ExternalRequestDocument = x.ExternalRequestDocument,
                                RequestDocumentDate = null,
                                ExternalReceiveDocument = x.ExternalReceiveDocument,
                                ReceiveDocumentDate = null
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

            if (!pomfLines.Any()) return false;

            if (pomfLines.Any(x => x.RequestQuantity > x.NPQuantity)) return false;

            if (pomfLines.Any(x => x.RequestQuantity <= 0)) return false;

            foreach (var f in pomfLines)
            {
                var th = maxThresholdQty.SingleOrDefault(y => y.ItemCode.Equals(f.ItemCode));

                if (th != null)
                {
                    if (f.RequestQuantity > (f.NPQuantity - th.MaxQuantity))
                        return false;
                }
            }

            var npAuto = POMFService.npTypes.SingleOrDefault(x => x.NPTypeID.Equals(pomfData.NPTypeID));

            if (npAuto != null)
            {
                if (pomfLines.Any(x => x.RequestToSite.Equals("")) && !npAuto.isAutomaticApproval) { return false; }
            }

            return true;
        }

        private void resetForm()
        {
            pomfData.CustomerName = string.Empty;
            pomfData.ReceiptNo = string.Empty;
            pomfData.NPNo = string.Empty;
            pomfData.NPTypeID = string.Empty;
            pomfLines.Clear();
            selectedNPReceiptData.Clear();
            maxThresholdQty = null;
            itemSelected = false;
            isMaxQtyLoading = false;
            isMiniLoading = false;
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

        private bool checkNPandReceiptData()
        {
            try
            {
                if (npReceiptData.Any())
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

        //
    }
}
