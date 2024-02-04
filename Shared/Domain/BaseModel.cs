using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fooddelivary.Shared.Domain
{
    public abstract class BaseModel
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateModified { get; set; } = null;

        public string? CreatedBy { get; set; } = null;
        public string? ModifiedBy { get; set; } = null;

    }
}
