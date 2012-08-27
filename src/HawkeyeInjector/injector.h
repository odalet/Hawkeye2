// (c) Copyright Cory Plotts.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

// Modified to suit Hawkeye needs (Olivier DALET)

#pragma once
#include "logging.h"

__declspec(dllexport)
	LRESULT __stdcall MessageHookProc(int nCode, WPARAM wparam, LPARAM lparam);

using namespace System;

namespace HawkeyeInjector
{
	public ref class InjectorParameters
	{
	public:
		InjectorParameters(array<String^>^ arguments);

		/// <summary>Gets or sets the handle of the window to spy.</summary>
		property IntPtr WindowHandle;

		/// <summary>Gets the handle of the Hawkeye original window.</summary>
		property IntPtr OriginalHandle;

		/// <summary>Gets the name of the assembly containing the code to execute when injected.</summary>
		property String^ AssemblyName;

		/// <summary>Gets the name of the class containing the code to execute when injected.</summary>
		property String^ ClassName;

		/// <summary>Gets the name of the method to invoke when injected.</summary>
		property String^ MethodName;

		array<String^>^ ToStringArray();

		virtual String^ ToString() override;
	};

	public ref class Injector : Object
	{
	public:
		static void Launch(InjectorParameters^ parameters);

	private:
		static void LogLastError(SimpleLogService^ log, String^ message);
	};
}