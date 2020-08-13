// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.Identity.Client.Instance
{
    internal class OIDCAuthority : Authority
    {
        internal OIDCAuthority(AuthorityInfo authorityInfo) : base(authorityInfo)
        {
            TenantId = GetFirstPathSegment(AuthorityInfo.CanonicalAuthority);
        }

        internal override string TenantId { get; }

        internal bool IsCommonOrganizationsOrConsumersTenant()
        {
            return IsCommonOrganizationsOrConsumersTenant(TenantId);
        }

        internal override string GetTenantedAuthority(string tenantId)
        {
            return AuthorityInfo.CanonicalAuthority;
        }

        internal static bool IsCommonOrganizationsOrConsumersTenant(string tenantId)
        {
            return false;
        }

        internal override AuthorityEndpoints GetHardcodedEndpoints()
        {
            string deviceEndpoint = null;

            return new AuthorityEndpoints(AuthorityInfo.AuthorizationUri, AuthorityInfo.TokenUri, deviceEndpoint);
        }
    }
}
