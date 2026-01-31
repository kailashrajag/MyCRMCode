using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client; // <--- REQUIRED for OrganizationServiceContext
using Microsoft.Xrm.Sdk.Query;
using MyPlugins.Entities; // Assumes your Early Bound classes are here
using System;
using System.Linq; // <--- REQUIRED for LINQ queries
using System.ServiceModel;

namespace MyPlugins
{
    public class DuplicateCheckLINQ : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];

                try
                {
                    // 1. Cast Target to Early Bound Contact
                    Contact contact = entity.ToEntity<Contact>();

                    // --- PART A: LINQ DUPLICATE CHECK (New Logic) ---

                    // Only check if the incoming contact actually has an email address
                    if (contact.EmailAddress1 != null)
                    {
                        // Create the Service Context to enable LINQ
                        using (OrganizationServiceContext ctx = new OrganizationServiceContext(service))
                        {
                            // Use LINQ to query the database for existing contacts with this email
                            var contactWithSameEmail = ctx.CreateQuery<Contact>()
                                                          .Where(c => c.EmailAddress1 == contact.EmailAddress1)
                                                          .Select(c => c.Id) // Optimization: Only select ID, not all columns
                                                          .FirstOrDefault();

                            if (contactWithSameEmail != Guid.Empty)
                            {
                                throw new InvalidPluginExecutionException("A contact with this email address already exists.");
                            }
                        }
                    }

                    // --- PART B: EXISTING BUSINESS LOGIC (Concatenation) ---

                    // Create Shared Variable
                    context.SharedVariables.Add("Key1", "Some Info");

                    // Read Data (Safe navigation with Early Binding)
                    string firstname = contact.FirstName ?? string.Empty;
                    string lastname = contact.LastName ?? string.Empty;

                    string message = "Hello World! " + firstname + " " + lastname;

                    // Update Target
                    contact.Description = message;

                    // Note: Ensure 'SpousesName' is generated in your proxy class. 
                    // If it is a custom field, it usually looks like contact.new_SpousesName
                    if (contact.Attributes.ContainsKey("new_spousesname")) // specific check if needed
                    {
                        // casting to dynamic or using generated property if available
                        // contact.new_SpousesName = message; 
                    }
                }
                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in MyPlug-in.", ex);
                }
                catch (Exception ex)
                {
                    tracingService.Trace("MyPlugin: {0}", ex.ToString());
                    throw;
                }
            }
        }
    }
}