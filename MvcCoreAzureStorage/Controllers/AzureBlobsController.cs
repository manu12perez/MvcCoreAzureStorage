using Microsoft.AspNetCore.Mvc;
using MvcCoreAzureStorage.Models;
using MvcCoreAzureStorage.Services;

namespace MvcCoreAzureStorage.Controllers
{
    public class AzureBlobsController : Controller
    {
        private ServiceStorageBlobs service;

        public AzureBlobsController(ServiceStorageBlobs service)
        {
            this.service = service;
        }

        public async Task<IActionResult> Index()
        {
            List<string> containers = await this.service.GetContainersAsync();
            return View(containers);
        }

        public IActionResult CreateContainer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateContainer(string containername)
        {
            await this.service.CreateContainerAsync(containername);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteContainer(string containername)
        {
            await this.service.DeleteContainerAsync(containername);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ListBlobs(string containername)
        {
            List<BlobModel> blobs = await this.service.GetBlobsAsync(containername);
            return View(blobs);
        }

        public IActionResult UploadBlob(string containername)
        {
            ViewData["CONTAINER"] = containername;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadBlob(string containername, IFormFile file)
        {
            string blobName = file.FileName;
            using (Stream stream = file.OpenReadStream())
            {
                await this.service.UploadBlobAsync(containername, blobName, stream);
            }
            return RedirectToAction("ListBlobs", new { containername = containername });
        }

        public async Task<IActionResult> DeleteBlob(string containername, string blobname)
        {
            await this.service.DeleteBlobAsync(containername, blobname);
            return RedirectToAction("ListBlobs", new { containername = containername });
        }

        //public async Task<IActionResult> DownloadBlob(string containername, string blobname)
        //{
        //    Stream stream = await this.service.DownloadBlobAsync(containername, blobname);
        //    return File(stream, "application/octet-stream", blobname);
        //}

        //public async Task<IActionResult> GetBlob(string containername, string blobname)
        //{

        //    Stream blobStream = await service.DownloadBlobAsync(containername, blobname);

        //    string contentType = GetContentType(blobname);

        //    return File(blobStream, contentType, blobname);
        //}

        //private string GetContentType(string blobName)
        //{
        //    var extension = Path.GetExtension(blobName).ToLower();
        //    switch (extension)
        //    {
        //        case ".jpg":
        //        case ".jpeg":
        //            return "image/jpeg";
        //        case ".png":
        //            return "image/png";
        //        case ".gif":
        //            return "image/gif";
        //        case ".pdf":
        //            return "application/pdf";
        //        case ".txt":
        //            return "text/plain";
        //        case ".zip":
        //            return "application/zip";
        //        default:
        //            return "application/octet-stream";
        //    }
        //}
    }
}

