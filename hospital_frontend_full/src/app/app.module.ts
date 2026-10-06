import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { LoginComponent } from './components/login.component';
import { AdminComponent } from './components/admin.component';
import { CalendarComponent } from './components/calendar.component';

@NgModule({
  declarations: [AppComponent, LoginComponent, AdminComponent, CalendarComponent],
  imports: [BrowserModule, FormsModule, HttpClientModule],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}



/*@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    AdminComponent,
    CalendarComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,          // ✅ REQUIRED for ngModel
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [
    AuthGuard,
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
*/