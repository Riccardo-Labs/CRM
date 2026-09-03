import { useState, type FormEvent } from "react"
import { useNavigate, useParams } from "react-router"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { useAgente, useCreateAgente, useUpdateAgente } from "./hooks"
import type { Agente } from "./api"
import { ApiError } from "@/api/client"

export function AgenteForm() {
  const { id } = useParams()
  const isEdit = id !== undefined
  const agenteId = isEdit ? Number(id) : undefined

  const { data: agenteEsistente, isLoading } = useAgente(agenteId ?? NaN)

  if (isEdit && isLoading) {
    return <p>Caricamento...</p>
  }

  return (
    <AgenteFormFields
      key={agenteId ?? "nuovo"}
      agenteEsistente={agenteEsistente}
      agenteId={agenteId}
    />
  )
}

function AgenteFormFields({
  agenteEsistente,
  agenteId,
}: {
  agenteEsistente: Agente | undefined
  agenteId: number | undefined
}) {
  const navigate = useNavigate()
  const isEdit = agenteId !== undefined
  const createAgente = useCreateAgente()
  const updateAgente = useUpdateAgente()

  const [nome, setNome] = useState(agenteEsistente?.nome ?? "")
  const [cognome, setCognome] = useState(agenteEsistente?.cognome ?? "")
  const [email, setEmail] = useState(agenteEsistente?.email ?? "")
  const [telefono, setTelefono] = useState(agenteEsistente?.telefono ?? "")
  const [dataAssunzione, setDataAssunzione] = useState(agenteEsistente?.dataAssunzione ?? "")
  const [attivo, setAttivo] = useState(agenteEsistente?.attivo ?? true)
  const [errore, setErrore] = useState<string | null>(null)

  async function handleSubmit(event: FormEvent) {
    event.preventDefault()
    setErrore(null)

    try {
      if (isEdit && agenteId !== undefined) {
        await updateAgente.mutateAsync({
          id: agenteId,
          dto: { nome, cognome, email, telefono, dataAssunzione, attivo },
        })
      } else {
        await createAgente.mutateAsync({ nome, cognome, email, telefono, dataAssunzione })
      }

      navigate("/agenti")
    } catch (err) {
      setErrore(err instanceof ApiError ? err.message : "Errore imprevisto, riprova.")
    }
  }

  return (
    <form onSubmit={handleSubmit} className="max-w-md space-y-4">
      <h1 className="text-2xl font-bold">{isEdit ? "Modifica Agente" : "Nuovo Agente"}</h1>
      <Input placeholder="Nome" value={nome} onChange={(e) => setNome(e.target.value)} required />
      <Input placeholder="Cognome" value={cognome} onChange={(e) => setCognome(e.target.value)} required />
      <Input type="email" placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)} required />
      <Input placeholder="Telefono" value={telefono} onChange={(e) => setTelefono(e.target.value)} />
      <Input type="date" value={dataAssunzione} onChange={(e) => setDataAssunzione(e.target.value)} required />
      {isEdit && (
        <label className="flex items-center gap-2 text-sm">
          <input type="checkbox" checked={attivo} onChange={(e) => setAttivo(e.target.checked)} />
          Attivo
        </label>
      )}
      {errore && <p className="text-sm text-red-600">{errore}</p>}
      <Button type="submit">{isEdit ? "Salva" : "Crea"}</Button>
    </form>
  )
}
