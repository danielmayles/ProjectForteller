using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class Serializer {
	
	private List<byte> data = new List<byte>(2048);

	public void WriteInData(byte[] data) {
		this.data.AddRange(data);
	}

	public byte[] ReadData() {
		byte[] dataToRead = data.ToArray();
		data.Clear();

		return dataToRead;
	}
	
	public void SerializeString(string stringToSerialize) {
		SerializeInt(stringToSerialize.Length);
		if (stringToSerialize.Length > 0) {
			data.AddRange(System.Text.Encoding.UTF8.GetBytes(stringToSerialize));
		}
	}
	
	public void SerializeInt(int int32) {
		data.AddRange(BitConverter.GetBytes((int32)));
	}

	public void SerializeFloat(float float32) {
		data.AddRange(BitConverter.GetBytes((float32)));
	}
	
	public void SerializeVector3(Vector3 vector) {
		data.AddRange(BitConverter.GetBytes(vector.x));
		data.AddRange(BitConverter.GetBytes(vector.y));
		data.AddRange(BitConverter.GetBytes(vector.z));
	}

	public void SerializeQuaternion(Quaternion quaternion) {
		data.AddRange(BitConverter.GetBytes(quaternion.x));
		data.AddRange(BitConverter.GetBytes(quaternion.y));
		data.AddRange(BitConverter.GetBytes(quaternion.z));
		data.AddRange(BitConverter.GetBytes(quaternion.w));
	}
	
	public string ReadString() {
		int stringSize = ReadInt();
		if (stringSize > 0) {
			byte[] stringData = new byte[stringSize];
			Array.Copy(data.ToArray(), 0, stringData, 0, stringSize);
			string result = System.Text.Encoding.UTF8.GetString(stringData);
			data.RemoveRange(0, stringSize);
			return result;
		}
		return "";
	}
	
	public int ReadInt() {
		int result = BitConverter.ToInt32(data.ToArray(), 0);
		data.RemoveRange(0, 4);
		return result;
	}

	public float ReadFloat() {
		float result = BitConverter.ToSingle(data.ToArray(), 0);
		data.RemoveRange(0, 4);
		return result;
	}
	
	public Vector3 ReadVector3() {
		Vector3 vector3 = Vector3.zero;
		vector3.x = BitConverter.ToSingle(data.ToArray(), 0);
		vector3.y = BitConverter.ToSingle(data.ToArray(), 4);
		vector3.z = BitConverter.ToSingle(data.ToArray(), 8);
		data.RemoveRange(0, 12);
		return vector3;
	}

	public Quaternion ReadQuaternion() {
		Quaternion quaternion = Quaternion.identity;
		quaternion.x = BitConverter.ToSingle(data.ToArray(), 0);
		quaternion.y = BitConverter.ToSingle(data.ToArray(), 4);
		quaternion.z = BitConverter.ToSingle(data.ToArray(), 8);	
		quaternion.w = BitConverter.ToSingle(data.ToArray(), 12);
		data.RemoveRange(0, 16);
		return quaternion;
	}
	
}
