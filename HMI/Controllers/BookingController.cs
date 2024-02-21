using HMI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HMI.Controllers
{
    public class BookingController : Controller
    {
        private readonly ILogger<BookingController> _logger;
        private readonly HttpClient _httpClient;

        public BookingController(ILogger<BookingController> logger, IHttpClientFactory httpClient)
        {
            _logger = logger;
            _httpClient = httpClient.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:6001");
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var request = await _httpClient.GetAsync("/gateway/simplebooking");

            if (request.IsSuccessStatusCode)
            {
                var appointments = await request.Content.ReadFromJsonAsync<List<BookingDto>>();

                _logger.LogInformation($"List of appointmentsretrieved successfully at {DateTime.UtcNow.ToLongTimeString()}");

                return View(appointments);
            }
            else
            {
                _logger.LogError("Error retrieving appointments");

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
