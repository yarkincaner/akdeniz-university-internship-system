using Azure.Storage.Blobs;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using System.IO;
using Internships.Core.Interfaces;
using Microsoft.Extensions.Options;
using Internships.Core.Settings;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Internships.Core.Interfaces.Repositories;
using Internships.Core.Exceptions;
using Internships.Core.Wrappers;

namespace Internships.Infrastructure.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _blobStorageConnectionString;
        private readonly BlobStorageSettings _blobStorageSettings;
        private readonly IInternshipRepositoryAsync _internshipRepositoryAsync;
        private readonly IDocumentRepositoryAsync _documentRepository;
        public BlobStorageService(IOptions<BlobStorageSettings> blobStorageSettings,IInternshipRepositoryAsync internshipRepositoryAsync, IDocumentRepositoryAsync documentRepository)
        {
            _blobStorageConnectionString = blobStorageSettings.Value.ConnectionString;
            _blobServiceClient = new BlobServiceClient(_blobStorageConnectionString);
            _blobStorageSettings = blobStorageSettings.Value;
            _internshipRepositoryAsync = internshipRepositoryAsync;
            _documentRepository = documentRepository;
        }
        public async Task<Response<int>> Upload(IFormFile file,string userId,int InternshipId,int DocumentTypeId)
        {
            var internship = await _internshipRepositoryAsync.GetByIdAsync(InternshipId);
            if (internship == null)
            {
                throw new EntityNotFoundException("Internship", InternshipId);
            }

            var blobContainer = _blobServiceClient.GetBlobContainerClient(_blobStorageSettings.ContainerName);
            string originalBlobName = userId + "/" + file.FileName;
            var blob = blobContainer.GetBlobClient(originalBlobName);
            int number = 1;
            while (await blob.ExistsAsync())
            {

                string fileExtension = Path.GetExtension(file.FileName);
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
                string modifiedFileName = fileNameWithoutExtension + $"({number})" + fileExtension;
                string modifiedBlobName = userId + "/" + modifiedFileName;
                blob = blobContainer.GetBlobClient(modifiedBlobName);
                number++;
            }

            var stream = file.OpenReadStream();
            await blob.UploadAsync(stream);

            var document = new Core.Entities.Document
            {
                InternshipId = internship.Id,
                DocumentURL = blob.Name,
                DocumentTypeId = DocumentTypeId
            };
            internship.Documents.Add(document);
            await _documentRepository.AddAsync(document);

            return new Response<int>(document.Id);



        }
        public async Task<List<string>> ListBlobs()
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_blobStorageSettings.ContainerName);
            var blobNames = new List<string>();

            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                blobNames.Add(blobItem.Name);
            }

            return blobNames;
        }
        public async Task<List<string>> ListContainers()
        {
            var containerNames = new List<string>();

            await foreach (var containerItem in _blobServiceClient.GetBlobContainersAsync())
            {
                containerNames.Add(containerItem.Name);
            }

            return containerNames;
        }
        

        /* public async Task Upload(string containerName, string blobName, Stream content)
         {
             var containerInstance = _blobServiceClient.GetBlobContainerClient(containerName);
             await containerInstance.UploadBlobAsync(blobName, content);
         }


         public async Task<BlobDownloadInfo> Download(string containerName, string blobName)
         {
             var containerInstance = _blobServiceClient.GetBlobContainerClient(containerName);
             var blobClient = containerInstance.GetBlobClient(blobName);

             BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();

             return blobDownloadInfo;
         }
         public async Task Delete(string containerName, string blobName)
         {
             var containerInstance = _blobServiceClient.GetBlobContainerClient(containerName);
             var blobClient = containerInstance.GetBlobClient(blobName);

             await blobClient.DeleteIfExistsAsync();
         }

         public async Task Create(string containerName)
         {
             await _blobServiceClient.CreateBlobContainerAsync(containerName);
         }

         public async Task<bool> HasFile(string containerName, string blobName)
         {
             var containerInstance = _blobServiceClient.GetBlobContainerClient(containerName);
             var blobClient = containerInstance.GetBlobClient(blobName);

             return await blobClient.ExistsAsync();
         }*/

    }
}


