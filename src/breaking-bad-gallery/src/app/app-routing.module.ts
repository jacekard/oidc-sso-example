import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UnauthorizedComponent } from './core/auth/unauthorized/unauthorized.component';
import { AppComponent } from './app.component';

const routes: Routes = [
  { path: 'unauthorized', component: UnauthorizedComponent },
  { path: 'forbidden', component: UnauthorizedComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
