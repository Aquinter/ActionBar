using System.Collections.Generic;

namespace FaceAuthentication
{
    public class RecognizeResponse
    {
        public List<Face> Faces { get; set; }
        public Thresholds Thresholds { get; set; }
    }

    public class Result
    {
        public string ModelId { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
    }

    public class Thresholds
    {
        public int Low { get; set; }
        public int Medium { get; set; }
        public int High { get; set; }
    }
}
