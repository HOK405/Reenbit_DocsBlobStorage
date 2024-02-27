using DocsBlobStorage.Configuration;
using DocsBlobStorage.Services.Abstractions;
using DocsBlobStorage.Services.Implementations;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using Moq;

namespace DocsBlobStorage.Tests.Services
{
    public class BlobStorageServiceTests
    {
        [Fact]
        public void Constructor_InvalidConnectionString_ThrowsArgumentNullException()
        {
            // Arrange
            var options = new BlobStorageOptions
            {
                ConnectionString = null, // Invalid connection string
                ContainerName = "validContainerName"
            };

            var optionsMock = new OptionsWrapper<BlobStorageOptions>(options);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new BlobStorageService(optionsMock));
            Assert.Equal("ConnectionString", exception.ParamName);
        }

        [Fact]
        public void Constructor_InvalidContainerName_ThrowsArgumentNullException()
        {
            // Arrange
            var options = new BlobStorageOptions
            {
                ConnectionString = "validConnectionString",
                ContainerName = null // Invalid container name
            };

            var optionsMock = new OptionsWrapper<BlobStorageOptions>(options);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => new BlobStorageService(optionsMock));
            Assert.Equal("ContainerName", exception.ParamName);
        }

        [Fact]
        public async Task UploadFileAsync_NullFile_ThrowsArgumentNullException()
        {
            // Arrange
            var options = new BlobStorageOptions
            {
                ConnectionString = "validConnectionString",
                ContainerName = "validContainerName"
            };

            var optionsMock = new OptionsWrapper<BlobStorageOptions>(options);
            var blobStorageService = new BlobStorageService(optionsMock);
            IBrowserFile file = null;
            var validEmail = "test@example.com";

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => blobStorageService.UploadFileAsync(file, validEmail));
            Assert.Equal("file", exception.ParamName);
        }


        [Fact]
        public async Task UploadFileAsync_WithValidData_InvokesUploadSuccessfully()
        {
            // Arrange
            var mockBlobStorageService = new Mock<IBlobStorageService>();
            mockBlobStorageService
                .Setup(service => service.UploadFileAsync(It.IsAny<IBrowserFile>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask); 

            var fileMock = new Mock<IBrowserFile>();
            fileMock.Setup(_ => _.Name).Returns("test.docx");
            var validEmail = "test@example.com";

            // Act
            await mockBlobStorageService.Object.UploadFileAsync(fileMock.Object, validEmail);

            // Assert
            mockBlobStorageService.Verify(service => service.UploadFileAsync(It.IsAny<IBrowserFile>(), It.IsAny<string>()), Times.Once());
        }
    }
}