@echo off

echo "builder start"
.\SProtocolBuilder.exe
call .\SProtocolBuilder_Proto.bat

::cd "C:\Users\admin\Desktop\ServerProject\Source\ThirdParty"

set PATH_OUTPUT_CS=".\..\Share\Packet"

echo "cs move to %PATH_OUTPUT_CS%"
::xcopy /cy .\*.cs %PATH_OUTPUT% /exclude:exclude_cs.txt
move /Y .\*.cs %PATH_OUTPUT_CS%

pause