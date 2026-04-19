# Sistema de Citas Medicas - Microservicios con .NET

Proyecto final del curso Building Microservices with .NET 10
Seccion: CURS-000538 | Instructor: Ronald Terrones Celis

## Microservicios

- PatientService - Gestion de pacientes (SQL Server)
- DoctorService - Catalogo de medicos y especialidades (SQL Server)
- AppointmentService - Gestion de citas medicas con Saga (MongoDB)
- PaymentService - Procesamiento de pagos (SQL Server)
- NotificationService - Recordatorios y notificaciones (SQL Server)
- EmailService - Envio de correos via Worker
- BffService - API Composition
- ApiGateway - Puerta de entrada unica con YARP

## Arquitectura

- Clean Architecture en cada microservicio
- DDD basico con Aggregates y Value Objects
- CQRS + MediatR
- Saga Pattern con MassTransit + RabbitMQ
- API Gateway con YARP
- Resiliencia con Polly
- Docker + Docker Compose

## Autor

Jheiner Meneses - CURS-000538