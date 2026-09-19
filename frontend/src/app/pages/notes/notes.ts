import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { NoteService } from '../../services/note.service';
import { AuthService } from '../../services/auth.service';
import { Note } from '../../models/note.model';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-notes',
  standalone: true,
  imports: [FormsModule, MatInputModule, MatButtonModule, MatCardModule, MatIconModule, DatePipe],
  templateUrl: './notes.html',
  styleUrl: './notes.css',
})
export class NotesComponent implements OnInit {
  notes: Note[] = [];

  // Variables conectadas al formulario para crear una nota nueva.
  title: string = '';
  content: string = '';
  tag: string = '';

  isLoading: boolean = false;
  errorMessage: string = '';

  constructor(
    private noteService: NoteService,
    private authService: AuthService,
    private router: Router,
  ) {}

  ngOnInit(): void {
    this.loadNotes();
  }

  // Pide al backend la lista de notas del usuario logueado.
  loadNotes(): void {
    this.noteService.getMyNotes().subscribe({
      next: (data) => {
        // Guardamos las notas en el arreglo para mostrarlas en el HTML.
        this.notes = data;
      },
      error: () => {
        this.errorMessage = 'No se pudieron cargar las notas.';
      },
    });
  }

  // Se ejecuta cuando el usuario hace click en "Crear nota".
  onCreate(): void {
    // Validación simple — el título no puede estar vacío.
    if (!this.title.trim()) {
      this.errorMessage = 'El título es obligatorio.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.noteService
      .create({
        title: this.title,
        content: this.content,
        tag: this.tag,
      })
      .subscribe({
        next: (newNote) => {
          this.notes.push(newNote);

          this.title = '';
          this.content = '';
          this.tag = '';
          this.isLoading = false;
        },
        error: () => {
          this.errorMessage = 'No se pudo crear la nota.';
          this.isLoading = false;
        },
      });
  }

  // Cierra sesión eliminando el token y manda al usuario al login.
  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}
