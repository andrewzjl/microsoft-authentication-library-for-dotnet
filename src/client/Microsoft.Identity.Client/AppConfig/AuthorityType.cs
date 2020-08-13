// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Identity.Client
{
    /// <summary>
    /// </summary>
    internal enum AuthorityType
    {
        /// <summary>
        /// a known Azure AD authority to the application to sign-in users specifying
        ///    the full authority Uri. See https://aka.ms/msal-net-application-configuration.
        /// </summary>
        Aad,

        /// <summary>
        /// a known Authority corresponding to an ADFS server. See https://aka.ms/msal-net-adfs
        /// </summary>
        Adfs,

        /// <summary>
        /// a known authority corresponding to an Azure AD B2C policy. See https://aka.ms/msal-net-b2c-specificities
        /// </summary>
        B2C,

        /// <summary>
        /// The authority corresponding to a standard oauth 2.0 policy. See https://openid.net/connect/
        /// </summary>
        OIDC
    }
}
