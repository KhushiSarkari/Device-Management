using System;
using System.Linq;
using System.Threading.Tasks;
using dm_backend.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dm_backend.Data
{
    public class EFRepository : IEFRepository
    {
    
        private readonly EFDbContext _context;
        public EFRepository(EFDbContext context)
        {
            _context = context;
        }

       public async Task<Statistics> GetStatus()
        {
            Statistics statisticsObject = new Statistics{
                 totalDevices=await  _context.Device.CountAsync(),
                 assignedDevices=await _context.AssignDevice.CountAsync(),
                 deviceRequests=await _context.RequestDevice.CountAsync(),
                 freeDevices= await _context.Device.Where(ss=> ss.Status.StatusName=="Free").CountAsync(),
                 faults=await _context.Complaints.Where(ss=>ss.ComplaintStatus.StatusName =="Unresolved").CountAsync(),
                 rejectedRequests=await _context.RequestHistory.Where(ss=> ss.Status.StatusName=="Rejected").CountAsync()
               };
               
            return statisticsObject;
        }

    
    }
}