using AppointmentService.Application.Abstractions.Clients;

namespace AppointmentService.Infrastructure.Clients
{
    public class PatientServiceClient : IPatientServiceClient
    {
        private readonly HttpClient _httpClient;

        public PatientServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> PatientExistsAsync(Guid patientId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/patients/{patientId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}