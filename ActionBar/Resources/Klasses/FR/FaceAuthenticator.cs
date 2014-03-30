using System;
using System.Collections.Generic;
using System.IO;
using RestSharp;
using RestSharp.Deserializers;
using FaceAuthentication.RestResponses;

namespace FaceAuthentication
{

    public class FaceAuthenticator
    {
        private string userNameKeyLemon;
        private string appKey;
        private System.Net.NetworkCredential networkCredential;
        private string ftpPath;
        private string netAddres;
        private string faceName;

        public string FaceName
        {
            get { return faceName; }
            set { faceName = value; }
        }

        public string ModelId { get; set; }

        private RestClient restClient;
        private JsonDeserializer myDeserializer;

        public FaceAuthenticator(string userNameKeyLemon, string appKey, string ftpPath, System.Net.NetworkCredential networkCredential, string netAddres, string faceName)
        {
            this.userNameKeyLemon = userNameKeyLemon;
            this.appKey = appKey;
            this.networkCredential = networkCredential;
            this.ftpPath = ftpPath;
            this.netAddres = netAddres;
            this.faceName = faceName;

            restClient = new RestClient("https://api.keylemon.com");
            restClient.ClearHandlers();

            myDeserializer = new JsonDeserializer();
        }

        public bool CreateModel(List<string> imagePath)
        {
            try
            {
                FtpUploader myFtpUploader = new FtpUploader(ftpPath, networkCredential);
                myFtpUploader.UploadImages(imagePath);
                string imageList = "&urls=";

                foreach (string currentImage in imagePath)
                {
                    imageList += netAddres + "/" + new FileInfo(currentImage).Name.ToString() + ",";
                }
                imageList = imageList.Remove(imageList.Length - 1);

                var request = new RestRequest(Method.POST);
                request.Resource = "api/model";
                request.AddParameter("user", userNameKeyLemon);
                request.AddParameter("key", appKey);
                request.AddParameter("urls", imageList);
                request.AddParameter("name", faceName);
                request.RequestFormat = DataFormat.Json;

                var response = restClient.Execute(request);

                var result = myDeserializer.Deserialize<ModelCreationResponse>(response);

                ModelId = result.ModelId;
                return true;
            }
            catch (Exception e)
            {
                //TODO add debugger
                return false;
            }
        }
        public bool RecognizeFace(string localImagePath, string imageName) //imageName should contain exstension !! .jpg
        {
            if (ModelId != null)
            {
                //upload the face 
                UploadSingleImage(localImagePath);
                //set url string
                string imageUrl = netAddres + "/" + imageName;

                var request = new RestRequest(Method.POST);
                request.Resource = "api/recognize";
                request.AddParameter("user", userNameKeyLemon);
                request.AddParameter("key", appKey);
                request.AddParameter("urls", imageUrl);
                request.AddParameter("models", ModelId);
                request.RequestFormat = DataFormat.Json;

                var response = restClient.Execute(request);
                var result = myDeserializer.Deserialize<RecognizeResponse>(response);

                List<Face> myFaces = result.Faces;

                foreach (Face curentFace in myFaces)
                {
                    List<Result> myResults = curentFace.Results;
                    foreach (Result currentResult in myResults)
                    {
                        if (currentResult.Score >= 65) //mayby set the score a little bit higer ...
                        {
                            AddFaceToModel(localImagePath, imageUrl);
                            return true;
                        }
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        public bool AddFaceToModel(string localImagePath, string imageName)
        {
            UploadSingleImage(localImagePath);
            string imageUrl = netAddres + "/" + imageName;
            var request = new RestRequest(Method.POST);
            request.Resource = "api/model/" + ModelId;
            request.AddParameter("user", userNameKeyLemon);
            request.AddParameter("key", appKey);
            request.AddParameter("urls", imageUrl);
            request.RequestFormat = DataFormat.Json;
            var response = restClient.Execute(request);
            var result = myDeserializer.Deserialize<ModelCreationResponse>(response);
            return true;
        }
        public bool AddFaceToModel(List<string> localImagePath)
        {
            FtpUploader myFtpUploader = new FtpUploader(ftpPath, networkCredential);
            myFtpUploader.UploadImages(localImagePath);
            string imageList = "&urls=";

            foreach (string currentImage in localImagePath)
            {
                imageList += netAddres + "/" + new FileInfo(currentImage).Name.ToString() + ",";
            }
            imageList = imageList.Remove(imageList.Length - 1);

            var request = new RestRequest(Method.POST);
            request.Resource = "api/model/" + ModelId;
            request.AddParameter("user", userNameKeyLemon);
            request.AddParameter("key", appKey);
            request.AddParameter("urls", imageList);
            request.RequestFormat = DataFormat.Json;

            var response = restClient.Execute(request);

            var result = myDeserializer.Deserialize<ModelCreationResponse>(response);

            return true;
        }
        private bool UploadSingleImage(string localImagePath)
        {
            try
            {
                List<string> singeImageList = new List<string>();
                singeImageList.Add(localImagePath);
                FtpUploader myFtpUploader = new FtpUploader(ftpPath, networkCredential);
                myFtpUploader.UploadImages(singeImageList);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool faceDeteced(string localImagePath, string imageName)
        {
            //upload the image
            UploadSingleImage(localImagePath);
            //check te face... building the rest request
            string imageUrl = netAddres + "/" + imageName;
            var request = new RestRequest(Method.POST);
            request.Resource = "api/face";
            request.AddParameter("user", userNameKeyLemon);
            request.AddParameter("key", appKey);
            request.AddParameter("urls", imageUrl);
            request.RequestFormat = DataFormat.Json;
            var response = restClient.Execute(request);
            var result = myDeserializer.Deserialize<FaceList>(response);

            if (result.faces != null)
            {
                Face detectedFace = (Face)result.faces[0];
                return true;
            }
            else
                return false;
        }
    }


}
