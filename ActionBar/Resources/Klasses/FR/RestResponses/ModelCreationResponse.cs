using System.Collections.Generic;

namespace FaceAuthentication
{
    public class ModelCreationResponse
    {
        public string ModelId { get; set; }
        public string Name { get; set; }
        public int NbFaces { get; set; }
        public List<Face> Faces { get; set; }

    }
    public class Face
    {
        public string FaceId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public string ImageUrl { get; set; }           
        public List<Result> Results { get; set; }
    }

}


//not handling erros yey --> TO DO