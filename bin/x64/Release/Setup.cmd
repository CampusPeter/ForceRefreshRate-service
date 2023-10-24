copy ForceRefreshRateService.exe c:\windows\
installutil c:\windows\ForceRefreshRateService.exe
sc config ForceRefRate start=auto
net start ForceRefRate
