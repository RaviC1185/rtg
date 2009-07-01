using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using rtg.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Collections;
using System.Text;

namespace rtg.Controllers
{
  public class PageController : Controller
  {
    rtgDataContext db = new rtgDataContext();

    public ActionResult Index()
    {
      return View("Pages",db.Pages.ToArray());
    }

    public ActionResult ContentView(int id)
    {
      Page p = db.Pages.First(pg => pg.PageID == id);

      return View("ContentTab", p);
    }

    public ActionResult GetPageSettings(int id) 
    {
      Page p = db.Pages.FirstOrDefault(pg => pg.PageID == id);

      if (p != null)
      {
        return View("PageSettingsPanel", p);
      }

      return null;
    }

    public JsonResult SavePageSettings(int id)
    {
      Page p = db.Pages.FirstOrDefault(pg => pg.PageID == id);

      if (p != null)
      {
        TryUpdateModel(p);
        string[] str = Request.Form["Locked"].Split(',');
        if (str[0] == "true")
          p.Locked = 1;
        else
          p.Locked = 0;
        db.SubmitChanges();
      }

      string cls = "page";
      if(p.DisplayInMenu == false)
        cls = "invisible";
      if(p.Locked == 1)
        cls = "locked";

      string[] attributes = { p.PageID.ToString(), p.Title, cls };

      return Json(attributes);
    }

    public ActionResult GetPageEditorPanel(int id)
    {
      Page p = db.Pages.FirstOrDefault(pg => pg.PageID == id);

      if (p.TemplateID == 1)
        return View("PageEditorPanel", p);
      else // if (p.TemplateID == 10)
        return View("GalleryEditor", p);
    }

    public string AddPage(string title, int? parentid)
    {
      Page lastpage;
      if (parentid > 0)
        lastpage = db.Pages.Where(pg => pg.ParentID == parentid).OrderByDescending(pg => pg.MenuOrder).FirstOrDefault();
      else
      {
        lastpage = db.Pages.Where(pg => object.Equals(pg.ParentID, null)).OrderByDescending(pg => pg.MenuOrder).FirstOrDefault();
        parentid = null;
      }

      int? menuOrder = 1;
      if (lastpage != null)
        menuOrder = lastpage.MenuOrder + 1;

      Page p = new Page();
      p.Title = title;
      p.MenuTitle = title;
      p.ParentID = parentid;
      p.MenuOrder = menuOrder;
      p.DisplayInMenu = false;
      p.Locked = 0;
      p.DevelopmentID = 1;
      p.CreatedBy = "admin";
      p.Enabled = true;
      p.Type = "template";

      PageObject po = new PageObject();
      po.Page = p;
      po.HtmlContent = "";
      po.Type = "{html_content}";

      db.Pages.InsertOnSubmit(p);
      db.PageObjects.InsertOnSubmit(po);
      db.SubmitChanges();

      return p.PageID.ToString();
    }

    public void Delete(int id)
    {
      Page p = db.Pages.FirstOrDefault(pg => pg.PageID == id);
      if (p != null)
      {
        DeletePage(p);
      }
      db.SubmitChanges();
    }

    private void DeletePage(Page p)
    {
      foreach (Page pg in p.Children)
      {
        DeletePage(pg);
      }
      db.PageObjects.DeleteAllOnSubmit(p.PageObjects);
      db.Pages.DeleteOnSubmit(p);
    }

    public void SavePageTree()
    {
      string temp = Request.Params["pagetree"];

      JArray a = JArray.Parse(temp);

      int i = 1;
      foreach (JObject j in a)
      {
        ProcessNode(j, 0, i++);
      }
    }

    private void ProcessNode(JObject j, int parentID, int menuOrderNum)
    {
      int id = j["attributes"].Value<int>("id");

      if (parentID != 0)
        UpdatePageRef(id, parentID, menuOrderNum);
      else
        UpdatePageRef(id, null, menuOrderNum);

      if (j["children"] != null)
      {
        JArray cArray = j.Value<JArray>("children");
        int i = 1;
        foreach (JObject cj in cArray)
        {
          ProcessNode(cj, id, i++);
        }
      }
    }

