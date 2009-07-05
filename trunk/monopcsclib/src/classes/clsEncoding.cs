using System;
using System.Text;

namespace Utility
{
	/// <summary>
	/// Class to manage bytes array and hexadecimal strings.
	/// </summary>
	public class cEncoding
	{

		/// <summary>
		/// Create an instance of object
		/// </summary>
		public cEncoding()
		{
		}
		
		/// <summary>
		///	Function to obtain an hexadecimal string (0x00 format)
		/// from a bytes array.
		/// </summary>
		/// <param name="inBytes">Bytes array to encoding</param>
		public string getHexFromBytes(byte[] inBytes)
		{
			int j;
			string tmpHexOut = "";
			
			if (inBytes.Length == 0 )
				return "";
			
			for (j=0; j<inBytes.Length; j++)
				tmpHexOut = tmpHexOut + inBytes[j].ToString("X2");
			
			return tmpHexOut;
		
		}
		
		/// <summary>
		///	Function to obtain an hexadecimal string (0x00 format)
		/// from a bytes array.
		/// </summary>
		/// <param name="inBytes">Bytes array to encoding</param>
		/// <param name="offSet">OffSet to start encoding</param>
		/// <param name="len">Encoding length</param>
		public string getHexFromBytes(byte[] inBytes, int offSet, int len)
		{
			int j;
			string tmpHexOut = "";
			
			if (inBytes.Length < (offSet + len) )
				return "";
			
			for (j=offSet; j<(offSet + len); j++)
				tmpHexOut = tmpHexOut + inBytes[j].ToString("X2");
			
			return tmpHexOut;
		
		}

		
		/// <summary>
		///	Function to obtain an hexadecimal string (0x00 format)
		/// from an Hexadecimal string
		/// </summary>
		/// <param name="inHex">string hexadecimal to swap</param>
		public string swapHex(string inHex)
		{
			int j;
			string tmpHexOut = "";
			
			if (inHex.Length == 0 )
				return "";
			
			for (j=(inHex.Length-2); j>=0; j=j-2)
			{
				tmpHexOut = tmpHexOut + inHex.Substring(j,2) ;
			}
			
			return tmpHexOut;
		
		}

		/// <summary>
		///	Function to obtain an hexadecimal string (0x00 format)
		/// from an Ascii string
		/// </summary>
		public string getHexFromAscii(string inAscii)
		{
			int j;
			string tmpHexOut = "";
			char[] tmpStep;
			
			if (inAscii.Length == 0 )
				return "";
			
			for (j=0; j<inAscii.Length; j++)
			{
				tmpStep = inAscii.Substring(j,1).ToCharArray();
				tmpHexOut = tmpHexOut + Convert.ToInt32(tmpStep[0]).ToString("X2");
			}
			
			return tmpHexOut;
		
		}


		/// <summary>
		///	Function to obtain an ASCII string 
		/// from an hexadecimal string.
		/// </summary>
		public string getAsciiFromHex(string inHexString)
		{
			int j, n;
			string tmpAsciiOut = "";
			
			if (inHexString.Length == 0 )
				return "";
			
			for (j=0; j<inHexString.Length; j=j+2)
			{
				n = Convert.ToInt32(inHexString.Substring(j,2), 16);
				tmpAsciiOut = tmpAsciiOut + (char)n;
			}
				
			return tmpAsciiOut;
		
		}
		

		/// <summary>
		///	Function to obtain an Hex String from int value 
		/// </summary>
		public string getHexFromInt(int inValue)
		{
			int j;
			string tmpHexOut = "";
			tmpHexOut = inValue.ToString("X8");
			
			return tmpHexOut;
		
		}


		/// <summary>
		///	Function to obtain an ASCII string 
		/// from bytes array.
		/// </summary>
		public string getAsciiFromArray(byte[] inData)
		{
			int j, n;
			string tmpAsciiOut = "";
			
			if (inData.Length == 0 )
				return "";
			
			for (j=0; j<inData.Length; j++)
			{
				if (inData[j] != 0)
					tmpAsciiOut = tmpAsciiOut + (char)inData[j];
			}
				
			return tmpAsciiOut;
		
		}


		/// <summary>
		///	Function to obtain an ASCII string 
		/// from bytes array. (from start to end)
		/// </summary>
		public string getAsciiFromArray(byte[] inData, int StartOffSet, int StopOffSet)
		{
			int j, n;
			string tmpAsciiOut = "";
			
			if (inData.Length == 0 )
				return "";
			
			for (j=StartOffSet; j<=StopOffSet; j++)
			{
				if (inData[j] != 0)
					tmpAsciiOut = tmpAsciiOut + (char)inData[j];
			}
				
			return tmpAsciiOut;
		
		}
		
		/// <summary>
		///	Function to obtain an Integer value from Bytes array
		/// </summary>
		public int getIntFromBytes(byte[] inBArray, int startOffSet, int length)
		{
			int j, n;
			n = 0;
			
			if (inBArray.Length == 0 )
				return 0;
			
			for (j=startOffSet; j<(length + startOffSet); j++)
				n = n + (inBArray[j]<<( 8*(j-startOffSet) ) );
				
			return n;
		
		}


		/// <summary>
		///	Function to obtain a Bytes array from an Hexadecimal string
		/// </summary>
		public int getBytesFromHex(ref byte[] outBArray, string inData)
		{
			int j, n;
			n = 0;
			inData = inData.Trim();
			
			if (inData.Length == 0 )
			{
				outBArray = null;
				return -1;
			}
			
			int inLen = (inData.Length/2);
			
			for (j=0; j<inData.Length-1; j+=2)
				outBArray[j/2] = Byte.Parse(inData.Substring(j,2) ,System.Globalization.NumberStyles.HexNumber);
			
			//outBArray = outBArrayTemp;
			return 0;
		
		}


		/// <summary>
		///	Compare two array 
		/// </summary>
		public int compareBytesArray(byte[] bigArray, byte[] smallArray, int offSet)
		{
			int len1 = bigArray.Length;
			int len2 = smallArray.Length;
			int k, j;
			bool okCompare=false;
			
			for (k=offSet; k<(len1-len2); k++)
			{
				okCompare=true;
				for (j=0; j<len2; j++)
				{
					if (bigArray[k+j] != smallArray[j])
					{
						okCompare=false;
						break;					
					}
					
				}
				
				if (okCompare==true)	return k;
			
			}
			
			return -1;
			
		
		}
		
		
	}
}
