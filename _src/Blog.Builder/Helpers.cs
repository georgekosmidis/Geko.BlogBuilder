using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

/// <summary>
/// Helper methods that don't deserve a class
/// </summary>
internal static class Helpers
{
    /// <summary>
    /// Copies everything from <paramref name="sourceDir"/> to <paramref name="targetDir"/>.
    /// Overwrites are not allowed.
    /// </summary>
    /// <param name="sourceDir">The directory to copy everything from.</param>
    /// <param name="targetDir">The directory to copy everything to.</param>
    public static void Copy(string sourceDir, string targetDir)
    {
        Directory.CreateDirectory(targetDir);

        foreach (var file in Directory.GetFiles(sourceDir))
        {
            File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)), false);
        }

        foreach (var directory in Directory.GetDirectories(sourceDir))
        {
            Copy(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
        }
    }

    /// <summary>
    /// Resize the image from <paramref name="inputPath"/> to the <paramref name="size"/> and saves it to <paramref name="outputPath"/>.
    /// It will reduce the size of the image until at least one side meets the criteria.
    /// If one side is already smalled, it does nothing.
    /// </summary>
    /// <param name="inputPath">The image to be processed.</param>
    /// <param name="outputPath">The destination of the processed image.</param>
    /// <param name="size">The maximum allowed size of the image.</param>
    public static void ResizeImage(string inputPath, string outputPath, Size size)
    {
        using var image = Image.Load(inputPath);
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = size
        }));
        image.Save(outputPath);
    }
}
