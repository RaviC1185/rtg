using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;

public class iLogic
{
	public iLogic(){}

    public static string QueryStr(object key)
    {
        string _return = string.Empty;
        if (key != null)
            _return = key.ToString();

        return _return;
    }

    public static string DateShort(object key)
    {
        string _return = string.Empty;
        if (key != null)
        {
            DateTime dtTemp;
            string dt_display = "";
            if (DateTime.TryParse(key.ToString(), out dtTemp))
                dt_display = dtTemp.ToString("dd/MM/yy");

            _return = dt_display;
        }
        return _return;
    }

    public static string DateLong(object key)
    {
        string _return = string.Empty;
        if (key != null)
        {
            DateTime dtTemp;
            string dt_display = "";
            if (DateTime.TryParse(key.ToString(), out dtTemp))
                dt_display = dtTemp.ToLongDateString();

            _return = dt_display;
        }
        return _return;
    }

    public static string clearImgUrl(string img)
    {
        return img.Replace("http://", "");
    }

    public static System.Drawing.Image ImageSize(System.Drawing.Image imgPhoto, int Width, int Height)
    {
        int sourceWidth = imgPhoto.Width;
        int sourceHeight = imgPhoto.Height;
        int sourceX = 0;
        int sourceY = 0;
        int destX = 0;
        int destY = 0;

        float nPercent = 0;
        float nPercentW = 0;
        float nPercentH = 0;

        nPercentW = ((float)Width / (float)sourceWidth);
        nPercentH = ((float)Height / (float)sourceHeight);
        if (nPercentH < nPercentW)
        {
            nPercent = nPercentH;
            destX = System.Convert.ToInt16((Width -
                          (sourceWidth * nPercent)) / 2);
        }
        else
        {
            nPercent = nPercentW;
            destY = System.Convert.ToInt16((Height -
                          (sourceHeight * nPercent)) / 2);
        }

        int destWidth = (int)(sourceWidth * nPercent);
        int destHeight = (int)(sourceHeight * nPercent);

        Bitmap bmPhoto = new Bitmap(Width, Height,
                          PixelFormat.Format24bppRgb);
        bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                         imgPhoto.VerticalResolution);

        Graphics grPhoto = Graphics.FromImage(bmPhoto);
        grPhoto.Clear(Color.Black);
        grPhoto.InterpolationMode =
                InterpolationMode.HighQualityBicubic;

        grPhoto.DrawImage(imgPhoto,
            new Rectangle(destX, destY, destWidth, destHeight),
            new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
            GraphicsUnit.Pixel);

