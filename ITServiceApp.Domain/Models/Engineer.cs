using System;

namespace ITServiceApp.Domain.Models
{
    public class Engineer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }

        public Engineer()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Specialization = string.Empty;
        }
    }
}