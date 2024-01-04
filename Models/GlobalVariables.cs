using B2S_API_Comm.Services.Interfaces;
using B2S_API_Comm.Services.Models;
using Models.Handlers;

namespace B2S_API_Comm.Models
{
    public static class GlobalVariables
    {
        public static string API_URI = "https://192.168.200.37:44390/"; // RELEASE API
		//public static string API_URI = "https://192.168.200.37:44400/"; // DEBUG API
		public static B2SHttpClientHandler B2SHttpClientHandler { get; set; } = new(API_URI);
        public static IB2SAPICommunicationMOP B2SAPICommunicationMOP { get; set; } = new B2SAPICommunicationModule(API_URI);
		public static IB2SAPICommunicationOCR B2SAPICommunicationOCR { get; set; } = new B2SAPICommunicationModule(API_URI);
	}
}
