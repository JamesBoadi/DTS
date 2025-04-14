namespace Tasks.Extension.Validate;

using System.ComponentModel.DataAnnotations;

public static class ValidationExtension
{
    public static IResult Validate<T>(this T model)
    {
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        if (!Validator.TryValidateObject(model, context, results, true))
        {
            return Results.BadRequest(results);
        }

        return Results.Ok();
    }
}