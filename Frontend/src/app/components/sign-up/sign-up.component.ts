import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http'; // Ensure HttpClientModule is imported
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-signup',
  templateUrl: './sign-up.component.html',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, HttpClientModule, RouterLink], // Add HttpClientModule here
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent {
  signupForm: FormGroup;

  constructor(private fb: FormBuilder, private http: HttpClient, private router: Router) {
    this.signupForm = this.fb.group({
      username: ['', [Validators.required, Validators.maxLength(10)]],
      fullName: ['', [Validators.required, Validators.maxLength(30)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
      phoneNo: ['', [Validators.required, Validators.pattern('^\\d{10}$')]],
      country: ['', [Validators.required, Validators.maxLength(20)]],
      city: ['', [Validators.required, Validators.maxLength(20)]],
      address: ['', [Validators.required, Validators.maxLength(200)]]
    });
  }

  onSubmit() {
    console.log('Form Submitted');
    if (this.signupForm.valid) {
      const registerData = this.signupForm.value;
      this.http.post('https://localhost:7183/api/Authentication/SignUp', registerData).subscribe(
        (response: any) => {
          alert('Signup Successful');
          this.router.navigate(['/login']);
        },
        (error) => {
          console.error('Signup failed', error);
          // alert('Signup Failed');
        }
      );
    } else {
      alert('Please fill out the form correctly.');
    }
  }
}
