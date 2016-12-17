using System.Collections.Generic;

namespace TagsCloudApp.Client
{
    public interface ITagsCloudAppUi
    {
        void Run();
        IEnumerable<string> GetSourceText(string filename);
        void SaveCloud();
    }
}