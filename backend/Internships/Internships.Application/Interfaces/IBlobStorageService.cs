using Internships.Core.Wrappers;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Internships.Core.Interfaces
{
    public interface IBlobStorageService
    {
        /* public Task Upload(string containerName, string blobName, Stream content);
         public Task<BlobDownloadInfo> Download(string containerName, string blobName);
         public Task Delete(string containerName, string blobName);
         public Task Create(string containerName);
         public Task<bool> HasFile(string containerName, string blobName);*/
        public Task<Response<int>> Upload(IFormFile file,string userId,int InternshipId,int DocumentTypeId);
        public Task<List<string>> ListBlobs();
        public Task<List<string>> ListContainers();
    }
}
