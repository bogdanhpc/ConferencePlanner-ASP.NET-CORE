using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FrontEnd.Pages
{
    public class SpeakersModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public SpeakersModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public IEnumerable<SpeakerResponse> Speakers { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Speakers = await _apiClient.GetSpeakersAsync();

            if (Speakers == null)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}