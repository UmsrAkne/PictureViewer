using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

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

        /// <summary>
        /// 引数に入力された文字列に基づき、ExFileInfo オブジェクトを取得します。
        /// </summary>
        /// <param name="imageFilePath">生成するファイルパス</param>
        /// <returns>
        /// 引数に渡された imageFilePath がデータベースに登録されているかを確認。<br/>
        /// 登録済みならばデータベースから値を取り出し、未登録ならば新規生成したインスタンスをデータベースに登録したあとに返します。<br/>
        /// また、未登録の場合はサムネイルの生成を同時に行います。
        /// </returns>
        public async Task<ExFileInfo> GetOrCreateExFileAsync(string imageFilePath)
        {
            var exFileInfo = new ExFileInfo(new FileInfo(imageFilePath));
            var all = await imageFileRepository.GetAllAsync();
            var value = all.FirstOrDefault(f => f.FullPath == exFileInfo.FileSystemInfo.FullName);

            value?.SetFileSystemInfo(new FileInfo(imageFilePath));

            if (value != null && value.Width != 0)
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(value.GetThumbnailFileInfo().FullName, UriKind.Absolute);
                bitmap.EndInit();
                value.Thumbnail = bitmap;
                return value;
            }

            if (!exFileInfo.IsImageFile)
            {
                return exFileInfo;
            }

            exFileInfo.Thumbnail = ExFileInfo.GenerateThumbnail(exFileInfo.FileSystemInfo.FullName, 80);

            var thumbnailInfo = exFileInfo.GetThumbnailFileInfo();

            if (thumbnailInfo.Directory is { Exists: false, })
            {
                thumbnailInfo.Directory.Create();
            }

            ExFileInfo.SaveBitmapSourceToFile(exFileInfo.Thumbnail, thumbnailInfo.FullName);
            exFileInfo.ThumbnailGenerated = true;

            await imageFileRepository.AddAsync(exFileInfo);
            return exFileInfo;
        }
    }
}