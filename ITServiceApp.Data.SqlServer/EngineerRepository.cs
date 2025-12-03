using System;
using System.Collections.Generic;
using System.Linq;
using ITServiceApp.Data.Interfaces;
using ITServiceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ITServiceApp.Data.SqlServer
{
    public class EngineerRepository : IEngineerRepository
    {
        private readonly ITServiceAppDbContext _context;

        public EngineerRepository(ITServiceAppDbContext context)
        {
            _context = context;
        }

        public IReadOnlyList<Engineer> GetAll()
        {
            return _context.Engineers.AsNoTracking().ToList();
        }

        public Engineer? GetById(Guid id)
        {
            return _context.Engineers.Find(id);
        }

        public Guid Add(Engineer engineer)
        {
            if (engineer.Id == Guid.Empty)
                engineer.Id = Guid.NewGuid();

            _context.Engineers.Add(engineer);
            _context.SaveChanges();
            return engineer.Id;
        }

        public bool Update(Guid id, Engineer updated)
        {
            var existing = _context.Engineers.Find(id);
            if (existing == null) return false;

            existing.Name = updated.Name;
            existing.Specialization = updated.Specialization;
            _context.SaveChanges();
            return true;
        }

        public bool Delete(Guid id)
        {
            var existing = _context.Engineers.Find(id);
            if (existing == null) return false;

            _context.Engineers.Remove(existing);
            _context.SaveChanges();
            return true;
        }
    }
}

