using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Allup_Template.Areas.Admin.Utilities.FileActions
{
    public static class FileSizeChecker
    {
        public static bool CheckFileSize(this IFormFile file)
        {
            if (file.Length>1024*1000)
            {
                return false;
            }
            return true;
        }
    }
}
