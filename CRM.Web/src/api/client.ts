import { AUTH_STORAGE_KEY } from "@/features/auth/auth-context"

const BASE_URL = import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5048"

type Method = "GET" | "POST" | "PUT" | "DELETE"

function leggiToken(): string | null {
  const raw = localStorage.getItem(AUTH_STORAGE_KEY)
  if (!raw) return null
  try {
    const auth = JSON.parse(raw) as { token?: string }
    return auth.token ?? null
  } catch {
    return null
  }
}

// API client per interagire con l'API del backend
export class ApiError extends Error {
  status: number
  constructor(status: number, message: string) {
    super(message)
    this.status = status
  }
}

// Funzione generics per effettuare richieste all'API del backend e restituire il tipo specificato
export async function apiFetch<T>(
  path: string,
  options: { method?: Method; body?: unknown } = {}
): Promise<T> {
  const token = leggiToken()

  const risposta = await fetch(`${BASE_URL}${path}`, {
    method: options.method ?? "GET",
    headers: {
      "Content-Type": "application/json",
      ...(token ? { Authorization: `Bearer ${token}` } : {}),
    },
    body: options.body ? JSON.stringify(options.body) : undefined,
  })

  if (!risposta.ok) {
    const text = await risposta.text()
    throw new ApiError(risposta.status, text || risposta.statusText)
  }

  if (risposta.status === 204) {
    return undefined as T
  }

  return risposta.json() as Promise<T> // Restituisce il corpo della risposta come JSON del tipo specificato
}
