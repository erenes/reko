@echo off
msbuild /t:Build /p:Platform=x64 /p:Configuration=Release /v:m
