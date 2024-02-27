using DocsBlobStorage.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using DocsBlobStorage.Services.Abstractions;

namespace DocsBlobStorage.Pages
{
    public partial class Index
    {
        [Inject]
        public IBlobStorageService _blobStorageService { get; set; }

        private FileUploadModel fileUploadModel = new();
        private string? fileErrorMessage;
        private string? uploadMessage;

        private void OnInputFileChange(InputFileChangeEventArgs e)
        {
            var file = e.File;
            if (file != null && file.Name.EndsWith(".docx"))
            {
                fileUploadModel.File = file;
                fileErrorMessage = "";
            }
            else
            {
                fileErrorMessage = "Only .docx files are allowed.";
            }
        }

        private async Task HandleValidSubmit()
        {
            if (fileUploadModel.File != null)
            {
                try
                {
                    await _blobStorageService.UploadFileAsync(fileUploadModel.File, fileUploadModel.Email);
                    uploadMessage = "File uploaded successfully!";
                }
                catch (Exception ex)
                {
                    uploadMessage = $"Error: {ex.Message}";
                }
            }
        }
    }
}
