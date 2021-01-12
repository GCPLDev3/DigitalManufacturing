#region Import Namspaces
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Web.Security;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Principal;
#endregion


public class clsAuthentication
{
    #region Login check authenticated user
    public bool IsAuthenticated(string domain, string username, string pwd)
    {
        string servername = System.Configuration.ConfigurationManager.AppSettings["serverpath"];

        //DirectoryEntry entry = new DirectoryEntry("LDAP://" + servername, domainAndUsername, pwd);
        try
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, servername))
            {
                bool isValid = pc.ValidateCredentials(username, pwd);

                if (isValid == false)
                    throw new Exception("LDAP : Invalid Username Or Password");

                return isValid;
            }
        }
        finally
        { }
    }
    #endregion

    #region Check User exists in LDAP
    public bool CheckUserExists(string otheruser, string username, string password)
    {
        DirectoryEntry searchRoot = null;
        DirectorySearcher searchForUser = null;

        try
        {
            string servername = System.Configuration.ConfigurationManager.AppSettings["serverpath"];

            searchRoot = new DirectoryEntry("LDAP://" + servername + "/dc=godrejinds,dc=com", username, password);
            searchForUser = new DirectorySearcher(searchRoot);

            searchForUser.PageSize = 100;

            searchForUser.SearchScope = System.DirectoryServices.SearchScope.Subtree;
            //searchForUser.Filter = string.Format("(&(objectCategory=Person)(anr={0}))", username);
            searchForUser.Filter = string.Format("(&(objectCategory=user)(anr={0}))", otheruser);

            if (searchForUser.FindOne() != null)
                return true;
            else
                return false;
        }
        finally
        {
            searchRoot.Dispose();
            searchForUser.Dispose();
        }
    }
    #endregion
}