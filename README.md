Loadmaster Monorepo
====================

This workspace contains a React frontend (Vite) and an ASP.NET Core Web API backend.

Projects
--------
- Frontend: `loadmaster-react`
- Backend: `loadmaster-api`

Getting Started (Development)
-----------------------------
1) Backend

```bash
cd loadmaster-api
dotnet restore
dotnet run
```

By default it runs at `http://localhost:5176` and exposes:
- `GET /api/health` (HealthController)
- `GET /api/hello?name=YourName` (HelloController)
- `GET /api/weatherforecast` (WeatherForecastController)

2) Frontend

```bash
cd loadmaster-react
npm install
npm run dev
```

The dev server runs at `http://localhost:8080` and proxies `/api` requests to the backend at `http://localhost:5176`.

Code Map
--------
- Backend (ASP.NET Core):
  - `loadmaster-api/Program.cs` → wiring (Swagger, CORS, controllers)
  - `loadmaster-api/Controllers/HealthController.cs`
  - `loadmaster-api/Controllers/HelloController.cs`
  - `loadmaster-api/Controllers/WeatherForecastController.cs`
- Frontend (React + Vite):
  - `loadmaster-react/src/lib/api.ts` → small API client
  - `loadmaster-react/src/pages/Dashboard.tsx` → shows API health and greeting

Production Notes
----------------
- Update CORS origins in `loadmaster-api/Program.cs` to include your deployed frontend domain.
- For single-origin deployments, serve the frontend separately (Netlify/Vercel/Static hosting) and point it to the API base URL.


