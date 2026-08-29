import { useState, type ReactNode } from "react"
import { apiFetch } from "@/api/client"
import type { components } from "@/api/types"
import { AuthContext, type AuthState } from "./auth-context"

type LoginResponseDto = components["schemas"]["LoginResponseDto"]

const STORAGE_KEY = "auth"

function leggiAuthSalvato(): AuthState {
  const raw = localStorage.getItem(STORAGE_KEY)
  if (!raw) return null
  try {
    return JSON.parse(raw) as AuthState
  } catch {
    return null
  }
}

export function AuthProvider({ children }: { children: ReactNode }) {
  const [auth, setAuth] = useState<AuthState>(() => leggiAuthSalvato())

  async function login(email: string, password: string) {
    const risposta = await apiFetch<LoginResponseDto>("/api/Auth/login", {
      method: "POST",
      body: { email, password },
    })

    const nuovoAuth: AuthState = {
      token: risposta.token!,
      email: risposta.email!,
      ruolo: risposta.ruolo!,
    }

    localStorage.setItem(STORAGE_KEY, JSON.stringify(nuovoAuth))
    setAuth(nuovoAuth)
  }

  function logout() {
    localStorage.removeItem(STORAGE_KEY)
    setAuth(null)
  }

  return (
    <AuthContext.Provider value={{ auth, isAuthenticated: auth !== null, login, logout }}>
      {children}
    </AuthContext.Provider>
  )
}
