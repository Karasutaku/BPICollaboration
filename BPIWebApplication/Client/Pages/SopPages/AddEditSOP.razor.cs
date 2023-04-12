﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;
using BPIWebApplication.Shared.FileUploadModel;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.JSInterop;
using BPIWebApplication.Shared.DbModel;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.MainModel.Procedure;
using Microsoft.IdentityModel.Tokens;

namespace BPIWebApplication.Client.Pages.SopPages
{
    public partial class AddEditSOP : ComponentBase
    {
        [Parameter]
        public string? param { get; set; }

        // message trigger flag
        private bool alertTrigger = false;
        private string alertMessage = string.Empty;
        private string alertBody = string.Empty;

        // upload valid submit flag
        private bool uploadTrigger = false;
        private bool successAlert = false;

        // private string filePath = string.Empty;

        private Procedure procedure = new Procedure();
        //private ActiveUser<LoginUser> activeUser = new ActiveUser<LoginUser>();
        private ActiveUser activeUser = new();
        private UserPrivileges privilegeDataParam = new();
        private List<string> userPriv = new();

        private IJSObjectReference _jsModule;

        private static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private Stream GetFileStream(byte[] data)
        {
            var fileBinData = data;
            var fileStream = new MemoryStream(fileBinData);

            return fileStream;
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

            await ProcedureService.GetAllProcedure();

            activeUser.token = await sessionStorage.GetItemAsync<string>("token");
            activeUser.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            activeUser.company = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[0];
            activeUser.location = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];
            activeUser.sessionId = await sessionStorage.GetItemAsync<string>("SessionId");
            activeUser.appV = Convert.ToInt32(Base64Decode(await sessionStorage.GetItemAsync<string>("AppV")));
            activeUser.userPrivileges = await sessionStorage.GetItemAsync<List<string>>("PagePrivileges");

            LoginService.activeUser.userPrivileges = activeUser.userPrivileges;

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/SopPages/Dashboard.razor.js");

        }

        protected override async Task OnParametersSetAsync()
        {
            if (param != null)
            {
                string temp = Base64Decode(param);

                if (ProcedureService.procedures.SingleOrDefault(a => a.ProcedureNo == temp) != null)
                {
                    procedure = ProcedureService.procedures.SingleOrDefault(a => a.ProcedureNo == temp);
                }
                else
                {
                    alertMessage = "Error Fetch Procedure Data";
                    alertBody = "Please retry your activity";
                    alertTrigger = true;
                }

            }
            else
            {
                //departmentProcedures.Department = ProcedureService.departments[0];
                //departmentProcedures.DepartmentID = departmentProcedures.Department.DepartmentID;

                //departmentProcedures.Department.BisnisUnit = ProcedureService.bisnisUnits[0];
                //departmentProcedures.Department.BisnisUnit.BisnisUnitID = departmentProcedures.Department.BisnisUnit.BisnisUnitID;
            }
        }


        IReadOnlyList<IBrowserFile> listFileUploadWi;
        private List<FileSelectUpload> tempFileSelectWi = new List<FileSelectUpload>();
        
        private async void WiUploadHandleSelection(InputFileChangeEventArgs files)
        {
            tempFileSelectWi.Clear();

            listFileUploadWi = files.GetMultipleFiles();

            // megabyte
            long maxFileSize = await ProcedureService.getProcedureMaxFileSize();

            foreach (var file in listFileUploadWi)
            {
                if (file.Size < maxFileSize)
                {

                    FileInfo fi = new FileInfo(file.Name);
                    string ext = fi.Extension;

                    if (ext == ".pdf")
                    {
                        tempFileSelectWi.Add(new FileSelectUpload
                        {
                            type = "WI",
                            fileName = file.Name,
                            fileType = ext,
                            fileSize = Convert.ToInt32(file.Size)
                        });

                    } else
                    {
                        // message that procedure file ext not appopriate
                        alertMessage = "File Extention is not PDF !";
                        alertBody = "Please reselect another File";
                        alertTrigger = true;
                    }
                    
                } else
                {
                    // message that procedure file size not appopriate
                    alertMessage = "File Size is too Big !";
                    alertBody = "Please check your upload File";
                    alertTrigger = true;
                }
            }

            this.StateHasChanged();

        }

