﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LessonsBot_DB.ModelService
{
    [Serializable]
    public class ApiGroups
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Serializable]
    public class ApiGroupsCache : ApiGroups
    {

    }
}
