// для работы с заявками
using System;
using System.Collections.Generic;
using ITServiceApp.Domain.Models;

namespace ITServiceApp.Data.Interfaces
{
    public interface IServiceRequestRepository
    {
        List<ServiceRequest> GetAll(ServiceRequestFilter filter);
        Guid Add(ServiceRequest request);
        void Update(ServiceRequest request);
        void Delete(ServiceRequest request);
    }
}