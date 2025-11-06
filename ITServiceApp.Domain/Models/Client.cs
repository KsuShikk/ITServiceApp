using System;

namespace ITServiceApp.Domain.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        

        public Client()
        {
            Id = Guid.NewGuid();
            Name = string.Empty;
            Phone = string.Empty;
            
        }

        public Client Clone()
        {
            return new Client
            {
                Id = this.Id,
                Name = this.Name,
                Phone = this.Phone,
            };
        }
    }
}