// (c) Copyright Cory Plotts.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

// Modified to suit Hawkeye needs (Olivier DALET)

#include "stdafx.h"

#include "injector.h"
#include <vcclr.h>

using namespace System;
using namespace System::IO;
using namespace System::Reflection;
using namespace System::Diagnostics;
using namespace ManagedInjector;

static unsigned int WM_GOBABYGO = ::RegisterWindowMessage(L"HawkeyeInjector_GOBABYGO!");
static HHOOK _messageHookHandle;

//-----------------------------------------------------------------------------
//Spying Process functions follow
//-----------------------------------------------------------------------------

/// <summary>
/// Launches the injection in the process owning the specified window handle.
/// </summary>
/// <param name="windowHandle">The window handle.</param>
/// <param name="hawkeyeHandle">The original hawkeye handle.</param>
/// <param name="assemblyName">Name of the assembly.</param>
/// <param name="className">Name of the class.</param>
/// <param name="methodName">Name of the method.</param>
/// <param name="logFileName">Name of the log file.</param>
void Injector::Launch(
	IntPtr windowHandle, IntPtr hawkeyeHandle, 
	String^ assembly, String^ className, String^ methodName, String^ logFileName)
{
	String^ messageParameters = windowHandle.ToString() + "$" + hawkeyeHandle.ToString() + "$" + assembly + "$" + className + "$" + methodName;
	pin_ptr<const wchar_t> acmLocal = PtrToStringChars(messageParameters);

	HINSTANCE hinstDLL;	

	if (::GetModuleHandleEx(GET_MODULE_HANDLE_EX_FLAG_FROM_ADDRESS, (LPCTSTR)&MessageHookProc, &hinstDLL))
	{
		LogMessage(logFileName, "GetModuleHandleEx successful", true);
		DWORD processID = 0;
		DWORD threadID = ::GetWindowThreadProcessId((HWND)windowHandle.ToPointer(), &processID);

		if (processID)
		{
			LogMessage(logFileName, "Got process id", true);
			HANDLE hProcess = ::OpenProcess(PROCESS_ALL_ACCESS, FALSE, processID);
			if (hProcess)
			{
				LogMessage(logFileName, "Got process handle", true);
				int buffLen = (messageParameters->Length + 1) * sizeof(wchar_t);
				void* acmRemote = ::VirtualAllocEx(hProcess, NULL, buffLen, MEM_COMMIT, PAGE_READWRITE);

				if (acmRemote)
				{
					LogMessage(logFileName, "VirtualAllocEx successful", true);
					::WriteProcessMemory(hProcess, acmRemote, acmLocal, buffLen, NULL);

					_messageHookHandle = ::SetWindowsHookEx(WH_CALLWNDPROC, &MessageHookProc, hinstDLL, threadID);

					if (_messageHookHandle)
					{
						LogMessage(logFileName, "SetWindowsHookEx successful", true);
						::SendMessage((HWND)windowHandle.ToPointer(), WM_GOBABYGO, (WPARAM)acmRemote, 0);
						::UnhookWindowsHookEx(_messageHookHandle);
					}

					::VirtualFreeEx(hProcess, acmRemote, buffLen, MEM_RELEASE);
				}

				::CloseHandle(hProcess);
			}
		}
		::FreeLibrary(hinstDLL);
	}
}

/// <summary>
/// Logs the message.
/// </summary>
/// <param name="logFileName">Name of the log file.</param>
/// <param name="message">The message.</param>
/// <param name="append">The append.</param>
void Injector::LogMessage(System::String^ filename, System::String^ message, bool append)
{	            
	if (!append && File::Exists(filename))
		File::Delete(filename);    

	FileInfo^ fi = gcnew FileInfo(filename);	            
	StreamWriter^ sw = fi->AppendText();   
	sw->WriteLine(DateTime::Now.ToString("MM/dd/yyyy HH:mm:ss") + " : " + message);
	sw->Close();
}

/// <summary>
/// The hook.
/// </summary>
/// <param name="nCode">The code.</param>
/// <param name="wparam">The wparam.</param>
/// <param name="lparam">The lparam.</param>
/// <returns></returns>
__declspec(dllexport)
	LRESULT __stdcall MessageHookProc(int nCode, WPARAM wparam, LPARAM lparam)
{
	if (nCode == HC_ACTION)
	{
		CWPSTRUCT* msg = (CWPSTRUCT*)lparam;
		if (msg != NULL && msg->message == WM_GOBABYGO)
		{
			Debug::WriteLine("Got WM_GOBABYGO message");

			wchar_t* acmRemote = (wchar_t*)msg->wParam;

			String^ acmLocal = gcnew String(acmRemote);
			Debug::WriteLine(String::Format("acmLocal = {0}", acmLocal));
			cli::array<System::String^>^ acmSplit = acmLocal->Split('$');

			Debug::WriteLine(String::Format("About to load assembly {0}", acmSplit[2]));
			Assembly^ assembly = Assembly::LoadFile(acmSplit[2]);
			if (assembly != nullptr)
			{
				Debug::WriteLine(String::Format("About to load type {0}", acmSplit[3]));
				Type^ type = assembly->GetType(acmSplit[3]);
				if (type != nullptr)
				{
					Debug::WriteLine(String::Format("Just loaded the type {0}", acmSplit[3]));
					BindingFlags flags = BindingFlags::Static | BindingFlags::Public | BindingFlags::NonPublic;
					MethodInfo^ methodInfo = type->GetMethod(acmSplit[4], flags);
					if (methodInfo != nullptr)
					{
						Debug::WriteLine(String::Format("About to invoke {0} on type {1}", methodInfo->Name, acmSplit[3]));
						try
						{
							Object^ returnValue = methodInfo->Invoke(nullptr, acmSplit);
							if (nullptr == returnValue)
								returnValue = "NULL";
							Debug::WriteLine(String::Format("Return value of {0} on type {1} is {2}", methodInfo->Name, acmSplit[3], returnValue));
						}
						catch (Exception^ e)
						{
							// for debugging purpose
							Exception^ debugException = e; 
						}
					}
					else Debug::WriteLine(System::String::Format("Could not find method {0} in type {1}", acmSplit[4], acmSplit[3]), "Error");
				}
				else Debug::WriteLine(System::String::Format("Could not find type {0} in assembly {1}", acmSplit[3], acmSplit[2]), "Error");
			}
			else Debug::WriteLine(System::String::Format("Could not load assembly from {0}", acmSplit[2]), "Error");
		}
	}
	return CallNextHookEx(_messageHookHandle, nCode, wparam, lparam);
}