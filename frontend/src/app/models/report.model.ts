export interface Zone { id: string; nombre: string; }
export interface PowerOutageReport {
  id: string; zonaId: string; direccion: string; horaInicio: string; estado: string;
  tecnicoAsignadoId: string; evidenciaUrl: string; confirmaciones: number; ciudadanoId?: string;
}
export interface ReportCreateDto { zonaId: string; direccion: string; horaInicio: string; evidenciaUrl: string; }
export interface ResolutionDto { causa: string; horaRestablecimiento: string; detalle: string; }
