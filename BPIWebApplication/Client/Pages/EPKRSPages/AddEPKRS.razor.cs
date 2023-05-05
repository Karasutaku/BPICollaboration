using BPILibrary;
using BPIWebApplication.Client.Services.CashierLogbookServices;
using BPIWebApplication.Client.Services.ManagementServices;
using BPIWebApplication.Client.Services.PettyCashServices;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel.EPKRS;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.PagesModel.EPKRS;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;

namespace BPIWebApplication.Client.Pages.EPKRSPages
{
    public partial class AddEPKRS : ComponentBase
    {
        [Parameter]
        public string? param { get; set; } = null;

        private ActiveUser activeUser = new();
        private UserPrivileges privilegeDataParam = new();
        private List<string> userPriv = new();

        private ItemCaseForm itemCaseData = new();
        private List<ItemLineForm> itemLineData = new();
        private IncidentAccidentForm incidentaccidentData = new();
        private List<CaseAttachmentForm> caseAttachments = new();
        private List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileUploads = new();

        private string RiskID { get; set; } = string.Empty;
        private string isLate { get; set; } = string.Empty;
        private string isCCTVCovered { get; set; } = string.Empty;
        private string isReportedtoSender { get; set; } = string.Empty;

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

        private bool isLoading = false;
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

            await EPKRSService.getEPRKSReportingType();
            await EPKRSService.getEPRKSRiskType();
            await ManagementService.GetAllDepartment(CommonLibrary.Base64Encode(loc));
            maxFileSize = await EPKRSService.getEPKRSMaxFileSize();

            formPage = 1;
            RiskID = "BLANK";
            isLate = "BLANK";
            isCCTVCovered = "BLANK";
            isReportedtoSender = "BLANK";
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
                        RiskID = temp[0];

                        itemCaseData = new ItemCaseForm
                        {
                            RiskID = data.itemCase.RiskID,
                            DocumentID = data.itemCase.DocumentID,
                            SiteReporter = data.itemCase.SiteReporter,
                            SiteSender = data.itemCase.SiteSender,
                            ReportDate = data.itemCase.ReportDate,
                            ItemPickupDate = data.itemCase.ItemPickupDate,
                            LoadingDocumentID = data.itemCase.LoadingDocumentID,
                            LoadingDocumentDate = data.itemCase.LoadingDocumentDate,
                            VarianceDate = data.itemCase.VarianceDate,
                            isLate = data.itemCase.isLate,
                            isCCTVCoverable = data.itemCase.isCCTVCoverable,
                            isReportedtoSender = data.itemCase.isReportedtoSender,
                            ExtendedMitigationPlan = data.itemCase.ExtendedMitigationPlan,
                            DocumentStatus = data.itemCase.DocumentStatus
                        };
                    }
                }
                else if (temp[0].Equals("INCAC"))
                {
                    var data = EPKRSService.incidentAccidents.SingleOrDefault(x => x.incidentAccident.DocumentID.Equals(temp[1]));

                    if (data != null)
                    {
                        RiskID = temp[0];

                        incidentaccidentData = new IncidentAccidentForm
                        {
                            RiskID = data.incidentAccident.RiskID,
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

        private void selectItembyLoadingDocumentID()
        {

        }

        private async void submitIncidentAccident()
        {
            try
            {
                isLoading = true;
                StateHasChanged();

                IncidentAccidentStream uploadData = new();
                uploadData.mainData = new();
                uploadData.mainData.Data = new();
                uploadData.mainData.Data.incidentAccident = new();
                uploadData.mainData.Data.incidentAccident.RiskID = incidentaccidentData.RiskID;
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
                isLoading = true;
                StateHasChanged();

                QueryModel<IncidentAccident> uploadData = new();
                uploadData.Data = new();
                uploadData.Data.RiskID = incidentaccidentData.RiskID;
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

        private void submitItemCase()
        {

        }

        private async void onFailedSubmit()
        {
            await _jsModule.InvokeVoidAsync("showAlert", "Failed : Please Check Your Input Field");
        }

        private void reportingTypeonChange(ChangeEventArgs e)
        {
            RiskID = e.Value.ToString();

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

        //
    }
}
