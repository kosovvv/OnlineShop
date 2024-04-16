import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ShopService } from '../../shop.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { of, switchMap } from 'rxjs';
import { Brand } from 'src/app/shared/models/brand';

@Component({
  selector: 'app-brand-manage',
  templateUrl: './brand-manage.component.html',
  styleUrls: ['./brand-manage.component.scss']
})
export class BrandManageComponent implements OnInit {

  fileName: string | null = null;
  image: File | null = null;
  errors: string[] | null = null;
  brands: Brand[] = [];

  brandForm = this.fb.group({
    name: ['', Validators.required],
    pictureUrl: ['', Validators.required],
  })

  constructor(private fb: FormBuilder, private shopService: ShopService, private router: Router, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.getBrands();
  }

  getBrands() {
    return this.shopService.getBrands().subscribe({
      next: (brands) => {
        if (brands) {
          this.brands = brands;
        }
      }
    })
  }

  deleteBrand(brandId : number) {
    this.shopService.deleteBrand(brandId).subscribe({
      next: () => {
        this.getBrands();
      }
    })
  }


}
