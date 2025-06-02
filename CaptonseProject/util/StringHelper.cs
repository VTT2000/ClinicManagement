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

    // public static bool IsMatchSearchKey(string searchKey, string input)
    // {
    //     // Chuẩn hóa keyword: bỏ dấu, lowercase, tách từ
    //     var keywords = RemoveDiacritics(searchKey).ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
    //     var normalizedName = RemoveDiacritics(input).ToLower();

    //     // Đảm bảo tất cả từ khóa đều xuất hiện trong tên
    //     return keywords.All(word => normalizedName.Contains(word));
    // }
    public static bool IsMatchSearchKey(string searchKey, string input)
    {
        var keywords = RemoveDiacritics(searchKey).ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var inputWords = RemoveDiacritics(input).ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        int keywordIndex = 0;

        foreach (var word in inputWords)
        {
            if (keywordIndex < keywords.Length && word.Contains(keywords[keywordIndex]))
            {
                keywordIndex++;
            }
        }

        return keywordIndex == keywords.Length;
    }
}