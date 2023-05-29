namespace Capstone_VotingSystem.Models.ResponseModels.AuthenResponse
{
    public class FirebaseResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }

        public void ToFailedResponse(string message)
        {
            Success = false;
            Message = message;
        }

        public void ToSuccessResponse(string message)
        {
            Success = true;
            Message = message;
        }

        public void ToSuccessResponse(T data, string message)
        {
            Success = true;
            Message = message;
            Data = data;

        }
    }
    public class FirebaseResponse
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;

        public void ToFailedResponse(string message)
        {
            Success = false;
            Message = message;
        }

        public void ToSuccessResponse(string message)
        {
            Success = true;
            Message = message;
        }
    }
}
