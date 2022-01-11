using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Allup_Template.Areas.Admin.Utilities.FileActions
{
    public static class FileContentChecker
    {
        public static bool CheckFileContent(this IFormFile file)
        {
            if (!file.ContentType.Contains("image"))
            {
                return false;
            }
            return true;
        }
    }
}
