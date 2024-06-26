using Internships.Core.Interfaces;
using Internships.Core.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Internships.WebApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IBlobStorageService blobStorage;
        public FileController(IBlobStorageService blobStorageService)
        {
            blobStorage = blobStorageService;
        }

        [HttpPost]
        public async Task<Response<int>> Upload(IFormFile file,int InternshipId,int DocumentTypeId)
        {
            string userId= User?.Claims.FirstOrDefault(e => e.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
            return await blobStorage.Upload(file,userId,InternshipId,DocumentTypeId);
        }

        [HttpGet("listBlobs")]
        public async Task<List<string>> ListBlobs()
        {
            return await blobStorage.ListBlobs();
        }

        [HttpGet("listContainers")]
        public async Task<List<string>> ListContainer()
        {
            return await blobStorage.ListContainers();
        }
       
        
    }
}
