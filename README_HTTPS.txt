
Changes to be made for HTTPS to work

Dockerfile :
- Uncomment #EXPOSE 443

launchSettings.json :
- Uncomment //,"sslPort": 44329
- "useSSL": false -> true
- "applicationUrl": "http://localhost:5000" -> "applicationUrl": "https://localhost:5001;http://localhost:5000"

Startup.cs :
- Uncomment //app.UseHttpsRedirection();