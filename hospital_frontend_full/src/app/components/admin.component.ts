import { Component } from '@angular/core';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html'
})
export class AdminComponent {
  doctors: any[] = [];
  appointments: any[] = [];

  newDoctor = {
    name: '',
    specialty: ''
  };

  addDoctor() {
    if (!this.newDoctor.name || !this.newDoctor.specialty) return;

    this.doctors.push({ ...this.newDoctor });
    this.newDoctor = { name: '', specialty: '' };
  }

  deleteDoctor(index: number) {
    this.doctors.splice(index, 1);
  }

  deleteAppointment(index: number) {
    this.appointments.splice(index, 1);
  }

  logout() {
    localStorage.removeItem('jwt');
    location.href = '/login';
  }
}
