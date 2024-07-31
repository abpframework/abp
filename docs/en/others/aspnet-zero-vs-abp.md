# ABP vs ASP.NET Zero

[ASP.NET Zero](https://aspnetzero.com/) is a startup project template which is also developed by [Volosoft](https://volosoft.com/). Here you can see the feature differences between ASP.NET Zero and ABP Platform (with commercial licenses).

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
<meta content="text/html; charset=utf-8" http-equiv="Content-Type" />
</head>
<body>

<table id="TemplateComparisonTable" class="table">
        <thead>
            <tr>
                <th>Feature</th>
                <th>ABP</th>
                <th>ASP.NET Zero</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td colspan="3"><strong>Base Infrastructure</strong></td>
            </tr>
            <tr>
                <td>Base Framework</td>
                <td><a href="https://github.com/abpframework/abp/" target="_blank"> ABP</a></td>
                <td><a href="https://github.com/aspnetboilerplate/aspnetboilerplate" target="_blank">AspNet Boilerplate</a></td>
            </tr>
            <tr>
                <td>Microservice compatible</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-minus text-secondary"></i></td>
            </tr>
            <tr>
                <td>Multi Tenancy</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Bootstrap Tag Helpers</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-minus text-secondary"></i></td>
            </tr>
            <tr>
                <td>Dynamic Forms</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-minus text-secondary"></i></td>
            </tr>
            <tr>
                <td>MongoDB Support</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-minus text-secondary"></i></td>
            </tr>
            <tr>
                <td>Distributed Event Bus</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-minus text-secondary"></i></td>
            </tr>
            <tr>
                <td>Command Line Interface</td>
                <td><a href="https://abp.io/docs/latest/cli" target="_blank">ABP CLI</a></td>
                <td><i class="fa fa-minus text-secondary"></i></td>
            </tr>
            <tr>
                <td colspan="3"><strong>User Interface (Overall)</strong></td>
            </tr>
            <tr>
                <td>ASP.NET Core UI</td>
                <td>Razor Pages</td>
                <td>MVC</td>
            </tr>
            <tr>
                <td>Angular UI</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Blazor UI</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-minus text-secondary"></i></td>
            </tr>
            <tr>
                <td colspan="3"><strong>User Interface (Account / Login)</strong></td>
            </tr>
            <tr>
                <td>Login</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Register</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Token based authentication</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Social logins</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Social logins per tenant</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>LDAP (Active Directory) / ADFS login</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Forgot password</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Email address &amp; phone number confirmation</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Password reset</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Two Factor authentication</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>OpenId Connect login</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>User lockout</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>User profile / change password</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Profile image</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Account linking</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>User Delegation</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Show login attempts</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Tenant registration</td>
                <td><i class="fa fa-minus text-secondary"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Identity Server 4 integration</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>OpenIddict integration</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-minus text-false"></i></td>
            </tr>
            <tr>
                <td>Identity Server Management UI</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-minus text-secondary"></i></td>
            </tr>
            <tr>
                <td>Password complexity settings</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td colspan="3"><strong>User Interface (Application)</strong></td>
            </tr>
            <tr>
                <td>User management</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Role management</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Tenant management</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Permission management</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Sample tenant dashboard</td>
                <td><i class="fa fa-minus text-secondary"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Setup screen</td>
                <td><i class="fa fa-minus text-secondary"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Edition &amp; feature management for SaaS applications</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Tenant subscription, payment &amp; billing system</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Tenant &amp; user Impersonation</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Organization Unit management</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Language (localization) management</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Audit log & Entity history report</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Real time notifications</td>
                <td><i class="fa fa-minus text-secondary"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Chat (with SignalR)</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Host dashboard</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Application settings</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>RTL support</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>   
            </tr>
            <tr>
                <td>Show website logs / clear caches</td>
                <td><i class="fa fa-minus text-secondary"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Custom tenant logo &amp; CSS</td>
                <td><i class="fa fa-minus text-secondary"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Time zone selection</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>UI personalization (top/left menu, dark/light skin... options)</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Customizable Dashboard</td>
                <td><i class="fa fa-minus text-secondary"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Dynamic Entity Parameters</td>
                <td><i class="fa fa-minus text-secondary"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Webhook System</td>
                <td><i class="fa fa-minus text-secondary"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Session timeout & User lock Screen</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>File Management</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-minus text-secondary"></i></td>
            </tr>
            <tr>
                <td>Text Template Management</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-minus text-secondary"></i></td>
            </tr>
            <tr>
                <td>Security Logs UI</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-minus text-secondary"></i></td>
            </tr>
            <tr>
                <td>GDPR (downloading personal data & deleting accounts)</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>                
            <tr>
                <td>UI Theme</td>
                <td><a href="http://x.leptontheme.com/" target="_blank">Lepton-X</a>, <a href="http://leptontheme.com/" target="_blank">Lepton</a></td>
                <td><a href="https://keenthemes.com/metronic/" target="_blank">Metronic</a></td>
            </tr>
            <tr>
                <td colspan="3"><strong>User Interface (Public Web Site)</strong></td>
            </tr>
            <tr>
                <td>Simple public web site</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td colspan="3"><strong>Mobile Application</strong></td>
            </tr>
            <tr>
                <td>Xamarin application</td>
                <td><i class="fa fa-minus text-secondary"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>React Native application</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-minus text-secondary"></i></td>
            </tr>
            <tr>
                <td>.NET MAUI</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td colspan="3"><strong>Rapid Application Development</strong></td>
            </tr>
            <tr>
                <td>Code generation extension</td>
                <td><a href="https://abp.io/docs/latest/suite" target="_blank">ABP Suite</a></td>
                <td><a href="https://docs.aspnetzero.com/en/common/latest/Rapid-Application-Development" target="_blank">Power Tools</a></td>
            </tr>
            <tr>
                <td colspan="3"><strong>Support</strong></td>
            </tr>
            <tr>
                <td>Premium forum support</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>Private ticket & email support</td>
                <td><i class="fa fa-check text-success"></i></td>
                <td><i class="fa fa-check text-success"></i></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td><a href="https://abp.io/pricing" target="_blank" rel="noopener">Pricing</a></td>
                <td><a href="https://aspnetzero.com/Pricing">Pricing</a></td>
            </tr>
        </tbody>
    </table>


</body>

</html>
