using System.Collections.Generic;
using System.Threading.Tasks;

namespace PictureViewer.Models.Dbs
{
    public class ImageFileService
    {
        private readonly IRepository<ExFileInfo> imageFileRepository;

        public ImageFileService(IRepository<ExFileInfo> imageFileRepository)
        {
            this.imageFileRepository = imageFileRepository;
        }

        public Task<IEnumerable<ExFileInfo>> GetImageFilesAsync()
        {
            return imageFileRepository.GetAllAsync();
        }

        public Task AddImageFileAsync(ExFileInfo imageFile)
        {
            return imageFileRepository.AddAsync(imageFile);
        }
    }
}