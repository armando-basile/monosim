using System;
using Utility;

// Neded for [DllImport] 
using System.Runtime.InteropServices;

	
public class clsPCSC
{
	

	#region DllImport
	
	[DllImport("pcsc")]
	private static extern int SCardEstablishContext(uint dwScope, 
	                                                int nNotUsed1, 
	                                                int nNotUsed2, 
	                                            ref IntPtr phContext);

	[DllImport("pcsc")]
	private static extern int SCardListReaders(IntPtr hContext, 
	                                           string cGroups,
		                                       byte[] cReaderLists, 
		                                   out IntPtr nReaderCount);
	
	[DllImport("pcsc")]
	private static extern int SCardReleaseContext(IntPtr phContext);

	[DllImport("pcsc")]
	private static extern int SCardListReaderGroups(IntPtr hContext, 
	                                           	    System.Text.StringBuilder cGroups, 
	                                            out int nStringSize);

	[DllImport("pcsc")]
	private static extern int SCardConnect(IntPtr hContext, 
	                                      string cReaderName,
		                                  uint dwShareMode, 
		                                  uint dwPrefProtocol, 
		                              ref IntPtr phCard, 
		                              ref int ActiveProtocol);

	[DllImport("pcsc")]
	private static extern int SCardDisconnect(IntPtr hCard, 
	                                         int Disposition);


	[DllImport("pcsc")]
	private static extern int SCardStatus(IntPtr hCard, 
	                                     System.Text.StringBuilder ReaderName, 
	                                 ref int RLen, 
	                                 ref int State, 
	                                 ref int Protocol, 
	                                     byte[] ATR, 
	                                 ref int ATRLen);

	[DllImport("pcsc", SetLastError=true)]
	private static extern int SCardTransmit(IntPtr hCard, 
	                                    ref SCARD_IO_REQUEST pioSendPci,
	                                        byte[] pbSendBuffer, 
	                                        int cbSendLength, 
	                                        IntPtr pioRecvPci,
                                            byte[] pbRecvBuffer, 
                                        out int pcbRecvLength);

    [DllImport("pcsc")]             
    private static extern int SCardGetStatusChange(IntPtr hContext, 
                                                   uint dwTimeout, 
                                               ref SCard_ReaderState rgReaderStates,
                                                   int cReaders);
	
	#endregion DllImport

	
	
	#region Constants for PC/SC
			
	private const int SCARD_PROTOCOL_ANY = 3;
	private const int SCARD_PROTOCOL_RAW = 4;
	private const int SCARD_PROTOCOL_T0 = 1;
	private const int SCARD_PROTOCOL_T1 = 2;
	private const int SCARD_SHARE_DIRECT = 3;
	private const int SCARD_SHARE_EXCLUSIVE = 1;
	private const int SCARD_SHARE_SHARED = 2;
	private const int SCARD_SCOPE_USER = 0;
	private const int SCARD_SCOPE_TERMINAL = 1;
	private const int SCARD_SCOPE_SYSTEM = 2;
	private const int SCARD_LEAVE_CARD = 0;
	private const int SCARD_UNPOWER_CARD = 2;

	public const uint SCARD_STATE_UNAWARE = 0x0;
    public const uint SCARD_STATE_IGNORE = 0x1;
	public const uint SCARD_STATE_CHANGED = 0x2;
	public const uint SCARD_STATE_UNKNOWN = 0x4;
	public const uint SCARD_STATE_UNAVAILABLE = 0x8;
	public const uint SCARD_STATE_EMPTY = 0x10;
	public const uint SCARD_STATE_PRESENT = 0x20;
	public const uint SCARD_STATE_ATRMATCH = 0x40;
	public const uint SCARD_STATE_EXCLUSIVE = 0x80;
	public const uint SCARD_STATE_INUSE = 0x100;
	public const uint SCARD_STATE_MUTE = 0x200;
	public const uint SCARD_STATE_UNPOWERED = 0x400;
	
