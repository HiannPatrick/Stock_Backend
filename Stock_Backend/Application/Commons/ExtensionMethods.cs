namespace Stock_Backend.Application
{
    public static class ExtensionMethods
    {
        public static string GetErrorMessage( this FluentValidation.Results.ValidationResult validationResult )
        {
            if( validationResult is null )
                return string.Empty;

            string error = "";

            validationResult.Errors.ForEach( o => error += o.ErrorMessage + "\n" );

            error = error.Equals( "" ) ? "Falha ao validar o produto" : error;

            return error;
        }
    }
}
