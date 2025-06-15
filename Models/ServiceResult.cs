namespace SepcialMomentBE.Models
{
    public class ServiceResult<T>
    {
        public T? Result { get; set; }
        public bool IsSuccessful { get; set; } = true;
        public List<string> ValidationMessages { get; set; } = new();

        public void AddValidationMessage(string message)
        {
            ValidationMessages.Add(message);
            IsSuccessful = false;
        }
    }
} 