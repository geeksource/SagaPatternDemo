# Microservices Architecture with SAGA Pattern in .NET 8

This project demonstrates a microservices architecture using **.NET 8**, **RabbitMQ**, and the **SAGA pattern** for handling distributed transactions across services like Order, Payment, and Inventory.

## 🚀 Features

- Microservice setup using ASP.NET Core 8
- SAGA pattern implementation (step-by-step)
- RabbitMQ for event-driven communication
- Swagger (OpenAPI) for each service
- Quartz.NET integration for scheduled tasks (optional)
- Minimal API support and MVC-style controllers

## 🧱 Services

| Service       | Description                            |
|---------------|----------------------------------------|
| Order         | Publishes events for order processing  |
| Inventory     | Reserves stock before payment          |
| Payment       | Confirms payment after inventory check |

> Note: Full SAGA orchestration is being implemented step-by-step.

---

## 🛠️ Technologies Used

- .NET 8.0 (ASP.NET Core Web API)
- RabbitMQ (with Docker)
- Swagger / Swashbuckle
- Visual Studio / VS Code

---

## 📦 Running the Solution

### 1. Clone the Repository

```bash
git clone https://github.com/your-username/your-repo-name.git
cd your-repo-name
