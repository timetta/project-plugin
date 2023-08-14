using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using IdentityModel.OidcClient;
using Red.WorkPoint.ApiClient;
using Red.WorkPoint.ProjectPlugin.Services;

namespace Red.WorkPoint.ProjectPlugin
{
    public partial class Passport : Form
    {
        private readonly OidcClient _oidcClient;
        private AuthorizeState _authorizeState;

        public event EventHandler LoginCompleted;

        public delegate void LoginCompletedHandler(EventArgs arg, EventArgs e);

        public Passport()
        {
            InitializeComponent();
            _oidcClient = AuthService.GetOidcClient();
            webBrowser.Navigated += WebBrowser_Navigated;
        }

        public void Login()
        {
            Task.Run(async () => await LoginHandler());
        }

        public void Logout()
        {
            Task.Run(async () => await LogoutHandler());
        }

        private async Task LoginHandler()
        {
            try
            {
                _authorizeState = await _oidcClient.PrepareLoginAsync();
                webBrowser.Navigate(_authorizeState.StartUrl);
                webBrowser.Refresh(WebBrowserRefreshOption.Completely);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private async Task LogoutHandler()
        {
            try
            {
                var url = await _oidcClient.PrepareLogoutAsync(new LogoutRequest
                {
                    IdTokenHint = Properties.Settings.Default.IdentityToken
                });

                webBrowser.Navigate(url);
                webBrowser.Refresh(WebBrowserRefreshOption.Completely);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private async void WebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            var url = e.Url.ToString();
            var position = url.IndexOf("?", StringComparison.Ordinal);

            if (position == -1) position = url.Length;

            var address = url.Substring(0, position);

            if (address.Contains("login-successful"))
            {
                var loginResult = await _oidcClient.ProcessResponseAsync(url, _authorizeState);
                Context.AccessToken = loginResult.AccessToken;
                Context.ExpiryTime = loginResult.AccessTokenExpiration;
                AuthService.SaveRefreshToken(loginResult.RefreshToken);
                Properties.Settings.Default.IdentityToken = loginResult.IdentityToken;
                Properties.Settings.Default.Tenant =
                    loginResult.User.Claims.SingleOrDefault(x => x.Type == "wp_tenant")?.Value;
                Properties.Settings.Default.User = loginResult.User.Claims.SingleOrDefault(x => x.Type == "sub")?.Value;
                Properties.Settings.Default.Save();
                DialogResult = DialogResult.OK;
                Close();
                LoginCompleted?.Invoke(this, null);
            }

            if (!address.Contains("logout-successful")) return;

            AuthService.ClearRefreshToken();
            Context.AccessToken = null;
            Properties.Settings.Default.IdentityToken = null;
            Properties.Settings.Default.Tenant = null;
            Properties.Settings.Default.User = null;
            Properties.Settings.Default.Save();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}