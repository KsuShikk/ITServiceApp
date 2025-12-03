using System;
using System.Collections.Generic;
using ITServiceApp.Domain.Models;

namespace ITServiceApp.Data.Interfaces
{
    public interface IServiceRequestRepository
    {
        IReadOnlyList<ServiceRequest> GetAll(ServiceRequestFilter filter);
        ServiceRequest? GetById(Guid id);
        Guid Add(ServiceRequest request);
        bool Update(Guid id, ServiceRequest updated);
        bool Delete(Guid id);
    }
}
