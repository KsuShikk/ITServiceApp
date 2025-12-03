using System;
using System.Collections.Generic;
using ITServiceApp.Domain.Models;

namespace ITServiceApp.Data.Interfaces
{
    public interface IEngineerRepository
    {
        IReadOnlyList<Engineer> GetAll();
        Engineer? GetById(Guid id);
        Guid Add(Engineer engineer);
        bool Update(Guid id, Engineer engineer);
        bool Delete(Guid id);
    }
}
