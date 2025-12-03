using System;
using System.Collections.Generic;
using System.Linq;
using ITServiceApp.Data.Interfaces;
using ITServiceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ITServiceApp.Data.SqlServer
{
    public class ServiceRequestRepository : IServiceRequestRepository
    {
        private readonly ITServiceAppDbContext _context;

        public ServiceRequestRepository(ITServiceAppDbContext context)
        {
            _context = context;
        }

        public IReadOnlyList<ServiceRequest> GetAll(ServiceRequestFilter filter)
        {
            var query = _context.ServiceRequests
                .Include(r => r.Engineer)
                .Include(r => r.Client)
                .AsQueryable()
                .AsNoTracking();

            if (filter?.StartDate.HasValue == true)
                query = query.Where(r => r.CreationDate >= filter.StartDate.Value);

            if (filter?.EndDate.HasValue == true)
                query = query.Where(r => r.CreationDate < filter.EndDate.Value.AddDays(1));

            return query.ToList();
        }

        public ServiceRequest? GetById(Guid id)
        {
            return _context.ServiceRequests
                .Include(r => r.Engineer)
                .Include(r => r.Client)
                .FirstOrDefault(r => r.Id == id);
        }

        public Guid Add(ServiceRequest request)
        {
            // Добавляем клиента, если он новый
            if (request.Client != null)
            {
                var clientEntry = _context.Entry(request.Client);

                if (clientEntry.State == EntityState.Detached)
                {
                    bool clientExists = _context.Clients.Any(c => c.Id == request.Client.Id);

                    if (!clientExists)
                    {
                        _context.Clients.Add(request.Client);
                    }
                    else
                    {
                        _context.Attach(request.Client);
                    }
                }
            }

            // Добавляем инженера, если он есть
            if (request.Engineer != null)
            {
                bool engineerExists = _context.Engineers.Any(e => e.Id == request.Engineer.Id);

                if (!engineerExists)
                    _context.Engineers.Add(request.Engineer);
                else
                    _context.Attach(request.Engineer);
            }

            if (request.Id == Guid.Empty)
                request.Id = Guid.NewGuid();

            _context.ServiceRequests.Add(request);
            _context.SaveChanges();

            return request.Id;
        }


        public bool Update(Guid id, ServiceRequest updated)
        {
            var existing = _context.ServiceRequests
                .Include(r => r.Engineer)
                .Include(r => r.Client)
                .FirstOrDefault(r => r.Id == id);

            if (existing == null)
                return false;

            // Копируем поля (копируй все нужные свойства, кроме Id)
            existing.RequestNumber = updated.RequestNumber;
            existing.CreationDate = updated.CreationDate;
            existing.Status = updated.Status;
            existing.EquipmentType = updated.EquipmentType;
            existing.EquipmentModel = updated.EquipmentModel;
            existing.ProblemDescription = updated.ProblemDescription;
            existing.Comments = updated.Comments;
            existing.OrderedParts = updated.OrderedParts;

            // Client: если передали Client с Id != пустого — присоединяем (attach) или обновляем
            if (updated.Client != null)
            {
                if (updated.Client.Id == Guid.Empty)
                {
                    updated.Client.Id = Guid.NewGuid();
                }

                // Можно обновить существующий client или присоединить
                existing.Client = updated.Client;
                _context.Entry(existing.Client).State = updated.Client.Id == Guid.Empty ? EntityState.Added : EntityState.Modified;
            }

            // Engineer: присоединяем по Id, если задан
            if (updated.Engineer != null)
            {
                if (updated.Engineer.Id != Guid.Empty)
                {
                    // присоединяем существующего инженера
                    var eng = _context.Engineers.Find(updated.Engineer.Id);
                    if (eng != null)
                        existing.Engineer = eng;
                    else
                    {
                        existing.Engineer = updated.Engineer;
                        _context.Entry(existing.Engineer).State = EntityState.Added;
                    }
                }
                else
                {
                    // новый инженер (в учебном проекте маловероятно)
                    updated.Engineer.Id = Guid.NewGuid();
                    existing.Engineer = updated.Engineer;
                    _context.Entry(existing.Engineer).State = EntityState.Added;
                }
            }
            else
            {
                existing.Engineer = null!;
            }

            _context.SaveChanges();
            return true;
        }

        public bool Delete(Guid id)
        {
            var existing = _context.ServiceRequests.Find(id);
            if (existing == null)
                return false;

            _context.ServiceRequests.Remove(existing);
            _context.SaveChanges();
            return true;
        }
    }
}

