using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PloomesApi.Models
{
    public class Case
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public int CustomerId { get; set; }
        public int? AttendantId { get; set; }
        public int Status { get; set; }
        public int? Priority { get; set; }
    }
}
