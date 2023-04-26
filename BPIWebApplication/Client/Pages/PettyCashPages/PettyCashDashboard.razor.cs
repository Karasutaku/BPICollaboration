﻿using BPIWebApplication.Client.Services.ManagementServices;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.MainModel.PettyCash;
using BPIWebApplication.Shared.MainModel.Procedure;
using BPIWebApplication.Shared.PagesModel.Dashboard;
using BPIWebApplication.Shared.PagesModel.PettyCash;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Office2013.Drawing.Chart;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

namespace BPIWebApplication.Client.Pages.PettyCashPages
{
    public partial class PettyCashDashboard : ComponentBase
    {
        //private ActiveUser<LoginUser> activeUser = new ActiveUser<LoginUser>();
        private ActiveUser activeUser = new();
        private UserPrivileges privilegeDataParam = new();
        private List<string> userPriv = new();

        private Advance advance = new Advance();
        private Expense expense = new Expense();
        private Reimburse reimburse = new Reimburse();

        private List<string> exp = new List<string>();
        private List<ReimburseLine> selectedReimburseLines = new();
        private List<string> coaSummary = new();
        List<ReimbursementMultiSelectStatusUpdate> selectedReimburse = new();
        List<ResultModel<ReimbursementMultiSelectStatusUpdate>> finishedDocument = new();

        private OutstandingBalance outstandingBalance = new();
        private BalanceDetails locBalanceDetails = new();
        private DateTime locationCutoffDate = DateTime.MinValue;

        List<ledgerParam> ledgerParam = new();
        private Location location = new();

        private decimal editedBudgetAmount = decimal.Zero;
        private DateTime editedCutoffDate = DateTime.MinValue;

        private bool isLoading = false;
        private bool showModal = false;
        private bool showBalanceModal = false;
        private bool showUpdateBudgetModal = false;
        private bool showUpdateCutoffModal = false;
        private bool showExportModal = false;
        private string transacType = string.Empty;
        private bool reimburseCheckAllisChecked = false;
        private bool isAdvanceActive = false;
        private bool isExpenseActive = false;
        private bool isReimburseActive = false;
        private bool isFetchBalanceActive = false;
        private bool showMultiSelectApprovalModal = false;

        // ongoing
        private int advancePageActive = 0;
        private int advanceNumberofPage = 0;
        private int expensePageActive = 0;
        private int expenseNumberofPage = 0;
        private int reimbursePageActive = 0;
        private int reimburseNumberofPage = 0;

        // posted
        private int padvancePageActive = 0;
        private int padvanceNumberofPage = 0;
        private int pexpensePageActive = 0;
        private int pexpenseNumberofPage = 0;
        private int preimbursePageActive = 0;
        private int preimburseNumberofPage = 0;

        // general filter
        private string storeFilter { get; set; } = string.Empty;

        // filter ongoing
        private bool isAdvanceFilterActive = false;
        private bool isExpenseFilterActive = false;
        private bool isReimburseFilterActive = false;
        private string advFilterType { get; set; } = string.Empty;
        private string advFilterValue { get; set; } = string.Empty;
        private string expFilterType { get; set; } = string.Empty;
        private string expFilterValue { get; set; } = string.Empty;
        private string remFilterType { get; set; } = string.Empty;
        private string remFilterValue { get; set; } = string.Empty;

        // filter posted
        private bool ispAdvanceFilterActive = false;
        private bool ispExpenseFilterActive = false;
        private bool ispReimburseFilterActive = false;
        private string padvFilterType { get; set; } = string.Empty;
        private string padvFilterValue { get; set; } = string.Empty;
        private string pexpFilterType { get; set; } = string.Empty;
        private string pexpFilterValue { get; set; } = string.Empty;
        private string premFilterType { get; set; } = string.Empty;
        private string premFilterValue { get; set; } = string.Empty;

        private IJSObjectReference _jsModule;

        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        protected override async Task OnInitializedAsync()
        {
            if (!LoginService.activeUser.userPrivileges.IsNullOrEmpty())
                LoginService.activeUser.userPrivileges.Clear();

            if (syncSessionStorage.ContainKey("PagePrivileges"))
                syncSessionStorage.RemoveItem("PagePrivileges");

            string tkn = syncSessionStorage.GetItem<string>("token");

            if (syncSessionStorage.ContainKey("userName"))
            {
                privilegeDataParam.moduleId = Convert.ToInt32(Base64Decode(syncSessionStorage.GetItem<string>("ModuleId")));
                privilegeDataParam.UserName = Base64Decode(syncSessionStorage.GetItem<string>("userName"));
                privilegeDataParam.userLocationParam = new();
                privilegeDataParam.userLocationParam.SessionId = syncSessionStorage.GetItem<string>("SessionId");
                privilegeDataParam.userLocationParam.MacAddress = "";
                privilegeDataParam.userLocationParam.IpClient = "";
                privilegeDataParam.userLocationParam.ApplicationId = Convert.ToInt32(Base64Decode(syncSessionStorage.GetItem<string>("AppV")));
                privilegeDataParam.userLocationParam.LocationId = Base64Decode(syncSessionStorage.GetItem<string>("CompLoc")).Split("_")[1];
                privilegeDataParam.userLocationParam.Name = Base64Decode(syncSessionStorage.GetItem<string>("userName"));
                privilegeDataParam.userLocationParam.CompanyId = Convert.ToInt32(Base64Decode(syncSessionStorage.GetItem<string>("CompLoc")).Split("_")[0]);
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
            activeUser.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            activeUser.company = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[0];
            activeUser.location = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];
            activeUser.sessionId = await sessionStorage.GetItemAsync<string>("SessionId");
            activeUser.appV = Convert.ToInt32(Base64Decode(await sessionStorage.GetItemAsync<string>("AppV")));
            activeUser.userPrivileges = new();
            activeUser.userPrivileges = await sessionStorage.GetItemAsync<List<string>>("PagePrivileges");

            LoginService.activeUser = activeUser;

            isAdvanceFilterActive = false;
            isExpenseFilterActive = false;
            isReimburseFilterActive = false;
            ispAdvanceFilterActive = false;
            ispExpenseFilterActive = false;
            ispReimburseFilterActive = false;

            string advStatus = "";
            string advFilType = "";
            string advFilValue = "";

            advancePageActive = 1;
            string advpz = "Advance!_!AdvanceID!_!" + advStatus + "!_!" + advFilType + "!_!" + advFilValue + "!_!" + activeUser.location;
            advanceNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(advpz));
            padvancePageActive = 1;
            string padvpz = "PostedAdvance!_!AdvanceID!_!" + advStatus + "!_!" + advFilType + "!_!" + advFilValue + "!_!" + activeUser.location;
            padvanceNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(padvpz));

            string expStatus = "";
            string expFilType = "";
            string expFilValue = "";

