import { createContext } from "react"

export type AuthState = {
  token: string
  email: string
  ruolo: string
} | null

export type AuthContextValue = {
  auth: AuthState
  isAuthenticated: boolean
  login: (email: string, password: string) => Promise<void>
  logout: () => void
}

export const AuthContext = createContext<AuthContextValue | undefined>(undefined)
