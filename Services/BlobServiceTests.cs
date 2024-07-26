using Azure.Storage;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Moq;
using ProductoImagenes.Services;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProductoImagenes.Tests.Services
{
    public class BlobServiceTests
    {
        private readonly Mock<BlobServiceClient> _mockBlobServiceClient;
        private readonly Mock<BlobContainerClient> _mockBlobContainerClient;
        private readonly Mock<BlobClient> _mockBlobClient;
        private readonly BlobService _blobService;

        public BlobServiceTests()
        {
            _mockBlobServiceClient = new Mock<BlobServiceClient>();
            _mockBlobContainerClient = new Mock<BlobContainerClient>();
            _mockBlobClient = new Mock<BlobClient>();

            _mockBlobServiceClient
                .Setup(x => x.GetBlobContainerClient(It.IsAny<string>()))
                .Returns(_mockBlobContainerClient.Object);

            _mockBlobContainerClient
                .Setup(x => x.GetBlobClient(It.IsAny<string>()))
                .Returns(_mockBlobClient.Object);

            _blobService = new BlobService("UseDevelopmentStorage=true", "test-container");

            // Use reflection to replace the BlobServiceClient
            var blobServiceClientField = typeof(BlobService).GetField("_blobServiceClient", BindingFlags.NonPublic | BindingFlags.Instance);
            blobServiceClientField?.SetValue(_blobService, _mockBlobServiceClient.Object);
        }

        [Fact]
        public async Task UploadFileAsync_ShouldReturnBlobUrl()
        {
            // Arrange
            string fileName = "testfile.txt";
            string contentType = "text/plain";
            var fileStream = new MemoryStream();

            _mockBlobClient.Setup(x => x.Uri).Returns(new Uri($"https://fakestorage.blob.core.windows.net/test-container/{fileName}"));
            _mockBlobContainerClient
                .Setup(x => x.CreateIfNotExistsAsync(It.IsAny<PublicAccessType>(), null, null, default))
                .ReturnsAsync(Mock.Of<Response<BlobContainerInfo>>());
            _mockBlobClient
                .Setup(x => x.UploadAsync(It.IsAny<Stream>(), It.IsAny<BlobHttpHeaders>(), It.IsAny<IDictionary<string, string>>(), It.IsAny<BlobRequestConditions>(), It.IsAny<IProgress<long>>(), It.IsAny<AccessTier?>(), It.IsAny<StorageTransferOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Mock.Of<Response<BlobContentInfo>>());

            // Act
            var result = await _blobService.UploadFileAsync(fileName, fileStream, contentType);

            // Assert
            Assert.Equal($"https://fakestorage.blob.core.windows.net/test-container/{fileName}", result);
        }

        [Fact]
        public async Task DeleteFileAsync_ShouldReturnTrue_WhenFileExists()
        {
            // Arrange
            string fileName = "testfile.txt";
            _mockBlobClient
                .Setup(x => x.DeleteIfExistsAsync(It.IsAny<DeleteSnapshotsOption>(), It.IsAny<BlobRequestConditions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Response.FromValue(true, Mock.Of<Response>()));

            // Act
            var result = await _blobService.DeleteFileAsync(fileName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task FileExistsAsync_ShouldReturnTrue_WhenFileExists()
        {
            // Arrange
            string fileName = "testfile.txt";
            _mockBlobClient
                .Setup(x => x.ExistsAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Response.FromValue(true, Mock.Of<Response>()));

            // Act
            var result = await _blobService.FileExistsAsync(fileName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void GetBlobContainerClient_ShouldReturnBlobContainerClient()
        {
            // Act
            var result = _blobService.GetBlobContainerClient();

            // Assert
            Assert.Equal(_mockBlobContainerClient.Object, result);
        }
    }
}