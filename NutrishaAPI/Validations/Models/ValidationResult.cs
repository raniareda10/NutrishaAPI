using System.Collections.Generic;

namespace NutrishaAPI.Validations.Models
{
    public class ValidationResult
    {
        public bool Success => Errors.Count == 0;
        public List<string> Errors { get; } = new List<string>();

        public void AddError(string error)
        {
            Errors.Add(error);
        }
    }
}