            expensePageActive = 1;
            string exppz = "Expense!_!ExpenseID!_!" + expStatus + "!_!" + expFilType + "!_!" + expFilValue + "!_!" + activeUser.location;
            expenseNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(exppz));
            pexpensePageActive = 1;
            string pexppz = "PostedExpense!_!ExpenseID!_!" + expStatus + "!_!" + expFilType + "!_!" + expFilValue + "!_!" + activeUser.location;
            pexpenseNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(pexppz));

            string remStatus = "";
            string remFilType = "";
            string remFilValue = "";

            reimbursePageActive = 1;
            string rbspz = "Reimburse!_!ReimburseID!_!" + remStatus + "!_!" + remFilType + "!_!" + remFilValue + "!_!" + activeUser.location;
            reimburseNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(rbspz));
            preimbursePageActive = 1;
            string prbspz = "PostedReimburse!_!ReimburseID!_!" + remStatus + "!_!" + remFilType + "!_!" + remFilValue + "!_!" + activeUser.location;
            preimburseNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(prbspz));

            string advlocPage = "MASTER!_!" + activeUser.location + "!_!" + advStatus + "!_!" + advFilType + "!_!" + advFilValue + "!_!" + advancePageActive.ToString();
            await PettyCashService.getAdvanceDatabyLocation(Base64Encode(advlocPage));

            string explocPage = "MASTER!_!" + activeUser.location + "!_!" + expStatus + "!_!" + expFilType + "!_!" + expFilValue + "!_!" + expensePageActive.ToString();
            await PettyCashService.getExpenseDatabyLocation(Base64Encode(explocPage));

            string rbslocPage = "MASTER!_!" + activeUser.location + "!_!" + remStatus + "!_!" + remFilType + "!_!" + remFilValue + "!_!" + reimbursePageActive.ToString();
            await PettyCashService.getReimburseDatabyLocation(Base64Encode(rbslocPage));

            string padvlocPage = "POSTED!_!" + activeUser.location + "!_!" + advStatus + "!_!" + advFilType + "!_!" + advFilValue + "!_!" + padvancePageActive.ToString();
            await PettyCashService.getAdvanceDatabyLocation(Base64Encode(padvlocPage));

            string pexplocPage = "POSTED!_!" + activeUser.location + "!_!" + expStatus + "!_!" + expFilType + "!_!" + expFilValue + "!_!" + pexpensePageActive.ToString();
            await PettyCashService.getExpenseDatabyLocation(Base64Encode(pexplocPage));

            string prbslocPage = "POSTED!_!" + activeUser.location + "!_!" + remStatus + "!_!" + remFilType + "!_!" + remFilValue + "!_!" + preimbursePageActive.ToString();
            await PettyCashService.getReimburseDatabyLocation(Base64Encode(prbslocPage));

            string loc = activeUser.location.Equals("") ? "HO" : activeUser.location;
            await ManagementService.GetAllDepartment(Base64Encode(loc));

            location.Condition = $"a.CompanyId={Convert.ToInt32(activeUser.company)}";
            location.PageIndex = 1;
            location.PageSize = 100;
            location.FieldOrder = "a.CompanyId";
            location.MethodOrder = "ASC";

            await ManagementService.GetCompanyLocations(location);
            await PettyCashService.getCoabyModule("PettyCash");

            //activeUser.Name = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            //activeUser.UserLogin = new LoginUser();
            //activeUser.UserLogin.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userEmail"));
            //activeUser.role = Base64Decode(await sessionStorage.GetItemAsync<string>("role"));

            //PettyCashService.expensesTestData();
            //PettyCashService.advanceTestData();
            //PettyCashService.reimburseTestData();
            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/PettyCashPages/PettyCashDashboard.razor.js");

            StateHasChanged();
        }

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        activeUser.token = await sessionStorage.GetItemAsync<string>("token");
        //        activeUser.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
        //        activeUser.company = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[0];
        //        activeUser.location = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];
        //        activeUser.sessionId = await sessionStorage.GetItemAsync<string>("SessionId");
        //        activeUser.appV = Convert.ToInt32(Base64Decode(await sessionStorage.GetItemAsync<string>("AppV")));
        //        activeUser.userPrivileges = new();
        //        activeUser.userPrivileges = await sessionStorage.GetItemAsync<List<string>>("PagePrivileges");

        //        LoginService.activeUser.userPrivileges = activeUser.userPrivileges;
        //    }
        //}

        private void hideDataModalonESC(KeyboardEventArgs e) { try { if (e.Key.Equals("Escape")) { showModal = false; } } catch (Exception exc) { } }
        private void hideBalanceModalonESC(KeyboardEventArgs e) { try { if (e.Key.Equals("Escape")) { showBalanceModal = false; } } catch (Exception exc) { } }
        private void hideUpdateBudgetModalonESC(KeyboardEventArgs e) { try { if (e.Key.Equals("Escape")) { showUpdateBudgetModal = false; } } catch (Exception exc) { } }
        private void hideCutoffModalonESC(KeyboardEventArgs e) { try { if (e.Key.Equals("Escape")) { showUpdateCutoffModal = false; } } catch (Exception exc) { } }
        private void hideExportModalonESC(KeyboardEventArgs e) { try { if (e.Key.Equals("Escape")) { showExportModal = false; } } catch (Exception exc) { } }
        private void hideReportProcessModalonESC(KeyboardEventArgs e) { try { if (e.Key.Equals("Escape")) { showMultiSelectApprovalModal = false; } } catch (Exception exc) { } }

        private bool checkUserPrivilegeViewable()
        {
            try
            {
                if (LoginService.activeUser.userPrivileges.Contains("VW"))
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

        private Stream GetFileStream(byte[] data)
        {
            var fileBinData = data;
            var fileStream = new MemoryStream(fileBinData);

            return fileStream;
        }

        private async Task HandleViewDocument(Byte[] content)
        {
            var filestream = GetFileStream(content);

            using var streamRef = new DotNetStreamReference(stream: filestream);

            await _jsModule.InvokeVoidAsync("downloadFileFromStream", "Attachment", streamRef);
        }

        private async Task HandleDownloadDocument(Byte[] content, string filename)
        {
            var filestream = GetFileStream(content);

            using var streamRef = new DotNetStreamReference(stream: filestream);

            await _jsModule.InvokeVoidAsync("exportStream", filename, streamRef);
        }

        private bool isVisible(LocationResp locData)
        {
            if (string.IsNullOrEmpty(storeFilter))
                return true;

            if (locData.locationId.Contains(storeFilter, StringComparison.OrdinalIgnoreCase) || locData.locationName.Contains(storeFilter, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        private void hideBalanceDetails() => showBalanceModal = false;
        private void hideBudgetDetails() => showUpdateBudgetModal = false;
        private void hideCutoffDetails() => showUpdateCutoffModal = false;

        private void hideExportModal()
        {
            showExportModal = false;
            ledgerParam.Clear();
        }

        private void triggerBalanceUpdateModal(string loc)
        {
            showUpdateBudgetModal = true;
            locBalanceDetails.LocationID = loc;
            editedBudgetAmount = decimal.Zero;
        }

        private void triggerCutoffUpdateModal(string loc)
        {
            showUpdateCutoffModal = true;
            locBalanceDetails.LocationID = loc;
            editedCutoffDate = DateTime.Now;
        }

        private void triggerExportModal(string loc)
        {
            string tempLoc = loc;

            showExportModal = true;
        }

        private void selectExportLocation(string loc)
        {
            if (ledgerParam.FirstOrDefault(a => a.locationID.Contains(loc)) == null)
            {
                ledgerParam.Add(new ledgerParam
                {
                    locationID = loc,
                    startDate = DateTime.Now,
                    endDate = DateTime.Now
                });
            }
            else
            {
                var itemRemove1 = ledgerParam.SingleOrDefault(a => a.locationID.Contains(loc));
                if (itemRemove1 != null)
                {
                    ledgerParam.Remove(itemRemove1);
                }
            }
        }

        private async Task exportLedgerData()
        {
            try
            {
                isLoading = true;

                if (ledgerParam.Any())
                {
                    var res = await PettyCashService.getLedgerDataEntriesbyDate(ledgerParam);

                    if (res.isSuccess)
                    {
                        await HandleDownloadDocument(res.Data.content, res.Data.fileName);

                        isLoading = false;
                        await _jsModule.InvokeVoidAsync("showAlert", "Export Success !");
                    }
                }
                else
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "Select at Least 1 Location !");

                    ledgerParam.Clear();
                    isLoading = false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task showBalanceDetails(string loc)
        {

            try
            {
                isFetchBalanceActive = true;

                string temp = loc.Equals("") ? "HO" : loc;

                var res = await PettyCashService.getPettyCashOutstandingAmount(temp);

                if (res.isSuccess)
                {
                    showBalanceModal = true;
                    outstandingBalance = res.Data.outstandingBalance;
                    locBalanceDetails = res.Data.balanceDetails;
                    locationCutoffDate = res.Data.CutOffDate;

                    isFetchBalanceActive = false;
                }
                else
                {
                    isFetchBalanceActive = false;
                    await _jsModule.InvokeVoidAsync("showAlert", "Fetch Balance Data Failed, Please Check Your Connection !");
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                isFetchBalanceActive = false;
                throw new Exception(ex.Message);
            }
            
        }

        private async void updateBudget(string loc)
        {
            try
            {
                isLoading = true;

                QueryModel<BalanceDetails> updateData = new();
                updateData.Data = new();

                updateData.Data.LocationID = loc;
                updateData.Data.BudgetAmount = editedBudgetAmount;
                updateData.Data.LatestAuditUser = activeUser.userName;
                updateData.Data.AuditDate = DateTime.Now;
                updateData.userEmail = activeUser.userName;
                updateData.userAction = "U";
                updateData.userActionDate = DateTime.Now;

                var res = await PettyCashService.updateLocationBudgetDetails(updateData);

                if (res.isSuccess)
                {
                    isLoading = false;

                    locBalanceDetails.BudgetAmount = res.Data.Data.BudgetAmount;
                    locBalanceDetails.LatestAuditUser = res.Data.Data.LatestAuditUser;
                    locBalanceDetails.AuditDate = res.Data.Data.AuditDate;

                    await _jsModule.InvokeVoidAsync("showAlert", "Update Success, Please Refresh Your Page !");
                }
                else
                {
                    isLoading = false;
                    await _jsModule.InvokeVoidAsync("showAlert", "Update Failed, Please Check Your Connection !");
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                isLoading = false;

                await _jsModule.InvokeVoidAsync("showAlert", $"Error, {ex.Message} !");
                throw new Exception(ex.Message);
            }
        }

        private async void updateCutoffDate(string loc)
        {
            isLoading = true;

            try
            {
                QueryModel<CutoffDetails> updateData = new();
                updateData.Data = new();

                updateData.Data.LocationID = loc;
                updateData.Data.ModuleLedgerName = "PettyCashLedger";
                updateData.Data.CutoffDate = editedCutoffDate;
                updateData.userEmail = activeUser.userName;
                updateData.userAction = "U";
                updateData.userActionDate = DateTime.Now;

                var res = await PettyCashService.updateLocationCutoffDate(updateData);

                if (res.isSuccess)
                {
                    isLoading = false;

                    locationCutoffDate = res.Data.Data.CutoffDate;

                    await _jsModule.InvokeVoidAsync("showAlert", "Update Success, Please Refresh Your Page !");
                }
                else
                {
                    isLoading = false;
                    await _jsModule.InvokeVoidAsync("showAlert", "Update Failed, Please Check Your Connection !");
                }
            }
            catch (Exception ex)
            {
                isLoading = false;
                await _jsModule.InvokeVoidAsync("showAlert", $"Error, {ex.Message} !");
                throw new Exception(ex.Message);
            }
        }

        private async void updateDocumentStatus(string action)
        {
            try
            {
                if (isAdvanceActive)
                {
                    isLoading = true;

                    string param = "Advance!_!" + advance.AdvanceID + "!_!" + action + "!_!";

                    QueryModel<string> statData = new();

                    statData.Data = param;
                    statData.userEmail = activeUser.userName;
                    statData.userAction = "U";
                    statData.userActionDate = DateTime.Now;

                    var res = await PettyCashService.updateDocumentStatus(statData);

                    if (res.isSuccess)
                    {

                        if (action.Contains("Rejected"))
                        {
                            if (advance.AdvanceStatus.Equals("Open"))
                            {
                                string temp = "PettyCash!_!StatusReject!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + advance.AdvanceID + "!_!" + advance.Applicant;
                                var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                                if (res3.isSuccess)
                                {
                                    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Reject Status Success Updated, Please Reload Your Page !");
                                }
                                else
                                {
                                    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Reject Status Success Updated, Please Reload Your Page !");
                                }
                            }
                            else if (advance.AdvanceStatus.Equals("Confirmed"))
                            {
                                string temp = "PettyCash!_!DirectEmail!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + advance.AdvanceID + "!_!" + advance.statusDetails.confirmUser;
                                var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                                if (res3.isSuccess)
                                {
                                    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Approval Status Success Updated, Please Reload Your Page !");
                                }
                                else
                                {
                                    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Approval Status Success Updated, Please Reload Your Page !");
                                }
                            }

                            advance.statusDetails.rejectUser = activeUser.userName;
                            PettyCashService.advances.SingleOrDefault(a => a.AdvanceID.Equals(advance.AdvanceID)).statusDetails.rejectUser = activeUser.userName;
                            advance.statusDetails.rejectDate = DateTime.Now;
                            PettyCashService.advances.SingleOrDefault(a => a.AdvanceID.Equals(advance.AdvanceID)).statusDetails.rejectDate = DateTime.Now;

                            //string temp = "PettyCash!_!StatusReject!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + advance.AdvanceID + "!_!" + advance.Applicant;
                            //var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                            //if (res3.isSuccess)
                            //{
                            //    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Reject Status Success Updated, Please Reload Your Page !");
                            //}
                            //else
                            //{
                            //    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Reject Status Success Updated, Please Reload Your Page !");
                            //}
                        }
                        else if (action.Contains("Submited"))
                        {
                            string temp = "PettyCash!_!DirectEmail!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + advance.AdvanceID + "!_!" + advance.Applicant.ToLower();
                            //string temp = "PettyCash!_!StatusSubmit!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + advance.AdvanceID;
                            var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                            if (res3.isSuccess)
                            {
                                await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Approval Status Success Updated, Please Reload Your Page !");
                            }
                            else
                            {
                                await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Approval Status Success Updated, Please Reload Your Page !");
                            }

                            advance.statusDetails.submitUser = activeUser.userName;
                            PettyCashService.advances.SingleOrDefault(a => a.AdvanceID.Equals(advance.AdvanceID)).statusDetails.submitUser = activeUser.userName;
                            advance.statusDetails.submitDate = DateTime.Now;
                            PettyCashService.advances.SingleOrDefault(a => a.AdvanceID.Equals(advance.AdvanceID)).statusDetails.submitDate = DateTime.Now;

                        }
                        else if (action.Contains("Confirmed"))
                        {
                            //if (activeUser.location.IsNullOrEmpty())
                            //{
                            //    string temp = "PettyCash!_!DirectEmail!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + advance.AdvanceID + "!_!" + advance.Approver.ToLower();
                            //    var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                            //    if (res3.isSuccess)
                            //    {
                            //        await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Approval Status Success Updated, Please Reload Your Page !");
                            //    }
                            //    else
                            //    {
                            //        await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Approval Status Success Updated, Please Reload Your Page !");
                            //    }
                            //}
                            //else
                            //{
                            //    string temp = "PettyCash!_!StatusConfirm!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + advance.AdvanceID;
                            //    var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                            //    if (res3.isSuccess)
                            //    {
                            //        await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Approval Status Success Updated, Please Reload Your Page !");
                            //    }
                            //    else
                            //    {
                            //        await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Approval Status Success Updated, Please Reload Your Page !");
                            //    }
                            //}

                            string temp = "PettyCash!_!StatusConfirm!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + advance.AdvanceID;
                            var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                            if (res3.isSuccess)
                            {
                                await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Approval Status Success Updated, Please Reload Your Page !");
                            }
                            else
                            {
                                await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Approval Status Success Updated, Please Reload Your Page !");
                            }

                            advance.statusDetails.confirmUser = activeUser.userName;
                            PettyCashService.advances.SingleOrDefault(a => a.AdvanceID.Equals(advance.AdvanceID)).statusDetails.confirmUser = activeUser.userName;
                            advance.statusDetails.confirmDate = DateTime.Now;
                            PettyCashService.advances.SingleOrDefault(a => a.AdvanceID.Equals(advance.AdvanceID)).statusDetails.confirmDate = DateTime.Now;
                        }

                        advance.AdvanceStatus = action;
                        PettyCashService.advances.SingleOrDefault(a => a.AdvanceID.Equals(advance.AdvanceID)).AdvanceStatus = action;

                        isLoading = false;
                        //await _jsModule.InvokeVoidAsync("showAlert", "");
                    }
                    else
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", "Status Might've Updated from Another User, Please Refresh Your Page !");
                    }
                }
                else if (isExpenseActive)
                {
                    isLoading = true;

                    string param = "Expense!_!" + expense.ExpenseID + "!_!" + action + "!_!!_!" + expense.ExpenseStatus;

                    QueryModel<string> statData = new();

                    statData.Data = param;
                    statData.userEmail = activeUser.userName;
                    statData.userAction = "U";
                    statData.userActionDate = DateTime.Now;

                    var res = await PettyCashService.updateDocumentStatus(statData);

                    if (res.isSuccess)
                    {

                        if (action.Contains("Rejected"))
                        {
                            if (expense.ExpenseStatus.Equals("Open"))
                            {
                                string temp = "PettyCash!_!StatusReject!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + expense.ExpenseID + "!_!" + expense.Applicant;
                                var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                                if (res3.isSuccess)
                                {
                                    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Reject Status Success Updated, Please Reload Your Page !");
                                }
                                else
                                {
                                    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Reject Status Success Updated, Please Reload Your Page !");
                                }
                            }
                            else if (expense.ExpenseStatus.Equals("Confirmed"))
                            {
                                string temp = "PettyCash!_!DirectEmail!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + expense.ExpenseID + "!_!" + expense.statusDetails.confirmUser;
                                var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                                if (res3.isSuccess)
                                {
                                    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Approval Status Success Updated, Please Reload Your Page !");
                                }
                                else
                                {
                                    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Approval Status Success Updated, Please Reload Your Page !");
                                }
                            }

                            expense.statusDetails.rejectUser = activeUser.userName;
                            PettyCashService.expenses.SingleOrDefault(a => a.ExpenseID.Equals(expense.ExpenseID)).statusDetails.rejectUser = activeUser.userName;
                            expense.statusDetails.rejectDate = DateTime.Now;
                            PettyCashService.expenses.SingleOrDefault(a => a.ExpenseID.Equals(expense.ExpenseID)).statusDetails.rejectDate = DateTime.Now;

                        }
                        else if (action.Contains("Submited"))
                        {
                            string temp = "PettyCash!_!DirectEmail!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + expense.ExpenseID + "!_!" + expense.Applicant.ToLower();
                            //string temp = "PettyCash!_!StatusSubmit!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + expense.ExpenseID;
                            var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                            if (res3.isSuccess)
                            {
                                await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Approval Status Success Updated, Please Reload Your Page !");
                            }
                            else
                            {
                                await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Approval Status Success Updated, Please Reload Your Page !");
                            }

                            expense.statusDetails.submitUser = activeUser.userName;
                            PettyCashService.expenses.SingleOrDefault(a => a.ExpenseID.Equals(expense.ExpenseID)).statusDetails.submitUser = activeUser.userName;
                            expense.statusDetails.submitDate = DateTime.Now;
                            PettyCashService.expenses.SingleOrDefault(a => a.ExpenseID.Equals(expense.ExpenseID)).statusDetails.submitDate = DateTime.Now;

                        }
                        else if (action.Contains("Confirmed"))
                        {
                            //if (activeUser.location.IsNullOrEmpty())
                            //{
                            //    string temp = "PettyCash!_!DirectEmail!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + expense.ExpenseID + "!_!" + expense.Approver.ToLower();
                            //    var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                            //    if (res3.isSuccess)
                            //    {
                            //        await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Approval Status Success Updated, Please Reload Your Page !");
                            //    }
                            //    else
                            //    {
                            //        await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Approval Status Success Updated, Please Reload Your Page !");
                            //    }
                            //}
                            //else
                            //{
                            //    string temp = "PettyCash!_!StatusConfirm!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + expense.ExpenseID;
                            //    var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                            //    if (res3.isSuccess)
                            //    {
                            //        await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Approval Status Success Updated, Please Reload Your Page !");
                            //    }
                            //    else
                            //    {
                            //        await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Approval Status Success Updated, Please Reload Your Page !");
                            //    }
                            //}

                            string temp = "PettyCash!_!StatusConfirm!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + expense.ExpenseID;
                            var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                            if (res3.isSuccess)
                            {
                                await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Approval Status Success Updated, Please Reload Your Page !");
                            }
                            else
                            {
                                await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Approval Status Success Updated, Please Reload Your Page !");
                            }

                            expense.statusDetails.confirmUser = activeUser.userName;
                            PettyCashService.expenses.SingleOrDefault(a => a.ExpenseID.Equals(expense.ExpenseID)).statusDetails.confirmUser = activeUser.userName;
                            expense.statusDetails.confirmDate = DateTime.Now;
                            PettyCashService.expenses.SingleOrDefault(a => a.ExpenseID.Equals(expense.ExpenseID)).statusDetails.confirmDate = DateTime.Now;

                        }

                        expense.ExpenseStatus = action;
                        PettyCashService.expenses.SingleOrDefault(a => a.ExpenseID.Equals(expense.ExpenseID)).ExpenseStatus = action;

                        isLoading = false;
                        //await _jsModule.InvokeVoidAsync("showAlert", "Approval Status Success Updated, Please Reload Your Page !");
                    }
                    else
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", "Status Might've Updated from Another User, Please Refresh Your Page !");
                    }
                }
                else if (isReimburseActive)
                {
                    isLoading = true;

                    string param = "Reimburse!_!" + reimburse.ReimburseID + "!_!" + action + "!_!" + reimburse.ReimburseNote;

                    QueryModel<string> statData = new();

                    statData.Data = param;
                    statData.userEmail = activeUser.userName;
                    statData.userAction = "U";
                    statData.userActionDate = DateTime.Now;

                    if (action.Contains("Verified"))
                    {
                        if (!reimburse.lines.Any(x => x.Status.Contains("OP")))
                        {
                            if (Decimal.Subtract(reimburse.lines.Sum(x => x.Amount), reimburse.lines.Sum(x => x.ApprovedAmount)) > 0 && reimburse.ReimburseNote.IsNullOrEmpty())
                            {
                                await _jsModule.InvokeVoidAsync("showAlert", "Fill Blank Note !");
                            }
                            else
                            {
                                var res = await PettyCashService.updateDocumentStatus(statData);

                                if (res.isSuccess)
                                {
                                    // update data to db

                                    try
                                    {
                                        QueryModel<Reimburse> updateData = new();
                                        updateData.Data = new();

                                        updateData.Data = reimburse;
                                        updateData.userEmail = activeUser.userName;
                                        updateData.userAction = "U";
                                        updateData.userActionDate = DateTime.Now;

                                        var res2 = await PettyCashService.updateReimburseLineData(updateData);

                                        if (res.isSuccess)
                                        {
                                            string temp = "PettyCash!_!StatusVerify!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + reimburse.ReimburseID;
                                            var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                                            if (res3.isSuccess)
                                            {
                                                await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Approval Status Success Updated, Please Reload Your Page !");
                                            }
                                            else
                                            {
                                                await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Approval Status Success Updated, Please Reload Your Page !");
                                            }

                                            reimburse.ReimburseStatus = action;
                                            PettyCashService.reimburses.SingleOrDefault(a => a.ReimburseID.Equals(reimburse.ReimburseID)).ReimburseStatus = action;
                                            reimburse.statusDetails.verifyUser = activeUser.userName;
                                            PettyCashService.reimburses.SingleOrDefault(a => a.ReimburseID.Equals(reimburse.ReimburseID)).statusDetails.verifyUser = activeUser.userName;
                                            reimburse.statusDetails.verifyDate = DateTime.Now;
                                            PettyCashService.reimburses.SingleOrDefault(a => a.ReimburseID.Equals(reimburse.ReimburseID)).statusDetails.verifyDate = DateTime.Now;
                                            //await _jsModule.InvokeVoidAsync("showAlert", "Approval Status Success Updated, Please Reload Your Page !");
                                        }

                                        isLoading = false;
                                    }
                                    catch (Exception ex)
                                    {
                                        throw new Exception(ex.Message);
                                    }
                                }
                                else
                                {
                                    await _jsModule.InvokeVoidAsync("showAlert", "Status Might've Updated from Another User, Please Refresh Your Page !");
                                }
                            }
                        }
                        else
                        {
                            await _jsModule.InvokeVoidAsync("showAlert", "Please Process all Lines !");
                        }

                        isLoading = false;
                    }
                    else
                    {
                        isLoading = true;

                        var res = await PettyCashService.updateDocumentStatus(statData);

                        if (res.isSuccess)
                        {
                            // update
                            if (action.Contains("Rejected"))
                            {
                                if (reimburse.ReimburseStatus.Equals("Open"))
                                {
                                    string temp = "PettyCash!_!StatusReject!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + reimburse.ReimburseID + "!_!" + reimburse.Applicant;
                                    var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                                    if (res3.isSuccess)
                                    {
                                        await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Reject Status Success Updated, Please Reload Your Page !");
                                    }
                                    else
                                    {
                                        await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Reject Status Success Updated, Please Reload Your Page !");
                                    }
                                }
                                else if (reimburse.ReimburseStatus.Equals("Confirmed"))
                                {
                                    string temp = "PettyCash!_!DirectEmail!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + reimburse.ReimburseID + "!_!" + reimburse.statusDetails.confirmUser;
                                    var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                                    if (res3.isSuccess)
                                    {
                                        await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Approval Status Success Updated, Please Reload Your Page !");
                                    }
                                    else
                                    {
                                        await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Approval Status Success Updated, Please Reload Your Page !");
                                    }
                                }
                                else if (reimburse.ReimburseStatus.Equals("Verified"))
                                {
                                    string temp = "PettyCash!_!DirectEmail!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + reimburse.ReimburseID + "!_!" + reimburse.statusDetails.verifyUser;
                                    var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                                    if (res3.isSuccess)
                                    {
                                        await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Approval Status Success Updated, Please Reload Your Page !");
                                    }
                                    else
                                    {
                                        await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Approval Status Success Updated, Please Reload Your Page !");
                                    }
                                }
                                else if (reimburse.ReimburseStatus.Equals("Approved"))
                                {
                                    string temp = "PettyCash!_!StatusApprove!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + reimburse.ReimburseID;
                                    //string temp = "PettyCash!_!DirectEmail!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + reimburse.ReimburseID + "!_!" + reimburse.statusDetails.approveUser;
                                    var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                                    if (res3.isSuccess)
                                    {
                                        await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Approval Status Success Updated, Please Reload Your Page !");
                                    }
                                    else
                                    {
                                        await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Approval Status Success Updated, Please Reload Your Page !");
                                    }
                                }

                                reimburse.statusDetails.rejectUser = activeUser.userName;
                                PettyCashService.reimburses.SingleOrDefault(a => a.ReimburseID.Equals(reimburse.ReimburseID)).statusDetails.rejectUser = activeUser.userName;
                                reimburse.statusDetails.rejectDate = DateTime.Now;
                                PettyCashService.reimburses.SingleOrDefault(a => a.ReimburseID.Equals(reimburse.ReimburseID)).statusDetails.rejectDate = DateTime.Now;

                            }
                            else if (action.Contains("Confirmed"))
                            {
                                string temp = "PettyCash!_!StatusRConfirm!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + reimburse.ReimburseID;
                                var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                                if (res3.isSuccess)
                                {
                                    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Approval Status Success Updated, Please Reload Your Page !");
                                }
                                else
                                {
                                    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Approval Status Success Updated, Please Reload Your Page !");
                                }

                                reimburse.statusDetails.confirmUser = activeUser.userName;
                                PettyCashService.reimburses.SingleOrDefault(a => a.ReimburseID.Equals(reimburse.ReimburseID)).statusDetails.confirmUser = activeUser.userName;
                                reimburse.statusDetails.confirmDate = DateTime.Now;
                                PettyCashService.reimburses.SingleOrDefault(a => a.ReimburseID.Equals(reimburse.ReimburseID)).statusDetails.confirmDate = DateTime.Now;

                            }
                            else if (action.Contains("Released"))
                            {
                                string temp = "PettyCash!_!DirectEmail!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + reimburse.ReimburseID + "!_!" + reimburse.Applicant;
                                //string temp = "PettyCash!_!StatusRelease!_!" + reimburse.LocationID + "!_!" + activeUser.userName + "!_!" + reimburse.ReimburseID;
                                var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                                if (res3.isSuccess)
                                {
                                    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Approval Status Success Updated, Please Reload Your Page !");
                                }
                                else
                                {
                                    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Approval Status Success Updated, Please Reload Your Page !");
                                }

                                reimburse.statusDetails.releaseUser = activeUser.userName;
                                PettyCashService.reimburses.SingleOrDefault(a => a.ReimburseID.Equals(reimburse.ReimburseID)).statusDetails.releaseUser = activeUser.userName;
                                reimburse.statusDetails.releaseDate = DateTime.Now;
                                PettyCashService.reimburses.SingleOrDefault(a => a.ReimburseID.Equals(reimburse.ReimburseID)).statusDetails.releaseDate = DateTime.Now;

                            }
                            else if (action.Contains("Approved"))
                            {
                                string temp = "PettyCash!_!StatusApprove!_!" + activeUser.location + "!_!" + activeUser.userName + "!_!" + reimburse.ReimburseID;
                                var res3 = await PettyCashService.autoEmail(Base64Encode(temp));

                                if (res3.isSuccess)
                                {
                                    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Success AND Approval Status Success Updated, Please Reload Your Page !");
                                }
                                else
                                {
                                    await _jsModule.InvokeVoidAsync("showAlert", "Email Auto Generate Failed BUT Approval Status Success Updated, Please Reload Your Page !");
                                }

                                reimburse.statusDetails.approveUser = activeUser.userName;
                                PettyCashService.reimburses.SingleOrDefault(a => a.ReimburseID.Equals(reimburse.ReimburseID)).statusDetails.approveUser = activeUser.userName;
                                reimburse.statusDetails.approveDate = DateTime.Now;
                                PettyCashService.reimburses.SingleOrDefault(a => a.ReimburseID.Equals(reimburse.ReimburseID)).statusDetails.approveDate = DateTime.Now;

                            }
                            else if (action.Contains("Resolved"))
                            {
                                await _jsModule.InvokeVoidAsync("showAlert", "Reimburse Amount Difference Resolved, Please Reload Your Page !");

                                reimburse.statusDetails.resolveUser = activeUser.userName;
                                PettyCashService.reimburses.SingleOrDefault(a => a.ReimburseID.Equals(reimburse.ReimburseID)).statusDetails.resolveUser = activeUser.userName;
                                reimburse.statusDetails.resolveDate = DateTime.Now;
                                PettyCashService.reimburses.SingleOrDefault(a => a.ReimburseID.Equals(reimburse.ReimburseID)).statusDetails.resolveDate = DateTime.Now;

                            }
                            else if (action.Contains("Claimed"))
                            {
                                await _jsModule.InvokeVoidAsync("showAlert", "Reimburse Amount Claimed, Please Reload Your Page !");

                                reimburse.statusDetails.claimUser = activeUser.userName;
                                PettyCashService.reimburses.SingleOrDefault(a => a.ReimburseID.Equals(reimburse.ReimburseID)).statusDetails.claimUser = activeUser.userName;
                                reimburse.statusDetails.claimDate = DateTime.Now;
                                PettyCashService.reimburses.SingleOrDefault(a => a.ReimburseID.Equals(reimburse.ReimburseID)).statusDetails.claimDate = DateTime.Now;

                            }

                            reimburse.ReimburseStatus = action;
                            PettyCashService.reimburses.SingleOrDefault(a => a.ReimburseID.Equals(reimburse.ReimburseID)).ReimburseStatus = action;

                            //await _jsModule.InvokeVoidAsync("showAlert", "Approval Status Success Updated, Please Reload Your Page !");
                        }
                        else
                        {
                            await _jsModule.InvokeVoidAsync("showAlert", "Status Might've Updated from Another User, Please Refresh Your Page !");
                        }

                        isLoading = false;
                    }
                    
                }

                StateHasChanged();

            }
            catch (Exception ex)
            {
                isLoading = false;
                await _jsModule.InvokeVoidAsync("showAlert", ex.Message);
                throw new Exception(ex.Message);
            }
            
        }

        private async void updateMultiSelectDocumentStatus(string action)
        {
            try
            {
                isLoading = true;

                if (!validateMultiSelectApproval(action))
                {
                    isLoading = false;
                    await _jsModule.InvokeVoidAsync("showAlert", "Please Check Your Selected Document !");
                }
                else
                {
                    selectedReimburse.ForEach(x =>
                    {
                        x.statusValue = action;
                    });

                    var res = await PettyCashService.editMultiSelectDocumentStatus(selectedReimburse);

                    if (res.Any(x => x.isSuccess.Equals(true)))
                    {
                        finishedDocument = res;

                        selectedReimburse.ForEach(x =>
                        {
                            var dt = PettyCashService.reimburses.SingleOrDefault(y => y.ReimburseID.Equals(x.documentID));

                            if (dt != null)
                            {
                                PettyCashService.reimburses.SingleOrDefault(y => y.ReimburseID.Equals(x.documentID)).ReimburseStatus = action;

                                if (action.Equals("Released"))
                                {
                                    PettyCashService.reimburses.SingleOrDefault(y => y.ReimburseID.Equals(x.documentID)).statusDetails.releaseUser = activeUser.userName;
                                    PettyCashService.reimburses.SingleOrDefault(y => y.ReimburseID.Equals(x.documentID)).statusDetails.releaseDate = DateTime.Now;
                                }
                            }

                        });
                        selectedReimburse.Clear();

                        showMultiSelectApprovalModal = true;
                        await _jsModule.InvokeVoidAsync("showAlert", "Process Finished ! Please Check Process Report !");
                    }
                    else
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", "ALL APPROVAL PROCESS FAILED ! Please Check Your Connection and Retry Your Action !");
                    }
                }

                isLoading = false;
                StateHasChanged();

            }
            catch (Exception ex)
            {
                isLoading = false;
                await _jsModule.InvokeVoidAsync("showAlert", ex.Message);
            }

        }

        private bool validateMultiSelectApproval(string act)
        {
            if (selectedReimburse.Any(x => x.currentDocumentStatus.Equals(act)))
                return false;

            if (act.Equals("Released") && selectedReimburse.Any(x => !x.currentDocumentStatus.Equals("Approved")))
                return false;

            return true;
        }

        private async Task modalShow(Advance advData, Expense expData, Reimburse reimData, string type, string denom)
        {
            transacType = type;
            showModal = true;
            isLoading = true;

            if (type.Contains("advance"))
            {
                isAdvanceActive = true;

                advData.Department = new Department();
                advance = new Advance();
                advance.Department = new Department();

                advance = advData;
                //advance.AdvanceType = advData.AdvanceType.Contains("TF") == true ? "Transfer" : "Cash";

                if (ManagementService.departments.SingleOrDefault(x => x.DepartmentID.Equals(advData.DepartmentID)) != null)
                {
                    advance.Department = ManagementService.departments.SingleOrDefault(x => x.DepartmentID.Equals(advData.DepartmentID));
                }

            }
            else if (type.Contains("expense"))
            {
                isExpenseActive = true;

                string temp = expData.ExpenseID + "!_!" + denom;

                PettyCashService.fileStreams.Clear();
                await Task.Run(async () => { await PettyCashService.getAttachmentFileStream(Base64Encode(temp)); });

                expData.Department = new Department();
                expense = new Expense();
                expense.Department = new Department();

                expense = expData;
                //expense.ExpenseType = expData.ExpenseType.Contains("TF") == true ? "Transfer" : "Cash";

                if (ManagementService.departments.SingleOrDefault(x => x.DepartmentID.Equals(expData.DepartmentID)) != null)
                {
                    expense.Department = ManagementService.departments.SingleOrDefault(x => x.DepartmentID.Equals(expData.DepartmentID));
                }
            }
            else if (type.Contains("reimburse"))
            {
                isReimburseActive = true;

                exp.Clear();
                coaSummary.Clear();
                PettyCashService.fileStreams.Clear();

                foreach (var line in reimData.lines)
                {
                    if (exp.FirstOrDefault(x => x.Equals(line.ExpenseID)) == null)
                    {
                        exp.Add(line.ExpenseID);
                    }
                }

                foreach (var coa in reimData.lines)
                {
                    if (coaSummary.FirstOrDefault(x => x.Equals(coa.AccountNo)) == null)
                    {
                        coaSummary.Add(coa.AccountNo);
                    }
                }

                foreach (var x in exp)
                {
                    string temp = x + "!_!" + denom;

                    await Task.Run(async () => { await PettyCashService.getAttachmentFileStream(Base64Encode(temp)); });

                }

                //await PettyCashService.getAttachmentFileStream(expData.ExpenseID);

                reimburse = reimData;
                
            }

            isLoading = false;
            StateHasChanged();

        }

        private void appendSelectedReimburseLine(ReimburseLine data)
        {
            if (selectedReimburseLines.FirstOrDefault(a => (a.ExpenseID == data.ExpenseID) && (a.ReimburseID == data.ReimburseID) && (a.LineNo == data.LineNo)) == null)
            {
                selectedReimburseLines.Add(data);
            }
            else
            {
                var itemRemove1 = selectedReimburseLines.SingleOrDefault(a => (a.ExpenseID == data.ExpenseID) && (a.ReimburseID == data.ReimburseID) && (a.LineNo == data.LineNo));

                if (itemRemove1 != null)
                {
                    selectedReimburseLines.Remove(itemRemove1);
                }

            }
        }

        private void checkAll()
        {

            if (!selectedReimburseLines.Any())
            {
                foreach (var line in reimburse.lines)
                {
                    selectedReimburseLines.Add(line);
                }

                reimburseCheckAllisChecked = true;
            }
            else
            {
                foreach (var line in reimburse.lines)
                {
                    selectedReimburseLines.Remove(line);
                }

                reimburseCheckAllisChecked = false;
            }
            
        }

        private void reimburseAction(string action)
        {
            if (selectedReimburseLines.Any())
            {
                if (action.Equals("reject"))
                {
                    foreach (var line in selectedReimburseLines)
                    {
                        reimburse.lines.SingleOrDefault(a => (a.ExpenseID == line.ExpenseID) && (a.ReimburseID == line.ReimburseID) && (a.LineNo == line.LineNo)).Status = "CL";
                        reimburse.lines.SingleOrDefault(a => (a.ExpenseID == line.ExpenseID) && (a.ReimburseID == line.ReimburseID) && (a.LineNo == line.LineNo)).ApprovedAmount = 0;
                    }
                }
                else if (action.Equals("approve"))
                {
                    foreach (var line in selectedReimburseLines)
                    {
                        reimburse.lines.SingleOrDefault(a => (a.ExpenseID == line.ExpenseID) && (a.ReimburseID == line.ReimburseID) && (a.LineNo == line.LineNo)).Status = "AP";
                        reimburse.lines.SingleOrDefault(a => (a.ExpenseID == line.ExpenseID) && (a.ReimburseID == line.ReimburseID) && (a.LineNo == line.LineNo)).ApprovedAmount = reimburse.lines.SingleOrDefault(a => (a.ExpenseID == line.ExpenseID) && (a.ReimburseID == line.ReimburseID) && (a.LineNo == line.LineNo)).Amount;
                    }
                }
                else if (action.Equals("revert"))
                {
                    foreach (var line in selectedReimburseLines)
                    {
                        reimburse.lines.SingleOrDefault(a => (a.ExpenseID == line.ExpenseID) && (a.ReimburseID == line.ReimburseID) && (a.LineNo == line.LineNo)).Status = "OP";
                        reimburse.lines.SingleOrDefault(a => (a.ExpenseID == line.ExpenseID) && (a.ReimburseID == line.ReimburseID) && (a.LineNo == line.LineNo)).ApprovedAmount = 0;
                    }
                }
            }

            selectedReimburseLines.Clear();
            reimburseCheckAllisChecked = false;
        }

        // master
        private async Task advancePageSelect(int currPage)
        {
            advancePageActive = currPage;
            isLoading = true;

            if (isAdvanceFilterActive)
            {
                string advStatus = "";
                string advFilType = advFilterType;
                string advFilValue = advFilterValue;

                string advlocPage = "MASTER!_!" + activeUser.location + "!_!" + advStatus + "!_!" + advFilType + "!_!" + advFilValue + "!_!" + advancePageActive.ToString();
                await PettyCashService.getAdvanceDatabyLocation(Base64Encode(advlocPage));
            }
            else
            {
                string advStatus = "";
                string advFilType = "";
                string advFilValue = "";

                string advlocPage = "MASTER!_!" + activeUser.location + "!_!" + advStatus + "!_!" + advFilType + "!_!" + advFilValue + "!_!" + advancePageActive.ToString();
                await PettyCashService.getAdvanceDatabyLocation(Base64Encode(advlocPage));
            }
            isLoading = false;
        }

        private async Task expensePageSelect(int currPage)
        {
            expensePageActive = currPage;
            isLoading = true;

            if (isExpenseFilterActive)
            {
                string expStatus = "";
                string expFilType = expFilterType;
                string expFilValue = expFilterValue;

                string explocPage = "MASTER!_!" + activeUser.location + "!_!" + expStatus + "!_!" + expFilType + "!_!" + expFilValue + "!_!" + expensePageActive.ToString();
                await PettyCashService.getExpenseDatabyLocation(Base64Encode(explocPage));
            }
            else
            {
                string expStatus = "";
                string expFilType = "";
                string expFilValue = "";

                string explocPage = "MASTER!_!" + activeUser.location + "!_!" + expStatus + "!_!" + expFilType + "!_!" + expFilValue + "!_!" + expensePageActive.ToString();
                await PettyCashService.getExpenseDatabyLocation(Base64Encode(explocPage));
            }
            isLoading = false;

        }

        private async Task reimbursePageSelect(int currPage)
        {
            reimbursePageActive = currPage;
            isLoading = true;

            if (isReimburseFilterActive)
            {
                string remStatus = "";
                string remFilType = remFilterType;
                string remFilValue = remFilterValue;

                string rbslocPage = "MASTER!_!" + activeUser.location + "!_!" + remStatus + "!_!" + remFilType + "!_!" + remFilValue + "!_!" + reimbursePageActive.ToString();
                await PettyCashService.getReimburseDatabyLocation(Base64Encode(rbslocPage));
            }
            else
            {
                string remStatus = "";
                string remFilType = "";
                string remFilValue = "";

                string rbslocPage = "MASTER!_!" + activeUser.location + "!_!" + remStatus + "!_!" + remFilType + "!_!" + remFilValue + "!_!" + reimbursePageActive.ToString();
                await PettyCashService.getReimburseDatabyLocation(Base64Encode(rbslocPage));
            }
            isLoading = false;

        }

        // posted
        private async Task padvancePageSelect(int currPage)
        {
            padvancePageActive = currPage;
            isLoading = true;

            if (ispAdvanceFilterActive)
            {
                string advStatus = "";
                string advFilType = padvFilterType;
                string advFilValue = padvFilterValue;

                string advlocPage = "POSTED!_!" + activeUser.location + "!_!" + advStatus + "!_!" + advFilType + "!_!" + advFilValue + "!_!" + padvancePageActive.ToString();
                await PettyCashService.getAdvanceDatabyLocation(Base64Encode(advlocPage));
            }
            else
            {
                string advStatus = "";
                string advFilType = "";
                string advFilValue = "";

                string advlocPage = "POSTED!_!" + activeUser.location + "!_!" + advStatus + "!_!" + advFilType + "!_!" + advFilValue + "!_!" + padvancePageActive.ToString();
                await PettyCashService.getAdvanceDatabyLocation(Base64Encode(advlocPage));
            }
            isLoading = false;

        }

        private async Task pexpensePageSelect(int currPage)
        {
            pexpensePageActive = currPage;
            isLoading = true;

            if (ispExpenseFilterActive)
            {
                string expStatus = "";
                string expFilType = pexpFilterType;
                string expFilValue = pexpFilterValue;

                string explocPage = "POSTED!_!" + activeUser.location + "!_!" + expStatus + "!_!" + expFilType + "!_!" + expFilValue + "!_!" + pexpensePageActive.ToString();
                await PettyCashService.getExpenseDatabyLocation(Base64Encode(explocPage));
            }
            else
            {
                string expStatus = "";
                string expFilType = "";
                string expFilValue = "";

                string explocPage = "POSTED!_!" + activeUser.location + "!_!" + expStatus + "!_!" + expFilType + "!_!" + expFilValue + "!_!" + pexpensePageActive.ToString();
                await PettyCashService.getExpenseDatabyLocation(Base64Encode(explocPage));
            }
            isLoading = false;

        }

        private async Task preimbursePageSelect(int currPage)
        {
            preimbursePageActive = currPage;
            isLoading = true;

            if (ispReimburseFilterActive)
            {
                string remStatus = "";
                string remFilType = premFilterType;
                string remFilValue = premFilterValue;

                string rbslocPage = "POSTED!_!" + activeUser.location + "!_!" + remStatus + "!_!" + remFilType + "!_!" + remFilValue + "!_!" + preimbursePageActive.ToString();
                await PettyCashService.getReimburseDatabyLocation(Base64Encode(rbslocPage));
            }
            else
            {
                string remStatus = "";
                string remFilType = "";
                string remFilValue = "";

                string rbslocPage = "POSTED!_!" + activeUser.location + "!_!" + remStatus + "!_!" + remFilType + "!_!" + remFilValue + "!_!" + preimbursePageActive.ToString();
                await PettyCashService.getReimburseDatabyLocation(Base64Encode(rbslocPage));
            }
            isLoading = false;

        }

        // master
        private async Task advanceFilter()
        {
            if (advFilterType.Length > 0)
            {
                advancePageActive = 1;
                isAdvanceFilterActive = true;
                isLoading = true;

                string advStatus = "";
                string advFilType = "";
                string advFilValue = "";

                if (!advFilterType.Equals("STATUS"))
                {
                    advStatus = "";
                    advFilType = advFilterType;
                    advFilValue = advFilterValue;
                }
                else
                {
                    advStatus = advFilterValue;
                    advFilType = advFilterType;
                    advFilValue = "";
                }

                PettyCashService.advances.Clear();

                string advpz = "Advance!_!AdvanceID!_!" + advStatus + "!_!" + advFilType + "!_!" + advFilValue + "!_!" + activeUser.location;
                advanceNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(advpz));

                string advlocPage = "MASTER!_!" + activeUser.location + "!_!" + advStatus + "!_!" + advFilType + "!_!" + advFilValue + "!_!" + advancePageActive.ToString();
                await PettyCashService.getAdvanceDatabyLocation(Base64Encode(advlocPage));
                isLoading = false;
            }
            else
            {
                await _jsModule.InvokeVoidAsync("showAlert", "Please Select Filter Type !");
            }
            
        }

        private async Task advanceFilterReset()
        {
            isLoading = true;
            advancePageActive = 1;
            isAdvanceFilterActive = false;
            advFilterType = "";
            advFilterValue = "";
            advancePageActive = 1;

            string advStatus = "";
            string advFilType = "";
            string advFilValue = "";

            string advpz = "Advance!_!AdvanceID!_!" + advStatus + "!_!" + advFilType + "!_!" + advFilValue + "!_!" + activeUser.location;
            advanceNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(advpz));

            string advlocPage = "MASTER!_!" + activeUser.location + "!_!" + advStatus + "!_!" + advFilType + "!_!" + advFilValue + "!_!" + advancePageActive.ToString();
            await PettyCashService.getAdvanceDatabyLocation(Base64Encode(advlocPage));
            isLoading = false;

        }

        private async Task expenseFilter()
        {
            if (expFilterType.Length > 0)
            {
                expensePageActive = 1;
                isExpenseFilterActive = true;
                isLoading = true;

                string expStatus = "";
                string expFilType = "";
                string expFilValue = "";

                if (!expFilterType.Equals("STATUS"))
                {
                    expStatus = "";
                    expFilType = expFilterType;
                    expFilValue = expFilterValue;
                }
                else
                {
                    expStatus = expFilterValue;
                    expFilType = expFilterType;
                    expFilValue = "";
                }

                PettyCashService.expenses.Clear();

                string exppz = "Expense!_!ExpenseID!_!" + expStatus + "!_!" + expFilType + "!_!" + expFilValue + "!_!" + activeUser.location;
                expenseNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(exppz));

                string explocPage = "MASTER!_!" + activeUser.location + "!_!" + expStatus + "!_!" + expFilType + "!_!" + expFilValue + "!_!" + expensePageActive.ToString();
                await PettyCashService.getExpenseDatabyLocation(Base64Encode(explocPage));
                isLoading = false;
            }
            else
            {
                await _jsModule.InvokeVoidAsync("showAlert", "Please Select Filter Type !");
            }

        }

        private async Task expenseFilterReset()
        {
            isLoading = true;
            expensePageActive = 1;
            isExpenseFilterActive = false;
            expFilterType = "";
            expFilterValue = "";
            expensePageActive = 1;

            string expStatus = "";
            string expFilType = "";
            string expFilValue = "";

            string explocPage = "MASTER!_!" + activeUser.location + "!_!" + expStatus + "!_!" + expFilType + "!_!" + expFilValue + "!_!" + expensePageActive.ToString();
            await PettyCashService.getExpenseDatabyLocation(Base64Encode(explocPage));

            string exppz = "Expense!_!ExpenseID!_!" + expStatus + "!_!" + expFilType + "!_!" + expFilValue + "!_!" + activeUser.location;
            expenseNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(exppz));
            isLoading = false;
        }

        private async Task reimburseFilter()
        {
            if (remFilterType.Length > 0)
            {
                reimbursePageActive = 1;
                isReimburseFilterActive = true;
                isLoading = true;

                PettyCashService.reimburses.Clear();

                if (remFilterType.Equals("STATUS"))
                {
                    string remStatus = remFilterValue;
                    string remFilType = "ID";
                    string remFilValue = "";

                    string rbspz = "Reimburse!_!ReimburseID!_!" + remStatus + "!_!" + remFilType + "!_!" + remFilValue + "!_!" + activeUser.location;
                    reimburseNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(rbspz));

                    string rbslocPage = "MASTER!_!" + activeUser.location + "!_!" + remStatus + "!_!" + remFilType + "!_!" + remFilValue + "!_!" + reimbursePageActive.ToString();
                    await PettyCashService.getReimburseDatabyLocation(Base64Encode(rbslocPage));
                }
                else
                {
                    string remStatus = "";
                    string remFilType = remFilterType;
                    string remFilValue = remFilterValue;

                    string rbspz = "Reimburse!_!ReimburseID!_!" + remStatus + "!_!" + remFilType + "!_!" + remFilValue + "!_!" + activeUser.location;
                    reimburseNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(rbspz));

                    string rbslocPage = "MASTER!_!" + activeUser.location + "!_!" + remStatus + "!_!" + remFilType + "!_!" + remFilValue + "!_!" + reimbursePageActive.ToString();
                    await PettyCashService.getReimburseDatabyLocation(Base64Encode(rbslocPage));
                }
                
                isLoading = false;
            }
            else
            {
                await _jsModule.InvokeVoidAsync("showAlert", "Please Select Filter Type !");
            }

        }

        private async Task reimburseFilterReset()
        {
            isLoading = true;
            reimbursePageActive = 1;
            isReimburseFilterActive = false;
            remFilterType = "";
            remFilterValue = "";
            reimbursePageActive = 1;

            string remStatus = "";
            string remFilType = "";
            string remFilValue = "";

            string rbspz = "Reimburse!_!ReimburseID!_!" + remStatus + "!_!" + remFilType + "!_!" + remFilValue + "!_!" + activeUser.location;
            reimburseNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(rbspz));

            string rbslocPage = "MASTER!_!" + activeUser.location + "!_!" + remStatus + "!_!" + remFilType + "!_!" + remFilValue + "!_!" + reimbursePageActive.ToString();
            await PettyCashService.getReimburseDatabyLocation(Base64Encode(rbslocPage));
            isLoading = false;
        }

        // posted
        private async Task padvanceFilter()
        {
            if (padvFilterType.Length > 0)
            {
                padvancePageActive = 1;
                ispAdvanceFilterActive = true;
                isLoading = true;

                string advStatus = "";
                string advFilType = padvFilterType;
                string advFilValue = padvFilterValue;

                PettyCashService.padvances.Clear();

                string advpz = "PostedAdvance!_!AdvanceID!_!" + advStatus + "!_!" + advFilType + "!_!" + advFilValue + "!_!" + activeUser.location;
                padvanceNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(advpz));

                string advlocPage = "POSTED!_!" + activeUser.location + "!_!" + advStatus + "!_!" + advFilType + "!_!" + advFilValue + "!_!" + padvancePageActive.ToString();
                await PettyCashService.getAdvanceDatabyLocation(Base64Encode(advlocPage));
                isLoading = false;
            }
            else
            {
                await _jsModule.InvokeVoidAsync("showAlert", "Please Select Filter Type !");
            }

        }

        private async Task padvanceFilterReset()
        {
            isLoading = true;
            padvancePageActive = 1;
            ispAdvanceFilterActive = false;
            padvFilterType = "";
            padvFilterValue = "";
            padvancePageActive = 1;

            string advStatus = "";
            string advFilType = "";
            string advFilValue = "";

            string advpz = "PostedAdvance!_!AdvanceID!_!" + advStatus + "!_!" + advFilType + "!_!" + advFilValue + "!_!" + activeUser.location;
            padvanceNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(advpz));

            string advlocPage = "POSTED!_!" + activeUser.location + "!_!" + advStatus + "!_!" + advFilType + "!_!" + advFilValue + "!_!" + padvancePageActive.ToString();
            await PettyCashService.getAdvanceDatabyLocation(Base64Encode(advlocPage));
            isLoading = false;

        }

        private async Task pexpenseFilter()
        {
            if (pexpFilterType.Length > 0)
            {
                pexpensePageActive = 1;
                ispExpenseFilterActive = true;
                isLoading = true;

                string expStatus = "";
                string expFilType = pexpFilterType;
                string expFilValue = pexpFilterValue;

                PettyCashService.pexpenses.Clear();

                string exppz = "PostedExpense!_!ExpenseID!_!" + expStatus + "!_!" + expFilType + "!_!" + expFilValue + "!_!" + activeUser.location;
                pexpenseNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(exppz));

                string explocPage = "POSTED!_!" + activeUser.location + "!_!" + expStatus + "!_!" + expFilType + "!_!" + expFilValue + "!_!" + pexpensePageActive.ToString();
                await PettyCashService.getExpenseDatabyLocation(Base64Encode(explocPage));
                isLoading = false;
            }
            else
            {
                await _jsModule.InvokeVoidAsync("showAlert", "Please Select Filter Type !");
            }

        }

        private async Task pexpenseFilterReset()
        {
            isLoading = true;
            pexpensePageActive = 1;
            ispExpenseFilterActive = false;
            pexpFilterType = "";
            pexpFilterValue = "";
            pexpensePageActive = 1;

            string expStatus = "";
            string expFilType = "";
            string expFilValue = "";

            string explocPage = "POSTED!_!" + activeUser.location + "!_!" + expStatus + "!_!" + expFilType + "!_!" + expFilValue + "!_!" + pexpensePageActive.ToString();
            await PettyCashService.getExpenseDatabyLocation(Base64Encode(explocPage));

            string exppz = "PostedExpense!_!ExpenseID!_!" + expStatus + "!_!" + expFilType + "!_!" + expFilValue + "!_!" + activeUser.location;
            pexpenseNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(exppz));
            isLoading = false;
        }

        private async Task preimburseFilter()
        {
            if (premFilterType.Length > 0)
            {
                preimbursePageActive = 1;
                ispReimburseFilterActive = true;
                isLoading = true;

                string remStatus = "";
                string remFilType = premFilterType;
                string remFilValue = premFilterValue;

                PettyCashService.preimburses.Clear();

                string rbspz = "PostedReimburse!_!ReimburseID!_!" + remStatus + "!_!" + remFilType + "!_!" + remFilValue + "!_!" + activeUser.location;
                preimburseNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(rbspz));

                string rbslocPage = "POSTED!_!" + activeUser.location + "!_!" + remStatus + "!_!" + remFilType + "!_!" + remFilValue + "!_!" + preimbursePageActive.ToString();
                await PettyCashService.getReimburseDatabyLocation(Base64Encode(rbslocPage));
                isLoading = false;
            }
            else
            {
                await _jsModule.InvokeVoidAsync("showAlert", "Please Select Filter Type !");
            }

        }

        private async Task preimburseFilterReset()
        {
            isLoading = true;
            preimbursePageActive = 1;
            ispReimburseFilterActive = false;
            premFilterType = "";
            premFilterValue = "";
            preimbursePageActive = 1;

            string remStatus = "";
            string remFilType = "";
            string remFilValue = "";

            string rbspz = "PostedReimburse!_!ReimburseID!_!" + remStatus + "!_!" + remFilType + "!_!" + remFilValue + "!_!" + activeUser.location;
            preimburseNumberofPage = await PettyCashService.getModulePageSize(Base64Encode(rbspz));

            string rbslocPage = "POSTED!_!" + activeUser.location + "!_!" + remStatus + "!_!" + remFilType + "!_!" + remFilValue + "!_!" + preimbursePageActive.ToString();
            await PettyCashService.getReimburseDatabyLocation(Base64Encode(rbslocPage));
            isLoading = false;
        }

        private void appendReimburseSelected(string docType, string docId, string docLoc, string currStatus)
        {
            if (selectedReimburse.FirstOrDefault(a => a.documentID == docId) == null)
            {
                selectedReimburse.Add(new ReimbursementMultiSelectStatusUpdate
                {
                    docType = docType,
                    documentID = docId,
                    statusValue = "",
                    approver = activeUser.userName,
                    documentLocation = docLoc,
                    currentDocumentStatus = currStatus
                });
            }
            else
            {
                var itemRemove1 = selectedReimburse.SingleOrDefault(a => a.documentID == docId);

                if (itemRemove1 != null)
                {
                    selectedReimburse.Remove(itemRemove1);
                }

            }
        }

        // master
        private bool checkAdvanceDataPresent()
        {
            try
            {
                if (PettyCashService.advances.Any())
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

        private bool checkExpenseDataPresent()
        {
            try
            {
                if (PettyCashService.expenses.Any())
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

        private bool checkReimburseDataPresent()
        {
            try
            {
                if (PettyCashService.reimburses.Any())
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

        // posted
        private bool checkpAdvanceDataPresent()
        {
            try
            {
                if (PettyCashService.padvances.Any())
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

        private bool checkpExpenseDataPresent()
        {
            try
            {
                if (PettyCashService.pexpenses.Any())
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

        private bool checkpReimburseDataPresent()
        {
            try
            {
                if (PettyCashService.preimburses.Any())
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

        private bool checkFinishedDocumentDataPresent()
        {
            try
            {
                if (finishedDocument.Any())
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

        private void modalHide()
        {
            showModal = false;

            isAdvanceActive = false;
            isExpenseActive = false;
            isReimburseActive = false;
        }

    }
}
