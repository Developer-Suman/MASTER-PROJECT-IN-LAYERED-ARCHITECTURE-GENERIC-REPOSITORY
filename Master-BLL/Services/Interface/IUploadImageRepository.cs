using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Master_BLL.Services.Interface
{
    public interface IUploadImageRepository
    {
        Task<string> UploadImage(IFormFile file);
        void DeleteImage(string ImageUrl);
        void UpdateImage(IFormFile file);
    }
}
