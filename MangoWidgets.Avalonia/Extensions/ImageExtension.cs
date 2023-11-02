using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace MangoWidgets.Avalonia.Extensions;

public static class ImageExtension
{
    /// <summary>
    /// byte[]转IBitmap
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static Bitmap? ToBitmap(this byte[]? bytes)
    {
        Bitmap? bmpImg = null;
        try
        {
            if (bytes == null || bytes.Length == 0)
                bmpImg = null;
            else
            {
                bmpImg = new Bitmap(new MemoryStream(bytes));
            }
        }
        catch
        {
            bmpImg = null;
        }
        return bmpImg;
    }
    
    public static Bitmap? ToBitmap(this string fileName)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"File Not Found:{fileName}");
        using var fs = File.OpenRead(fileName);
        return new Bitmap(fs);
    }
    
}