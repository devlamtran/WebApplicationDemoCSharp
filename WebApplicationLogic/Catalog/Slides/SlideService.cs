using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplicationData.EF;
using WebApplicationLogic.Catalog.Slides.Dto;

namespace WebApplicationLogic.Catalog.Slides
{
    public class SlideService : ISlideService
    {
        private readonly WebApplicationContext _context;

        public SlideService(WebApplicationContext context)
        {
            _context = context;
        }
        public async Task<List<SlideViewModel>> GetAll()
        {
            var slides = await _context.Slides.OrderBy(x => x.SortOrder)
                .Select(x => new SlideViewModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Url = x.Url,
                    Image = x.Image
                }).ToListAsync();

            return slides;
        }
    }
}
