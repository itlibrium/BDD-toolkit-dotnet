using ITLIBRIUM.BddToolkit.Docs.Gherkin;

namespace ITLIBRIUM.BddToolkit.Docs
{
    public static class DocPublishers
    {
        public static GherkinFilesPublisher GherkinFiles() =>
            new GherkinFilesPublisher(new FileSystem());

        public static GherkinFilesPublisher GherkinFiles(string basePath) =>
            new GherkinFilesPublisher(basePath, new FileSystem());
    }
}