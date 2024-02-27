using Microsoft.AspNetCore.Components.Forms;

namespace DocsBlobStorage.Services.Abstractions
{
    public interface IBlobStorageService
    {
        Task UploadFileAsync(IBrowserFile file, string email);
    }
}
