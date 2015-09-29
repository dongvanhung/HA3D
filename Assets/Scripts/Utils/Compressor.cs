using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class Compressor {

	public static string Compress(string aString) {
		byte[] bytesToEncode = Encoding.UTF8.GetBytes(aString);
		return Convert.ToBase64String(bytesToEncode);
	}

	public static string UnCompress(string aString) {
		byte[] decodedBytes = Convert.FromBase64String(aString);
		return Encoding.UTF8.GetString(decodedBytes);
	}
}
