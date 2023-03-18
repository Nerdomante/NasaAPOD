# NasaAPOD

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
