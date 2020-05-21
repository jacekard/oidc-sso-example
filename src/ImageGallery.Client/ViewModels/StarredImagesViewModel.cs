using System.Collections.Generic;
using System.Linq;
using ImageGallery.Model;

namespace ImageGallery.Client.ViewModels
{
    public class StarredImagesViewModel
    {
        public IEnumerable<Image> Images { get; private set; }
            = new List<Image>();

        public StarredImagesViewModel(IEnumerable<Image> images)
        {
            Images = images;
        }
    }
}
