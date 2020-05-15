using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using dm_backend.Data;
using dm_backend.EFModels;
using System.Threading.Tasks;
using System.Threading;
using dm_backend.MyData;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace dm_backend.Controllers
{
    [Route("api/[controller]")]
    
 
 public class BulkRegister : Controller
    {
         private readonly IHubContext<FirstTry> _hubContext;
        private IAuthRepository _repo;

     
         
        public BulkRegister(IAuthRepository repo,IHubContext<FirstTry> hubContext)
    {
        _repo=repo;
        _hubContext=hubContext;
    }

        [HttpPost("UploadFiles")]
public async Task<IActionResult> PostAsync(List<IFormFile> photo)
{

 List<string> uploadedFiles = new List<string>();
        foreach (IFormFile postedFile in photo)
        {
            string fileName = Path.GetFileName(postedFile.FileName);
            using (FileStream stream = new FileStream(Path.Combine(fileName), FileMode.Create))
            {
                postedFile.CopyTo(stream);
                uploadedFiles.Add(fileName);
                Console.WriteLine("this is :"+fileName+"path is "+stream);

            }
        }
    
    long size = photo.Sum(f => f.Length);

    // full path to file in temp location
    var filePath = Path.GetTempFileName();
    foreach (var formFile in photo)
    {
        if (formFile.Length > 0)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
               await  formFile.CopyToAsync(stream);
            }
        }
    }

    Console.WriteLine(filePath);
    var json1 = ReadCSVFile(filePath);
    dynamic json  = JsonConvert.DeserializeObject(json1);
    List<string> AlreadyExists =new List<string>();
    for(int i=1;i<=json.Count;i++)
    {

     Thread.Sleep(500);
     int countn = Convert.ToInt32(json.Count);
     //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
     SendProgress("Process", i , countn);
 
        string UserEmail =Convert.ToString(json[i-1].email);
        string UserLastName =Convert.ToString(json[i-1].LastName);
        string UserFirstName =Convert.ToString(json[i-1].FirstName);
        string UserPassword = Convert.ToString(json[i-1].password);
        
         if(! await _repo.UserExists(UserEmail))
         { 
                     var userTocreate = new User
            {
                Email = UserEmail,
                FirstName = UserFirstName,
                LastName=  UserLastName,
            };
             var createdUser =await _repo.Register(userTocreate, UserPassword);
         }else{AlreadyExists.Add(UserEmail);}
     }
    

    return Ok(new { count = photo.Count,size,filePath,UsersAlreadyExists =AlreadyExists});
}

        private void SendProgress(string progressMessage, int progressCount, int totalItems)
        {
            var percentage = (progressCount * 100) / totalItems;
            _hubContext.Clients.All.SendAsync(progressMessage, percentage);
        }

        public  string ReadCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();
            string jsonString = string.Empty;
            try
            {
                using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
                {
                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    string[] colFields;
                    bool tableCreated = false;
                    while (tableCreated == false)
                    {
                        colFields = csvReader.ReadFields();
                        
                        foreach (string column in colFields)
                        {
                           // Console.WriteLine(column);
                            DataColumn datecolumn = new DataColumn(column);
                            datecolumn.AllowDBNull = true;
                            csvData.Columns.Add(datecolumn);
                        }
                        tableCreated = true;
                    }
                    while (!csvReader.EndOfData)
                    {
                           
                        csvData.Rows.Add(csvReader.ReadFields());
                     
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Error:Parsing CSV";
            }
                       //if everything goes well, serialize csv to json 
            jsonString = JsonConvert.SerializeObject(csvData);
            
           
            return jsonString;
        }

}
}