        IReadOnlyList<IBrowserFile> listFileUploadSop;
        private List<FileSelectUpload> tempFileSelectSop = new List<FileSelectUpload>();
        private async void SopUploadHandleSelection(InputFileChangeEventArgs files)
        {
            tempFileSelectSop.Clear();

            listFileUploadSop = files.GetMultipleFiles();

            // megabyte
            long maxFileSize = await ProcedureService.getProcedureMaxFileSize();

            foreach (var file in listFileUploadSop)
            {
                if (file.Size < maxFileSize)
                {

                    FileInfo fi = new FileInfo(file.Name);
                    string ext = fi.Extension;

                    if (ext == ".pdf")
                    {
                        tempFileSelectSop.Add(new FileSelectUpload
                        {
                            type = "SOP",
                            fileName = file.Name,
                            fileType = ext,
                            fileSize = Convert.ToInt32(file.Size)
                        });

                    }
                    else
                    {
                        // message that procedure file ext not appopriate
                        alertMessage = "File Extention is not PDF !";
                        alertBody = "Please reselect another File";
                        alertTrigger = true;
                    }

                }
                else
                {
                    // message that procedure file size not appopriate
                    alertMessage = "File Size is too Big !";
                    alertBody = "Please check your upload File";
                    alertTrigger = true;
                }
            }

            this.StateHasChanged();
        }

        bool bClearInputFile;
        private void ClearFiles()
        {
            bClearInputFile = !bClearInputFile;

            procedure.ProcedureNo = "";
            procedure.ProcedureName = "";
            procedure.ProcedureDate = DateTime.Now;
            procedure.ProcedureWi = "";
            procedure.ProcedureSop = "";

            this.StateHasChanged();

            listFileUploadWi = null;
            listFileUploadSop = null;
            tempFileSelectWi.Clear();
            tempFileSelectSop.Clear();

            this.StateHasChanged();
        }

        private async void HandleSubmit()
        {
            try
            {
                uploadTrigger = await ProcedureService.checkProsedureExisting(procedure.ProcedureNo);

                List<BPIWebApplication.Shared.MainModel.Stream.FileStream> readyUpload = new List<BPIWebApplication.Shared.MainModel.Stream.FileStream>();

                if (!uploadTrigger)
                {
                    if (activeUser.userPrivileges.Contains("CR"))
                    {
                        if (listFileUploadWi != null)
                        {
                            // wi file to stream
                            foreach (var file in listFileUploadWi)
                            {
                                FileInfo fi = new FileInfo(file.Name);
                                string ext = fi.Extension;

                                Stream stream = file.OpenReadStream(file.Size);
                                MemoryStream ms = new MemoryStream();
                                await stream.CopyToAsync(ms);

                                stream.Close();

                                readyUpload.Add(new BPIWebApplication.Shared.MainModel.Stream.FileStream
                                {
                                    type = "WI",
                                    fileName = fi.Name,
                                    fileType = ext,
                                    fileSize = Convert.ToInt32(file.Size),
                                    content = ms.ToArray()
                                });
                            }

                        }

                        if (listFileUploadSop != null)
                        {
                            // sop file to stream
                            foreach (var file in listFileUploadSop)
                            {
                                FileInfo fi = new FileInfo(file.Name);
                                string ext = fi.Extension;

                                Stream stream = file.OpenReadStream(file.Size);
                                MemoryStream ms = new MemoryStream();
                                await stream.CopyToAsync(ms);

                                stream.Close();

                                readyUpload.Add(new BPIWebApplication.Shared.MainModel.Stream.FileStream
                                {
                                    type = "SOP",
                                    fileName = fi.Name,
                                    fileType = ext,
                                    fileSize = Convert.ToInt32(file.Size),
                                    content = ms.ToArray()
                                });
                            }
                        }

                        if ((listFileUploadWi == null) && (listFileUploadSop == null))
                        {
                            // message that no file selected
                            alertMessage = "No File(s) Selected !";
                            alertBody = "Please upload your file";
                            alertTrigger = true;
                            this.StateHasChanged();
                        }
                        else
                        {
                            // pass data files and procedure details

                            ProcedureStream procedureUpload = new ProcedureStream();
                            procedureUpload.procedureDetails = new QueryModel<Procedure>();
                            procedureUpload.files = new List<BPIWebApplication.Shared.MainModel.Stream.FileStream>();

                            procedureUpload.procedureDetails.Data = procedure;
                            procedureUpload.procedureDetails.userEmail = activeUser.userName;
                            procedureUpload.procedureDetails.userAction = "I";
                            procedureUpload.procedureDetails.userActionDate = DateTime.Now;
                            procedureUpload.files = readyUpload;

                            await ProcedureService.createProcedure(procedureUpload);

                            alertMessage = "Add Procedure Success !";
                            alertBody = "";
                            successAlert = true;

                            ClearFiles();
                        }

                    }
                    else
                    {
                        // message that user is not admin
                        alertMessage = "User is not Admin !";
                        alertBody = "Contact ADMIN in charge - Require User Elevation";
                        alertTrigger = true;
                        this.StateHasChanged();
                    }

                }
                else
                {
                    // message that user is not admin
                    alertMessage = "Procedure Already Exist !";
                    alertBody = "Please check your Procedure Number";
                    alertTrigger = true;
                    StateHasChanged();
                }

                uploadTrigger = false;
            }
            catch(Exception ex)
            {
                throw new Exception(alertMessage, ex);
                // await JS.InvokeVoidAsync("alert", ex.Message.ToString());
            }
        }

