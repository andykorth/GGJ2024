using System;
using System.Text;
using UnityEngine;

namespace FutzSys
{
/// for pooling
public class MsgBuffer
{
	public const int BUFFER_SIZE = 100_000; // 16 * 1024; // TODO: what size?

	public readonly byte[] Bytes = new byte[BUFFER_SIZE];
	public int Length;
}

public static class UtilsMsgBuffer
{
	public static void Set(
		this MsgBuffer msgBuffer,
		int byteIndex,
		byte byteValue,
		bool setLength = false
	)
	{
		msgBuffer.Bytes[byteIndex] = byteValue;
		if (setLength) msgBuffer.Length = byteIndex + 1;
	}

	/// will throw if byteValue >255
	public static void Set(
		this MsgBuffer msgBuffer,
		int byteIndex,
		int byteValue,
		bool setLength = false
	)
	{
		msgBuffer.Bytes[byteIndex] = Convert.ToByte(byteValue);
		if (setLength) msgBuffer.Length = byteIndex + 1;
	}

	/// UTF8 string
	public static void Fill(
		this MsgBuffer msgBuffer,
		string str,
		int bufferStartIndex = 0,
		int stringStartIndex = 0,
		int stringLength = -1
	)
		=> msgBuffer.Length = bufferStartIndex
		                    + Encoding.UTF8.GetBytes(
			                      str,
			                      stringStartIndex,
			                      stringLength < 0 ? str.Length : stringLength,
			                      msgBuffer.Bytes,
			                      bufferStartIndex
		                      );

	/// UTF8 string (json)
	public static void FillJson(
		this MsgBuffer msgBuffer,
		object obj,
		int bufferStartIndex = 0
	)
		=> msgBuffer.Fill(JsonUtility.ToJson(obj), bufferStartIndex);

	/// UTF8 string
	public static string GetString(
		this MsgBuffer msgBuffer,
		int bufferStartIndex = 0
	)
		=> Encoding.UTF8.GetString(
			msgBuffer.Bytes,
			bufferStartIndex,
			msgBuffer.Length - bufferStartIndex
		);

	public static T GetJson<T>(
		this MsgBuffer msgBuffer,
		int bufferStartIndex = 0
	)
		=> JsonUtility.FromJson<T>(msgBuffer.GetString(bufferStartIndex));

	public static void CopyTo(this MsgBuffer msgBuffer, byte[] array, int bufferStartIndex = 0)
		=> Array.Copy(
			msgBuffer.Bytes,
			bufferStartIndex,
			array,
			0,
			msgBuffer.Length - bufferStartIndex
		);

	/// GARBAGE: new array
	public static byte[] NewSubset(this MsgBuffer msgBuffer, int bufferStartIndex = 0)
	{
		var subset = new byte[msgBuffer.Length - bufferStartIndex];
		msgBuffer.CopyTo(subset, bufferStartIndex);
		return subset;
	}
}
}