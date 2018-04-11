import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { MessagesComponent } from './messages/messages.component';
import { SettingsComponent } from './settings/settings.component';
import { SupportComponent } from './support/support.component';
import { RegisterComponent } from './register/register.component';

export const appRoutes: Routes = [
    {path: 'home', component: HomeComponent},
    {path: 'dashboard', component: DashboardComponent},
    {path: 'messages', component: MessagesComponent},
    {path: 'settings', component: SettingsComponent},
    {path: 'support', component: SupportComponent},
    {path: 'register', component: RegisterComponent},
    {path: '**', redirectTo: 'home', pathMatch: 'full'}
];
