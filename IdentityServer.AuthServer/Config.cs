using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace IdentityServer.AuthServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                // Intrspection endpoint  Basic Auth alıyor ya, onun username ve passwordu bunlar
                new ApiResource("resource_api1") //username
                {
                    Scopes = { "api1.read", "api1.write", "api1.update" },
                    ApiSecrets = new[]{new Secret("secretapi1".Sha256())} // password 
                },
                new ApiResource("resource_api2")
                {
                    Scopes = { "api2.read", "api2.write", "api2.update" } ,
                    ApiSecrets = new[]{new Secret("secretapi2".Sha256())}
                },
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("api1.read","API 1 için okuma izni"),
                new ApiScope("api1.write","API 1 için yazma izni"),
                new ApiScope("api1.update","API 1 için güncelleme izni"),

                new ApiScope("api2.read","API 2 için okuma izni"),
                new ApiScope("api2.write","API 2 için yazma izni"),
                new ApiScope("api2.update","API 2 için güncelleme izni"),

            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                #region Client Credential
		        
                new Client()
                {
                    ClientId="Client1",
                    ClientName= "Client 1 web app",
                    ClientSecrets= new []{ new Secret("secret".Sha256())},
                    AllowedGrantTypes= GrantTypes.ClientCredentials,
                    AllowedScopes = { "api1.read" }
                },
                new Client()
                {
                    ClientId="Client2",
                    ClientName= "Client 2 web app",
                    ClientSecrets= new []{ new Secret("secret".Sha256())},
                    AllowedGrantTypes= GrantTypes.ClientCredentials,
                    AllowedScopes = { "api1.read", "api1.update", "api2.write", "api2.update" }
                },

                #endregion

                #region Üyelik sistemi var :  IdentityResources olmalı bu nednele
                // Client 1 : https://localhost:7086
                new Client()
                {
                    ClientId="Client1-Mvc",
                    ClientName= "Client 1 app Mvc application",
                    RequirePkce= false, // serverside uygulama
                    ClientSecrets= new []{ new Secret("secret".Sha256())},
                    AllowedGrantTypes= GrantTypes.Hybrid,
                    RedirectUris = new List<string>{ "https://localhost:7086/signin-oidc" }, // token alma işlemini gerçekleştiren url : Authorize Endpoint bu urle dönüş yapar otomatik, OpenIdConnect paketi kullandığımız için   bu URL otomatik oluşur
                    AllowedScopes = { IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, "api1.read" }
                }, 
	            #endregion

            };
        }

        // GrantTypes.ClientCredentials : uluşturulan tokenda kullanıcı ile ilgili bilgiler olmicak, tamamen clientın API ye bağlanması ile ilgili izinler olcak.

        #region Kullanıcılar ve tokenda kullanıcıya ait yer alacak bilgiler

        /// <summary>
        /// tokendan geriye illa ki kullanıcı Id (subject Id: subId) dönmesi gerekli
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(), // == subId kullanıcı ID
                new IdentityResources.Profile() // userın claimleri (https://developer.okta.com/blog/2017/07/25/oidc-primer-part-1)
                #region IdentityResources.Profile()
                /*
                    The default profile claims are:

                    name
                    family_name
                    given_name
                    middle_name
                    nickname
                    preferred_username
                    profile
                    picture
                    website
                    gender
                    birthdate
                    zoneinfo
                    locale
                    updated_at
                 
                    */

	                #endregion
            };
        }


        /// <summary>
        /// User Index viewında User.Claims ile bu claimleri direk göremedik çünkü
        /// Cookie şişmemesi için IdentityServer bu bilgileri otomatik olarak cookie içine eklemez. UserInfo Endpoint ile bu bilgileri görebiliriz.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId="1", Username="ysera", Password="password",
                    Claims=new List<Claim>
                    {
                        new Claim("given_name","Merve"),
                        new Claim("family_name","Uğursaç"),
                    }
                },
                new TestUser
                {
                    SubjectId="2", Username="simurg", Password="password",
                    Claims=new List<Claim>
                    {
                        new Claim("given_name","Taylan"),
                        new Claim("family_name","Altun"),
                    }
                }
            };
        }
        #endregion
    }
}
