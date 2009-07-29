using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.IO;
using Newtonsoft.Json.Linq;
using rtg.Models;

namespace rtg.Controllers
{
  public class FileManagerController : Controller
  {
    rtgDataContext db = new rtgDataContext();


    //
    // GET: /FileManager/
    public ActionResult Index()
    {
        return View();
    }

    public string Upload(HttpPostedFileBase FileData)
    {
      FileData.SaveAs(rtg.Properties.Settings.Default.UploadFolder + FileData.FileName);

      return "Upload OK!";
    }

    public string Move(string item, string folder)
    {
      item = WebPathToWinPath(item);
      folder = WebPathToWinPath(folder);

      if (!System.IO.Directory.Exists(folder))
      {
        //This is to overcome when jsTree sends through the above or below node
        //which may not be a dir
        if (System.IO.File.Exists(folder))
          folder = new FileInfo(folder).Directory.FullName;
      }

      if (System.IO.Directory.Exists(folder) && System.IO.Directory.Exists(item))
      {
        //DirectoryInfo d = new DirectoryInfo(folder);
        //System.IO.Directory.Move(item, folder);

        //UpdateFolderRefs(item, folder);
        return "success";      
      }
      if (System.IO.Directory.Exists(folder) && System.IO.File.Exists(item))
      {
        try
        {
          FileInfo file = new FileInfo(item);

          System.IO.File.Move(item, folder +"\\"+ file.Name);

          //now update references
          UpdateFileRefs(item, folder + "\\" + file.Name);
          return "success"; 
        }
        catch
        { return "fail!"; }
      }
      return "fail!";
    }

    /*private void UpdateFolderRefs(string oldRef, string newRef)
    {
      DirectoryInfo dir = new DirectoryInfo(newRef);
      foreach(DirectoryInfo d in dir.GetDirectories())
        UpdateFolderRefs();

      foreach(FileInfo f in dir.GetFiles())
        UpdateFileRefs();
    }*/

    private void UpdateFileRefs(string currentPath, string newPath)
    {

      currentPath = WinPathToWebPath(currentPath);
      newPath = WinPathToWebPath(newPath);

      foreach (PageObject po in db.PageObjects)
      {
        po.HtmlContent = po.HtmlContent.Replace(currentPath, newPath);
      }

      foreach (GalleryImage gi in db.GalleryImages.Where(i => i.Src == currentPath))
      {
        gi.Src = newPath;
        db.SubmitChanges();
      }
    }

    //turns "c:\\some\\file\\upload\\blah\\blah\\"
    //to "/upload/blah/blah/"
    private string WinPathToWebPath(string str)
    {
      string[] temp = rtg.Properties.Settings.Default.UploadFolder.Split('/');
      string uploadsFolderName = "";
      for (int i = temp.Length-1; i >= 0 && uploadsFolderName.Length == 0; i--)
        uploadsFolderName = temp[i];

      uploadsFolderName = string.Format("\\{0}\\", uploadsFolderName);


      string src = str.Substring(str.IndexOf(uploadsFolderName)).Replace("\\", "/");
      return src;
    }

    
    private string WebPathToWinPath(string str)
    {
      string[] temp = rtg.Properties.Settings.Default.UploadFolder.Split('/');
      string uploadsFolderName = "";
      for (int i = temp.Length - 1; i >= 0 && uploadsFolderName.Length == 0; i--)
        uploadsFolderName = temp[i];

      uploadsFolderName = string.Format("/{0}/", uploadsFolderName);


      string src = rtg.Properties.Settings.Default.UploadFolder+ str.Replace(uploadsFolderName, "");
      return src.Replace("/", "\\");
    }

    public string UploadTree()
    {
      return GetDirTree(rtg.Properties.Settings.Default.UploadFolder);
    }

    public string GetDirTree(string dir)
    {
      if (System.IO.Directory.Exists(dir))
      {
        /*return "[ " +
            "{ attributes: { id : 'pjson_1' }, state: 'open', data: 'Root node 1', children :" +
           "[" +
              "{ attributes: { id : 'pjson_2' }, data: { title : 'Custom icon'} }," +
              "{ attributes: { id : 'pjson_3' }, data: 'Child node 2' }," +
              "{ attributes: { id : 'pjson_4' }, data: 'Some other child node' }" +
            "]}," +
            "{ attributes: { id : 'pjson_5' }, data: 'Root node 2' } " +
           "]";*/
         
        DirectoryInfo dirBase = new DirectoryInfo(dir);
        JObject o = DirToJson(dirBase);
        return o.ToString();
      }
      else
        return "";
    }

    private int dircount = 0;
    private int filecount = 0;

    private JObject DirToJson(DirectoryInfo d)
    {
      JObject o = new JObject(
            new JProperty("state", dircount == 0? "open" : "closed"),
            new JProperty("attributes", new JObject(new JProperty("id", d.Name.ToLower() + "_" + dircount++), new JProperty("fsdata", WinPathToWebPath(d.FullName)))),
            new JProperty("data", 
              new JObject(
                new JProperty("title", d.Name)
              )
            )
      );
      JArray children = new JArray();
      foreach (DirectoryInfo cDir in d.GetDirectories())
      { 
        children.Add(DirToJson(cDir));
      }
      foreach (FileInfo cFile in d.GetFiles())
      {
        children.Add(FileToJson(cFile));
      }
      o.Add("children", children);
      return o;
    }

    private JToken FileToJson(FileInfo f)
    {
      JObject o = new JObject(
            new JProperty("attributes", 
              new JObject(
                new JProperty("id", f.Name.ToLower() + "_" + filecount++),
                new JProperty("fsdata", WinPathToWebPath(f.FullName)), 
                new JProperty("class", f.Extension.Replace(".", ""))
              )
            ),
            new JProperty("data", f.Name)
      );
      return o;
    }

  }
}
