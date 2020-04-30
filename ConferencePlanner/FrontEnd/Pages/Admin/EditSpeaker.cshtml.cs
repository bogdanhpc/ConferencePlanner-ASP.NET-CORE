using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages.Admin
{
    public class EditSpeakerModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public EditSpeakerModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        [TempData]
        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        [BindProperty]
        public Speaker Speaker { get; set; }

        public async Task OnGetAsync(int id)
        {
            var speaker = await _apiClient.GetSpeakerAsync(id);
            Speaker = new Speaker
            {
                Id = speaker.Id,
                Name = speaker.Name,
                Bio = speaker.Bio,
                WebSite = speaker.WebSite
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Message = "Speaker updated successfully!";

            await _apiClient.PutSpeakerAsync(Speaker);

            return Page();
        }

        
    }
}