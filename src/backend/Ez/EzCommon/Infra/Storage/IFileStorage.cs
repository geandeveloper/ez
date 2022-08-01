using System.IO;
using System.Threading.Tasks;

namespace EzCommon.Infra.Storage
{
    public interface IFileStorage
    {
        public Task<string> UploadFileAsync(MemoryStream stream, string fileName, string extension);
    }
}
