using BPILibrary;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.EPKRS;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.MainModel.Mailing;
using BPIWebApplication.Shared.PagesModel.EPKRS;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.Linq;
using static System.Net.WebRequestMethods;

namespace BPIWebApplication.Client.Pages.EPKRSPages
{
    public partial class EPKRSDashboard : ComponentBase
    {
        private ActiveUser activeUser = new();
        private UserPrivileges privilegeDataParam = new();
        private List<string> userPriv = new();

        private Location paramGetCompanyLocation = new();

        private ItemCase previewItemCase = new();
        private IncidentAccident previewIncidentAccident = new();
        private List<ItemLine> previewItemLine = new();
        private List<CaseAttachment> previewCaseAttachment = new();
        private List<DocumentDiscussionForm> previewDocumentDiscussion = new();
        private DocumentApproval previewDocumentApproval = new();
        private List<DocumentApproval> previewDocumentListApproval = new();
        private List<IncidentAccidentInvolver> previewIncidentAccidentInvolver = new();
        private List<EPKRSDocumentStatistics>? previewGeneralItemCaseStatistics = null;
        private List<EPKRSItemCaseCategoryStatistics>? previewItemCaseCategoryStatistics = null;
        private List<EPKRSTopLocationReportStatistics>? previewTopLocationReportStatistics = null;
        private List<EPKRSItemCaseItemCategoryStatistics>? previewItemCategoryStatistics = null;

        private List<EPKRSIncidentAccidentforStats>? incacDataforStatistics = null;

        private ItemCase updateItemCase = new();
        private IncidentAccident updateIncidentAccident = new();
        private List<IncidentAccidentInvolver> partyInvolved = new();

        private DocumentDiscussion uploadDocumentDiscussion = new();
        //private List<CaseAttachment> uploadDocumentDiscussionAttachment = new();
        private List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileDiscussionUpload = new();

        private DocumentDiscussion uploadReplyDocumentDiscussion = new();
        //private List<CaseAttachment> uploadReplyDocumentDiscussionAttachment = new();
        private List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileReplyDiscussionUpload = new();

        private CustomMailing customMailData { get; set; } = new();
        private string emailInputTo = string.Empty;
        private string emailInputCC = string.Empty;

        private string previousPreviewedDocumentID = string.Empty;
        private string previousPreviewedDocumentLocation = string.Empty;

        private int itemCaseNumberofPages = 0;
        private int incidentAccidentNumberofPages = 0;
        private int itemCasePageActive = 0;
        private int incidentAccidentPageActive = 0;

        private int regionStatPageActive = 0;
        private int locStatPageActive = 0;
        private int involvedPositionPageActive = 0;
        private int involvedDeptPageActive = 0;
        private int statRowPerPage = 0;
        private int prevStatDocPageActive = 0;

        private string itemCaseFilterType { get; set; } = string.Empty; 
        private string incidentAccidentFilterType { get; set; } = string.Empty;

        private string itemCaseFilterValue { get; set; } = string.Empty;
        private string incidentAccidentFilterValue { get; set; } = string.Empty;

        private bool itemCaseFilterActive = false;
        private bool incidentAccidentFilterActive = false;

        private List<string> incacRiskTypeValueFilter = new() { };
        private List<string> incacRegionValueFilter = new() { };
        private List<string> incacLocationValueFilter = new() { };
        private List<string> incacPositionValueFilter = new() { };
        private List<string> incacDeptValueFilter = new() { };

        private int selectedYearFilter = DateTime.Now.Year;
        private int selectedYearFilterChart = DateTime.Now.Year;
        private string dinamicGeneralStatisticsDateFilter = string.Empty;
        private string dinamicMonitoringPanelDateFilter = string.Empty;
        private string dinamicMonitoringPanelDateFilterExt = string.Empty;

        private DateTime startDateExport = new(2023, 1, 1);
        private DateTime endDateExport = DateTime.Now;
        private bool isExported = false;
        private bool isExportLoading = false;

        private Dictionary<int, bool> monthSelector = new Dictionary<int, bool>
        {
            { 1, false },
            { 2, false },
            { 3, false },
            { 4, false },
            { 5, false },
            { 6, false },
            { 7, false },
            { 8, false },
            { 9, false },
            { 10, false },
            { 11, false },
            { 12, false },
        };
        private Dictionary<int, bool> monthSelectorChart = new Dictionary<int, bool>
        {
            { 1, false },
            { 2, false },
            { 3, false },
            { 4, false },
            { 5, false },
            { 6, false },
            { 7, false },
            { 8, false },
            { 9, false },
            { 10, false },
            { 11, false },
            { 12, false },
        };

        private bool isLoading = false;
        private bool chartTriggered = false;
        private bool triggerReplyButton = false;

        private int maxFileSize { get; set; } = 0;

        private bool alertTrigger = false;
        private bool successAlert = false;
        private string alertBody = string.Empty;
        private string alertMessage = string.Empty;

        private IJSObjectReference _jsModule;
        private IReadOnlyList<IBrowserFile>? listFileUpload, listReplyFileUpload;

        private ElementReference topLocationReportStatsRef;
        private ElementReference itemCategoryStatsRef;
        private ElementReference itemCaseValueStatsRef;
        private ElementReference itemCaseCategoryStatsRef;

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

            paramGetCompanyLocation.Condition = $"a.CompanyId={Convert.ToInt32(activeUser.company)}";
            paramGetCompanyLocation.PageIndex = 1;
            paramGetCompanyLocation.PageSize = 100;
            paramGetCompanyLocation.FieldOrder = "a.CompanyId";
            paramGetCompanyLocation.MethodOrder = "ASC";

            string paramGetAllDepartment = activeUser.location.Equals("") ? "HO" : activeUser.location;

            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(async () =>
            {
                await ManagementService.GetCompanyLocations(paramGetCompanyLocation);
                await ManagementService.GetAllDepartment(CommonLibrary.Base64Encode(paramGetAllDepartment));
                await ManagementService.getAllCategories();
                await ManagementService.getRegionData();

                StateHasChanged();
            });

            Task.Run(async () =>
            {
                await EPKRSService.getEPRKSReportingType();
                await EPKRSService.getEPRKSRiskType();
                await EPKRSService.getEPRKSRiskSubType();
                await EPKRSService.getEPKRSItemRiskCategory();
                await EPKRSService.getEPKRSIncidentAccidentInvolverType();
                maxFileSize = await EPKRSService.getEPKRSMaxFileSize();

                StateHasChanged();
            });
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed


            incidentAccidentFilterActive = false;
            itemCaseFilterActive = false;
            QueryModel<string> param = new();

            string paramGetItemCaseInitialization = activeUser.location + "!_!!_!1";
            param = new();
            param.Data = CommonLibrary.Base64Encode(paramGetItemCaseInitialization);
            var itemCaseDt = await EPKRSService.getEPKRSItemCaseData(param);

            string paramGetIncidentAccidentInitialization = activeUser.location + "!_!!_!1";
            param = new();
            param.Data = CommonLibrary.Base64Encode(paramGetIncidentAccidentInitialization);
            var incidentAccidentDt = await EPKRSService.getEPKRSIncidentAccidentData(param);

            List<DocumentListParams> docParams = new();

            if (itemCaseDt.Data != null)
            {
                if (itemCaseDt.Data.Count > 0)
                {
                    itemCaseDt.Data.ForEach(x =>
                    {
                        docParams.Add(new DocumentListParams
                        {
                            LocationID = x.itemCase.SiteReporter,
                            DocumentID = x.itemCase.DocumentID
                        });
                    });
                }
            }

            if (incidentAccidentDt.Data != null)
            {
                if (incidentAccidentDt.Data.Count > 0)
                {
                    incidentAccidentDt.Data.ForEach(x =>
                    {
                        docParams.Add(new DocumentListParams
                        {
                            LocationID = x.incidentAccident.SiteReporter,
                            DocumentID = x.incidentAccident.DocumentID
                        });
                    });
                }
            }

            #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(async () =>
            {
                await EPKRSService.getEPKRSInitializationDocumentDiscussions(docParams);
                await EPKRSService.getEPKRSDocumentDiscussionReadHistory(docParams);

                StateHasChanged();
            });
            #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            if (activeUser.location.Equals(""))
            {
                string itemCaseParampz = $"[EPKRS].ItemCase!_!{activeUser.location}!_!";
                param.Data = CommonLibrary.Base64Encode(itemCaseParampz);
                var itemCasepz = await EPKRSService.getEPKRSModuleNumberOfPage(param);
                itemCaseNumberofPages = itemCasepz.Data;
                
                string incidentAccidentParampz = $"[EPKRS].IncidentAccident!_!{activeUser.location}!_!";
                param.Data = CommonLibrary.Base64Encode(incidentAccidentParampz);
                var incidentAccidentpz = await EPKRSService.getEPKRSModuleNumberOfPage(param);
                incidentAccidentNumberofPages = incidentAccidentpz.Data;

                #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Task.Run(async () =>
                {
                    string generalStatisticsItemcConditions = $"WHERE a.ReportDate BETWEEN \'{selectedYearFilterChart}-01-01 00:00:00\' AND \'{selectedYearFilterChart}-12-31 23:59:59\'!_!ITEMC";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(generalStatisticsItemcConditions);
                    var resGeneralItemcStat = await EPKRSService.getEPKRSGeneralStatistics(param);

                    string itemCaseCategoryConditions = $"WHERE a.AuditActionDate BETWEEN \'{selectedYearFilter}-01-01 00:00:00\' AND \'{selectedYearFilter}-12-31 23:59:59\'!_!AND (p.ReportDate BETWEEN \'{selectedYearFilter}-01-01 00:00:00\' AND \'{selectedYearFilter}-12-31 23:59:59\')";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(itemCaseCategoryConditions);
                    var resItemCaseCategoryStat = await EPKRSService.getEPKRSItemCaseCategoryStatistics(param);

                    string topLocationReportConditions = $"WHERE a.ReportDate BETWEEN \'{selectedYearFilter}-01-01 00:00:00\' AND \'{selectedYearFilter}-12-31 23:59:59\'!_!AND (p.ReportDate BETWEEN \'{selectedYearFilter}-01-01 00:00:00\' AND \'{selectedYearFilter}-12-31 23:59:59\')";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(topLocationReportConditions);
                    var resTopLocationReportStat = await EPKRSService.getEPKRSTopLocationReportStatistics(param);

                    string itemCategoryStatsConditions = $"WHERE a.AuditActionDate BETWEEN \'{selectedYearFilter}-01-01 00:00:00\' AND \'{selectedYearFilter}-12-31 23:59:59\'!_!AND (p.ReportDate BETWEEN \'{selectedYearFilter}-01-01 00:00:00\' AND \'{selectedYearFilter}-12-31 23:59:59\')";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(itemCategoryStatsConditions);
                    var resItemCategoryStat = await EPKRSService.getEPKRSItemCategoriesStatistics(param);

                    if (resGeneralItemcStat.isSuccess)
                    {
                        previewGeneralItemCaseStatistics = new();
                        previewGeneralItemCaseStatistics = resGeneralItemcStat.Data;
                    }

                    if (resItemCaseCategoryStat.isSuccess)
                    {
                        previewItemCaseCategoryStatistics = new();
                        previewItemCaseCategoryStatistics = resItemCaseCategoryStat.Data;
                    }

                    if (resTopLocationReportStat.isSuccess)
                    {
                        previewTopLocationReportStatistics = new();
                        previewTopLocationReportStatistics = resTopLocationReportStat.Data;
                    }

                    if (resItemCategoryStat.isSuccess)
                    {
                        previewItemCategoryStatistics = new();
                        previewItemCategoryStatistics = resItemCategoryStat.Data;
                    }

                    StateHasChanged();
                });

