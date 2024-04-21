import { Component, ElementRef, Input, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Brand } from 'src/app/shared/models/brand';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { of, switchMap } from 'rxjs';
import { BrandService } from 'src/app/core/services/brand.service';
import { ImageService } from 'src/app/core/services/image.service';

@Component({
  selector: 'app-brand-edit',
  templateUrl: './brand-edit.component.html',
  styleUrls: ['./brand-edit.component.scss']
})
export class BrandEditComponent {
  @Input() brand!: Brand
  @ViewChild("closeButton") closeButton!: ElementRef;
  fileName: string | null = null;
  image: File | null = null;
  errors: string[] | null = null;

  brandForm = this.fb.group({
    name: ['', Validators.required],
    pictureUrl: [''],
  })

  constructor(private fb: FormBuilder,private brandService: BrandService, private imageService: ImageService, private router: Router, private toastr: ToastrService) {}
  
  ngOnInit(): void {
    this.loadForm();
  }

  onSubmit() {
    const formData = this.GenerateFormData();

    this.brandService.editBrand(this.brand.id, this.brandForm.value as any).pipe(
      switchMap((createProductResult) => {
        if (this.fileName && this.image) {
          return this.imageService.uploadImage(formData)
        }
        return of(null);
      })).subscribe({
      next: (uploadImageResult) => {
        this.toastr.success("Success editing brand.")
        this.closeButton.nativeElement.click();
      },
      error: (error) => this.toastr.error(error)
    });
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];

    if (file) {
      this.fileName = file.name;
      this.image = file;
      this.brandForm.get('pictureUrl')?.setValue(`api/images/types/${this.image.name}`)
    }
  }

  loadForm() {
    this.brandForm.get('name')?.patchValue(this.brand.name);
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
