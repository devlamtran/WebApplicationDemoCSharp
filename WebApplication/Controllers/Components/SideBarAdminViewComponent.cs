using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplicationLogic.Catalog.Categories;

namespace WebApplication.Controllers.Components
{
    public class SideBarAdminViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public SideBarAdminViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var items = await _categoryService.GetAll(CultureInfo.CurrentCulture.Name);
            return View(items);
        }
    }
}
