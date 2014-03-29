using System.Linq;
using RestSharp;
using RestSharp.Deserializers;
using System.Net;
using System.Collections.Generic;

namespace Aping
{
    public class ApIng
    {
        private const string baseApiUri = "https://apisandbox.ingdirect.es";
        private const string contentType = "application/json";

        private string sessionTicket;
        private string apiKey;
        private string cookieValue;
        private string documentNumber;
        private string birthDay;
        private RestClient restClient;
        private JsonDeserializer jsonDeserializer;
        private CookieContainer cookieContainer;

        public ApIng(string apiKey, string documentNumber, string birthDay)
        {
            this.apiKey = apiKey;
            this.restClient = new RestClient(baseApiUri);
            this.cookieContainer = new CookieContainer();
            this.birthDay = birthDay;
            this.documentNumber = documentNumber;
            this.restClient.CookieContainer = cookieContainer;
            this.cookieContainer.PerDomainCapacity = 1;
            this.cookieContainer.Capacity = 1;
            this.restClient.ClearHandlers();
            this.jsonDeserializer = new JsonDeserializer();
            createSessionTicket();
            setCookie();
        }

        public bool createSessionTicket()
        {
            try
            {
                var request = new RestRequest(Method.POST);
                request.Resource = "openlogin/rest/ticket?apikey=" + apiKey;
                request.AddHeader("Content-Type", contentType);   
         
                LoginBody loginBody = new LoginBody();
                LoginDocument loginDocument = new LoginDocument();
                loginBody.birthday = birthDay;
                loginDocument.documentType = 0;
                loginDocument.document = documentNumber;
                loginBody.loginDocument = loginDocument;

                //string temp = request.JsonSerializer.Serialize(loginBody);
                request.RequestFormat = DataFormat.Json;
                request.AddBody(loginBody);
                var response = restClient.Execute(request);
                var ticketResult = jsonDeserializer.Deserialize<TicketResponse>(response);
                sessionTicket = ticketResult.ticket;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool setCookie()
        {
            try
            {
                var request = new RestRequest(Method.POST);
                request.Resource = "openapi/login/auth/response?apikey=" + apiKey;
                TicketBody myBody = new TicketBody();
                myBody.ticket = sessionTicket;
                string ticket = "ticket=" + sessionTicket;
                request.AddParameter("application/x-www-form-urlencoded", "ticket=" + sessionTicket, ParameterType.RequestBody);
                request.AddParameter("Content-Type", "application/x-www-form-urlencoded", ParameterType.HttpHeader);
                var response = restClient.Execute(request);
                var sessionCookie = response.Cookies.ToList();
                cookieValue = "genoma-session-id=" + sessionCookie[0].Value + "; Path=/openapi/";

                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<ProductsList> requestProducts()
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "openapi/rest/products?apikey=" + apiKey;
            request.RequestFormat = DataFormat.Json;
            var response = restClient.Execute(request);
            var result = jsonDeserializer.Deserialize<List<ProductsList>>(response);
            List<ProductsList> productList = result;
            return productList;
        }
        public Customer requestCustomerProfile()
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "openapi/rest/client?apikey=" + apiKey;
            request.RequestFormat = DataFormat.Json;
            var response = restClient.Execute(request);
            var result = jsonDeserializer.Deserialize<Customer>(response);
            return (Customer)result;
        }
        public CustomerContract requestCustomerContract()
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "openapi/rest/client-contact-information?apikey=" + apiKey;
            request.RequestFormat = DataFormat.Json;
            var response = restClient.Execute(request);
            var result = jsonDeserializer.Deserialize<CustomerContract>(response);
            return (CustomerContract) result;
        }
        public CustomerFinancialInformation requestCustomerFinancialInformation()
        {
            var request = new RestRequest(Method.GET);
            request.Resource = "openapi/rest/client-financial-information?apikey=" + apiKey;
            request.RequestFormat = DataFormat.Json;
            var response = restClient.Execute(request);
            var result = jsonDeserializer.Deserialize<CustomerFinancialInformation>(response);
            return (CustomerFinancialInformation)result;
        }
        public PrepareForTransfer requestPrepareForTransfer()
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "openapi/rest/transfers?apikey=" + apiKey;
            request.RequestFormat = DataFormat.Json;
            var response = restClient.Execute(request);
            var result = jsonDeserializer.Deserialize<PrepareForTransfer>(response);
            return (PrepareForTransfer)result;
        }
        public UpdateTransfer requestUpdateTransfer(string transferUuid, MoneyTransfer toTransfer)
        {
            var request = new RestRequest(Method.PUT);
            request.Resource = "openapi/rest/transfers/"+transferUuid+"?apikey=" + apiKey;
            request.AddHeader("transfer_uuid", transferUuid);
            request.AddHeader("Content-Type", contentType);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(toTransfer);
            var response = restClient.Execute(request);
            var result = jsonDeserializer.Deserialize<UpdateTransfer>(response);
            return (UpdateTransfer)result;
        }
        public ConfirmationOfTransfer requestConfirmationOfTransfer(string pinCode, string transferUuid) //must be always "1,1";
        {
            var request = new RestRequest(Method.PUT);
            request.Resource = "openapi/rest/transfers/" + transferUuid + "/accept?apikey=" + apiKey;
            request.AddHeader("transfer_uuid", transferUuid);
            request.AddHeader("Content-Type", contentType);
            request.RequestFormat = DataFormat.Json;
            request.AddBody(new AcceptanceValue { acceptanceValue = pinCode });
            var response = restClient.Execute(request);
            var result = jsonDeserializer.Deserialize<ConfirmationOfTransfer>(response);
            return (ConfirmationOfTransfer)result;
        }
        public bool LogOut()
        {
            var request = new RestRequest(Method.DELETE);
            request.Resource = "openapi/rest/session?apikey=" + apiKey;
            request.AddHeader("Content-Type", contentType);
            request.RequestFormat = DataFormat.Json;
            var response = restClient.Execute(request);
            if(response.Content.ToString().ToLower().Contains("ok"))return true;
            else return false;
        }
        public ConfirmationOfTransfer EasyTransfer(MoneyTransfer toTransfer, string pinCode)
        {
            string id = requestPrepareForTransfer().id;
            UpdateTransfer myUpdateTransfer =  requestUpdateTransfer(id, toTransfer);
            return requestConfirmationOfTransfer(pinCode, id);            
        }
    }
}
