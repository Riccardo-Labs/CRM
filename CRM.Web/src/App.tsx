import { BrowserRouter, Routes, Route } from "react-router"
import { LoginPage } from "@/features/auth/LoginPage"
import { ProtectedRoute } from "@/features/auth/ProtectedRoute"
import { Layout } from "@/components/Layout"

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
        </Route>
      </Routes>
    </BrowserRouter>
  )
}

export default App
