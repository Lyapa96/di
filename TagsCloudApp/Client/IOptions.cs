namespace TagsCloudApp.Client
{
    public interface IOptions
    {
        int ImageWidth { get; set; }
        int ImageHeight { get; set; }
        int[] CenterPoints { get; set; }
        string ImageFormat { get; set; }
        string Filename { get; set; }
        string ImageOutputFile { get; set; }
        string Fontname { get; set; }
        string BackgroundColor { get; set; }
        string[] TextColors { get; set; }
        string NameDeterminatorOfWordSize { get; set; }
        string[] FiltersNames { get; set; }
        string AlgorithmName { get; set; }
    }
}