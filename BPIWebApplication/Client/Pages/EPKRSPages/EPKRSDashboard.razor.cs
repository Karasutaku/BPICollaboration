using BPILibrary;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel;
using BPIWebApplication.Shared.MainModel.EPKRS;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.PagesModel.EPKRS;
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

        private ItemCase updateItemCase = new();
        private IncidentAccident updateIncidentAccident = new();

        private DocumentDiscussion uploadDocumentDiscussion = new();
        private List<CaseAttachment> uploadDocumentDIscussionAttachment = new();
        private List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileDiscussionUpload = new();

        private DocumentDiscussion uploadReplyDocumentDiscussion = new();
        private List<CaseAttachment> uploadReplyDocumentDIscussionAttachment = new();
        private List<BPIWebApplication.Shared.MainModel.Stream.FileStream> fileReplyDiscussionUpload = new();

        private string previousPreviewedDocumentID = string.Empty;
        private string previousPreviewedDocumentLocation = string.Empty;

        private bool isLoading = false;
        private bool triggerReplyButton = false;

        private int maxFileSize { get; set; } = 0;

        private bool alertTrigger = false;
        private bool successAlert = false;
        private string alertBody = string.Empty;
        private string alertMessage = string.Empty;

        private IJSObjectReference _jsModule;
        private IReadOnlyList<IBrowserFile>? listFileUpload, listReplyFileUpload;

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

            await EPKRSService.getEPRKSReportingType();
            await EPKRSService.getEPRKSRiskType();
            maxFileSize = await EPKRSService.getEPKRSMaxFileSize();

            string paramGetItemCaseInitialization = activeUser.location + "!_!!_!1";
            await EPKRSService.getEPKRSItemCaseData(CommonLibrary.Base64Encode(paramGetItemCaseInitialization));

            string paramGetIncidentAccidentInitialization = activeUser.location + "!_!!_!1";
            await EPKRSService.getEPKRSIncidentAccidentData(CommonLibrary.Base64Encode(paramGetIncidentAccidentInitialization));

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/EPKRSPages/EPKRSDashboard.razor.js");
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
                                    fileName = Path.GetRandomFileName() + "!_!" + fi.Name,
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
                                    fileName = Path.GetRandomFileName() + "!_!" + fi.Name,
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

                updateItemCase = data.itemCase;

                EPKRSService.fileStreams.Clear();
                await EPKRSService.getEPKRSFileStream(CommonLibrary.Base64Encode(previewItemCase.DocumentID));

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

                updateIncidentAccident = data.incidentAccident;

                EPKRSService.fileStreams.Clear();
                await EPKRSService.getEPKRSFileStream(CommonLibrary.Base64Encode(previewIncidentAccident.DocumentID));
                
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

                    EPKRSService.fileStreams.Clear();
                    Task.Run(async () => { await EPKRSService.getEPKRSFileStream(CommonLibrary.Base64Encode(DocumentID)); StateHasChanged(); });
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

                    //if (EPKRSService.itemCases.SingleOrDefault(x => x.itemCase.DocumentID.Equals(previousPreviewedDocumentID)) != null)
                    //    EPKRSService.itemCases.SingleOrDefault(x => x.itemCase.DocumentID.Equals(previousPreviewedDocumentID)).attachment.AddRange(discussionData.Data.attachment);

                    //if (EPKRSService.incidentAccidents.SingleOrDefault(x => x.incidentAccident.DocumentID.Equals(previousPreviewedDocumentID)) != null)
                    //    EPKRSService.incidentAccidents.SingleOrDefault(x => x.incidentAccident.DocumentID.Equals(previousPreviewedDocumentID)).attachment.AddRange(discussionData.Data.attachment);

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
                    
                    //if (EPKRSService.itemCases.SingleOrDefault(x => x.itemCase.DocumentID.Equals(previousPreviewedDocumentID)) != null)
                    //    EPKRSService.itemCases.SingleOrDefault(x => x.itemCase.DocumentID.Equals(previousPreviewedDocumentID)).attachment.AddRange(discussionData.Data.attachment);

                    //if (EPKRSService.incidentAccidents.SingleOrDefault(x => x.incidentAccident.DocumentID.Equals(previousPreviewedDocumentID)) != null)
                    //    EPKRSService.incidentAccidents.SingleOrDefault(x => x.incidentAccident.DocumentID.Equals(previousPreviewedDocumentID)).attachment.AddRange(discussionData.Data.attachment);

                    uploadReplyDocumentDiscussion.Comment = "";
                }
                else
                {
                    await _jsModule.InvokeVoidAsync("showAlert", $"Failed : {res.ErrorCode} - {res.ErrorMessage} !");
                }

                //triggerReplyButton = false;
                fileReplyDiscussionUpload.Clear();
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
                    string param = approveType.Equals("Approve") ? "Approved" : approveType.Equals("OnProgress") ? "On Progress" : approveType.Equals("Close") ? "Closed" : "NAN";

                    var a = EPKRSService.itemCases.SingleOrDefault(x => x.itemCase.DocumentID.Equals(res.Data.Data.DocumentID));

                    if (a != null)
                        EPKRSService.itemCases.SingleOrDefault(x => x.itemCase.DocumentID.Equals(res.Data.Data.DocumentID)).itemCase.DocumentStatus = param;

                    var b = EPKRSService.incidentAccidents.SingleOrDefault(x => x.incidentAccident.DocumentID.Equals(res.Data.Data.DocumentID));

                    if (b != null)
                        EPKRSService.incidentAccidents.SingleOrDefault(x => x.incidentAccident.DocumentID.Equals(res.Data.Data.DocumentID)).incidentAccident.DocumentStatus = param;


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

                uploadData.userEmail = activeUser.userName;
                uploadData.userAction = "I";
                uploadData.userActionDate = DateTime.Now;

                var res = await EPKRSService.createEPKRSDocumentApprovalExtendedData(uploadData);

                if (res.isSuccess)
                {
                    string param = approveType.Equals("Approve") ? "Approved" : approveType.Equals("OnProgress") ? "On Progress" : approveType.Equals("Close") ? "Closed" : "NAN";

                    var a = EPKRSService.itemCases.SingleOrDefault(x => x.itemCase.DocumentID.Equals(res.Data.Data.approval.DocumentID));

                    if (a != null)
                        EPKRSService.itemCases.SingleOrDefault(x => x.itemCase.DocumentID.Equals(res.Data.Data.approval.DocumentID)).itemCase.DocumentStatus = param;

                    var b = EPKRSService.incidentAccidents.SingleOrDefault(x => x.incidentAccident.DocumentID.Equals(res.Data.Data.approval.DocumentID));

                    if (b != null)
                        EPKRSService.incidentAccidents.SingleOrDefault(x => x.incidentAccident.DocumentID.Equals(res.Data.Data.approval.DocumentID)).incidentAccident.DocumentStatus = param;


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
