import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ShopService } from '../shop.service';
import { Router } from '@angular/router';
import { of, switchMap } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-product-create',
  templateUrl: './product-create.component.html',
  styleUrls: ['./product-create.component.scss']
})
export class ProductCreateComponent {

  fileName: string | null = null;
  image: File | null = null;
  errors: string[] | null = null;

  constructor(private fb: FormBuilder, private shopService: ShopService, private router: Router, private toastr: ToastrService) {}

  productForm = this.fb.group({
    name: ['', Validators.required],
    description: ['', [Validators.required,]],
    price: ['', [Validators.required]],
    pictureUrl: [''],
    productType: ['', [Validators.required]],
    productBrand: ['', [Validators.required]],
  })

  onSubmit() {
    const formData = this.GenerateFormData();

    this.shopService.createProduct(this.productForm.value).pipe(
      switchMap((createProductResult) => {
        if (this.fileName && this.image) {
          return this.shopService.uploadImage(formData)
        }
        return of(null);
      })).subscribe({
      next: (uploadImageResult) => this.router.navigateByUrl('/shop'),
      error: (error) => this.toastr.error('An error occurred while uploading image.')
    });
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];

    if (file) {
      this.fileName = file.name;
      this.image = file;
      this.productForm.get('pictureUrl')?.setValue(`api/images/products/${this.image.name}`)
    }
  }

  private GenerateFormData(): FormData{
    const formData = new FormData();

    if (this.fileName && this.image) {
      formData.append('Name', this.fileName);
      formData.append('Image', this.image, this.image.name);
    }
    return formData;
  }
}


