using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotifyNewPost
{
    internal class RequestModel
    {
        [JsonProperty("email")]
        public string Email { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
