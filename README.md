Weather Forecasting System

This project is a weather forecasting system developed using modern software development practices and a clean architectural approach. It showcases how to build a scalable, maintainable, and extensible application that integrates with cloud services and external APIs.

Features

API Layer:
	•	Exposes two endpoints (GET and POST) for weather data operations.
	•	Built using Onion Architecture for separation of concerns.
	•	Implements CQRS (Command Query Responsibility Segregation) with MediatR for handling requests.
	•	Uses PostgreSQL as the primary database.
	•	Integrated with Serilog for structured logging and error tracking.
 Azure Function Layer:
	•	Handles background tasks and additional data processing.
	•	Fetches raw weather data from external Weather APIs.
	•	Stores raw data in Azure Blob Storage for further processing.
	•	Processes data using Azure Queue Storage, parses the information, and forwards it to the Application Layer for storage in PostgreSQL.

Current Status
	•	The API layer has been fully developed, tested, and dockerized for deployment.
	•	The Azure Function Layer has been developed and tested but is not yet dockerized.
Technologies Used
	•	Backend Framework: .NET
	•	Database: PostgreSQL
	•	Architecture: Onion Architecture with CQRS and MediatR
	•	Logging: Serilog
	•	Cloud Services: Azure Blob Storage, Azure Queue Storage, Azure Functions
	•	Containerization: Docker

Future Plans
	•	Dockerize the Azure Function layer for seamless deployment.
	•	Expand functionality to incorporate predictive analytics and big data integration.
	•	Enhance visualization by building a dashboard for weather data insights.

