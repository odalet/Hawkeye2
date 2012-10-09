#include "StdAfx.h"
#include "logging.h"

using namespace System;
using namespace System::IO;
using namespace System::Text;
using namespace System::Threading;
using namespace System::Diagnostics;

namespace HawkeyeInjector
{
#pragma region SimpleLogService

	/// <summary>Constructor.</summary>
	/// <param name="filename">The name of the file to log to.</param>
	/// <param name="type">The type to log.</param>
	/// <param name="method">The method to log.</param>
	SimpleLogService::SimpleLogService(String^ filename, Type^ type, String^ method)
	{
		FileName = filename;
		TypeName = type == nullptr ? "<global>" : type->ToString();
		MethodName = method;
	}

	/// <summary>
	/// Logs the specified message with the 'Error' level.
	/// </summary>
	/// <param name="message">The message.</param>
	void SimpleLogService::Error(String^ message)
	{
		Log(LogLevel::Error, message);
	}

	/// <summary>
	/// Logs the specified message with the 'Info' level.
	/// </summary>
	/// <param name="message">The message.</param>
	void SimpleLogService::Info(String^ message)
	{
		Log(LogLevel::Info, message);
	}

	/// <summary>
	/// Logs the specified message with the 'Debug' level.
	/// </summary>
	/// <param name="message">The message.</param>
	void SimpleLogService::Debug(String^ message)
	{
		Log(LogLevel::Debug, message);
	}

	/// <summary>
	/// Logs the specified message with the specified level.
	/// </summary>
	/// <param name="level">The log level.</param>
	/// <param name="message">The message.</param>
	void SimpleLogService::Log(LogLevel level, String^ message)
	{
		StringBuilder^ sb = gcnew StringBuilder();
		sb->Append(DateTime::Now.ToString("yyyy-MM-dd HH:MM:ss,fff"));
		sb->AppendFormat(" [{0},{1}]", Process::GetCurrentProcess()->Id, Thread::CurrentThread->ManagedThreadId);
		sb->AppendFormat(" {0} - ", level.ToString()->ToUpperInvariant()->PadRight(5));
		sb->AppendFormat(" {0}.{1} - ", TypeName, MethodName);
		sb->Append(message);
		String^ text = sb->ToString();
		Debug::WriteLine(text);

		FileInfo^ fi = gcnew FileInfo(FileName);
		StreamWriter^ sw = nullptr;
		try
		{
			sw = fi->AppendText();
			sw->WriteLine(text);
		}
		finally 
		{
			if (sw != nullptr) delete sw;
		}
	}

#pragma endregion

#pragma region SimpleLogManager

	void SimpleLogManager::Initialize(String^ filename)
	{
		if (initialized) return;

		if (String::IsNullOrEmpty(filename))
			filename = defaultFileName;

		if (Path::IsPathRooted(filename))
			theFileName = filename;
		else
		{
			String^ hawkeyeDirectory = Path::Combine(
				Environment::GetFolderPath(Environment::SpecialFolder::CommonApplicationData),
				"Hawkeye");
			if (!Directory::Exists(hawkeyeDirectory))
				Directory::CreateDirectory(hawkeyeDirectory);
			
			String^ logFileDirectory = Path::Combine(hawkeyeDirectory, "logs");
            if (!Directory::Exists(logFileDirectory))
                Directory::CreateDirectory(logFileDirectory);

			theFileName = Path::Combine(logFileDirectory, filename);
		}

		if (File::Exists(theFileName))
		{
			FileInfo^ fi = gcnew FileInfo(theFileName);
			DateTime lastWrite = fi->LastWriteTimeUtc;
			TimeSpan duration = DateTime::UtcNow.Subtract(lastWrite);

			if (duration.TotalHours >= 24.0) // We keep maximum 24h
				File::Delete(theFileName);
		}

		initialized = true;
	}

	SimpleLogService^ SimpleLogManager::GetLogger(Type^ type, String^ method)
	{
		Initialize(nullptr);
		return gcnew SimpleLogService(theFileName, type, method);
	}

#pragma endregion
}