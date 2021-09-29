using System;
using Newtonsoft.Json;

namespace FakeApiContractTests.Models
{
    [JsonObject(ItemRequired = Required.Always)]
    public class User
    {
        public string id { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public int role { get; set; }

    }
}
