import { BrowserRouter, Routes, Route } from "react-router-dom";

import Bookings from "./pages/Bookings";
import MainLayout from "./layouts/MainLayout";
import Dashboard from "./pages/Dashboard";
import Schedules from "./pages/Schedule";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<MainLayout />}>
          <Route
            path="/"
            element={<Dashboard />}
          />
          <Route
            path="/bookings"
            element={<Bookings />}
          />
          <Route
            path="/schedules"
            element={<Schedules />}
          />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;