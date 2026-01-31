using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 1. Connect to Dataverse
                Console.WriteLine("Connecting to Dataverse...");
                var service = CrmConnection.GetService();
                Console.WriteLine("Connection Successful.\n");

                // --- SECTION A: CREATE (Commented out for now) ---
                // Uncomment this block only when you actually want to create data.
                /*
                Console.WriteLine("Creating a new contact...");
                CRMService crmService = new CRMService(service);
                Guid newId = crmService.CreateContact("Doe", "555-0101", "doe@test.com");
                Console.WriteLine($"Contact created with ID: {newId}\n");
                */

                // --- SECTION B: READ CONTACTS ---
                Console.WriteLine("--- Retrieving Contacts ---");
                XMLQuery query = new XMLQuery(service);
                var contacts = query.GetContacts();

                foreach (var contact in contacts)
                {
                    // Using a -20 padding to align the text nicely
                    Console.WriteLine($"{contact.FullName,-20} | {contact.Phone,-15} | {contact.Email}");
                }
                Console.WriteLine($"Rows returned: {contacts.Count}\n");

                // --- SECTION C: READ LEAD COUNT (Aggregation) ---
                Console.WriteLine("--- Retrieving Account Count ---");
                int accountCount = query.GetAccountCount();
                Console.WriteLine($"Total Accounts in System: {accountCount}");

                Console.WriteLine("\nPress Enter to exit...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                Console.ReadLine();
            }
        }
    }
}