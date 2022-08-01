using Google.Cloud.Storage.V1;
using System.IO;
using System.Threading.Tasks;

namespace EzCommon.Infra.Storage
{
    public class GCPFileStorage : IFileStorage
    {

        private readonly StorageClient _storageClient;
        private readonly string _bucketName;

        public GCPFileStorage(StorageClient storageClient, string bucketName)
        {
            _storageClient = storageClient;
            _bucketName = bucketName;
        }

        public async Task UploadFileAsync(MemoryStream stream, string fileName)
        {
            const string contentType = "application/octet-stream";
            await _storageClient.UploadObjectAsync(_bucketName, fileName, contentType, stream);
        }
    }
}
