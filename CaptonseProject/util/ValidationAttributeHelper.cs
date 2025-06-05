using System.ComponentModel.DataAnnotations;

public class ValidationAttributeHelper
{
    // public class DateOnlyLargeAsCurrentAttribute : ValidationAttribute
    // {
    //     public override bool IsValid(object? value)
    //     {
    //         if (value != null && value is DateOnly date)
    //         {
    //             return date >= DateOnly.FromDateTime(DateTime.Today);
    //         }
    //         return false;
    //     }
    //     public override string FormatErrorMessage(string name)
    //     {
    //         return $"{name} phải lớn hơn hoặc bằng ngày hiện tại.";
    //     }
    // }
}