using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplicationLogic.Catalog.Languages.Dto;

namespace WebApplicationLogic.Catalog.Languages
{
    public interface ILanguageService
    {
        Task<List<LanguageViewModel>> GetAll();
    }
}
