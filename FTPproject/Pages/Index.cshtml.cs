using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace FTPproject.Pages
{
    public class IndexModel : PageModel
    {
        public string timestamp { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var image = Request.Form.Files.GetFile("imageFile");
            timestamp = GetTimestamp(DateTime.Now);

            await Upload(image);

            return RedirectToPage("./Index");
        }



        public static string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }


        public async Task Upload(IFormFile file)
        {
            string url = "ftp://[YOUR_FTP_CONNECTION_DOMAIN]/images/" + timestamp + "-" + file.FileName;
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
            request.Credentials = new NetworkCredential("USERNAME", "PASSWORD");
            request.Method = WebRequestMethods.Ftp.UploadFile;

            using (Stream ftpStream = request.GetRequestStream())
            {
                file.CopyTo(ftpStream);
            }
        }
    }
}