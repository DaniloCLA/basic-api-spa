import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import * as alertify from 'alertify.js';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  loggedIn: boolean;
  loginForm: FormGroup;

  constructor(private router: Router, private fb: FormBuilder) {
    this.model.photoUrl = '../../assets/user.png';
    this.model.displayName = 'Usuário';
    this.model.userPreferredLanguage = 'pt-br';
    this.model.userRoles = ['admin'];
  }

  createRegisterForm() {
    this.loginForm = this.fb.group({
      userName: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  ngOnInit() {
    this.loggedIn = false;
    this.createRegisterForm();
  }

  login() {
    this.loggedIn = true;

    this.router.navigate(['/dashboard']);
    alertify.success('Bem vindo, ' + this.model.displayName);
    this.loginForm.reset();
  }

  logout() {
    this.loggedIn = false;
    this.router.navigate(['/home']);
    alertify.success('Até logo, ' + this.model.displayName);
    this.loginForm.reset();
  }

  isLoggedIn() {
    return this.loggedIn;
  }

  isAdmin() {
    return this.model.userRoles.includes('admin');
  }

  toggleLanguage(key: string) {
    this.model.userPreferredLanguage = key;
    alertify.success('Idioma ativado com sucesso.');
  }
}
