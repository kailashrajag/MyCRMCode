using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    internal class XMLQuery
    {
        private readonly IOrganizationService _service;

        public XMLQuery(IOrganizationService service)
        {
            _service = service;
        }

        // Method 1: Get list of Contacts
        public List<ContactModel> GetContacts()
        {
            var contacts = new List<ContactModel>();

            string fetchXml = @"
            <fetch version='1.0' distinct='true'>
              <entity name='contact'>
                <attribute name='fullname' />
                <attribute name='telephone1' />
                <attribute name='emailaddress1' />
                <attribute name='contactid' />
                <filter>
                  <condition attribute='statecode' operator='eq' value='0' />
                </filter>
              </entity>
            </fetch>";

            EntityCollection results = _service.RetrieveMultiple(new FetchExpression(fetchXml));

            foreach (var entity in results.Entities)
            {
                contacts.Add(new ContactModel
                {
                    ContactId = entity.Id,
                    FullName = entity.GetAttributeValue<string>("fullname"),
                    Phone = entity.GetAttributeValue<string>("telephone1"),
                    Email = entity.GetAttributeValue<string>("emailaddress1")
                });
            }

            return contacts;
        }

        // Method 2: Get Count of Leads (Aggregation)
        public int GetAccountCount()
        {
            // 1. Define the Alias clearly
            string countAlias = "TotalLeads";

            string query = $@"
            <fetch distinct='false' mapping='logical' aggregate='true'>
              <entity name='account'>
                 <attribute name='accountid' aggregate='count' alias='{countAlias}'/>
              </entity>
            </fetch>";

            // 2. Pass the correct variable 'query' here
            EntityCollection collection = _service.RetrieveMultiple(new FetchExpression(query));

            // 3. Handle the result safely
            foreach (Entity item in collection.Entities)
            {
                // Aggregates return "AliasedValue", not standard attributes
                if (item.Contains(countAlias) && item[countAlias] is AliasedValue aliasedValue)
                {
                    // Return the integer value found
                    return (int)aliasedValue.Value;
                }
            }

            return 0; // Return 0 if nothing found
        }
    }
}