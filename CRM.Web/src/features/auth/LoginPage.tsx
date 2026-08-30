import { useState, type FormEvent } from "react"
import { useNavigate } from "react-router"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { useAuth } from "./useAuth"
import { ApiError } from "@/api/client"

export function LoginPage() {
  const { login } = useAuth()
  const navigate = useNavigate()
  const [email, setEmail] = useState("")
  const [password, setPassword] = useState("")
  const [errore, setErrore] = useState<string | null>(null)
  const [caricamento, setCaricamento] = useState(false)

  async function handleSubmit(event: FormEvent) {
    event.preventDefault()
    setErrore(null)
    setCaricamento(true)

    try {
      await login(email, password)
      navigate("/")
    } catch (err) {
      setErrore(err instanceof ApiError ? err.message : "Errore imprevisto, riprova.")
    } finally {
      setCaricamento(false)
    }
  }

  return (
    <div className="flex min-h-screen items-center justify-center">
      <form onSubmit={handleSubmit} className="w-80 space-y-4">
        <h1 className="text-2xl font-bold">GapsCRM</h1>
        <Input
          type="email"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <Input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
        />
        {errore && <p className="text-sm text-red-600">{errore}</p>}
        <Button type="submit" disabled={caricamento} className="w-full">
          {caricamento ? "Accesso in corso..." : "Accedi"}
        </Button>
      </form>
    </div>
  )
}
