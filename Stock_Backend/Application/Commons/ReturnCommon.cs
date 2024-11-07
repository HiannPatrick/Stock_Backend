using System.Text.Json.Serialization;

namespace Stock_Backend.Application
{
    public class ReturnCommon
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public object Data { get; private set; }

        [JsonConstructor]
        public ReturnCommon( bool success, string message, object? data = null )
        {
            Success = success;
            Message = message;
            Data = data ?? string.Empty;
        }

        public static ReturnCommon SuccessMessage( string message, object? data = null )
        {
            return new ReturnCommon( true, message, data );
        }

        public static ReturnCommon FailureMessage( string message )
        {
            return new ReturnCommon( false, message );
        }
    }
}
