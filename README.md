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

appsettings.json file contain all configurable values, these can be also updated in settings panel of the application:

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
    "ScaleThresholdHeight": 300,
    "ScaleThresholdWidth": 320,
    "FillerTransparency":  200
  }
}
```

**ApiKey**: You can leave DEMO_KEY. This default key has a limit of 30 hourly requests per IP address and 50 daily requests per day. This program only makes 24 calls each day, but you can set a higher value in the HoursInterval to make it do less.

**Endpoint**: This is the endpoint of the NASA REST API.

**HoursInterval**: This is the interval of time between each request.

**Lang**: This is the ISO 639-1 code of the language to translate into (reference: https://en.wikipedia.org/wiki/List_of_ISO_639-1_codes).

**BlurLevel**: If you want, you can specify a level of blur effect to apply to filler images for those APODs that do not have the correct size to become a nice wallpaper.

**FillerPath**: If you want, you can change the filler image for those APODs that do not have the correct size to become a nice wallpaper.

**Ratio**: This is the difference between the height and width of the image. If this result is lower or equal to the value, the image is set as wallpaper in FILL mode.

**ScaleThresholdHeight**: This is the difference between your screen's height and the height of the image. If this result is lower or equal to the value, the image is not scaled.

**ScaleThresholdWidth**: This is the difference between your screen's width and the width of the image. If this result is lower or equal to the value, the image is not scaled.

**FillerTransparency**: This is the transparency effect (alpha channel) of a solid color mask applied to the image.

**!IMPORTANT!**: The default values for ScaleThresholdHeight, ScaleThresholdWidth, and Ratio are fine for a monitor resolution of 1080 x 1920. For higher resolutions, it is highly recommended to increase these values.
