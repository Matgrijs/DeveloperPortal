﻿using Auth0.AuthenticationApi;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeveloperPortal.Services
{
    public class UserService
    {
        private readonly Auth0ManagementService _auth0ManagementServiceService;
        private readonly ILogger<UserService> _logger;
        private readonly string _domain = "developerportal.eu.auth0.com";

        public UserService(Auth0ManagementService auth0ManagementServiceService, ILogger<UserService> logger)
        {
            _auth0ManagementServiceService = auth0ManagementServiceService;
            _logger = logger;
        }

        public async Task<IList<CustomUser>> GetUsersAsync()
        {
            try
            {
                var token = await _auth0ManagementServiceService.GetManagementApiToken();
                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("Access token is missing.");
                }

                var managementApiClient = new ManagementApiClient(token, new Uri($"https://{_domain}/api/v2/"));
                var users = await managementApiClient.Users.GetAllAsync(new GetUsersRequest());
                return users.Select(u => new CustomUser
                {
                    Name = u.FullName,
                }).ToList();
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
                throw;
            }
        }

        public class CustomUser
        {
            public string Name { get; set; }
            public string SelectedValue { get; set; }
        }

    }
}

