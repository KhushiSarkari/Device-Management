using System.Collections.Generic;
using dm_backend.Models;

namespace dm_backend.Data
{
    public interface IDeviceRepository
    {
        string ResolveRequest(int complaintId);
        string MarkFaultyRequest(int complaintId);
        List<FaultyDeviceModel> getFaultyDevice(string search, string serialNumber, string sortAttribute, string direction);
    }
}