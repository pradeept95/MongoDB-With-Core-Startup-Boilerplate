# Introduction
This is the generic repository for the AspNet Core Application with mongoDb Database. Current Support verison is Core 2.2 or above.

# Installation
To install the Core Repository follow the following either process

## Using Package Manager
Run following command in your respective project in package manager console

	Install-Package AspNetCore.MongoDb.Repository

## Using .Net CLI

    dotnet add package AspNetCore.MongoDb.Repository 

# Register
In Startup.cs file in your host project in the ConfigureServices() method register MongoDb Services by adding following 
Add following line at your import section

		using AspNetCore.MongoDb.Repository;


		public void ConfigureServices(IServiceCollection services)
        {
		  ......

		  services.AddMongoDbRepository(Configuration);

		  ......
		}


#Usase
