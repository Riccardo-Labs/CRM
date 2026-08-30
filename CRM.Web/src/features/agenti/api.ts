import { apiFetch } from "@/api/client"
import type { components } from "@/api/types"

export type Agente = components["schemas"]["Agente"]
export type AgenteCreateDto = components["schemas"]["AgenteCreateDto"]
export type AgenteUpdateDto = components["schemas"]["AgenteUpdateDto"]

export function getAgenti() {
  return apiFetch<Agente[]>("/api/Agenti")
}

export function getAgente(id: number) {
  return apiFetch<Agente>(`/api/Agenti/${id}`)
}

export function createAgente(dto: AgenteCreateDto) {
  return apiFetch<Agente>("/api/Agenti", { method: "POST", body: dto })
}

export function updateAgente(id: number, dto: AgenteUpdateDto) {
  return apiFetch<void>(`/api/Agenti/${id}`, { method: "PUT", body: dto })
}

export function deleteAgente(id: number) {
  return apiFetch<void>(`/api/Agenti/${id}`, { method: "DELETE" })
}
