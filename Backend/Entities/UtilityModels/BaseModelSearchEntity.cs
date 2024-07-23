using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.UtilityModels
{
    public class BaseModelSearchEntity
    {
        public int? id { get; set; }
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public string? sortBy { get; set; }
        public string? sortOrder { get; set; }
    }
}
