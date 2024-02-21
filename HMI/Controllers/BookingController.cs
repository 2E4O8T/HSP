using HMI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookingDto appointment)
        {
            var content = new StringContent(JsonSerializer.Serialize(appointment), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/gateway/simplebooking", content);

            if(response.IsSuccessStatusCode)
            {
                _logger.LogInformation($"{response.StatusCode} : Appointment added successfully");

                return RedirectToAction("Index");
            }
            else
            {
                _logger.LogError($"{response.StatusCode} : Something went wrong");

                return View(appointment);
            }
        }
    }
}
