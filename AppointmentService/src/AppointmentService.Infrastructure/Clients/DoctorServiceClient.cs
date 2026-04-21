using AppointmentService.Application.Abstractions.Clients;

namespace AppointmentService.Infrastructure.Clients
{
    public class DoctorServiceClient : IDoctorServiceClient
    {
        private readonly HttpClient _httpClient;

        public DoctorServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DoctorExistsAsync(Guid doctorId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/doctors/{doctorId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}