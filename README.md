# WebVRChatOSC

Awesome OSC Controller

You can control your avatar through your browser.

Full customized button. You can give the button any OSC function.

## Requirement
Asp.Net Core Runtime 7 or DotNet SDK 7

https://dotnet.microsoft.com/en-us/download/dotnet/7.0

Double click run.bat to run it.

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

## Build
Require npm and dotnet sdk 7

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
