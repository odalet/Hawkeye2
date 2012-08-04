// (c) Copyright Cory Plotts.
// This source is subject to the Microsoft Public License (Ms-PL).
// Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
// All other rights reserved.

// Modified to suit Hawkeye needs (Olivier DALET)

#include "stdafx.h"

#include "Injector.h"
#include <vcclr.h>

using namespace ManagedInjector;

static unsigned int WM_GOBABYGO = ::RegisterWindowMessage(L"HawkeyeInjector_GOBABYGO!");
static HHOOK _messageHookHandle;

//-----------------------------------------------------------------------------
//Spying Process functions follow
//-----------------------------------------------------------------------------
void Injector::Launch(
	System::IntPtr windowHandle, 
	System::IntPtr hawkeyeHandle, 
	System::String^ assembly, 
	System::String^ className,
	System::String^ methodName,
	System::String^ logFileName)
{
	System::String^ messageParameters = windowHandle.ToString() + "$" + hawkeyeHandle.ToString() + "$" + assembly + "$" + className + "$" + methodName;
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

void Injector::LogMessage(System::String^ filename, System::String^ message, bool append)
{	            
	if (!append && System::IO::File::Exists(filename))
		System::IO::File::Delete(filename);    
	
	System::IO::FileInfo ^ fi = gcnew System::IO::FileInfo(filename);	            
	System::IO::StreamWriter ^ sw = fi->AppendText();   
	sw->WriteLine(System::DateTime::Now.ToString("MM/dd/yyyy HH:mm:ss") + " : " + message);
	sw->Close();
}

__declspec(dllexport) 
LRESULT __stdcall MessageHookProc(int nCode, WPARAM wparam, LPARAM lparam)
{
	if (nCode == HC_ACTION)
	{
		CWPSTRUCT* msg = (CWPSTRUCT*)lparam;
		if (msg != NULL && msg->message == WM_GOBABYGO)
		{
			System::Diagnostics::Debug::WriteLine("Got WM_GOBABYGO message");

			wchar_t* acmRemote = (wchar_t*)msg->wParam;

			String^ acmLocal = gcnew System::String(acmRemote);
			System::Diagnostics::Debug::WriteLine(System::String::Format("acmLocal = {0}", acmLocal));
			cli::array<System::String^>^ acmSplit = acmLocal->Split('$');

			System::Diagnostics::Debug::WriteLine(String::Format("About to load assembly {0}", acmSplit[2]));
			System::Reflection::Assembly^ assembly = System::Reflection::Assembly::LoadFile(acmSplit[2]);
			if (assembly != nullptr)
			{
				System::Diagnostics::Debug::WriteLine(String::Format("About to load type {0}", acmSplit[3]));
				System::Type^ type = assembly->GetType(acmSplit[3]);
				if (type != nullptr)
				{
					System::Diagnostics::Debug::WriteLine(String::Format("Just loaded the type {0}", acmSplit[3]));
					System::Reflection::MethodInfo^ methodInfo = type->GetMethod(acmSplit[4], 
						System::Reflection::BindingFlags::Static | System::Reflection::BindingFlags::Public | System::Reflection::BindingFlags::NonPublic);
					if (methodInfo != nullptr)
					{
						System::Diagnostics::Debug::WriteLine(System::String::Format("About to invoke {0} on type {1}", methodInfo->Name, acmSplit[3]));
						Object^ returnValue = methodInfo->Invoke(nullptr, acmSplit);
						if (nullptr == returnValue)
							returnValue = "NULL";
						System::Diagnostics::Debug::WriteLine(String::Format("Return value of {0} on type {1} is {2}", methodInfo->Name, acmSplit[3], returnValue));
					}
					else System::Diagnostics::Debug::WriteLine(System::String::Format("Could not find method {0} in type {1}", acmSplit[4], acmSplit[3]), "Error");
				}
				else System::Diagnostics::Debug::WriteLine(System::String::Format("Could not find type {0} in assembly {1}", acmSplit[3], acmSplit[2]), "Error");
			}
			else System::Diagnostics::Debug::WriteLine(System::String::Format("Could not load assembly from {0}", acmSplit[2]), "Error");
		}
	}
	return CallNextHookEx(_messageHookHandle, nCode, wparam, lparam);
}