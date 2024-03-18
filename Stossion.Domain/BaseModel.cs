using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.Domain
{
    public class BaseModel
    {
        public DateTime CreatedAt { get; set; }

        public DateTime ModifiedAt { get; set; }
    }
}
