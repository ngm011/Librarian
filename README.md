# Librarian

Setup of API Portal

1. In appsettings.json, change a) LibrarianDB connection string b) GoogleApiKey
2. Set Librarian.ApiPortal as startup project
3. Invoke :Add-Migration Initial" in Package Manager Console
4. Invoke "Update-Database" in Package Manager Console

Testing of API Portal

5. POST to "https://localhost:[port]/Library/auth" with body set to ApiKey - "12212112":

  POST /Library/auth HTTP/1.1
  Host: localhost:44345
  Content-Type: application/json
  Content-Length: 10

  "12212112"

6. Obtain JWT token from response 
7. GET to "https://localhost:[port]/Library?titleTerm=microsoft&authorTerm=William" with the token set in the header as "Authorization" key:

  GET /Library?titleTerm=microsoft&authorTerm=William HTTP/1.1
  Host: localhost:44345
  Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjEyMjEyMTEyIiwibmJmIjoxNjM5NzIxMTE5LCJleHAiOjE2Mzk3MjQ3MTksImlhdCI6MTYzOTcyMTExOX0.Gp6xpdz_1rLokDQSqUKBUmkIZL2APETURzYOhOlHYCI

Setup of Kiosk Client

1. In app.config, change a) Endpoint to match your IIS/IIS Express URL and port
