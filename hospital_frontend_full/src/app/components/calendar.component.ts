import { Component } from '@angular/core';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html'
})
export class CalendarComponent {
  date = '';
  doctorId = '';
  reason = '';
  message = '';

  book() {
    if (!this.date || !this.doctorId || !this.reason) {
      this.message = 'All fields required';
      return;
    }

    this.message = 'Appointment booked successfully!';
  }
}
