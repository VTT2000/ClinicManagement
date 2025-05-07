using System.Globalization;
using System.Text;

public static class StringHelper
{
    /// <summary>
    /// Hàm bỏ dấu
    /// </summary>
    /// <param name="input">giá trị chuỗi string</param>
    /// <returns>Chuỗi strig được bỏ dấu</returns>
    public static string RemoveDiacritics(string input)
    {
        var normalized = input.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}