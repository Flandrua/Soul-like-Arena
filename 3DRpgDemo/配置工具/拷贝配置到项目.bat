@echo off
echo copy config
copy /y "表结构\*.*" "C:\UnityDemo\3DRpgDemo\Assets\Scripts\Configs\"
copy /y "配置输出JSON\*.*" "C:\UnityDemo\3DRpgDemo\Assets\Resources\Config\"
echo success!
pause