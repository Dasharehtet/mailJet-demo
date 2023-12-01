using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace mailJet
{
    public static class Program
    {
        private static void Main()
        {
            RunAsync().Wait();
        }

        private static async Task RunAsync()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: true);
        
            IConfiguration config = builder.Build();
            
            var apiKey = config.GetSection("ApiKey").Value;
            var secret = config.GetSection("Secret").Value;
            var emailFrom = config.GetSection("EmailFrom").Value;
            var nameFrom = config.GetSection("NameFrom").Value;
            var emailTo = config.GetSection("EmailTo").Value;
            
            var client = new MailjetClient(apiKey, secret);
            var request = new MailjetRequest
                {
                    Resource = SendV31.Resource
                }
                .Property(Send.Messages, new JArray {
                    new JObject {
                        {
                            "From",
                            new JObject {
                                {"Email", emailFrom},
                                {"Name", nameFrom}
                            }
                        }, {
                            "To",
                            new JArray {
                                new JObject {
                                    {
                                        "Email", emailTo
                                    },
                                }
                            }
                        }, {
                            "Subject",
                            "Greetings from Mailjet."
                        }, {
                            "TextPart",
                            "My first Mailjet email"
                        }, {
                            "HTMLPart",
                            "<h3>Dear passenger 1, welcome to <a href='https://www.mailjet.com/'>Mailjet</a>!</h3><br />May the delivery force be with you!"
                        }, {
                            "CustomID",
                            "AppGettingStartedTest"
                        }
                    }
                });
            var response = await client.PostAsync(request);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Total: {response.GetTotal()}, Count: {response.GetCount()}\n");
                Console.WriteLine(response.GetData());
            }
            else
            {
                Console.WriteLine($"StatusCode: {response.StatusCode}\n");
                Console.WriteLine($"ErrorInfo: {response.GetErrorInfo()}\n");
                Console.WriteLine(response.GetData());
                Console.WriteLine($"ErrorMessage: {response.GetErrorMessage()}\n");
            }
        }
    }
}