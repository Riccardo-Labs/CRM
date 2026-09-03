import { BrowserRouter, Routes, Route } from "react-router"
import { LoginPage } from "@/features/auth/LoginPage"
import { ProtectedRoute } from "@/features/auth/ProtectedRoute"
import { Layout } from "@/components/Layout"
import { AgentiPage } from "@/features/agenti/AgentiPage"
import { AgenteForm } from "@/features/agenti/AgenteForm"

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route
          element={
            <ProtectedRoute>
              <Layout />
            </ProtectedRoute>
          }
        >
          <Route path="/" element={<div>Dashboard (placeholder)</div>} />
          <Route path="/agenti" element={<AgentiPage />} />
          <Route path="/agenti/nuovo" element={<AgenteForm />} />
          <Route path="/agenti/:id/modifica" element={<AgenteForm />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}

export default App
