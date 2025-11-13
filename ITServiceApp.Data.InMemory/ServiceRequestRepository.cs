//реализация хранения заявок В ПАМЯТИ
using System;
using System.Collections.Generic;
using System.Linq;
using ITServiceApp.Data.Interfaces;
using ITServiceApp.Domain.Models;

namespace ITServiceApp.Data.InMemory
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly List<ServiceRequest> _requests = new List<ServiceRequest>();

        public List<ServiceRequest> GetAll(ServiceRequestFilter filter)
        {
            return _requests.Select(r => r.Clone()).ToList();
        }

        public Guid Add(ServiceRequest request)
        {
            var clone = request.Clone();
            clone.Id = Guid.NewGuid();
            _requests.Add(clone);
            return clone.Id;
        }

        public void Update(ServiceRequest request)
        {
            var index = _requests.FindIndex(r => r.Id == request.Id);
            if (index >= 0)
            {
                _requests[index] = request.Clone();
            }
        }

        public void Delete(ServiceRequest request)
        {
            _requests.RemoveAll(r => r.Id == request.Id);
        }
    }
}
