# Sitecore.Examples.Authentication.AzureAd
 Connect Sitecore to Azure Active Directory 

This code example registeres "Azure Active Directory" authentication provider for the default "shell" site in Sitecore Content Management instance. 

Make sure to update settings in Sitecore.Examples.Authentication.AzureAd.config:
- sitecoreHost
- Authentication.AzureAd.ApplicationID
- Authentication.AzureAd.DirectoryID

**Important:** By default, the "shell" site redirects to the "Identity Server" login page in Sitecore 9.3. If you want to see options, when multiple authentication providers registered, you need to enable loginPage="/sitecore/login"  on the &lt;site name="shell"&gt; config node.

Useful links:
 - [Using federated authentication with Sitecore](https://doc.sitecore.com/developers/93/sitecore-experience-manager/en/using-federated-authentication-with-sitecore.html)
  - [Configure federated authentication](https://doc.sitecore.com/developers/93/sitecore-experience-manager/en/configure-federated-authentication.html)
 
