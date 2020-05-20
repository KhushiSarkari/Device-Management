using System.Collections.Generic;
using System.Threading.Tasks;
using dm_backend.EFModels;
using dm_backend.Models;

namespace dm_backend.Data
{
    public interface IEFRepository
    {
          Task<Statistics> GetStatus();
   }
}