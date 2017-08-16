reg add HKLM\SYSTEM\CurrentControlSet\Control\Lsa /v "Notification Packages" /t REG_MULTI_SZ /d scecli\0WinPasswordFilter /f
mkdir C:\WinPasswordFilter
icacls C:\WinPasswordFilter /inheritance:d
icacls C:\WinPasswordFilter /remove:g Users
icacls C:\WinPasswordFilter /remove:g "Authenticated Users"
exit