	#endregion Constants for PC/SC
	
	
	[StructLayout(LayoutKind.Sequential)]
	public struct	SCard_ReaderState
	{
	    public string szReader;
        public IntPtr pvUserData;
        public uint dwCurrentState;
        public uint dwEventState;
        public uint cbAtr;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
        public byte[] rgbAtr;
	}

	
	[StructLayout(LayoutKind.Sequential)]
	public struct SCARD_IO_REQUEST
	{
		public uint dwProtocol;
		public uint cbPciLength;
	}
	
	
	
	#region private vars
	
	private IntPtr nContext = IntPtr.Zero;			//Card reader context handle - DWORD
	private IntPtr nCard = IntPtr.Zero;				//Connection handle - DWORD
	private int nActiveProtocol = 0;	//T0/T1
	private int nNotUsed1 = 0;
	private int nNotUsed2 = 0;
	private string selectedReader = "";
	private int ReaderNameLen = 0; 
	private int Reader_State = 0; 
	private int Card_Protocol = 0; 
	private byte[] ATR_Value;
    private int ATR_Len = 33;
	private cEncoding utilityObj = new cEncoding();		
	
	#endregion private vars
	
	
	public clsPCSC()
	{
	}

	
	#region exported methods
	
	public string CreateContext()
	{

		// Delete previous context
		if (nContext.ToInt64() > 0)
			DeleteContext();

		// Call external function
		int ret = SCardEstablishContext(SCARD_SCOPE_SYSTEM, nNotUsed1, nNotUsed2, ref nContext);
		
		if (ret != 0)
			return parseError(ret);
			
		return "";
	}


	public string DeleteContext()
	{
		if (nContext.ToInt64() == 0)
			return "";
		
		// Call external function
		int ret = SCardReleaseContext(nContext);
		
		if (ret != 0)
			return parseError(ret);
		
		nContext = IntPtr.Zero;
		return "";
	}


	public string ListReaders(out string[] theReaders)
	{

		int ret;
		
		int nReadersCount = 0;
		IntPtr nReadersCountptr = IntPtr.Zero;
		string theListReaders = "";
		//System.Text.StringBuilder readers = null;
		byte[] readers = null;
		string[] cReaders = new string[0];
		
		// Preassign the output parameter
		theReaders = cReaders;
		
		//First time to retrieve the len of string for readers name
		ret = SCardListReaders(nContext, null, readers, out nReadersCountptr);
		
		if (ret != 0)
			return parseError(ret);
		
		nReadersCount = nReadersCountptr.ToInt32();
		
		// Create the container for list of readers
		readers = new byte[nReadersCount];
		
		ret = SCardListReaders(nContext, null, readers, out nReadersCountptr);

		if (ret != 0)
			return parseError(ret);
		
		int startR = 0;
		int stopR = 0;
		
		nReadersCount = nReadersCountptr.ToInt32();
		
		if (readers.Length < 5)
			return "";
		
		for (int j=0; j<readers.Length; j++)
		{
			if (readers[j] == 0)
			{
				theListReaders = theListReaders + utilityObj.getAsciiFromArray(readers,	startR, j-1);
				startR = j+1;
			
				if (readers[startR] == 0)
					break;
			
				theListReaders = theListReaders + Convert.ToChar(0);
			
			}
			
			
			
		
		}
		
		//theListReaders = readers.ToString();
				
		// Split the list in a string array
		char[] delimiter = new char[1];
		delimiter[0] = Convert.ToChar(0);
		
		cReaders = theListReaders.Split(delimiter);
		
		theReaders = cReaders;		
		return "";			

	}
    

	public string ConnectSmartCard(string theReaderName, ref string retError)
	{
		retError = "";
		
		// Disconnect before connect
		if (nCard.ToInt64() != 0)
			SCardDisconnect(nCard, SCARD_UNPOWER_CARD);
		
		Console.WriteLine("theReaderName = " + theReaderName);
		
		int ret = SCardConnect(nContext, theReaderName, SCARD_SHARE_SHARED, SCARD_PROTOCOL_T0 | SCARD_PROTOCOL_T1 , ref nCard, ref nActiveProtocol);
		
		if (ret != 0)
		{
			retError = parseError(ret);
			return "";
		}
		
		selectedReader = theReaderName;
		Console.WriteLine("nActiveProtocol = " + nActiveProtocol.ToString() );
		
		string myErr = "";
		string retStatus = ReaderStatus(ref myErr);
		if (myErr != "")
		{
			retError = myErr;
			return "";
		}
		
		return retStatus;
	}
	

