using System;
using System.IO;

namespace uisetup
{
	class MainClass
	{
		// ATTRIBUTI
		static UiStreamer uis = new UiStreamer();
		static Arguments AppArgs;
		
		
		
		public static void Main(string[] args)
		{
			
			string helpMessage = 
				"use one of this command lines type:\r\n\r\n" + 
                "uisetup -if <file to manage> -of <file to produce>\r\n" + 
                "uisetup -id <folder to manage> -od <folder where write output>\r\n\r\n";

			// if there aren't arguments, generate help message
			if (args.Length < 1)
			{
				Console.WriteLine(helpMessage);
				return;
			}
			
			// fill Arguments object
			AppArgs = new Arguments(args);
			
			if ((AppArgs["if"] != null) && (AppArgs["of"] != null))
			{
				// single file
				parseFile(AppArgs["if"].ToString(), AppArgs["of"].ToString());
				
			}
			else if ((AppArgs["id"] != null) && (AppArgs["od"] != null))
			{
				// folder
				parseFolder(AppArgs["id"].ToString(), AppArgs["od"].ToString());
				
			}
			else
			{
				// wrong command, generate help message
				Console.WriteLine(helpMessage);
				return;
				
			}

		}
		
		
		/// <summary>
		/// manage file and produce output
		/// </summary>
		static bool parseFile(string inputPath, string outputPath)
		{	
			
			inputPath = Path.GetFullPath(inputPath);
			outputPath = Path.GetFullPath (outputPath);
			
			if (Path.GetExtension(inputPath).ToLower() == ".ui")
			{
				// User Interface File
				return uis.ManageUiFile(inputPath, outputPath);
			
			}
			else if (Path.GetExtension(inputPath).ToLower() == ".qrc")
			{
				// Qt Resource File
				return uis.ManageQrcFile(inputPath, outputPath);
			
			}
			
			return true;
		}
		
		
		

		/// <summary>
		/// Parse all files in folder
		/// </summary>
		static void parseFolder(string inputPath, string outputPath)
		{
			DirectoryInfo di = new DirectoryInfo(inputPath);
			string tmpOutFile;
			
			foreach (FileInfo dif in di.GetFiles()) 
			{
				tmpOutFile = Path.GetFileNameWithoutExtension(dif.FullName) + ".cs";
				tmpOutFile = outputPath + Path.DirectorySeparatorChar + tmpOutFile;
				
				if (parseFile(dif.FullName, tmpOutFile) != true)
				{
					return;
					
				}
			}			
		}
		
		
		
		
		
		
	}
}