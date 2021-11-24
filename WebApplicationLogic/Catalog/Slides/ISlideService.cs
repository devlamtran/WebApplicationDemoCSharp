using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApplicationLogic.Catalog.Slides.Dto;

namespace WebApplicationLogic.Catalog.Slides
{
    public interface ISlideService
    {
        
            Task<List<SlideViewModel>> GetAll();
        
    }
}
