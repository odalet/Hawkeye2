Version 0.1.0 - 2012/08/07
==========================
* Not quite yet fnished! but tagged so that issues created in github can reference it.

Version 1.2.5 - 2011/01/10
==========================
* Fixed the **Hawkeye does not detect x86 applications on x86 Windows!!!**. See <http://hawkeye.codeplex.com/workitem/7791>

Version 1.2.4 - 2011/01/09
==========================
* Fixed the **can't open Reflector 7** issue. See <http://hawkeye.codeplex.com/workitem/7784>
* Provided two versions of Hawkeye: one targets .NET 2 , the other .NET 4

Version 1.2.3 - 2011/01/02
==========================
* Added new config file `settings.xml` and an option in it allowing to configure Hawkeye's hot key (empty to disable it). See <http://hawkeye.codeplex.com/workitem/7480>
* Added a **Go To Parent** button to the toolbar. See <http://hawkeye.codeplex.com/workitem/7156>
* Added Back/Forward buttons to the toolbar. See <http://hawkeye.codeplex.com/workitem/7157>
* Removed assemblies strong naming (this prepares the next release).
* Added the **Snapshot** feature (based on code by Sharpmao; see <http://hawkeye.codeplex.com/workitem/2783>)
* Updated the About box so that it supports VS designer.
* Modified the About box to include the history (this file) and the credits.
* Updated the link to Hawkeye (in Corneliu's text in the About box) to <http://hawkeye.codeplex.com/>
* Removed the "copyright" and "all rights reserved" and replaced with a link to the Ms-RL license

Version 1.2.2 - 2010/08/23
==========================
* Fixed the "Show Source Code in Reflector" button (cf. patch **#2728**)
* Removed the reference to a missing WPF plugin from `ACorns.Hawkeye.CoreUI.dll.config`
* Fixed references to extenders in code (TODO: declare the extenders in `app.config`). Only the search box is loaded.
* Simplified the solution configurations: now there are only x64 and x86 configurations that need be built both.
* Updated the main form code so that it supports VS designer.
* Added this file (History.txt)

Version 1.2.1
=============
* First build on codeplex by Corneliu I. Tusnea