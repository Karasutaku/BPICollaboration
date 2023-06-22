﻿using BPIWebApplication.Client.Services.CashierLogbookServices;
using BPIWebApplication.Shared.MainModel.Login;
using BPIWebApplication.Shared.MainModel.Standarizations;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using BPIWebApplication.Client.Shared.CustomLayout;

namespace BPIWebApplication.Client.Pages.StandarizationPages
{
    public partial class StandarizationDashboard : ComponentBase
    {
        private ActiveUser activeUser = new();
        private UserPrivileges privilegeDataParam = new();
        private List<string> userPriv = new();

        Standarizations previewData = new();

        private int standarizationPageActive = 0;
        private int standarizationNumberofPage = 0;

        private bool showPreviewModal = false;
        private bool isLoading = false;
        private bool isFilterActive = false;

        private string previousDocumentPreviewed = string.Empty;

        // test
        private bool show = false;
        private string filetextname = string.Empty;
        private string fileext = string.Empty;
        private byte[] filecont = new byte[0];

        private string standarizationFilterType { get; set; } = string.Empty;
        private string standarizationFilterValue { get; set; } = string.Empty;
        private string standarizationFilterSelectValue { get; set; } = string.Empty;

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

            activeUser.token = await sessionStorage.GetItemAsync<string>("token");
            activeUser.userName = Base64Decode(await sessionStorage.GetItemAsync<string>("userName"));
            activeUser.company = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[0];
            activeUser.location = Base64Decode(await sessionStorage.GetItemAsync<string>("CompLoc")).Split("_")[1];
            activeUser.sessionId = await sessionStorage.GetItemAsync<string>("SessionId");
            activeUser.appV = Convert.ToInt32(Base64Decode(await sessionStorage.GetItemAsync<string>("AppV")));
            activeUser.userPrivileges = new();
            activeUser.userPrivileges = await sessionStorage.GetItemAsync<List<string>>("PagePrivileges");

            LoginService.activeUser.userPrivileges = activeUser.userPrivileges;

            await StandarizationService.getStandarizationTypes();

            standarizationPageActive = 1;
            string conditions = "!_!" + standarizationPageActive.ToString();
            string mainpz = "StandarizationDetails!_!" + activeUser.location + "!_!";

            standarizationNumberofPage = await StandarizationService.getModulePageSize(Base64Encode(mainpz));
            await StandarizationService.getStandarizations(Base64Encode(conditions));

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/StandarizationPages/StandarizationDashboard.razor.js");
        }

        private Stream GetFileStream(byte[] data)
        {
            var fileBinData = data;
            var fileStream = new MemoryStream(fileBinData);

            return fileStream;
        }

        private async Task HandleDownloadDocument(Byte[] content, string filename)
        {
            var filestream = GetFileStream(content);

            using var streamRef = new DotNetStreamReference(stream: filestream);

            await _jsModule.InvokeVoidAsync("downloadFileFromStream", filename, streamRef);
        }