                Task.Run(async () =>
                {
                    var statpz = await EPKRSService.getEPKRSMaxStatisticsRow();

                    string incacDataforStatsParam = $"WHERE a.ReportDate BETWEEN \'{selectedYearFilter}-01-01 00:00:00\' AND \'{selectedYearFilter}-12-31 23:59:59\'";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(incacDataforStatsParam);
                    var resIncacDataforStats = await EPKRSService.getEPKRSIncidentAccidentDataforStatistics(param);
                    
                    if (resIncacDataforStats.isSuccess)
                    {
                        statRowPerPage = statpz;
                        regionStatPageActive = 1;
                        locStatPageActive = 1;
                        involvedPositionPageActive = 1;
                        involvedDeptPageActive = 1;
                        prevStatDocPageActive = 1;

                        incacDataforStatistics = new();
                        incacDataforStatistics = resIncacDataforStats.Data;
                    }

                    StateHasChanged();
                });
                #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
            else
            {
                string itemCaseParampz = $"[EPKRS].ItemCase!_!{activeUser.location}!_!WHERE SiteReporter = \'{activeUser.location}\' OR SiteSender = \'{activeUser.location}\'";
                param.Data = CommonLibrary.Base64Encode(itemCaseParampz);
                var itemCasepz = await EPKRSService.getEPKRSModuleNumberOfPage(param);
                itemCaseNumberofPages = itemCasepz.Data;

                string incidentAccidentParampz = $"[EPKRS].IncidentAccident!_!{activeUser.location}!_!WHERE SiteReporter = \'{activeUser.location}\'";
                param.Data = CommonLibrary.Base64Encode(incidentAccidentParampz);
                var incidentAccidentpz = await EPKRSService.getEPKRSModuleNumberOfPage(param);
                incidentAccidentNumberofPages = incidentAccidentpz.Data;
            }
            itemCasePageActive = 1;
            incidentAccidentPageActive = 1;