        grPhoto.Dispose();
        return bmPhoto;
    }

    public static System.Drawing.Image ImageSizeLocal(string imgURL, int Width, int Height)
    {
        try
        {
            Bitmap b;
            b = (Bitmap)System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(imgURL));
            return ImageSize(b, Width, Height);
        }
        catch (Exception ex) { }
        Bitmap bm = new Bitmap(Width, Height);
        Graphics g = Graphics.FromImage(bm);
        g.FillRectangle(new SolidBrush(Color.Black), 0, 0, (float)Width, (float)Height);
        g.Dispose();
        return bm;
    }

    public static System.Drawing.Image ImageCropLocal(string imgURL, int Width, int Height, AnchorPosition Anchor)
    {
        try
        {
            Bitmap b;
            b = (Bitmap)System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(imgURL));
            return ImageCrop(b, Width, Height, Anchor);
        }
        catch (Exception ex) { }
        Bitmap bm = new Bitmap(Width, Height);
        Graphics g = Graphics.FromImage(bm);
        g.FillRectangle(new SolidBrush(Color.Black), 0, 0, (float)Width, (float)Height);
        g.Dispose();
        return bm;
    }

    public static System.Drawing.Image ImageSize(string imgURL, int Width, int Height)
    {
        try
        {
            Bitmap b;
            //try
            //{
                WebClient wc = new WebClient();
                byte[] img = wc.DownloadData(imgURL);
                MemoryStream imgStream = new MemoryStream(img);
                b = (Bitmap)System.Drawing.Image.FromStream(imgStream);
            //}
            //catch (Exception ex1)
            //{
                //b = (Bitmap)System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(imgURL.Replace("http://", "")));
            //}
            return ImageSize(b, Width, Height);
        }
        catch (Exception ex){ }
        Bitmap bm = new Bitmap(Width, Height);
        Graphics g = Graphics.FromImage(bm);
        g.FillRectangle(new SolidBrush(Color.Black), 0, 0, (float)Width, (float)Height);
        g.Dispose();
        return bm;
    }

    public static System.Drawing.Image ImageCrop(string imgURL, int Width, int Height, AnchorPosition Anchor)
    {
        try
        {
            Bitmap b;
            //try
            //{
                WebClient wc = new WebClient();
                byte[] img = wc.DownloadData(imgURL);
                MemoryStream imgStream = new MemoryStream(img);
                b = (Bitmap)System.Drawing.Image.FromStream(imgStream);
            //}
            //catch (Exception ex1)
            //{
                //b = (Bitmap)System.Drawing.Image.FromFile(HttpContext.Current.Server.MapPath(imgURL.Replace("http://", "")));
            //}
            return ImageCrop(b, Width, Height, Anchor);
        }
        catch (Exception ex) { }
        Bitmap bm = new Bitmap(Width, Height);
        Graphics g = Graphics.FromImage(bm);
        g.FillRectangle(new SolidBrush(Color.Black), 0, 0, (float)Width, (float)Height);
        g.Dispose();
        return bm;
    }

    public static System.Drawing.Image ImageCrop(System.Drawing.Image imgPhoto, int Width, int Height, AnchorPosition Anchor)
    {
        int sourceWidth = imgPhoto.Width;
        int sourceHeight = imgPhoto.Height;
        int sourceX = 0;
        int sourceY = 0;
        int destX = 0;
        int destY = 0;

        float nPercent = 0;
        float nPercentW = 0;
        float nPercentH = 0;

        nPercentW = ((float)Width / (float)sourceWidth);
        nPercentH = ((float)Height / (float)sourceHeight);

        if (nPercentH < nPercentW)
        {
            nPercent = nPercentW;
            switch (Anchor)
            {
                case AnchorPosition.Top:
                    destY = 0;
                    break;
                case AnchorPosition.Bottom:
                    destY = (int)
                        (Height - (sourceHeight * nPercent));
                    break;
                default:
                    destY = (int)
                        ((Height - (sourceHeight * nPercent)) / 2);
                    break;
            }
        }
        else
        {
            nPercent = nPercentH;
            switch (Anchor)
            {
                case AnchorPosition.Left:
                    destX = 0;
                    break;
                case AnchorPosition.Right:
                    destX = (int)
                      (Width - (sourceWidth * nPercent));
                    break;
                default:
                    destX = (int)
                      ((Width - (sourceWidth * nPercent)) / 2);
                    break;
            }
        }

        int destWidth = (int)(sourceWidth * nPercent);
        int destHeight = (int)(sourceHeight * nPercent);

        Bitmap bmPhoto = new Bitmap(Width,
                Height, PixelFormat.Format24bppRgb);
        bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                imgPhoto.VerticalResolution);

        Graphics grPhoto = Graphics.FromImage(bmPhoto);
        grPhoto.InterpolationMode =
                InterpolationMode.HighQualityBicubic;

        grPhoto.DrawImage(imgPhoto,
            new Rectangle(destX, destY, destWidth, destHeight),
            new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
            GraphicsUnit.Pixel);

        grPhoto.Dispose();
        return bmPhoto;
    }

    public static string formatPrice(string num)
    {
        decimal d = 0;
        string _return = num;
        if (decimal.TryParse(num, out d))
            _return = string.Format("{0:c}", d);
        return _return;
    }

    public enum Dimensions
    {
        Width,
        Height
    }
    public enum AnchorPosition
    {
        Top,
        Center,
        Bottom,
        Left,
        Right
    }
}
