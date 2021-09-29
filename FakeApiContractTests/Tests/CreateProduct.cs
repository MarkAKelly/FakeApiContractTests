using FakeApiContractTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FakeApiContractTests
{
    [TestClass]
    public class CreateProductContractTests
    {

        string productId = "d290f1ee-6c54-4b01-90e6-d701748f0851";

        [TestMethod]
        public async Task CreateProductSuccess()
        {

            using var httpClient = new HttpClient();
            //Arrange
            var url = $"http://localhost:9002/product";
            Product product = new Product
            {
                id = productId,
                name = "Burger",
                category = "Main",
                price = new Price {
                    currency = "EUR",
                    amount = 2.99 }
            };
            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");


            //Act
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, content);

            //Assert
            httpResponseMessage.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);

            string contentString = await httpResponseMessage.Content.ReadAsStringAsync();
            var resp = JsonSerializer.Deserialize<Product>(
                contentString
                );

            //TODO: add more stringent json response checks to this, e.g. required fields, invalid objects 
            Assert.IsNotNull(resp);
        }

        [TestMethod]
        public async Task CreateProductBadRequest()
        {

            using var httpClient = new HttpClient();
            //Arrange
            var url = $"http://localhost:9002/product";
            var content = new StringContent(@"{""invalid"":""product body""}", Encoding.UTF8, "application/json");


            //Act
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, content);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);

            string contentString = await httpResponseMessage.Content.ReadAsStringAsync();
            string response = @"{""error"":""An incorrect body was supplied""}";
 
            Assert.AreEqual(response, contentString);
        }

        [TestMethod]
        public async Task CreateProductForbidden()
        {

            using var httpClient = new HttpClient();
            //Arrange
            var url = $"http://localhost:9002/product";
            var content = new StringContent(@"{""invalid"":""product body""}", Encoding.UTF8, "application/json");


            //Act
            httpClient.DefaultRequestHeaders.Add("TEST_SCENARIO", "FORBIDDEN");
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, content);

            //Assert
            Assert.AreEqual(HttpStatusCode.Forbidden, httpResponseMessage.StatusCode);

            string contentString = await httpResponseMessage.Content.ReadAsStringAsync();
            string response = @"{""error"":""User not authorised to access this resource""}";

            Assert.AreEqual(response, contentString);
        }

        [TestMethod]
        public async Task CreateProductServerError()
        {

            using var httpClient = new HttpClient();
            //Arrange
            var url = $"http://localhost:9002/product";
            var content = new StringContent(@"{""invalid"":""product body""}", Encoding.UTF8, "application/json");


            //Act
            httpClient.DefaultRequestHeaders.Add("TEST_SCENARIO", "SERVER_ERROR");
            HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(url, content);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, httpResponseMessage.StatusCode);

            string contentString = await httpResponseMessage.Content.ReadAsStringAsync();
            string response = @"{""error"":""An Internal Server Error Occurred""}";

            Assert.AreEqual(response, contentString);
        }
    }
}