using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace RedditStoreApp.Data.Core
{
    public abstract class PasswordVaultWrapper
    {
        static PasswordVaultWrapper()
        {
            Task.Factory.StartNew(() => { PasswordVaultWrapper.LoadPasswordVault(); });
        }

        private static void LoadPasswordVault()
        {
            // any call to the password vault will load the vault
            var vault = new PasswordVault();
            vault.RetrieveAll();
        }

        public static async Task<bool> IsStored()
        {
            return await Task.Factory.StartNew(() =>
            {
                var vault = new PasswordVault();
                IReadOnlyList<PasswordCredential> creds = vault.FindAllByResource("redditAuth");
                return creds != null && creds.Count > 0;
            });
        }

        public static async Task Store(string userName, string password)
        {
            await Task.Factory.StartNew(() =>
            {
                PasswordCredential pc = new PasswordCredential("redditAuth", userName, password);
                var vault = new PasswordVault();

                try
                {
                    IReadOnlyList<PasswordCredential> creds = vault.FindAllByResource("redditAuth");
                    if (creds == null) return;
                    foreach (var cred in creds)
                    {
                        vault.Remove(cred);
                    }
                }
                catch (Exception)
                {
                    // The password vault is mildly retarded.
                }

                vault.Add(pc);
            });
        }

        public static async Task<string> GetUsername()
        {
            return await Task.Factory.StartNew(() =>
            {
                var vault = new PasswordVault();
                var creds = vault.FindAllByResource("redditAuth");
                return (creds != null && creds.Count > 0) ? creds[0].UserName : null;
            });
        }

        public static async Task<string> GetPassword()
        {
            return await Task.Factory.StartNew(() =>
            {
                var vault = new PasswordVault();
                var creds = vault.FindAllByResource("redditAuth");
                if (creds != null && creds.Count > 0)
                {
                    creds[0].RetrievePassword();
                    return creds[0].Password;
                }
                else
                {
                    return null;
                }
            });
        }
    }
}
