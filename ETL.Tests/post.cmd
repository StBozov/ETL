curl -X POST "http://localhost:5091/liveEvent" ^
     -H "Authorization: Bearer fake-secret-token" ^
     -H "Content-Type: application/json" ^
     -d "{\"userId\":\"user10\",\"eventName\":\"add_revenue\",\"revenueValue\":100}"
pause