        private async Task downloadStandarizationFile(StandarizationAttachment data)
        {
            try
            {
                if (StandarizationService.fileStreams.SingleOrDefault(x => x.type.Equals(data.StandarizationID) && x.fileName.Equals(data.FilePath)) != null)
                {
                    isLoading = true;

                    var content = StandarizationService.fileStreams.SingleOrDefault(x => x.type.Equals(data.StandarizationID) && x.fileName.Equals(data.FilePath)).content;
                    string filename = data.FilePath.Split("!_!")[1];

                    await HandleDownloadDocument(content, filename);
                    //await _jsModule.InvokeVoidAsync("loadPDF", Convert.ToBase64String(content));

                    // test

                    //filetextname = filename;
                    //FileInfo f = new(filename);
                    //fileext = f.Extension;
                    //filecont = content;
                    ////show = true;

                    //FileViewer.setShowModal(true);

                    isLoading = false;
                }
                else
                {
                    await _jsModule.InvokeVoidAsync("showAlert", "Fetch Data Failed - Refresh Your Page and Try Again !");
                }
            }
            catch (Exception ex)
            {
                isLoading = false;
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message}");
            }
        }

        private void editDocument(Standarizations data)
        {
            string param = data.StandarizationID + "!_!" + previousDocumentPreviewed;

            navigate.NavigateTo($"standarization/editstandarization/{Base64Encode(param)}");
        }

        private async Task previewStandarization(Standarizations data)
        {
            try
            {
                showPreviewModal = true;
                
                if (!previousDocumentPreviewed.Equals(data.StandarizationID))
                {
                    isLoading = true;
                    StandarizationService.fileStreams.Clear();

                    previewData = data;

                    string temp = data.StandarizationID + "!_!" + data.TypeID;

                    await Task.Run(async () => { await StandarizationService.getFileStream(Base64Encode(temp)); });
                }

                isLoading = false;
                previousDocumentPreviewed = data.StandarizationID;
                StateHasChanged();
            }
            catch (Exception ex)
            {
                isLoading = false;
                await _jsModule.InvokeVoidAsync("showAlert", $"Error : {ex.Message}");
            }
        }

        private async Task standarizationPageSelect(int currPage)
        {
            standarizationPageActive = currPage;
            isLoading = true;

            if (isFilterActive)
            {
                string conditions = "";
                string temp = "";

                if (standarizationFilterType.Equals("TypeID"))
                {
                    conditions = $"WHERE {standarizationFilterType} = \'{standarizationFilterSelectValue}\'";
                    temp = conditions + "!_!" + standarizationPageActive.ToString();
                }
                else if (standarizationFilterType.Equals("TagDescriptions"))
                {
                    //conditions = $"WHERE StandarizationID IN (SELECT StandarizationID FROM StandarizationTags WHERE CONTAINS({standarizationFilterType}, \' \"{standarizationFilterValue}*\" \'))";
                    conditions = $"WHERE StandarizationID IN (SELECT Standarization ID FROM StandarizationTags WHERE TagDescriptions LIKE \'%{standarizationFilterValue}%\')";

                    temp = conditions + "!_!" + standarizationPageActive.ToString();
                }
                else
                {
                    conditions = $"WHERE {standarizationFilterType} LIKE \'%{standarizationFilterValue}%\'";
                    temp = conditions + "!_!" + standarizationPageActive.ToString();
                }

                StandarizationService.standarizations.Clear();
                await StandarizationService.getStandarizations(Base64Encode(temp));
            }
            else
            {
                string temp = "!_!" + standarizationPageActive.ToString();

                await StandarizationService.getStandarizations(Base64Encode(temp));
            }

            isLoading = false;
            StateHasChanged();
        }

        private async Task standarizationFilter()
        {
            if (standarizationFilterType.Length > 0)
            {
                standarizationPageActive = 1;
                isFilterActive = true;
                isLoading = true;

                string conditions = "";
                string temp = "";
                string mainpz = "";

                if (standarizationFilterType.Equals("TypeID"))
                {
                    conditions = $"WHERE {standarizationFilterType} = \'{standarizationFilterSelectValue}\'";
                    temp = conditions + "!_!" + standarizationPageActive.ToString();

                    mainpz = "StandarizationDetails!_!" + activeUser.location + $"!_!WHERE {standarizationFilterType} LIKE \'%{standarizationFilterSelectValue}%\'";
                }
                else if (standarizationFilterType.Equals("TagDescriptions"))
                {
                    conditions = $"WHERE StandarizationID IN (SELECT StandarizationID FROM StandarizationTags WHERE CONTAINS({standarizationFilterType}, \' \"{standarizationFilterValue}*\" \'))";
                    temp = conditions + "!_!" + standarizationPageActive.ToString();

                    mainpz = "StandarizationDetails!_!" + activeUser.location + $"!_!WHERE StandarizationID IN (SELECT StandarizationID FROM StandarizationTags WHERE CONTAINS({standarizationFilterType}, \' \"{standarizationFilterValue}*\" \'))";
                }
                else
                {
                    conditions = $"WHERE {standarizationFilterType} LIKE \'%{standarizationFilterValue}%\'";
                    temp = conditions + "!_!" + standarizationPageActive.ToString();

                    mainpz = "StandarizationDetails!_!" + activeUser.location + $"!_!WHERE {standarizationFilterType} LIKE \'%{standarizationFilterValue}%\'";
                }

                StandarizationService.standarizations.Clear();
                standarizationNumberofPage = await StandarizationService.getModulePageSize(Base64Encode(mainpz));
                await StandarizationService.getStandarizations(Base64Encode(temp));

                isLoading = false;
                StateHasChanged();
            }
            else
            {
                await _jsModule.InvokeVoidAsync("showAlert", "Please Select Filter Type !");
            }
        }

        private async Task standarizationFilterReset()
        {
            isLoading = true;
            standarizationPageActive = 1;
            isFilterActive = false;
            standarizationFilterType = "";
            standarizationFilterValue = "";
            standarizationFilterSelectValue = "";

            string conditions = "!_!" + standarizationPageActive.ToString();
            string mainpz = "StandarizationDetails!_!" + activeUser.location + "!_!";

            standarizationNumberofPage = await StandarizationService.getModulePageSize(Base64Encode(mainpz));
            await StandarizationService.getStandarizations(Base64Encode(conditions));

            isLoading = false;
            StateHasChanged();
        }

        private bool checkStandarizationPresent()
        {
            try
            {
                if (StandarizationService.standarizations.Any())
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

        private bool checkStandarizationTypesDataPresent()
        {
            try
            {
                if (StandarizationService.standarizationTypes.Any())
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
