﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Nsk.Web.Services.Data;
using SixLabors.ImageSharp;
using System.Threading.Tasks;
using SixLabors.ImageSharp.PixelFormats;

namespace Nsk.Web.Services.Controllers
{
    public class ImageController : Controller
    {
        public IDatabase Database { get; private set; }

        public ImageController(IDatabase database)
        {
            this.Database = database ?? throw new ArgumentNullException(nameof(database));
        }

        [HttpGet]
        [ResponseCache(Duration = 30)]
        public IActionResult Category(int id)
        {
            using (var image = Database.GetCategoryThumbnail(id))
            {
                var result = BuildHttpResponse(image);
                return File(result, "image/jpeg");
            }
        }

        [HttpGet]
        [ResponseCache(Duration = 30)]
        public IActionResult Product(int id)
        {
            using (var image = Database.GetProductThumbnail(id))
            {
                var result = BuildHttpResponse(image);
                return File(result, "image/jpeg");
            }
        }

        private static MemoryStream BuildHttpResponse(Image<Rgba32> image)
        {
            var memoryStream = new MemoryStream();
            image.SaveAsJpeg(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
    }
}
