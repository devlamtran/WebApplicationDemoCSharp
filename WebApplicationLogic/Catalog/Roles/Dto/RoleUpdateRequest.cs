﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplicationLogic.Catalog.Roles.Dto
{
    public class RoleUpdateRequest
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
