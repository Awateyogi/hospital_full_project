using Microsoft.AspNetCore.Http.HttpResults;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;



namespace HospitalProjBackend.Services
{
    public interface ISmsService { Task SendAsync(string to, string message); }

    public class SmsService : ISmsService
    {
        private readonly IConfiguration _cfg;
        public SmsService(IConfiguration cfg) { _cfg = cfg; }
        public Task SendAsync(string to, string message)
        {
            var sid = _cfg["Twilio:AccountSid"];
            var token = _cfg["Twilio:AuthToken"];
            var from = _cfg["Twilio:FromNumber"];
            if (string.IsNullOrEmpty(sid) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(from))
            {
                Console.WriteLine($"[SMS stub] To:{to} Msg:{message} - Twilio not configured");
                return Task.CompletedTask;
            }
            TwilioClient.Init(sid, token);
            var msg = MessageResource.Create(to: new PhoneNumber(to), from: new PhoneNumber(from), body: message);
            Console.WriteLine($"[SMS] SID: {msg.Sid}");
            return Task.CompletedTask;
        }

    }
}

