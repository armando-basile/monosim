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
	                                            out IntPtr nStringSize);

	[DllImport("pcsc")]
	private static extern int SCardConnect(IntPtr hContext, 
	                                      string cReaderName,
		                                  uint dwShareMode, 
		                                  uint dwPrefProtocol, 
		                              ref IntPtr phCard, 
		                              ref IntPtr ActiveProtocol);

	[DllImport("pcsc")]
	private static extern int SCardDisconnect(IntPtr hCard, 
	                                         int Disposition);


	[DllImport("pcsc")]
	private static extern int SCardStatus(IntPtr hCard, 
	                                     byte[] ReaderName, 
	                                 ref IntPtr RLen, 
	                                 ref int State, 
	                                 ref int Protocol, 
	                                     byte[] ATR, 
	                                 ref IntPtr ATRLen);

	[DllImport("pcsc", SetLastError=true)]
	private static extern int SCardTransmit(IntPtr hCard, 
	                                    ref SCARD_IO_REQUEST pioSendPci,
	                                        byte[] pbSendBuffer, 
	                                        int cbSendLength, 
	                                        IntPtr pioRecvPci,
                                            byte[] pbRecvBuffer, 
                                        out IntPtr pcbRecvLength);

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
	private IntPtr nActiveProtocol = new IntPtr(0);	//T0/T1
	private int nNotUsed1 = 0;
	private int nNotUsed2 = 0;
	private string selectedReader = "";
	//private int ReaderNameLen = 0; 
	private IntPtr ReaderNameLen = new IntPtr(0); 
	private int Reader_State = 0; 
	private int Card_Protocol = 0; 
	private byte[] ATR_Value;
    //private int ATR_Len = 33;
	private IntPtr ATR_Len = new IntPtr(33);
	private cEncoding utilityObj = new cEncoding();		

	#endregion private vars
	
	public bool DebugModeStatus = false;
	
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
		// System.Text.StringBuilder retRName = new System.Text.StringBuilder(1024);		
		byte[] retRName = new byte[64];
		string retRNameSTR = "";
		ReaderNameLen = new IntPtr(64);
				
		int ret = SCardStatus(nCard, retRName, ref ReaderNameLen, ref Reader_State, ref Card_Protocol, ATR_Value, ref ATR_Len);
		
		retRNameSTR = utilityObj.getAsciiFromArray(retRName);
		
		if (DebugModeStatus)
		{
			Console.WriteLine("\r\nATR_Value.Length = " + ATR_Value.Length.ToString());
			Console.WriteLine("retRName = " + retRNameSTR);
			Console.WriteLine("ReaderNameLen = " + ReaderNameLen.ToString());
			Console.WriteLine("ATR_Len = " + ATR_Len.ToString());
			Console.WriteLine("SCardStatus = " + parseError(ret));
		}
		
		if (ret != 0)
		{
			ATR_Value = new byte[ATR_Len.ToInt32()];
			// retRName = new System.Text.StringBuilder(ReaderNameLen);
			retRName = new byte[ReaderNameLen.ToInt32()];
			ret = SCardStatus(nCard, retRName, ref ReaderNameLen, ref Reader_State, ref Card_Protocol, ATR_Value, ref ATR_Len);

			retRNameSTR = utilityObj.getAsciiFromArray(retRName);
			
			if (DebugModeStatus)
			{
				Console.WriteLine("\r\n2 STEP\r\nATR_Value.Length = " + ATR_Value.Length.ToString());
				Console.WriteLine("retRName = " + retRNameSTR);
				Console.WriteLine("ReaderNameLen = " + ReaderNameLen.ToString());
				Console.WriteLine("ATR_Len = " + ATR_Len.ToString());
				Console.WriteLine("SCardStatus = " + parseError(ret));
			}
		}
		
		if (ret != 0)
		{
			retError = parseError(ret);
			Console.WriteLine("\r\nSCardStatus = " + retError);			
			return "";
		}
		
		Console.WriteLine("retRName = " + retRNameSTR);
		
		for (int i=0; i<ATR_Len.ToInt32() ; i++)
			strATR = strATR + ATR_Value[i].ToString("X2"); 
		
		return strATR;
	}

	
	public string SendCommand(string cardCommand, ref string retError )
	{
		string retCommand = "";
		retError = "";
		IntPtr retCommandLen = new IntPtr(261);

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
		
		retCommand = utilityObj.getHexFromBytes(outCommandByte, 0, retCommandLen.ToInt32());
		
		return retCommand;
	}


	private string parseError(int errorCode)
	{
		string hexError = string.Format("{0:x2}", errorCode);
		hexError = hexError.ToUpper();
		
		if (hexError == "80100001") {hexError += " - SCARD_F_INTERNAL_ERROR"; }
		if (hexError == "80100002") {hexError += " - SCARD_E_CANCELLED"; }
		if (hexError == "80100003") {hexError += " - SCARD_E_INVALID_HANDLE"; }
		if (hexError == "80100004") {hexError += " - SCARD_E_INVALID_PARAMETER"; }
		if (hexError == "80100005") {hexError += " - SCARD_E_INVALID_TARGET"; }
		if (hexError == "80100006") {hexError += " - SCARD_E_NO_MEMORY"; }
		if (hexError == "80100007") {hexError += " - SCARD_F_WAITED_TOO_LONG"; }
		if (hexError == "80100008") {hexError += " - SCARD_E_INSUFFICIENT_BUFFER"; }
		if (hexError == "80100009") {hexError += " - SCARD_E_UNKNOWN_READER"; }
		if (hexError == "8010000A") {hexError += " - SCARD_E_TIMEOUT"; }
		if (hexError == "8010000B") {hexError += " - SCARD_E_SHARING_VIOLATION"; }
		if (hexError == "8010000C") {hexError += " - SCARD_E_NO_SMARTCARD"; }
		if (hexError == "8010000D") {hexError += " - SCARD_E_UNKNOWN_CARD"; }
		if (hexError == "8010000E") {hexError += " - SCARD_E_CANT_DISPOSE"; }
		if (hexError == "8010000F") {hexError += " - SCARD_E_PROTO_MISMATCH"; }
		if (hexError == "80100010") {hexError += " - SCARD_E_NOT_READY"; }
		if (hexError == "80100011") {hexError += " - SCARD_E_INVALID_VALUE"; }
		if (hexError == "80100012") {hexError += " - SCARD_E_SYSTEM_CANCELLED"; }
		if (hexError == "80100013") {hexError += " - SCARD_F_COMM_ERROR"; }
		if (hexError == "80100014") {hexError += " - SCARD_F_UNKNOWN_ERROR"; }
		if (hexError == "80100015") {hexError += " - SCARD_E_INVALID_ATR"; }
		if (hexError == "80100016") {hexError += " - SCARD_E_NOT_TRANSACTED"; }
		if (hexError == "80100017") {hexError += " - SCARD_E_READER_UNAVAILABLE"; }
		if (hexError == "80100018") {hexError += " - SCARD_P_SHUTDOWN"; }
		if (hexError == "80100019") {hexError += " - SCARD_E_PCI_TOO_SMALL"; }
		if (hexError == "8010001A") {hexError += " - SCARD_E_READER_UNSUPPORTED"; }
		if (hexError == "8010001B") {hexError += " - SCARD_E_DUPLICATE_READER"; }
		if (hexError == "8010001C") {hexError += " - SCARD_E_CARD_UNSUPPORTED"; }
		if (hexError == "8010001D") {hexError += " - SCARD_E_NO_SERVICE"; }
		if (hexError == "8010001E") {hexError += " - SCARD_E_SERVICE_STOPPED"; }
		if (hexError == "8010001F") {hexError += " - SCARD_E_UNEXPECTED"; }
		if (hexError == "80100020") {hexError += " - SCARD_E_ICC_INSTALLATION"; }
		if (hexError == "80100021") {hexError += " - SCARD_E_ICC_CREATEORDER"; }
		if (hexError == "80100022") {hexError += " - SCARD_E_UNSUPPORTED_FEATURE"; }
		if (hexError == "80100023") {hexError += " - SCARD_E_DIR_NOT_FOUND"; }
		if (hexError == "80100024") {hexError += " - SCARD_E_FILE_NOT_FOUND"; }
		if (hexError == "80100025") {hexError += " - SCARD_E_NO_DIR"; }
		if (hexError == "80100026") {hexError += " - SCARD_E_NO_FILE"; }
		if (hexError == "80100027") {hexError += " - SCARD_E_NO_ACCESS"; }
		if (hexError == "80100028") {hexError += " - SCARD_E_WRITE_TOO_MANY"; }
		if (hexError == "80100029") {hexError += " - SCARD_E_BAD_SEEK"; }
		if (hexError == "8010002A") {hexError += " - SCARD_E_INVALID_CHV"; }
		if (hexError == "8010002B") {hexError += " - SCARD_E_UNKNOWN_RES_MNG"; }
		if (hexError == "8010002C") {hexError += " - SCARD_E_NO_SUCH_CERTIFICATE"; }
		if (hexError == "8010002D") {hexError += " - SCARD_E_CERTIFICATE_UNAVAILABLE"; }
		if (hexError == "8010002E") {hexError += " - SCARD_E_NO_READERS_AVAILABLE"; }
		if (hexError == "8010002F") {hexError += " - SCARD_E_COMM_DATA_LOST"; }
		
		if (hexError == "80100065") {hexError += " - SCARD_W_UNSUPPORTED_CARD"; }
		if (hexError == "80100066") {hexError += " - SCARD_W_UNRESPONSIVE_CARD"; }
		if (hexError == "80100067") {hexError += " - SCARD_W_UNPOWERED_CARD"; }
		if (hexError == "80100068") {hexError += " - SCARD_W_RESET_CARD"; }
		if (hexError == "80100069") {hexError += " - SCARD_W_REMOVED_CARD"; }
		if (hexError == "8010006A") {hexError += " - SCARD_W_SECURITY_VIOLATION"; }
		if (hexError == "8010006B") {hexError += " - SCARD_W_WRONG_CHV"; }
		if (hexError == "8010006C") {hexError += " - SCARD_W_CHV_BLOCKED"; }
		if (hexError == "8010006D") {hexError += " - SCARD_W_EOF"; }
		if (hexError == "8010006E") {hexError += " - SCARD_W_CANCELLED_BY_USER"; }
		
		
		return hexError;
	}
	
	#endregion exported methods
	
	
	
	
	
	
	
	
	
	
	
	
}
