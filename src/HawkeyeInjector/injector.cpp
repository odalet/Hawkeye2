// (c) Copyright Cory Plotts.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

// Modified to suit Hawkeye needs (Olivier DALET)

#include "stdafx.h"

#include "injector.h"
#include "logging.h"
#include <vcclr.h>
#include <msclr\marshal_cppstd.h>

using namespace System;
using namespace System::IO;
using namespace System::Reflection;
using namespace System::Diagnostics;
using namespace HawkeyeInjector;

static unsigned int WM_GOBABYGO = ::RegisterWindowMessage(L"HawkeyeInjector_GOBABYGO!");
static HHOOK _messageHookHandle;

/// <summary>
/// The hook.
/// </summary>
/// <param name="code">The code.</param>
/// <param name="wparam">The wparam.</param>
/// <param name="lparam">The lparam.</param>
/// <returns></returns>
__declspec(dllexport)
	LRESULT __stdcall MessageHookProc(int code, WPARAM wparam, LPARAM lparam)
{
	LRESULT result;
	try
	{	
		if (code == HC_ACTION)
		{
			CWPSTRUCT* msg = (CWPSTRUCT*)lparam;
			if (msg != NULL && msg->message == WM_GOBABYGO)
			{
				SimpleLogService^ log = SimpleLogManager::GetLogger(Injector::typeid, "MessageHookProc");	
				log->Debug("Got WM_GOBABYGO message");

				wchar_t* acmRemote = (wchar_t*)msg->wParam;

				String^ acmLocal = gcnew String(acmRemote);
				log->Debug(String::Format("acmLocal = {0}", acmLocal));
				InjectorParameters^ parameters = gcnew InjectorParameters(acmLocal->Split('$'));
				log->Debug(String::Format("parameters = {0}", parameters));

				log->Debug(String::Format("About to load assembly {0}", parameters->AssemblyName));
				Assembly^ assembly = Assembly::LoadFile(parameters->AssemblyName);
				if (assembly != nullptr)
				{
					log->Debug(String::Format("About to load type {0}", parameters->ClassName));
					Type^ type = assembly->GetType(parameters->ClassName);
					if(type != nullptr)
					{
						log->Debug(String::Format("Just loaded the type {0}; now looking for method {1}", 
							parameters->ClassName, parameters->MethodName));
						System::Reflection::BindingFlags flags = 
							System::Reflection::BindingFlags::Static | 
							System::Reflection::BindingFlags::Public | 
							System::Reflection::BindingFlags::NonPublic;
						MethodInfo^ methodInfo = type->GetMethod(parameters->MethodName, flags);
						if(methodInfo != nullptr)
						{
							log->Debug(String::Format("About to invoke {0}.{1}", 
								parameters->ClassName, parameters->MethodName));
							try
							{
								Object^ returnValue = methodInfo->Invoke(nullptr, gcnew array<Object^>(2) 
								{ 
									parameters->WindowHandle, parameters->OriginalHandle 
								});
								log->Debug(String::Format("Return value of {0} on type {1} is: {2}", 
									methodInfo->Name, parameters->ClassName, 
									returnValue == nullptr ? "null" : returnValue->ToString()));
							}
							catch (Exception^ ex)
							{
								log->Error(String::Format("Could not invoke {0}.{1}: {2}\r\n{3}", 
									parameters->ClassName, parameters->MethodName, ex->Message, ex));
							}
						}
						else log->Error(String::Format("Could not find method {0} in type {1}", 
							parameters->MethodName, parameters->ClassName));
					}
					else log->Error(String::Format("Could not find type {0} in assembly {1}", 
						parameters->ClassName, parameters->AssemblyName));
				}
				else log->Error(String::Format("Could not load assembly from {0}", parameters->AssemblyName));
			}
		}
	}
	finally
	{
		result = ::CallNextHookEx(_messageHookHandle, code, wparam, lparam);
	}

	return result;
}

namespace HawkeyeInjector
{
	//-----------------------------------------------------------------------------
	// Injector parameters class
	//-----------------------------------------------------------------------------

	/// <summary>
	/// Initializes a new instance of the <see cref="InjectorParameters" /> class.
	/// </summary>
	/// <param name="arguments">The array of arguments.</param>
	/// <remarks>
	/// 5 arguments must be passed:
	/// <list type="number">
	/// <item>The handle of the window to spy (must be convertible to a <see cref="IntPtr"/>).</item>
	/// <item>The handle of the Hawkeye original window (must be convertible to a <see cref="IntPtr"/>).</item>
	/// <item>The name of the assembly containing the code to execute when injected.</item>
	/// <item>The name of the class containing the code to execute when injected.</item>
	/// <item>The name of the method to invoke when injected.</item>
	/// </list>
	/// </remarks>
	/// <exception cref="System.ArgumentNullException"></exception>
	/// <exception cref="System.ArgumentException"></exception>
	InjectorParameters::InjectorParameters(array<String^>^ arguments)
	{
		if (arguments == nullptr)
			throw gcnew ArgumentNullException("arguments");

		if (arguments->Length < 5)
			throw gcnew ArgumentException("Wrong number of arguments: 5 values are expected.", "arguments");

		WindowHandle = IntPtr(Int64::Parse(arguments[0]));
		OriginalHandle = IntPtr(Int64::Parse(arguments[1]));
		AssemblyName = arguments[2];
		ClassName = arguments[3];
		MethodName = arguments[4];
	}