    private void UpdatePageRef(int id, int? parentID, int menuOrderNum)
    {
      Page p = db.Pages.FirstOrDefault(pg => pg.PageID == id);
      if (p.ParentID != parentID || p.MenuOrder != menuOrderNum)
      {
        p.ParentID = parentID;
        p.MenuOrder = menuOrderNum;
        db.SubmitChanges();
      }
    }

    [ValidateInput(false)]
    public void UpdatePageContent(int id)
    {
      Page p = db.Pages.FirstOrDefault(pg => pg.PageID == id);

      if (p != null)
      {
        PageObject po = p.PageObjects.FirstOrDefault();
        TryUpdateModel(po);
        db.SubmitChanges();
      }
    }

    public ActionResult GetFolderGallery(string dir)
    {
      System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(dir);
      
      return View("FilesToGallery", di.GetFiles());
    }

    public ActionResult GetFilePreview(string file)
    { 
      FileInfo fi = new FileInfo(file);

      string webref = file.Substring(file.IndexOf("/upload/")); 

      ViewData["webref"] = webref;

      return View("FilePreview", fi);
    }

    public ActionResult AddGalleryCategory()
    {
      return View("GalleryCategory");
    }

    public int SaveGalleryCategory(string CategoryTitle)
    {
      GalleryCategory gc = db.GalleryCategories.FirstOrDefault(c => c.Title == CategoryTitle);

      if (gc == null)
      {
        gc = new GalleryCategory();
        gc.Title = CategoryTitle;
        db.GalleryCategories.InsertOnSubmit(gc);
        db.SubmitChanges(); 
      }

      return gc.GalleryCategoryID;
    }

    public void SaveCategoryCollection(int categoryID, string[] galleries)
    {
      GalleryCategory gc = db.GalleryCategories.FirstOrDefault(cat => cat.GalleryCategoryID == categoryID);

      if (gc != null)
      {
        foreach (string s in galleries)
        {
          if (s.Contains("Gallery") && !s.Contains("GalleryCategoryID"))
          {
            int galleryid = int.Parse(s.Replace("Gallery", ""));
            Gallery g = db.Galleries.FirstOrDefault(gl => gl.GalleryID == galleryid);
            g.GalleryCategoryID = categoryID;
            db.SubmitChanges();
          }
        }
      }
    }

    public ActionResult AddGallery()
    {
      Gallery g = new Gallery();
      g.GalleryID = 0;
      g.Title = "";
      g.Description = "";
      return View("BlankGallery", g);
    }

    public int SaveGallery( int GalleryID, int? pageid)
    {
      if (GalleryID != 0) //existing gallery
      {
        Gallery g = db.Galleries.FirstOrDefault(gl => gl.GalleryID == GalleryID);
        if (g != null)
        {
          TryUpdateModel(g);
          db.SubmitChanges();
        }
      }
      else //new gallery 
      {
        Gallery g = new Gallery();
        TryUpdateModel(g);
        g.GalleryCategoryID = 0;
        db.Galleries.InsertOnSubmit(g);
        db.SubmitChanges();
        GalleryID = g.GalleryID;
      }
      return GalleryID;
    }

    public void DeleteGallery(int id)
    {
      Gallery g = db.Galleries.FirstOrDefault(gl => gl.GalleryID == id);
      if (g != null)
      {
        db.GalleryImages.DeleteAllOnSubmit(g.GalleryImages);
        db.Galleries.DeleteOnSubmit(g);
        db.SubmitChanges();
      }
    }

