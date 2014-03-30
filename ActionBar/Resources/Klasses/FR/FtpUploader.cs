using System;
using System.Collections.Generic;
using System.IO;

namespace FaceAuthentication
{
    public class FtpUploader
    {
        private string ftpPath;
        private System.Net.NetworkCredential networkCredential;

        public FtpUploader(string ftpPath, System.Net.NetworkCredential networkCredential)
        {
            this.networkCredential = networkCredential;
            this.ftpPath = ftpPath;
        }

        public bool UploadImages(List<string> imageList)
        {
            try
            {
                using (System.Net.WebClient myWebClient = new System.Net.WebClient())
                {
                    myWebClient.Credentials = networkCredential;
                    foreach (string currentPath in imageList)
                    {
                        myWebClient.UploadFile(ftpPath + "/" + new FileInfo(currentPath).Name, "STOR", currentPath);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                //TODO add debugger
                return false;
            }
        }
    }
}
