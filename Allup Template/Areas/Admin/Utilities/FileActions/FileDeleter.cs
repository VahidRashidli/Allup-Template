using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Allup_Template.Areas.Admin.Utilities.FileActions
{
    public static class FileDeleter
    {
        public static void Delete(string fileName,string folder)
        {
            string path = Path.Combine(folder, fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
    }
}
