using FakeApiContractTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FakeApiContractTests
{
    [TestClass]
    public class GetProductContractTests
    {

        string productId = "d290f1ee-6c54-4b01-90e6-d701748f0851";

        [TestMethod]
        public async Task GetProductsSuccess()
        {

            using var httpClient = new HttpClient();
            //Arrange
            var url = $"http://localhost:9002/product/{productId}";

            //Act
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);

            //Assert
            httpResponseMessage.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, httpResponseMessage.StatusCode);

            string contentString = await httpResponseMessage.Content.ReadAsStringAsync();
            var product = JsonSerializer.Deserialize<Product>(
                contentString
                );

            //TODO: add more stringent json response checks to this, e.g. required fields, invalid objects 
            Assert.IsNotNull(product);
        }

        [TestMethod]
        public async Task GetProductsBadParams()
        {

            using var httpClient = new HttpClient();
            //Arrange
            var url = "http://localhost:9002/product/not-a-product-id";

            //Act
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);

            //Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);

            string contentString = await httpResponseMessage.Content.ReadAsStringAsync();
            string response = @"{""error"":""Invalid ID""}";


            Assert.AreEqual(response, contentString);

        }

        [TestMethod]
        public async Task GetProductsNotFound()
        {

            using var httpClient = new HttpClient();

            //Arrange
            var url = $"http://localhost:9002/product/{productId}";

            //Act
            httpClient.DefaultRequestHeaders.Add("TEST_SCENARIO", "NOT_FOUND");
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);

            //Assert
            Assert.AreEqual(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);

            string contentString = await httpResponseMessage.Content.ReadAsStringAsync();
            string response = @"{""error"":""A product could not be found with the information supplied""}";


            Assert.AreEqual(response, contentString);

        }

        [TestMethod]
        public async Task GetProductsServerError()
        {

            using var httpClient = new HttpClient();

            //Arrange
            var url = $"http://localhost:9002/product/{productId}";

            //Act
            httpClient.DefaultRequestHeaders.Add("TEST_SCENARIO", "SERVER_ERROR");
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);

            //Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, httpResponseMessage.StatusCode);

            string contentString = await httpResponseMessage.Content.ReadAsStringAsync();
            string response = @"{""error"":""An Internal Server Error Occurred""}";


            Assert.AreEqual(response, contentString);

        }

    }
}