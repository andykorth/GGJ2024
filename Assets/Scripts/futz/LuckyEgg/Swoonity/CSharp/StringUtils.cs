using System;
using System.Collections.Generic;
using System.Text;
using Swoonity.Collections;
using UnityEngine;

namespace Swoonity.CSharp
{
public static class StringUtils
{
	/// $"{str}: {val}"
	public static string Val<T>(this string str, T val) => $"{str}: {val}";

	/// tries to parse string into given enum type, otherwise returns default enum value
	public static T ToEnum<T>(this string str, T defaultVal = default) where T : struct
	{
		return Enum.TryParse(str, out T enumVal) ? enumVal : defaultVal;
	}

	public static string Line(this string str, string text = "") => $"{str}\n{text}";
	public static string Bold(this string str) => $"<b>{str}</b>";
	public static string Italics(this string str) => $"<i>{str}</i>";

	public static string SpaceIfAny(this string str) => str.Any() ? $"{str} " : string.Empty;

	public static string Space(this string str) => $"{str} ";
	public static string Add(this string str, string add) => $"{str}{add}";

	/// ifThis ? $"{str}{add}" : $"{str}{or}"
	public static string AddIf(this string str, bool ifThis, string add, string or = "")
		=> ifThis ? $"{str}{add}" : $"{str}{or}";

	/// maybe use str[startIndex..endIndex] instead ;)
	public static string Sub(this string str, int startIndex, int endIndex)
		=> str.Substring(startIndex, endIndex - startIndex);

	public static string Str(this bool condition, string ifTrue, string ifFalse = "")
		=> condition ? ifTrue : ifFalse;

	/// ordinal
	public static int Compare(this string a, string b, bool ignoreCase = true)
		=> ignoreCase
			? string.Compare(a, b, StringComparison.OrdinalIgnoreCase)
			: string.Compare(a, b, StringComparison.Ordinal);

	public static string Join(this (string a, string b) v, string delimiter)
		=> $"{v.a}{delimiter}{v.b}";

	public static string Join(this (string a, string b, string c) v, string delimiter)
		=> $"{v.a}{delimiter}{v.b}{delimiter}{v.c}";

	public static string Join(this (string a, string b, string c, string d) v, string delimiter)
		=> $"{v.a}{delimiter}{v.b}{delimiter}{v.c}{delimiter}{v.d}";

	/// "a.b".GetStringBefore(".") => "a"  |  "ab".GetStringBefore(".") => ""
	public static string GetStringBefore(this string str, string search)
	{
		var index = str.IndexOf(search, StringComparison.OrdinalIgnoreCase);
		if (index <= 0) return "";
		return str.Sub(0, index).Trim();
	}

	public static string GetStringAfter(this string str, string search)
	{
		var index = str.IndexOf(search, StringComparison.OrdinalIgnoreCase);
		if (index <= 0) return "";
		return str.Sub(index, str.Length).Trim();
	}

	public static string Cut(this string str, string search)
		=> str.Replace(search, "");

	/// "a,b".SplitFirst(",") => "a"  |  "ab".SplitFirst(",") => "ab"
	public static string SplitFirst(this string str, string search)
	{
		var index = str.IndexOf(search, StringComparison.OrdinalIgnoreCase);
		if (index <= 0) return str;
		return str.Sub(0, index).Trim();
	}

	/// "a,b".SplitAfterFirst(",") => "b"  |  "ab".SplitAfterFirst(",") => "ab"  |  "a,b,c".SplitAfterFirst(",") => "b,c"
	public static string SplitAfterFirst(this string str, string search)
	{
		var index = str.IndexOf(search, StringComparison.OrdinalIgnoreCase);
		if (index <= 0) return str;
		return str.Sub(index + 1, str.Length).Trim();
	}

	/// splits string on first occurence
	public static (string before, string after) SplitOn(this string str, string search)
	{
		var index = str.IndexOf(search, StringComparison.OrdinalIgnoreCase);
		if (index <= 0) return ("", "");
		return (
			str.Sub(0, index).Trim(),
			str.Sub(index + search.Length, str.Length).Trim()
		);
	}

	/// splits string on first and second occurence
	public static (string begin, string middle, string end)
		SplitOn2(this string str, string search)
	{
		var first = str.IndexOf(search, StringComparison.OrdinalIgnoreCase);
		if (first <= 0) return ("", "", "");

		var second = str.IndexOf(search, first + 1, StringComparison.OrdinalIgnoreCase);
		if (second <= 0) return ("", "", "");

		return (
			str.Sub(0, first).Trim(),
			str.Sub(first + search.Length, second).Trim(),
			str.Sub(second + search.Length, str.Length).Trim()
		);
	}

