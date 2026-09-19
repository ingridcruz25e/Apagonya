export interface LoginResponse { idToken: string; localId: string; email: string; }
export interface RegisterResponse { id: string; email: string; displayName?: string; role?: string; }
export interface AppUser {
  id: string; email: string; displayName: string; username: string; phoneNumber: string;
  birthDate: string; country: string; bio: string; role: string; createdAt: string;
}
