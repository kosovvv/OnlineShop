import { Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { of, switchMap } from 'rxjs';
import { BrandService } from 'src/app/core/services/brand.service';
import { ImageService } from 'src/app/core/services/image.service';

@Component({
  selector: 'app-brand-create',
  templateUrl: './brand-create.component.html',
  styleUrls: ['./brand-create.component.scss']
})
export class BrandCreateComponent {
  @ViewChild("closeButton") closeButton!: ElementRef;
  fileName: string | null = null;
  image: File | null = null;
  errors: string[] | null = null;

  brandForm = this.fb.group({
    name: ['', Validators.required],
    pictureUrl: ['', Validators.required],
  })

  constructor(private fb: FormBuilder, private imageService: ImageService, private brandService: BrandService,private router: Router, private toastr: ToastrService) {}

  onSubmit() {
    const formData = this.GenerateFormData();

    this.brandService.createBrand(this.brandForm.value as any).pipe(
      switchMap((brand) => {
        if (this.fileName && this.image) {
          return this.imageService.uploadImage(formData)
        }
        return of(null);
      })).subscribe({
      next: (image) => {
        this.toastr.success("Success creating brand.")
        this.closeButton.nativeElement.click();
      },
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

  private GenerateFormData(): FormData{
    const formData = new FormData();

    if (this.fileName && this.image) {
      formData.append('Name', this.fileName);
      formData.append('Image', this.image, this.image.name);
    }
    return formData;
  }
}
