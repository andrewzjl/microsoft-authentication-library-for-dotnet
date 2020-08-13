// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Instance.Discovery;
using Microsoft.Identity.Client.Internal.Requests;

namespace Microsoft.Identity.Client.Instance
{
    internal class AuthorityEndpoints
    {
        public AuthorityEndpoints(string authorizationEndpoint, string tokenEndpoint, string deviceCodeEndpoint)
        {
            AuthorizationEndpoint = authorizationEndpoint;
            TokenEndpoint = tokenEndpoint;
            SelfSignedJwtAudience = tokenEndpoint;
            DeviceCodeEndpoint = deviceCodeEndpoint;
        }

        public string AuthorizationEndpoint { get; }
        public string TokenEndpoint { get; }
        public string SelfSignedJwtAudience { get; }
        public string DeviceCodeEndpoint { get; }

        public string LogString()
        {
            var builder = new StringBuilder(
                   Environment.NewLine + "=== Authority endpoint ===");
            builder.AppendLine("AuthorizationEndpoint - " + AuthorizationEndpoint);
            builder.AppendLine("TokenEndpoint - " + TokenEndpoint);
            builder.AppendLine("SelfSignedJwtAudience - " + SelfSignedJwtAudience);
            builder.AppendLine("DeviceCodeEndpoint - " + DeviceCodeEndpoint);
            return builder.ToString();
        }

        public static async Task UpdateAuthorityEndpointsAsync(
            AuthenticationRequestParameters requestParameters)
        {
            // This will make a network call unless instance discovery is cached, but this ok
            // GetAccounts and AcquireTokenSilent do not need this
            await UpdateAuthorityWithPreferredNetworkHostAsync(requestParameters).ConfigureAwait(false);

            requestParameters.Endpoints = await
                requestParameters.RequestContext.ServiceBundle.AuthorityEndpointResolutionManager.ResolveEndpointsAsync(
                    requestParameters.AuthorityInfo,
                    requestParameters.LoginHint,
                    requestParameters.RequestContext).ConfigureAwait(false);
            requestParameters.RequestContext.Logger.Info($"[Update Authority] requestParameters Endpoints: {requestParameters.Endpoints.LogString()}");
        }

        private async static Task UpdateAuthorityWithPreferredNetworkHostAsync(AuthenticationRequestParameters requestParameters)
        {
            InstanceDiscoveryMetadataEntry metadata = await
                requestParameters.RequestContext.ServiceBundle.InstanceDiscoveryManager.GetMetadataEntryAsync(
                    requestParameters.AuthorityInfo.CanonicalAuthority,
                    requestParameters.RequestContext)
                .ConfigureAwait(false);
            requestParameters.RequestContext.Logger.Info($"[Update Authority] requestParameters AuthorityType: {requestParameters.AuthorityInfo.AuthorityType}");
            requestParameters.RequestContext.Logger.Info($"[Update Authority] requestParameters AuthorizationUri: {requestParameters.AuthorityInfo.AuthorizationUri}");
            requestParameters.RequestContext.Logger.Info($"[Update Authority] requestParameters TokenUri: {requestParameters.AuthorityInfo.TokenUri}");
            if (requestParameters.AuthorityInfo.AuthorityType != AuthorityType.OIDC)
            {
                requestParameters.Authority = Authority.CreateAuthorityWithEnvironment(
                        requestParameters.AuthorityInfo,
                        metadata.PreferredNetwork);
            }
            else
            {
                requestParameters.Authority = Authority.CreateAuthority(requestParameters.AuthorityInfo);
            }
        }
    }
}