    public ActionResult AddImageToGallery(int galleryID, string imageSrc)
    {
      string src = imageSrc.Substring(imageSrc.IndexOf("/upload/"));
      FileInfo fi = new FileInfo(imageSrc);

      if (fi.Extension == ".jpg" || fi.Extension == ".gif" || fi.Extension == ".png")
      {

        GalleryImage gi = new GalleryImage();
        gi.GalleryID = galleryID;
        gi.Src = src;
        gi.Position = db.GalleryImages.Count(i => i.GalleryID == galleryID);
        db.GalleryImages.InsertOnSubmit(gi);
        db.SubmitChanges();

        return View("GalleryImage", gi);
      }

      return null;
    }

    public void DeleteGalleryImage(int id)
    {
      GalleryImage gi = db.GalleryImages.FirstOrDefault(g => g.GalleryImageID == id);

      if (gi != null)
      {
        db.GalleryImages.DeleteOnSubmit(gi);
        db.SubmitChanges();
      }
    }

    public ActionResult AddFolderToGallery(int galleryID, string folder)
    {
      Gallery g = db.Galleries.FirstOrDefault(gl => gl.GalleryID == galleryID);

      if (g != null)
      {
        DirectoryInfo dir = new DirectoryInfo(folder);
        FolderToGallery(g, dir);
      }

      return View("GalleryImageCollection", g.GalleryImages.OrderBy(gi=>gi.Position));
    }

    private void FolderToGallery(Gallery g, DirectoryInfo dir)
    {
        foreach(DirectoryInfo d in dir.GetDirectories())
        {
          FolderToGallery(g, d);
        }
        foreach (FileInfo f in dir.GetFiles())
        {
          if (f.Extension == ".jpg" || f.Extension == ".gif" || f.Extension == ".png")
          {
            AddImage(g, f);
          }
        }
    }

    private void AddImage(Gallery g, FileInfo f)
    {
      GalleryImage gi = new GalleryImage();
      gi.Gallery = g;
      gi.Src = FileInfoToSrc(f);
      gi.Position = g.GalleryImages.Count();
      db.GalleryImages.InsertOnSubmit(gi);
      db.SubmitChanges();
    }

    private string FileStringToSrc(string s)
    {
      return s.Substring(s.IndexOf("/upload/"));
    }

    private string FileInfoToSrc(FileInfo f)
    {
      string src = f.FullName.Substring(f.FullName.IndexOf("\\upload\\")).Replace("\\", "/");
      return src;
    }
    
    public ActionResult MoveImageToGallery(int galleryID, int imageID)
    {
      GalleryImage gi = db.GalleryImages.FirstOrDefault(i => i.GalleryImageID == imageID);
      if (gi != null)
      {
        gi.GalleryID = galleryID;
        gi.Position = db.GalleryImages.Count(i => i.GalleryID == galleryID);
        db.SubmitChanges();
      }

      return View("GalleryImage", gi);
    }

    public void SaveGalleryCollection(int galleryID, string[] images)
    {
      Gallery g = db.Galleries.FirstOrDefault(gl => gl.GalleryID == galleryID);
      if (g != null)
      {
        int i = 0;
        IDictionary<int, int> giIDs = new Dictionary<int, int>();
        foreach (string s in images)
        {
          if (s != "")
          {
            int id = int.Parse(s.Replace("GI", ""));
            giIDs.Add(id, i++);
          }
        }

        foreach (GalleryImage gi in g.GalleryImages)
        {
          if (giIDs.ContainsKey(gi.GalleryImageID)) //had to do this because sortable sends through the gallery your removing the image from first
          {                                         //and then sends through the gallery your moving the image to.
            gi.Position = giIDs[gi.GalleryImageID];
            giIDs.Remove(gi.GalleryImageID);
          }
        }

        GalleryImage newgi;
        foreach (int id in giIDs.Keys)
        {
          newgi = db.GalleryImages.FirstOrDefault(item => item.GalleryImageID == id);
          newgi.Gallery = g;
          newgi.Position = giIDs[id];
        }

        db.SubmitChanges();
      }
    }
  }
}
