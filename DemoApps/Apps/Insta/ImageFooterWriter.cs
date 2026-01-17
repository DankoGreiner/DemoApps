using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

public static class ImageFooterWriter
{
    public static void AddFooterTextInPlace_Multiline(string inputPath, string footerText)
    {
        if (string.IsNullOrWhiteSpace(inputPath))
            throw new ArgumentException("inputPath is required.", nameof(inputPath));

        if (!File.Exists(inputPath))
            throw new FileNotFoundException("Input image not found.", inputPath);

        if (footerText == null)
            footerText = string.Empty;

        byte[] bytes = File.ReadAllBytes(inputPath);

        using (var ms = new MemoryStream(bytes))
        using (var original = (Bitmap)Image.FromStream(ms))
        using (var bmp = new Bitmap(original.Width, original.Height, PixelFormat.Format32bppArgb))
        using (var g = Graphics.FromImage(bmp))
        {
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            g.DrawImage(original, 0, 0);

            int paddingX = Math.Max(12, original.Width / 50);
            int paddingY = Math.Max(10, original.Height / 80);
            float fontSize = Math.Max(12f, original.Height / 25f);

            using (var font = new Font("Segoe UI", fontSize, FontStyle.Bold, GraphicsUnit.Pixel))
            using (var textBrush = new SolidBrush(Color.White))
            using (var bgBrush = new SolidBrush(Color.FromArgb(140, 0, 0, 0)))
            using (var sf = new StringFormat())
            {
                sf.Alignment = StringAlignment.Near;
                sf.LineAlignment = StringAlignment.Near;
                sf.Trimming = StringTrimming.None;

                int textWidth = original.Width - (paddingX * 2);
                if (textWidth < 1)
                    textWidth = 1;

                // Measure text height (multi-line)
                SizeF textSize = g.MeasureString(footerText, font, textWidth, sf);

                // Measure one line height
                float lineHeight = g.MeasureString("Ag", font).Height;

                // Empty space below footer (~2 lines)
                int emptyBottomSpace = (int)Math.Ceiling(lineHeight * 2);

                // Footer height = text + padding
                int footerHeight = (int)Math.Ceiling(textSize.Height) + (paddingY * 2);

                // Footer rectangle FLOATS above the bottom
                var footerRect = new Rectangle(
                    0,
                    original.Height - footerHeight - emptyBottomSpace,
                    original.Width,
                    footerHeight
                );

                // Safety clamp (never go negative)
                if (footerRect.Y < 0)
                    footerRect.Y = 0;

                g.FillRectangle(bgBrush, footerRect);

                var textRect = new RectangleF(
                    footerRect.Left + paddingX,
                    footerRect.Top + paddingY,
                    footerRect.Width - (paddingX * 2),
                    footerRect.Height - (paddingY * 2)
                );

                g.DrawString(footerText, font, textBrush, textRect, sf);
            }

            SaveOverwriting(bmp, inputPath);
        }
    }


    private static void SaveOverwriting(Bitmap bmp, string path)
    {
        string ext = Path.GetExtension(path).ToLowerInvariant();

        if (ext == ".jpg" || ext == ".jpeg")
        {
            var codec = GetEncoder(ImageFormat.Jpeg);
            if (codec == null)
            {
                bmp.Save(path, ImageFormat.Jpeg);
                return;
            }

            using (var p = new EncoderParameters(1))
            {
                p.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);
                bmp.Save(path, codec, p);
            }
        }
        else if (ext == ".png")
        {
            bmp.Save(path, ImageFormat.Png);
        }
        else
        {
            bmp.Save(path); // fallback
        }
    }

    private static ImageCodecInfo GetEncoder(ImageFormat format)
    {
        foreach (var c in ImageCodecInfo.GetImageDecoders())
            if (c.FormatID == format.Guid)
                return c;
        return null;
    }
}
