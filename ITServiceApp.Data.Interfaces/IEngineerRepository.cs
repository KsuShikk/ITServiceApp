// для работы с инженерами
using System.Collections.Generic;
using ITServiceApp.Domain.Models;

namespace ITServiceApp.Data.Interfaces
{
    public interface IEngineerRepository
    {
        List<Engineer> GetAll();
    }
}