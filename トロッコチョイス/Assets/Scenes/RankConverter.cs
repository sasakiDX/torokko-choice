using UnityEngine;

public static class RankConverter
{
    public static string GetRank(int value)
    {
        if (value >= 90) return "A";
        if (value >= 80) return "B";
        if (value >= 60) return "C";
        if (value >= 40) return "D";
        if (value >= 20) return "E";
        return "F";
    }
}