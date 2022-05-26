using System.Collections.Generic;

namespace DL.ResultModels
{
    public class BaseServiceResult
    {
        public bool Success => Errors.Count == 0;
        public IList<string> Errors { get; set; } = new List<string>();

    }
}