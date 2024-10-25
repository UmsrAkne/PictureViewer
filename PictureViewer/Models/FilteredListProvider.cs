using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Mvvm;

namespace PictureViewer.Models
{
    public class FilteredListProvider : BindableBase
    {
        private readonly List<string> searchExtensions = new () { ".png", ".jpg", ".jpeg", ".bmp", ".gif", "webp", };
        private ObservableCollection<ExFileInfo> files = new ();

        public ObservableCollection<ExFileInfo> Files { get => files; set => SetProperty(ref files, value); }

        private List<ExFileInfo> OriginalFiles { get; set; } = new ();

        public void AddRange(IEnumerable<ExFileInfo> fs)
        {
            var fss = fs.ToList();
            OriginalFiles.AddRange(fss);
            Files.AddRange(fss);
        }

        public void Add(ExFileInfo f)
        {
            OriginalFiles.Add(f);
            if (searchExtensions.Contains(f.FileSystemInfo.Extension.ToLower()) || f.IsDirectory)
            {
                Files.Add(f);
            }
        }

        public void Replace(IEnumerable<ExFileInfo> fs)
        {
            OriginalFiles = fs.ToList();
            Files = new ObservableCollection<ExFileInfo>(Sort(Filter(OriginalFiles)));
        }

        private IEnumerable<ExFileInfo> Sort(IEnumerable<ExFileInfo> fs)
        {
            return fs.OrderBy(f => f.FileSystemInfo.Name);
        }

        private IEnumerable<ExFileInfo> Filter(IEnumerable<ExFileInfo> fs)
        {
            return fs.Where(f => searchExtensions.Contains(f.FileSystemInfo.Extension.ToLower()) || f.IsDirectory);
        }
    }
}