import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import {
  createAgente,
  deleteAgente,
  getAgente,
  getAgenti,
  updateAgente,
  type AgenteCreateDto,
  type AgenteUpdateDto,
} from "./api"

const QUERY_KEY = ["agenti"]

export function useAgenti() {
  return useQuery({ queryKey: QUERY_KEY, queryFn: getAgenti })
}

export function useAgente(id: number) {
  return useQuery({
    queryKey: [...QUERY_KEY, id],
    queryFn: () => getAgente(id),
    enabled: !Number.isNaN(id),
  })
}

export function useCreateAgente() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (dto: AgenteCreateDto) => createAgente(dto),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: QUERY_KEY }),
  })
}

export function useUpdateAgente() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: ({ id, dto }: { id: number; dto: AgenteUpdateDto }) => updateAgente(id, dto),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: QUERY_KEY }),
  })
}

export function useDeleteAgente() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (id: number) => deleteAgente(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: QUERY_KEY }),
  })
}
