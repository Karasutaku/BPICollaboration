using BPILibrary;
using BPIWebApplication.Client.Services.CashierLogbookServices;
using BPIWebApplication.Client.Services.ManagementServices;
using BPIWebApplication.Client.Services.PettyCashServices;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.EPKRS;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.PagesModel.EPKRS;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;

namespace BPIWebApplication.Client.Pages.EPKRSPages
{
    public partial class AddEPKRS : ComponentBase
    {
        [Parameter]
        public string? param { get; set; } = null;

        private ActiveUser activeUser = new();
        private UserPrivileges privilegeDataParam = new();
        private List<string> userPriv = new();

        private Location paramGetCompanyLocation = new();

        private ItemCaseForm itemCaseData = new();
        private List<ItemLineForm> itemLineData = new();
        private ItemLineForm itemLineExtender = new();
        private IncidentAccidentForm incidentaccidentData = new();
        private List<CaseAttachmentForm> caseAttachments = new();
        private List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileUploads = new();

        private List<LoadingManifestResp>? lmData = null;
        private List<LoadingManifestResp> selectedLMData = new();

        private string ReportingType { get; set; } = string.Empty;
        private string RiskID { get; set; } = string.Empty;

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

        private string previousLoadedLM = string.Empty;

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

            paramGetCompanyLocation.Condition = $"a.CompanyId={Convert.ToInt32(activeUser.company)}";
            paramGetCompanyLocation.PageIndex = 1;
            paramGetCompanyLocation.PageSize = 100;
            paramGetCompanyLocation.FieldOrder = "a.CompanyId";
            paramGetCompanyLocation.MethodOrder = "ASC";

            await ManagementService.GetCompanyLocations(paramGetCompanyLocation);

            await EPKRSService.getEPRKSReportingType();
            await EPKRSService.getEPRKSRiskType();
            await EPKRSService.getEPRKSRiskSubType();
            await EPKRSService.getEPKRSItemRiskCategory();
            await ManagementService.GetAllDepartment(CommonLibrary.Base64Encode(loc));
            maxFileSize = await EPKRSService.getEPKRSMaxFileSize();

            formPage = 1;
            ReportingType = "BLANK";
            incidentaccidentData.SiteReporter = loc;
            itemCaseData.SiteReporter = loc;

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/EPKRSPages/addEPKRS.razor.js");
        }

