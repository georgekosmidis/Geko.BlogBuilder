using Blog.Builder.Exceptions;
using Blog.Builder.Interfaces.RazorEngineServices;
using Blog.Builder.Models.Templates;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using System.Text;

namespace Blog.Builder.Services.RazorEngineServices;

/// <inheritdoc/>
internal class RazorEngineWrapperService : IRazorEngineWrapperService, IDisposable
{
    private readonly IRazorEngineService razorEngineService;

    /// <summary>
    /// Creates an instance of a <see cref="RazorEngineService"/> and keeps in a private field.
    /// </summary>
    /// <param name="templateManager">The <see cref="ITemplateManager"/> that will be used with this instance of <see cref="RazorEngineService"/></param>
    public RazorEngineWrapperService(ITemplateManager templateManager)
    {
        ArgumentNullException.ThrowIfNull(templateManager);

        var config = new TemplateServiceConfiguration
        {
            EncodedStringFactory = new RawStringFactory(),
            TemplateManager = templateManager
        };
#if DEBUG
        config.Debug = true;
#endif
        razorEngineService = RazorEngine.Templating.RazorEngineService.Create(config);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        razorEngineService.Dispose();
        //If the dev decies to call dispose, destructor is not necessary 
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// It is super important to release unmanaged resources from RazorEngine, 
    ///  so we can't just wait from the devs not to forget the dispose.
    /// The undeterministic way below, is better than nothing!
    /// </summary>
    ~RazorEngineWrapperService()
    {
        Dispose();
    }

    /// <inheritdoc/>
    public string RunCompile<T>(T data) where T : ModelBase
    {
        ExceptionHelpers.ThrowIfNull(data);

        var key = new RazorEngine.Templating.NameOnlyTemplateKey(
                typeof(T).Name,
                RazorEngine.Templating.ResolveType.Global,
                null);

        var sb = new StringBuilder();
        var sw = new StringWriter(sb);
        razorEngineService.RunCompile(key, sw, typeof(T), data);

        return sb.ToString();
    }
}