        private async void editProcedure()
        {
            // edit procedure

            try
            {
                List<BPIWebApplication.Shared.MainModel.Stream.FileStream> readyUpload = new List<BPIWebApplication.Shared.MainModel.Stream.FileStream>();

                if (activeUser.userPrivileges.Contains("ED"))
                {
                    if (listFileUploadWi != null)
                    {
                        // wi file to stream
                        foreach (var file in listFileUploadWi)
                        {
                            FileInfo fi = new FileInfo(file.Name);
                            string ext = fi.Extension;

                            Stream stream = file.OpenReadStream(file.Size);
                            MemoryStream ms = new MemoryStream();
                            await stream.CopyToAsync(ms);

                            stream.Close();

                            readyUpload.Add(new BPIWebApplication.Shared.MainModel.Stream.FileStream
                            {
                                type = "WI",
                                fileName = fi.Name,
                                fileType = ext,
                                fileSize = Convert.ToInt32(file.Size),
                                content = ms.ToArray()
                            });
                        }

                    }

                    if (listFileUploadSop != null)
                    {
                        // sop file to stream
                        foreach (var file in listFileUploadSop)
                        {
                            FileInfo fi = new FileInfo(file.Name);
                            string ext = fi.Extension;

                            Stream stream = file.OpenReadStream(file.Size);
                            MemoryStream ms = new MemoryStream();
                            await stream.CopyToAsync(ms);

                            stream.Close();

                            readyUpload.Add(new BPIWebApplication.Shared.MainModel.Stream.FileStream
                            {
                                type = "SOP",
                                fileName = fi.Name,
                                fileType = ext,
                                fileSize = Convert.ToInt32(file.Size),
                                content = ms.ToArray()
                            });
                        }
                    }

                    if ((listFileUploadWi == null) && (listFileUploadSop == null))
                    {
                        // message that no file selected
                        alertMessage = "No File(s) Selected !";
                        alertBody = "Please upload your file";
                        alertTrigger = true;
                        this.StateHasChanged();
                    }
                    else
                    {
                        // pass data files and procedure details

                        ProcedureStream procedureUpload = new ProcedureStream();
                        procedureUpload.procedureDetails = new QueryModel<Procedure>();
                        procedureUpload.files = new List<BPIWebApplication.Shared.MainModel.Stream.FileStream>();

                        procedureUpload.procedureDetails.Data = procedure;
                        procedureUpload.procedureDetails.userEmail = activeUser.userName;
                        procedureUpload.procedureDetails.userAction = "U";
                        procedureUpload.procedureDetails.userActionDate = DateTime.Now;
                        procedureUpload.files = readyUpload;

                        await ProcedureService.editProcedure(procedureUpload);

                        alertMessage = "Edit Procedure Success !";
                        alertBody = "";
                        successAlert = true;

                        ClearFiles();
                    }

                }
                else
                {
                    // message that user is not admin
                    alertMessage = "User is not Admin !";
                    alertBody = "Contact ADMIN in charge - Require User Elevation";
                    alertTrigger = true;
                    this.StateHasChanged();
                }

                uploadTrigger = false;

            }
            catch (Exception ex)
            {
                throw new Exception(alertMessage, ex);
                // await JS.InvokeVoidAsync("alert", ex.Message.ToString());
            }
        }

        private async void handleDownload(string path, string procNo)
        {
            var temp = path + "!_!" + procNo;

            var dt = await ProcedureService.GetFile(temp);

            if (!dt.isSuccess)
            {
                // alert file download not found
            }
            else
            {
                try
                {
                    // download file

                    var filestream = GetFileStream(dt.Data.content);
                    string filename = procNo + ".pdf";

                    using var streamRef = new DotNetStreamReference(stream: filestream);

                    await _jsModule.InvokeVoidAsync("downloadFileFromStream", filename, streamRef);
                    await _jsModule.InvokeVoidAsync("showAlert", $"File {procNo} Downloaded");

                }
                catch (Exception ex)
                {
                    await _jsModule.InvokeVoidAsync("showAlert", ex.Message);
                    throw new Exception(ex.Message);
                }

            }
        }

        private void resetTrigger()
        {
            alertTrigger = false;
            this.StateHasChanged();
        }

        private void resetSuccessAlert()
        {
            successAlert = false;
            this.StateHasChanged();
        }
    }
}
