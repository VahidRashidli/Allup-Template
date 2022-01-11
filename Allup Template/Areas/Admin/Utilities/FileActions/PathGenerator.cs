using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Allup_Template.Areas.Admin.Utilities.FileActions
{
    public static class PathGenerator
    {
        public static string GeneratePath(string folder,IFormFile file,Guid guid)
        {
            return Path.Combine(folder, guid + file.FileName);
        }
    }
}
