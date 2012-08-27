#pragma once

using namespace System;

namespace HawkeyeInjector {

	public ref class SimpleLogService
	{
	public:
		SimpleLogService(String^ filename, Type^ type, String^ method);

		void Error(String^ message);
		void Info(String^ message);
		void Debug(String^ message);

	private:
		enum class LogLevel
		{
			Error = 0,
			Info = 1,
			Debug = 2
		};

		property String^ FileName;
		property String^ TypeName;
		property String^ MethodName;

		void Log(LogLevel level, String^ message);
	};

	/// <summary>
	/// Factory of <see cref="SimpleLogService"/> objects.
	/// </summary>
	public ref class SimpleLogManager
	{
	public:
		static void Initialize(String^ filename);
		static SimpleLogService^ GetLogger(Type^ type, String^ method);

	private:
		static initonly String^ defaultFileName = "HawkeyeInjector.log";
		static bool initialized = false;
		static String^ theFileName; 
	};
}
