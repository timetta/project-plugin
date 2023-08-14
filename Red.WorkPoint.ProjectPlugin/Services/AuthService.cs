using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;
using IdentityModel.OidcClient;
using System.Security.Cryptography;
using System.Text;
using IdentityModel.Client;

namespace Red.WorkPoint.ProjectPlugin.Services
{
    /// <summary>
    /// Auth service.
    /// </summary>
    public class AuthService
    {
        private static readonly byte[] AdditionalEntropy = { 9, 2, 3, 3, 4 };

        /// <summary>
        /// Updates access token.
        /// </summary>
        /// <returns>True if success.</returns>
        public static async Task<bool> UpdateAccessToken()
        {
            if (!string.IsNullOrWhiteSpace(Context.AccessToken) && (Context.ExpiryTime - DateTime.Now).TotalMinutes > 5)
            {
                return true;
            }

            // Reading refresh token.
            var refreshToken = ReadRefreshToken();
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return Login();
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var oidcClient = GetOidcClient();
                var result = await oidcClient.RefreshTokenAsync(refreshToken);

                if (result.IsError)
                {
                    return Login();
                }

                SaveRefreshToken(result.RefreshToken);
                Context.AccessToken = result.AccessToken;
                Context.ExpiryTime = DateTime.Now.AddMilliseconds(result.ExpiresIn);

                return true;
            }
        }

        /// <summary>
        /// Clears refresh token.
        /// </summary>
        public static void ClearRefreshToken()
        {
            Properties.Settings.Default.RefreshToken = "";
            Properties.Settings.Default.Tenant = "";
            Properties.Settings.Default.User = "";
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Saves refresh token.
        /// </summary>
        /// <param name="refreshToken"></param>
        public static void SaveRefreshToken(string refreshToken)
        {
            var bytes = Encoding.Default.GetBytes(refreshToken);
            var protectedData = ProtectedData.Protect(bytes, AdditionalEntropy, DataProtectionScope.CurrentUser);
            Properties.Settings.Default.RefreshToken = Encoding.Default.GetString(protectedData);
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Reads refresh token.
        /// </summary>
        /// <returns>Refresh token.</returns>
        public static string ReadRefreshToken()
        {
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.RefreshToken))
                return null;

            var protectedData = Encoding.Default.GetBytes(Properties.Settings.Default.RefreshToken);

            try
            {
                var data = ProtectedData.Unprotect(protectedData, AdditionalEntropy, DataProtectionScope.CurrentUser);
                return Encoding.Default.GetString(data);
            }
            catch (Exception)
            {
                Properties.Settings.Default.RefreshToken = "";
                Properties.Settings.Default.Save();
                return null;
            }
        }

        /// <summary>
        /// Returns OpenID client.
        /// </summary>
        /// <returns>OpenID client.</returns>
        public static OidcClient GetOidcClient()
        {
            var oidcClient = new OidcClient(new OidcClientOptions
            {
                Authority = Config.PassportUrl,
                ClientId = "project_plugin",
                LoadProfile = false,
                Flow = OidcClientOptions.AuthenticationFlow.AuthorizationCode,
                RedirectUri = $"{Config.PassportUrl}/login-successful",
                PostLogoutRedirectUri = $"{Config.PassportUrl}/logout-successful",
                Scope = "openid profile offline_access all",
                Policy = new Policy
                {
                    Discovery = new DiscoveryPolicy
                    {
                        ValidateIssuerName = false,
                        ValidateEndpoints = false
                    }
                }
            });

            return oidcClient;
        }

        /// <summary>
        /// Logs in.
        /// </summary>
        /// <returns>True if success.</returns>
        private static bool Login()
        {
            ClearRefreshToken();
            var passport = new Passport();
            passport.Login();
            passport.ShowDialog();

            if (passport.DialogResult != DialogResult.OK) return false;

            // Set telemetry context.
            Telemetry.SetUserContext();

            return true;
        }

        /// <summary>
        /// Logs out.
        /// </summary>
        public static void Logout()
        {
            var passport = new Passport();
            passport.Logout();
            passport.ShowDialog();
        }
    }
}