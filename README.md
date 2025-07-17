# Clarity

Clarity is a modern task management application featuring JWT authentication. The frontend is built with React, Vite, and TypeScript, while the backend is powered by ASP.NET Core with Entity Framework Core and PostgreSQL. The app supports full CRUD operations for tasks, secured via JWT tokens and secure cookies.

---

## Technology Stack
- Frontend: React, Vite, TypeScript  
- Backend: ASP.NET Core, Entity Framework Core,  JWT Authentication
- Database: PostgreSQL  
- Containerization: Docker, Docker Compose

---

## Getting Started

### Using Docker Compose (Recommended)

1. Make sure Docker and Docker Compose are installed on your machine.
2. From the root folder of the project, run:

```
docker-compose up --build
```
This will start the backend API and a PostgreSQL database container.

---

## Environment Variables
### Frontend
In the frontend folder, create or update .env.production with:
```
VITE_API_URL=http://localhost:8080/api/Notes
```

### Frontend
In the backend folder, create or update .env with:
```
ConnectionStrings__ClarityDbContext=YOUR_CONNECTION_STRING

JwtOptions__SecretKey=YOUR_SECRET_KEY
JwtOptions__Issuer=YOUR_ISSUER
JwtOptions__Audience=YOUR_AUDIENCE
JwtOptions__ExpiresMinutes=YOUR_EXPIRES_MINUTES
JwtOptions__AuthCookieName=YOUR_AUTH_COOKIE_NAME
JwtOptions__RefreshCookieName=YOUR_REFRESH_COOKIE_NAME

FrontendUrl=YOUR_FRONTEND_URL
```

---

## Running Manually
If you prefer to run frontend and backend separately:
### Frontend
```
cd frontend
npm install
npm run dev
```
### Backend
```
cd backend
dotnet run
```
