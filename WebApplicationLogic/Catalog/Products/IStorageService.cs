using System.IO;
using System.Threading.Tasks;

namespace WebApplicationLogic.Catalog.Products
{
    public interface IStorageService
    {
        string GetFileUrl(string fileName);

        Task SaveFileAsync(Stream mediaBinaryStream, string fileName);

        Task SaveFileUserAsync(Stream mediaBinaryStream, string fileName);

        Task DeleteFileAsync(string fileName);
    }
}