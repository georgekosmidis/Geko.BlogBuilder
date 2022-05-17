using Blog.Builder.Services;

namespace Blog.Builder.Interfaces;

/// <summary>
/// Entry point for the website building. 
/// Method <see cref="WebsitePreparation.PrepareAsync"/> should be the first thing to call.
/// </summary>
internal interface IWebsiteProcessor : IDisposable
{
    /// <summary>
    /// Prepares everything needed for the website. 
    /// Once this method is done, the website is ready at <see cref="Globals.OutputFolderPath"/>.
    /// </summary>
    Task PrepareAsync();
}