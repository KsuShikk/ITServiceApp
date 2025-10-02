using System;

namespace ITServiceApp.Domain.Models
{
    public class ServiceRequest
    {
        public Guid Id { get; set; }
        public string RequestNumber { get; set; }
        public Client Client { get; set; }
        public Engineer Engineer { get; set; }
        public string EquipmentType { get; set; }
        public string EquipmentModel { get; set; }
        public string ProblemDescription { get; set; }
        public DateTime CreationDate { get; set; }
        public RequestStatus Status { get; set; }
        public string Comments { get; set; }
        public string OrderedParts { get; set; }

        public ServiceRequest()
        {
            Id = Guid.NewGuid();
            RequestNumber = GenerateRequestNumber();
            Client = new Client();
            Engineer = null;
            EquipmentType = string.Empty;
            EquipmentModel = string.Empty;
            ProblemDescription = string.Empty;
            CreationDate = DateTime.Now;
            Status = RequestStatus.Created;
            Comments = string.Empty;
            OrderedParts = string.Empty;
        }

        public static ServiceRequest Create()
        {
            return new ServiceRequest();
        }

        public ServiceRequest Clone()
        {
            return new ServiceRequest
            {
                Id = Id,
                RequestNumber = RequestNumber,
                Client = new Client
                {
                    Name = Client.Name,
                    Phone = Client.Phone
                },
                Engineer = Engineer,
                EquipmentType = EquipmentType,
                EquipmentModel = EquipmentModel,
                ProblemDescription = ProblemDescription,
                CreationDate = CreationDate,
                Status = Status,
                Comments = Comments,
                OrderedParts = OrderedParts
            };
        }

        private static string GenerateRequestNumber()
        {
            return $"REQ{DateTime.Now:yyyyMMddHHmmss}";
        }
    }
}
