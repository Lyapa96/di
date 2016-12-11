namespace TagsCloudApp
{
    public class WordInformation
    {
        public readonly string Content;
        public readonly int Frequency;

        public WordInformation(string content, int frequency)
        {
            Content = content;
            Frequency = frequency;
        }
    }
}