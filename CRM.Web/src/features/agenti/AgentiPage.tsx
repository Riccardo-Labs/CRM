import { Link } from "react-router"
import { useAgenti, useDeleteAgente } from "./hooks"
import { Button, buttonVariants } from "@/components/ui/button"
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table"

export function AgentiPage() {
  const { data: agenti, isLoading, isError } = useAgenti()
  const deleteAgente = useDeleteAgente()

  if (isLoading) return <p>Caricamento...</p>
  if (isError) return <p className="text-red-600">Errore nel caricamento degli agenti.</p>

  function handleDelete(id: number) {
    if (confirm("Disattivare questo agente?")) {
      deleteAgente.mutate(id)
    }
  }

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold">Agenti</h1>
        <Link to="/agenti/nuovo" className={buttonVariants()}>
          Nuovo Agente
        </Link>
      </div>

      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Nome</TableHead>
            <TableHead>Cognome</TableHead>
            <TableHead>Email</TableHead>
            <TableHead>Telefono</TableHead>
            <TableHead>Attivo</TableHead>
            <TableHead />
          </TableRow>
        </TableHeader>
        <TableBody>
          {agenti?.map((agente) => (
            <TableRow key={agente.idAgente}>
              <TableCell>{agente.nome}</TableCell>
              <TableCell>{agente.cognome}</TableCell>
              <TableCell>{agente.email}</TableCell>
              <TableCell>{agente.telefono}</TableCell>
              <TableCell>{agente.attivo ? "Sì" : "No"}</TableCell>
              <TableCell className="space-x-2 text-right">
                <Link
                  to={`/agenti/${agente.idAgente}/modifica`}
                  className={buttonVariants({ variant: "outline", size: "sm" })}
                >
                  Modifica
                </Link>
                <Button
                  variant="destructive"
                  size="sm"
                  onClick={() => handleDelete(agente.idAgente!)}
                >
                  Disattiva
                </Button>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  )
}
