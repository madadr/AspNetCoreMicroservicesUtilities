using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests
{
    internal static class TestApi
    {
        class ValueObject
        {
            public int Value { get; set; }
        }

        public static async Task<int> GetCounterValue(HttpClient client, string counterEndpoint)
        {
            var response = (await client.GetAsync(counterEndpoint));
            Assert.InRange((int) response.StatusCode, 200, 299);
            var responseContentAsString = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(responseContentAsString));
            var valueObject = JsonConvert.DeserializeObject<ValueObject>(responseContentAsString);
            Assert.NotNull(valueObject);
            return valueObject.Value;
        }

        public static async Task ResetCounterValue(HttpClient client, string counterEndpoint)
        {
            var response = (await client.DeleteAsync(counterEndpoint));
            Assert.InRange((int) response.StatusCode, 200, 299);
        }
    }

    internal static class TicketsApi
    {
        private const string Url = "http://localhost:5550/";
        private const string OrderUrl = Url + "order/";
        private const string ProcessedCommandsUrl = Url + "test/processedcommands/";

        public static async Task PostRandomOrder(HttpClient client)
        {
            string payload = JsonConvert.SerializeObject(new
            {
                seat = "random",
            });
            var requestContent = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(OrderUrl, requestContent);
            Assert.InRange((int) response.StatusCode, 200, 299); 
            // Disable above assertion if testing implementation without outbox
        }

        public static async Task<int> GetProcessedCommands(HttpClient client)
        {
            return await TestApi.GetCounterValue(client, ProcessedCommandsUrl);
        }

        public static async Task ResetProcessedCommandsCounter(HttpClient client)
        {
            await TestApi.ResetCounterValue(client, ProcessedCommandsUrl);
        }
    }

    internal static class MailNotificationsApi
    {
        private const string Url = "http://localhost:5551/";
        private const string ReceivedEventsUrl = Url + "test/receivedevents/";
        private const string UniqueReceivedEventsUrl = Url + "test/uniquereceivedevents/";


        public static async Task<int> GetReceivedEvents(HttpClient client)
        {
            return await TestApi.GetCounterValue(client, ReceivedEventsUrl);
        }

        public static async Task ResetReceivedEvents(HttpClient client)
        {
            await TestApi.ResetCounterValue(client, ReceivedEventsUrl);
        }

        public static async Task<int> GetUniqueReceivedEvents(HttpClient client)
        {
            return await TestApi.GetCounterValue(client, UniqueReceivedEventsUrl);
        }

        public static async Task ResetUniqueReceivedEvents(HttpClient client)
        {
            await TestApi.ResetCounterValue(client, UniqueReceivedEventsUrl);
        }
    }

    public class TicketBoughtEventTest
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly HttpClient _client;
        private static readonly TimeSpan WaitTimeInterval = TimeSpan.FromSeconds(20);

        // Preconditions:
        // Ticket service running at TicketsApi.Url
        // MailNotifications service running at MailNotificationsApi.Url
        // RabbitMq running on port 5672
        // MongoDb running on port 27017
        public TicketBoughtEventTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
            _client = new HttpClient();
        }

        [Fact]
        public async Task MailNotificationsServiceShouldReceiveEventProducedByTicketsService()
        {
            await TicketsApi.ResetProcessedCommandsCounter(_client);
            await MailNotificationsApi.ResetReceivedEvents(_client);
            await MailNotificationsApi.ResetUniqueReceivedEvents(_client);

            await TicketsApi.PostRandomOrder(_client);

            await Task.Delay(WaitTimeInterval);

            await ValidateEventHandling();
        }

        [Fact]
        public async Task MailNotificationsServiceShouldReceiveAllEventsProducedByTicketsService()
        {
            await TicketsApi.ResetProcessedCommandsCounter(_client);
            await MailNotificationsApi.ResetReceivedEvents(_client);
            await MailNotificationsApi.ResetUniqueReceivedEvents(_client);

            const int requests = 10000;
            for (var i = 1; i <= requests; ++i)
            {
                await TicketsApi.PostRandomOrder(_client);
            }

            await Task.Delay(WaitTimeInterval);

            await ValidateEventHandling(requests);
        }

        [Fact]
        public async Task MailNotificationsServiceShouldReceiveAllEventsProducedByTicketsService_RequestsWithDelay()
        {
            await TicketsApi.ResetProcessedCommandsCounter(_client);
            await MailNotificationsApi.ResetReceivedEvents(_client);
            await MailNotificationsApi.ResetUniqueReceivedEvents(_client);

            const int requests = 120;
            for (var i = 1; i <= requests; ++i)
            {
                await TicketsApi.PostRandomOrder(_client);
                await Task.Delay(TimeSpan.FromSeconds(0.5));
            }

            await Task.Delay(WaitTimeInterval);

            await ValidateEventHandling(requests);
        }

        private async Task ValidateEventHandling(int requests = 1)
        {
            var processedCommands = await TicketsApi.GetProcessedCommands(_client);
            var uniqueReceivedEvents = await MailNotificationsApi.GetUniqueReceivedEvents(_client);
            var receivedEvents = await MailNotificationsApi.GetReceivedEvents(_client);
            _testOutputHelper.WriteLine($"TicketsApi: processed commands: {processedCommands}");
            _testOutputHelper.WriteLine($"MailNotificationsApi: unique received events: {uniqueReceivedEvents}");
            _testOutputHelper.WriteLine($"MailNotificationsApi: received events: {receivedEvents}");
            Assert.Equal(requests, processedCommands);
            Assert.Equal(uniqueReceivedEvents, processedCommands);
            Assert.True(receivedEvents >= processedCommands);
        }
    }
}