	/// <summary>
	/// Returns a <see cref="System.String" /> that represents this instance.
	/// </summary>
	/// <returns>
	/// A <see cref="System.String" /> that represents this instance.
	/// </returns>
	String^ InjectorParameters::ToString()
	{
		return WindowHandle.ToString() + "$" + OriginalHandle.ToString() + "$" +
			AssemblyName + "$" + ClassName + "$" + MethodName;
	}

	array<String^>^ InjectorParameters::ToStringArray()
	{
		return gcnew array<String^>(5)		
		{ 
			WindowHandle.ToString(), OriginalHandle.ToString(), 
				AssemblyName, ClassName, MethodName 
		};
	}
	

	//-----------------------------------------------------------------------------
	//Spying Process functions follow
	//-----------------------------------------------------------------------------

	void Injector::Launch(InjectorParameters^ parameters)
	{
		SimpleLogService^ log = SimpleLogManager::GetLogger(Injector::typeid, "Launch");
		try
		{
			String^ messageParameters = parameters->ToString();
			pin_ptr<const wchar_t> acmLocal = PtrToStringChars(messageParameters);

			log->Debug(String::Format("WM_GOBABYGO Message value      = 0x{0}", ::WM_GOBABYGO.ToString("X")));
			log->Debug(String::Format("Target Window Handle           = 0x{0}", parameters->WindowHandle.ToString("X")));
			log->Debug(String::Format("Original Hawkeye window Handle = 0x{0}", parameters->OriginalHandle.ToString("X")));
			log->Debug(String::Format("All parameters = {0} [{1}]", messageParameters, *acmLocal));

			HINSTANCE hinstDll;
			if (::GetModuleHandleEx(GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS, (LPCTSTR)&MessageHookProc, &hinstDll))
			{
				log->Debug("GetModuleHandleEx succeeded.");
				DWORD processId = 0;
				DWORD threadId = ::GetWindowThreadProcessId((HWND)parameters->WindowHandle.ToPointer(), &processId);
				if (processId)
				{
					log->Debug(String::Format("GetWindowThreadProcessId succeeded - pid={0}, tid={1}.", processId, threadId));
					HANDLE hProcess = ::OpenProcess(PROCESS_ALL_ACCESS, FALSE, processId);
					if (hProcess)
					{
						log->Debug(String::Format("OpenProcess succeeded - hprocess={0}.", (int)hProcess));
						int buffLen = (messageParameters->Length + 1) * sizeof(wchar_t);
						void* acmRemote = ::VirtualAllocEx(hProcess, NULL, buffLen, MEM_COMMIT, PAGE_READWRITE);
						if (acmRemote)
						{
							log->Debug(String::Format("VirtualAllocEx succeeded - acmRemote={0}.", (int)acmRemote));
							if (::WriteProcessMemory(hProcess, acmRemote, acmLocal, buffLen, NULL))
							{
								log->Debug("WriteProcessMemory succeeded.");

								_messageHookHandle = ::SetWindowsHookEx(WH_CALLWNDPROC, &MessageHookProc, hinstDll, threadId);
								if (_messageHookHandle)
								{
									log->Info("Hook was successfully installed");

									// How to test message was sent?
									::SendMessage((HWND)parameters->WindowHandle.ToPointer(), WM_GOBABYGO, (WPARAM)acmRemote, 0);

									if (::UnhookWindowsHookEx(_messageHookHandle))
										log->Debug("UnhookWindowsHookEx succeeded.");
									else LogLastError(log, "UnhookWindowsHookEx failed.");
								}
								else LogLastError(log, "SetWindowsHookEx failed.");
							}
							else LogLastError(log, "WriteProcessMemory failed.");

							// NB: according to http://msdn.microsoft.com/en-us/library/windows/desktop/aa366894.aspx
							// when using MEM_RELEASE, the dwSize parameter must be 0 (and not buffLen)
							// Otherwise, the call to VirtualFreeEx fails and lastError is 87 (incorrect parameter).
							if(::VirtualFreeEx(hProcess, acmRemote, 0, MEM_RELEASE))
								log->Debug("VirtualFreeEx succeeded.");
							else LogLastError(log, "VirtualFreeEx failed.");
						}
						else LogLastError(log, "VirtualAllocEx failed.");

						if (::CloseHandle(hProcess))
							log->Debug("CloseHandle succeeded.");
						else LogLastError(log, "CloseHandle failed.");
					}
					else LogLastError(log, "OpenProcess failed.");
				}
				else LogLastError(log, "GetWindowThreadProcessId failed.");

				if (::FreeLibrary(hinstDll))
					log->Debug("FreeLibrary succeeded.");
				else LogLastError(log, "FreeLibrary failed.");
			}
			else LogLastError(log, "GetModuleHandleEx failed.");			
		}
		catch (Exception^ ex)
		{
			log->Error(String::Format("{0}\r\n{1}", ex->Message, ex));
		}
	}

	void Injector::LogLastError(SimpleLogService^ log, String^ message)
	{
		DWORD error = ::GetLastError(); 
		wchar_t buffer[256];
		::FormatMessage(FORMAT_MESSAGE_FROM_SYSTEM, nullptr, error, 0, buffer, 256, nullptr);
		log->Error(message + String::Format(" - Win32: {0} - {1}", error, msclr::interop::marshal_as<String^>(buffer)));
	}
}