	/// splits string on first and second occurence
	public static (string first, string second, string third, string fourth) SplitOn3(
		this string str,
		string search
	)
	{
		var first = str.IndexOf(search, StringComparison.OrdinalIgnoreCase);
		if (first <= 0) return ("", "", "", "");

		var second = str.IndexOf(search, first + 1, StringComparison.OrdinalIgnoreCase);
		if (second <= 0) return ("", "", "", "");

		var third = str.IndexOf(search, second + 1, StringComparison.OrdinalIgnoreCase);
		if (third <= 0) return ("", "", "", "");

		return (
			str.Sub(0, first).Trim(),
			str.Sub(first + search.Length, second).Trim(),
			str.Sub(second + search.Length, third).Trim(),
			str.Sub(third + search.Length, str.Length).Trim()
		);
	}

	/// splits string on first and second occurence
	public static (string first, string second, string third, string fourth, string fifth) SplitOn4(
		this string str,
		string search
	)
	{
		var first = str.IndexOf(search, StringComparison.OrdinalIgnoreCase);
		if (first <= 0) return ("", "", "", "", "");

		var second = str.IndexOf(search, first + 1, StringComparison.OrdinalIgnoreCase);
		if (second <= 0) return ("", "", "", "", "");

		var third = str.IndexOf(search, second + 1, StringComparison.OrdinalIgnoreCase);
		if (third <= 0) return ("", "", "", "", "");

		var fourth = str.IndexOf(search, third + 1, StringComparison.OrdinalIgnoreCase);
		if (fourth <= 0) return ("", "", "", "", "");

		return (
			str.Sub(0, first).Trim(),
			str.Sub(first + search.Length, second).Trim(),
			str.Sub(second + search.Length, third).Trim(),
			str.Sub(third + search.Length, str.Length).Trim(),
			str.Sub(fourth + search.Length, str.Length).Trim()
		);
	}

	public static string[] Split(this string str, string splitter, bool includeEmpties = false)
	{
		return str.Split(
			new[] { splitter },
			includeEmpties ? StringSplitOptions.None : StringSplitOptions.RemoveEmptyEntries
		);
	}

	/// foo || FOO => Foo
	public static string ToCapitalize(this string str)
		=> str.Any()
			? $"{str[0].ToString().ToUpper()}{str.Substring(1).ToLower()}"
			: "";

	/// Contains but ignores case
	public static bool ContainsLoose(this string str, string lookFor)
		=> str.IndexOf(lookFor, StringComparison.OrdinalIgnoreCase) >= 0;

	/// Similar to 'Contains', ignores case by default
	public static bool Has(
		this string str,
		string lookFor,
		StringComparison comparison = StringComparison.OrdinalIgnoreCase
	)
		=> str.IndexOf(lookFor, comparison) >= 0;

	/// Similar to 'Contains', ignores case by default
	public static bool IsIn(
		this string lookFor,
		string inside,
		StringComparison comparison = StringComparison.OrdinalIgnoreCase
	)
		=> inside.IndexOf(lookFor, comparison) >= 0;

	// /// Contains, but everything made lowercase first
	// public static bool ContainsLoose(this string str, string lookFor)
	// 	=> (str ?? "").ToLower().Contains((lookFor ?? "").ToLower());

	/// Similar to !str.Contains, ignores case by default
	public static bool Missing(
		this string str,
		string lookFor,
		StringComparison comparison = StringComparison.OrdinalIgnoreCase
	)
		=> str.IndexOf(lookFor, comparison) < 0;

	/// String is NOT null/empty
	public static bool Any(this string str) => !string.IsNullOrEmpty(str);

	/// String is null/empty
	public static bool Nil(this string str) => string.IsNullOrEmpty(str);

	/// str || or (if str is null/empty)
	public static string Or(this string str, string or) => string.IsNullOrEmpty(str) ? or : str;

	/// str || placeholder|"?" (if str is null/empty)
	public static string OrPh(this string str, string placeholder = "?")
		=> string.IsNullOrEmpty(str) ? placeholder : str;


	/// array Contains, but everything made lowercase first
	public static bool ContainsLoose(this string[] array, string lookFor)
	{
		foreach (var str in array) {
			if (str.ContainsLoose(lookFor)) return true;
		}

		return false;
	}

	public static string ToStr(this bool isTrue, string trueStr, string falseStr = null)
	{
		if (isTrue) return trueStr;
		return falseStr ?? $"!{trueStr}";
	}

	/*

		TODO: cleanup

	*/


	/// <summary>
	/// Converts string to int (or returns 0 if invalid string).
	/// </summary>
	public static int ToInt(this string stringValue)
	{
		return string.IsNullOrEmpty(stringValue) ? 0 : int.Parse(stringValue);
	}

