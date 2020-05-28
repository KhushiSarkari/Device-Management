using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dm_backend.Controllers;
using dm_backend.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using dm_backend.EFModels;
using Microsoft.AspNetCore.Mvc;
using dm_backend.Utilities;

namespace dm_backend.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly EFDbContext _context;
        private IAuthRepository _Irepo;
        public UserRepository(EFDbContext context,IAuthRepository repo)
        {
            _context = context;
            _Irepo=repo;
        }

        public Models.User GetOneUser(int user_id)
        {
            var result = GetUserQuery().Where(User=>User.UserId==user_id).First();
             return result;  
        }
        public  IQueryable<Models.User> GetUserQuery(){
              
                  var result =  (from u in _context.User
                          
                          join rtl in _context.UserToRole on u.UserId equals rtl.UserId
                          
                          join rl in _context.Role on rtl.RoleId equals rl.RoleId 
                        
                          join s in _context.Status on u.Status equals s.StatusId
                       
                          join ddi in _context.DepartmentDesignation on u.DepartmentDesignationId equals ddi.DepartmentDesignationId into ddic
                          from ert in ddic.DefaultIfEmpty()
                          join dep in _context.Department on ert.DepartmentId equals dep.DepartmentId into yop
                          from ert1 in yop.DefaultIfEmpty()
                          join desg in _context.Designation on ert.DesignationId equals desg.DesignationId into abc
                          from sl1 in abc.DefaultIfEmpty() 
                          join sl in _context.Salutation on u.SalutationId equals sl.SalutationId into bcd
                          from sl2 in bcd.DefaultIfEmpty() 
                          join gen in _context.Gender on u.GenderId equals gen.GenderId into efg
                          from hijk in efg.DefaultIfEmpty()                           
                        
                          select new Models.User {UserId = u.UserId, Salutation= GetSafeStrings(sl2.Salutation1), FirstName = u.FirstName, MiddleName = GetSafeStrings(u.MiddleName),
                           LastName = u.LastName,DOB =GetSafeStrings(u.DateOfBirth.ToString("yyyy/MM/dd")),DOJ =GetSafeStrings(u.DateOfJoining.ToString("yyyy/MM/dd")), 
                           Email = u.Email, RoleName = rl.RoleName, Status = s.StatusName,DepartmentName = ert1.DepartmentName,DesignationName = sl1.DesignationName,Gender = hijk.Gender1,
                    
                           phones = (
                                 from ph1 in _context.ContactNumber      
                                 join ct1 in _context.ContactType on ph1.ContactTypeId equals ct1.ContactTypeId
                                 join cc1 in _context.Country on ph1.CountryId equals cc1.CountryId
                                 where ph1.UserId==u.UserId 
                               select new ContactNumberModel{Number = ph1.Number,ContactNumberType = ct1.ContactType1,AreaCode = ph1.AreaCode,CountryCode = cc1.CountryCode.ToString()}//cc.ToString()
                           ).ToList(),
                           addresses = (
                               from add1 in _context.Address where add1.UserId==u.UserId
                               join adt in _context.AddressType on add1.AddressTypeId equals adt.AddressTypeId
                               join citi in _context.City on add1.CityId equals citi.CityId
                               join stat in _context.State on citi.StateId equals stat.StateId
                               join countr in _context.Country on stat.CountryId equals countr.CountryId
                               select new AddressModel{AddressType = adt.AddressType1,AddressLine1 = add1.AddressLine1,
                               AddressLine2 = add1.AddressLine2,PIN = add1.Pin,City = citi.CityName,State = stat.StateName,Country = countr.CountryName}
                           ).ToList()
                           });
                      
                      return result;
        } 

        public static string GetSafeStrings(string colName)
        {
            return colName != null? colName.ToString() : "";
        }
        public List<Models.User> GetAllUsers(string tosearch)
        {
           var result = GetUserQuery();
        
             if(!String.IsNullOrEmpty(tosearch)){
                 result = result.Where(r=>r.FirstName.Contains(tosearch));
                 }

          return result.ToList();
        }

        public void PostUser(Models.User item){
            var DepartmentId = _context.Department.FirstOrDefault(e=>e.DepartmentName==item.DepartmentName).DepartmentId;
            var DesignatioId = _context.Designation.FirstOrDefault(e=>e.DesignationName==item.DesignationName).DesignationId;
            var  AddUser = new EFModels.User();
            AddUser.FirstName = item.FirstName;
            AddUser.MiddleName = item.MiddleName;
            AddUser.LastName = item.LastName;
            AddUser.SalutationId = _context.Salutation.FirstOrDefault(e=>e.Salutation1==item.Salutation).SalutationId;
            AddUser.Email = item.Email;
            AddUser.GenderId = _context.Gender.FirstOrDefault(e=>e.Gender1==item.Gender).GenderId;
            AddUser.Status = 1;
            AddUser.DateOfBirth = Convert.ToDateTime(item.DOB);
            AddUser.DateOfJoining = Convert.ToDateTime(item.DOJ);
            AddUser.DepartmentDesignationId = _context.DepartmentDesignation.FirstOrDefault(e=>(e.DepartmentId==DepartmentId && e.DesignationId==DesignatioId)).DepartmentDesignationId;
            byte[] hp,sp;
             _Irepo.CreatePasswordHash(item.Password,out hp,out sp);
            AddUser.Hashpassword = hp;
            AddUser.Saltpassword = sp;
            _context.User.Add(AddUser);
            _context.SaveChanges();

             var _Id=_context.User.FirstOrDefault(e=>e.Email==AddUser.Email).UserId;
             
             var Obj = new UserToRole{UserId=_Id,
             RoleId=_context.Role.FirstOrDefault(e=>e.RoleName==item.RoleName).RoleId};
             _context.UserToRole.Add(Obj);
             _context.SaveChanges();


             item.phones.ForEach(i=>{
               var phObj=new ContactNumber();
                 phObj.ContactTypeId=_context.ContactType.FirstOrDefault(e=>e.ContactType1==i.ContactNumberType).ContactTypeId;
                 phObj.AreaCode=i.AreaCode;
                 phObj.Number=i.Number;
                 phObj.CountryId=_context.Country.FirstOrDefault(e=>e.CountryCode==Int16.Parse(i.CountryCode)).CountryId;//cc.ToString()
                 phObj.UserId=_Id;
                   _context.ContactNumber.Add(phObj);
                                 _context.SaveChanges();
                                 });

          item.addresses.ForEach(i=>{
               var addObj=new Address();

                addObj.AddressLine1=i.AddressLine1;
                addObj.AddressLine2=i.AddressLine2;
                addObj.AddressTypeId=_context.AddressType.FirstOrDefault(e=>e.AddressType1==i.AddressType).AddressTypeId;
                addObj.Pin=i.PIN;
                addObj.CityId=_context.City.FirstOrDefault(e=>e.CityName==i.City).CityId;
                 addObj.UserId=_Id;
                 _context.Address.Add(addObj);
                 _context.SaveChanges();
                  });

        }

        public void UpdateUser(int user_id, [FromBody]Models.User body){
              var result=_context.User.SingleOrDefault(e=>e.UserId==user_id); 
     
        
         result.SalutationId = _context.Salutation.FirstOrDefault(e=>e.Salutation1==body.Salutation).SalutationId;
         result.FirstName=body.FirstName;
         result.MiddleName=GetSafeStrings(body.MiddleName);
         result.LastName=body.LastName;
         result.DateOfBirth=Convert.ToDateTime(body.DOB);
         result.DateOfJoining=Convert.ToDateTime(body.DOJ);
        // result.Email=body.Email;
         var DepartmentId=_context.Department.FirstOrDefault(e=>e.DepartmentName==body.DepartmentName).DepartmentId;
         var DesignatioId=_context.Designation.FirstOrDefault(e=>e.DesignationName==body.DesignationName).DesignationId;
       if(body.Gender.Length>0)result.GenderId = _context.Gender.FirstOrDefault(e=>e.Gender1==body.Gender).GenderId;
         result.DepartmentDesignationId = _context.DepartmentDesignation.FirstOrDefault(e=>(e.DepartmentId==DepartmentId && e.DesignationId==DesignatioId)).DepartmentDesignationId;
         if(body.Password.Length>0)
         {
         byte[] hp,sp;
             _Irepo.CreatePasswordHash(body.Password,out hp,out sp);
            result.Hashpassword = hp;
            result.Saltpassword = sp;
        }
        
         
         
         var _roleId=_context.Role.SingleOrDefault(e=>e.RoleName==body.RoleName).RoleId;
       
         var _userToRole=_context.UserToRole.SingleOrDefault(e=>e.UserId==user_id);
      
         _userToRole.RoleId=_roleId;

         _context.SaveChanges();
      
        body.phones.ForEach(i=>{
          if(i.Number.Length>0){
              var id=_context.ContactType.FirstOrDefault(e=>e.ContactType1==i.ContactNumberType).ContactTypeId;
              var phNum=_context.ContactNumber.FirstOrDefault(e=>e.UserId==user_id&&e.ContactTypeId==id); 
              var conId=_context.Country.FirstOrDefault(e=>e.CountryCode== Int16.Parse(i.CountryCode)).CountryId;
            Console.WriteLine(i);
            Console.WriteLine(id); 
             Console.WriteLine(phNum);
              Console.WriteLine(conId);
              if(phNum==null){
                  var phObj=new ContactNumber();
                 phObj.ContactTypeId=id;
                 phObj.AreaCode=i.AreaCode;
                 phObj.Number=i.Number;
                 phObj.CountryId=conId;
                 phObj.UserId=user_id;
                   _context.ContactNumber.Add(phObj);
             
              }
    
              else {  phNum.ContactTypeId=id;
              phNum.Number=i.Number;
              phNum.AreaCode=i.AreaCode;
              phNum.CountryId=conId;}

              _context.SaveChanges();
         } });
             
          body.addresses.ForEach(i=>{
           var id=_context.AddressType.FirstOrDefault(e=>e.AddressType1==i.AddressType).AddressTypeId;
            var add=_context.Address.FirstOrDefault(e=>e.UserId==user_id&&e.AddressTypeId==id);
              if(add==null){
                 
                  var addObj=new Address();

                addObj.AddressLine1=i.AddressLine1;
                addObj.AddressLine2=i.AddressLine2;
                addObj.AddressTypeId=id;
                addObj.Pin=i.PIN;
                addObj.CityId=_context.City.FirstOrDefault(e=>e.CityName==i.City).CityId;
                 addObj.UserId=user_id;
                 _context.Address.Add(addObj);
              }
          
          else{
                add.AddressLine1=i.AddressLine1;
                add.AddressLine2=i.AddressLine2;
                add.AddressTypeId=id;
                add.Pin=i.PIN;
                add.CityId=_context.City.SingleOrDefault(e=>e.CityName==i.City).CityId;}

                 _context.SaveChanges();
                  });
        }

 public void DeleteUser(int user_id){
      var HasDevice=_context.AssignDevice.Any(e=>e.UserId==user_id);
              if(!HasDevice)
                {
                    _context.UserToRole.RemoveRange(_context.UserToRole.Where(e=> e.UserId == user_id));
                    _context.ContactNumber.RemoveRange(_context.ContactNumber.Where(e=> e.UserId == user_id));
                    _context.Address.RemoveRange(_context.Address.Where(e=> e.UserId == user_id));
                    _context.SaveChanges();
                 
                    _context.User.RemoveRange(_context.User.Where(e=> e.UserId == user_id));
                   
                      
                     _context.SaveChanges();
                }


                
            }
       public List<EFModels.User> SortUserbyName(string sortby, string direction, string searchby = "")
        {
      
            
            var result=GetUserQuery();

            var list = new List<EFModels.User>(); 
            return list;
        }


    }

}