﻿// ------------------------------------------------------------------------------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PermissionsScraper.Helpers
{
    /// <summary>
    /// Helper class to call a protected API and returns the result of the call
    /// </summary>
    internal static class ProtectedApiCallHelper
    {
        /// <summary>
        /// Calls a protected Web API
        /// </summary>
        /// <param name="webApiUrl">Url of the Web API to call</param>
        /// <param name="accessToken">Access token used as a bearer security token to call the Web API</param>
        /// <returns>The response of the call to the Web API</returns>
        internal static async Task<string> CallWebApiAsync(string webApiUrl, string accessToken)
        {
            if (string.IsNullOrEmpty(webApiUrl))
            {
                throw new ArgumentNullException(nameof(webApiUrl), "Parameter cannot be null or empty");
            }
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken), "Parameter cannot be null or empty");
            }

            HttpClient httpClient = new HttpClient();
            HttpRequestHeaders defaultRequestHeaders = httpClient.DefaultRequestHeaders;
            if (!defaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }
            defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

            HttpResponseMessage response = await httpClient.GetAsync(webApiUrl);
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return json;
            }
            else
            {
                throw new Exception(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