        protected override async Task OnParametersSetAsync()
        {
            if (param != null)
            {
                string[] temp = CommonLibrary.Base64Decode(param).Split("!_!");

                if (temp[0].Equals("ITEMC"))
                {
                    var data = EPKRSService.itemCases.SingleOrDefault(x => x.itemCase.DocumentID.Equals(temp[1]));
                    
                    if (data != null)
                    {
                        ReportingType = temp[0];
                        var type = EPKRSService.riskSubTypes.SingleOrDefault(x => x.SubRiskID.Equals(data.itemCase.SubRiskID));
                        
                        if (type != null)
                            RiskID = type.RiskID;

                        itemCaseData = new ItemCaseForm
                        {
                            SubRiskID = data.itemCase.SubRiskID,
                            DocumentID = data.itemCase.DocumentID,
                            SiteReporter = data.itemCase.SiteReporter,
                            SiteSender = data.itemCase.SiteSender,
                            ReportDate = data.itemCase.ReportDate,
                            ItemPickupDate = data.itemCase.ItemPickupDate,
                            LoadingDocumentID = data.itemCase.LoadingDocumentID,
                            LoadingDocumentDate = data.itemCase.LoadingDocumentDate,
                            ExtendedMitigationPlan = data.itemCase.ExtendedMitigationPlan,
                            DocumentStatus = data.itemCase.DocumentStatus
                        };

                        data.itemLine.ForEach(x =>
                        {
                            itemLineData.Add(new ItemLineForm
                            {
                                DocumentID = x.DocumentID,
                                LineNum = x.LineNum,
                                TRID = x.TRID,
                                TRDate = x.TRDate,
                                ItemCode = x.ItemCode,
                                ItemDescription = x.ItemDescription,
                                ItemRiskCategoryID = x.ItemRiskCategoryID,
                                CategoryID = x.CategoryID,
                                ItemQuantity = x.ItemQuantity,
                                UOM = x.UOM,
                                ItemValue = x.ItemValue,
                                ItemStock = x.ItemStock,
                                VarianceDate = x.VarianceDate,
                                isLate = x.isLate ? "TRUE" : "False",
                                isCCTVCoverable = x.isCCTVCoverable ? "TRUE" : "FALSE",
                                isReportedtoSender = x.isReportedtoSender ? "TRUE" : "FALSE"
                            });
                        });
                    }
                }
                else if (temp[0].Equals("INCAC"))
                {
                    var data = EPKRSService.incidentAccidents.SingleOrDefault(x => x.incidentAccident.DocumentID.Equals(temp[1]));

                    if (data != null)
                    {
                        ReportingType = temp[0];
                        var type = EPKRSService.riskSubTypes.SingleOrDefault(x => x.SubRiskID.Equals(data.incidentAccident.SubRiskID));

                        if (type != null)
                            RiskID = type.RiskID;

                        incidentaccidentData = new IncidentAccidentForm
                        {
                            SubRiskID = data.incidentAccident.SubRiskID,
                            DocumentID = data.incidentAccident.DocumentID,
                            ReportDate = data.incidentAccident.ReportDate,
                            OccurenceDate = data.incidentAccident.OccurenceDate,
                            SiteReporter = data.incidentAccident.SiteReporter,
                            DepartmentReporter = data.incidentAccident.DepartmentReporter,
                            RiskRPName = data.incidentAccident.RiskRPName,
                            RiskRPEmail = data.incidentAccident.RiskRPEmail,
                            DORMName = data.incidentAccident.DORMName,
                            DORMEmail = data.incidentAccident.DORMEmail,
                            CaseDescription = data.incidentAccident.CaseDescription,
                            DepartmentAffected = data.incidentAccident.DepartmentAffected,
                            Cronology = data.incidentAccident.Cronology,
                            RootCause = data.incidentAccident.RootCause,
                            LossDescription = data.incidentAccident.LossDescription,
                            LossEstimation = data.incidentAccident.LossEstimation,
                            ReturnAmount = data.incidentAccident.ReturnAmount,
                            RiskDescription = data.incidentAccident.RiskDescription,
                            CauseDescription = data.incidentAccident.CauseDescription,
                            PIC = data.incidentAccident.PIC,
                            ActionPlan = data.incidentAccident.ActionPlan,
                            TargetDate = data.incidentAccident.TargetDate,
                            MitigationPlan = data.incidentAccident.MitigationPlan,
                            MitigationDate = data.incidentAccident.MitigationDate,
                            ExtendedRootCause = data.incidentAccident.ExtendedRootCause,
                            ExtendedMitigationPlan = data.incidentAccident.ExtendedMitigationPlan,
                            DocumentStatus = data.incidentAccident.DocumentStatus
                        };
                    }
                }

                StateHasChanged();
            }
        }

