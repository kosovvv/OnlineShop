import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { of, switchMap } from 'rxjs';
import { Brand } from 'src/app/shared/models/brand';
import { BrandService } from 'src/app/core/services/brand.service';

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

  constructor(private fb: FormBuilder,private brandService: BrandService, private router: Router, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.getBrands();
  }

  getBrands() {
    return this.brandService.getBrands().subscribe({
      next: (brands) => {
        if (brands) {
          this.brands = brands;
        }
      }
    })
  }

  deleteBrand(brandId : number) {
    this.brandService.deleteBrand(brandId).subscribe({
      next: () => {
        this.getBrands();
      }
    })
  }


}
