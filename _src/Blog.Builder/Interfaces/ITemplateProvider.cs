namespace Blog.Builder.Interfaces;

/// <summary>
/// A service that provides the html for the templates.
/// </summary>
internal interface ITemplateProvider
{
    /// <summary>
    /// Returns the proper template HTML based on the model <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The template model to get the equivalent HTML.</typeparam>.
    /// <returns>The HTML of the proper template.</returns>
    public string GetHtml<T>();

    /// <summary>
    /// Returns the proper template HTML based on the <paramref name="nameOfType"/> that represents
    /// the type of the model (e.g. nameof(LayoutArticleModel)).
    /// </summary>
    /// <param name="nameOfType">A string that represents the type of the model (e.g. nameof(LayoutArticleModel)).</param>
    /// <returns>The HTML of the proper template.</returns>
    string GetHtml(string nameOfType);

    /// <summary>
    /// Returns the path to the proper template based on the model <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The template model to get the equivalent tempalte path.</typeparam>.
    /// <returns>The template path of the proper template.</returns>
    string GetPath<T>();

    /// <summary>
    /// Returns the path to the proper template based on the <paramref name="nameOfType"/> that represents
    /// the type of the model (e.g. nameof(LayoutArticleModel)).
    /// </summary>
    /// <param name="nameOfType">A string that represents the type of the model (e.g. nameof(LayoutArticleModel)).</param>
    /// <returns>The template path of the proper template.</returns>
    string GetPath(string nameOfType);
}