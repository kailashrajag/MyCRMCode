using Microsoft.Xrm.Sdk;
using MyPlugins.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace MyPlugins
{
    public class HelloWorld : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            // Extract the tracing service for use in debugging sandboxed plug-ins.  
            // If you are not registering the plug-in in the sandbox, then you do  
            // not have to add any tracing service related code.  
            ITracingService tracingService =
                (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // Obtain the execution context from the service provider.  
            IPluginExecutionContext context = (IPluginExecutionContext)
                serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Obtain the organization service reference which you will need for  
            // web service calls.  
            IOrganizationServiceFactory serviceFactory =
                (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);



            // The InputParameters collection contains all the data passed in the message request.  
            if (context.InputParameters.Contains("Target") &&
                context.InputParameters["Target"] is Entity)
            {
                // Obtain the target entity from the input parameters.  
                Entity entity = (Entity)context.InputParameters["Target"];


                try
                {
                    // Plug-in business logic goes here.  

                    //Create Shared Variable to be reused in TaskCreate because Hello World and TaskCrate plugins are registered in the
                    //same pipeline i.e on Create of Contact in one plugin in Pre Validation and another in Post Operation
                    // Define shared variable: in Hello World Plugin
                    context.SharedVariables.Add("Key1","Some Info");

                    // 1. Cast the generic entity to your Early Bound class
                    // This gives you access to .FirstName instead of ["firstname"]
                    Contact contact = entity.ToEntity<Contact>();


                    // 2. Read FirstName 
                    // We use the null-coalescing operator (??) to handle empty values safely.
                    string firstname = contact.FirstName ?? string.Empty;
                    

                    // 3. Read LastName
                    // Early Binding is safer here: The original .ToString() would crash if LastName was null.
                    string lastname = contact.LastName ?? string.Empty;

                    // 4. Assign data
                    // This replaces 'Attributes.Add'. It is safer because 'Add' throws an error 
                    // if the field already exists, whereas this property setter handles add-or-update automatically.
                    string message = "Hello World! " + firstname + lastname;

                    contact.Description = message;
                    contact.SpousesName = message;

                }

                catch (FaultException<OrganizationServiceFault> ex)
                {
                    throw new InvalidPluginExecutionException("An error occurred in MyPlug-in.", ex);
                }

                catch (Exception ex)
                {
                    tracingService.Trace("MyPlugin: {0}", ex.ToString());
                    throw;
                    //return; 
                }
            }
        }
    }
}