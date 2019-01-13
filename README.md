# Micro services with MassTransit & CQRS pattern & Saga
The prupose of this project is to develop two micro services in .net core console application and manage message queue using Masstransit. I have used topshelf, an open source nuget library to manage my services in windows. The payment service cover an implementation of CQRS pattern.

# URL:
- https://localhost:44332/api/Student/EnrollStudent

Note: Both API get methods are just for demo the actual purpose was to focuse on micros service. 

#Configuration:
Please enter RabbitMq login credentials in all three projects startup files (CQRS, Payment.Service, Notification.Service)
