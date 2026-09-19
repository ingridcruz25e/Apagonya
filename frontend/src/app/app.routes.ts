import { Routes } from '@angular/router';
import { LandingComponent } from './pages/landing/landing';
import { LoginComponent } from './pages/login/login';
import { RegisterComponent } from './pages/register/register';
import { DashboardComponent } from './pages/dashboard/dashboard';
import { ReportFormComponent } from './pages/report-form/report-form';
import { TechnicianComponent } from './pages/technician/technician';
import { AdminComponent } from './pages/admin/admin';
import { authGuard } from './services/auth.guard';
import { roleGuard } from './services/role.guard';
export const routes: Routes = [
 {path:'',component:LandingComponent},
 {path:'login',component:LoginComponent},
 {path:'register',component:RegisterComponent},
 {path:'dashboard',component:DashboardComponent,canActivate:[authGuard]},
 {path:'reportar',component:ReportFormComponent,canActivate:[authGuard]},
 {path:'tecnico',component:TechnicianComponent,canActivate:[roleGuard],data:{roles:['tecnico']}},
 {path:'admin',component:AdminComponent,canActivate:[roleGuard],data:{roles:['administrador']}},
 {path:'**',redirectTo:''}
];
