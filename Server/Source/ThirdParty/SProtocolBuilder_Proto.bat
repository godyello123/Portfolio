@echo off

set PATH_PROTO="..\ThirdParty"
set PATH_PROTOC="..\ThirdParty\tools\protobuf"
set PATH_OUTPUT=%PATH_PROTO%
set PATH_OUTPUT_CS="..\ThirdParty\CS_Dir"

%PATH_PROTOC%\protoc -I=%PATH_PROTO% --js_out=import_style=commonjs,binary:%PATH_OUTPUT% --csharp_out=%PATH_OUTPUT% %PATH_PROTO%\*.proto

::echo "cs copy to %PATH_OUTPUT_CS%"
::xcopy /cy .\*.cs %PATH_OUTPUT_CS% /exclude:exclude_cs.txt

::pause