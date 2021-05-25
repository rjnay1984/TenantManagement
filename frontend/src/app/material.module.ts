import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [],
  imports: [CommonModule, BrowserAnimationsModule, MatButtonModule, MatIconModule, MatToolbarModule, MatSnackBarModule],
  exports: [BrowserAnimationsModule, MatButtonModule, MatIconModule, MatToolbarModule, MatSnackBarModule],
})
export class MaterialModule {}
