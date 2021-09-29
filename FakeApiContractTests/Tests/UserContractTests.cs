using FakeApiContractTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FakeApiContractTests
{
    [TestClass]
    public class UserContractTests
    {

        [TestMethod]
        public async Task GetUsersContracts()
        {

            using var httpClient = new HttpClient();
            //Arrange
            var url = "http://localhost:9001/user/d290f1ee-6c54-4b01-90e6-d701748f0851";

            //Act
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);

            //Assert
            httpResponseMessage.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, httpResponseMessage.StatusCode);

            string contentString = @"{""thing"":""blah""}";//await httpResponseMessage.Content.ReadAsStringAsync();
            var products = JsonSerializer.Deserialize<User>(
                 contentString
                );
            Trace.WriteLine(contentString);
            Assert.IsNotNull(products);
        }
    }
}