namespace Blog.Builder.Interfaces.Builders;

internal interface IStaticAppConfigBuilder
{
    void Add(string relativeUrl, DateTime datePublished);
    void Build();
}