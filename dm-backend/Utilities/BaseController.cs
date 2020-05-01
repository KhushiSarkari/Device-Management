using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using dm_backend.Data;
using Microsoft.AspNetCore.Mvc;

public abstract class BaseController : Controller
{
    protected EFDbContext _dbcontext { get; set; }
    protected BaseController(EFDbContext _ef){
        _dbcontext = _ef;
    }
    protected int GetUserId()
    {
        return int.Parse(this.User.Claims.First(i => i.Type == ClaimTypes.NameIdentifier).Value);
    }
    protected string GetUserName()
    {
        return this.User.Claims.First(i => i.Type == ClaimTypes.Name).Value;
    }
    protected List<string> GetUserRoles()
    {
        return this.User.Claims.Where(i => i.Type == ClaimTypes.Role).Select(i => i.Value).ToList();
    }
    protected List<string> GetUserPermissions()
    {
        return  (from role in _dbcontext.Role
                join rp in _dbcontext.RoleToPermission on
                role.RoleId equals rp.RoleId
                join perm in _dbcontext.Permission on
                rp.PermissionId equals perm.PermissionId
                select perm.PermissionName).Distinct().ToList();
    }
}
