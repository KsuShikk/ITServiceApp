//Реализация хранения инженеров В ПАМЯТИ
using System.Collections.Generic;
using System.Linq;
using ITServiceApp.Data.Interfaces;
using ITServiceApp.Domain.Models;

namespace ITServiceApp.Data.InMemory
{
    public class EngineerRepository : IEngineerRepository
    {
        private readonly List<Engineer> _engineers = new List<Engineer>();

        public List<Engineer> GetAll()
        {
            return _engineers.ToList();
        }

        public void Seed(List<Engineer> engineers)
        {
            _engineers.Clear();
            _engineers.AddRange(engineers);
        }
    }
}