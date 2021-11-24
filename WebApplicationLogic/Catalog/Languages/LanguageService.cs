using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApplicationData.EF;
using WebApplicationLogic.Catalog.Languages.Dto;

namespace WebApplicationLogic.Catalog.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly IConfiguration _config;
        private readonly WebApplicationContext _context;

        public LanguageService(IConfiguration config, WebApplicationContext context)
        {
            _config = config;
            _context = context;
        }

        public async Task<List<LanguageViewModel>> GetAll()
        {
            var languages = await _context.Languages.Select(x => new LanguageViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                IsDefault = x.IsDefault
            }).ToListAsync();
            return languages;
        }
    }
}
