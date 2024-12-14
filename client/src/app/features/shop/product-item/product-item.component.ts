import { Component, CUSTOM_ELEMENTS_SCHEMA, Input } from '@angular/core';
import { MatCard, MatCardActions, MatCardContent } from '@angular/material/card';
import { Product } from '../../../shared/models/product';
import { CurrencyPipe } from '@angular/common';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-product-item',
  standalone: true,
  imports: [MatCard,MatCardContent,CurrencyPipe,MatButton,MatCardActions,MatIcon],
  templateUrl: './product-item.component.html',
  styleUrl: './product-item.component.scss',
  schemas:[CUSTOM_ELEMENTS_SCHEMA]
})
export class ProductItemComponent {
  @Input() product?:Product 
}
