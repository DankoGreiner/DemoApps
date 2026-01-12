using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public static class InstagramStoryConverter
{
    // Instagram Story canvas size
    public const int StoryWidth = 1080;
    public const int StoryHeight = 1920;

    /// <summary>
    /// Converts any input image to an Instagram Story-friendly image (1080x1920),
    /// preserving aspect ratio and padding with a solid background color (default: black).
    /// </summary>
    public static void ConvertToStory(
        string inputPath,
        string outputPath,
        Color? backgroundColor = null,
        long jpegQuality = 92L)
    {
        if (string.IsNullOrWhiteSpace(inputPath)) throw new ArgumentException("inputPath is required.", nameof(inputPath));
        if (string.IsNullOrWhiteSpace(outputPath)) throw new ArgumentException("outputPath is required.", nameof(outputPath));

        var bg = backgroundColor ?? Color.Black;

        using (var src = Image.FromFile(inputPath))
        using (var dest = new Bitmap(StoryWidth, StoryHeight, PixelFormat.Format24bppRgb))
        using (var g = Graphics.FromImage(dest))
        {
            // Quality settings
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;

            // Fill background
            g.Clear(bg);

            // Compute scaled size that fits inside 1080x1920 (no cropping)
            var scale = Math.Min((double)StoryWidth / src.Width, (double)StoryHeight / src.Height);
            var newW = (int)Math.Round(src.Width * scale);
            var newH = (int)Math.Round(src.Height * scale);

            // Center it
            var x = (StoryWidth - newW) / 2;
            var y = (StoryHeight - newH) / 2;

            // Draw
            var destRect = new Rectangle(x, y, newW, newH);
            g.DrawImage(src, destRect, new Rectangle(0, 0, src.Width, src.Height), GraphicsUnit.Pixel);

            SaveImage(dest, outputPath, jpegQuality);
        }
    }

    private static void SaveImage(Image image, string outputPath, long jpegQuality)
    {
        if (outputPath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
            outputPath.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
        {
            var codec = GetEncoder(ImageFormat.Jpeg);
            if (codec == null)
            {
                image.Save(outputPath, ImageFormat.Jpeg);
                return;
            }

            using (var ep = new EncoderParameters(1))
            {
                jpegQuality = Clamp(jpegQuality, 1L, 100L);
                ep.Param[0] = new EncoderParameter(Encoder.Quality, jpegQuality);
                image.Save(outputPath, codec, ep);
            }
        }
        else if (outputPath.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
        {
            image.Save(outputPath, ImageFormat.Png);
        }
        else
        {
            // Default fallback: PNG
            image.Save(outputPath, ImageFormat.Png);
        }
    }

    private static ImageCodecInfo GetEncoder(ImageFormat format)
    {
        var codecs = ImageCodecInfo.GetImageDecoders();
        foreach (var c in codecs)
        {
            if (c.FormatID == format.Guid) return c;
        }
        return null;
    }

    private static long Clamp(long value, long min, long max)
    {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }
}
