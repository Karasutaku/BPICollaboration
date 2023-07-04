using BPILibrary;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.EPKRS;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.PagesModel.EPKRS;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

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
        private List<EPKRSDocumentStatistics>? previewGeneralStatistics = null;
        private List<EPKRSItemCaseCategoryStatistics>? previewItemCaseCategoryStatistics = null;
        private List<EPKRSTopLocationReportStatistics>? previewTopLocationReportStatistics = null;
        private List<EPKRSItemCaseItemCategoryStatistics>? previewItemCategoryStatistics = null;
        private List<EPKRSIncidentAccidentRegionalStatistics>? previewRegionalStatistics = null;
        private List<EPKRSIncidentAccidentInvolverStatisticsbyPosition>? previewInvolvedbyPosStatistics = null;
        private List<EPKRSIncidentAccidentInvolverStatisticsbyDept>? previewInvolvedbyDeptStatistics = null;

        private ItemCase updateItemCase = new();
        private IncidentAccident updateIncidentAccident = new();
        private List<IncidentAccidentInvolver> partyInvolved = new();

        private DocumentDiscussion uploadDocumentDiscussion = new();
        //private List<CaseAttachment> uploadDocumentDiscussionAttachment = new();
        private List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileDiscussionUpload = new();

        private DocumentDiscussion uploadReplyDocumentDiscussion = new();
        //private List<CaseAttachment> uploadReplyDocumentDiscussionAttachment = new();
        private List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileReplyDiscussionUpload = new();

        private string previousPreviewedDocumentID = string.Empty;
        private string previousPreviewedDocumentLocation = string.Empty;

        private int itemCaseNumberofPages = 0;
        private int incidentAccidentNumberofPages = 0;
        private int itemCasePageActive = 0;
        private int incidentAccidentPageActive = 0;

        private string itemCaseFilterType { get; set; } = string.Empty; 
        private string incidentAccidentFilterType { get; set; } = string.Empty;

        private string itemCaseFilterValue { get; set; } = string.Empty;
        private string incidentAccidentFilterValue { get; set; } = string.Empty;

        private bool itemCaseFilterActive = false;
        private bool incidentAccidentFilterActive = false;
        private bool generalStatisticsFiltered = false;
        private bool monitoringPanelFiltered = false;
        private bool generalStatisticsMonthFilterActive = false;
        private bool generalStatisticsRiskTypeFilterActive = false;
        private bool generalStatisticsRegionalFilterActive = false;
        private bool generalStatisticsPositionFilterActive = false;
        private bool generalStatisticsDepartmentFilterActive = false;

        private int selectedYearFilter = DateTime.Now.Year;
        private int selectedYearFilterChart = DateTime.Now.Year;
        private string dinamicGeneralStatisticsDateFilter = string.Empty;
        private string dinamicGeneralStatisticsRiskTypeFilter = string.Empty;
        private string dinamicGeneralStatisticsRegionalFilter = string.Empty;
        private string dinamicGeneralStatisticsPositionFilter = string.Empty;
        private string dinamicGeneralStatisticsDepartmentFilter = string.Empty;
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

            await ManagementService.GetCompanyLocations(paramGetCompanyLocation);
            await ManagementService.GetAllDepartment(CommonLibrary.Base64Encode(paramGetAllDepartment));
            await ManagementService.getAllCategories();

            await EPKRSService.getEPRKSReportingType();
            await EPKRSService.getEPRKSRiskType();
            await EPKRSService.getEPRKSRiskSubType();
            await EPKRSService.getEPKRSItemRiskCategory();
            await EPKRSService.getEPKRSIncidentAccidentInvolverType();
            maxFileSize = await EPKRSService.getEPKRSMaxFileSize();

            incidentAccidentFilterActive = false;
            itemCaseFilterActive = false;

            string paramGetItemCaseInitialization = activeUser.location + "!_!!_!1";
            var itemCaseDt = await EPKRSService.getEPKRSItemCaseData(CommonLibrary.Base64Encode(paramGetItemCaseInitialization));

            string paramGetIncidentAccidentInitialization = activeUser.location + "!_!!_!1";
            var incidentAccidentDt = await EPKRSService.getEPKRSIncidentAccidentData(CommonLibrary.Base64Encode(paramGetIncidentAccidentInitialization));

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

            Task.Run(async () =>
            {
                await EPKRSService.getEPKRSInitializationDocumentDiscussions(docParams);
                await EPKRSService.getEPKRSDocumentDiscussionReadHistory(docParams);

                StateHasChanged();
            });

            if (activeUser.location.Equals(""))
            {
                string itemCaseParampz = $"[EPKRS].ItemCase!_!{activeUser.location}!_!";
                var itemCasepz = await EPKRSService.getEPKRSModuleNumberOfPage(CommonLibrary.Base64Encode(itemCaseParampz));
                itemCaseNumberofPages = itemCasepz.Data;
                
                string incidentAccidentParampz = $"[EPKRS].IncidentAccident!_!{activeUser.location}!_!";
                var incidentAccidentpz = await EPKRSService.getEPKRSModuleNumberOfPage(CommonLibrary.Base64Encode(incidentAccidentParampz));
                incidentAccidentNumberofPages = incidentAccidentpz.Data;
                
                await Task.Run(async () =>
                {
                    string generalStatisticsConditions = $"WHERE a.ReportDate BETWEEN \'{selectedYearFilter}-01-01 00:00:00\' AND \'{selectedYearFilter}-12-31 23:59:59\'";
                    var resGeneralStat = await EPKRSService.getEPKRSGeneralStatistics(CommonLibrary.Base64Encode(generalStatisticsConditions));

                    string itemCaseCategoryConditions = $"WHERE a.AuditActionDate BETWEEN \'{selectedYearFilter}-01-01 00:00:00\' AND \'{selectedYearFilter}-12-31 23:59:59\'!_!AND (p.ReportDate BETWEEN \'{selectedYearFilter}-01-01 00:00:00\' AND \'{selectedYearFilter}-12-31 23:59:59\')";
                    var resItemCaseCategoryStat = await EPKRSService.getEPKRSItemCaseCategoryStatistics(CommonLibrary.Base64Encode(itemCaseCategoryConditions));

                    string topLocationReportConditions = $"WHERE a.ReportDate BETWEEN \'{selectedYearFilter}-01-01 00:00:00\' AND \'{selectedYearFilter}-12-31 23:59:59\'!_!AND (p.ReportDate BETWEEN \'{selectedYearFilter}-01-01 00:00:00\' AND \'{selectedYearFilter}-12-31 23:59:59\')";
                    var resTopLocationReportStat = await EPKRSService.getEPKRSTopLocationReportStatistics(CommonLibrary.Base64Encode(topLocationReportConditions));

                    string itemCategoryStatsConditions = $"WHERE a.AuditActionDate BETWEEN \'{selectedYearFilter}-01-01 00:00:00\' AND \'{selectedYearFilter}-12-31 23:59:59\'!_!AND (p.ReportDate BETWEEN \'{selectedYearFilter}-01-01 00:00:00\' AND \'{selectedYearFilter}-12-31 23:59:59\')";
                    var resItemCategoryStat = await EPKRSService.getEPKRSItemCategoriesStatistics(CommonLibrary.Base64Encode(itemCategoryStatsConditions));

                    if (resGeneralStat.isSuccess)
                    {
                        previewGeneralStatistics = new();
                        previewGeneralStatistics = resGeneralStat.Data;
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
                });

                await Task.Run(async () => 
                {
                    string regionalStatsParam = $"WHERE a.ReportDate BETWEEN \'{selectedYearFilter}-01-01 00:00:00\' AND \'{selectedYearFilter}-12-31\'";
                    QueryModel<string> param = new();
                    param.Data = CommonLibrary.Base64Encode(regionalStatsParam);
                    var resRegionalStats = await EPKRSService.getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmail(param);

                    string involvedbyPosParam = $"WHERE a.ReportDate BETWEEN \'{selectedYearFilter}-01-01 00:00:00\' AND \'{selectedYearFilter}-12-31\'!_!AND x.ReportDate BETWEEN '{selectedYearFilter}-01-01' AND '{selectedYearFilter}-12-31'";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(involvedbyPosParam);
                    var resInvolvedbyPos = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPosition(param);

                    string involvedbyDeptParam = $"WHERE a.ReportDate BETWEEN \'{selectedYearFilter}-01-01 00:00:00\' AND \'{selectedYearFilter}-12-31\'!_!AND x.ReportDate BETWEEN '{selectedYearFilter}-01-01' AND '{selectedYearFilter}-12-31'";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(involvedbyDeptParam);
                    var resInvolvedbyDept = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDept(param);

                    if (resRegionalStats.isSuccess)
                    {
                        previewRegionalStatistics = new();
                        previewRegionalStatistics = resRegionalStats.Data;
                    }

                    if (resInvolvedbyPos.isSuccess)
                    {
                        previewInvolvedbyPosStatistics = new();
                        previewInvolvedbyPosStatistics = resInvolvedbyPos.Data;
                    }

                    if (resInvolvedbyDept.isSuccess)
                    {
                        previewInvolvedbyDeptStatistics = new();
                        previewInvolvedbyDeptStatistics = resInvolvedbyDept.Data;
                    }
                });

            }
            else
            {
                string itemCaseParampz = $"[EPKRS].ItemCase!_!{activeUser.location}!_!WHERE SiteReporter = \'{activeUser.location}\' OR SiteSender = \'{activeUser.location}\'";
                var itemCasepz = await EPKRSService.getEPKRSModuleNumberOfPage(CommonLibrary.Base64Encode(itemCaseParampz));
                itemCaseNumberofPages = itemCasepz.Data;

                string incidentAccidentParampz = $"[EPKRS].IncidentAccident!_!{activeUser.location}!_!WHERE SiteReporter = \'{activeUser.location}\'";
                var incidentAccidentpz = await EPKRSService.getEPKRSModuleNumberOfPage(CommonLibrary.Base64Encode(incidentAccidentParampz));
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
                        "Top 10 Location with Most Reports",
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
                        "Item Case Count by Risk Type",
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
                        "Item Case Value by Risk Type",
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
                        "Item Category Report Statistics",
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
                    "Top 10 Location with Most Reports",
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
                    itemCategoryStatsRef,
                    "itemCaseCategoryStats",
                    arrayLabels,
                    arrayIntValues,
                    "Item Case Count by Risk Type",
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
                    "Item Case Value by Risk Type",
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
                    itemCaseCategoryStatsRef,
                    "itemCategoryStats",
                    arrayLabels,
                    arrayIntValues,
                    "Item Category Report Statistics",
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

            if (itemCaseFilterActive)
            {
                if (itemCaseFilterType.Equals("ID"))
                {
                    string paramGetItemCaseInitialization = activeUser.location + $"!_!WHERE a.DocumentID LIKE \'%{itemCaseFilterValue}%\'!_!" + itemCasePageActive.ToString();
                    await EPKRSService.getEPKRSItemCaseData(CommonLibrary.Base64Encode(paramGetItemCaseInitialization));
                }
                else if (itemCaseFilterType.Equals("LOC"))
                {
                    string paramGetItemCaseInitialization = activeUser.location + $"!_!WHERE a.SiteReporter = \'{itemCaseFilterValue}\'!_!" + itemCasePageActive.ToString();
                    await EPKRSService.getEPKRSItemCaseData(CommonLibrary.Base64Encode(paramGetItemCaseInitialization));
                }
                else if (itemCaseFilterType.Equals("STATUS"))
                {
                    string paramGetItemCaseInitialization = activeUser.location + $"!_!WHERE a.DocumentStatus LIKE \'%{itemCaseFilterValue}%\'!_!" + itemCasePageActive.ToString();
                    await EPKRSService.getEPKRSItemCaseData(CommonLibrary.Base64Encode(paramGetItemCaseInitialization));
                }
            }
            else
            {
                string paramGetItemCaseInitialization = activeUser.location + "!_!!_!" + itemCasePageActive.ToString();
                await EPKRSService.getEPKRSItemCaseData(CommonLibrary.Base64Encode(paramGetItemCaseInitialization));
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

            if (incidentAccidentFilterActive)
            {
                if (incidentAccidentFilterType.Equals("ID"))
                {
                    string paramGetIncidentAccidentInitialization = activeUser.location + $"!_!WHERE a.DocumentID LIKE \'%{incidentAccidentFilterValue}%\'!_!" + incidentAccidentPageActive.ToString();
                    await EPKRSService.getEPKRSIncidentAccidentData(CommonLibrary.Base64Encode(paramGetIncidentAccidentInitialization));
                }
                else if (incidentAccidentFilterType.Equals("LOC"))
                {
                    string paramGetIncidentAccidentInitialization = activeUser.location + $"!_!WHERE a.SiteReporter = \'{incidentAccidentFilterValue}\'!_!" + incidentAccidentPageActive.ToString();
                    await EPKRSService.getEPKRSIncidentAccidentData(CommonLibrary.Base64Encode(paramGetIncidentAccidentInitialization));
                }
                else if (incidentAccidentFilterType.Equals("STATUS"))
                {
                    string paramGetIncidentAccidentInitialization = activeUser.location + $"!_!WHERE a.DocumentStatus LIKE \'%{incidentAccidentFilterValue}%\'!_!" + incidentAccidentPageActive.ToString();
                    await EPKRSService.getEPKRSIncidentAccidentData(CommonLibrary.Base64Encode(paramGetIncidentAccidentInitialization));
                }
            }
            else
            {
                string paramGetIncidentAccidentInitialization = activeUser.location + "!_!!_!" + incidentAccidentPageActive.ToString();
                await EPKRSService.getEPKRSIncidentAccidentData(CommonLibrary.Base64Encode(paramGetIncidentAccidentInitialization));
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

                if (itemCaseFilterType.Equals("ID"))
                {
                    string itemCaseParampz = $"[EPKRS].ItemCase!_!{activeUser.location}!_!WHERE DocumentID LIKE '%{itemCaseFilterValue}%'";
                    var itemCasepz = await EPKRSService.getEPKRSModuleNumberOfPage(CommonLibrary.Base64Encode(itemCaseParampz));
                    incidentAccidentNumberofPages = itemCasepz.Data;

                    string paramGetItemCaseInitialization = activeUser.location + $"!_!WHERE a.DocumentID LIKE \'%{itemCaseFilterValue}%\'!_!1";
                    await EPKRSService.getEPKRSItemCaseData(CommonLibrary.Base64Encode(paramGetItemCaseInitialization));
                }
                else if (itemCaseFilterType.Equals("LOC"))
                {
                    string itemCaseParampz = $"[EPKRS].ItemCase!_!{activeUser.location}!_!WHERE SiteReporter = \'{itemCaseFilterValue}\'";
                    var itemCasepz = await EPKRSService.getEPKRSModuleNumberOfPage(CommonLibrary.Base64Encode(itemCaseParampz));
                    incidentAccidentNumberofPages = itemCasepz.Data;

                    string paramGetItemCaseInitialization = activeUser.location + $"!_!WHERE a.SiteReporter = \'{itemCaseFilterValue}\'!_!1";
                    await EPKRSService.getEPKRSItemCaseData(CommonLibrary.Base64Encode(paramGetItemCaseInitialization));
                }
                else if (itemCaseFilterType.Equals("STATUS"))
                {
                    string itemCaseParampz = $"[EPKRS].ItemCase!_!{activeUser.location}!_!WHERE DocumentStatus LIKE \'%{itemCaseFilterValue}%\'";
                    var itemCasepz = await EPKRSService.getEPKRSModuleNumberOfPage(CommonLibrary.Base64Encode(itemCaseParampz));
                    incidentAccidentNumberofPages = itemCasepz.Data;

                    string paramGetItemCaseInitialization = activeUser.location + $"!_!WHERE a.DocumentStatus LIKE \'%{itemCaseFilterValue}%\'!_!1";
                    await EPKRSService.getEPKRSItemCaseData(CommonLibrary.Base64Encode(paramGetItemCaseInitialization));
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

                if (incidentAccidentFilterType.Equals("ID"))
                {
                    string incidentAccidentParampz = $"[EPKRS].IncidentAccident!_!{activeUser.location}!_!WHERE DocumentID LIKE '%{incidentAccidentFilterValue}%'";
                    var incidentAccidentpz = await EPKRSService.getEPKRSModuleNumberOfPage(CommonLibrary.Base64Encode(incidentAccidentParampz));
                    incidentAccidentNumberofPages = incidentAccidentpz.Data;

                    string paramGetIncidentAccidentInitialization = activeUser.location + $"!_!WHERE a.DocumentID LIKE \'%{incidentAccidentFilterValue}%\'!_!1";
                    await EPKRSService.getEPKRSIncidentAccidentData(CommonLibrary.Base64Encode(paramGetIncidentAccidentInitialization));
                }
                else if (incidentAccidentFilterType.Equals("LOC"))
                {
                    string incidentAccidentParampz = $"[EPKRS].IncidentAccident!_!{activeUser.location}!_!WHERE SiteReporter = \'{incidentAccidentFilterValue}\'";
                    var incidentAccidentpz = await EPKRSService.getEPKRSModuleNumberOfPage(CommonLibrary.Base64Encode(incidentAccidentParampz));
                    incidentAccidentNumberofPages = incidentAccidentpz.Data;

                    string paramGetIncidentAccidentInitialization = activeUser.location + $"!_!WHERE a.SiteReporter = \'{incidentAccidentFilterValue}\'!_!1";
                    await EPKRSService.getEPKRSIncidentAccidentData(CommonLibrary.Base64Encode(paramGetIncidentAccidentInitialization));
                }
                else if (incidentAccidentFilterType.Equals("STATUS"))
                {
                    string incidentAccidentParampz = $"[EPKRS].IncidentAccident!_!{activeUser.location}!_!WHERE DocumentStatus LIKE \'%{incidentAccidentFilterValue}%\'";
                    var incidentAccidentpz = await EPKRSService.getEPKRSModuleNumberOfPage(CommonLibrary.Base64Encode(incidentAccidentParampz));
                    incidentAccidentNumberofPages = incidentAccidentpz.Data;

                    string paramGetIncidentAccidentInitialization = activeUser.location + $"!_!WHERE a.DocumentStatus LIKE \'%{incidentAccidentFilterValue}%\'!_!1";
                    await EPKRSService.getEPKRSIncidentAccidentData(CommonLibrary.Base64Encode(paramGetIncidentAccidentInitialization));
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

                string itemCaseParampz = $"[EPKRS].ItemCase!_!{activeUser.location}!_!";
                var itemCasepz = await EPKRSService.getEPKRSModuleNumberOfPage(CommonLibrary.Base64Encode(itemCaseParampz));
                incidentAccidentNumberofPages = itemCasepz.Data;

                string paramGetItemCaseInitialization = activeUser.location + "!_!!_!1";
                await EPKRSService.getEPKRSItemCaseData(CommonLibrary.Base64Encode(paramGetItemCaseInitialization));

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

                string incidentAccidentParampz = $"[EPKRS].IncidentAccident!_!{activeUser.location}!_!";
                var incidentAccidentpz = await EPKRSService.getEPKRSModuleNumberOfPage(CommonLibrary.Base64Encode(incidentAccidentParampz));
                incidentAccidentNumberofPages = incidentAccidentpz.Data;

                string paramGetIncidentAccidentInitialization = activeUser.location + "!_!!_!1";
                await EPKRSService.getEPKRSIncidentAccidentData(CommonLibrary.Base64Encode(paramGetIncidentAccidentInitialization));

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

        private async Task generalStatisticsFilterbyYear(int year, bool isReset)
        {
            try
            {
                dinamicGeneralStatisticsDateFilter = string.Empty;
                selectedYearFilter = year;

                previewGeneralStatistics = null;
                previewRegionalStatistics = null;
                previewInvolvedbyPosStatistics = null;
                previewInvolvedbyDeptStatistics = null;

                string generalStatisticsConditions = $"WHERE a.ReportDate BETWEEN \'{year}-01-01 00:00:00\' AND \'{year}-12-31 23:59:59\'";
                var resGeneralStat = await EPKRSService.getEPKRSGeneralStatistics(CommonLibrary.Base64Encode(generalStatisticsConditions));

                string regionalStatsParam = $"WHERE a.ReportDate BETWEEN \'{year}-01-01 00:00:00\' AND \'{year}-12-31\'";
                QueryModel<string> param = new();
                param.Data = CommonLibrary.Base64Encode(regionalStatsParam);
                var resRegionalStats = await EPKRSService.getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmail(param);

                string involvedbyPosParam = $"WHERE a.ReportDate BETWEEN \'{year}-01-01 00:00:00\' AND \'{year}-12-31\'!_!AND (x.ReportDate BETWEEN \'{year}-01-01\' AND \'{year}-12-31\')";
                param = new();
                param.Data = CommonLibrary.Base64Encode(involvedbyPosParam);
                var resInvolvedbyPos = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPosition(param);

                string involvedbyDeptParam = $"WHERE a.ReportDate BETWEEN \'{year}-01-01 00:00:00\' AND \'{year}-12-31\'!_!AND (x.ReportDate BETWEEN \'{year}-01-01\' AND \'{year}-12-31\')";
                param = new();
                param.Data = CommonLibrary.Base64Encode(involvedbyDeptParam);
                var resInvolvedbyDept = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDept(param);

                if (resGeneralStat.isSuccess)
                {
                    previewGeneralStatistics = new();
                    previewGeneralStatistics = resGeneralStat.Data;
                }

                if (resRegionalStats.isSuccess)
                {
                    previewRegionalStatistics = new();
                    previewRegionalStatistics = resRegionalStats.Data;
                }

                if (resInvolvedbyPos.isSuccess)
                {
                    previewInvolvedbyPosStatistics = new();
                    previewInvolvedbyPosStatistics = resInvolvedbyPos.Data;
                }

                if (resInvolvedbyDept.isSuccess)
                {
                    previewInvolvedbyDeptStatistics = new();
                    previewInvolvedbyDeptStatistics = resInvolvedbyDept.Data;
                }

                if (isReset)
                {
                    foreach (var dt in monthSelector)
                    {
                        monthSelector[dt.Key] = false;
                    }

                    generalStatisticsMonthFilterActive = false;
                    generalStatisticsRiskTypeFilterActive = false;
                    generalStatisticsRegionalFilterActive = false;
                    generalStatisticsPositionFilterActive = false;
                    generalStatisticsFiltered = false;
                }

                StateHasChanged();
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task generalStatisticsFilterbyMonth()
        {
            try
            {
                if (!monthSelector.Any(x => x.Value.Equals(true)))
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "Select at Least 1 Month to Filter !");
                }
                else
                {
                    generalStatisticsMonthFilterActive = true;
                    generalStatisticsFiltered = true;

                    previewGeneralStatistics = null;
                    previewRegionalStatistics = null;
                    previewInvolvedbyPosStatistics = null;
                    previewInvolvedbyDeptStatistics = null;

                    dinamicGeneralStatisticsDateFilter = "WHERE ";

                    foreach (var selectedMonth in monthSelector.Where(x => x.Value.Equals(true)))
                    {
                        dinamicGeneralStatisticsDateFilter += $"(a.ReportDate BETWEEN \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, selectedMonth.Key)} 23:59:59\') OR ";
                    }

                    dinamicGeneralStatisticsDateFilter = dinamicGeneralStatisticsDateFilter.Substring(0, dinamicGeneralStatisticsDateFilter.Length - 4);

                    var resGeneralStat = await EPKRSService.getEPKRSGeneralStatistics(CommonLibrary.Base64Encode(dinamicGeneralStatisticsDateFilter));
                    QueryModel<string> param = new();
                    param.Data = CommonLibrary.Base64Encode(dinamicGeneralStatisticsDateFilter);
                    var resRegionalStats = await EPKRSService.getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmail(param);

                    dinamicGeneralStatisticsDateFilter += "!_!AND (";

                    foreach (var selectedMonth in monthSelector.Where(x => x.Value.Equals(true)))
                    {
                        dinamicGeneralStatisticsDateFilter += $"(x.ReportDate BETWEEN \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, selectedMonth.Key)} 23:59:59\') OR ";
                    }

                    dinamicGeneralStatisticsDateFilter = dinamicGeneralStatisticsDateFilter.Substring(0, dinamicGeneralStatisticsDateFilter.Length - 4);
                    dinamicGeneralStatisticsDateFilter += ")";

                    param = new();
                    param.Data = CommonLibrary.Base64Encode(dinamicGeneralStatisticsDateFilter);
                    var resInvolvedbyPos = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPosition(param);
                    var resInvolvedbyDept = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDept(param);

                    if (resGeneralStat.isSuccess)
                    {
                        previewGeneralStatistics = new();
                        previewGeneralStatistics = resGeneralStat.Data;
                    }

                    if (resRegionalStats.isSuccess)
                    {
                        previewRegionalStatistics = new();
                        previewRegionalStatistics = resRegionalStats.Data;
                    }

                    if (resInvolvedbyPos.isSuccess)
                    {
                        previewInvolvedbyPosStatistics = new();
                        previewInvolvedbyPosStatistics = resInvolvedbyPos.Data;
                    }

                    if (resInvolvedbyDept.isSuccess)
                    {
                        previewInvolvedbyDeptStatistics = new();
                        previewInvolvedbyDeptStatistics = resInvolvedbyDept.Data;
                    }

                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task generalStatisticsFilterbyRiskType(string riskId)
        {
            try
            {
                var repType = EPKRSService.riskTypes.SingleOrDefault(x => x.RiskID.Equals(riskId));
               
                if (repType == null && repType.ReportingID.Equals("ITEMC"))
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "Select Incident Accident Type Only !");
                }
                else
                {
                    generalStatisticsRegionalFilterActive = false;
                    generalStatisticsPositionFilterActive = false;
                    generalStatisticsDepartmentFilterActive = false;
                    generalStatisticsRiskTypeFilterActive = true;
                    generalStatisticsFiltered = true;

                    //previewGeneralStatistics = null;
                    previewRegionalStatistics = null;
                    previewInvolvedbyPosStatistics = null;
                    previewInvolvedbyDeptStatistics = null;

                    if (generalStatisticsMonthFilterActive)
                    {
                        dinamicGeneralStatisticsDateFilter = "WHERE ";

                        foreach (var selectedMonth in monthSelector.Where(x => x.Value.Equals(true)))
                        {
                            dinamicGeneralStatisticsDateFilter += $"(a.ReportDate BETWEEN \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, selectedMonth.Key)} 23:59:59\') OR ";
                        }

                        dinamicGeneralStatisticsDateFilter = dinamicGeneralStatisticsDateFilter.Substring(0, dinamicGeneralStatisticsDateFilter.Length - 4);
                        dinamicGeneralStatisticsRiskTypeFilter += dinamicGeneralStatisticsDateFilter + $" AND c.RiskID = \'{riskId}\'";

                        //var resGeneralStat = await EPKRSService.getEPKRSGeneralStatistics(CommonLibrary.Base64Encode(dinamicGeneralStatisticsRiskTypeFilter));
                        QueryModel<string> param = new();
                        param.Data = CommonLibrary.Base64Encode(dinamicGeneralStatisticsRiskTypeFilter);
                        var resRegionalStats = await EPKRSService.getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmail(param);

                        dinamicGeneralStatisticsDateFilter += "!_!AND (";

                        foreach (var selectedMonth in monthSelector.Where(x => x.Value.Equals(true)))
                        {
                            dinamicGeneralStatisticsDateFilter += $"(x.ReportDate BETWEEN \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, selectedMonth.Key)} 23:59:59\') OR ";
                        }

                        dinamicGeneralStatisticsDateFilter = dinamicGeneralStatisticsDateFilter.Substring(0, dinamicGeneralStatisticsDateFilter.Length - 4);
                        dinamicGeneralStatisticsDateFilter += ")";

                        dinamicGeneralStatisticsRiskTypeFilter += dinamicGeneralStatisticsDateFilter + $" AND v.RiskID = \'{riskId}\'";

                        param = new();
                        param.Data = CommonLibrary.Base64Encode(dinamicGeneralStatisticsDateFilter);
                        var resInvolvedbyPos = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPosition(param);
                        var resInvolvedbyDept = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDept(param);

                        //if (resGeneralStat.isSuccess)
                        //{
                        //    previewGeneralStatistics = new();
                        //    previewGeneralStatistics = resGeneralStat.Data;
                        //}

                        if (resRegionalStats.isSuccess)
                        {
                            previewRegionalStatistics = new();
                            previewRegionalStatistics = resRegionalStats.Data;
                        }

                        if (resInvolvedbyPos.isSuccess)
                        {
                            previewInvolvedbyPosStatistics = new();
                            previewInvolvedbyPosStatistics = resInvolvedbyPos.Data;
                        }

                        if (resInvolvedbyDept.isSuccess)
                        {
                            previewInvolvedbyDeptStatistics = new();
                            previewInvolvedbyDeptStatistics = resInvolvedbyDept.Data;
                        }
                    }
                    else
                    {
                        dinamicGeneralStatisticsRiskTypeFilter = $"WHERE c.RiskID = \'{riskId}\'";

                        QueryModel<string> param = new();
                        param.Data = CommonLibrary.Base64Encode(dinamicGeneralStatisticsRiskTypeFilter);

                        //var resGeneralStat = await EPKRSService.getEPKRSGeneralStatistics(CommonLibrary.Base64Encode(dinamicGeneralStatisticsRiskTypeFilter));
                        var resRegionalStats = await EPKRSService.getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmail(param);

                        dinamicGeneralStatisticsRiskTypeFilter += $"!_!AND v.RiskID = \'{riskId}\'";

                        param = new();
                        param.Data = CommonLibrary.Base64Encode(dinamicGeneralStatisticsRiskTypeFilter);

                        var resInvolvedbyPos = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPosition(param);
                        var resInvolvedbyDept = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDept(param);

                        //if (resGeneralStat.isSuccess)
                        //{
                        //    previewGeneralStatistics = new();
                        //    previewGeneralStatistics = resGeneralStat.Data;
                        //}

                        if (resRegionalStats.isSuccess)
                        {
                            previewRegionalStatistics = new();
                            previewRegionalStatistics = resRegionalStats.Data;
                        }

                        if (resInvolvedbyPos.isSuccess)
                        {
                            previewInvolvedbyPosStatistics = new();
                            previewInvolvedbyPosStatistics = resInvolvedbyPos.Data;
                        }

                        if (resInvolvedbyDept.isSuccess)
                        {
                            previewInvolvedbyDeptStatistics = new();
                            previewInvolvedbyDeptStatistics = resInvolvedbyDept.Data;
                        }
                    }

                    dinamicGeneralStatisticsRiskTypeFilter = string.Empty;
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task generalStatisticsFilterbyRegion(string dormEmail)
        {
            try
            {
                generalStatisticsRiskTypeFilterActive = false;
                generalStatisticsPositionFilterActive = false;
                generalStatisticsDepartmentFilterActive = false;
                generalStatisticsRegionalFilterActive = true;
                generalStatisticsFiltered = true;

                previewGeneralStatistics = null;
                //previewRegionalStatistics = null;
                previewInvolvedbyPosStatistics = null;
                previewInvolvedbyDeptStatistics = null;

                if (generalStatisticsMonthFilterActive)
                {
                    dinamicGeneralStatisticsDateFilter = "WHERE (";

                    foreach (var selectedMonth in monthSelector.Where(x => x.Value.Equals(true)))
                    {
                        dinamicGeneralStatisticsDateFilter += $"(a.ReportDate BETWEEN \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, selectedMonth.Key)} 23:59:59\') OR ";
                    }

                    dinamicGeneralStatisticsDateFilter = dinamicGeneralStatisticsDateFilter.Substring(0, dinamicGeneralStatisticsDateFilter.Length - 4);
                    dinamicGeneralStatisticsRegionalFilter += dinamicGeneralStatisticsDateFilter + $") AND a.DORMEmail = \'{dormEmail}\'";
                    dinamicGeneralStatisticsRegionalFilter += "!_!AND ((";

                    foreach (var selectedMonth in monthSelector.Where(x => x.Value.Equals(true)))
                    {
                        dinamicGeneralStatisticsRegionalFilter += $"(z.ReportDate BETWEEN \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, selectedMonth.Key)} 23:59:59\') OR ";
                    }

                    dinamicGeneralStatisticsRegionalFilter = dinamicGeneralStatisticsRegionalFilter.Substring(0, dinamicGeneralStatisticsRegionalFilter.Length - 4);
                    dinamicGeneralStatisticsRegionalFilter += $") AND z.DORMEmail = \'{dormEmail}\')";

                    QueryModel<string> param = new();
                    param.Data = CommonLibrary.Base64Encode(dinamicGeneralStatisticsRegionalFilter);

                    var resGeneralStat = await EPKRSService.getEPKRSGeneralIncidentAccidentStatistics(param);
                    //var resRegionalStats = await EPKRSService.getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmail(CommonLibrary.Base64Encode(dinamicGeneralStatisticsRiskTypeFilter));

                    dinamicGeneralStatisticsRegionalFilter = string.Empty;
                    dinamicGeneralStatisticsRegionalFilter += dinamicGeneralStatisticsDateFilter + $") AND a.DORMEmail = \'{dormEmail}\'!_!AND (";

                    foreach (var selectedMonth in monthSelector.Where(x => x.Value.Equals(true)))
                    {
                        dinamicGeneralStatisticsRegionalFilter += $"(x.ReportDate BETWEEN \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, selectedMonth.Key)} 23:59:59\') OR ";
                    }

                    dinamicGeneralStatisticsRegionalFilter = dinamicGeneralStatisticsRegionalFilter.Substring(0, dinamicGeneralStatisticsRegionalFilter.Length - 4);
                    dinamicGeneralStatisticsRegionalFilter += $") AND x.DORMEmail = \'{dormEmail}\'";

                    param = new();
                    param.Data = CommonLibrary.Base64Encode(dinamicGeneralStatisticsRegionalFilter);
                    var resInvolvedbyPos = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPosition(param);
                    var resInvolvedbyDept = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDept(param);

                    if (resGeneralStat.isSuccess)
                    {
                        previewGeneralStatistics = new();
                        previewGeneralStatistics = resGeneralStat.Data;
                    }

                    //if (resRegionalStats.isSuccess)
                    //{
                    //    previewRegionalStatistics = new();
                    //    previewRegionalStatistics = resRegionalStats.Data;
                    //}

                    if (resInvolvedbyPos.isSuccess)
                    {
                        previewInvolvedbyPosStatistics = new();
                        previewInvolvedbyPosStatistics = resInvolvedbyPos.Data;
                    }

                    if (resInvolvedbyDept.isSuccess)
                    {
                        previewInvolvedbyDeptStatistics = new();
                        previewInvolvedbyDeptStatistics = resInvolvedbyDept.Data;
                    }
                }
                else
                {
                    dinamicGeneralStatisticsRegionalFilter = $"WHERE a.DORMEmail = \'{dormEmail}\'!_!AND z.DORMEmail = \'{dormEmail}\'";

                    QueryModel<string> param = new();
                    param.Data = CommonLibrary.Base64Encode(dinamicGeneralStatisticsRegionalFilter);

                    var resGeneralStat = await EPKRSService.getEPKRSGeneralIncidentAccidentStatistics(param);
                    //var resRegionalStats = await EPKRSService.getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmail(CommonLibrary.Base64Encode(dinamicGeneralStatisticsRiskTypeFilter));

                    dinamicGeneralStatisticsRegionalFilter = $"WHERE a.DORMEmail = \'{dormEmail}\'!_!AND x.DORMEmail = \'{dormEmail}\'";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(dinamicGeneralStatisticsRegionalFilter);

                    var resInvolvedbyPos = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPosition(param);
                    var resInvolvedbyDept = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDept(param);

                    if (resGeneralStat.isSuccess)
                    {
                        previewGeneralStatistics = new();
                        previewGeneralStatistics = resGeneralStat.Data;
                    }

                    //if (resRegionalStats.isSuccess)
                    //{
                    //    previewRegionalStatistics = new();
                    //    previewRegionalStatistics = resRegionalStats.Data;
                    //}

                    if (resInvolvedbyPos.isSuccess)
                    {
                        previewInvolvedbyPosStatistics = new();
                        previewInvolvedbyPosStatistics = resInvolvedbyPos.Data;
                    }

                    if (resInvolvedbyDept.isSuccess)
                    {
                        previewInvolvedbyDeptStatistics = new();
                        previewInvolvedbyDeptStatistics = resInvolvedbyDept.Data;
                    }
                }

                dinamicGeneralStatisticsRegionalFilter = string.Empty;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task generalStatisticsFilterbyPosition(string involverPos)
        {
            try
            {
                generalStatisticsRiskTypeFilterActive = false;
                generalStatisticsRegionalFilterActive = false;
                generalStatisticsDepartmentFilterActive = false;
                generalStatisticsPositionFilterActive = true;
                generalStatisticsFiltered = true;

                previewGeneralStatistics = null;
                previewRegionalStatistics = null;
                //previewInvolvedbyPosStatistics = null;
                previewInvolvedbyDeptStatistics = null;

                if (generalStatisticsMonthFilterActive)
                {
                    var joinA = "INNER JOIN (SELECT DISTINCT l.DocumentID, l.InvolverPosition FROM [EPKRS].IncidentAccidentInvolverDetails l) b on a.DocumentID = b.DocumentID ";
                    var joinB = "INNER JOIN (SELECT DISTINCT l.DocumentID, l.InvolverPosition FROM [EPKRS].IncidentAccidentInvolverDetails l) d on a.DocumentID = d.DocumentID ";
                    dinamicGeneralStatisticsDateFilter = "WHERE (";

                    foreach (var selectedMonth in monthSelector.Where(x => x.Value.Equals(true)))
                    {
                        dinamicGeneralStatisticsDateFilter += $"(a.ReportDate BETWEEN \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, selectedMonth.Key)} 23:59:59\') OR ";
                    }

                    dinamicGeneralStatisticsDateFilter = dinamicGeneralStatisticsDateFilter.Substring(0, dinamicGeneralStatisticsDateFilter.Length - 4);
                    dinamicGeneralStatisticsPositionFilter += dinamicGeneralStatisticsDateFilter + $") AND b.InvolverPosition = \'{involverPos}\'";
                    dinamicGeneralStatisticsPositionFilter += "!_!AND ((";

                    foreach (var selectedMonth in monthSelector.Where(x => x.Value.Equals(true)))
                    {
                        dinamicGeneralStatisticsPositionFilter += $"(z.ReportDate BETWEEN \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, selectedMonth.Key)} 23:59:59\') OR ";
                    }

                    dinamicGeneralStatisticsPositionFilter = dinamicGeneralStatisticsPositionFilter.Substring(0, dinamicGeneralStatisticsPositionFilter.Length - 4);
                    dinamicGeneralStatisticsPositionFilter += $") AND m.InvolverPosition = \'{involverPos}\')";

                    QueryModel<string> param = new();
                    param.Data = CommonLibrary.Base64Encode(joinA + dinamicGeneralStatisticsPositionFilter);

                    var resGeneralStat = await EPKRSService.getEPKRSGeneralIncidentAccidentStatistics(param);

                    dinamicGeneralStatisticsPositionFilter = string.Empty;
                    dinamicGeneralStatisticsPositionFilter += dinamicGeneralStatisticsDateFilter + $") AND d.InvolverPosition = \'{involverPos}\'";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(joinB + dinamicGeneralStatisticsPositionFilter);

                    var resRegionalStats = await EPKRSService.getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmail(param);

                    dinamicGeneralStatisticsPositionFilter = string.Empty;
                    dinamicGeneralStatisticsPositionFilter += dinamicGeneralStatisticsDateFilter + $") AND b.InvolverPosition = \'{involverPos}\'!_!AND (";

                    foreach (var selectedMonth in monthSelector.Where(x => x.Value.Equals(true)))
                    {
                        dinamicGeneralStatisticsPositionFilter += $"(x.ReportDate BETWEEN \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, selectedMonth.Key)} 23:59:59\') OR ";
                    }

                    dinamicGeneralStatisticsPositionFilter = dinamicGeneralStatisticsPositionFilter.Substring(0, dinamicGeneralStatisticsPositionFilter.Length - 4);
                    dinamicGeneralStatisticsPositionFilter += $") AND z.InvolverPosition = \'{involverPos}\'";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(dinamicGeneralStatisticsPositionFilter);

                    //var resInvolvedbyPos = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPosition(CommonLibrary.Base64Encode(dinamicGeneralStatisticsPositionFilter));
                    var resInvolvedbyDept = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDept(param);

                    if (resGeneralStat.isSuccess)
                    {
                        previewGeneralStatistics = new();
                        previewGeneralStatistics = resGeneralStat.Data;
                    }

                    if (resRegionalStats.isSuccess)
                    {
                        previewRegionalStatistics = new();
                        previewRegionalStatistics = resRegionalStats.Data;
                    }

                    //if (resInvolvedbyPos.isSuccess)
                    //{
                    //    previewInvolvedbyPosStatistics = new();
                    //    previewInvolvedbyPosStatistics = resInvolvedbyPos.Data;
                    //}

                    if (resInvolvedbyDept.isSuccess)
                    {
                        previewInvolvedbyDeptStatistics = new();
                        previewInvolvedbyDeptStatistics = resInvolvedbyDept.Data;
                    }
                }
                else
                {
                    dinamicGeneralStatisticsPositionFilter = $"INNER JOIN (SELECT DISTINCT l.DocumentID, l.InvolverPosition FROM [EPKRS].IncidentAccidentInvolverDetails l) b on a.DocumentID = b.DocumentID WHERE b.InvolverPosition = \'{involverPos}\'!_!AND m.InvolverPosition = \'{involverPos}\'";

                    QueryModel<string> param = new();
                    param.Data = CommonLibrary.Base64Encode(dinamicGeneralStatisticsPositionFilter);

                    var resGeneralStat = await EPKRSService.getEPKRSGeneralIncidentAccidentStatistics(param);

                    dinamicGeneralStatisticsPositionFilter = $"INNER JOIN (SELECT DISTINCT l.DocumentID, l.InvolverPosition FROM [EPKRS].IncidentAccidentInvolverDetails l) d on a.DocumentID = d.DocumentID WHERE d.InvolverPosition = \'{involverPos}\'";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(dinamicGeneralStatisticsPositionFilter);

                    var resRegionalStats = await EPKRSService.getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmail(param);

                    dinamicGeneralStatisticsPositionFilter = $"WHERE b.InvolverPosition = \'{involverPos}\'!_!AND z.InvolverPosition = \'{involverPos}\'";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(dinamicGeneralStatisticsPositionFilter);

                    //var resInvolvedbyPos = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPosition(CommonLibrary.Base64Encode(dinamicGeneralStatisticsPositionFilter));
                    var resInvolvedbyDept = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDept(param);

                    if (resGeneralStat.isSuccess)
                    {
                        previewGeneralStatistics = new();
                        previewGeneralStatistics = resGeneralStat.Data;
                    }

                    if (resRegionalStats.isSuccess)
                    {
                        previewRegionalStatistics = new();
                        previewRegionalStatistics = resRegionalStats.Data;
                    }

                    //if (resInvolvedbyPos.isSuccess)
                    //{
                    //    previewInvolvedbyPosStatistics = new();
                    //    previewInvolvedbyPosStatistics = resInvolvedbyPos.Data;
                    //}

                    if (resInvolvedbyDept.isSuccess)
                    {
                        previewInvolvedbyDeptStatistics = new();
                        previewInvolvedbyDeptStatistics = resInvolvedbyDept.Data;
                    }
                }

                dinamicGeneralStatisticsPositionFilter = string.Empty;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task generalStatisticsFilterbyDept(string involverPos)
        {
            try
            {
                generalStatisticsRiskTypeFilterActive = false;
                generalStatisticsRegionalFilterActive = false;
                generalStatisticsPositionFilterActive = false;
                generalStatisticsDepartmentFilterActive = true;
                generalStatisticsFiltered = true;

                previewGeneralStatistics = null;
                previewRegionalStatistics = null;
                previewInvolvedbyPosStatistics = null;
                //previewInvolvedbyDeptStatistics = null;

                if (generalStatisticsMonthFilterActive)
                {
                    var joinA = "INNER JOIN (SELECT DISTINCT l.DocumentID, l.InvolverDept FROM [EPKRS].IncidentAccidentInvolverDetails l) b on a.DocumentID = b.DocumentID ";
                    var joinB = "INNER JOIN (SELECT DISTINCT l.DocumentID, l.InvolverDept FROM [EPKRS].IncidentAccidentInvolverDetails l) d on a.DocumentID = d.DocumentID ";
                    dinamicGeneralStatisticsDateFilter = "WHERE (";

                    foreach (var selectedMonth in monthSelector.Where(x => x.Value.Equals(true)))
                    {
                        dinamicGeneralStatisticsDateFilter += $"(a.ReportDate BETWEEN \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, selectedMonth.Key)} 23:59:59\') OR ";
                    }

                    dinamicGeneralStatisticsDateFilter = dinamicGeneralStatisticsDateFilter.Substring(0, dinamicGeneralStatisticsDateFilter.Length - 4);
                    dinamicGeneralStatisticsDepartmentFilter += dinamicGeneralStatisticsDateFilter + $") AND b.InvolverDept = \'{involverPos}\'";
                    dinamicGeneralStatisticsDepartmentFilter += "!_!AND ((";

                    foreach (var selectedMonth in monthSelector.Where(x => x.Value.Equals(true)))
                    {
                        dinamicGeneralStatisticsDepartmentFilter += $"(z.ReportDate BETWEEN \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, selectedMonth.Key)} 23:59:59\') OR ";
                    }

                    dinamicGeneralStatisticsDepartmentFilter = dinamicGeneralStatisticsDepartmentFilter.Substring(0, dinamicGeneralStatisticsDepartmentFilter.Length - 4);
                    dinamicGeneralStatisticsDepartmentFilter += $") AND m.InvolverDept = \'{involverPos}\')";

                    QueryModel<string> param = new();
                    param.Data = CommonLibrary.Base64Encode(joinA + dinamicGeneralStatisticsDepartmentFilter);

                    var resGeneralStat = await EPKRSService.getEPKRSGeneralIncidentAccidentStatistics(param);

                    dinamicGeneralStatisticsDepartmentFilter = string.Empty;
                    dinamicGeneralStatisticsDepartmentFilter += dinamicGeneralStatisticsDateFilter + $") AND d.InvolverDept = \'{involverPos}\'";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(joinB + dinamicGeneralStatisticsDepartmentFilter);

                    var resRegionalStats = await EPKRSService.getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmail(param);

                    dinamicGeneralStatisticsDepartmentFilter = string.Empty;
                    dinamicGeneralStatisticsDepartmentFilter += dinamicGeneralStatisticsDateFilter + $") AND b.InvolverDept = \'{involverPos}\'!_!AND (";

                    foreach (var selectedMonth in monthSelector.Where(x => x.Value.Equals(true)))
                    {
                        dinamicGeneralStatisticsDepartmentFilter += $"(x.ReportDate BETWEEN \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilter}-{selectedMonth.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, selectedMonth.Key)} 23:59:59\') OR ";
                    }

                    dinamicGeneralStatisticsDepartmentFilter = dinamicGeneralStatisticsDepartmentFilter.Substring(0, dinamicGeneralStatisticsDepartmentFilter.Length - 4);
                    dinamicGeneralStatisticsDepartmentFilter += $") AND z.InvolverDept = \'{involverPos}\'";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(dinamicGeneralStatisticsDepartmentFilter);

                    var resInvolvedbyPos = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPosition(param);
                    //var resInvolvedbyDept = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDept(CommonLibrary.Base64Encode(dinamicGeneralStatisticsDepartmentFilter));

                    if (resGeneralStat.isSuccess)
                    {
                        previewGeneralStatistics = new();
                        previewGeneralStatistics = resGeneralStat.Data;
                    }

                    if (resRegionalStats.isSuccess)
                    {
                        previewRegionalStatistics = new();
                        previewRegionalStatistics = resRegionalStats.Data;
                    }

                    if (resInvolvedbyPos.isSuccess)
                    {
                        previewInvolvedbyPosStatistics = new();
                        previewInvolvedbyPosStatistics = resInvolvedbyPos.Data;
                    }

                    //if (resInvolvedbyDept.isSuccess)
                    //{
                    //    previewInvolvedbyDeptStatistics = new();
                    //    previewInvolvedbyDeptStatistics = resInvolvedbyDept.Data;
                    //}
                }
                else
                {
                    dinamicGeneralStatisticsDepartmentFilter = $"INNER JOIN (SELECT DISTINCT l.DocumentID, l.InvolverDept FROM [EPKRS].IncidentAccidentInvolverDetails l) b on a.DocumentID = b.DocumentID WHERE b.InvolverDept = \'{involverPos}\'!_!AND m.InvolverDept = \'{involverPos}\'";

                    QueryModel<string> param = new();
                    param.Data = CommonLibrary.Base64Encode(dinamicGeneralStatisticsDepartmentFilter);

                    var resGeneralStat = await EPKRSService.getEPKRSGeneralIncidentAccidentStatistics(param);

                    dinamicGeneralStatisticsDepartmentFilter = $"INNER JOIN (SELECT DISTINCT l.DocumentID, l.InvolverDept FROM [EPKRS].IncidentAccidentInvolverDetails l) d on a.DocumentID = d.DocumentID WHERE d.InvolverDept = \'{involverPos}\'";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(dinamicGeneralStatisticsDepartmentFilter);

                    var resRegionalStats = await EPKRSService.getEPKRSIncidentAccidentRegionalStatisticsbyDORMEmail(param);

                    dinamicGeneralStatisticsDepartmentFilter = $"WHERE b.InvolverDept = \'{involverPos}\'!_!AND z.InvolverDept = \'{involverPos}\'";
                    param = new();
                    param.Data = CommonLibrary.Base64Encode(dinamicGeneralStatisticsDepartmentFilter);

                    var resInvolvedbyPos = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverPosition(param);
                    //var resInvolvedbyDept = await EPKRSService.getEPKRSIncidentAccidentInvolverStatisticsbyInvolverDept(CommonLibrary.Base64Encode(dinamicGeneralStatisticsPositionFilter));

                    if (resGeneralStat.isSuccess)
                    {
                        previewGeneralStatistics = new();
                        previewGeneralStatistics = resGeneralStat.Data;
                    }

                    if (resRegionalStats.isSuccess)
                    {
                        previewRegionalStatistics = new();
                        previewRegionalStatistics = resRegionalStats.Data;
                    }

                    if (resInvolvedbyPos.isSuccess)
                    {
                        previewInvolvedbyPosStatistics = new();
                        previewInvolvedbyPosStatistics = resInvolvedbyPos.Data;
                    }

                    //if (resInvolvedbyDept.isSuccess)
                    //{
                    //    previewInvolvedbyDeptStatistics = new();
                    //    previewInvolvedbyDeptStatistics = resInvolvedbyDept.Data;
                    //}
                }

                dinamicGeneralStatisticsDepartmentFilter = string.Empty;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} from {ex.Source} {ex.InnerException} !");
            }
        }

        private async Task monitoringPanelFilterbyYear(int year, bool isReset)
        {
            try
            {
                //chartTriggered = false;
                dinamicMonitoringPanelDateFilter = string.Empty;
                selectedYearFilterChart = year;

                previewTopLocationReportStatistics = null;
                previewItemCaseCategoryStatistics = null;
                previewItemCategoryStatistics = null;

                string itemCaseCategoryConditions = $"WHERE a.AuditActionDate BETWEEN \'{year}-01-01 00:00:00\' AND \'{year}-12-31 23:59:59\'!_!AND (p.ReportDate BETWEEN \'{year}-01-01 00:00:00\' AND \'{year}-12-31 23:59:59\')";
                var resItemCaseCategoryStat = await EPKRSService.getEPKRSItemCaseCategoryStatistics(CommonLibrary.Base64Encode(itemCaseCategoryConditions));

                string topLocationReportConditions = $"WHERE a.ReportDate BETWEEN \'{year}-01-01 00:00:00\' AND \'{year}-12-31 23:59:59\'!_!AND (p.ReportDate BETWEEN \'{year}-01-01 00:00:00\' AND \'{year}-12-31 23:59:59\')";
                var resTopLocationReportStat = await EPKRSService.getEPKRSTopLocationReportStatistics(CommonLibrary.Base64Encode(topLocationReportConditions));

                string itemCategoryStatsConditions = $"WHERE a.AuditActionDate BETWEEN \'{year}-01-01 00:00:00\' AND \'{year}-12-31 23:59:59\'!_!AND (p.ReportDate BETWEEN \'{year}-01-01 00:00:00\' AND \'{year}-12-31 23:59:59\')";
                var resItemCategoryStat = await EPKRSService.getEPKRSItemCategoriesStatistics(CommonLibrary.Base64Encode(itemCategoryStatsConditions));

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

                if (isReset)
                {
                    foreach (var dt in monthSelectorChart)
                    {
                        monthSelectorChart[dt.Key] = false;
                    }

                    monitoringPanelFiltered = false;
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
                    monitoringPanelFiltered = true;
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

                    dinamicMonitoringPanelDateFilterExt = dinamicMonitoringPanelDateFilterExt.Substring(0, dinamicMonitoringPanelDateFilterExt.Length - 4);
                    dinamicMonitoringPanelDateFilterExt += ")";
                    dinamicMonitoringPanelDateFilter = dinamicMonitoringPanelDateFilter.Substring(0, dinamicMonitoringPanelDateFilter.Length - 4);
                    dinamicMonitoringPanelDateFilter += ")";
                    var resItemCaseCategoryStat = await EPKRSService.getEPKRSItemCaseCategoryStatistics(CommonLibrary.Base64Encode(dinamicMonitoringPanelDateFilter + dinamicMonitoringPanelDateFilterExt));

                    dinamicMonitoringPanelDateFilter = "WHERE (";

                    foreach (var selectedMonth in monthSelectorChart.Where(x => x.Value.Equals(true)))
                    {
                        dinamicMonitoringPanelDateFilter += $"(a.ReportDate BETWEEN \'{selectedYearFilterChart}-{selectedMonth.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilterChart}-{selectedMonth.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, selectedMonth.Key)} 23:59:59\') OR ";
                    }

                    dinamicMonitoringPanelDateFilter = dinamicMonitoringPanelDateFilter.Substring(0, dinamicMonitoringPanelDateFilter.Length - 4);
                    dinamicMonitoringPanelDateFilter += ")";
                    var resTopLocationReportStat = await EPKRSService.getEPKRSTopLocationReportStatistics(CommonLibrary.Base64Encode(dinamicMonitoringPanelDateFilter + dinamicMonitoringPanelDateFilterExt));

                    dinamicMonitoringPanelDateFilter = "WHERE (";

                    foreach (var selectedMonth in monthSelectorChart.Where(x => x.Value.Equals(true)))
                    {
                        dinamicMonitoringPanelDateFilter += $"(a.AuditActionDate BETWEEN \'{selectedYearFilterChart}-{selectedMonth.Key.ToString("00")}-01 00:00:00\' AND \'{selectedYearFilterChart}-{selectedMonth.Key.ToString("00")}-{DateTime.DaysInMonth(selectedYearFilter, selectedMonth.Key)} 23:59:59\') OR ";
                    }

                    dinamicMonitoringPanelDateFilter = dinamicMonitoringPanelDateFilter.Substring(0, dinamicMonitoringPanelDateFilter.Length - 4);
                    dinamicMonitoringPanelDateFilter += ")";
                    var resItemCategoryStat = await EPKRSService.getEPKRSItemCategoriesStatistics(CommonLibrary.Base64Encode(dinamicMonitoringPanelDateFilter + dinamicMonitoringPanelDateFilterExt));

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

        private void resetExportDate()
        {
            startDateExport = new(2023, 1, 1);
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
