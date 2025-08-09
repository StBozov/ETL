@echo off
setlocal

set TOKEN=fake-secret-token
set PORT=5091

for %%U in (user1 user2 user3 user4 user5 user6 user7 user8 user9 user10) do (
    echo ==== Revenue for %%U ====
    curl "http://localhost:%PORT%/userRevenue?userId=%%U" -H "Authorization: Bearer %TOKEN%"
    echo.
)

pause