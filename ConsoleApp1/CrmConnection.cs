using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class CrmConnection
    {
        public static CrmServiceClient GetService()
        {
            string connectionString =
                        @"AuthType=ClientSecret;
                        Url=https://org74.crm12.dynamics.com;
                        ClientId=<id>;
                        ClientSecret=<secret>;
                        TenantId=<tenant id>";

            var service = new CrmServiceClient(connectionString);

            if (!service.IsReady)
            {
                throw new Exception(
                    $"CRM connection failed: {service.LastCrmError}",
                    service.LastCrmException
                );
            }

            return service;
        }
    }
}
