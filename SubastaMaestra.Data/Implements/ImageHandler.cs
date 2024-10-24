using SubastaMaestra.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubastaMaestra.Data.Implements
{
    public class ImageHandler
    {
        public ImageHandler() { }
        public async Task<string> SaveImageAsync(UploadImage image)
        {
            string url = String.Empty;
            if (image != null)
            {
                var imageName = Guid.NewGuid().ToString() + ".jpg";
                url = $"Images/{imageName}";

                using (var stream = new FileStream(url, FileMode.Create))
                {
                    await image.ImageFile.CopyToAsync(stream);

                }
            }

            return url; 
        }
    }
}
