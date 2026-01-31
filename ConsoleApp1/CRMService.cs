using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class CRMService
    {
        private readonly IOrganizationService _service;

        public CRMService(IOrganizationService service)
        {
            _service = service;
        }

        public Guid CreateContact(string lastName)
        {
            Entity contact = new Entity("contact");
            contact["lastname"] = lastName;

            return _service.Create(contact);
        }

    }
}
