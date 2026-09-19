export interface Note {
  id: string;
  title: string;
  content: string;
  tag: string;
  userId: string;
  createdAt: string;
}

export interface NoteDto{
  title: string;
  content: string;
  tag: string;
}
