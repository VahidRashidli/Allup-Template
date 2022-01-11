using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Allup_Template.Areas.Admin.Utilities.FileActions
{
    public static class FileCreator
    {
        public static async Task CreateFileAsync(this IFormFile file,string folder,Guid guid)
        {
            string path = PathGenerator.GeneratePath(folder, file, guid);
             FileStream stream = new(path, FileMode.Create);
            await file.CopyToAsync(stream);
            stream.Close();
        }
    }
}
