import { Component, CUSTOM_ELEMENTS_SCHEMA, inject, OnInit } from '@angular/core';
import { ShopService } from '../../core/services/shop.service';
import { Product } from '../../shared/models/product';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { ProductItemComponent } from "./product-item/product-item.component";
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MatIcon } from '@angular/material/icon';
import { MatListOption, MatSelectionList, MatSelectionListChange } from '@angular/material/list';
import { FormsModule } from '@angular/forms';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import { shopParams } from '../../shared/models/shopParams';
import {MatPaginator, MatPaginatorModule, PageEvent} from '@angular/material/paginator';
import { Pagination } from '../../shared/models/pagination';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [
    MatCardModule,
    ProductItemComponent,
    MatButton,
    MatIcon,
    MatSelectionList,
    MatListOption,
    MatMenu,
    MatMenuTrigger,
    FormsModule,
    MatPaginator
  ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
  schemas:[CUSTOM_ELEMENTS_SCHEMA]
})

export class ShopComponent implements OnInit{
  private shopService = inject(ShopService);
  private dialogService = inject(MatDialog);
  products?: Pagination<Product>;

  sortOptions = [
    {name: 'Alphabetical', value: 'name'},
    {name: 'Price: Low-High', value: 'priceAsc'},
    {name: 'Price: High-Low', value: 'priceDesc'},
  ]
  shopParams = new shopParams();
  pageSizeOptions = [5,10,15,20]

  ngOnInit(): void {
    this.initializeShop();
  }
  onSearchChange() {
    this.shopParams.pageNumber = 1;
    this.getProducts();
  }

  handlePageEvent(event: PageEvent) {
    this.shopParams.pageNumber = event.pageIndex + 1;
    this.shopParams.pageSize = event.pageSize;
    this.getProducts();
  }
  initializeShop()
  {
  this.shopService.getBrands();
  this.shopService.getTypes();
  this.getProducts()
  }

  getProducts(){
    this.shopService.getProducts(this.shopParams).subscribe({
      next:response => this.products = response  ,
      error: error=> console.log(error),      
    })
  }
  onSortChange(event: MatSelectionListChange) {
    const selectedOption = event.options[0];
    if (selectedOption) {
      this.shopParams.sort = selectedOption.value;
      this.shopParams.pageNumber = 1;
      this.getProducts();
    }
  }
  openFiltersDialog(){
    const dialogRef= this.dialogService.open(FiltersDialogComponent,{
      minWidth :'500px',
      data:{
        selectedBrands:this.shopParams.brands,
        selectedTypes:this.shopParams.types
      }
    });

  dialogRef.afterClosed().subscribe({
      next:result=>{
        if(result)
        {
          this.shopParams.brands = result.selectedBrands;
          this.shopParams.types =result.selectedTypes;
          this.shopParams.pageNumber = 1;
          this.getProducts();
        }
      }
    })
  }
}
