﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackEnd.Data;
using ConferenceDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public SearchController(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        [HttpPost]
        public async Task<ActionResult<List<SearchResult>>> Search(SearchTerm term)
        {
            var query = term.Query.ToLowerInvariant();
            var sessionResults = (await _dbContext.Sessions.Include(s => s.Track)
                                                .Include(s => s.SessionSpeakers)
                                                    .ThenInclude(ss => ss.Speaker)
                                                    .ToListAsync())
                                                .Where(s =>
                                                    s.Title.ToLowerInvariant().Contains(query) ||
                                                    s.Track.Name.ToLowerInvariant().Contains(query)
                                                )
                                                .ToList();

            var speakerResults = (await _dbContext.Speakers.Include(s => s.SessionSpeakers)
                                                    .ThenInclude(ss => ss.Session)
                                                    .ToListAsync())
                                                .Where(s =>
                                                    (s.Name?.ToLowerInvariant().Contains(query) ?? false) ||
                                                    (s.Bio?.ToLowerInvariant().Contains(query) ?? false )||
                                                    (s.WebSite?.ToLowerInvariant().Contains(query) ?? false)
                                                )
                                                .ToList();

            var results = sessionResults.Select(s => new SearchResult
            {
                Type = SearchResultType.Session,
                Session = s.MapSessionResponse()
            })
            .Concat(speakerResults.Select(s => new SearchResult
            {
                Type = SearchResultType.Speaker,
                Speaker = s.MapSpeakerResponse()
            }));

            return results.ToList();
        }
    }
}