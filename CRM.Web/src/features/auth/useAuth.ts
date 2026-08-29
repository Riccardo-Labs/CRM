import { useContext } from "react"
import { AuthContext } from "./auth-context"

export function useAuth() {
  const context = useContext(AuthContext)
  if (context === undefined) {
    throw new Error("useAuth deve essere usato dentro AuthProvider")
  }
  return context
}