	/// <summary>
	/// Shortcut for string.Format
	/// </summary>
	public static string Fmt(this string formatString, params object[] args)
	{
		return string.Format(formatString, args);
	}

	/// <summary>
	/// Returns enum from string
	/// </summary>
	// public static T ToEnum<T>(this string enumValueName, bool ignoreCase = true) {
	//     return (T)Enum.Parse(typeof(T), enumValueName, ignoreCase);
	// }


	/// <summary>
	/// Returns true if string is empty
	/// </summary>
	public static bool IsEmpty(this string targetString)
	{
		return targetString == string.Empty;
	}

	/// <summary>
	/// Returns true if string is NOT empty
	/// </summary>
	public static bool IsNotEmpty(this string targetString)
	{
		return targetString != string.Empty;
	}

	public static string Prefix(this string str, string prefix) => $"{prefix}{str}";
	public static string Suffix(this string str, string suffix) => $"{str}{suffix}";
	public static string And(this string str, string suffix) => $"{str}{suffix}";

	/// <summary>
	/// Iterates through a s using a callback on each character
	/// </summary>
	public static void Each(this string targetString, Action<char> callback)
	{
		for (int index = 0; index < targetString.Length; index++) {
			callback(targetString[index]);
		}
	}

	/// <summary>
	/// Returns the same string, unless it's blank in which case it returns "<blank>" instead. Used for better logging.
	/// </summary>
	public static string ValueOrBlank(this string targetString, string blankText = "<blank>")
	{
		return targetString.IsEmpty()
			? blankText
			: targetString;
	}

	/// <summary>
	/// Returns true if string contains character
	/// </summary>
	public static bool Contains(this string targetString, char character)
	{
		return targetString.IndexOf(character) >= 0;
	}

	// /// <summary>
	// /// Iterates through a string split by a character
	// /// </summary>
	// public static void SplitEach(
	// 	this string targetString,
	// 	char character,
	// 	Action<string> callback
	// ) {
	// 	targetString.Split(character).Each(callback);
	//
	//
	// 	//			var currentStart = 0;
	// 	//			for (int index = 0; index < targetString.Length; index++) {
	// 	//				if (targetString[index] == character) {
	// 	//					callback(targetString.Substring(currentStart, index - currentStart));
	// 	//					currentStart = index;
	// 	//				}
	// 	//
	// 	//
	// 	//			}
	// }

	public static List<string> SplitTrim(this string str, string separator)
	{
		var list = new List<string>();
		var split = str.Split(separator);
		foreach (var s in split) {
			var trimmed = s.Trim();
			if (trimmed != "") list.Add(trimmed);
		}

		return list;
	}

	// public static string[] SplitRemoveEmpty(this string targetString, params char[] separator) {
	// 	return targetString
	// 	   .Split(separator, StringSplitOptions.RemoveEmptyEntries)
	// 	   .Select(stringEntry => stringEntry.Trim())
	// 	   .Where(stringEntry => stringEntry != "")
	// 	   .ToArray();
	// }

	/// <summary>
	/// Adds string to list<string> if not empty
	/// </summary>
	public static void AddIfNotEmpty(this List<string> list, string targetString)
	{
		if (targetString.IsNotEmpty()) {
			list.Add(targetString);
		}
	}

	/// <summary>
	/// Joins list of strings together using a delimiter
	/// </summary>
	// public static string Join(this List<string> list, string delimiter = ", ") {
	//     return String.Join(delimiter, list.ToArray());
	// }
	public static void AddIfAny(this List<string> list, string str)
	{
		if (str == string.Empty) return;
		list.Add(str);
	}

	// TODO: rewrite
	//SOURCE : https://stackoverflow.com/questions/272633/add-spaces-before-capital-letters
	public static string AddSpacesToSentence(
		this string text,
		bool preserveAcronyms,
		string patternToRemove = null
	)
	{
		if (string.IsNullOrWhiteSpace(text))
			return string.Empty;
		if (!string.IsNullOrWhiteSpace(patternToRemove)) {
			text = text.Replace(patternToRemove, string.Empty);
		}

		StringBuilder newText = new StringBuilder(text.Length * 2);
		newText.Append(text[0]);
		for (int i = 1; i < text.Length; i++) {
			if (char.IsUpper(text[i]))
				if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1]))
				 || (preserveAcronyms
				  && char.IsUpper(text[i - 1])
				  && i < text.Length - 1
				  && !char.IsUpper(text[i + 1])))
					newText.Append(' ');
			newText.Append(text[i]);
		}

		return newText.ToString();
	}


	// public static Color ToColor(this string str) {
	// 	return ColorUtility.TryParseHtmlString(str, out var color) ? color : Color.black;
	// }
}
}