        private async void UploadHandleSelection(InputFileChangeEventArgs files, BPIWebApplication.Shared.MainModel.Stream.FileStream line)
        {
            listFileUpload = files.GetMultipleFiles();
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

                            //line.type = "";
                            line.fileName = Path.GetRandomFileName() + "!_!" + fi.Name;
                            //line.fileDesc = "";
                            line.fileType = ext;
                            line.fileSize = Convert.ToInt32(file.Size);
                            line.content = ms.ToArray();

                            stream.Close();
                        }
                    }
                }
            }

            this.StateHasChanged();
        }

        private void appendSelectedLMItem(LoadingManifestResp data)
        {
            if (selectedLMData.FirstOrDefault(x => x.siteNo.Equals(data.siteNo) && x.trNo.Equals(data.trNo) && x.itemCode.Equals(data.itemCode)) == null)
            {
                selectedLMData.Add(new LoadingManifestResp
                {
                    lmNo = data.lmNo,
                    createdDate = data.createdDate,
                    siteNo = data.siteNo,
                    trNo = data.trNo,
                    sentRequestDate = data.sentRequestDate,
                    itemCode = data.itemCode,
                    itemDesc = data.itemDesc,
                    itemValue = data.itemValue,
                    uom = data.uom
                });
            }
            else
            {
                var dt = selectedLMData.SingleOrDefault(x => x.siteNo.Equals(data.siteNo) && x.trNo.Equals(data.trNo) && x.itemCode.Equals(data.itemCode));
                if (dt != null)
                {
                    selectedLMData.Remove(dt);
                }
            }
        }

        private async void selectItembyLoadingDocument()
        {
            if (selectedLMData.Count > 0)
            {
                itemLineData.Clear();

                int n = 0;
                selectedLMData.ForEach(x =>
                {
                    n++;
                    itemLineData.Add(new ItemLineForm
                    {
                        DocumentID = "",
                        LineNum = n,
                        TRID = x.trNo,
                        TRDate = Convert.ToDateTime(x.sentRequestDate),
                        ItemCode = x.itemCode,
                        ItemDescription = x.itemDesc,
                        ItemRiskCategoryID = "BLANK",
                        CategoryID = x.itemCode.Substring(0, 2),
                        ItemQuantity = 0,
                        UOM = x.uom,
                        ItemValue = x.itemValue,
                        ItemStock = 0,
                        VarianceDate = 0,
                        isLate = "FALSE",
                        isCCTVCoverable = "FALSE",
                        isReportedtoSender = "FALSE"
                    });
                });

                var lmdate = selectedLMData.DistinctBy(x => x.createdDate);

                if (lmdate.Count() > 1)
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "NP Type is More than 1 Type : Please Contact IT OPS !");
                    return;
                }

                itemCaseData.LoadingDocumentDate = Convert.ToDateTime(lmdate.First().createdDate);
            }

            StateHasChanged();
        }

        private async Task selectItembyLoadingDocumentID()
        {
            try
            {
                if (previousLoadedLM == itemCaseData.LoadingDocumentID)
                    return;

                if (!(itemCaseData.LoadingDocumentID.Length > 0))
                {
                    await _jsModule.InvokeVoidAsync("showAlert", $"Input Loading Manifest ID !");
                }
                else
                {
                    isMiniLoading = true;

                    LMNotoTMS param = new();
                    param.lmNo = itemCaseData.LoadingDocumentID;

                    #pragma warning disable CS4014
                    Task.Run(async () =>
                    {
                        var res = await EPKRSService.getEPKRSItemDetailsbyLMNo(param, activeUser.token);

                        if (res.isSuccess)
                        {
                            lmData = new();
                            lmData = res.Data;

                            previousLoadedLM = itemCaseData.LoadingDocumentID;
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

        private bool validateIncidentAccident()
        {
            if (RiskID.IsNullOrEmpty() || RiskID.Equals("BLANK")) { return false; }
            if (incidentaccidentData.SubRiskID.IsNullOrEmpty() || incidentaccidentData.SubRiskID.Equals("BLANK")) { return false; }
            //if (incidentaccidentData.DocumentID.IsNullOrEmpty()) { return false; }
            //incidentaccidentData.ReportDate
            //incidentaccidentData.OccurenceDate
            if (incidentaccidentData.SiteReporter.IsNullOrEmpty()) { return false; }
            if (incidentaccidentData.DepartmentReporter.IsNullOrEmpty()) { return false; }
            if (incidentaccidentData.RiskRPName.IsNullOrEmpty()) { return false; }
            if (incidentaccidentData.RiskRPEmail.IsNullOrEmpty()) { return false; }
            if (incidentaccidentData.DORMName.IsNullOrEmpty()) { return false; }
            if (incidentaccidentData.DORMEmail.IsNullOrEmpty()) { return false; }
            if (incidentaccidentData.CaseDescription.IsNullOrEmpty()) { return false; }
            if (incidentaccidentData.DepartmentAffected.IsNullOrEmpty()) { return false; }
            if (incidentaccidentData.Cronology.IsNullOrEmpty()) { return false; }
            if (incidentaccidentData.RootCause.IsNullOrEmpty()) { return false; }
            if (incidentaccidentData.LossDescription.IsNullOrEmpty()) { return false; }
            if (incidentaccidentData.LossEstimation < 0) { return false; }
            if (incidentaccidentData.ReturnAmount < 0) { return false; }
            if (incidentaccidentData.RiskDescription.IsNullOrEmpty()) { return false; }
            if (incidentaccidentData.CauseDescription.IsNullOrEmpty()) { return false; }
            if (incidentaccidentData.PIC.IsNullOrEmpty()) { return false; }
            if (incidentaccidentData.ActionPlan.IsNullOrEmpty()) { return false; }
            //incidentaccidentData.TargetDate
            if (incidentaccidentData.MitigationPlan.IsNullOrEmpty()) { return false; }
            //incidentaccidentData.MitigationDate
            //incidentaccidentData.ExtendedRootCause
            //incidentaccidentData.ExtendedMitigationPlan
            //incidentaccidentData.DocumentStatus

            return true;
        }

        private bool validateItemCase()
        {
            if (RiskID.IsNullOrEmpty() || RiskID.Equals("BLANK")) { return false; }
            if (itemCaseData.SubRiskID.IsNullOrEmpty() || itemCaseData.SubRiskID.Equals("BLANK")) { return false; }
            //itemCaseData.DocumentID
            if (itemCaseData.SiteReporter.IsNullOrEmpty()) { return false; }
            if (itemCaseData.SiteSender.IsNullOrEmpty()) { return false; }
            //itemCaseData.ReportDate
            //itemCaseData.ItemPickupDate
            if (itemCaseData.LoadingDocumentID.IsNullOrEmpty()) { return false; }
            //itemCaseData.LoadingDocumentDate
            //if (itemCaseData.ExtendedMitigationPlan.IsNullOrEmpty()) { return false; }
            //itemCaseData.DocumentStatus
            //itemLineData.DocumentID
            if (itemLineData.Count < 1) { return false; }
            if (itemLineData.Any(x => x.LineNum <= 0)) { return false; }
            if (itemLineData.Any(x => x.TRID.IsNullOrEmpty())) { return false; }
            //if (itemLineData.Any(x => x.TRDate)) { }
            if (itemLineData.Any(x => x.ItemCode.IsNullOrEmpty())) { return false; }
            if (itemLineData.Any(x => x.ItemDescription.IsNullOrEmpty())) { return false; }
            if (itemLineData.Any(x => x.ItemRiskCategoryID.Equals("BLANK"))) { return false; }
            //if (itemLineData.Any(x => x.).CategoryID) { }
            if (itemLineData.Any(x => x.ItemQuantity <= 0)) { return false; }
            if (itemLineData.Any(x => x.UOM.IsNullOrEmpty())) { return false; }
            if (itemLineData.Any(x => x.ItemValue < 0)) { return false; }
            if (itemLineData.Any(x => x.ItemStock < 0)) { return false; }
            //if (itemLineData.Any(x => x.).VarianceDate) { }
            if (itemLineData.Any(x => x.isLate.Equals("BLANK"))) { return false; }
            if (itemLineData.Any(x => x.isCCTVCoverable.Equals("BLANK"))) { return false; }
            if (itemLineData.Any(x => x.isReportedtoSender.Equals("BLANK"))) { return false; }

            return true;
        }

        private async void submitIncidentAccident()
        {
            try
            {
                if (!validateIncidentAccident())
                {
                    StateHasChanged();
                    await _jsModule.InvokeVoidAsync("showAlert", "Form Validation : Please Check your input Field !");
                    return;
                }

                isLoading = true;
                StateHasChanged();

                IncidentAccidentStream uploadData = new();
                uploadData.mainData = new();
                uploadData.mainData.Data = new();
                uploadData.mainData.Data.incidentAccident = new();
                uploadData.mainData.Data.incidentAccident.SubRiskID = incidentaccidentData.SubRiskID;
                uploadData.mainData.Data.incidentAccident.DocumentID = "";
                uploadData.mainData.Data.incidentAccident.ReportDate = incidentaccidentData.ReportDate;
                uploadData.mainData.Data.incidentAccident.OccurenceDate = incidentaccidentData.OccurenceDate;
                uploadData.mainData.Data.incidentAccident.SiteReporter = incidentaccidentData.SiteReporter;
                uploadData.mainData.Data.incidentAccident.DepartmentReporter = incidentaccidentData.DepartmentReporter;
                uploadData.mainData.Data.incidentAccident.RiskRPName = incidentaccidentData.RiskRPName;
                uploadData.mainData.Data.incidentAccident.RiskRPEmail = incidentaccidentData.RiskRPEmail.ToLower();
                uploadData.mainData.Data.incidentAccident.DORMName = incidentaccidentData.DORMName;
                uploadData.mainData.Data.incidentAccident.DORMEmail = incidentaccidentData.DORMEmail.ToLower();
                uploadData.mainData.Data.incidentAccident.CaseDescription = incidentaccidentData.CaseDescription;
                uploadData.mainData.Data.incidentAccident.DepartmentAffected = incidentaccidentData.DepartmentAffected;
                uploadData.mainData.Data.incidentAccident.Cronology = incidentaccidentData.Cronology;
                uploadData.mainData.Data.incidentAccident.RootCause = incidentaccidentData.RootCause;
                uploadData.mainData.Data.incidentAccident.LossDescription = incidentaccidentData.LossDescription;
                uploadData.mainData.Data.incidentAccident.LossEstimation = incidentaccidentData.LossEstimation;
                uploadData.mainData.Data.incidentAccident.ReturnAmount = incidentaccidentData.ReturnAmount;
                uploadData.mainData.Data.incidentAccident.RiskDescription = incidentaccidentData.RiskDescription;
                uploadData.mainData.Data.incidentAccident.CauseDescription = incidentaccidentData.CauseDescription;
                uploadData.mainData.Data.incidentAccident.PIC = incidentaccidentData.PIC.ToLower();
                uploadData.mainData.Data.incidentAccident.ActionPlan = incidentaccidentData.ActionPlan;
                uploadData.mainData.Data.incidentAccident.TargetDate = incidentaccidentData.TargetDate;
                uploadData.mainData.Data.incidentAccident.MitigationPlan = incidentaccidentData.MitigationPlan;
                uploadData.mainData.Data.incidentAccident.MitigationDate = incidentaccidentData.MitigationDate;
                uploadData.mainData.Data.incidentAccident.ExtendedRootCause = "";
                uploadData.mainData.Data.incidentAccident.ExtendedMitigationPlan = "";
                uploadData.mainData.Data.incidentAccident.DocumentStatus = "Open";

                uploadData.mainData.Data.attachment = new();
                int c = 0;

                fileUploads.ForEach(x =>
                {
                    uploadData.mainData.Data.attachment.Add(new CaseAttachment
                    {
                        DocumentID = "",
                        LineNum = c + 1,
                        UploadDate = DateTime.Now,
                        FileExtension = x.fileType,
                        FilePath = x.fileName
                    });

                    c++;
                });

                uploadData.mainData.userEmail = activeUser.userName;
                uploadData.mainData.userAction = "I";
                uploadData.mainData.userActionDate = DateTime.Now;

                uploadData.files = new();
                uploadData.files = fileUploads;

                var res = await EPKRSService.createEPKRSIncidentAccidentDocument(uploadData);

                if (res.isSuccess)
                {
                    successUpload = true;
                    incidentaccidentData.DocumentID = res.Data.mainData.Data.incidentAccident.DocumentID;
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

        private async void editIncidentAccident()
        {
            try
            {
                if (!validateIncidentAccident())
                {
                    StateHasChanged();
                    await _jsModule.InvokeVoidAsync("showAlert", "Form Validation : Please Check your input Field !");
                    return;
                }

                isLoading = true;
                StateHasChanged();

                QueryModel<IncidentAccident> uploadData = new();
                uploadData.Data = new();
                uploadData.Data.SubRiskID = incidentaccidentData.SubRiskID;
                uploadData.Data.DocumentID = incidentaccidentData.DocumentID;
                uploadData.Data.ReportDate = incidentaccidentData.ReportDate;
                uploadData.Data.OccurenceDate = incidentaccidentData.OccurenceDate;
                uploadData.Data.SiteReporter = incidentaccidentData.SiteReporter;
                uploadData.Data.DepartmentReporter = incidentaccidentData.DepartmentReporter;
                uploadData.Data.RiskRPName = incidentaccidentData.RiskRPName;
                uploadData.Data.RiskRPEmail = incidentaccidentData.RiskRPEmail.ToLower();
                uploadData.Data.DORMName = incidentaccidentData.DORMName;
                uploadData.Data.DORMEmail = incidentaccidentData.DORMEmail.ToLower();
                uploadData.Data.CaseDescription = incidentaccidentData.CaseDescription;
                uploadData.Data.DepartmentAffected = incidentaccidentData.DepartmentAffected;
                uploadData.Data.Cronology = incidentaccidentData.Cronology;
                uploadData.Data.RootCause = incidentaccidentData.RootCause;
                uploadData.Data.LossDescription = incidentaccidentData.LossDescription;
                uploadData.Data.LossEstimation = incidentaccidentData.LossEstimation;
                uploadData.Data.ReturnAmount = incidentaccidentData.ReturnAmount;
                uploadData.Data.RiskDescription = incidentaccidentData.RiskDescription;
                uploadData.Data.CauseDescription = incidentaccidentData.CauseDescription;
                uploadData.Data.PIC = incidentaccidentData.PIC.ToLower();
                uploadData.Data.ActionPlan = incidentaccidentData.ActionPlan;
                uploadData.Data.TargetDate = incidentaccidentData.TargetDate;
                uploadData.Data.MitigationPlan = incidentaccidentData.MitigationPlan;
                uploadData.Data.MitigationDate = incidentaccidentData.MitigationDate;
                uploadData.Data.ExtendedRootCause = incidentaccidentData.ExtendedRootCause;
                uploadData.Data.ExtendedMitigationPlan = incidentaccidentData.ExtendedMitigationPlan;
                uploadData.Data.DocumentStatus = incidentaccidentData.DocumentStatus;

                uploadData.userEmail = activeUser.userName;
                uploadData.userAction = "I";
                uploadData.userActionDate = DateTime.Now;

                var res = await EPKRSService.editEPKRSIncidentAccidentData(uploadData);

                if (res.isSuccess)
                {
                    successUpload = true;
                    await _jsModule.InvokeVoidAsync("showAlert", "Data Edit Success !");
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

        private async void submitItemCase()
        {
            try
            {
                if (!validateItemCase())
                {
                    StateHasChanged();
                    await _jsModule.InvokeVoidAsync("showAlert", "Form Validation : Please Check your input Field !");
                    return;
                }

                isLoading = true;
                StateHasChanged();

                ItemCaseStream uploadData = new();
                uploadData.mainData = new();
                uploadData.mainData.Data = new();
                uploadData.mainData.Data.itemCase = new();
                uploadData.mainData.Data.itemCase.SubRiskID = itemCaseData.SubRiskID;
                uploadData.mainData.Data.itemCase.DocumentID = "";
                uploadData.mainData.Data.itemCase.SiteReporter = itemCaseData.SiteReporter;
                uploadData.mainData.Data.itemCase.SiteSender = itemCaseData.SiteSender;
                uploadData.mainData.Data.itemCase.ReportDate = itemCaseData.ReportDate;
                uploadData.mainData.Data.itemCase.ItemPickupDate = itemCaseData.ItemPickupDate;
                uploadData.mainData.Data.itemCase.LoadingDocumentID = itemCaseData.LoadingDocumentID;
                uploadData.mainData.Data.itemCase.LoadingDocumentDate = itemCaseData.LoadingDocumentDate;
                uploadData.mainData.Data.itemCase.ExtendedMitigationPlan = "";
                uploadData.mainData.Data.itemCase.DocumentStatus = "Open";

                int n = 0;

                uploadData.mainData.Data.itemLine = new();
                itemLineData.ForEach(x =>
                {
                    n++;
                    var parseLate = bool.TryParse(x.isLate, out var result1) ? result1 : result1;
                    var parseCctv = bool.TryParse(x.isCCTVCoverable, out var result2) ? result2 : result2;
                    var parseReported = bool.TryParse(x.isReportedtoSender, out var result3) ? result3 : result3;

                    uploadData.mainData.Data.itemLine.Add(new ItemLine
                    {
                        DocumentID = "",
                        LineNum = n,
                        TRID = x.TRID,
                        TRDate = x.TRDate,
                        ItemCode = x.ItemCode,
                        ItemDescription = x.ItemDescription,
                        ItemRiskCategoryID = x.ItemRiskCategoryID,
                        CategoryID = x.CategoryID,
                        ItemQuantity = x.ItemQuantity,
                        UOM = x.UOM,
                        ItemValue = x.ItemValue,
                        ItemStock = x.ItemStock,
                        VarianceDate = (itemCaseData.ItemPickupDate - x.TRDate).Days,
                        isLate = parseLate,
                        isCCTVCoverable = parseCctv,
                        isReportedtoSender = parseReported
                    });
                });

                uploadData.mainData.Data.attachment = new();
                int c = 0;

                fileUploads.ForEach(x =>
                {
                    uploadData.mainData.Data.attachment.Add(new CaseAttachment
                    {
                        DocumentID = "",
                        LineNum = c + 1,
                        UploadDate = DateTime.Now,
                        FileExtension = x.fileType,
                        FilePath = x.fileName
                    });

                    c++;
                });

                uploadData.mainData.userEmail = activeUser.userName;
                uploadData.mainData.userAction = "I";
                uploadData.mainData.userActionDate = DateTime.Now;

                uploadData.files = new();
                uploadData.files = fileUploads;

                var res = await EPKRSService.createEPKRSItemCaseDocument(uploadData);

                if (res.isSuccess)
                {
                    successUpload = true;
                    itemCaseData.DocumentID = res.Data.mainData.Data.itemCase.DocumentID;
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
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} !");
            }
        }

        private async void editItemCase()
        {
            try
            {
                if (!validateItemCase())
                {
                    StateHasChanged();
                    await _jsModule.InvokeVoidAsync("showAlert", "Form Validation : Please Check your input Field !");
                    return;
                }

                isLoading = true;
                StateHasChanged();

                QueryModel<EPKRSUploadItemCase> uploadData = new();
                uploadData = new();
                uploadData.Data = new();
                uploadData.Data.itemCase = new();
                uploadData.Data.itemCase.SubRiskID = itemCaseData.SubRiskID;
                uploadData.Data.itemCase.DocumentID = itemCaseData.DocumentID;
                uploadData.Data.itemCase.SiteReporter = itemCaseData.SiteReporter;
                uploadData.Data.itemCase.SiteSender = itemCaseData.SiteSender;
                uploadData.Data.itemCase.ReportDate = itemCaseData.ReportDate;
                uploadData.Data.itemCase.ItemPickupDate = itemCaseData.ItemPickupDate;
                uploadData.Data.itemCase.LoadingDocumentID = itemCaseData.LoadingDocumentID;
                uploadData.Data.itemCase.LoadingDocumentDate = itemCaseData.LoadingDocumentDate;
                uploadData.Data.itemCase.ExtendedMitigationPlan = itemCaseData.ExtendedMitigationPlan;
                uploadData.Data.itemCase.DocumentStatus = itemCaseData.DocumentStatus;

                int n = 0;

                uploadData.Data.itemLine = new();
                itemLineData.ForEach(x =>
                {
                    n++;
                    var parseLate = bool.TryParse(x.isLate, out var result1) ? result1 : result1;
                    var parseCctv = bool.TryParse(x.isCCTVCoverable, out var result2) ? result2 : result2;
                    var parseReported = bool.TryParse(x.isReportedtoSender, out var result3) ? result3 : result3;

                    uploadData.Data.itemLine.Add(new ItemLine
                    {
                        DocumentID = itemCaseData.DocumentID,
                        LineNum = n,
                        TRID = x.TRID,
                        TRDate = x.TRDate,
                        ItemCode = x.ItemCode,
                        ItemDescription = x.ItemDescription,
                        ItemRiskCategoryID = x.ItemRiskCategoryID,
                        CategoryID = x.CategoryID,
                        ItemQuantity = x.ItemQuantity,
                        UOM = x.UOM,
                        ItemValue = x.ItemValue,
                        ItemStock = x.ItemStock,
                        VarianceDate = x.VarianceDate,
                        isLate = parseLate,
                        isCCTVCoverable = parseCctv,
                        isReportedtoSender = parseReported
                    });
                });

                uploadData.userEmail = activeUser.userName;
                uploadData.userAction = "I";
                uploadData.userActionDate = DateTime.Now;

                var res = await EPKRSService.editEPKRSItemCaseData(uploadData);

                if (res.isSuccess)
                {
                    successUpload = true;
                    await _jsModule.InvokeVoidAsync("showAlert", "Edit Data Success !");
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
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message} !");
            }
        }

        private async void onFailedSubmit()
        {
            await _jsModule.InvokeVoidAsync("showAlert", "Failed : Please Check Your Input Field");
        }

        private void reportingTypeonChange(ChangeEventArgs e)
        {
            ReportingType = e.Value.ToString();

            RiskID = "BLANK";
            itemCaseData.SubRiskID = "BLANK";
            incidentaccidentData.SubRiskID = "BLANK";

            formPage = 1;
            caseAttachments.Clear();
            itemLineData.Clear();
            fileUploads.Clear();

            itemCaseData = new();
            incidentaccidentData = new();

            itemCaseData.SiteReporter = activeUser.location.Equals("") ? "HO" : activeUser.location;
            incidentaccidentData.SiteReporter = activeUser.location.Equals("") ? "HO" : activeUser.location;

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

        private bool checkFileAttachment()
        {
            try
            {
                if (fileUploads.Any())
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

        private bool checkSubRiskType()
        {
            try
            {
                if (EPKRSService.riskSubTypes.Any())
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

        private bool checkItemRiskCategory()
        {
            try
            {
                if (EPKRSService.itemRiskCategories.Any())
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

        private bool checkLMData()
        {
            try
            {
                if (lmData != null)
                {
                    if (lmData.Any())
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

        //
    }
}
