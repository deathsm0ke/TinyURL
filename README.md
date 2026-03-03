\## 🛠️ Prerequisites

\* \*\*.NET 10

\* \*\*Node.js\*\* (v18 or higher) \& \*\*npm\*\*

\* \*\*Angular CLI\*\* (`npm install -g @angular/cli`)

\* \*\*Entity Framework Core packages









How to Run the Project



\### 1. Backend (ASP.NET Core)

1\. Navigate to the `TinyUrlApi` folder.

2\. Open the solution in \*\*Visual Studio 2022\*\* and add Required NuGet Packages

&nbsp;  The following packages must be installed in the Web API project:

&nbsp;	\* Microsoft.EntityFrameworkCore.Sqlite 

&nbsp;	\* Microsoft.EntityFrameworkCore.Design 

&nbsp;	\* Microsoft.EntityFrameworkCore.Tools

&nbsp;	\* Microsoft.NET.Sdk.Functions ( for azure function 'TinyUrlCleaner' project)
	\* Install-Package Microsoft.EntityFrameworkCore.SqlServer (for Azure SQL)

3\. Ensure the `tinyurl.db` SQLite database is initialized.
	If the `tinyurl.db` file is not present, run below commands in the Package Manager Console or Terminal:



&nbsp;	# Generate the initial migration

&nbsp;	dotnet ef migrations add InitialCreate



&nbsp;	# Create the database and apply the schema

&nbsp;	dotnet ef database update

4\. Press `F5` or `dotnet run` to start the server.

&nbsp;  - \*\*Base URL\*\*: `https://localhost:7135`



\### 2. Frontend (Angular)

1\. Open a terminal in the `tiny-url-ui` folder.

2\. run below to Install dependencies(only first time):

&nbsp;	npm install

3\. to run the project 

&nbsp;	ng serve

4\. Open your browser to http://localhost:4200











\## 🔧 Troubleshooting Port Mismatches



If your environment runs the Backend or Frontend on different ports than the defaults (`7135` and `4200`), you must update the following settings to avoid CORS errors:





If your Backend port changes, update the `apiUrl` in:

&nbsp;	tiny-url-ui/src/app/services/url-api.ts

&nbsp;	private apiUrl = 'https://localhost:NEW\_PORT/api';

&nbsp;		OR

&nbsp;	You can permanently change the Backend port by editing: TinyUrlApi/Properties/launchSettings.json



If your Frontend port changes, update this line 'policy.WithOrigins("http://localhost:4200")' in program.cs file with new port







\##	tinyurlcleaner Azure Functions (Run local)



&nbsp;       1.Install Azurite via NPM (Azure storage emulator for local running purpose)



&nbsp;		Open your terminal and run:

&nbsp;		npm install -g azurite



&nbsp;		Once finished, type azurite again. It will now work.

&nbsp;	2. Install below dependency using Nuget package manager console "
		Install-Package Microsoft.EntityFrameworkCore.Sqlite

&nbsp;		Install-Package Microsoft.EntityFrameworkCore.Design

&nbsp;		Install-Package Microsoft.EntityFrameworkCore.SqlServer (For AzureSQL)









