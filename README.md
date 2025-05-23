# MAMS - Medical Appointment Management System

MAMS is a full-stack web application built using ASP.NET Core MVC and Web API, designed to manage medical appointments for hospitals and clinics. The system supports features like doctor availability scheduling, patient registration, appointment booking, and administrative reporting.

## Features

- **Doctor Management**: Register, update, and view doctors' availability.
- **Patient Management**: Register and manage patient records.
- **Appointment Booking**: Book appointments based on doctor availability with 10 patients per hour limit.
- **Search Functionality**: Find doctors by name or specialization.
- **Role-Based Access**: Role management for Admins, Doctors, and Patients.
- **Reports**: Downloadable reports for doctor appointments and availability.
- **Notifications**: User-friendly acknowledgments for data updates and actions.

## Technologies Used

- **Frontend**: ASP.NET Core MVC (AdminLTE Template)
- **Backend**: ASP.NET Core Web API
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **JSON Handling**: Newtonsoft.Json
- **Authentication**: Role-based authentication
- **Deployment**: Azure Virtual Machine (CI/CD optional)

## Project Structure

```plaintext
MAMS/          # Frontend ASP.NET MVC application
MAMS.API/      # Backend ASP.NET Core Web API
Models/        # Shared model classes
Views/         # Razor views using AdminLTE
Controllers/   # MVC controllers
Services/      # API service layer
