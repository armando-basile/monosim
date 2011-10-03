
using System;
using System.IO;
using System.Diagnostics;

namespace uisetup
{
		
	public class UiStreamer
	{
		// ATTRIBUTI
		StreamReader sr;
		StreamWriter sw;
		
		
		public UiStreamer()
		{
		}
		
		
		
		public bool ManageUiFile(string inputFile, string outputFile)
		{
			Console.WriteLine("Manage UI  file: " + Path.GetFileName(inputFile));
			
			// remove xml tag from ui file
			RemoveXmlTag(inputFile);
			
			// rewrite .cs file to manage associated ui file
			return RewriteUiClasses(inputFile, outputFile);
			
		}
		

		
		public bool ManageQrcFile(string inputFile, string outputFile)
		{
			Console.WriteLine("Manage QRC file: " + Path.GetFileName(inputFile));
			
			// rewrite .cs file to manage associated qrc file
			return UpdateResourceClass(inputFile, outputFile);
			
		}
		
		
		
		/// <summary>
		/// Remove xml initial tag from ui file
		/// </summary>
		private void RemoveXmlTag(string p_UiFilePath)
		{
			sr = new StreamReader(p_UiFilePath);
			string firstLine = sr.ReadLine();
			string oldUi = sr.ReadToEnd();
			
			if (firstLine.IndexOf("<?xml") < 0)
			{
				oldUi = firstLine + "\r\n" + oldUi;
			}
			
			sr.Close();
			sr.Dispose();
			sr = null;
			
			sw = new StreamWriter(p_UiFilePath, false);
			
			sw.Write(oldUi);
			
			sw.Close();
			sw.Dispose();
			sw = null;
			
		}
		
		
		
		
		private bool RewriteUiClasses(string p_UiFilePath, string p_UiOutFilePath)
		{
			string outBuffer, errBuffer= "";
			
			Process uicsProcess = new Process();
			StreamWriter sw;
			StreamReader sr;
			StreamReader err;
			
			ProcessStartInfo proc = 
				new ProcessStartInfo("uics");

			proc.Arguments = "-o \"" + p_UiOutFilePath + "\" \"" + p_UiFilePath + "\"";
			
			proc.UseShellExecute = false;
			proc.RedirectStandardInput = true;
			proc.RedirectStandardOutput = true;
			proc.RedirectStandardError = true;
			proc.CreateNoWindow = true;
			uicsProcess.StartInfo = proc;
			
			uicsProcess.Start();
			sw = uicsProcess.StandardInput;
			sr = uicsProcess.StandardOutput;
			err = uicsProcess.StandardError;
			
			sw.AutoFlush = true;
			
			outBuffer = sr.ReadToEnd().Trim();
			errBuffer = err.ReadToEnd().Trim();
			
			
			sw.Close();
			
			if ((outBuffer != "") || (errBuffer != ""))
			{
				Console.WriteLine("COMMAND: uics " + "-o \"" + p_UiOutFilePath + "\" \"" + p_UiFilePath + "\"");
				Console.WriteLine("OUTPUT:" + outBuffer);
				Console.WriteLine("ERROR:" + errBuffer);
				return false;
			}
			
			return true;
			
		}
		
		
		
		
		private bool UpdateResourceClass(string p_QrcFilePath, string p_QrcOutFilePath)
		{

			string outBuffer, errBuffer= "";			
			string fileNameWithExtention = Path.GetFileNameWithoutExtension(p_QrcFilePath);
			string fileName = Path.GetFileName(p_QrcFilePath);
			string qrcFolder = Path.GetDirectoryName(p_QrcFilePath);
			
			Process csrccProcess = new Process();
			StreamWriter sw;
			StreamReader sr;
			StreamReader err;
			
			ProcessStartInfo proc = 
				new ProcessStartInfo("csrcc");
			
			proc.Arguments = fileName + " -name " + fileNameWithExtention + " -o " + p_QrcOutFilePath;

			proc.UseShellExecute = false;
			proc.RedirectStandardInput = true;
			proc.RedirectStandardOutput = true;
			proc.RedirectStandardError = true;
			proc.CreateNoWindow = true;
			proc.WorkingDirectory = qrcFolder;
			//Console.WriteLine("Set WorkingDirectory = " + qrcFolder);
			
			csrccProcess.StartInfo = proc;			
			csrccProcess.Start();
			
			sw = csrccProcess.StandardInput;
			sr = csrccProcess.StandardOutput;
			err = csrccProcess.StandardError;
			
			sw.AutoFlush = true;
			
			outBuffer = sr.ReadToEnd().Trim();
			errBuffer = err.ReadToEnd().Trim();
			
			sw.Close();
			
			if ((outBuffer != "") || (errBuffer != ""))
			{
				Console.WriteLine("COMMAND: csrcc " + fileName + " -name " + fileNameWithExtention + " > " + fileName);
				Console.WriteLine("OUTPUT:" + outBuffer);
				Console.WriteLine("ERROR:" + errBuffer);
				return false;
			}
			
			return true;

			
		}
		
		
		
		
		
	}
}
