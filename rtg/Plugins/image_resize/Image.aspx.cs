using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing.Imaging;
using System.Drawing;

public partial class Image : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int width = 100;
        int height = 100;

        if (Request.QueryString["width"] != null)
            width = int.Parse(Request.QueryString["width"].ToString());

        if (Request.QueryString["height"] != null)
            height = int.Parse(Request.QueryString["height"].ToString());

        string img = iLogic.QueryStr(Request.QueryString["imgsrc"]);
        string type = iLogic.QueryStr(Request.QueryString["type"]);

        Response.ContentType = "image/jpeg";

        //if (iLogic.QueryStr(Request.QueryString["local"]) == string.Empty)
        //{
        //    if (!img.Contains("http://"))
        // /       img = "http://" + img;
//
  //          if (type == "crop")
    //            iLogic.ImageCrop(img, width, height, iLogic.AnchorPosition.Center).Save(Response.OutputStream, ImageFormat.Jpeg);
      //      else
        //        iLogic.ImageSize(img, width, height).Save(Response.OutputStream, ImageFormat.Jpeg);
        //}
        //else
        //{
            if (type == "crop")
                iLogic.ImageCropLocal(img, width, height, iLogic.AnchorPosition.Center).Save(Response.OutputStream, ImageFormat.Jpeg);
            else
                iLogic.ImageSizeLocal(img, width, height).Save(Response.OutputStream, ImageFormat.Jpeg);
        //}
    }
}