            StateHasChanged();
            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/EPKRSPages/EPKRSDashboard.razor.js");
        }

        private async Task initializeChart()
        {
            if (!chartTriggered)
            {
                List<string> arrayLabels = new();
                List<string> arrayStringValues = new();
                List<int> arrayIntValues = new();
                List<decimal> arrayDecimalValues = new();

                if (previewTopLocationReportStatistics != null)
                {
                    previewTopLocationReportStatistics.ForEach(x =>
                    {
                        if (x.LocationID.Equals("HO"))
                        {
                            arrayLabels.Add("HEAD OFFICE");
                        }
                        else
                        {
                            var loc = ManagementService.locations.SingleOrDefault(y => y.locationId.Equals(x.LocationID));

                            if (loc != null)
                                arrayLabels.Add(loc.locationName);
                            else
                                arrayLabels.Add(x.LocationID);
                        }

                        arrayIntValues.Add(x.TotalDocuments);
                    });

                    await _jsModule.InvokeVoidAsync(
                        "initializeBarChart",
                        "topLocationReportStats",
                        arrayLabels,
                        arrayIntValues,
                        "10 Lokasi dengan Pelaporan Terbanyak",
                        "# of Reports"
                    );
                }

                arrayLabels.Clear();
                arrayStringValues.Clear();
                arrayIntValues.Clear();
                arrayDecimalValues.Clear();

                if (previewItemCaseCategoryStatistics != null)
                {
                    previewItemCaseCategoryStatistics.ForEach(x =>
                    {
                        var riskCat = EPKRSService.itemRiskCategories.SingleOrDefault(y => y.ItemRiskCategoryID.Equals(x.ItemRiskCategoryID));

                        if (riskCat != null)
                            arrayLabels.Add(riskCat.CategoryDescription);
                        else
                            arrayLabels.Add(x.ItemRiskCategoryID);
                        
                        arrayIntValues.Add(x.TotalItemQty);
                    });

                    await _jsModule.InvokeVoidAsync(
                        "initializeDoughnutChart",
                        "itemCaseCategoryStats",
                        arrayLabels,
                        arrayIntValues,
                        "Total Qty Barang Bermasalah berdasarkan Tipe",
                        "# of Reports"
                    );
                }

                arrayLabels.Clear();
                arrayStringValues.Clear();
                arrayIntValues.Clear();
                arrayDecimalValues.Clear();

                if (previewItemCaseCategoryStatistics != null)
                {
                    previewItemCaseCategoryStatistics.ForEach(x =>
                    {
                        var riskCat = EPKRSService.itemRiskCategories.SingleOrDefault(y => y.ItemRiskCategoryID.Equals(x.ItemRiskCategoryID));

                        if (riskCat != null)
                            arrayLabels.Add(riskCat.CategoryDescription);
                        else
                            arrayLabels.Add(x.ItemRiskCategoryID);

                        arrayDecimalValues.Add(x.TotalItemValue);
                    });

                    await _jsModule.InvokeVoidAsync(
                        "initializeBarChart",
                        "itemCaseValueStats",
                        arrayLabels,
                        arrayDecimalValues,
                        "Total Value Barang Bermasalah berdasarkan Tipe",
                        "Loss Estimation"
                    );
                }

                arrayLabels.Clear();
                arrayStringValues.Clear();
                arrayIntValues.Clear();
                arrayDecimalValues.Clear();

                if (previewItemCategoryStatistics != null)
                {
                    previewItemCategoryStatistics.ForEach(x =>
                    {
                        var cat = ManagementService.categories.SingleOrDefault(y => y.CategoryID.Equals(x.CategoryID));

                        if (cat != null)
                            arrayLabels.Add(cat.CategoryDescription);
                        else
                            arrayLabels.Add(x.CategoryID);

                        arrayIntValues.Add(x.TotalDocuments);
                    });

                    await _jsModule.InvokeVoidAsync(
                        "initializeBarChart",
                        "itemCategoryStats",
                        arrayLabels,
                        arrayIntValues,
                        "Total Pelaporan berdasarkan Kategori Barang",
                        "# of Reports"
                    );
                }
            }

            chartTriggered = true;
        }

        private async Task updateChart()
        {
            List<string> arrayLabels = new();
            List<string> arrayStringValues = new();
            List<int> arrayIntValues = new();
            List<decimal> arrayDecimalValues = new();

            if (previewTopLocationReportStatistics != null)
            {
                previewTopLocationReportStatistics.ForEach(x =>
                {
                    if (x.LocationID.Equals("HO"))
                    {
                        arrayLabels.Add("HEAD OFFICE");
                    }
                    else
                    {
                        var loc = ManagementService.locations.SingleOrDefault(y => y.locationId.Equals(x.LocationID));

                        if (loc != null)
                            arrayLabels.Add(loc.locationName);
                        else
                            arrayLabels.Add(x.LocationID);
                    }

                    arrayIntValues.Add(x.TotalDocuments);
                });

                await _jsModule.InvokeVoidAsync(
                    "updateBarChart",
                    topLocationReportStatsRef,
                    "topLocationReportStats",
                    arrayLabels,
                    arrayIntValues,
                    "10 Lokasi dengan Pelaporan Terbanyak",
                    "# of Reports"
                );
            }

            arrayLabels.Clear();
            arrayStringValues.Clear();
            arrayIntValues.Clear();
            arrayDecimalValues.Clear();

            if (previewItemCaseCategoryStatistics != null)
            {
                previewItemCaseCategoryStatistics.ForEach(x =>
                {
                    var riskCat = EPKRSService.itemRiskCategories.SingleOrDefault(y => y.ItemRiskCategoryID.Equals(x.ItemRiskCategoryID));

                    if (riskCat != null)
                        arrayLabels.Add(riskCat.CategoryDescription);
                    else
                        arrayLabels.Add(x.ItemRiskCategoryID);

                    arrayIntValues.Add(x.TotalItemQty);
                });

                await _jsModule.InvokeVoidAsync(
                    "updateDoughnutChart",
                    itemCaseCategoryStatsRef,
                    "itemCaseCategoryStats",
                    arrayLabels,
                    arrayIntValues,
                    "Total Qty Barang Bermasalah berdasarkan Tipe",
                    "# of Reports"
                );
            }

            arrayLabels.Clear();
            arrayStringValues.Clear();
            arrayIntValues.Clear();
            arrayDecimalValues.Clear();

            if (previewItemCaseCategoryStatistics != null)
            {
                previewItemCaseCategoryStatistics.ForEach(x =>
                {
                    var riskCat = EPKRSService.itemRiskCategories.SingleOrDefault(y => y.ItemRiskCategoryID.Equals(x.ItemRiskCategoryID));

                    if (riskCat != null)
                        arrayLabels.Add(riskCat.CategoryDescription);
                    else
                        arrayLabels.Add(x.ItemRiskCategoryID);

                    arrayDecimalValues.Add(x.TotalItemValue);
                });

                await _jsModule.InvokeVoidAsync(
                    "updateBarChart",
                    itemCaseValueStatsRef,
                    "itemCaseValueStats",
                    arrayLabels,
                    arrayDecimalValues,
                    "Total Value Barang Bermasalah berdasarkan Tipe",
                    "Loss Estimation"
                );
            }

            arrayLabels.Clear();
            arrayStringValues.Clear();
            arrayIntValues.Clear();
            arrayDecimalValues.Clear();

            if (previewItemCategoryStatistics != null)
            {
                previewItemCategoryStatistics.ForEach(x =>
                {
                    var cat = ManagementService.categories.SingleOrDefault(y => y.CategoryID.Equals(x.CategoryID));

                    if (cat != null)
                        arrayLabels.Add(cat.CategoryDescription);
                    else
                        arrayLabels.Add(x.CategoryID);

                    arrayIntValues.Add(x.TotalDocuments);
                });

                await _jsModule.InvokeVoidAsync(
                    "updateBarChart",
                    itemCategoryStatsRef,
                    "itemCategoryStats",
                    arrayLabels,
                    arrayIntValues,
                    "Total Pelaporan berdasarkan Kategori Barang",
                    "# of Reports"
                );
            }
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

        private async Task HandleDownloadDocument(Byte[] content, string filename)
        {
            var filestream = GetFileStream(content);

            using var streamRef = new DotNetStreamReference(stream: filestream);

            await _jsModule.InvokeVoidAsync("exportStream", filename, streamRef);
        }

        private async void UploadHandleSelection(InputFileChangeEventArgs files, string which)
        {
            fileDiscussionUpload.Clear();
            fileReplyDiscussionUpload.Clear();

            if (which.Equals("New"))
            {
                listFileUpload = files.GetMultipleFiles();
            }
            else if (which.Equals("Reply"))
            {
                listReplyFileUpload = files.GetMultipleFiles();
            }
            else
            {
                throw new Exception("Upload Handle Selection parameter \'which\' on EPKRSDashboard is not Relevant");
            }

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
                if (which.Equals("New"))
                {
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

                                fileDiscussionUpload.Add(new BPIWebApplication.Shared.MainModel.Stream.FileStream
                                {
                                    type = "Discussion",
                                    fileName = System.IO.Path.GetRandomFileName() + "!_!" + fi.Name,
                                    fileDesc = "",
                                    fileType = ext,
                                    fileSize = 0,
                                    content = ms.ToArray()
                                });

                                stream.Close();
                            }
                        }
                    }
                }
                else if (which.Equals("Reply"))
                {
                    if (listReplyFileUpload != null)
                    {
                        foreach (var file in listReplyFileUpload)
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

                                fileReplyDiscussionUpload.Add(new BPIWebApplication.Shared.MainModel.Stream.FileStream
                                {
                                    type = "Discussion",
                                    fileName = System.IO.Path.GetRandomFileName() + "!_!" + fi.Name,
                                    fileDesc = "",
                                    fileType = ext,
                                    fileSize = 0,
                                    content = ms.ToArray()
                                });

                                stream.Close();
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception("Upload Handle Selection parameter \'which\' on EPKRSDashboard is not Relevant");
                }
                
            }

            this.StateHasChanged();
        }

        private async Task setCustomEmailDocReminder(string typeAction)
        {
            try
            {
                customMailData.from = activeUser.userName;
                customMailData.moduleName = "EPKRS";
                customMailData.actionName = typeAction;
                customMailData.locationId = "ALL";

                StateHasChanged();
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task setItemCasePreviewData(EPKRSUploadItemCase data)
        {
            try
            {
                previewItemCase = data.itemCase;
                previewItemLine = data.itemLine;
                previewCaseAttachment = data.attachment;
                previewDocumentListApproval = data.Approval;

                updateItemCase = data.itemCase;

                EPKRSService.fileStreams.Clear();
                #pragma warning disable CS4014 
                Task.Run(async () =>
                {
                    await EPKRSService.getEPKRSFileStream(CommonLibrary.Base64Encode(previewItemCase.DocumentID));
                    StateHasChanged();
                });
                #pragma warning restore CS4014

                //if (!res.isSuccess)
                //{
                //    await _jsModule.InvokeVoidAsync("showAlert", $"Failed : {res.ErrorCode} - {res.ErrorMessage} !");
                //}

                previousPreviewedDocumentID = previewItemCase.DocumentID;
                previousPreviewedDocumentLocation = previewItemCase.SiteReporter;
                
                StateHasChanged();
            } 
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task setIncidentAccidentPreviewData(EPKRSUploadIncidentAccident data)
        {
            try
            {
                previewIncidentAccident = data.incidentAccident;
                previewCaseAttachment = data.attachment;
                previewDocumentListApproval = data.Approval;
                previewIncidentAccidentInvolver = data.Involver;

                updateIncidentAccident = data.incidentAccident;

                EPKRSService.fileStreams.Clear();
                #pragma warning disable CS4014
                Task.Run(async () =>
                {
                    await EPKRSService.getEPKRSFileStream(CommonLibrary.Base64Encode(previewIncidentAccident.DocumentID));
                    StateHasChanged();
                });
                #pragma warning restore CS4014

                //if (!res.isSuccess)
                //{
                //    await _jsModule.InvokeVoidAsync("showAlert", $"Failed : {res.ErrorCode} - {res.ErrorMessage} !");
                //}

                previousPreviewedDocumentID = previewIncidentAccident.DocumentID;
                previousPreviewedDocumentLocation = previewIncidentAccident.SiteReporter;
                
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task setDocumentDiscussionPreviewData(string DocumentID, string LocationID)
        {
            try
            {
                previewDocumentDiscussion.Clear();
                string temp = LocationID + "!_!" + DocumentID;

                var res = await EPKRSService.getEPKRSDocumentDiscussion(CommonLibrary.Base64Encode(temp));

                if (res.isSuccess)
                {
                    res.Data.ForEach(x =>
                    {
                        previewDocumentDiscussion.Add(new DocumentDiscussionForm
                        {
                            formId = CommonLibrary.RandomString(7, "ALPHA"),
                            rowGuid = x.rowGuid,
                            DocumentID = x.DocumentID,
                            UserName = x.UserName,
                            CommentDate = x.CommentDate,
                            Comment = x.Comment,
                            isEdited = x.isEdited,
                            ReplyRowGuid = x.ReplyRowGuid
                        });
                    });

                    #pragma warning disable CS4014
                    Task.Run(async () => {
                        EPKRSService.fileStreams.Clear();
                        await EPKRSService.getEPKRSFileStream(CommonLibrary.Base64Encode(DocumentID));
                        StateHasChanged();
                    });
                    #pragma warning restore CS4014

                    var dt = EPKRSService.documentDiscussions.ExceptBy(EPKRSService.documentDiscussionReadHistories.Select(f => f.rowGuid), g => g.rowGuid).Where(h => h.DocumentID.Equals(DocumentID)).ToList();

                    if (dt.Count > 0)
                    {
                        List<DocumentDiscussionReadHistory> param = new();
                        QueryModel<DocumentDiscussionReadStream> readHistoryParam = new();
                        readHistoryParam.Data = new();
                        readHistoryParam.Data.Data = new();

                        dt.ForEach(x =>
                        {
                            param.Add(new DocumentDiscussionReadHistory
                            {
                                rowGuid = x.rowGuid,
                                DocumentID = x.DocumentID,
                                UserName = activeUser.userName,
                                ReadDate = DateTime.Now
                            });
                        });

                        readHistoryParam.Data.LocationID = LocationID;
                        readHistoryParam.Data.Data = param;
                        readHistoryParam.userEmail = activeUser.userName;
                        readHistoryParam.userAction = "I";
                        readHistoryParam.userActionDate = DateTime.Now;

                        #pragma warning disable CS4014
                        Task.Run(async () => {
                            var res = await EPKRSService.createEPKRSDocumentDiscussionReadHistory(readHistoryParam);

                            if (res.isSuccess)
                                EPKRSService.documentDiscussionReadHistories.AddRange(param);

                            StateHasChanged();
                        });
                        #pragma warning restore CS4014
                    }
                }
                else 
                {
                    if (res.ErrorCode.Equals("99"))
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", $"Failed : {res.ErrorCode} - {res.ErrorMessage} !");
                    }
                }

                previousPreviewedDocumentID = DocumentID;
                previousPreviewedDocumentLocation = LocationID;

                StateHasChanged();
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task setDocumentApprovalData(string approveType)
        {
            try
            {
                previewDocumentApproval.DocumentID = previousPreviewedDocumentID;
                previewDocumentApproval.ApprovalAction = approveType;
                previewDocumentApproval.Approver = activeUser.userName;
                previewDocumentApproval.ApproveDate = DateTime.Now;

                StateHasChanged();
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task createDocumentDiscussion()
        {
            try
            {
                if (uploadDocumentDiscussion.Comment.Length > 0)
                {
                    isLoading = true;

                    DocumentDiscussionStream uploadData = new();
                    uploadData.mainData = new();
                    uploadData.files = new();

                    QueryModel<EPKRSUploadDiscussion> discussionData = new();
                    discussionData.Data = new();
                    discussionData.Data.discussion = new();
                    discussionData.Data.attachment = new();

                    discussionData.Data.discussion.rowGuid = "";
                    discussionData.Data.discussion.DocumentID = previousPreviewedDocumentID;
                    discussionData.Data.discussion.UserName = activeUser.userName;
                    discussionData.Data.discussion.Comment = uploadDocumentDiscussion.Comment;
                    discussionData.Data.discussion.CommentDate = DateTime.Now;
                    discussionData.Data.discussion.isEdited = false;
                    discussionData.Data.discussion.ReplyRowGuid = "";

                    if (fileDiscussionUpload.Count > 0)
                    {
                        int n = 0;

                        if (checkPreviewCaseAttachment())
                        {
                            n = EPKRSService.fileStreams.Count;
                        }

                        fileDiscussionUpload.ForEach(x =>
                        {
                            n++;

                            discussionData.Data.attachment.Add(new CaseAttachment
                            {
                                DocumentID = previousPreviewedDocumentID,
                                LineNum = n,
                                UploadDate = DateTime.Now,
                                FileExtension = x.fileType,
                                FilePath = x.fileName
                            });
                        });
                    }

                    discussionData.userEmail = activeUser.userName; ;
                    discussionData.userAction = "I";
                    discussionData.userActionDate = DateTime.Now;

                    discussionData.Data.LocationID = previousPreviewedDocumentLocation;

                    uploadData.mainData = discussionData;
                    uploadData.files = fileDiscussionUpload;

                    var res = await EPKRSService.createEPKRSDocumentDiscussion(uploadData);

                    if (res.isSuccess)
                    {
                        await setDocumentDiscussionPreviewData(previousPreviewedDocumentID, previousPreviewedDocumentLocation);
                        previewCaseAttachment.AddRange(discussionData.Data.attachment);

                        List<DocumentDiscussionReadHistory> param = new();
                        QueryModel<DocumentDiscussionReadStream> readHistoryParam = new();
                        readHistoryParam.Data = new();
                        readHistoryParam.Data.Data = new();

                        param.Add(new DocumentDiscussionReadHistory
                        {
                            rowGuid = res.Data.mainData.Data.discussion.rowGuid,
                            DocumentID = res.Data.mainData.Data.discussion.DocumentID,
                            UserName = activeUser.userName,
                            ReadDate = DateTime.Now
                        });

                        readHistoryParam.Data.LocationID = previousPreviewedDocumentLocation;
                        readHistoryParam.Data.Data = param;
                        readHistoryParam.userEmail = activeUser.userName;
                        readHistoryParam.userAction = "I";
                        readHistoryParam.userActionDate = DateTime.Now;
                        
                        #pragma warning disable CS4014
                        Task.Run(async () => {
                            var res = await EPKRSService.createEPKRSDocumentDiscussionReadHistory(readHistoryParam);

                            if (res.isSuccess)
                                EPKRSService.documentDiscussionReadHistories.AddRange(param);

                            StateHasChanged();
                        });
                        #pragma warning restore CS4014

                        uploadDocumentDiscussion.Comment = "";
                    }
                    else
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", $"Failed : {res.ErrorCode} - {res.ErrorMessage} !");
                    }

                    fileDiscussionUpload.Clear();
                    isLoading = false;
                    StateHasChanged();
                }
                else
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "Input Comment !");
                }
            }
            catch (Exception ex)
            {
                isLoading = false;
                StateHasChanged();
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task replyDiscussion(DocumentDiscussionForm data)
        {
            try
            {
                if (uploadReplyDocumentDiscussion.Comment.Length > 0)
                {
                    isLoading = true;

                    DocumentDiscussionStream uploadData = new();
                    uploadData.mainData = new();
                    uploadData.files = new();

                    QueryModel<EPKRSUploadDiscussion> discussionData = new();
                    discussionData.Data = new();
                    discussionData.Data.discussion = new();
                    discussionData.Data.attachment = new();

                    discussionData.Data.discussion.rowGuid = "";
                    discussionData.Data.discussion.DocumentID = previousPreviewedDocumentID;
                    discussionData.Data.discussion.UserName = activeUser.userName;
                    discussionData.Data.discussion.Comment = uploadReplyDocumentDiscussion.Comment;
                    discussionData.Data.discussion.CommentDate = DateTime.Now;
                    discussionData.Data.discussion.isEdited = false;
                    discussionData.Data.discussion.ReplyRowGuid = data.rowGuid;

                    if (fileReplyDiscussionUpload.Count > 0)
                    {
                        int n = 0;

                        if (checkPreviewCaseAttachment())
                        {
                            n = EPKRSService.fileStreams.Count;
                        }

                        fileReplyDiscussionUpload.ForEach(x =>
                        {
                            n++;

                            discussionData.Data.attachment.Add(new CaseAttachment
                            {
                                DocumentID = previousPreviewedDocumentID,
                                LineNum = n,
                                UploadDate = DateTime.Now,
                                FileExtension = x.fileType,
                                FilePath = x.fileName
                            });
                        });
                    }

                    discussionData.userEmail = activeUser.userName; ;
                    discussionData.userAction = "I";
                    discussionData.userActionDate = DateTime.Now;

                    discussionData.Data.LocationID = previousPreviewedDocumentLocation;

                    uploadData.mainData = discussionData;
                    uploadData.files = fileReplyDiscussionUpload;

                    var res = await EPKRSService.createEPKRSDocumentDiscussion(uploadData);

                    if (res.isSuccess)
                    {
                        await setDocumentDiscussionPreviewData(previousPreviewedDocumentID, previousPreviewedDocumentLocation);
                        previewCaseAttachment.AddRange(discussionData.Data.attachment);

                        List<DocumentDiscussionReadHistory> param = new();
                        QueryModel<DocumentDiscussionReadStream> readHistoryParam = new();
                        readHistoryParam.Data = new();
                        readHistoryParam.Data.Data = new();

                        param.Add(new DocumentDiscussionReadHistory
                        {
                            rowGuid = res.Data.mainData.Data.discussion.rowGuid,
                            DocumentID = res.Data.mainData.Data.discussion.DocumentID,
                            UserName = activeUser.userName,
                            ReadDate = DateTime.Now
                        });

                        readHistoryParam.Data.LocationID = previousPreviewedDocumentLocation;
                        readHistoryParam.Data.Data = param;
                        readHistoryParam.userEmail = activeUser.userName;
                        readHistoryParam.userAction = "I";
                        readHistoryParam.userActionDate = DateTime.Now;

                        #pragma warning disable CS4014
                        Task.Run(async () => {
                            var res = await EPKRSService.createEPKRSDocumentDiscussionReadHistory(readHistoryParam);

                            if (res.isSuccess)
                                EPKRSService.documentDiscussionReadHistories.AddRange(param);

                            StateHasChanged();
                        });
                        #pragma warning restore CS4014

                        uploadReplyDocumentDiscussion.Comment = "";
                    }
                    else
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", $"Failed : {res.ErrorCode} - {res.ErrorMessage} !");
                    }

                    fileReplyDiscussionUpload.Clear();
                    isLoading = false;
                    StateHasChanged();
                }
                else
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "Input Comment !");
                }
            }
            catch (Exception ex)
            {
                isLoading = false;
                StateHasChanged();
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task createDocumentApprove(string approveType)
        {
            try
            {
                isLoading = true;

                QueryModel<DocumentApproval> uploadData = new();
                uploadData.Data = new();

                uploadData.Data = previewDocumentApproval;
                uploadData.userEmail = activeUser.userName;
                uploadData.userAction = "I";
                uploadData.userActionDate = DateTime.Now;

                var res = await EPKRSService.createEPKRSDocumentApprovalData(uploadData);

                if (res.isSuccess)
                {
                    string param = approveType.Equals("Approve") ? "Approved" : approveType.Equals("OnProgress") ? "OnProgress" : approveType.Equals("Close") ? "Closed" : "NAN";

                    var a = EPKRSService.itemCases.SingleOrDefault(x => x.itemCase.DocumentID.Equals(res.Data.Data.DocumentID));

                    if (a != null)
                    {
                        previewDocumentListApproval.Add(previewDocumentApproval);
                        a.itemCase.DocumentStatus = param;
                    }

                    var b = EPKRSService.incidentAccidents.SingleOrDefault(x => x.incidentAccident.DocumentID.Equals(res.Data.Data.DocumentID));

                    if (b != null)
                    {
                        previewDocumentListApproval.Add(previewDocumentApproval);
                        b.incidentAccident.DocumentStatus = param;
                    }

                    await _jsModule.InvokeVoidAsync("showAlert", "Update Status Success !");
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

        private async Task createDocumentApproveExtended(string approveType)
        {
            try
            {
                isLoading = true;

                QueryModel<RISKApprovalExtended> uploadData = new();
                uploadData.Data = new();
                uploadData.Data.approval = new();
                uploadData.Data.extendedData = new();
                uploadData.Data.involver = new();

                uploadData.Data.approval = previewDocumentApproval;

                ReportingExtended temp = new();
                temp.DocumentID = previewDocumentApproval.DocumentID;

                if (EPKRSService.itemCases.SingleOrDefault(x => x.itemCase.DocumentID.Equals(previewDocumentApproval.DocumentID)) != null)
                {
                    uploadData.Data.reportingType = "ITEMC";

                    temp.ExtendedRootCause = "";
                    temp.ExtendedMitigationPlan = updateItemCase.ExtendedMitigationPlan;
                }

                if (EPKRSService.incidentAccidents.SingleOrDefault(x => x.incidentAccident.DocumentID.Equals(previewDocumentApproval.DocumentID)) != null)
                {
                    uploadData.Data.reportingType = "INCAC";

                    temp.ExtendedRootCause = updateIncidentAccident.ExtendedRootCause;
                    temp.ExtendedMitigationPlan = updateIncidentAccident.ExtendedMitigationPlan;
                }

                uploadData.Data.extendedData = temp;

                if (partyInvolved.Count > 0)
                {
                    uploadData.Data.involver = partyInvolved;
                    uploadData.Data.involver.ForEach(x =>
                    {
                        x.DocumentID = previewDocumentApproval.DocumentID;
                    });
                }

                uploadData.userEmail = activeUser.userName;
                uploadData.userAction = "I";
                uploadData.userActionDate = DateTime.Now;

                var res = await EPKRSService.createEPKRSDocumentApprovalExtendedData(uploadData);

                if (res.isSuccess)
                {
                    string param = approveType.Equals("Approve") ? "Approved" : approveType.Equals("OnProgress") ? "OnProgress" : approveType.Equals("Close") ? "Closed" : "NAN";

                    var a = EPKRSService.itemCases.SingleOrDefault(x => x.itemCase.DocumentID.Equals(res.Data.Data.approval.DocumentID));

                    if (a != null)
                    {
                        if (param.Equals("OnProgress"))
                            a.itemCase.ExtendedMitigationPlan = updateItemCase.ExtendedMitigationPlan;

                        previewDocumentListApproval.Add(previewDocumentApproval);
                        a.itemCase.DocumentStatus = param;
                    }

                    var b = EPKRSService.incidentAccidents.SingleOrDefault(x => x.incidentAccident.DocumentID.Equals(res.Data.Data.approval.DocumentID));

                    if (b != null)
                    {
                        if (param.Equals("OnProgress"))
                        {
                            b.incidentAccident.ExtendedRootCause = updateIncidentAccident.ExtendedRootCause;
                            b.incidentAccident.ExtendedMitigationPlan = updateIncidentAccident.ExtendedMitigationPlan;
                        }

                        previewDocumentListApproval.Add(previewDocumentApproval);
                        b.incidentAccident.DocumentStatus = param;
                    }


                    await _jsModule.InvokeVoidAsync("showAlert", "Update Status Success !");
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

        private async Task deleteEPKRSDocument(string docType)
        {
            try
            {
                string temp = previousPreviewedDocumentID  + "!_!" + previousPreviewedDocumentLocation;
                
                QueryModel<string> paramData = new();
                paramData.Data = CommonLibrary.Base64Encode(temp);
                paramData.userEmail = activeUser.userName;
                paramData.userAction = "D";
                paramData.userActionDate = DateTime.Now;

                if (docType.Equals("ITCASE"))
                {
                    var res = await EPKRSService.deleteEPKRSItemCaseDocumentData(paramData);

                    if (res.isSuccess)
                    {
                        previewItemCase.DocumentStatus = "DELETED";
                        var a = EPKRSService.itemCases.SingleOrDefault(x => x.itemCase.DocumentID.Equals(res.Data.Data.Split("!_!")[0]));

                        if (a != null)
                            EPKRSService.itemCases.SingleOrDefault(x => x.itemCase.DocumentID.Equals(res.Data.Data.Split("!_!")[0])).itemCase.DocumentStatus = "DELETED";

                        await _jsModule.InvokeVoidAsync("showAlert", "Delete Document Success, Please REFRESH Your Page !");
                    }
                    else
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", $"Failed : {res.ErrorCode} - {res.ErrorMessage} !");
                    }
                }
                else if (docType.Equals("INCAC"))
                {
                    previewIncidentAccident.DocumentStatus = "DELETED";
                    var res = await EPKRSService.deleteEPKRSIncidentAccidentDocumentData(paramData);

                    if (res.isSuccess)
                    {
                        var b = EPKRSService.incidentAccidents.SingleOrDefault(x => x.incidentAccident.DocumentID.Equals(res.Data.Data.Split("!_!")[0]));

                        if (b != null)
                            EPKRSService.incidentAccidents.SingleOrDefault(x => x.incidentAccident.DocumentID.Equals(res.Data.Data.Split("!_!")[0])).incidentAccident.DocumentStatus = "DELETED";

                        await _jsModule.InvokeVoidAsync("showAlert", "Delete Document Success, Please REFRESH Your Page !");
                    }
                    else
                    {
                        await _jsModule.InvokeVoidAsync("showAlert", $"Failed : {res.ErrorCode} - {res.ErrorMessage} !");
                    }
                }
                else
                {
                    throw new Exception("Delete Document : docType parameter isn\'t correctly supplied !");
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

        private async Task itemCasePageSelect(int currPage)
        {
            itemCasePageActive = currPage;
            isLoading = true;
            triggerReplyButton = false;
            QueryModel<string> param = new();

            if (itemCaseFilterActive)
            {
                if (itemCaseFilterType.Equals("ID"))
                {
                    string paramGetItemCaseInitialization = activeUser.location + $"!_!WHERE a.DocumentID LIKE \'%{itemCaseFilterValue}%\'!_!" + itemCasePageActive.ToString();
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(paramGetItemCaseInitialization);
                    await EPKRSService.getEPKRSItemCaseData(param);
                }
                else if (itemCaseFilterType.Equals("LOC"))
                {
                    string paramGetItemCaseInitialization = activeUser.location + $"!_!WHERE a.SiteReporter = \'{itemCaseFilterValue}\'!_!" + itemCasePageActive.ToString();
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(paramGetItemCaseInitialization);
                    await EPKRSService.getEPKRSItemCaseData(param);
                }
                else if (itemCaseFilterType.Equals("STATUS"))
                {
                    string paramGetItemCaseInitialization = activeUser.location + $"!_!WHERE a.DocumentStatus LIKE \'%{itemCaseFilterValue}%\'!_!" + itemCasePageActive.ToString();
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(paramGetItemCaseInitialization);
                    await EPKRSService.getEPKRSItemCaseData(param);
                }
            }
            else
            {
                string paramGetItemCaseInitialization = activeUser.location + "!_!!_!" + itemCasePageActive.ToString();
                param = new();
                param.Data = CommonLibrary.Base64Encode(paramGetItemCaseInitialization);
                await EPKRSService.getEPKRSItemCaseData(param);
            }

            List<DocumentListParams> docParams = new();

            if (EPKRSService.itemCases != null)
            {
                if (EPKRSService.itemCases.Count > 0)
                {
                    EPKRSService.itemCases.ForEach(x =>
                    {
                        docParams.Add(new DocumentListParams
                        {
                            LocationID = x.itemCase.SiteReporter,
                            DocumentID = x.itemCase.DocumentID
                        });
                    });
                }
            }

            if (EPKRSService.incidentAccidents != null)
            {
                if (EPKRSService.incidentAccidents.Count > 0)
                {
                    EPKRSService.incidentAccidents.ForEach(x =>
                    {
                        docParams.Add(new DocumentListParams
                        {
                            LocationID = x.incidentAccident.SiteReporter,
                            DocumentID = x.incidentAccident.DocumentID
                        });
                    });
                }
            }

            Task.Run(async () =>
            {
                await EPKRSService.getEPKRSInitializationDocumentDiscussions(docParams);
                await EPKRSService.getEPKRSDocumentDiscussionReadHistory(docParams);

                StateHasChanged();
            });

            isLoading = false;
            StateHasChanged();
        }

        private async Task incidentAccidentPageSelect(int currPage)
        {
            incidentAccidentPageActive = currPage;
            isLoading = true;
            triggerReplyButton = false;
            QueryModel<string> param = new();

            if (incidentAccidentFilterActive)
            {
                if (incidentAccidentFilterType.Equals("ID"))
                {
                    string paramGetIncidentAccidentInitialization = activeUser.location + $"!_!WHERE a.DocumentID LIKE \'%{incidentAccidentFilterValue}%\'!_!" + incidentAccidentPageActive.ToString();
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(paramGetIncidentAccidentInitialization);
                    await EPKRSService.getEPKRSIncidentAccidentData(param);
                }
                else if (incidentAccidentFilterType.Equals("LOC"))
                {
                    string paramGetIncidentAccidentInitialization = activeUser.location + $"!_!WHERE a.SiteReporter = \'{incidentAccidentFilterValue}\'!_!" + incidentAccidentPageActive.ToString();
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(paramGetIncidentAccidentInitialization);
                    await EPKRSService.getEPKRSIncidentAccidentData(param);
                }
                else if (incidentAccidentFilterType.Equals("STATUS"))
                {
                    string paramGetIncidentAccidentInitialization = activeUser.location + $"!_!WHERE a.DocumentStatus LIKE \'%{incidentAccidentFilterValue}%\'!_!" + incidentAccidentPageActive.ToString();
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(paramGetIncidentAccidentInitialization);
                    await EPKRSService.getEPKRSIncidentAccidentData(param);
                }
            }
            else
            {
                string paramGetIncidentAccidentInitialization = activeUser.location + "!_!!_!" + incidentAccidentPageActive.ToString();
                param = new();
                param.Data = CommonLibrary.Base64Encode(paramGetIncidentAccidentInitialization);
                await EPKRSService.getEPKRSIncidentAccidentData(param);
            }

            List<DocumentListParams> docParams = new();

            if (EPKRSService.itemCases != null)
            {
                if (EPKRSService.itemCases.Count > 0)
                {
                    EPKRSService.itemCases.ForEach(x =>
                    {
                        docParams.Add(new DocumentListParams
                        {
                            LocationID = x.itemCase.SiteReporter,
                            DocumentID = x.itemCase.DocumentID
                        });
                    });
                }
            }

            if (EPKRSService.incidentAccidents != null)
            {
                if (EPKRSService.incidentAccidents.Count > 0)
                {
                    EPKRSService.incidentAccidents.ForEach(x =>
                    {
                        docParams.Add(new DocumentListParams
                        {
                            LocationID = x.incidentAccident.SiteReporter,
                            DocumentID = x.incidentAccident.DocumentID
                        });
                    });
                }
            }

            Task.Run(async () =>
            {
                await EPKRSService.getEPKRSInitializationDocumentDiscussions(docParams);
                await EPKRSService.getEPKRSDocumentDiscussionReadHistory(docParams);

                StateHasChanged();
            });

            isLoading = false;
            StateHasChanged();
        }

        private async Task itemCaseFilter()
        {
            if (itemCaseFilterType.Length > 0)
            {
                itemCaseFilterActive = true;
                isLoading = true;
                EPKRSService.itemCases.Clear();
                itemCasePageActive = 1;
                QueryModel<string> param = new();

                if (itemCaseFilterType.Equals("ID"))
                {
                    string itemCaseParampz = $"[EPKRS].ItemCase!_!{activeUser.location}!_!WHERE DocumentID LIKE '%{itemCaseFilterValue}%'";
                    param.Data = CommonLibrary.Base64Encode(itemCaseParampz);
                    var itemCasepz = await EPKRSService.getEPKRSModuleNumberOfPage(param);
                    incidentAccidentNumberofPages = itemCasepz.Data;

                    string paramGetItemCaseInitialization = activeUser.location + $"!_!WHERE a.DocumentID LIKE \'%{itemCaseFilterValue}%\'!_!1";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(paramGetItemCaseInitialization);
                    await EPKRSService.getEPKRSItemCaseData(param);
                }
                else if (itemCaseFilterType.Equals("LOC"))
                {
                    string itemCaseParampz = $"[EPKRS].ItemCase!_!{activeUser.location}!_!WHERE SiteReporter = \'{itemCaseFilterValue}\'";
                    param.Data = CommonLibrary.Base64Encode(itemCaseParampz);
                    var itemCasepz = await EPKRSService.getEPKRSModuleNumberOfPage(param);
                    incidentAccidentNumberofPages = itemCasepz.Data;

                    string paramGetItemCaseInitialization = activeUser.location + $"!_!WHERE a.SiteReporter = \'{itemCaseFilterValue}\'!_!1";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(paramGetItemCaseInitialization);
                    await EPKRSService.getEPKRSItemCaseData(param);
                }
                else if (itemCaseFilterType.Equals("STATUS"))
                {
                    string itemCaseParampz = $"[EPKRS].ItemCase!_!{activeUser.location}!_!WHERE DocumentStatus LIKE \'%{itemCaseFilterValue}%\'";
                    param.Data = CommonLibrary.Base64Encode(itemCaseParampz);
                    var itemCasepz = await EPKRSService.getEPKRSModuleNumberOfPage(param);
                    incidentAccidentNumberofPages = itemCasepz.Data;

                    string paramGetItemCaseInitialization = activeUser.location + $"!_!WHERE a.DocumentStatus LIKE \'%{itemCaseFilterValue}%\'!_!1";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(paramGetItemCaseInitialization);
                    await EPKRSService.getEPKRSItemCaseData(param);
                }

                List<DocumentListParams> docParams = new();

                if (EPKRSService.itemCases != null)
                {
                    if (EPKRSService.itemCases.Count > 0)
                    {
                        EPKRSService.itemCases.ForEach(x =>
                        {
                            docParams.Add(new DocumentListParams
                            {
                                LocationID = x.itemCase.SiteReporter,
                                DocumentID = x.itemCase.DocumentID
                            });
                        });
                    }
                }

                if (EPKRSService.incidentAccidents != null)
                {
                    if (EPKRSService.incidentAccidents.Count > 0)
                    {
                        EPKRSService.incidentAccidents.ForEach(x =>
                        {
                            docParams.Add(new DocumentListParams
                            {
                                LocationID = x.incidentAccident.SiteReporter,
                                DocumentID = x.incidentAccident.DocumentID
                            });
                        });
                    }
                }

                Task.Run(async () =>
                {
                    await EPKRSService.getEPKRSInitializationDocumentDiscussions(docParams);
                    await EPKRSService.getEPKRSDocumentDiscussionReadHistory(docParams);

                    StateHasChanged();
                });

                isLoading = false;
                StateHasChanged();
            }
            else
            {
                await _jsModule.InvokeVoidAsync("showAlert", "Please Select Filter Type !");
            }
        }

        private async Task incidentAccidentFilter()
        {
            if (incidentAccidentFilterType.Length > 0)
            {
                incidentAccidentFilterActive = true;
                isLoading = true;
                EPKRSService.incidentAccidents.Clear();
                incidentAccidentPageActive = 1;
                QueryModel<string> param = new();

                if (incidentAccidentFilterType.Equals("ID"))
                {
                    string incidentAccidentParampz = $"[EPKRS].IncidentAccident!_!{activeUser.location}!_!WHERE DocumentID LIKE '%{incidentAccidentFilterValue}%'";
                    param.Data = CommonLibrary.Base64Encode(incidentAccidentParampz);
                    var incidentAccidentpz = await EPKRSService.getEPKRSModuleNumberOfPage(param);
                    incidentAccidentNumberofPages = incidentAccidentpz.Data;

                    string paramGetIncidentAccidentInitialization = activeUser.location + $"!_!WHERE a.DocumentID LIKE \'%{incidentAccidentFilterValue}%\'!_!1";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(paramGetIncidentAccidentInitialization);
                    await EPKRSService.getEPKRSIncidentAccidentData(param);
                }
                else if (incidentAccidentFilterType.Equals("LOC"))
                {
                    string incidentAccidentParampz = $"[EPKRS].IncidentAccident!_!{activeUser.location}!_!WHERE SiteReporter = \'{incidentAccidentFilterValue}\'";
                    param.Data = CommonLibrary.Base64Encode(incidentAccidentParampz);
                    var incidentAccidentpz = await EPKRSService.getEPKRSModuleNumberOfPage(param);
                    incidentAccidentNumberofPages = incidentAccidentpz.Data;

                    string paramGetIncidentAccidentInitialization = activeUser.location + $"!_!WHERE a.SiteReporter = \'{incidentAccidentFilterValue}\'!_!1";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(paramGetIncidentAccidentInitialization);
                    await EPKRSService.getEPKRSIncidentAccidentData(param);
                }
                else if (incidentAccidentFilterType.Equals("STATUS"))
                {
                    string incidentAccidentParampz = $"[EPKRS].IncidentAccident!_!{activeUser.location}!_!WHERE DocumentStatus LIKE \'%{incidentAccidentFilterValue}%\'";
                    param.Data = CommonLibrary.Base64Encode(incidentAccidentParampz);
                    var incidentAccidentpz = await EPKRSService.getEPKRSModuleNumberOfPage(param);
                    incidentAccidentNumberofPages = incidentAccidentpz.Data;

                    string paramGetIncidentAccidentInitialization = activeUser.location + $"!_!WHERE a.DocumentStatus LIKE \'%{incidentAccidentFilterValue}%\'!_!1";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(paramGetIncidentAccidentInitialization);
                    await EPKRSService.getEPKRSIncidentAccidentData(param);
                }

                List<DocumentListParams> docParams = new();

                if (EPKRSService.itemCases != null)
                {
                    if (EPKRSService.itemCases.Count > 0)
                    {
                        EPKRSService.itemCases.ForEach(x =>
                        {
                            docParams.Add(new DocumentListParams
                            {
                                LocationID = x.itemCase.SiteReporter,
                                DocumentID = x.itemCase.DocumentID
                            });
                        });
                    }
                }

                if (EPKRSService.incidentAccidents != null)
                {
                    if (EPKRSService.incidentAccidents.Count > 0)
                    {
                        EPKRSService.incidentAccidents.ForEach(x =>
                        {
                            docParams.Add(new DocumentListParams
                            {
                                LocationID = x.incidentAccident.SiteReporter,
                                DocumentID = x.incidentAccident.DocumentID
                            });
                        });
                    }
                }

                Task.Run(async () =>
                {
                    await EPKRSService.getEPKRSInitializationDocumentDiscussions(docParams);
                    await EPKRSService.getEPKRSDocumentDiscussionReadHistory(docParams);

                    StateHasChanged();
                });

                isLoading = false;
                StateHasChanged();
            }
            else
            {
                await _jsModule.InvokeVoidAsync("showAlert", "Please Select Filter Type !");
            }
        }

        private async Task resetItemCaseFilter()
        {
            try
            {
                isLoading = true;
                itemCasePageActive = 1;
                QueryModel<string> param = new();

                string itemCaseParampz = $"[EPKRS].ItemCase!_!{activeUser.location}!_!";
                param.Data = CommonLibrary.Base64Encode(itemCaseParampz);
                var itemCasepz = await EPKRSService.getEPKRSModuleNumberOfPage(param);
                incidentAccidentNumberofPages = itemCasepz.Data;

                string paramGetItemCaseInitialization = activeUser.location + "!_!!_!1";
                param = new();
                param.Data = CommonLibrary.Base64Encode(paramGetItemCaseInitialization);
                await EPKRSService.getEPKRSItemCaseData(param);

                List<DocumentListParams> docParams = new();

                if (EPKRSService.itemCases != null)
                {
                    if (EPKRSService.itemCases.Count > 0)
                    {
                        EPKRSService.itemCases.ForEach(x =>
                        {
                            docParams.Add(new DocumentListParams
                            {
                                LocationID = x.itemCase.SiteReporter,
                                DocumentID = x.itemCase.DocumentID
                            });
                        });
                    }
                }

                if (EPKRSService.incidentAccidents != null)
                {
                    if (EPKRSService.incidentAccidents.Count > 0)
                    {
                        EPKRSService.incidentAccidents.ForEach(x =>
                        {
                            docParams.Add(new DocumentListParams
                            {
                                LocationID = x.incidentAccident.SiteReporter,
                                DocumentID = x.incidentAccident.DocumentID
                            });
                        });
                    }
                }

                Task.Run(async () =>
                {
                    await EPKRSService.getEPKRSInitializationDocumentDiscussions(docParams);
                    await EPKRSService.getEPKRSDocumentDiscussionReadHistory(docParams);

                    StateHasChanged();
                });

                itemCaseFilterActive = false;
                isLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task resetIncidentAccidentFilter()
        {
            try
            {
                isLoading = true;
                incidentAccidentPageActive = 1;
                QueryModel<string> param = new();

                string incidentAccidentParampz = $"[EPKRS].IncidentAccident!_!{activeUser.location}!_!";
                param.Data = CommonLibrary.Base64Encode(incidentAccidentParampz);
                var incidentAccidentpz = await EPKRSService.getEPKRSModuleNumberOfPage(param);
                incidentAccidentNumberofPages = incidentAccidentpz.Data;

                string paramGetIncidentAccidentInitialization = activeUser.location + "!_!!_!1";
                param = new();
                param.Data = CommonLibrary.Base64Encode(paramGetIncidentAccidentInitialization);
                await EPKRSService.getEPKRSIncidentAccidentData(param);

                List<DocumentListParams> docParams = new();

                if (EPKRSService.itemCases != null)
                {
                    if (EPKRSService.itemCases.Count > 0)
                    {
                        EPKRSService.itemCases.ForEach(x =>
                        {
                            docParams.Add(new DocumentListParams
                            {
                                LocationID = x.itemCase.SiteReporter,
                                DocumentID = x.itemCase.DocumentID
                            });
                        });
                    }
                }

                if (EPKRSService.incidentAccidents != null)
                {
                    if (EPKRSService.incidentAccidents.Count > 0)
                    {
                        EPKRSService.incidentAccidents.ForEach(x =>
                        {
                            docParams.Add(new DocumentListParams
                            {
                                LocationID = x.incidentAccident.SiteReporter,
                                DocumentID = x.incidentAccident.DocumentID
                            });
                        });
                    }
                }

                Task.Run(async () =>
                {
                    await EPKRSService.getEPKRSInitializationDocumentDiscussions(docParams);
                    await EPKRSService.getEPKRSDocumentDiscussionReadHistory(docParams);

                    StateHasChanged();
                });

                incidentAccidentFilterActive = false;
                isLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task incacStatisticsFilterbyYear(int year)
        {
            try
            {
                selectedYearFilter = year;

                QueryModel<string> param = new();
                incacDataforStatistics = null;

                string incacDataforStatsParam = $"WHERE a.ReportDate BETWEEN \'{selectedYearFilter}-01-01 00:00:00\' AND \'{selectedYearFilter}-12-31 23:59:59\'";
                param.Data = CommonLibrary.Base64Encode(incacDataforStatsParam);
                var resIncacDataforStats = await EPKRSService.getEPKRSIncidentAccidentDataforStatistics(param);

                if (resIncacDataforStats.isSuccess)
                {
                    incacDataforStatistics = new();
                    incacDataforStatistics = resIncacDataforStats.Data;
                }

                incacRiskTypeValueFilter.Clear();
                incacRegionValueFilter.Clear();
                incacLocationValueFilter.Clear();
                incacPositionValueFilter.Clear();
                incacDeptValueFilter.Clear();

                foreach (var dt in monthSelector)
                {
                    monthSelector[dt.Key] = false;
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task incacStatisticsFilterbyMonth()
        {
            try
            {
                if (!monthSelector.Any(x => x.Value.Equals(true)))
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "Select at Least 1 Month to Filter !");
                }
                else
                {
                    QueryModel<string> param = new();
                    ResultModel<List<EPKRSIncidentAccidentforStats>> res = new();
                    res.Data = new();
                    incacDataforStatistics = null;

                    await Parallel.ForEachAsync(monthSelector.Where(x => x.Value.Equals(true)), new ParallelOptions { MaxDegreeOfParallelism = 4 }, async (dt, i) =>
                    {
                        List<EPKRSIncidentAccidentforStats> taskResult = new();

                        string incacDataforStatsParam = $"WHERE a.ReportDate BETWEEN \'{selectedYearFilter}-{dt.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilter}-{dt.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, dt.Key)} 23:59:59\'";
                        param.Data = CommonLibrary.Base64Encode(incacDataforStatsParam);
                        var resIncacDataforStats = await EPKRSService.getEPKRSIncidentAccidentDataforStatistics(param);

                        if (resIncacDataforStats.isSuccess)
                        {
                            res.Data.AddRange(resIncacDataforStats.Data);
                        }
                    });

                    incacDataforStatistics = new();
                    incacDataforStatistics = res.Data;

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task statsFilterbyRiskType(string filValue)
        {
            try
            {
                if (incacDataforStatistics != null)
                {
                    incacRiskTypeValueFilter.Add(filValue);

                    var filtered = incacDataforStatistics.Where(y => y.incidentAccident.RiskID.Equals(filValue)).ToList();

                    incacDataforStatistics = new();
                    incacDataforStatistics = filtered;

                    StateHasChanged();
                }
                else
                {
                    await _jsModule.InvokeVoidAsync("showAlert", $"Parameter Statistics Incident Accident Data is NULL !");
                }
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task statsFilterbyRegion(string filValue)
        {
            try
            {
                if (incacDataforStatistics != null)
                {
                    incacRegionValueFilter.Add(filValue);

                    var filtered = incacDataforStatistics.Where(y => y.incidentAccident.DORMEmail.Equals(filValue)).ToList();

                    incacDataforStatistics = new();
                    incacDataforStatistics = filtered;

                    StateHasChanged();
                }
                else
                {
                    await _jsModule.InvokeVoidAsync("showAlert", $"Parameter Statistics Incident Accident Data is NULL !");
                }
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task statsFilterbyInvolverPosition(string filValue)
        {
            try
            {
                if (incacDataforStatistics != null)
                {
                    incacPositionValueFilter.Add(filValue);

                    var filtered = incacDataforStatistics.Where(y => y.Involver.Any(x => x.InvolverPosition.Equals(filValue))).ToList();

                    incacDataforStatistics = new();
                    incacDataforStatistics = filtered;

                    StateHasChanged();
                }
                else
                {
                    await _jsModule.InvokeVoidAsync("showAlert", $"Parameter Statistics Incident Accident Data is NULL !");
                }
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task statsFilterbyInvolverDept(string filValue)
        {
            try
            {
                if (incacDataforStatistics != null)
                {
                    incacDeptValueFilter.Add(filValue);

                    var filtered = incacDataforStatistics.Where(y => y.Involver.Any(x => x.InvolverDept.Equals(filValue))).ToList();

                    incacDataforStatistics = new();
                    incacDataforStatistics = filtered;

                    StateHasChanged();
                }
                else
                {
                    await _jsModule.InvokeVoidAsync("showAlert", $"Parameter Statistics Incident Accident Data is NULL !");
                }
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task statsFilterbyLocation(string filValue)
        {
            try
            {
                if (incacDataforStatistics != null)
                {
                    incacLocationValueFilter.Add(filValue);

                    var filtered = incacDataforStatistics.Where(y => y.incidentAccident.SiteReporter.Equals(filValue)).ToList();

                    incacDataforStatistics = new();
                    incacDataforStatistics = filtered;

                    StateHasChanged();
                }
                else
                {
                    await _jsModule.InvokeVoidAsync("showAlert", $"Parameter Statistics Incident Accident Data is NULL !");
                }
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task regionStatPageSelect(int currPage)
        {
            try
            {
                regionStatPageActive = currPage;

                StateHasChanged();
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task locStatPageSelect(int currPage)
        {
            try
            {
                locStatPageActive = currPage;

                StateHasChanged();
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task posStatPageSelect(int currPage)
        {
            try
            {
                involvedPositionPageActive = currPage;

                StateHasChanged();
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task deptStatPageSelect(int currPage)
        {
            try
            {
                involvedDeptPageActive = currPage;

                StateHasChanged();
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task prevStatDocPageSelect(int currPage)
        {
            try
            {
                prevStatDocPageActive = currPage;

                StateHasChanged();
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task monitoringPanelFilterbyYear(int year)
        {
            try
            {
                //chartTriggered = false;
                dinamicMonitoringPanelDateFilter = string.Empty;
                selectedYearFilterChart = year;

                previewGeneralItemCaseStatistics = null;
                previewTopLocationReportStatistics = null;
                previewItemCaseCategoryStatistics = null;
                previewItemCategoryStatistics = null;

                QueryModel<string> param = new();

                string generalStatisticsItemcConditions = $"WHERE a.ReportDate BETWEEN \'{selectedYearFilterChart}-01-01 00:00:00\' AND \'{selectedYearFilterChart}-12-31 23:59:59\'!_!ITEMC";
                param = new();
                param.Data = CommonLibrary.Base64Encode(generalStatisticsItemcConditions);
                var resGeneralItemcStat = await EPKRSService.getEPKRSGeneralStatistics(param);

                string itemCaseCategoryConditions = $"WHERE a.AuditActionDate BETWEEN \'{year}-01-01 00:00:00\' AND \'{year}-12-31 23:59:59\'!_!AND (p.ReportDate BETWEEN \'{year}-01-01 00:00:00\' AND \'{year}-12-31 23:59:59\')";
                param.Data = CommonLibrary.Base64Encode(itemCaseCategoryConditions);
                var resItemCaseCategoryStat = await EPKRSService.getEPKRSItemCaseCategoryStatistics(param);

                param = new();
                string topLocationReportConditions = $"WHERE a.ReportDate BETWEEN \'{year}-01-01 00:00:00\' AND \'{year}-12-31 23:59:59\'!_!AND (p.ReportDate BETWEEN \'{year}-01-01 00:00:00\' AND \'{year}-12-31 23:59:59\')";
                param.Data = CommonLibrary.Base64Encode(topLocationReportConditions);
                var resTopLocationReportStat = await EPKRSService.getEPKRSTopLocationReportStatistics(param);

                param = new();
                string itemCategoryStatsConditions = $"WHERE a.AuditActionDate BETWEEN \'{year}-01-01 00:00:00\' AND \'{year}-12-31 23:59:59\'!_!AND (p.ReportDate BETWEEN \'{year}-01-01 00:00:00\' AND \'{year}-12-31 23:59:59\')";
                param.Data = CommonLibrary.Base64Encode(itemCategoryStatsConditions);
                var resItemCategoryStat = await EPKRSService.getEPKRSItemCategoriesStatistics(param);

                if (resGeneralItemcStat.isSuccess && resGeneralItemcStat.Data != null)
                {
                    previewGeneralItemCaseStatistics = new();
                    previewGeneralItemCaseStatistics = resGeneralItemcStat.Data;
                }

                if (resItemCaseCategoryStat.isSuccess && resItemCaseCategoryStat.Data != null)
                {
                    previewItemCaseCategoryStatistics = new();
                    previewItemCaseCategoryStatistics = resItemCaseCategoryStat.Data;
                }

                if (resTopLocationReportStat.isSuccess && resTopLocationReportStat.Data != null)
                {
                    previewTopLocationReportStatistics = new();
                    previewTopLocationReportStatistics = resTopLocationReportStat.Data;
                }

                if (resItemCategoryStat.isSuccess && resItemCategoryStat.Data != null)
                {
                    previewItemCategoryStatistics = new();
                    previewItemCategoryStatistics = resItemCategoryStat.Data;
                }

                foreach (var dt in monthSelectorChart)
                {
                    monthSelectorChart[dt.Key] = false;
                }

#pragma warning disable CS4014
                Task.Run(async () => {
                    await updateChart();
                    StateHasChanged();
                });
                #pragma warning restore CS4014
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task monitoringPanelFilterbyMonth()
        {
            try
            {
                if (!monthSelectorChart.Any(x => x.Value.Equals(true)))
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "Select at Least 1 Month to Filter !");
                }
                else
                {
                    //chartTriggered = false;
                    previewGeneralItemCaseStatistics = null;
                    previewTopLocationReportStatistics = null;
                    previewItemCaseCategoryStatistics = null;
                    previewItemCategoryStatistics = null;

                    dinamicMonitoringPanelDateFilter = "WHERE (";
                    dinamicMonitoringPanelDateFilterExt = "!_!AND (";

                    foreach (var selectedMonth in monthSelectorChart.Where(x => x.Value.Equals(true)))
                    {
                        dinamicMonitoringPanelDateFilter += $"(a.AuditActionDate BETWEEN \'{selectedYearFilterChart}-{selectedMonth.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilterChart}-{selectedMonth.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, selectedMonth.Key)} 23:59:59\') OR ";
                        dinamicMonitoringPanelDateFilterExt += $"(p.ReportDate BETWEEN \'{selectedYearFilterChart}-{selectedMonth.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilterChart}-{selectedMonth.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, selectedMonth.Key)} 23:59:59\') OR ";
                    }

                    QueryModel<string> param = new();

                    ResultModel<List<EPKRSDocumentStatistics>> res = new();
                    res.Data = new();

                    await Parallel.ForEachAsync(monthSelectorChart.Where(x => x.Value.Equals(true)), new ParallelOptions { MaxDegreeOfParallelism = 4 }, async (dt, i) =>
                    {
                        string generalStatisticsItemcConditions = $"WHERE a.ReportDate BETWEEN \'{selectedYearFilterChart}-{dt.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilterChart}-{dt.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, dt.Key)} 23:59:59\'!_!ITEMC";
                        param = new();
                        param.Data = CommonLibrary.Base64Encode(generalStatisticsItemcConditions);
                        var resGeneralItemcStat = await EPKRSService.getEPKRSGeneralStatistics(param);

                        if (resGeneralItemcStat.isSuccess && resGeneralItemcStat.Data != null)
                        {
                            res.Data.AddRange(resGeneralItemcStat.Data);
                        }
                    });

                    previewGeneralItemCaseStatistics = new();
                    previewGeneralItemCaseStatistics = res.Data;

                    dinamicMonitoringPanelDateFilterExt = dinamicMonitoringPanelDateFilterExt.Substring(0, dinamicMonitoringPanelDateFilterExt.Length - 4);
                    dinamicMonitoringPanelDateFilterExt += ")";
                    dinamicMonitoringPanelDateFilter = dinamicMonitoringPanelDateFilter.Substring(0, dinamicMonitoringPanelDateFilter.Length - 4);
                    dinamicMonitoringPanelDateFilter += ")";
                    param.Data = CommonLibrary.Base64Encode(dinamicMonitoringPanelDateFilter + dinamicMonitoringPanelDateFilterExt);
                    var resItemCaseCategoryStat = await EPKRSService.getEPKRSItemCaseCategoryStatistics(param);

                    dinamicMonitoringPanelDateFilter = "WHERE (";

                    foreach (var selectedMonth in monthSelectorChart.Where(x => x.Value.Equals(true)))
                    {
                        dinamicMonitoringPanelDateFilter += $"(a.ReportDate BETWEEN \'{selectedYearFilterChart}-{selectedMonth.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilterChart}-{selectedMonth.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, selectedMonth.Key)} 23:59:59\') OR ";
                    }

                    dinamicMonitoringPanelDateFilter = dinamicMonitoringPanelDateFilter.Substring(0, dinamicMonitoringPanelDateFilter.Length - 4);
                    dinamicMonitoringPanelDateFilter += ")";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(dinamicMonitoringPanelDateFilter + dinamicMonitoringPanelDateFilterExt);
                    var resTopLocationReportStat = await EPKRSService.getEPKRSTopLocationReportStatistics(param);

                    dinamicMonitoringPanelDateFilter = "WHERE (";

                    foreach (var selectedMonth in monthSelectorChart.Where(x => x.Value.Equals(true)))
                    {
                        dinamicMonitoringPanelDateFilter += $"(a.AuditActionDate BETWEEN \'{selectedYearFilterChart}-{selectedMonth.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilterChart}-{selectedMonth.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, selectedMonth.Key)} 23:59:59\') OR ";
                    }

                    dinamicMonitoringPanelDateFilter = dinamicMonitoringPanelDateFilter.Substring(0, dinamicMonitoringPanelDateFilter.Length - 4);
                    dinamicMonitoringPanelDateFilter += ")";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(dinamicMonitoringPanelDateFilter + dinamicMonitoringPanelDateFilterExt);
                    var resItemCategoryStat = await EPKRSService.getEPKRSItemCategoriesStatistics(param);

                    if (resItemCaseCategoryStat.isSuccess && resItemCaseCategoryStat.Data != null)
                    {
                        previewItemCaseCategoryStatistics = new();
                        previewItemCaseCategoryStatistics = resItemCaseCategoryStat.Data;
                    }

                    if (resTopLocationReportStat.isSuccess && resTopLocationReportStat.Data != null)
                    {
                        previewTopLocationReportStatistics = new();
                        previewTopLocationReportStatistics = resTopLocationReportStat.Data;
                    }

                    if (resItemCategoryStat.isSuccess && resItemCategoryStat.Data != null)
                    {
                        previewItemCategoryStatistics = new();
                        previewItemCategoryStatistics = resItemCategoryStat.Data;
                    }

                    #pragma warning disable CS4014
                    Task.Run(async () => {
                        await updateChart();
                        StateHasChanged();
                    });
                    #pragma warning restore CS4014

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task ExportItemCase()
        {
            try
            {
                isExportLoading = true;

                string temp = $"WHERE a.ReportDate BETWEEN \'{startDateExport.ToString("yyyy/MM/dd")} 00:00:00\' AND \'{endDateExport.ToString("yyyy/MM/dd")} 23:59:59\'!_!";

                QueryModel<string> param = new();
                param.Data = CommonLibrary.Base64Encode(temp);
                param.userEmail = activeUser.userName;
                param.userAction = "X";
                param.userActionDate = DateTime.Now;

                var res = await EPKRSService.getEPKRSItemCaseReport(param);

                if (res.isSuccess)
                {
                    await HandleDownloadDocument(res.Data.content, res.Data.fileName);
                    await _jsModule.InvokeVoidAsync("showAlert", "Export Success !");

                    resetExportDate();
                }
                else
                {
                    await _jsModule.InvokeVoidAsync("showAlert", $"Err: {res.ErrorCode} - {res.ErrorMessage} !");
                }

                isExportLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task ExportIncidentAccident()
        {
            try
            {
                isExportLoading = true;

                string temp = $"WHERE a.ReportDate BETWEEN \'{startDateExport.ToString("yyyy/MM/dd")} 00:00:00\' AND \'{endDateExport.ToString("yyyy/MM/dd")} 23:59:59\'!_!";

                QueryModel<string> param = new();
                param.Data = CommonLibrary.Base64Encode(temp);
                param.userEmail = activeUser.userName;
                param.userAction = "X";
                param.userActionDate = DateTime.Now;

                var res = await EPKRSService.getEPKRSIncidentAccidentReport(param);

                if (res.isSuccess)
                {
                    await HandleDownloadDocument(res.Data.content, res.Data.fileName);
                    await _jsModule.InvokeVoidAsync("showAlert", "Export Success !");

                    resetExportDate();
                }
                else
                {
                    await _jsModule.InvokeVoidAsync("showAlert", $"Err: {res.ErrorCode} - {res.ErrorMessage} !");
                }

                isExportLoading = false;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task sendCustomEmailEPKRS()
        {
            try
            {
                CustomMailing mail = new();

                mail.moduleName = customMailData.moduleName;
                mail.actionName = customMailData.actionName;
                mail.locationId = customMailData.locationId;

                mail.from = customMailData.from;

                mail.to = new();
                if (customMailData.to.Count() > 0)
                    customMailData.to.ForEach(x => { mail.to.Add(new EmailLine { LineNo = x.LineNo, userEmail = x.userEmail + "@mitra10.com" }); });

                mail.cc = new();
                if (customMailData.cc.Count() > 0)
                    customMailData.cc.ForEach(x => { mail.cc.Add(new EmailLine { LineNo = x.LineNo, userEmail = x.userEmail + "@mitra10.com" }); });

                mail.Subject = customMailData.Subject;
                mail.Body = customMailData.Body;
                mail.OtherString = previousPreviewedDocumentID;
                mail.OtherDate = DateTime.Now;
                mail.OtherListString = new();

                var res = await ManagementService.sendManualEmail(mail);

                if (res.isSuccess)
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "Email Sent !");
                }
                else
                {
                    await _jsModule.InvokeVoidAsync("showAlert", $"Failed : {res.ErrorCode} - {res.ErrorMessage} !");
                }
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private void resetExportDate()
        {
            startDateExport = new(DateTime.Now.Year, 1, 1);
            endDateExport = DateTime.Now;

            isExported = false;
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

        private bool checkItemCaseData()
        {
            try
            {
                if (EPKRSService.itemCases.Any())
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

        private bool checkIncidentAccidentData()
        {
            try
            {
                if (EPKRSService.incidentAccidents.Any())
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

        private bool checkPreviewItemLine()
        {
            try
            {
                if (previewItemLine.Any())
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

        private bool checkPreviewCaseAttachment()
        {
            try
            {
                if (previewCaseAttachment.Any() && EPKRSService.fileStreams.Any())
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

        private bool checkDocumentDiscussions()
        {
            try
            {
                if (previewDocumentDiscussion.Any())
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

        private bool checkFileDiscussUpload()
        {
            try
            {
                if (fileDiscussionUpload.Any())
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

        private bool checkReplyFileDiscussUpload()
        {
            try
            {
                if (fileReplyDiscussionUpload.Any())
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

        private bool checkPartyInvolved()
        {
            try
            {
                if (partyInvolved.Any())
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

        private bool checkPreviewPartyInvolved()
        {
            try
            {
                if (previewIncidentAccidentInvolver.Any())
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

        private bool checkPreviewApproval()
        {
            try
            {
                if (previewDocumentListApproval.Any())
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

        private void redirectDocumentEdit(string type)
        {
            string temp = "";

            if (type.Equals("ITEMC"))
            {
                temp = type + "!_!" + previewItemCase.DocumentID;
            }
            else
            {
                temp = type + "!_!" + previewIncidentAccident.DocumentID;
            }

            string param = CommonLibrary.Base64Encode(temp);

            navigate.NavigateTo($"epkrs/editepkrs/{param}");
        }

        //
    }
}
