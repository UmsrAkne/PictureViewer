using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Mvvm;

namespace PictureViewer.Models
{
    public class FilteredListProvider : BindableBase
    {
        private ObservableCollection<ExFileInfo> files = new ();

        public ObservableCollection<ExFileInfo> Files { get => files; set => SetProperty(ref files, value); }

        private List<ExFileInfo> OriginalFiles { get; set; } = new ();

        public void AddRange(IEnumerable<ExFileInfo> fs)
        {
            var fss = fs.ToList();
            OriginalFiles.AddRange(fss);
            Files.AddRange(fss);
            Sort();
        }

        public void Add(ExFileInfo f)
        {
            OriginalFiles.Add(f);
            Files.Add(f);
            Sort();
        }

        public void Replace(IEnumerable<ExFileInfo> fs)
        {
            OriginalFiles = fs.ToList();
            Files = new ObservableCollection<ExFileInfo>(OriginalFiles);
            Sort();
        }

        private void Sort()
        {
            Files = new ObservableCollection<ExFileInfo>(Files.OrderBy(f => f.FileSystemInfo.Name));
        }
    }
}