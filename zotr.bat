@echo off
msbuild /restore /t:Build /p:Platform=x64 /p:Configuration=Release /v:m
