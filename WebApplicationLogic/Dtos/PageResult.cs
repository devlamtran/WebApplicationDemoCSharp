using System.Collections.Generic;

namespace WebApplicationLogic.Dtos
{
    public class PageResult<T> : PageResultBase
    {
        public List<T> Items { get; set; }
      

    }

    
}
