using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplicationLogic.Dtos
{
    public  class Group<k, T>
    {
        public k Key;
        public IEnumerable<T> Values;

    }
}
