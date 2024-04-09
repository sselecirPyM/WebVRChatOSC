# WebVRChatOSC

Awesome OSC Controller

You can control your avatar through your browser.

Full customized button. You can give the button any OSC function.

## Requirement
DotNet SDK 8

https://dotnet.microsoft.com/en-us/download/dotnet/8.0

Install the .net8 SDK. Then double-click run.bat run. It doesn't seem to be possible to use the x86 SDK on 64-bit Windows, so choose the x64 SDK.


Or use the PowerShell command line to install.

```
winget install Microsoft.DotNet.SDK.8
```

## Quickly add a button
1. Run VRChat, making sure you have OSC open and the program is running.
1. Change your avatar.
1. You can now find the avatar's parameters in the right button of the Action.

Use this.value to get slider's value.

See Icons:

https://fonts.google.com/icons?icon.set=Material+Icons

To access from outside of localhost, use the following parameters:

```
dotnet WebVRChatOSC.dll --urls http://*:5000
```

## If you want to build from source
Require npm and dotnet sdk 8

Run

```
dotnet build WebVRChatOSC.sln
```

```
cd webui
npm run build
```

## Use it on your phone
execute run_public.bat then acccess it by browser.

Quasar UI adapts very well to the phone screen.
![](https://github.com/sselecirPyM/WebVRChatOSC/assets/63526047/7bd69264-1cbf-4bd8-a858-760930b2777d)
![](https://github.com/sselecirPyM/WebVRChatOSC/assets/63526047/94c88255-6608-4f21-9a19-a9bd15e88fe9)

You can run on termux as well. Use proot-distro install ubuntu. Login in ubuntu, install asp.net core runtime. Then execute the following command.

```
export DOTNET_GCHeapHardLimit=1C0000000
dotnet WebVRChatOSC.dll
```
