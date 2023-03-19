# NasaAPOD

If you like this project or want support it, buy me a coffee ☕

[![](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/donate/?hosted_button_id=L34HN43UDM36Q)

Also starring ⭐ the repo is appreciated!

![alt text](https://github.com/Nerdomante/NasaAPOD/blob/master/demo_screen.jpg?raw=true)

1) If needed you can change value of "ApiKey": "DEMO_KEY" with one provided by: https://api.nasa.gov/#signUp an api key will be sent to your email address.

2) Publish to folder with these settings:

```
<Project>
  <PropertyGroup>
    <Configuration>Release</Configuration>
    <Platform>Any CPU</Platform>
    <PublishDir>C:\<where you want>\NasaAPOD</PublishDir>
    <PublishProtocol>FileSystem</PublishProtocol>
    <_TargetId>Folder</_TargetId>
    <TargetFramework>net6.0-windows10.0.17763.0</TargetFramework>
    <SelfContained>false</SelfContained>
    <RuntimeIdentifier>win-x64</RuntimeIdentifier>
    <PublishSingleFile>true</PublishSingleFile>
    <PublishReadyToRun>true</PublishReadyToRun>
  </PropertyGroup>
</Project>
```

appsettings.json file contain all configurable values:

```
{
  "AppSettings": {
    "ApiKey": "DEMO_KEY",
    "Endpoint": "https://api.nasa.gov",
    "HoursInterval": 1,
    "Lang": "it",
    "BlurLevel": 0,
    "FillerPath": "Res/skyfiller.png",
    "Ratio": 550,
    "ScaleThreshold": 250
  }
}
```

**ApiKey**: You can leave DEMO_KEY, this default key have 30 requests hourly limit per IP address and 50 daily request per day. 
            This program make only 24 call each day, but you can set an highter value in *HoursInterval* value to make it do less.
           
**Endpoint**: Is the endpoint of the nasa rest api.

**HoursInterval**: Interval of time between each request.

**Lang**: ISO 639-1 code of the language to translate into (ref: https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes)

**BlurLevel**: If you want, you can specify a level to blur effect to apply on filler image for those APOD not have correct size to become a nice wallpaper.

**FillerPath**: If you want, you can change the filler image for those APOD not have correct size to become a nice wallpaper.

**Ratio**: Is the difference between Height minus Width of the image. If this result is lower or equal the value, the image is set as wallpaper in FILL mode.

**ScaleThreshold**: Is the difference between your screen Height minus Height of the image. If this result is lower or equal the value, the image is not scaled.
