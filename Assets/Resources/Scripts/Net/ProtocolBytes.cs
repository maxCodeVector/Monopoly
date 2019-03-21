using System;
using System.Collections;
using System.Linq;



public class ProtocolBytes : ProtocolBase
{
	
	public byte[] bytes;
	
	public override ProtocolBase Decode(byte[] readbuff, int start, int length)
	{
		
		ProtocolBytes protocol = new ProtocolBytes();
		byte[] lenBytes = new byte[length + 4];
		for(int i=0;i<length;i++){
			lenBytes[4+i] = readbuff[start+i];
		}
		protocol.bytes = lenBytes;
		return protocol;
	}
	
	
	public override byte[] Encode()
	{
		byte[] lenBytes = this.bytes;
		Int32 i = lenBytes.Length - 4;
		lenBytes[0] = (byte)((i >> 24) & 0xFF);
        lenBytes[1] = (byte)((i >> 16) & 0xFF);
        lenBytes[2] = (byte)((i >> 8) & 0xFF);
        lenBytes[3] = (byte)(i & 0xFF);
		return bytes;
	}
	
	//协议名称
	public override string GetName()
	{
		return GetDesc().Split(',')[0].Trim();
	}
	
	//描述
	public override string GetDesc()
	{
		return System.Text.Encoding.UTF8.GetString(bytes, 4, bytes.Length-4);
	}


	//添加字符�?
	public void AddString(string str)
	{
		byte[] strBytes = System.Text.Encoding.UTF8.GetBytes (str);
		//Debug.Log("发送消�?: "+lenBytes[0]+""+lenBytes[1]+""+lenBytes[2]+""+lenBytes[3]);
		if(bytes == null)
			bytes = (new byte[4]).Concat(strBytes).ToArray();
		else
			bytes = bytes.Concat(strBytes).ToArray();
	}
	
	//从字节数组的start处开始读取字符串
	public string GetString(int start, ref int end)
	{
		if (bytes == null)
			return "";
		if (bytes.Length < start + sizeof(Int32))
			return "";
		Int32 strLen = BitConverter.ToInt32 (bytes, start);
		if (bytes.Length < start + sizeof(Int32) + strLen)
			return "";
		string str = System.Text.Encoding.UTF8.GetString(bytes,start + sizeof(Int32),strLen);
		end = start + sizeof(Int32) + strLen;
		return str;
	}
	
	public string GetString(int start)
	{
		int end = 0;
		return GetString (start, ref end);
	}



	public void AddInt(int num)
	{
		byte[] numBytes = BitConverter.GetBytes (num);
		if (bytes == null)
			bytes = numBytes;
		else
			bytes = bytes.Concat(numBytes).ToArray();
	}
	
	public int GetInt(int start, ref int end)
	{
		if (bytes == null)
			return 0;
		if (bytes.Length < start + sizeof(Int32))
			return 0;
		end = start + sizeof(Int32);
		return BitConverter.ToInt32(bytes, start);
	}
	
	public int GetInt(int start)
	{
		int end = 0;
		return GetInt (start, ref end);
	}


	public void AddFloat(float num)
	{
		byte[] numBytes = BitConverter.GetBytes (num);
		if (bytes == null)
			bytes = numBytes;
		else
			bytes = bytes.Concat(numBytes).ToArray();
	}
	
	public float GetFloat(int start, ref int end)
	{
		if (bytes == null)
			return 0;
		if (bytes.Length < start + sizeof(float))
			return 0;
		end = start + sizeof(float);
		return BitConverter.ToSingle(bytes, start);
	}
	
	public float GetFloat(int start)
	{
		int end = 0;
		return GetFloat (start, ref end);
	}

}