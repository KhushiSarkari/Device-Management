// using System;
// using System.Collections.Generic;
// using System.Linq;
// using dm_backend.Data;
// using dm_backend.Models;


// namespace dm_backend.Utilities
// {
//     public class EfQueries
//     {
//         private  readonly EFDbContext _context;
//         public  EfQueries(EFDbContext context)
//         {
//             _context = context;

// }

//         public  IQueryable<User> GetUserQuery(){
              
//                   var result =  (from u in _context.User
//                           join sl in _context.Salutation.DefaultIfEmpty() on u.SalutationId equals sl.SalutationId
//                           join rtl in _context.UserToRole.DefaultIfEmpty() on u.UserId equals rtl.UserId
//                           join rl in _context.Role.DefaultIfEmpty() on rtl.RoleId equals rl.RoleId
//                           join s in _context.Status.DefaultIfEmpty() on u.Status equals s.StatusId
//                          // join ph1 in _context.ContactNumber.DefaultIfEmpty() on u.UserId equals ph1.UserId
//                        //   join ct in _context.ContactType.DefaultIfEmpty() on ph1.ContactTypeId equals ct.ContactTypeId
//                           join ddi in _context.DepartmentDesignation.DefaultIfEmpty() on u.DepartmentDesignationId equals ddi.DepartmentDesignationId
//                           join dep in _context.Department.DefaultIfEmpty() on ddi.DepartmentId equals dep.DepartmentId
//                           join desg in _context.Designation.DefaultIfEmpty() on ddi.DesignationId equals desg.DesignationId
//                           join gen in _context.Gender.DefaultIfEmpty() on u.GenderId equals gen.GenderId
                          
//                         // where u.UserId==abc
                          
//                           select new Models.User {UserId = u.UserId, Salutation = sl.Salutation1, FirstName = u.FirstName, MiddleName = u.MiddleName,
//                            LastName = u.LastName,DOB = u.DateOfBirth.ToString("yyyy/dd/MM"),DOJ = u.DateOfJoining.ToString("yyyy/dd/mm"), 
//                            Email = u.Email, RoleName = rl.RoleName, Status = s.StatusName,DepartmentName = dep.DepartmentName,DesignationName = desg.DesignationName,Gender = gen.Gender1,
//                            phones = (
//                                 from ph1 in _context.ContactNumber.Where(e=>e.UserId==u.UserId)
//                                  join ct1 in _context.ContactType.DefaultIfEmpty() on ph1.ContactTypeId equals ct1.ContactTypeId
//                                 join cc1 in _context.Country.DefaultIfEmpty() on ph1.CountryId equals cc1.CountryId
//                                select new ContactNumberModel{Number = ph1.Number,ContactNumberType = ct1.ContactType1,AreaCode = ph1.AreaCode,CountryCode = cc1.CountryCode.ToString()}
//                            ).ToList(),
//                            addresses = (
//                                from add1 in _context.Address.Where(e=>e.UserId==u.UserId)
//                                join adt in _context.AddressType on add1.AddressTypeId equals adt.AddressTypeId
//                                join citi in _context.City on add1.CityId equals citi.CityId
//                                join stat in _context.State on citi.StateId equals stat.StateId
//                                join countr in _context.Country on stat.CountryId equals countr.CountryId
//                                select new AddressModel{AddressType = adt.AddressType1,AddressLine1 = add1.AddressLine1,
//                                AddressLine2 = add1.AddressLine2,PIN = add1.Pin,City = citi.CityName,State = stat.StateName,Country = countr.CountryName}
//                            ).ToList()});
                         
//                            // var list = new List<Models.User>(result);
//                             // var OneUser = new List<Models.User>();
//                             // if(abc!=-1){
//                             //     list.ForEach(i=>
//                             //   {
//                             //       if(i.UserId==abc)
//                             //       {

//                             //         //  Console.WriteLine
//                             //       OneUser.Add(i);
//                             //      }
//                             //  });
//                             //  return OneUser;
                         
//                             // }
//                         return result;
//         } 
//     }
// }