// (c) Copyright Cory Plotts.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

// Modified to suit Hawkeye needs (Olivier DALET)

#pragma once

__declspec(dllexport)
LRESULT __stdcall MessageHookProc(int nCode, WPARAM wparam, LPARAM lparam);

using namespace System;

namespace ManagedInjector
{
	public ref class Injector : Object
	{
		public:

		static void Launch(
			IntPtr windowHandle, IntPtr hawkeyeHandle, 
			String^ assemblyName, String^ className, String^ methodName, String^ logFileName);

		static void LogMessage(String^ logFileName, String^ message, bool append);
	};
}