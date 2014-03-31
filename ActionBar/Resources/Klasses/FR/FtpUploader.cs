using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace FaceAuthentication
{
    /*public class FtpUploader
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
    }*/

    public class FtpUploader
    {
        private string ftpPath;
        private System.Net.NetworkCredential networkCredential;


        public FtpUploader(string ftpPath, System.Net.NetworkCredential networkCredential)
        {
            this.ftpPath = ftpPath;
            this.networkCredential = networkCredential;
            //private string ftpServer = "ftp://ftp.oasa.be/SYTYCC/";
        }
        public bool UploadImages(List<string> imageList)
        {
            try
            {
                foreach (string afbeelding in imageList)
                {
                    FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create("ftp://ftp.oasa.be/SYTYCC/face.jpg");
                    ftp.Credentials = networkCredential;
                    ftp.KeepAlive = true;
                    ftp.UseBinary = true;
                    ftp.Method = WebRequestMethods.Ftp.UploadFile;

                    FileStream fs = File.OpenRead(afbeelding);
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();

                    Stream ftpstream = ftp.GetRequestStream();
                    ftpstream.Write(buffer, 0, buffer.Length);
                    ftpstream.Close();
                    ftpstream.Flush();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        
    }
}
