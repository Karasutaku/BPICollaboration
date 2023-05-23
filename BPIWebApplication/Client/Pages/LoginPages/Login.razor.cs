using BPIWebApplication.Shared.MainModel.Login;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using BPIWebApplication.Shared.MainModel;

namespace BPIWebApplication.Client.Pages.LoginPages
{
    public partial class Login : ComponentBase
	{
		private LoginUser user { get; set; } = new LoginUser();

		//private ResultModel<ActiveUser<LoginUser>> data = new ResultModel<ActiveUser<LoginUser>>();

        private FacadeLogin loginData = new FacadeLogin();
        private Location location = new();

        //private FacadeUserModule moduleData = new();
        private bool triggerShowPassword { get; set; } = false;

        private bool isLoginProgress = false;
        private bool isFetchLocationProgress = false;

        private IJSObjectReference _jsModule;

        protected override async Task OnInitializedAsync()
        {
            isLoginProgress = false;
            triggerShowPassword = false;

            user.userName = "";
            user.password = "";
            user.companyId = "BLANK";
            user.locationId = "BLANK";

            _jsModule = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/LoginPages/Login.razor.js");
        }
        private static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        async Task checkLogin()
		{
            try
            {
                isLoginProgress = true;

                loginData.userName = user.userName;
                loginData.password = user.password;
                loginData.companyId = Convert.ToInt32(user.companyId);
                loginData.locationId = user.locationId;
                loginData.fromApplicationId = 0;
                loginData.fromApplicationSession = "";
                loginData.ipClient = "";
                loginData.macAddress = "";

                var res = await loginService.smsApiFacadeLogin(loginData);

                if (res.isSuccess)
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(res.Data.token);
                    var tokenz = jsonToken as JwtSecurityToken;

                    // claims

                    string email = tokenz.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
                    string compLoc = tokenz.Claims.FirstOrDefault(c => c.Type.Equals("CompLoc")).Value;
                    string sessionId = tokenz.Claims.FirstOrDefault(c => c.Type.Equals("SessionId")).Value;
                    string appV = tokenz.Claims.FirstOrDefault(c => c.Type.Equals("AppV")).Value;

                    loginService.activeUser.token = res.Data.token;
                    loginService.activeUser.userName = email.ToLower();
                    loginService.activeUser.company = compLoc.Split("_")[0];
                    loginService.activeUser.location = compLoc.Split("_")[1];
                    loginService.activeUser.sessionId = sessionId;
                    loginService.activeUser.appV = Convert.ToInt32(appV);

                    sessionStorage.SetItem("token", res.Data.token);
                    sessionStorage.SetItem("userName", Base64Encode(email.ToLower()));
                    sessionStorage.SetItem("CompLoc", Base64Encode(compLoc));
                    sessionStorage.SetItem("SessionId", sessionId);
                    sessionStorage.SetItem("AppV", Base64Encode(appV));

                    navigate.NavigateTo("/Index");

                    loginData = new();
                    user.userName = "";
                    user.password = "";
                    user.companyId = "BLANK";
                    user.locationId = "BLANK";

                    isLoginProgress = false;
                    StateHasChanged();
                }
                else
                {
                    loginData = new();
                    user.userName = "";
                    user.password = "";
                    user.companyId = "BLANK";
                    user.locationId = "BLANK";

                    isLoginProgress = false;
                    StateHasChanged();

                    await _jsModule.InvokeAsync<IJSObjectReference>("showAlert", $"Login Failed ! {res.ErrorCode} - {res.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                isLoginProgress = false;
                StateHasChanged();
                await _jsModule.InvokeAsync<IJSObjectReference>("showAlert", $"Error : {ex.Message} ! From {ex.InnerException}");
            }
        }

        private async void getLocationsByCompany(ChangeEventArgs e)
        {
            isFetchLocationProgress = true;

            user.companyId = e.Value.ToString();

            location.Condition = $"a.CompanyId={e.Value}";
            location.PageIndex = 1;
            location.PageSize = 100;
            location.FieldOrder = "a.CompanyId";
            location.MethodOrder = "ASC";

            var res = await ManagementService.GetCompanyLocations(location);

            if (res.isSuccess)
            {
               isFetchLocationProgress = false;
            }
            else
            {
                await _jsModule.InvokeAsync<IJSObjectReference>("showAlert", "Fetch Data Location Failed ! Please Refresh your page and Relogin");
                isFetchLocationProgress = false;
            }

            StateHasChanged();
        }

        private void formClear()
        {
            loginData = new();
            user.userName = "";
            user.password = "";
            user.companyId = "BLANK";
            user.locationId = "BLANK";
        }

	}
}
