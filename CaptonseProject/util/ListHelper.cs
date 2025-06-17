public static class ListHelper
{
    public static bool AreEqualExactButUnordered(List<string> list1, List<string> list2)
    {
        if (list1.Count != list2.Count) return false;

        var sorted1 = list1.OrderBy(x => x).ToList();
        var sorted2 = list2.OrderBy(x => x).ToList();

        return sorted1.SequenceEqual(sorted2);
    }
}