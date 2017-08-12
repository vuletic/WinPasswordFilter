reg add HKLM\SYSTEM\CurrentControlSet\Control\Lsa /v "Notification Packages" /t REG_MULTI_SZ /d scecli\0WinPasswordFilter /f
exit