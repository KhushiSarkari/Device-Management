using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.VisualBasic.FileIO;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace dm_backend.Controllers
{
    [Route("api/[controller]")]
    
 
 public class BulkRegister : ControllerBase
    {
    

        [HttpPost("UploadFiles")]
public IActionResult Post(List<IFormFile> photo)
{

 List<string> uploadedFiles = new List<string>();
        foreach (IFormFile postedFile in photo)
        {
            string fileName = Path.GetFileName(postedFile.FileName);
            using (FileStream stream = new FileStream(Path.Combine(fileName), FileMode.Create))
            {
                postedFile.CopyTo(stream);
                uploadedFiles.Add(fileName);
                Console.WriteLine(fileName);


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
                 formFile.CopyToAsync(stream);
            }
        }
    }

    Console.WriteLine(filePath);
    var json1 = ReadCSVFile(filePath);
   // Console.WriteLine(json1);

    // process uploaded files
    // Don't rely on or trust the FileName property without validation.

    return Ok(new { count = photo.Count,size,filePath});
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
            Console.WriteLine(jsonString);
            var jsonDeserialize= Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);
            Console.WriteLine(jsonDeserialize);

            return jsonString;
        }

}
}