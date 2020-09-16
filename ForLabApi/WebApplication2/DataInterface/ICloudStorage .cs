using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForLabApi.DataInterface
{
    public interface ICloudStorage
    {
        Task<string>  UploadFileAsync(IFormFile imageFile, string fileNameForStorage);
        Task DeleteFileAsync (string fileNameForStorage);
    }
}
