@ECHO OFF
REM build script for windows system

SET TARGET=Debug

REM detect if there is a target specified
IF NOT "%1"=="" (
  SET TARGET=%1
)

REM Clean and Build 
msbuild /t:Rebuild /p:Configuration=%TARGET%  ../solutions/monosim-gtk.sln
