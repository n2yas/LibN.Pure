using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace LibN.Pure.Extensions;

public static class ExtString
{
    #region substring



    /// <summary>
    /// sが指定したprefixから始まっている場合、それを取り除いたものを返す
    /// そうでない場合、nullを返す
    /// </summary>
    /// <param name="s"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public static string? PrefixRemoved(this string s, string prefix) => s.StartsWith(prefix) ? s.Substring(prefix.Length) : null;

    /// <summary>
    /// sが指定したprefixから始まっている場合、それを取り除いたものを返す
    /// そうでない場合、sを返す
    /// </summary>
    /// <param name="s"></param>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public static string PrefixRemovedOrSelf(this string s, string prefix) => s.PrefixRemoved(prefix) ?? s;

    /// <summary>
    /// sが指定したsuffixで終わっている場合、それを取り除いたものを返す
    /// そうでない場合、nullを返す
    /// </summary>
    /// <param name="s"></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    public static string? SuffixRemoved(this string s, string suffix) => s.EndsWith(suffix) ? s.Substring(0, s.Length - suffix.Length) : null;

    /// <summary>
    /// sが指定したsuffixで終わっている場合、それを取り除いたものを返す
    /// そうでない場合、sを返す
    /// </summary>
    /// <param name="s"></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    public static string SuffixRemovedOrSelf(this string s, string suffix) => s.SuffixRemoved(suffix) ?? s;

    /// <summary>
    /// sの左右から指定した文字列を取り除いて返す
    /// 取り除けなかった場合はnullを返す
    /// </summary>
    /// <param name="s"></param>
    /// <param name="prefix"></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    public static string? Inner(this string s, string prefix, string suffix) => s.SuffixRemoved(suffix)?.PrefixRemoved(prefix);

    /// <summary>
    /// sの左右から指定した文字列を取り除いて返す
    /// 取り除けなかった場合はsを返す
    /// </summary>
    /// <param name="s"></param>
    /// <param name="prefix"></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    public static string InnerOrSelf(this string s, string prefix, string suffix) => s.Inner(prefix, suffix) ?? s;

    /// <summary>
    /// 左から部分文字列を探し、最初に見つかった部分よりも前を返す
    /// 見つからなかった場合はnullを返す
    /// </summary>
    /// <param name="haystack"></param>
    /// <param name="needle"></param>
    /// <returns></returns>
    public static string? Before(this string haystack, string needle)
    {
        var i = haystack.IndexOf(needle);
        return 0 <= i ? haystack.Substring(0, i) : null;
    }

    /// <summary>
    /// 左から部分文字列を探し、最初に見つかった部分よりも前を返す
    /// 見つからなかった場合はhaystackを返す
    /// </summary>
    /// <param name="haystack"></param>
    /// <param name="needle"></param>
    /// <returns></returns>
    public static string BeforeOrSelf(this string haystack, string needle) => haystack.Before(needle) ?? haystack;

    /// <summary>
    /// 左から部分文字列を探し、最初に見つかった部分の前と後ろを返す
    /// 見つからなかった場合は(haystack, null)を返す
    /// </summary>
    /// <param name="haystack"></param>
    /// <param name="needle"></param>
    /// <returns></returns>
    public static (string, string?) BeforeAfter(this string haystack, string needle)
    {
        var i = haystack.IndexOf(needle);
        return 0 <= i ? (haystack.Substring(0, i), haystack.Substring(i + needle.Length)) : (haystack, null);
    }

    /// <summary>
    /// 左から部分文字列を探して最初に見つかった部分よりも前を返し、haystackからその文字列とneedleを取り除く
    /// 見つからなかった場合はhaystackを返し、haystackをnullにする
    /// </summary>
    /// <param name="haystack"></param>
    /// <param name="needle"></param>
    /// <returns></returns>
    public static string PopBefore(this string needle, [DisallowNull] ref string? haystack)
    {
        (var ret, haystack) = haystack.BeforeAfter(needle);
        return ret;
    }



    #endregion
    #region Join



    public static string Join<T>(this IEnumerable<T> xs, string? separator = null) => string.Join(separator, xs);
    public static string Join<T, U>(this IEnumerable<T> xs, string? separator, Func<T, U> f) => xs.Select(f).Join(separator);
    public static string Join<T, U>(this IEnumerable<T> xs, Func<T, U> f) => xs.Join(null, f);
    public static string Join<T, U>(this IEnumerable<T> xs, string? separator, Func<T, int, U> f) => xs.Select(f).Join(separator);
    public static string Join<T, U>(this IEnumerable<T> xs, Func<T, int, U> f) => xs.Join(null, f);



    #endregion
    #region 改行



    /// <summary>
    /// 文字列末端に改行コードがあれば除去する (LFのみ)
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string Chomp(this string s) => s.SuffixRemovedOrSelf("\n");

    /// <summary>
    /// 文字列が改行で終わるように変更する (LFのみ)
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string Unchomp(this string s) => s.EndsWith("\n") ? s : s + "\n";

    /// <summary>
    /// 行のリストを返す (LFのみ)
    /// </summary>
    /// <param name="lines"></param>
    /// <returns></returns>
    public static IEnumerable<string> Lines(this string lines) => lines.Chomp().Split('\n');

    /// <summary>
    /// 行のリストを1つの文字列にまとめる
    /// </summary>
    /// <param name="xs"></param>
    /// <returns></returns>
    public static string Unlines<T>(this IEnumerable<T> xs) => string.Join(null, xs.Select(x => $"{x}\n"));



    #endregion
}
