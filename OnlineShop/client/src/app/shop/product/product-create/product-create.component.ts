import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { forkJoin, of, switchMap } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { Type } from 'src/app/shared/models/type';
import { Brand } from 'src/app/shared/models/brand';
import { TypeService } from 'src/app/shared/services/type-service';
import { BrandService } from 'src/app/shared/services/brand.service';
import { ImageService } from 'src/app/shared/services/image.service';
import { ProductService } from 'src/app/shared/services/product.service';

@Component({
  selector: 'app-product-create',
  templateUrl: './product-create.component.html',
  styleUrls: ['./product-create.component.scss']
})
export class ProductCreateComponent implements OnInit {

  fileName: string | null = null;
  image: File | null = null;
  errors: string[] | null = null;
  types: Type[] = [];
  brands : Brand[] = [];
  productForm = this.fb.group({
    name: ['', Validators.required],
    description: ['', [Validators.required,]],
    price: ['', [Validators.required]],
    pictureUrl: [''],
    productType: ['', [Validators.required]],
    productBrand: ['', [Validators.required]],
  })

  constructor(private fb: FormBuilder,private typeService: TypeService, private productService: ProductService, private imageService: ImageService,
    private router: Router, private toastr: ToastrService, private brandService: BrandService) {}
  
  ngOnInit(): void {
    this.getProductsAndTypes();
  }

  getProductsAndTypes() {
    forkJoin([
      this.typeService.getTypes(),
      this.brandService.getBrands()
    ]).subscribe({
      next: ([types, brands]) => {
        this.types = types;
        this.brands = brands;
      },
      error: (error) => {
        this.toastr.error(error);
      }
    });
  }

  onSubmit() {
    const formData = this.GenerateFormData();

    this.productService.createProduct(this.productForm.value).pipe(
      switchMap((createProductResult) => {
        if (this.fileName && this.image) {
          return this.imageService.uploadImage(formData)
        }
        return of(null);
      })).subscribe({
      next: (uploadImageResult) => this.router.navigateByUrl('/shop'),
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


