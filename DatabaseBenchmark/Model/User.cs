using System;
using System.ComponentModel.DataAnnotations;

namespace DatabaseBenchmark
{
    public class User
    {
        public Guid Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Category { get; set; }
        public string Bio { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ModifiedAt { get; set; }
    }
}
