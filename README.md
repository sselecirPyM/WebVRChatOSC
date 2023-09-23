# WebVRChatOSC

Awesome OSC Controller

You can control your avatar through your browser.

Full customized button. You can give the button any OSC function.

See Icons:

https://fonts.google.com/icons?icon.set=Material+Icons

To access from outside of localhost, use the following parameters:

```
WebVRChatOSC.exe --urls http://*:5000
```

## Quickly add a button
1. Run VRChat, making sure you have OSC open and the program is running.
1. Change your avatar.
1. You can now find the avatar's parameters in the right button of the Action.

## Slider
Use this.value to get value.

## Requirement
Asp.Net Core Runtime 7 or DotNet SDK 7

https://dotnet.microsoft.com/en-us/download/dotnet/7.0

## Build
Require npm and dotnet sdk 7

```
dotnet build WebVRChatOSC.sln
```

```
cd webui
npm run build
```
