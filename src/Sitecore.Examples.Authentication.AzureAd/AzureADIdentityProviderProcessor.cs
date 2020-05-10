using Microsoft.Owin.Infrastructure;
using Sitecore.Abstractions;
using Sitecore.Diagnostics;
using Sitecore.Owin.Authentication.Configuration;
using Sitecore.Owin.Authentication.Pipelines.IdentityProviders;
using Owin;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security;
using Sitecore.Owin.Authentication.Services;

namespace Sitecore.Examples.Authentication.AzureAd
{

  public class AzureADIdentityProviderProcessor : IdentityProvidersProcessor
  {
    public AzureADIdentityProviderProcessor(FederatedAuthenticationConfiguration federatedAuthenticationConfiguration, ICookieManager cookieManager, BaseSettings settings)
        : base(federatedAuthenticationConfiguration, cookieManager, settings)
    {
      
    }
    protected override string IdentityProviderName
    {
      get { return "AzureAD"; }
    }
    protected override void ProcessCore(IdentityProvidersArgs args)
    {
      Assert.ArgumentNotNull(args, nameof(args));

      var identityProvider = this.GetIdentityProvider();
      var authenticationType = this.GetAuthenticationType();

      string clientId = Settings.GetSetting("Authentication.AzureAd.ApplicationID");      
      string loginUrl = Settings.GetSetting("Authentication.AzureAd.PostLogoutRedirectUri");
      string redirectURI = Settings.GetSetting("Authentication.AzureAd.RedirectUri");
      string authority = "https://login.microsoftonline.com/" + Settings.GetSetting("Authentication.AzureAd.DirectoryID");

      args.App.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
      {
        Caption = identityProvider.Caption,
        AuthenticationType = authenticationType,
        AuthenticationMode = AuthenticationMode.Passive,
        ClientId = clientId,
        Authority = authority,
        PostLogoutRedirectUri = loginUrl,
        RedirectUri = redirectURI,
        CookieManager = this.CookieManager,

        Notifications = new OpenIdConnectAuthenticationNotifications
        {

          SecurityTokenValidated = notification =>
          {
            var identity = notification.AuthenticationTicket.Identity;
            
            foreach (var claimTransformationService in identityProvider.Transformations)
            {
              claimTransformationService.Transform(identity,
                  new TransformationContext(FederatedAuthenticationConfiguration, identityProvider));
            }

            notification.AuthenticationTicket = new AuthenticationTicket(identity, notification.AuthenticationTicket.Properties);
            return Task.FromResult(0);
          }
        }
      });
    }
  }
}