	public void DisconnectSmartCard()
	{	
		SCardDisconnect(nCard, SCARD_UNPOWER_CARD);
		nCard = IntPtr.Zero;
		return;
	}

	
	public uint ReaderGetStatusChange(string readerName, ref string retError)
	{
	    SCard_ReaderState RState = new SCard_ReaderState();
	    RState.szReader = readerName + Convert.ToChar(0) ;
	    
	    int ret = SCardGetStatusChange(nContext, 50, ref RState, 1);
	    if (ret != 0)
		{
			retError = parseError(ret);
			Console.WriteLine("SCardGetStatusChange = " + retError);			
			return 0;
		}
	    
	    return RState.dwEventState;
	}
	
	
	public string ReaderStatus(ref string retError)
	{
		string strATR = "";
		ATR_Value = new byte[33];
		System.Text.StringBuilder retRName = new System.Text.StringBuilder(1024);		
		ReaderNameLen = 1024;
				
		int ret = SCardStatus(nCard, retRName, ref ReaderNameLen, ref Reader_State, ref Card_Protocol, ATR_Value, ref ATR_Len);
		
		if (ret != 0)
			ret = SCardStatus(nCard, retRName, ref ReaderNameLen, ref Reader_State, ref Card_Protocol, ATR_Value, ref ATR_Len);
		
		if (ret != 0)
		{
			retError = parseError(ret);
			Console.WriteLine("SCardStatus = " + retError);			
			return "";
		}
		
		Console.WriteLine("retRName = " + retRName.ToString());
		
		for (int i=0; i<ATR_Len; i++)
			strATR = strATR + ATR_Value[i].ToString("X2"); 
		
		return strATR;
	}

	
	public string SendCommand(string cardCommand, ref string retError )
	{
		string retCommand = "";
		retError = "";
		int retCommandLen = 261;

		string cardCommandTmp = cardCommand.Trim();
		
		byte[] inCommandByte = new byte[cardCommandTmp.Length/2];
		byte[] outCommandByte = new byte[261];
		

		
		int ret = utilityObj.getBytesFromHex(ref inCommandByte, cardCommand);
		
		if (ret != 0)
		{	
			retError = "Error returned...";
			Console.WriteLine("getBytesFromHex = " + ret.ToString());			
			return "";
		}
		
		SCARD_IO_REQUEST pioSend = new SCARD_IO_REQUEST();
		SCARD_IO_REQUEST pioRecv = new SCARD_IO_REQUEST();
		pioSend.dwProtocol = (uint)Card_Protocol;
		//pioSend.cbPciLength = (uint)inCommandByte.Length;
		pioSend.cbPciLength = (uint)8;
		pioRecv.dwProtocol = (uint)Card_Protocol;
		pioRecv.cbPciLength = 0;
		
		ret = SCardTransmit(nCard, 
		                ref pioSend, 
		                    inCommandByte, 		                    
		                    inCommandByte.Length, 
		                    IntPtr.Zero,
		                    outCommandByte, 
		                out retCommandLen);
		if (ret != 0)
		{	
			retError = parseError(ret);
			Console.WriteLine("SCardTransmit = " + retError);			
			return "";
		}
		
		retCommand = utilityObj.getHexFromBytes(outCommandByte, 0, retCommandLen);
		
		return retCommand;
	}


	private string parseError(int errorCode)
	{
		string hexError = string.Format("{0:x2}", errorCode);
		
		
		return hexError;
	}
	
	#endregion exported methods
	
	
	
	
	
	
	
	
	
	
	
	
}
